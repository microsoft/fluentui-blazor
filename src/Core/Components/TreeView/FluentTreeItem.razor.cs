// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a tree item in a <see cref="FluentTree"/>.
/// </summary>
public partial class FluentTreeItem : FluentComponentBase, IDisposable
{
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentTreeItem"/> class.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the size of the tree. Default is <see cref="TreeSize.Medium"/>.
    /// </summary>
    [Parameter]
    public TreeSize? Size { get; set; } = TreeSize.Medium;

    /// <summary>
    /// Gets or sets the appearance of the tree. Default is <see cref="TreeAppearance.Subtle"/>.
    /// </summary>
    [Parameter]
    public TreeAppearance? Appearance { get; set; } = TreeAppearance.Subtle;

    /// <summary>
    /// Gets or sets the list of sub-items to bind to the tree item
    /// </summary>
    [Parameter]
    public IEnumerable<ITreeViewItem>? Items { get; set; }

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

    /// <summary/>
    [Parameter]
    public Icon? IconStartCollapsed { get; set; }

    /// <summary/>
    [Parameter]
    public Icon? IconStartExpanded { get; set; }

    /// <summary/>
    [Parameter]
    public Icon? IconEndCollapsed { get; set; }

    /// <summary/>
    [Parameter]
    public Icon? IconEndExpanded { get; set; }

    /// <summary/>
    [Parameter]
    public Icon? IconAsideCollapsed { get; set; }

    /// <summary/>
    [Parameter]
    public Icon? IconAsideExpanded { get; set; }

    /// <summary>
    /// Gets or sets the owning FluentTreeView
    /// </summary>
    [CascadingParameter]
    private FluentTree? Owner { get; set; }

    /// <summary>
    /// Returns <see langword="true"/> if the tree item is collapsed,
    /// and <see langword="false"/> if expanded.
    /// </summary>
    public bool Collapsed => !Expanded;

    void IDisposable.Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the component and releases any resources.
    /// </summary>
    /// <param name="disposing">If currently disposing</param>
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

    /// <summary/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        Owner?.Register(this);

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

    internal async Task HandleExpandedChangeAsync(TreeChangeEventArgs args)
    {
        if (!string.Equals(args.AffectedId, Id, StringComparison.Ordinal) || args.Expanded is null || args.Expanded == Expanded)
        {
            return;
        }

        Expanded = args.Expanded.Value;

        if (ExpandedChanged.HasDelegate)
        {
            await ExpandedChanged.InvokeAsync(Expanded);
        }

        if (Owner != null)
        {
            await Owner.ItemExpandedChangeAsync(this);
        }
    }

    internal async Task HandleSelectedChangeAsync(TreeChangeEventArgs args)
    {
        if (!string.Equals(args.AffectedId, Id, StringComparison.OrdinalIgnoreCase) || args.Selected is null || args.Selected == Selected)
        {
            return;
        }

        if (Owner?.Items == null)
        {
            await SetSelectedAsync(args.Selected.Value);
        }

        if (Owner != null)
        {
            Selected = args.Selected.Value;
            await Owner.ItemSelectedChangeAsync(this);
        }
    }

    internal static RenderFragment GetFluentTreeItem(FluentTree owner, ITreeViewItem item)
    {
        return TreeItem;

        void TreeItem(RenderTreeBuilder builder)
        {
            builder.OpenComponent<FluentTreeItem>(0);
            builder.AddAttribute(1, "Id", item.Id);
            builder.AddAttribute(2, "Items", item.Items);
            builder.AddAttribute(3, "Text", item.Text);
            builder.AddAttribute(4, "Selected", owner.SelectedItem == item);
            builder.AddAttribute(5, "Expanded", item.Expanded);
            builder.AddAttribute(6, "Disabled", item.Disabled);
            builder.AddAttribute(7, "IconStartCollapsed", item.IconStartCollapsed);
            builder.AddAttribute(8, "IconStartExpanded", item.IconStartExpanded);
            builder.AddAttribute(9, "IconEndCollapsed", item.IconEndCollapsed);
            builder.AddAttribute(10, "IconEndExpanded", item.IconEndExpanded);
            builder.AddAttribute(11, "IconAsideCollapsed", item.IconAsideCollapsed);
            builder.AddAttribute(12, "IconAsideExpanded", item.IconAsideExpanded);
            builder.SetKey(item.Id);

            if (owner.ItemTemplate != null)
            {
                builder.AddAttribute(9, "ChildContent", owner.ItemTemplate(item));
            }

            builder.CloseComponent();
        }
    }
}

