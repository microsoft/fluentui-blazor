// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the validation state of the progress bar.
/// </summary>
public enum ProgressState
{
    /// <summary>
    /// Display a red bar.
    /// </summary>
    [Description("error")]
    Error,

    /// <summary>
    /// Display a green bar.
    /// </summary>
    [Description("success")]
    Success,

    /// <summary>
    /// Display an orange bar.
    /// </summary>
    [Description("warning")]
    Warning,
}
