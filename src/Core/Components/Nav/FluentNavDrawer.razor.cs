// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a navigation drawer component that displays a hamburger icon and provides navigation functionality within
/// a Fluent UI application.
/// </summary>
/// <remarks>Use the FluentNavDrawer to present a collapsible navigation menu, typically accessed via a hamburger
/// icon. The component supports customization of the displayed icon and its tooltip text. It is designed for
/// integration into Fluent UI layouts and can be configured using the provided parameters.</remarks>
public partial class FluentNavDrawer : FluentComponentBase
{
    /// <summary />
    public FluentNavDrawer(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the icon to display.
    /// By default, this icon is a hamburger icon.
    /// </summary>
    [Parameter]
    public Icon Icon { get; set; } = new CoreIcons.Regular.Size20.LineHorizontal3();

    /// <summary>
    /// Gets or sets the title to display when the user hovers over the hamburger icon.
    /// </summary>
    [Parameter]
    public string? IconTitle { get; set; }

    /// <summary>
    /// Gets or sets wether to enable using links in the nav items.
    /// </summary>
    [Parameter]
    public bool UseLinks { get; set; } = true;
}
