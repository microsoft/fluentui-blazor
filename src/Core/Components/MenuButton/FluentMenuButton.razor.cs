using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentMenuButton : FluentComponentBase
{
    private bool _visible;
    private Color _iconColor = Color.Fill;
    private string? _buttonId;

    protected string? MenuStyleValue => new StyleBuilder(MenuStyle)
        .AddStyle("position", "relative")
        .Build();

    /// <summary>
    /// Gets or sets a reference to the button.
    /// </summary>
    [Parameter]
    public FluentButton? Button { get; set; }

    /// <summary>
    /// Gets or sets the button appearance.
    /// </summary>
    [Parameter]
    public Appearance ButtonAppearance { get; set; } = Appearance.Accent;

    /// <summary>
    /// Gets or sets a reference to the menu.
    /// </summary>
    [Parameter]
    public FluentMenu? Menu { get; set; }

    /// <summary>
    /// Gets or sets the texts shown on th button.
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the button style.
    /// </summary>
    [Parameter]
    public string? ButtonStyle { get; set; }

    /// <summary>
    /// Gets or sets the menu style.
    /// </summary>
    [Parameter]
    public string? MenuStyle { get; set; }

    /// <summary>
    /// Gets or sets the items to show in the menu.
    /// </summary>
    [Parameter]
    public Dictionary<string, string> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the content to be shown in the menu.
    /// Should consist of <see cref="FluentMenuItem"/> components.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// The callback to invoke when a menu item is chosen.
    /// </summary>
    [Parameter]
    public EventCallback<MenuChangeEventArgs> OnMenuChanged { get; set; }

    protected override void OnInitialized()
    {
        _buttonId = Identifier.NewId();
    }

    protected override void OnParametersSet()
    {
        _iconColor = ButtonAppearance == Appearance.Accent ? Color.Fill : Color.FillInverse;
    }

    private void ToggleMenu()
    {
        _visible = !_visible;
    }

    private async Task OnMenuChangeAsync(MenuChangeEventArgs args)
    {
        if (args is not null && args.Id is not null)
        {
            await OnMenuChanged.InvokeAsync(args);
        }
        _visible = false;
    }

    private void OnKeyDown(KeyboardEventArgs args)
    {
        if (args is not null && args.Key == "Escape")
        {
            _visible = false;
        }
    }
}
