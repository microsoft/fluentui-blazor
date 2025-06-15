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
    public static Appearance? Appearance => Components.Appearance.Outline;

    /// <summary>
    /// Default disabled state for FluentButton components.
    /// </summary>
    [FluentDefault("FluentButton")]
    public static bool Disabled => false;

    /// <summary>
    /// Default class for FluentButton components.
    /// Uses descriptive property name with ParameterName to map to the "Class" parameter.
    /// </summary>
    [FluentDefault("FluentButton", ParameterName = "Class")]
    public static string? ButtonClass => "default-button-style";

    /// <summary>
    /// Default class for FluentTextInput components.
    /// Demonstrates how multiple properties can map to the same parameter name for different components.
    /// </summary>
    [FluentDefault("FluentTextInput", ParameterName = "Class")]
    public static string? TextInputClass => "default-input-style";

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