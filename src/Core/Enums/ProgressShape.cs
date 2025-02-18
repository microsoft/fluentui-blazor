// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the shape of the progress bar.
/// </summary>
public enum ProgressShape
{
    /// <summary>
    /// Display progress bar with rounded corners.
    /// </summary>
    [Description("rounded")]
    Rounded,

    /// <summary>
    /// Display progress bar with square corners.
    /// </summary>
    [Description("square")]
    Square,
}
