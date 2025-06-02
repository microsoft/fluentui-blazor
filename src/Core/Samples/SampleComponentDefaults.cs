// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.Samples;

/// <summary>
/// Sample class demonstrating how to define externalized default values for Fluent UI components.
/// This class shows how to use the FluentDefault attribute to specify default parameter values
/// for components without modifying the component code itself.
/// </summary>
public static class SampleComponentDefaults
{
    /// <summary>
    /// Default appearance for FluentButton components.
    /// </summary>
    [FluentDefault("FluentButton")]
    public static Appearance? DefaultButtonAppearance => Appearance.Outline;

    /// <summary>
    /// Default disabled state for FluentButton components.
    /// </summary>
    [FluentDefault("FluentButton")]
    public static bool Disabled => false;

    /// <summary>
    /// Default class for FluentButton components.
    /// </summary>
    [FluentDefault("FluentButton")]
    public static string? Class => "default-button-style";

    /// <summary>
    /// Default direction for FluentDesignSystemProvider components.
    /// </summary>
    [FluentDefault("FluentDesignSystemProvider")]
    public static LocalizationDirection? Direction => LocalizationDirection.LeftToRight;

    /// <summary>
    /// Default density for FluentDesignSystemProvider components.
    /// </summary>
    [FluentDefault("FluentDesignSystemProvider")]
    public static int? Density => 0;
}