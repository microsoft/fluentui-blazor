﻿using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentNavMenuGroup : FluentComponentBase, INavMenuChildElement, INavMenuParentElement, IDisposable
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the icon to display for this group
    /// before its <see cref="Text"/>.
    /// </summary>
    [Parameter]
    public Icon? Icon { get; set; }

    /// <summary>
    /// Gets or sets the destination of the link.
    /// </summary>
    [Parameter]
    public string? Href { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the menu group is disabled
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }


    /// <summary>
    /// Returns <see langword="true"/> if the group is expanded,
    /// and <see langword="false"/> if collapsed.
    /// </summary>
    [Parameter]
    public bool Expanded { get; set; }

    /// <summary>
    /// Gets or sets a callback that is triggered whenever <see cref="Expanded"/> changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> ExpandedChanged { get; set; }

    /// <summary>
    /// Gets or sets the text of the menu group
    /// </summary>
    [Parameter]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the width of the menu group
    /// </summary>
    [Parameter]
    public int? Width { get; set; }

    /// <summary>
    /// If set to <see langword="true"/> then the tree will
    /// expand when it is created.
    /// </summary>
    [Parameter]
    public bool InitiallyExpanded { get; set; }

    /// <summary>
    /// Gets or sets if the item is selected.
    /// </summary>
    [Parameter]
    public bool Selected { get; set; }

    /// <summary>
    /// Event callback for when <see cref="Selected"/> changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> SelectedChanged { get; set; }

    [CascadingParameter]
    private FluentNavMenu NavMenu { get; set; } = default!;

    [CascadingParameter(Name = "NavMenuExpanded")]
    private bool NavMenuExpanded { get; set; }

    [CascadingParameter]
    private INavMenuParentElement Owner { get; set; } = null!;

    [CascadingParameter(Name = "NavMenuItemSiblingHasIcon")]
    private bool SiblingHasIcon { get; set; }

    /// <summary>
    /// Returns <see langword="true"/> if the group is collapsed,
    /// and <see langword="false"/> if expanded.
    /// </summary>
    public bool Collapsed => !Expanded;

    private FluentTreeItem _treeItem = null!;
    private readonly List<INavMenuChildElement> _childElements = new();
    private bool HasChildIcons => ((INavMenuParentElement)this).HasChildIcons;



    public FluentNavMenuGroup()
    {
        Id = Identifier.NewId();
    }

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("navmenu-parent-element")
        .AddClass("navmenu-group")
        .AddClass("navmenu-element")
        .AddClass("navmenu-child-element")
        .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle("width", $"{Width}px", () => Width.HasValue)
        .AddStyle(Style)
        .Build();

    public bool HasIcon => Icon != null;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Owner.Register(this);
        NavMenu.Register(this);
        if (InitiallyExpanded && Collapsed)
        {
            Expanded = true;
            if (ExpandedChanged.HasDelegate)
            {
                await ExpandedChanged.InvokeAsync(true);
            }
        }
    }

    /// <summary>
    /// Dispose of this navmenu group.
    /// </summary>
    void IDisposable.Dispose()
    {
        Owner.Unregister(this);
        NavMenu.Unregister(this);
        _childElements.Clear();
    }


    void INavMenuParentElement.Register(INavMenuChildElement child)
    {
        Owner.Register(child);
        StateHasChanged();
    }

    void INavMenuParentElement.Unregister(INavMenuChildElement child)
    {
        Owner.Unregister(child);
        StateHasChanged();
    }

    IEnumerable<INavMenuChildElement> INavMenuParentElement.GetChildElements() => _childElements;

    FluentTreeItem INavMenuChildElement.TreeItem => _treeItem;


    private Task ToggleCollapsedAsync() => HandleExpandedChangedAsync(!Expanded);

    private async Task HandleExpandedChangedAsync(bool value)
    {
        if (value == Expanded)
        {
            return;
        }

        Expanded = value;
        if (ExpandedChanged.HasDelegate)
        {
            await ExpandedChanged.InvokeAsync(value);
        }

        await NavMenu.MenuItemExpandedChangedAsync(this);
    }

    private async Task HandleSelectedChangedAsync(bool value)
    {
        if (value == Selected)
        {
            return;
        }

        Selected = value;
        if (SelectedChanged.HasDelegate)
        {
            await SelectedChanged.InvokeAsync(value);
        }
    }

    private bool Visible => NavMenu.Expanded || HasIcon;

}
