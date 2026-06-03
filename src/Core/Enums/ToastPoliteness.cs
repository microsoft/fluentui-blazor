// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Controls the politeness setting of the toast live region.
/// </summary>
public enum ToastPoliteness
{
    /// <summary />
    [Description("polite")]
    Polite,

    /// <summary />
    [Description("assertive")]
    Assertive,
}
