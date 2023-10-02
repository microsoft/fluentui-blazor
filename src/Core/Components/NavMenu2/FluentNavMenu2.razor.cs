using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentNavMenu2 : FluentComponentBase
{
    private const string WIDTH_COLLAPSED_MENU = "40px";

    internal string? ClassValue => new CssBuilder("fluent-nav-menu")
        .AddClass(Class)
        .Build();

    internal string? StyleValue => new StyleBuilder(Style)
        .AddStyle("margin", Margin, !string.IsNullOrEmpty(Margin))
        .AddStyle("width", $"{Width}px", () => !Collapsed && Width.HasValue)
        .AddStyle("width", WIDTH_COLLAPSED_MENU, () => Collapsed)
        .AddStyle("min-width", WIDTH_COLLAPSED_MENU, () => Collapsed)
        .Build();
    
    /// <summary>
    /// Gets or sets the content to be rendered for the collapse icon
    /// when the menu is collapsible. The default icon will be used if
    /// this is not specified.
    /// </summary>
    [Parameter]
    public RenderFragment? CollapserContent { get; set; }

    /// <summary>
    /// Gets or sets the title of the navigation menu
    /// Default to "Navigation menu"
    /// </summary>
    [Parameter]
    public string? Title { get; set; } = "Navigation menu";

    /// <summary>
    /// Gets or sets the width of the menu (in pixels).
    /// </summary>
    [Parameter]
    public int? Width { get; set; }

    /// <summary>
    /// Gets or sets whether or not the menu can be collapsed.
    /// </summary>
    [Parameter]
    public bool Collapsible { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public bool Collapsed { get; set; } = false;

    /// <summary>
    /// Event callback for when the <see cref="Collapsed"/> property changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> CollapsedChanged { get; set; }

    /// <summary>
    ///  Adjust the vertical spacing between navlinks.
    /// </summary>
    [Parameter]
    public string? Margin { get; set; }


    [Parameter]
    public RenderFragment? ChildContent { get; set; }

   
    /// <summary>
    /// Navigation manager
    /// </summary>
    [Inject]
    protected NavigationManager NavigationManager { get; private set; } = null!;

    public FluentNavMenu2()
    {
        Id = Identifier.NewId();
    }

    private Task ToggleCollapsedAsync() => SetCollapsedAsync(!Collapsed);

    private async Task HandleExpandCollapseKeyDownAsync(KeyboardEventArgs args)
    {
        Task handler = args.Code switch
        {
            "Enter" => SetCollapsedAsync(true),
            "ArrowRight" => SetCollapsedAsync(true),
            "ArrowLeft" => SetCollapsedAsync(false),
            _ => Task.CompletedTask
        };
        await handler;
    }

    private async Task SetCollapsedAsync(bool value)
    {
        if (value == Collapsed)
        {
            return;
        }

        Collapsed = value;
        if (CollapsedChanged.HasDelegate)
        {
            await CollapsedChanged.InvokeAsync(value);
        }

        StateHasChanged();

    }
}