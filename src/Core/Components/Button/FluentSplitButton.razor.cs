// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Web;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentSplitButton component allows users to combine a button with a menu where the left part triggers a primary action and the right part opens a menu with secondary actions.
/// is often found inside forms, dialogs, drawers (panels) and/or pages.
/// </summary>
public partial class FluentSplitButton : FluentComponentBase
{
    /// <summary />
    public FluentSplitButton(LibraryConfiguration configuration) : base(configuration) { }

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
    ///  Gets or sets the content to be rendered inside the button.
    ///  This can be used as an alternative to specifying the content as a child component of the button.
    ///  If both are specified, both will be rendered.
    ///  </summary>
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
    /// </summary>
    [Parameter]
    public EventCallback<MenuItemEventArgs> OnMenuCheckedChanged { get; set; }
}
