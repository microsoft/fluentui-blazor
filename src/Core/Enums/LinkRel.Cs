// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License..
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Relationship of the linked document to the current document.
/// </summary>
public enum LinkRel
{
    /// <summary>
    /// Alternate representations of the current document.
    /// </summary>
    [Description("alternate")]
    Alternate,

    /// <summary>
    /// Author of the current document or article.
    /// </summary>
    [Description("author")]
    Author,

    /// <summary>
    /// Preferred URL for the current document.
    /// </summary>
    [Description("canonical")]
    Canonical,

    /// <summary>
    /// Allows the page to be render-blocked until the essential parts of the document are parsed so it will render consistently.
    /// </summary>
    [Description("expect")]
    Expect,

    /// <summary>
    /// Link to context-sensitive help.
    /// </summary>
    [Description("help")]
    Help,

    /// <summary>
    /// Indicates that the main content of the current document is covered by the copyright license described by the referenced document.
    /// </summary>
    [Description("license")]
    License,

    /// <summary>
    /// Web app manifest.
    /// </summary>
    [Description("manifest")]
    Manifest,

    /// <summary>
    /// Indicates that the current document represents the person who owns the linked content.
    /// </summary>
    [Description("me")]
    Me,

    /// <summary>
    /// Indicates that the current document is a part of a series and that the next document in the series is the referenced document.
    /// </summary>
    [Description("next")]
    Next,

    /// <summary>
    /// Indicates that the current document is a part of a series and that the previous document in the series is the referenced document.
    /// </summary>
    [Description("prev")]
    Prev,

    /// <summary>
    /// Gives a link to a information about the data collection and usage practices that apply to the current document.
    /// </summary>
    [Description("privacy-policy")]
    PrivacyPolicy,

    /// <summary>
    /// Gives a link to a resource that can be used to search through the current document and its related pages.
    /// </summary>
    [Description("search")]
    Search,

    /// <summary>
    /// Link to the agreement, or terms of service, between the document's provider and users who wish to use the document.
    /// </summary>
    [Description("terms-of-service")]
    TermsOfService,
}
