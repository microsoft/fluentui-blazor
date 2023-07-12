using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.Fast.Components.FluentUI;
public partial class FluentButton : FluentComponentBase
{
    /// <summary>
    /// Determines if the element should receive document focus on page load.
    /// </summary>
    [Parameter]
    public bool? Autofocus { get; set; }

    /// <summary>
    /// The id of a form to associate the element to.
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
    /// The button type. See <see cref="ButtonType"/> for more details.
    /// Default is ButtonType.Button"
    /// </summary>
    [Parameter]
    public ButtonType? Type { get; set; } = ButtonType.Button;

    /// <summary>
    /// The value of the element
    /// </summary>
    [Parameter]
    public string? Value { get; set; }

    /// <summary>
    /// The element's current value 
    /// </summary>
    [Parameter]
    public string? CurrentValue { get; set; }

    /// <summary>
    /// Disables the form control, ensuring it doesn't participate in form submission
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// The name of the element.Allows access by name from the associated form.
    /// </summary>
    [Parameter]
    public string? Name { get; set; }

    /// <summary>
    /// The element needs to have a value
    /// </summary>
    [Parameter]
    public bool Required { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance. See <seealso cref="FluentUI.Appearance"/>
    /// Defaults to <seealso cref="FluentUI.Appearance.Neutral"/>
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; } = FluentUI.Appearance.Neutral;

    /// <summary>
    /// <see cref="Icon"/> displayed to the left of button content.
    /// </summary>
    [Parameter]
    public Icon? IconStart { get; set; }

    /// <summary>
    /// <see cref="Icon"/> displayed to the right of button content.
    /// </summary>
    [Parameter]
    public Icon? IconEnd { get; set; }

    /// <summary>
    /// Title of the button: the text usually displayed in a 'tooltip' popup when the mouse is over the button.
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
        string[] values = { "_self", "_blank", "_parent", "_top" };
        if (!string.IsNullOrEmpty(Target) && !values.Contains(Target))
            throw new ArgumentException("Target must be one of the following values: _self, _blank, _parent, _top");
    }

    /// <summary />
    protected Task OnClickHandlerAsync(MouseEventArgs e)
    {
        if (!Disabled && OnClick.HasDelegate)
        {
            return OnClick.InvokeAsync(e);
        }

        return Task.CompletedTask;
    }

    public void SetDisabled(bool disabled)
    {
        Disabled = disabled;
        StateHasChanged();
    }
}