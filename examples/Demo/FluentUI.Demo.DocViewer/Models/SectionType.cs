// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.DocViewer.Models;

/// <summary>
/// Types of sections in a markdown document.
/// </summary>
public enum SectionType
{
    /// <summary>
    /// HTML
    /// </summary>
    Html,

    /// <summary>
    /// Source code
    /// </summary>
    Code,

    /// <summary>
    /// Component to display
    /// </summary>
    Component,

    /// <summary>
    /// API array to display
    /// </summary>
    Api,
}

