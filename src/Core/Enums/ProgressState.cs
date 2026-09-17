// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
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
