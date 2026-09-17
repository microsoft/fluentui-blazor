// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Defines the direction of a pull motion.
/// </summary>
public enum PullDirection
{
    /// <summary>
    /// Pull down
    /// </summary>
    [Description("down")]
    Down,

    /// <summary>
    /// Pull up
    /// </summary>
    [Description("up")]
    Up
}
