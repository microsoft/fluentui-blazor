using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentTreeItem : FluentComponentBase, IDisposable
{
    private bool _disposed;

    /// <summary>
    /// Gets or sets the text of the tree item
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Returns <see langword="true"/> if the tree item is expanded,
    /// and <see langword="false"/> if collapsed.
    /// </summary>
    [Parameter]
    public bool Expanded { get; set; }

    /// <summary>
    /// Called whenever <see cref="Expanded"/> changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> ExpandedChanged { get; set; }

    /// <summary>
    /// When true, the control will appear selected by user interaction.
    /// </summary>
    [Parameter]
    public bool Selected { get; set; }

    /// <summary>
    /// Called whenever <see cref="Selected"/> changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> SelectedChanged { get; set; }

    /// <summary>
    /// When true, the control will be immutable by user interaction. See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/disabled">disabled</see> HTML attribute for more information.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// If set to <see langword="true"/> then the tree item will
    /// be expanded when it is created.
    /// </summary>
    [Parameter]
    public bool InitiallyExpanded { get; set; }

    /// <summary>
    /// If set to <see langword="true"/> then the tree item will
    /// be selected when it is created.
    /// </summary>
    [Parameter]
    public bool InitiallySelected { get; set; }

    /// <summary>
    /// Gets or sets the owning FluentTreeView
    /// </summary>
    [CascadingParameter]
    private FluentTreeView Owner { get; set; } = default!;

    /// <summary>
    /// Returns <see langword="true"/> if the tree item is collapsed,
    /// and <see langword="false"/> if expanded.
    /// </summary>
    public bool Collapsed => !Expanded;

    public FluentTreeItem()
    {
        Id = Identifier.NewId();
    }

    void IDisposable.Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            Owner?.Unregister(this);
        }

        _disposed = true;
    }

    internal async Task SetSelectedAsync(bool value)
    {
        if (value == Selected)
        {
            return;
        }

        Selected = value;
        if (SelectedChanged.HasDelegate)
        {
            await SelectedChanged.InvokeAsync(Selected);
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Owner.Register(this);
        if (InitiallyExpanded && !Expanded)
        {
            Expanded = true;
            if (ExpandedChanged.HasDelegate)
            {
                await ExpandedChanged.InvokeAsync(true);
            }
        }
        if (InitiallySelected)
        {
            await SetSelectedAsync(true);
        }
    }

    private async Task HandleExpandedChangeAsync(TreeChangeEventArgs args)
    {
        if (args.AffectedId != Id || args.Expanded is null || args.Expanded == Expanded)
        {
            return;
        }

        Expanded = args.Expanded.Value;
        if (ExpandedChanged.HasDelegate)
        {
            await ExpandedChanged.InvokeAsync(Expanded);
        }

        if (Owner is FluentTreeView tree)
        {
            await tree.ItemExpandedChangeAsync(this);
        }
    }

    private async Task HandleSelectedChangeAsync(TreeChangeEventArgs args)
    {
        if (args.AffectedId != Id || args.Selected is null || args.Selected == Selected)
        {
            return;
        }

        await SetSelectedAsync(args.Selected.Value);

        if (Owner is FluentTreeView tree)
        {
            await tree.ItemSelectedChangeAsync(this);
        }
    }

}
