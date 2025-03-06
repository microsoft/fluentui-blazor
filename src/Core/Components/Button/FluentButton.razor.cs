// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentButton component allows users to commit a change or trigger an action via a single click or tap and
/// is often found inside forms, dialogs, drawers (panels) and/or pages.
/// </summary>
public partial class FluentButton : FluentComponentBase
{
    /// <summary />
    private bool LoadingOverlay => Loading && IconStart == null && IconEnd == null;

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("loading-button", when: () => LoadingOverlay)
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("background-color", BackgroundColor, when: () => !string.IsNullOrEmpty(BackgroundColor))
        .AddStyle("color", Color, when: () => !string.IsNullOrEmpty(Color))
        .AddStyle("opacity", "0.3", when: () => Disabled && (!string.IsNullOrEmpty(BackgroundColor) || !string.IsNullOrEmpty(Color)))
        .Build();

    /// <summary>
    /// Gets or sets whether the button should be focused when the page is loaded.
    /// </summary>
    [Parameter]
    public bool AutoFocus { get; set; } = false;

    /// <summary>
    /// Gets or sets the id of a form to associate the element to.
    /// </summary>
    [Parameter]
    public string? FormId { get; set; }

    /// <summary>
    /// Gets or sets the URL that processes the information submitted by the button.
    /// </summary>
    [Parameter]
    public string? FormAction { get; set; }

    /// <summary>
    /// Gets or sets how to encode the form data that is submitted
    /// (if the button is a submit button).
    /// </summary>
    [Parameter]
    public string? FormEncType { get; set; }

    /// <summary>
    /// Gets or sets the HTTP method used to submit the form
    /// (if the button is a submit button).
    /// </summary>
    [Parameter]
    public string? FormMethod { get; set; }

    /// <summary>
    /// Gets or sets if the form need to be validated when it is submitted
    /// (if the button is a submit button).
    /// </summary>
    [Parameter]
    public bool? FormNoValidate { get; set; }

    /// <summary>
    /// Gets or sets the author-defined name or standardized, underscore-prefixed keyword indicating where to display the response from submitting the form.
    /// Possible values: `_self` | `_blank` | `_parent` | `_top`.
    /// </summary>
    [Parameter]
    public string? FormTarget { get; set; }

    /// <summary>
    /// Gets or sets the button type. See <see cref="ButtonType"/> for more details.
    /// Default is `null`. Internally the component uses  <see cref="ButtonType.Button"/> as default.
    /// </summary>
    [Parameter]
    public ButtonType? Type { get; set; }

    /// <summary>
    /// Gets or sets the shape of the button.
    /// Default is `null`. Internally the component uses <see cref="ButtonShape.Rounded"/> as default.
    /// </summary>
    [Parameter]
    public ButtonShape? Shape { get; set; }

    /// <summary>
    /// Gets or sets the size of the button.
    /// Default is `null`. Internally the component uses <see cref="ButtonSize.Medium"/> as default.
    /// </summary>
    [Parameter]
    public ButtonSize? Size { get; set; }

    /// <summary>
    /// Gets or sets the value associated with the button.
    /// This value is passed to the server in params when the form is submitted using this button.
    /// </summary>
    [Parameter]
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets the element's disabled state, ensuring it doesn't participate in form submission.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; } = false;

    /// <summary>
    /// Gets or sets the value indicating the button is focusable.
    /// </summary>
    [Parameter]
    public bool DisabledFocusable { get; set; } = false;

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
    /// Gets or sets the visual appearance.
    /// Default is `null'. Internally the component uses <see cref="ButtonAppearance.Default"/> as default.
    /// </summary>
    [Parameter]
    public ButtonAppearance? Appearance { get; set; }

    /// <summary>
    /// Gets or sets the background color of this button (overrides the <see cref="Appearance"/> property).
    /// Set the value `rgba(0, 0, 0, 0)` to display a transparent button.
    /// </summary>
    [Parameter]
    public string? BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the color of the font (overrides the <see cref="Appearance"/> property).
    /// </summary>
    [Parameter]
    public string? Color { get; set; }

    /// <summary>
    /// Gets or sets whether to display a progress ring and disable the button.
    /// </summary>
    [Parameter]
    public bool Loading { get; set; } = false;

    /// <summary>
    /// Gets or sets if the button only shows an icon
    /// Can be used when using <see cref="ChildContent"/> that renders as an icon
    /// </summary>
    [Parameter]
    public bool IconOnly { get; set; }

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
    /// The text usually displayed in a `tooltip` popup when the mouse is over the button.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets a way to prevent further propagation of the current event in the capturing and bubbling phases.
    /// </summary>
    [Parameter]
    public bool StopPropagation { get; set; } = false;

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

    /// <summary />
    protected override void OnParametersSet()
    {
        string[] values = ["_self", "_blank", "_parent", "_top"];

        if (!string.IsNullOrEmpty(FormTarget) && !values.Contains(FormTarget, StringComparer.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Target must be one of the following values: _self, _blank, _parent, _top");
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

    /// <summary>
    /// Disables the button.
    /// </summary>
    /// <param name="disabled">True to disable the button</param>
    public void SetDisabled(bool disabled)
    {
        Disabled = disabled;
        StateHasChanged();
    }

    /// <summary />
    private static string RingStyle(Icon icon)
    {
        var size = icon.Width.ToString(CultureInfo.InvariantCulture);

        return $"width: {size}px; height: {size}px;";
    }
}
