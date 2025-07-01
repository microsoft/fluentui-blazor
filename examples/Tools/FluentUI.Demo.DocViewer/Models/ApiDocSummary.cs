// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.DocViewer.Models;

/// <summary>
/// Represents the summary of the API documentation.
/// </summary>
public class ApiDocSummary
{
    /// <summary>
    /// Gets or sets the content of the API documentation, read from the api-comments.json file.
    /// </summary>
    public Dictionary<string, Dictionary<string, string>>? Items { get; set; }

    /// <summary>
    /// Gets or sets the cached API documentation.
    /// </summary>
    public static ApiDocSummary? Cached { get; set; }
}
