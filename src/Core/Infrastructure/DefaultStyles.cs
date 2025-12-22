// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Defines the global Fluent UI Blazor component library services configuration
/// </summary>
public class DefaultStyles
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultStyles"/> class.
    /// </summary>
    internal DefaultStyles()
    {
    }

    /// <summary>
    /// Gets or sets the default CSS class for the FluentField component, used with all the input components.
    /// The default value is "my-3-o", an overridable "my-3" class. Spacing between text fields and other components is 12 pixels.
    /// </summary>
    public string? FluentFieldClass { get; set; } = "my-3-o";

    /// <summary>
    /// Gets or sets the gap between vertically stacked components.
    /// Default is undefined. For example, you can use `var(--spacingVerticalM)` (12px).
    /// See the CSS <see href="https://developer.mozilla.org/docs/Web/CSS/column-gap">column-gap</see> property.
    /// </summary>
    public string? FluentStackVerticalGap { get; set; }

    /// <summary>
    /// Gets or sets the gap between horizontally stacked components.
    /// Default is undefined. For example, you can use `var(--spacingHorizontalM)` (12px).
    /// See the CSS <see href="https://developer.mozilla.org/docs/Web/CSS/row-gap">row-gap</see> property.
    /// </summary>
    public string? FluentStackHorizontalGap { get; set; }
}
