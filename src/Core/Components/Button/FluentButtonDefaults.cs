// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Example static defaults class for FluentButton component.
/// Property names MUST match the FluentButton parameter names exactly.
/// This ensures externalized defaults work as intended.
/// </summary>
[FluentDefault("FluentButton")]
public static class FluentButtonDefaults
{
    /// <summary>
    /// Default appearance for FluentButton.
    /// Property name "Appearance" matches the FluentButton.Appearance parameter exactly.
    /// </summary>
    public static Appearance Appearance { get; set; } = AspNetCore.Components.Appearance.Neutral;

    /// <summary>
    /// Default disabled state for FluentButton.
    /// Property name "Disabled" matches the FluentButton.Disabled parameter exactly.
    /// </summary>
    public static bool Disabled { get; set; } = false;

    /// <summary>
    /// Default loading state for FluentButton.
    /// Property name "Loading" matches the FluentButton.Loading parameter exactly.
    /// </summary>
    public static bool Loading { get; set; } = false;

    // Note: This would be INCORRECT:
    // public static Appearance DefaultButtonAppearance { get; set; }
    // Because it doesn't match the parameter name "Appearance" exactly
}