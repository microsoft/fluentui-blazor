// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Indicates the tag to render in the <see cref="FluentText"/>.
/// </summary>
public enum TextTag
{
    /// <summary>
    /// Span tag
    /// </summary>
    [Description("span")]
    Span,

    /// <summary>
    /// Paragraph tag
    /// </summary>
    [Description("p")]
    Paragraph,

    /// <summary>
    /// Pre tag
    /// </summary>
    [Description("pre")]
    Pre,

    /// <summary>
    /// H1 tag
    /// </summary>
    [Description("h1")]
    H1,

    /// <summary>
    /// H2 tag
    /// </summary>
    [Description("h2")]
    H2,

    /// <summary>
    /// H3 tag
    /// </summary>
    [Description("h3")]
    H3,

    /// <summary>
    /// H4 tag
    /// </summary>
    [Description("h4")]
    H4,

    /// <summary>
    /// H5 tag
    /// </summary>
    [Description("h5")]
    H5,

    /// <summary>
    /// H6 tag
    /// </summary>
    [Description("h6")]
    H6,
}
