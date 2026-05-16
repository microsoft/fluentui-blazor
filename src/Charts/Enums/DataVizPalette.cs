// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Contains the data visualization palette tokens that map to the underlying color values.
/// Serialize a value using <see cref="DataVizPaletteJsonConverter"/>, which writes the
/// token string (e.g. <c>"color5"</c>) recognized by the chart web components.
/// </summary>
[JsonConverter(typeof(DataVizPaletteJsonConverter))]
public enum DataVizPalette
{
    // Qualitative

    /// <summary>
    /// Color slot 1.
    /// </summary>
    [Description("color1")]
    Color1,

    /// <summary>
    /// Color slot 2.
    /// </summary>
    [Description("color2")]
    Color2,

    /// <summary>
    /// Color slot 3.
    /// </summary>
    [Description("color3")]
    Color3,

    /// <summary>
    /// Color slot 4.
    /// </summary>
    [Description("color4")]
    Color4,

    /// <summary>
    /// Color slot 5.
    /// </summary>
    [Description("color5")]
    Color5,

    /// <summary>
    /// Color slot 6.
    /// </summary>
    [Description("color6")]
    Color6,

    /// <summary>
    /// Color slot 7.
    /// </summary>
    [Description("color7")]
    Color7,

    /// <summary>
    /// Color slot 8.
    /// </summary>
    [Description("color8")]
    Color8,

    /// <summary>
    /// Color slot 9.
    /// </summary>
    [Description("color9")]
    Color9,

    /// <summary>
    /// Color slot 10.
    /// </summary>
    [Description("color10")]
    Color10,

    /// <summary>
    /// Color slot 11.
    /// </summary>
    [Description("color11")]
    Color11,

    /// <summary>
    /// Color slot 12.
    /// </summary>
    [Description("color12")]
    Color12,

    /// <summary>
    /// Color slot 13.
    /// </summary>
    [Description("color13")]
    Color13,

    /// <summary>
    /// Color slot 14.
    /// </summary>
    [Description("color14")]
    Color14,

    /// <summary>
    /// Color slot 15.
    /// </summary>
    [Description("color15")]
    Color15,

    /// <summary>
    /// Color slot 16.
    /// </summary>
    [Description("color16")]
    Color16,

    /// <summary>
    /// Color slot 17.
    /// </summary>
    [Description("color17")]
    Color17,

    /// <summary>
    /// Color slot 18.
    /// </summary>
    [Description("color18")]
    Color18,

    /// <summary>
    /// Color slot 19.
    /// </summary>
    [Description("color19")]
    Color19,

    /// <summary>
    /// Color slot 20.
    /// </summary>
    [Description("color20")]
    Color20,

    /// <summary>
    /// Color slot 21.
    /// </summary>
    [Description("color21")]
    Color21,

    /// <summary>
    /// Color slot 22.
    /// </summary>
    [Description("color22")]
    Color22,

    /// <summary>
    /// Color slot 23.
    /// </summary>
    [Description("color23")]
    Color23,

    /// <summary>
    /// Color slot 24.
    /// </summary>
    [Description("color24")]
    Color24,

    /// <summary>
    /// Color slot 25.
    /// </summary>
    [Description("color25")]
    Color25,

    /// <summary>
    /// Color slot 26.
    /// </summary>
    [Description("color26")]
    Color26,

    /// <summary>
    /// Color slot 27.
    /// </summary>
    [Description("color27")]
    Color27,

    /// <summary>
    /// Color slot 28.
    /// </summary>
    [Description("color28")]
    Color28,

    /// <summary>
    /// Color slot 29.
    /// </summary>
    [Description("color29")]
    Color29,

    /// <summary>
    /// Color slot 30.
    /// </summary>
    [Description("color30")]
    Color30,

    /// <summary>
    /// Color slot 31.
    /// </summary>
    [Description("color31")]
    Color31,

    /// <summary>
    /// Color slot 32.
    /// </summary>
    [Description("color32")]
    Color32,

    /// <summary>
    /// Color slot 33.
    /// </summary>
    [Description("color33")]
    Color33,

    /// <summary>
    /// Color slot 34.
    /// </summary>
    [Description("color34")]
    Color34,

    /// <summary>
    /// Color slot 35.
    /// </summary>
    [Description("color35")]
    Color35,

    /// <summary>
    /// Color slot 36.
    /// </summary>
    [Description("color36")]
    Color36,

    /// <summary>
    /// Color slot 37.
    /// </summary>
    [Description("color37")]
    Color37,

    /// <summary>
    /// Color slot 38.
    /// </summary>
    [Description("color38")]
    Color38,

    /// <summary>
    /// Color slot 39.
    /// </summary>
    [Description("color39")]
    Color39,

    /// <summary>
    /// Color slot 40.
    /// </summary>
    [Description("color40")]
    Color40,

    // Semantic

    /// <summary>
    /// Semantic color for informational states.
    /// </summary>
    [Description("info")]
    Info,

    /// <summary>
    /// Semantic color for disabled states.
    /// </summary>
    [Description("disabled")]
    Disabled,

    /// <summary>
    /// Semantic color for high-severity error states.
    /// </summary>
    [Description("highError")]
    HighError,

    /// <summary>
    /// Semantic color for error states.
    /// </summary>
    [Description("error")]
    Error,

    /// <summary>
    /// Semantic color for warning states.
    /// </summary>
    [Description("warning")]
    Warning,

    /// <summary>
    /// Semantic color for success states.
    /// </summary>
    [Description("success")]
    Success,

    /// <summary>
    /// Semantic color for high-severity success states.
    /// </summary>
    [Description("highSuccess")]
    HighSuccess,
}
