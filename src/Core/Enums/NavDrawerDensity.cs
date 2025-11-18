// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The density of the <see cref="FluentNavDrawer" /> items.
/// </summary>
public enum NavDrawerDensity
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
