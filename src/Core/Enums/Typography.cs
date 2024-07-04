using System.ComponentModel.DataAnnotations;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The typography marker
/// </summary>
public enum Typography
{
    [Display(Name = "body")]
    Body,

    [Display(Name = "subject")]
    Subject,

    [Display(Name = "header")]
    Header,

    [Display(Name = "pane-header")]
    PaneHeader,

    [Display(Name = "email-header")]
    EmailHeader,

    [Display(Name = "page-title")]
    PageTitle,

    [Display(Name = "hero-title")]
    HeroTitle,

    [Display(Name = "h1")]
    H1,

    [Display(Name = "h2")]
    H2,

    [Display(Name = "h3")]
    H3,

    [Display(Name = "h4")]
    H4,

    [Display(Name = "h5")]
    H5,

    [Display(Name = "h6")]
    H6,
}
