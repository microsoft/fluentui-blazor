using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentNavMenu : FluentComponentBase
{
    private const string WIDTH_COLLAPSED_MENU = "40px";
    
    internal string? ClassValue => new CssBuilder("fluent-nav-menu")
        .AddClass(Class)
        .AddClass("collapsed", () => !Expanded)
        .Build();

    internal string? StyleValue => new StyleBuilder(Style)
        .AddStyle("margin", Margin, !string.IsNullOrEmpty(Margin))
        .AddStyle("width", $"{Width}px", () => Expanded && Width.HasValue)
        .AddStyle("width", "100%", () => Expanded && !Width.HasValue)
        .AddStyle("width", WIDTH_COLLAPSED_MENU, () => !Expanded)
        .AddStyle("min-width", WIDTH_COLLAPSED_MENU, () => !Expanded)
        .Build();

    /// <summary>
    /// Gets or sets the content to be rendered for the collapse icon when the menu is collapsible. 
    /// The default icon will be used if this is not specified.
    /// </summary>
    [Parameter]
    public RenderFragment? ExpanderContent { get; set; }

    /// <summary>
    /// Gets or sets the title of the navigation menu using the aria-label attribute.
    /// Defaults to "Navigation menu".
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
    public bool Expanded { get; set; } = true;

    /// <summary>
    /// Event callback for when the <see cref="Expanded"/> property changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> ExpandedChanged { get; set; }

    /// <summary>
    /// Adjust the vertical spacing between navlinks.
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

    public FluentNavMenu()
    {
        Id = Identifier.NewId();
    }

    private Task ToggleExpandedAsync() => SetExpandedAsync(!Expanded);

    private async Task HandleExpandCollapseKeyDownAsync(FluentKeyCodeEventArgs args)
    {
        if (args.TargetId != $"{Id}-expander")
        {
            return;
        }
        Task handler = args.Key switch
        {
            KeyCode.Enter => SetExpandedAsync(!Expanded),
            KeyCode.Right => SetExpandedAsync(true),
            KeyCode.Left => SetExpandedAsync(false),
            _ => Task.CompletedTask
        };
        await handler;
    }

    private async Task SetExpandedAsync(bool value)
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

        StateHasChanged();
    }
}
