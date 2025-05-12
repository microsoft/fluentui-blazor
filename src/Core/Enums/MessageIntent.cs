// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Specifies the intent of a <see cref="FluentMessageBar"/>.
/// </summary>

public enum MessageIntent
{
    /// <summary>
    /// Info intent.
    /// </summary>
    [Description("info")]
    Info,

    /// <summary>
    /// Success intent.
    /// </summary>
    [Description("success")]
    Success,

    /// <summary>
    /// Warning intent.
    /// </summary>
    [Description("warning")]
    Warning,

    /// <summary>
    /// Error intent.
    /// </summary>
    [Description("error")]
    Error,

    /// <summary >
    /// Custom intent.
    /// </summary>
    [Description("custom")]
    Custom,
}
