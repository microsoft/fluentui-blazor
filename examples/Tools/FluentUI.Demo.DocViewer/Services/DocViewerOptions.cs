// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
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
    /// Assemblies containing the API classes to display in API sections.
    /// </summary>
    public Assembly[]? ApiAssemblies { get; set; }

    /// <summary>
    /// Function to get the summary of an API comment.
    /// </summary>
    public Func<Models.ApiDocSummary?, Type, MemberInfo?, string> ApiCommentSummary { get; set; } = (data, type, member) => member?.Name ?? type.Name;

    /// <summary>
    /// Path to the external source code files, where {0} will be replaced by the razor component name 
    /// </summary>
    public string SourceCodeUrl { get; set; } = "/sources/{0}.txt";

    /// <summary>
    /// Gets or sets whether to use the console log provider.
    /// </summary>
    public bool EnableConsoleLogProvider { get; set; } = true;

    /// <summary>
    /// URL to the JSON index file that lists all API documentation JSON files to load (e.g. api-index.json).
    /// At build time a <c>api-index.json</c> is generated automatically from all <c>api-*.json</c> files
    /// in <c>wwwroot</c>, so adding a new API JSON file there requires no code changes.
    /// </summary>
    public string ApiDocIndexUrl { get; set; } = "/api-index.json";
}
