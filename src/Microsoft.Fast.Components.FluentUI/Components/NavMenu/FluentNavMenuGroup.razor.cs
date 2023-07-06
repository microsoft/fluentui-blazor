﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.Utilities;


namespace Microsoft.Fast.Components.FluentUI;
public partial class FluentNavMenuGroup : FluentComponentBase, IDisposable
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the icon content to be displayed for this group
    /// before its <see cref="Text"/>.
    /// This setting takes priority over <see cref="Icon"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? IconContent { get; set; }

    /// <summary>
    /// Gets or sets the icon to display for this group
    /// before its <see cref="Text"/>.
    /// This setting is not used when <see cref="IconContent"/>
    /// is not null.
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
    /// Gets or sets whether the menu group is expanded
    /// </summary>
    [Parameter]
    public bool Expanded { get; set; }

    /// <summary>
    /// Gets or sets whether the menu group is selected
    /// </summary>
    [Parameter]
    public bool Selected { get; set; }

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
    /// Callback function for when the menu group is expanded
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnExpandedChanged { get; set; }


    [CascadingParameter]
    private FluentNavMenu NavMenu { get; set; } = default!;

    [CascadingParameter(Name = "NavMenuExpanded")]
    private bool NavMenuExpanded { get; set; }

    private bool Collapsed => !Expanded;

    public FluentNavMenuGroup()
    {
        Id = Identifier.NewId();
    }

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("navmenu-group")
        .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle("width", $"{Width}px", () => Width.HasValue)
        .AddStyle(Style)
        .Build();

    internal bool HasIcon => Icon != null || IconContent is not null;

    /// <summary>
    /// Ensures the <see cref="FluentNavMenu"/> is collasped.
    /// </summary>
    /// <returns></returns>
    public async Task CollapseAsync()
    {
        if (Collapsed)
            return;

        Expanded = false;

        if (OnExpandedChanged.HasDelegate)
        {
            await OnExpandedChanged.InvokeAsync(Expanded);
        }

        StateHasChanged();
    }

    /// <summary>
    /// Ensures the <see cref="FluentNavMenu"/> is expanded.
    /// </summary>
    /// <returns></returns>
    public async Task ExpandAsync()
    {
        if (Expanded)
            return;

        Expanded = true;

        if (OnExpandedChanged.HasDelegate)
        {
            await OnExpandedChanged.InvokeAsync(Expanded);
        }

        await NavMenu.GroupExpandedAsync(this);
        StateHasChanged();
    }

    private async Task HandleKeyDownAsync(KeyboardEventArgs args)
    {
        Task handler = args.Code switch
        {
            "Enter" => ExpandAsync(),
            "ArrowRight" => ExpandAsync(),
            "ArrowLeft" => CollapseAsync(),
            _ => Task.CompletedTask
        };
        await handler;
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        NavMenu.AddNavMenuGroup(this);
    }

    /// <summary>
    /// Dispose of this navmenu group.
    /// </summary>
    void IDisposable.Dispose()
    {
        NavMenu.RemoveNavMenuGroup(this);
    }

    private Task ToggleCollapsedAsync() =>
        Expanded
        ? CollapseAsync()
        : ExpandAsync();
}
