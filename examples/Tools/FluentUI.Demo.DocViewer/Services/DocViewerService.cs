// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
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
        ApiAssemblies = options.ApiAssemblies;
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
    /// Gets the assemblies containing the classes to display in API sections.
    /// </summary>
    public IReadOnlyList<Assembly> ApiAssemblies { get; }

    /// <summary>
    /// Gets the primary assembly containing the classes to display in API sections.
    /// </summary>
    [Obsolete("Use ApiAssemblies instead.")]
    public Assembly? ApiAssembly => ApiAssemblies.Count > 0 ? ApiAssemblies[0] : null;

    /// <summary>
    /// Function to get the summary of an API comment.
    /// </summary>
    public Func<ApiDocSummary?, Type, MemberInfo?, string> ApiCommentSummary { get; }

    /// <summary>
    /// Gets the list of all markdown pages found in the resources
    /// </summary>
    public IEnumerable<Page> Pages => _pages ??= LoadAllPages();

    /// <summary>
    /// Returns the <see cref="Type"/> associated to the <paramref name="fullName"/>.
    /// </summary>
    /// <param name="fullName">The full name of the type.</param>
    /// <returns>The <see cref="Type"/> if found; otherwise, <c>null</c>.</returns>
    public Type? FindApiType(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            return null;
        }

        foreach (var assembly in ApiAssemblies)
        {
            var type = assembly.GetType(fullName, throwOnError: false, ignoreCase: false);
            if (type is not null)
            {
                return type;
            }
        }

        return ApiAssemblies
            .SelectMany(GetTypes)
            .FirstOrDefault(type => string.Equals(type.FullName, fullName, StringComparison.Ordinal)
                                 || string.Equals(type.Name, fullName, StringComparison.Ordinal));
    }

    /// <summary>
    /// Returns the <see cref="Page" /> associated to the <paramref name="routeName"/>.
    /// Or null if not found.
    /// </summary>
    /// <param name="routeName"></param>
    /// <returns></returns>
    public Page? FromRoute(string routeName)
    {
        var uri = new Uri("http://dummy.com/" + routeName);
        var path = uri.AbsolutePath.TrimStart('/');

        return Pages.FirstOrDefault(i => string.Compare(i.Route, path, StringComparison.InvariantCultureIgnoreCase) == 0 ||
                                         string.Compare(i.Route, $"/{path}", StringComparison.InvariantCultureIgnoreCase) == 0);
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

    /// <summary />
    private static IEnumerable<Type> GetTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types.OfType<Type>();
        }
    }
}
