// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentMenuButton : FluentComponentBase
{
    private bool _visible;
    private Color _iconColor = Color.Fill;
    private string? _buttonId;
    private string? _menuId;

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
    /// Gets or sets the texts shown on the button. This property will be ignored if <see cref="ButtonContent"/> is provided.
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the start of button content.
    /// </summary>
    [Parameter]
    public Icon? IconStart { get; set; }

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
    /// Using this event prevents the execution of any OnClick event on an included FluentMenuItem.
    /// </summary>
    [Parameter]
    public EventCallback<MenuChangeEventArgs> OnMenuChanged { get; set; }

    /// <summary>
    /// The content to be rendered inside the button. This parameter should be supplied if you do not want to render a chevron
    /// on the menu button. Only one of <see cref="Text"/> or <see cref="ButtonContent"/> may be provided.
    /// </summary>
    [Parameter]
    public RenderFragment? ButtonContent { get; set; }

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

    protected override void OnInitialized()
    {
        _buttonId = Identifier.NewId();
        _menuId = $"{_buttonId}-menu";
    }

    protected override void OnParametersSet()
    {
        if (Text is not null && ButtonContent is not null)
        {
            throw new ArgumentException($"Only one of the parameters {nameof(Text)} or {nameof(ButtonContent)} can be provided.");
        }

        _iconColor = ButtonAppearance == Appearance.Accent ? Color.Fill : Color.FillInverse;
    }

    private void ToggleMenu()
    {
        _visible = !_visible;
    }

    private async Task OnMenuChangeAsync(MenuChangeEventArgs args)
    {
        if (!OnMenuChanged.HasDelegate)
        {
            return;
        }

        if (args is not null && args.Id is not null)
        {
            await OnMenuChanged.InvokeAsync(args);
        }

        _visible = false;
    }
}
