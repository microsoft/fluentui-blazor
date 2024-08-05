// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Reflection;

namespace FluentUI.Demo.DocViewer.Services;

/// <summary>
/// DocViewer service options.
/// </summary>
public class DocViewerOptions
{
    /// <summary>
    /// Title of the page. The {0} will be replace by the page title defined in the markdown header.
    /// </summary>
    public string PageTitle { get; set; } = "{0}";

    /// <summary>
    /// Assembly containing the razor components to display in markdown pages.
    /// </summary>
    public Assembly? ComponentsAssembly { get; set; }

    /// <summary>
    /// Assembly containing the embedded markdown pages.
    /// </summary>
    public Assembly? ResourcesAssembly { get; set; }

    /// <summary>
    /// Path to the external source code files, where {0} will be replaced by the component name.
    /// </summary>
    public string SourceCodeUrl { get; set; } = "/sources/{0}.razor.txt";
}
