// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A FluentUI AnchorButton component.
/// </summary>
public partial class FluentCompoundButton : FluentComponentBase, ITooltipComponent
{
    /// <summary />
    public FluentCompoundButton(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("background-color", BackgroundColor, when: () => !string.IsNullOrEmpty(BackgroundColor))
        .AddStyle("color", Color, when: () => !string.IsNullOrEmpty(Color))
        .Build();

    /// <summary>
    /// Gets or sets whether the button should be focused when the page is loaded.
    /// </summary>
    [Parameter]
    public bool AutoFocus { get; set; } = false;

    /// <summary>
    /// Gets or sets the shape of the button.
    /// The default value is `null`. Internally the component uses <see cref="ButtonShape.Rounded"/> as its default value.
    /// </summary>
    [Parameter]
    public ButtonShape? Shape { get; set; }

    /// <summary>
    /// Gets or sets the size of the button.
    /// The default value is `null`. Internally the component uses <see cref="ButtonSize.Medium"/> as its default value.
    /// </summary>
    [Parameter]
    public ButtonSize? Size { get; set; }

    /// <summary>
    /// Gets or sets the name of the element.
    /// Allows access by name from the associated form.
    /// </summary>
    [Parameter]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance.
    /// The default value is `null`. Internally the component uses <see cref="ButtonAppearance.Outline"/> as its default value.
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
    /// Gets or sets a way to prevent further propagation of the current event in the capturing and bubbling phases.
    /// </summary>
    [Parameter]
    public bool StopPropagation { get; set; } = false;

    /// <summary>
    /// Gets or sets if the button only shows an icon
    /// Can be used when using <see cref="ChildContent"/> that renders as an icon
    /// </summary>
    [Parameter]
    public bool IconOnly { get; set; }

    /// <summary>
    ///  Gets or sets the content to be rendered inside the button.
    ///  This can be used as an alternative to specifying the content as a child component of the button.
    ///  If both are specified, both will be rendered.
    ///  </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside description area of the button.
    /// </summary>
    [Parameter]
    public RenderFragment? Description { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the button.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Command executed when the user clicks on the button.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <inheritdoc cref="ITooltipComponent.Tooltip" />
    [Parameter]
    public string? Tooltip { get; set; }

    /// <summary />
    protected override async Task OnInitializedAsync()
    {
        await base.RenderTooltipAsync(Tooltip);
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
}
