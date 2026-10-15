// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Web;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentSplitButton component allows users to combine a button with a menu where the left part triggers a primary action and the right part opens a menu with secondary actions.
/// is often found inside forms, dialogs, drawers (panels) and/or pages.
/// </summary>
public partial class FluentSplitButton : FluentComponentBase
{
    /// <summary />
    public FluentSplitButton(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
       .AddStyle("--menu-max-height", Height, when: !string.IsNullOrEmpty(Height))
       .Build();

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
    /// Gets or sets a value indicating whether the split button is disabled.
    /// When disabled, neither the primary action nor the menu can be activated.
    /// Use <see cref="PrimaryActionDisabled"/> or <see cref="MenuButtonDisabled"/> to override this value for one half only.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the split button is disabled yet still reachable via keyboard focus.
    /// Like <see cref="Disabled"/>, neither the primary action nor the menu can be activated,
    /// but both halves stay in the tab order (with <c>aria-disabled="true"</c>), so keyboard and screen reader users can still reach them.
    /// When combined with <see cref="Disabled"/>, the halves remain focusable.
    /// </summary>
    [Parameter]
    public bool DisabledFocusable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the primary action button is disabled.
    /// When set, this overrides <see cref="Disabled"/> and <see cref="DisabledFocusable"/> for the primary action only.
    /// The default value is `null`, meaning the primary action follows <see cref="Disabled"/> and <see cref="DisabledFocusable"/>.
    /// </summary>
    [Parameter]
    public bool? PrimaryActionDisabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the menu button is disabled.
    /// When set, this overrides <see cref="Disabled"/> and <see cref="DisabledFocusable"/> for the menu button only.
    /// The default value is `null`, meaning the menu button follows <see cref="Disabled"/> and <see cref="DisabledFocusable"/>.
    /// </summary>
    [Parameter]
    public bool? MenuButtonDisabled { get; set; }

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
    /// Gets or sets a value indicating whether the button renders icon-only (no visible text label).
    /// Typically used when <see cref="IconStart"/> is set and no text label is needed.
    /// When using icon-only mode, provide an accessible name via <see cref="Title"/> or an <c>aria-label</c> attribute to ensure screen reader accessibility.
    /// </summary>
    [Parameter]
    public bool IconOnly { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the start of button content.
    /// </summary>
    [Parameter]
    public Icon? IconStart { get; set; }

    ///// <summary>
    ///// Gets or sets the <see cref="Icon"/> displayed at the end of button content.
    ///// </summary>
    // This can be enabled once the web components have been fixed
    //[Parameter]
    //public Icon? IconToggle { get; set; }

    /// <summary>
    /// Gets or sets the title of the button.
    /// The text usually displayed in a `tooltip` popup when the mouse is over the button.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the plain-text label rendered inside the primary button area (e.g., <c>Label="Save"</c>).
    /// If both <see cref="Label"/> and <see cref="ChildContent"/> are set, both are rendered together.
    /// For rich content such as icons or custom markup, use <see cref="ChildContent"/> instead.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// Gets or sets the max height of the menu, e.g. 300px
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

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

    /// <summary>
    /// Raised when a FluentMenuItem is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<MenuItemEventArgs> OnMenuClick { get; set; }

    /// <summary>
    /// Raised when a FluentMenuItem's Checked state changes.
    /// Unlike <see cref="OnMenuClick"/>, this is also raised while the menu button is disabled,
    /// so the checked state of radio items unchecked by the menu itself stays in sync.
    /// </summary>
    [Parameter]
    public EventCallback<MenuItemEventArgs> OnMenuCheckedChanged { get; set; }

    /// <summary />
    private bool IsPrimaryActionDisabled => PrimaryActionDisabled ?? (Disabled || DisabledFocusable);

    /// <summary />
    private bool IsMenuButtonDisabled => MenuButtonDisabled ?? (Disabled || DisabledFocusable);

    /// <summary />
    private async Task OnMenuClickHandlerAsync(MenuItemEventArgs args)
    {
        if (!IsMenuButtonDisabled && OnMenuClick.HasDelegate)
        {
            await OnMenuClick.InvokeAsync(args);
        }
    }

    /// <summary />
    private async Task OnMenuCheckedChangedHandlerAsync(MenuItemEventArgs args)
    {
        if (OnMenuCheckedChanged.HasDelegate)
        {
            await OnMenuCheckedChanged.InvokeAsync(args);
        }
    }
}
