// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Component to display a hamburger icon that can be used to open and close a menu, on mobile device only.
/// </summary>
public partial class FluentLayoutHamburger
{
    /// <summary>
    /// <inheritdoc cref="FluentComponentBase.Class"/>
    /// </summary>
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-layout-hamburger")
        .Build();

    /// <summary>
    /// Gets or sets the parent layout component.
    /// </summary>
    [CascadingParameter]
    protected FluentLayout? LayoutContainer { get; set; }

    /// <summary>
    /// Gets or sets the layout to which this hamburger belongs.
    /// If not set, the parent layout is used.
    /// </summary>
    [Parameter]
    public FluentLayout? Layout { get; set; }

    /// <summary>
    /// Gets or sets the icon to display.
    /// By default, this icon is a hamburger icon.
    /// </summary>
    [Parameter]
    public Icon Icon { get; set; } = new CoreIcons.Regular.Size20.LineHorizontal3();

    /// <summary>
    /// Allows for capturing a mouse click on an icon.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnClick { get; set; }

    /// <summary>
    /// Gets or sets whether the hamburger icon is shown.
    /// Default is `true`
    /// </summary>
    internal bool ShowMobileOnly { get; set; } = true;

    /// <summary />
    private async Task HamburgerClickAsync(MouseEventArgs e)
    {
        var layout = Layout ?? LayoutContainer;

        if (layout != null)
        {
            layout.MenuOpened = !(layout.MenuOpened == true);
        }

        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(layout?.MenuOpened ?? false);
        }
    }
}
