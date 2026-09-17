// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Specifies the overflow behavior for an item.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<OverflowBehavior>))]
public enum OverflowBehavior
{
    /// <summary>
    /// The item is kept fixed in place and not subject to overflow.
    /// </summary>
    [Description("fixed")]
    Fixed,

    /// <summary>
    /// The item can be hidden with an ellipsis indicator when space is constrained.
    /// </summary>
    [Description("ellipsis")]
    Ellipsis,
}
