// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a tree item in a <see cref="FluentTreeView"/>.
/// </summary>
public partial class FluentTreeItem : FluentComponentBase
{
    private static readonly MarkupString DefaultIcon_Expanded = new(" <svg expanded  width=\"12\" height=\"12\" xmlns=\"http://www.w3.org/2000/svg\" fill=\"currentColor\" slot=\"chevron\"><path d=\"M4.65 2.15a.5.5 0 000 .7L7.79 6 4.65 9.15a.5.5 0 10.7.7l3.5-3.5a.5.5 0 000-.7l-3.5-3.5a.5.5 0 00-.7 0z\"></path></svg>");
    private static readonly MarkupString DefaultIcon_Collapsed = new("<svg collapsed width=\"12\" height=\"12\" xmlns=\"http://www.w3.org/2000/svg\" fill=\"currentColor\" slot=\"chevron\"><path d=\"M4.65 2.15a.5.5 0 000 .7L7.79 6 4.65 9.15a.5.5 0 10.7.7l3.5-3.5a.5.5 0 000-.7l-3.5-3.5a.5.5 0 00-.7 0z\"></path></svg>");

    /// <summary/>
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary/>
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

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

    ///// <summary>
    ///// When true, the control will appear selected by user interaction.
    ///// </summary>
    //[Parameter]
    //public bool Selected { get; set; }

    ///// <summary>
    ///// Called whenever <see cref="Selected"/> changes.
    ///// </summary>
    //[Parameter]
    //public EventCallback<bool> SelectedChanged { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    private async Task OnTreeChangedAsync(TreeItemChangedEventArgs args)
    {
        Console.WriteLine($"Item: {args.Id} - {args.Selected}");
        await Task.CompletedTask;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    private async Task OnTreeToggleAsync(TreeItemToggleEventArgs args)
    {
        Console.WriteLine($"Item: {args.Id} - {args.OldState} - {args.NewState}");
        await Task.CompletedTask;
    }
}
