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
        .Build();

    /// <summary/>
    [CascadingParameter]
    internal FluentTreeView? TreeView { get; set; }

    /// <summary>
    /// Gets or sets the size of the tree item.
    /// Default is <see cref="TreeSize.Medium"/>.
    /// </summary>
    [Parameter]
    public TreeSize? Size { get; set; }

    /// <summary>
    /// Gets or sets the appearance of the tree item.
    /// Default is <see cref="TreeAppearance.Subtle"/>.
    /// </summary>
    [Parameter]
    public TreeAppearance? Appearance { get; set; }

    /// <summary>
    /// Gets or sets the text of the tree item
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered on the right side of the tree item.
    /// </summary>
    [Parameter]
    public RenderFragment? AsideTemplate { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the start of tree item,
    /// when the node is collapsed.
    /// If this icon is not set, the <see cref="IconExpanded"/> will be used.
    /// </summary>
    [Parameter]
    public Icon? IconCollapsed { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the start of tree item,
    /// when the node is expanded.
    /// If this icon is not set, the <see cref="IconCollapsed"/> will be used.
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
    protected override void OnInitialized()
    {
        if (TreeView is not null && !string.IsNullOrEmpty(Id))
        {
            TreeView.InternalItems.TryAdd(Id, this);
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

        // Update the FluentTree owner (only to inform the new selected item)
        if (isSelected && TreeView is not null && TreeView.OnSelectedChanged.HasDelegate)
        {
            await TreeView.OnSelectedChanged.InvokeAsync(this);
        }

        // Update the FluentTree owner for the SelectedId property
        if (isSelected && TreeView is not null && TreeView.SelectedIdChanged.HasDelegate)
        {
            await TreeView.SelectedIdChanged.InvokeAsync(Id);
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
        if (TreeView is not null && TreeView.OnExpandedChanged.HasDelegate)
        {
            await TreeView.OnExpandedChanged.InvokeAsync(this);
        }
    }

    /// <summary />
    public override ValueTask DisposeAsync()
    {
        if (TreeView is not null && !string.IsNullOrEmpty(Id))
        {
            TreeView.InternalItems.TryRemove(Id, out _);
        }

        return base.DisposeAsync();
    }
}
