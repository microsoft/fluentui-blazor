using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentTreeView : FluentComponentBase, IDisposable
{
    private readonly Dictionary<string, FluentTreeItem> _allItems = [];
    private readonly Debouncer _currentSelectedChangedDebouncer = new();
    private bool _disposed;

    public static string LoadingMessage = "Loading...";

    [Parameter]
    public IEnumerable<ITreeViewItem>? Items { get; set; }

    /// <summary>
    /// Gets or sets whether the tree should render nodes under collapsed items
    /// Defaults to false
    /// </summary>
    [Parameter]
    [Obsolete("Please, prefer to use the `LazyLoadItems` attribute.")]
    public bool RenderCollapsedNodes { get; set; }

    /// <summary>
    /// Gets or sets the currently selected tree item
    /// </summary>
    [Parameter]
    public FluentTreeItem? CurrentSelected { get; set; } = default!;

    /// <summary>
    /// Called when <see cref="CurrentSelected"/> changes.
    /// </summary>
    [Parameter]
    public EventCallback<FluentTreeItem?> CurrentSelectedChanged { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Called whenever <see cref="FluentTreeItem.Selected"/> changes on an
    /// item within the tree.
    /// </summary>
    [Parameter]
    public EventCallback<FluentTreeItem> OnSelectedChange { get; set; }

    /// <summary>
    /// Called whenever <see cref="FluentTreeItem.Expanded"/> changes on an
    /// item within the tree.
    /// </summary>
    [Parameter]
    public EventCallback<FluentTreeItem> OnExpandedChange { get; set; }

    /// <summary>
    /// Gets or sets the template for rendering tree items.
    /// </summary>
    [Parameter]
    public RenderFragment<ITreeViewItem>? ItemTemplate { get; set; }

    /// <summary>
    /// Can only be used when the <see cref="Items"/> is defined.
    /// Gets or sets whether the tree should use lazy loading when expanding nodes.
    /// If True, the tree will only render the children of a node when it is expanded and will remove them when it is collapsed.
    /// </summary>
    [Parameter]
    public bool LazyLoadItems { get; set; } = false;

    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(TreeChangeEventArgs))]
    public FluentTreeView()
    {
    }

    void IDisposable.Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    internal async Task ItemExpandedChangeAsync(FluentTreeItem item)
    {
        if (OnExpandedChange.HasDelegate)
        {
            await OnExpandedChange.InvokeAsync(item);
        }
    }

    internal async Task ItemSelectedChangeAsync(FluentTreeItem item)
    {
        if (OnSelectedChange.HasDelegate)
        {
            await OnSelectedChange.InvokeAsync(item);
        }
    }

    internal void Register(FluentTreeItem fluentTreeItem)
    {
        ArgumentNullException.ThrowIfNull(fluentTreeItem);
        _allItems[fluentTreeItem.Id!] = fluentTreeItem;
    }

    internal void Unregister(FluentTreeItem fluentTreeItem)
    {
        ArgumentNullException.ThrowIfNull(fluentTreeItem);
        _allItems.Remove(fluentTreeItem.Id!);
    }

    private async Task HandleCurrentSelectedChangeAsync(TreeChangeEventArgs args)
    {
        Console.WriteLine("HandleCurrentSelectedChangeAsync");

        if (!_allItems.TryGetValue(args.AffectedId!, out FluentTreeItem? treeItem))
        {
            return;
        }

        var previouslySelected = CurrentSelected;
        await _currentSelectedChangedDebouncer.DebounceAsync(50, () => InvokeAsync(async () =>
        {
            CurrentSelected = treeItem?.Selected == true ? treeItem : null;
            if (CurrentSelected != previouslySelected && CurrentSelectedChanged.HasDelegate)
            {
                foreach (FluentTreeItem item in _allItems.Values)
                {
                    if (item != CurrentSelected && item.Selected)
                    {
                        await item.SetSelectedAsync(false);
                    }
                }
                await CurrentSelectedChanged.InvokeAsync(CurrentSelected);
            }
        }));
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _currentSelectedChangedDebouncer?.Dispose();
            _allItems.Clear();
        }

        _disposed = true;
    }

}
