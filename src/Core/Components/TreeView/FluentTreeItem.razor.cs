// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a tree item in a <see cref="FluentTreeView"/>.
/// </summary>
public partial class FluentTreeItem : FluentComponentBase
{
    private const string DefaultIcon_CommonSvgAttributes = "width=\"12\" height=\"12\" xmlns=\"http://www.w3.org/2000/svg\" fill=\"currentColor\" slot=\"chevron\"";
    private const string DefaultIcon_CommonSvgPath = "<path d=\"M4.65 2.15a.5.5 0 000 .7L7.79 6 4.65 9.15a.5.5 0 10.7.7l3.5-3.5a.5.5 0 000-.7l-3.5-3.5a.5.5 0 00-.7 0z\" />";
    private static readonly MarkupStringSanitized DefaultIcon_Expanded = new($" <svg expanded {DefaultIcon_CommonSvgAttributes}>{DefaultIcon_CommonSvgPath}</svg>", isSanitized: true);
    private static readonly MarkupStringSanitized DefaultIcon_Collapsed = new($"<svg collapsed {DefaultIcon_CommonSvgAttributes}>{DefaultIcon_CommonSvgPath}</svg>", isSanitized: true);

    /// <summary/>
    [DynamicDependency(nameof(OnTreeChangedAsync))]
    [DynamicDependency(nameof(OnTreeToggleAsync))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(TreeItemChangedEventArgs))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(TreeItemToggleEventArgs))]
    public FluentTreeItem(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary/>
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary/>
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("--tree-item-height", Height, when: () => !string.IsNullOrEmpty(Height))
        .Build();

    /// <summary/>
    [CascadingParameter]
    internal FluentTreeView? OwnerTreeView { get; set; }

    /// <summary>
    /// Gets or sets the size of the tree item.
    /// Default is <see cref="TreeSize.Medium"/>.
    /// </summary>
    [Parameter]
    public TreeSize? Size { get; set; }

    /// <summary>
    /// Gets or sets the height of the tree item.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the appearance of the tree item.
    /// Default is <see cref="TreeAppearance.Subtle"/>.
    /// </summary>
    [Parameter]
    public TreeAppearance? Appearance { get; set; }

    /// <summary>
    /// Gets or sets the list of sub-items to bind to the tree item
    /// </summary>
    [Parameter]
    public IEnumerable<ITreeViewItem>? Items { get; set; }

    /// <summary>
    /// Gets or sets the text of the tree item.
    /// If this text is too long, it will be truncated with ellipsis.
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the start of item content.
    /// We recommend using an icon size of 16px.
    /// </summary>
    [Parameter]
    public Icon? IconStart { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the end of item content.
    /// We recommend using an icon size of 16px.
    /// </summary>
    [Parameter]
    public Icon? IconEnd { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the "far end" of item content.
    /// We recommend using an icon size of 16px.
    /// </summary>
    [Parameter]
    public Icon? IconAside { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed to indicate the tree item is collapsed.
    /// If this icon is not set, the <see cref="IconExpanded"/> will be used.
    /// We recommend using an icon size of 16px.
    /// </summary>
    [Parameter]
    public Icon? IconCollapsed { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed to indicate the tree item is expanded.
    /// If this icon is not set, the <see cref="IconCollapsed"/> will be used.
    /// A 90-degree rotation effect is applied to the icon.
    /// Please select an icon that will look correct after rotation.
    /// We recommend using an icon size of 16px.
    /// </summary>
    [Parameter]
    public Icon? IconExpanded { get; set; }

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
    /// Called whenever the selected item changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> SelectedChanged { get; set; }

    /// <summary>
    /// Gets the item associated with the current element, based on its Id.
    /// </summary>
    public ITreeViewItem? Item => OwnerTreeView is null || OwnerTreeView.Items is null
                                ? null
                                : TreeViewItem.FindItemById(OwnerTreeView.Items, Id);

    /// <summary />
    private bool IsSelected => string.CompareOrdinal(OwnerTreeView?.SelectedId, Id) == 0 ||
                               string.CompareOrdinal(OwnerTreeView?.SelectedItem?.Id, Id) == 0;

    /// <summary />
    protected override void OnInitialized()
    {
        if (OwnerTreeView is not null && !string.IsNullOrEmpty(Id))
        {
            OwnerTreeView.InternalItems.TryAdd(Id, this);
        }
    }

    /// <summary />
    internal async Task OnTreeChangedAsync(TreeItemChangedEventArgs args)
    {
        if (!string.Equals(Id, args.Id, StringComparison.Ordinal))
        {
            return;
        }

        // Selected?
        var isSelected = args.Selected;

        // Update the state
        if (SelectedChanged.HasDelegate)
        {
            await SelectedChanged.InvokeAsync(isSelected);
        }

        // Update the FluentTree owner (only to inform the new selected item
        if (OwnerTreeView is not null && isSelected)
        {
            // SelectedIdChanged
            if (OwnerTreeView.SelectedIdChanged.HasDelegate &&
                !string.Equals(OwnerTreeView.SelectedId, Id, StringComparison.Ordinal))
            {
                await OwnerTreeView.SelectedIdChanged.InvokeAsync(Id);
            }

            // CurrentSelectedItem
            if (OwnerTreeView.CurrentSelectedChanged.HasDelegate &&
                OwnerTreeView.CurrentSelected != this)
            {
                await OwnerTreeView.CurrentSelectedChanged.InvokeAsync(this);
            }

            // SelectedItem
            if (OwnerTreeView.Items is not null &&
                OwnerTreeView.SelectedItemChanged.HasDelegate)
            {
                var selectedItem = TreeViewItem.FindItemById(OwnerTreeView.Items, Id);

                if (OwnerTreeView.SelectedItem != selectedItem)
                {
                    await OwnerTreeView.SelectedItemChanged.InvokeAsync(selectedItem);
                }
            }

            // OnSelectedChanged
            if (OwnerTreeView.OnSelectedChanged.HasDelegate)
            {
                await OwnerTreeView.OnSelectedChanged.InvokeAsync(this);
            }
        }
    }

    /// <summary />
    internal async Task OnTreeToggleAsync(TreeItemToggleEventArgs args)
    {
        // Only for the correct TreeItem
        if (!string.Equals(Id, args.Id, StringComparison.Ordinal))
        {
            return;
        }

        // Expanded?
        var isExpanded = string.Equals(args.NewState, "open", StringComparison.Ordinal);

        // Update the state
        if (isExpanded != Expanded)
        {
            Expanded = isExpanded;

            if (ExpandedChanged.HasDelegate)
            {
                await InvokeAsync(async () => await ExpandedChanged.InvokeAsync(isExpanded));
            }
        }

        // Update the FluentTree owner
        if (OwnerTreeView is not null && OwnerTreeView.OnExpandedChanged.HasDelegate)
        {
            await InvokeAsync(async () => await OwnerTreeView.OnExpandedChanged.InvokeAsync(this));
        }
    }

    /// <summary />
    internal async Task OnCheckChangedHandlerAsync(bool? newState = null)
    {
        var checkedItem = TreeViewItem.FindItemById(OwnerTreeView?.Items, Id);

        if (OwnerTreeView is null || checkedItem is null)
        {
            return;
        }

        var selectedItems = OwnerTreeView.SelectedItems?.ToList() ?? [];
        if (OwnerTreeView.SelectionMode != TreeSelectionMode.MultipleRecursive)
        {
            if (newState.HasValue)
            {
                SetSelection(selectedItems, checkedItem, newState.Value);
            }
            else
            {
                ToggleSelection(selectedItems, checkedItem);
            }

            if (OwnerTreeView.SelectedItemsChanged.HasDelegate)
            {
                await OwnerTreeView.SelectedItemsChanged.InvokeAsync(selectedItems);
            }

            return;
        }

        var currentState = GetSelectionState(checkedItem, selectedItems);
        var selectDescendants = newState switch
        {
            true when currentState is not null => true,
            null when currentState is false => true,
            _ => false,
        };

        if (selectDescendants)
        {
            foreach (var item in GetDescendantsAndSelf(checkedItem))
            {
                AddIfMissing(selectedItems, item);
            }
        }
        else
        {
            foreach (var item in GetDescendantsAndSelf(checkedItem))
            {
                selectedItems.Remove(item);
            }
        }

        SynchronizeParentSelection(OwnerTreeView.Items, checkedItem, selectedItems);

        if (OwnerTreeView.SelectedItemsChanged.HasDelegate)
        {
            await OwnerTreeView.SelectedItemsChanged.InvokeAsync(selectedItems);
        }
    }

    private static void ToggleSelection(List<ITreeViewItem> selectedItems, ITreeViewItem item)
    {
        if (!selectedItems.Remove(item))
        {
            selectedItems.Add(item);
        }
    }

    private static void SetSelection(List<ITreeViewItem> selectedItems, ITreeViewItem item, bool isSelected)
    {
        if (isSelected)
        {
            AddIfMissing(selectedItems, item);
        }
        else
        {
            selectedItems.Remove(item);
        }
    }

    private static bool? GetSelectionState(ITreeViewItem item, IEnumerable<ITreeViewItem> selectedItems)
    {
        var descendants = GetDescendantsAndSelf(item).ToList();
        if (descendants.All(selectedItems.Contains))
        {
            return true;
        }

        if (descendants.Any(selectedItems.Contains))
        {
            return null;
        }

        return false;
    }

    private static IEnumerable<ITreeViewItem> GetDescendantsAndSelf(ITreeViewItem item)
    {
        yield return item;

        if (item.Items is null)
        {
            yield break;
        }

        foreach (var child in item.Items)
        {
            foreach (var descendant in GetDescendantsAndSelf(child))
            {
                yield return descendant;
            }
        }
    }

    private static bool SynchronizeParentSelection(IEnumerable<ITreeViewItem>? items, ITreeViewItem changedItem, List<ITreeViewItem> selectedItems)
    {
        if (items is null)
        {
            return false;
        }

        foreach (var item in items)
        {
            if (SynchronizeParentSelection(item, changedItem, selectedItems))
            {
                return true;
            }
        }

        return false;
    }

    private static bool SynchronizeParentSelection(ITreeViewItem item, ITreeViewItem changedItem, List<ITreeViewItem> selectedItems)
    {
        var children = item.Items?.ToList();
        if (children is null or { Count: 0 })
        {
            return false;
        }

        if (!children.Any(child => string.Equals(child.Id, changedItem.Id, StringComparison.Ordinal)) &&
            !children.Any(child => SynchronizeParentSelection(child, changedItem, selectedItems)))
        {
            return false;
        }

        var childStates = children.Select(child => GetSelectionState(child, selectedItems)).ToList();
        var state = childStates.All(childState => childState == true);

        if (state)
        {
            AddIfMissing(selectedItems, item);
        }
        else
        {
            selectedItems.Remove(item);
        }

        return true;
    }

    private static void AddIfMissing(List<ITreeViewItem> selectedItems, ITreeViewItem item)
    {
        if (!selectedItems.Contains(item))
        {
            selectedItems.Add(item);
        }
    }

    /// <summary>
    /// Renders a FluentTreeItem component, using a <see cref="ITreeViewItem" />
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    internal static RenderFragment GetFluentTreeItem(FluentTreeView owner, ITreeViewItem item)
    {
        RenderFragment fluentTreeItem = builder =>
        {
            builder.OpenComponent<FluentTreeItem>(0);
            builder.AddAttribute(1, nameof(Id), item.Id);
            builder.AddAttribute(2, nameof(Items), item.Items);
            builder.AddAttribute(3, nameof(Text), owner.ItemTemplate is null && owner.SelectionMode == TreeSelectionMode.Single ? item.Text : null);
            builder.AddAttribute(4, nameof(Expanded), item.Expanded);
            builder.AddAttribute(5, nameof(IconStart), item.IconStart);
            builder.AddAttribute(6, nameof(IconEnd), item.IconEnd);
            builder.AddAttribute(7, nameof(IconAside), item.IconAside);
            builder.AddAttribute(8, nameof(IconCollapsed), item.IconCollapsed);
            builder.AddAttribute(9, nameof(IconExpanded), item.IconExpanded);

            AddFluentTreeItemChildContent(builder, owner, item);

            builder.AddAttribute(11, nameof(ExpandedChanged), EventCallback.Factory.Create<bool>(owner, async expanded =>
            {
                item.Expanded = expanded;
                if (item.OnExpandedAsync != null)
                {
                    await item.OnExpandedAsync(new TreeViewItemExpandedEventArgs(item, expanded));
                }
            }));

            builder.CloseComponent();
        };

        return fluentTreeItem;
    }

    /// <summary />
    private static void AddFluentTreeItemChildContent(RenderTreeBuilder builder, FluentTreeView owner, ITreeViewItem item)
    {
        switch (owner.SelectionMode)
        {
            case TreeSelectionMode.Single:
                if (owner.ItemTemplate != null)
                {
                    builder.AddAttribute(10, nameof(ChildContent), owner.ItemTemplate.Invoke(item));
                }

                break;

            case TreeSelectionMode.Multiple:
            case TreeSelectionMode.MultipleRecursive:
                builder.AddAttribute(10, nameof(ChildContent), (RenderFragment)(childBuilder =>
                {
                    var visibility = owner.MultipleSelectionVisibility?.Invoke(item) ?? TreeSelectionVisibility.Visible;
                    var selectionState = owner.SelectionMode == TreeSelectionMode.MultipleRecursive
                        ? GetSelectionState(item, owner.SelectedItems ?? [])
                        : owner.SelectedItems?.Contains(item) == true ? true : false;

                    // Checkbox
                    switch (visibility)
                    {
                        // Visible
                        case TreeSelectionVisibility.Visible:
                            if (owner.SelectionMode == TreeSelectionMode.Multiple)
                            {
                                AddPlainCheckbox(childBuilder, owner, item, selectionState);
                            }
                            else
                            {
                                AddRecursiveCheckbox(childBuilder, owner, item, selectionState);
                            }

                            break;

                        // Hidden
                        case TreeSelectionVisibility.Hidden:
                            childBuilder.OpenElement(0, "div");
                            childBuilder.AddAttribute(1, "class", "hidden-checkbox");
                            childBuilder.CloseElement();
                            break;
                    }

                    // Content
                    childBuilder.AddContent(4, owner.ItemTemplate?.Invoke(item) ?? (RenderFragment)(builder2 => builder2.AddContent(0, item.Text)));
                }));

                break;
        }
    }

    private static void AddPlainCheckbox(RenderTreeBuilder builder, FluentTreeView owner, ITreeViewItem item, bool? selectionState)
    {
        builder.OpenElement(0, "fluent-checkbox");
        builder.AddAttribute(1, "checked", selectionState == true ? "true" : null);
        builder.AddAttribute(3, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(owner, async _ =>
        {
            var fluentTreeItem = owner.InternalItems.TryGetValue(item.Id, out var ti) ? ti : null;
            if (fluentTreeItem != null)
            {
                await fluentTreeItem.OnCheckChangedHandlerAsync(selectionState != true);
            }
        }));
        builder.AddAttribute(4, "tabindex", -1);
        builder.CloseElement();
    }

    private static void AddRecursiveCheckbox(RenderTreeBuilder builder, FluentTreeView owner, ITreeViewItem item, bool? selectionState)
    {
        builder.OpenComponent<FluentCheckbox>(0);
        builder.AddAttribute(1, nameof(FluentCheckbox.CheckState), selectionState);
        builder.AddAttribute(2, nameof(FluentCheckbox.Value), selectionState == true);
        builder.AddAttribute(3, nameof(FluentCheckbox.ThreeState), true);
        builder.AddAttribute(4, nameof(FluentCheckbox.ThreeStateOrderUncheckToIntermediate), true);
        builder.AddAttribute(5, nameof(FluentCheckbox.CheckStateChanged), EventCallback.Factory.Create<bool?>(owner, async state =>
        {
            var fluentTreeItem = owner.InternalItems.TryGetValue(item.Id, out var ti) ? ti : null;
            if (fluentTreeItem != null)
            {
                await fluentTreeItem.OnCheckChangedHandlerAsync(state);
            }
        }));
        builder.AddAttribute(6, "tabindex", -1);
        builder.CloseComponent();
    }

    /// <summary />
    public override ValueTask DisposeAsync()
    {
        if (OwnerTreeView is not null && !string.IsNullOrEmpty(Id))
        {
            OwnerTreeView.InternalItems.TryRemove(Id, out _);
        }

        return base.DisposeAsync();
    }
}
