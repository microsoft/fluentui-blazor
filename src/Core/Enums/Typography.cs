using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The typography marker
/// </summary>
public enum Typography
{
    [Description("body")]
    Body,

    [Description("subject")]
    Subject,

    [Description("header")]
    Header,

    [Description("pane-header")]
    PaneHeader,

    [Description("email-header")]
    EmailHeader,

    [Description("page-title")]
    PageTitle,

    [Description("hero-title")]
    HeroTitle,

    [Description("h1")]
    H1,

    [Description("h2")]
    H2,

    [Description("h3")]
    H3,

    [Description("h4")]
    H4,

    [Description("h5")]
    H5,

    [Description("h6")]
    H6,
}
