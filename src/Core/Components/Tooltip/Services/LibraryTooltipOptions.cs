// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Options for the Fluent UI Blazor component library.
/// </summary>
public class LibraryTooltipOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LibraryTooltipOptions"/> class.
    /// </summary>
    internal LibraryTooltipOptions()
    {
    }

    /// <summary>
    /// Gets or sets a value indicating whether the library should use the TooltipServiceProvider.
    /// If set to true, add the FluentTooltipProvider component at end of the MainLayout.razor page.
    /// </summary>
    public bool UseServiceProvider { get; set; } = true;

    /// <summary>
    /// Gets or sets the default tooltip positioning.
    /// </summary>
    public Positioning? Positioning { get; set; }

    /// <summary>
    /// Gets or sets number of milliseconds to delay the tooltip from showing/hiding on hover.
    /// The default value is `null`. Internally the component uses 250ms when no value is provided.
    /// </summary>
    public int? Delay { get; set; }
}
