// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The density of the <see cref="FluentNav" /> items.
/// </summary>
public enum NavDensity
{
    /// <summary>
    /// Medium density
    /// </summary>
    [Description("medium")]
    Medium,

    /// <summary>
    /// Small density
    /// </summary>
    [Description("small")]
    Small,
}
