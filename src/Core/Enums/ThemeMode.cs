// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Defines available theme modes.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ThemeMode>))]
public enum ThemeMode
{
    /// <summary>
    /// Light theme mode.
    /// </summary>
    [Description("light")]
#if NET9_0_OR_GREATER
    [JsonStringEnumMemberName("light")]
#endif
    Light,

    /// <summary>
    /// Dark theme mode.
    /// </summary>
    [Description("dark")]
#if NET9_0_OR_GREATER
    [JsonStringEnumMemberName("dark")]
#endif
    Dark,

    /// <summary>
    /// Follows the system theme mode.
    /// </summary>
    [Description("system")]
#if NET9_0_OR_GREATER
    [JsonStringEnumMemberName("system")]
#endif
    System,
}
