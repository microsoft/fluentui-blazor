// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Reflection;
using System.Text.RegularExpressions;
using FluentUI.Demo.DocViewer.Models;
using Markdig;

namespace FluentUI.Demo.DocViewer.Services;

/// <summary>
/// Service to manage the markdown pages.
/// </summary>
public class DocViewerService
{
    private IEnumerable<Page>? _pages;

    /// <summary>
    /// Gets the markdown pipeline used to render the markdown pages.
    /// </summary>
    internal static readonly MarkdownPipeline MarkdownPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

    /// <summary>
    /// Initializes a new instance of the <see cref="DocViewerService"/> class.
    /// </summary>
    /// <param name="options"></param>
    public DocViewerService(DocViewerOptions options)
    {
        Options = options;
        ComponentsAssembly = options.ComponentsAssembly;
        ResourcesAssembly = options.ResourcesAssembly;
        ApiAssembly = options.ApiAssembly;
        ApiCommentSummary = options.ApiCommentSummary;
    }

    /// <summary>
    /// Gets the options used to configure the <see cref="DocViewerService"/>.
    /// </summary>
    public DocViewerOptions Options { get; }

    /// <summary>
    /// Gets the assembly containing the razor components to display in markdown pages.
    /// </summary>
    public Assembly? ComponentsAssembly { get; }

    /// <summary>
    /// Gets the assembly containing the embedded markdown pages.
    /// </summary>
    public Assembly? ResourcesAssembly { get; }

    /// <summary>
    /// Gets the assembly containing the classes to display in API sections.
    /// </summary>
    public Assembly? ApiAssembly { get; }

    /// <summary>
    /// Function to get the summary of an API comment.
    /// </summary>
    public Func<Type, MemberInfo?, string> ApiCommentSummary { get; }

    /// <summary>
    /// Gets the list of all markdown pages found in the resources
    /// </summary>
    public IEnumerable<Page> Pages => _pages ??= LoadAllPages();

    /// <summary>
    /// Returns the <see cref="Page" /> associated to the <paramref name="routeName"/>.
    /// Or null if not found.
    /// </summary>
    /// <param name="routeName"></param>
    /// <returns></returns>
    public Page? FromRoute(string routeName)
    {
        return Pages.FirstOrDefault(i => string.Compare(i.Route, routeName, StringComparison.InvariantCultureIgnoreCase) == 0 ||
                                         string.Compare(i.Route, $"/{routeName}", StringComparison.InvariantCultureIgnoreCase) == 0);
    }

    /// <summary>
    /// Loads the resource with the specified name.
    /// </summary>
    /// <param name="resourceName"></param>
    /// <param name="isFullName"></param>
    /// <param name="removeHeaders"></param>
    /// <returns></returns>
    public string LoadResource(string resourceName, bool isFullName = true, bool removeHeaders = false)
    {
        if (ResourcesAssembly is null)
        {
            return string.Empty;
        }

        var name = resourceName;

        // For short name, take the first resource ending with the ".name"
        if (!isFullName)
        {
            name = ResourcesAssembly.GetManifestResourceNames().FirstOrDefault(i => i.EndsWith("." + resourceName, StringComparison.InvariantCultureIgnoreCase));
        }

        // Read the resource content
        if (!string.IsNullOrEmpty(name))
        {
            using var stream = ResourcesAssembly.GetManifestResourceStream(name);
            if (stream != null)
            {
                using var reader = new StreamReader(stream);
                var content = reader.ReadToEnd();

                // Remove the headers: --- xxx ---
                if (removeHeaders)
                {
                    var patternHeaders = @"---[\s\S]*?---";
                    content = Regex.Replace(content, patternHeaders, string.Empty);
                }

                return content;
            }
        }

        return string.Empty;
    }

    /// <summary />
    private IEnumerable<Page> LoadAllPages()
    {
        if (ResourcesAssembly is null)
        {
            return [];
        }

        var resourceNames = ResourcesAssembly.GetManifestResourceNames();
        var pages = new List<Page>();

        foreach (var resourceName in resourceNames)
        {
            if (resourceName.EndsWith(".md"))
            {
                var content = LoadResource(resourceName);
                pages.Add(new Page(this, resourceName, content));
            }
        }

        return pages.Where(i => !string.IsNullOrEmpty(i.Route)).OrderBy(i => i.Route);
    }
}
