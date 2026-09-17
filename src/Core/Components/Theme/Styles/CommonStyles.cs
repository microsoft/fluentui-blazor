// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Contains common CSS styles used throughout the application.
/// </summary>
public static class CommonStyles
{
    /// <summary>
    /// Gets the CSS style for a thin border with a neutral color.
    /// </summary>
    public const string NeutralBorder1 = "border: var(--strokeWidthThin) solid var(--colorNeutralStroke1);";

    /// <summary>
    /// Gets the CSS style for a brand background (blue) with a inverted text color (white).
    /// </summary>
    public const string BrandBackground = "background-color: var(--colorBrandBackground); color: var(--colorNeutralForegroundOnBrand);";

    /// <summary>
    /// Gets the CSS style for a neutral background (white) with a neutral text color (black).
    /// </summary>
    public const string NeutralBackground = "background-color: var(--colorNeutralBackground1); color: var(--colorNeutralForeground1);";

    /// <summary>
    /// Gets the CSS style for a shadow border (4px) with a neutral color.
    /// </summary>
    public const string NeutralBorderShadow4 = "box-shadow: var(--shadow4);";

    /// <summary>
    /// Gets the CSS style for a shadow border (8px) with a neutral color.
    /// </summary>
    public const string NeutralBorderShadow8 = "box-shadow: var(--shadow8);";
}
