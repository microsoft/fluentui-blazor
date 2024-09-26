using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;
public partial class FluentButton : FluentComponentBase, IAsyncDisposable
{

    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Button/FluentButton.razor.js";

    private readonly RenderFragment _renderButton;

    /// <summary />
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    private IJSObjectReference? _jsModule { get; set; }

    private bool LoadingOverlay => Loading && IconStart == null && IconEnd == null;

    /// <summary>
    /// Determines if the element should receive document focus on page load.
    /// </summary>
    [Parameter]
    public bool? Autofocus { get; set; }

    /// <summary>
    /// Gets or sets the id of a form to associate the element to.
    /// </summary>
    [Parameter]
    public string? FormId { get; set; }

    /// <summary>
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/button">button</see> element for more details.
    /// </summary>
    [Parameter]
    public string? Action { get; set; }

    /// <summary>
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/button">button</see> element for more details.
    /// </summary>
    [Parameter]
    public string? Enctype { get; set; }

    /// <summary>
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/button">button</see> element for more details.
    /// </summary>
    [Parameter]
    public string? Method { get; set; }

    /// <summary>
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/button">button</see> element for more details.
    /// </summary>
    [Parameter]
    public bool? NoValidate { get; set; }

    /// <summary>
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/button">button</see> element for more details.
    /// Possible values: "_self" | "_blank" | "_parent" | "_top"
    /// </summary>
    [Parameter]
    public string? Target { get; set; }

    /// <summary>
    /// Gets or sets the button type. See <see cref="ButtonType"/> for more details.
    /// Default is ButtonType.Button.
    /// </summary>
    [Parameter]
    public ButtonType? Type { get; set; } = ButtonType.Button;

    /// <summary>
    /// Gets or sets the value of the element.
    /// </summary>
    [Parameter]
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets the element's current value.
    /// </summary>
    [Parameter]
    public string? CurrentValue { get; set; }

    /// <summary>
    /// Disables the form control, ensuring it doesn't participate in form submission.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the name of the element.
    /// Allows access by name from the associated form.
    /// </summary>
    [Parameter]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the element needs to have a value.
    /// </summary>
    [Parameter]
    public bool Required { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance. See <seealso cref="AspNetCore.Components.Appearance"/>
    /// Defaults to <seealso cref="AspNetCore.Components.Appearance.Neutral"/>
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; } = AspNetCore.Components.Appearance.Neutral;

    /// <summary>
    /// Gets or sets the background color of this button (overrides the <see cref="Appearance"/> property).
    /// Set the value "rgba(0, 0, 0, 0)" to display a transparent button.
    /// </summary>
    [Parameter]
    public string? BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the color of the font (overrides the <see cref="Appearance"/> property).
    /// </summary>
    [Parameter]
    public string? Color { get; set; }

    /// <summary>
    /// Display a progress ring and disable the button.
    /// </summary>
    [Parameter]
    public bool Loading { get; set; } = false;

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the start of button content.
    /// </summary>
    [Parameter]
    public Icon? IconStart { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the end of button content.
    /// </summary>
    [Parameter]
    public Icon? IconEnd { get; set; }

    /// <summary>
    /// Gets or sets the title of the button.
    /// The text usually displayed in a 'tooltip' popup when the mouse is over the button.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Command executed when the user clicks on the button.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    protected override void OnParametersSet()
    {
        string[] values = ["_self", "_blank", "_parent", "_top"];
        if (!string.IsNullOrEmpty(Target) && !values.Contains(Target))
        {
            throw new ArgumentException("Target must be one of the following values: _self, _blank, _parent, _top");
        }
        if (Appearance == AspNetCore.Components.Appearance.Filled)
        {
            throw new ArgumentException("Appearance.Filled is not supported for FluentButton");
        }
        if (Appearance == AspNetCore.Components.Appearance.Hypertext)
        {
            throw new ArgumentException("Appearance.Hypertext is not supported for FluentButton");
        }
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && Id is not null && Type != ButtonType.Button)
        {
            _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));
            await _jsModule.InvokeVoidAsync("updateProxy", Id);
        }
    }

    /// <summary />
    protected virtual MarkupString CustomStyle => new InlineStyleBuilder()
        .AddStyle($"#{Id}::part(control)", "background", $"padding-box linear-gradient({BackgroundColor}, {BackgroundColor}), border-box {BackgroundColor}", when: !string.IsNullOrEmpty(BackgroundColor))
        .AddStyle($"#{Id}::part(control)", "color", $"{Color}", when: !string.IsNullOrEmpty(Color))
        .AddStyle($"#{Id}::part(control):hover", "opacity", "0.8", when: !string.IsNullOrEmpty(Color) || !string.IsNullOrEmpty(BackgroundColor))
        .BuildMarkupString();

    /// <summary>
    /// Constructs an instance of <see cref="FluentButton"/>.
    /// </summary>
    public FluentButton()
    {
        _renderButton = RenderButton;
    }

    /// <summary />
    protected override void OnInitialized()
    {
        if (string.IsNullOrEmpty(Id) && (!string.IsNullOrEmpty(BackgroundColor) || !string.IsNullOrEmpty(Color)))
        {
            Id = Identifier.NewId();
        }
    }

    /// <summary />
    protected async Task OnClickHandlerAsync(MouseEventArgs e)
    {
        if (!Disabled && OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(e);
        }

        await Task.CompletedTask;
    }

    public void SetDisabled(bool disabled)
    {
        Disabled = disabled;
        StateHasChanged();
    }

    private string RingStyle(Icon icon)
    {
        var size = icon.Width - 4;
        var inverse = Appearance == AspNetCore.Components.Appearance.Accent ? " filter: invert(1);" : string.Empty;

        return $"width: {size}px; height: {size}px;{inverse}";
    }
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_jsModule is not null)
            {
                await _jsModule.DisposeAsync();
            }
        }
        catch (Exception ex) when (ex is JSDisconnectedException ||
                                   ex is OperationCanceledException)
        {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
        }
    }
}
