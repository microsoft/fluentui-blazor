// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
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
    /// The default value is "mt-6". Spacing between text fields and other components is 24 pixels.
    /// </summary>
    public string? FluentFieldClass { get; set; } = "my-3";

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
