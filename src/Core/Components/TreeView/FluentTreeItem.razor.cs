// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a tree item in a <see cref="FluentTreeView"/>.
/// </summary>
public partial class FluentTreeItem : FluentComponentBase
{
    private const string DefaultIcon_CommonSvgAttributes = "width=\"12\" height=\"12\" xmlns=\"http://www.w3.org/2000/svg\" fill=\"currentColor\" slot=\"chevron\"";
    private const string DefaultIcon_CommonSvgPath = "<path d=\"M4.65 2.15a.5.5 0 000 .7L7.79 6 4.65 9.15a.5.5 0 10.7.7l3.5-3.5a.5.5 0 000-.7l-3.5-3.5a.5.5 0 00-.7 0z\" />";
    private static readonly MarkupString DefaultIcon_Expanded = new($" <svg expanded {DefaultIcon_CommonSvgAttributes}>{DefaultIcon_CommonSvgPath}</svg>");
    private static readonly MarkupString DefaultIcon_Collapsed = new($"<svg collapsed {DefaultIcon_CommonSvgAttributes}>{DefaultIcon_CommonSvgPath}</svg>");

    /// <summary/>
    public FluentTreeItem()
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
    private async Task OnTreeChangedAsync(TreeItemChangedEventArgs args)
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
    private async Task OnTreeToggleAsync(TreeItemToggleEventArgs args)
    {
        const string StateClosed = "closed";
        const string StateOpened = "open";

        // Only for the correct TreeItem
        if (!string.Equals(Id, args.Id, StringComparison.Ordinal))
        {
            return;
        }

        // Expanded?
        var isExpanded = args.NewState switch
        {
            StateClosed => false,
            StateOpened => true,
            _ => false,
        };

        // Update the state
        if (isExpanded != Expanded)
        {
            Expanded = isExpanded;

            if (ExpandedChanged.HasDelegate)
            {
                await ExpandedChanged.InvokeAsync(isExpanded);
            }
        }

        // Update the FluentTree owner
        if (OwnerTreeView is not null && OwnerTreeView.OnExpandedChanged.HasDelegate)
        {
            await OwnerTreeView.OnExpandedChanged.InvokeAsync(this);
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
            builder.AddAttribute(3, nameof(Text), owner.ItemTemplate is null ? item.Text : null);
            builder.AddAttribute(4, nameof(Expanded), item.Expanded);
            builder.AddAttribute(5, nameof(IconStart), item.IconStart);
            builder.AddAttribute(6, nameof(IconEnd), item.IconEnd);
            builder.AddAttribute(7, nameof(IconAside), item.IconAside);
            builder.AddAttribute(8, nameof(IconCollapsed), item.IconCollapsed);
            builder.AddAttribute(9, nameof(IconExpanded), item.IconExpanded);
            builder.SetKey(item.Id);

            if (owner.ItemTemplate != null)
            {
                builder.AddAttribute(10, nameof(ChildContent), owner.ItemTemplate(item));
            }

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
    public override ValueTask DisposeAsync()
    {
        if (OwnerTreeView is not null && !string.IsNullOrEmpty(Id))
        {
            OwnerTreeView.InternalItems.TryRemove(Id, out _);
        }

        return base.DisposeAsync();
    }
}
