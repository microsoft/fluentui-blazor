// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Services;

/// <summary>
/// Service for providing component-level documentation (usage guides and Razor examples)
/// from embedded markdown and Razor files originating from the Demo.Client project.
/// </summary>
public sealed partial class ComponentDocumentationService
{
    /// <summary>
    /// Represents a resolved component documentation entry.
    /// </summary>
    /// <param name="ComponentName">The component name (e.g. "FluentButton").</param>
    /// <param name="Title">The title from YAML front matter.</param>
    /// <param name="RawMarkdown">The original markdown content.</param>
    /// <param name="ResolvedMarkdown">The markdown with {{ }} directives resolved.</param>
    public record ComponentDocumentation(
        string ComponentName,
        string Title,
        string RawMarkdown,
        string ResolvedMarkdown);

    // Matches {{ ExampleName }} or {{ ExampleName Files=Code:file1.razor;Label:file2.razor }}
    private const string DirectivePattern = @"\{\{\s*(?<name>\w+)(?:\s+Files=(?<files>[^\}]+))?\s*\}\}";

    // Matches {{ API Type=TypeName ... }}
    private const string ApiDirectivePattern = @"\{\{\s*API\s+[^\}]+\}\}";

    /// <summary>
    /// Key = component name (e.g. "FluentButton", "FluentDataGrid"), case-insensitive.
    /// Value = list of markdown files for that component (a component may have multiple docs).
    /// </summary>
    private readonly Dictionary<string, List<ComponentDocumentation>> _componentDocs = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Key = example name without extension (e.g. "ButtonDefault"), case-insensitive.
    /// Value = the Razor source code.
    /// </summary>
    private readonly Dictionary<string, string> _examples = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Key = example name without extension (e.g. "BadgeAttached"), case-insensitive.
    /// Value = the code-behind C# source code.
    /// </summary>
    private readonly Dictionary<string, string> _codeBehind = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentDocumentationService"/> class.
    /// Loads all embedded component documentation and example resources.
    /// </summary>
    public ComponentDocumentationService()
    {
        LoadEmbeddedResources();
    }

    /// <summary>
    /// Gets the resolved documentation for a component by name.
    /// Returns concatenated documentation from all matching markdown files with examples inlined.
    /// </summary>
    /// <param name="componentName">
    /// The component name (e.g. "FluentButton", "Button", "DataGrid").
    /// The "Fluent" prefix is optional.
    /// </param>
    /// <returns>The resolved markdown documentation, or null if not found.</returns>
    public string? GetComponentDocumentation(string componentName)
    {
        var docs = FindDocumentation(componentName);
        if (docs == null || docs.Count == 0)
        {
            return null;
        }

        // If there is only one doc, return it directly
        if (docs.Count == 1)
        {
            return docs[0].ResolvedMarkdown;
        }

        // Concatenate multiple docs (e.g. FluentButton has a category summary + individual page)
        var sb = new StringBuilder();
        foreach (var doc in docs)
        {
            sb.AppendLine(doc.ResolvedMarkdown);
            sb.AppendLine();
        }

        return sb.ToString().TrimEnd();
    }

    /// <summary>
    /// Searches component documentation for a given term.
    /// Returns matching component names.
    /// </summary>
    /// <param name="searchTerm">The term to search for.</param>
    /// <returns>A list of component names whose documentation matches the search term.</returns>
    public IReadOnlyList<string> SearchDocumentation(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return [];
        }

        var results = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var (componentName, docs) in _componentDocs)
        {
            foreach (var doc in docs)
            {
                if (doc.RawMarkdown.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    doc.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(componentName);
                    break;
                }
            }
        }

        return [.. results.OrderBy(n => n, StringComparer.OrdinalIgnoreCase)];
    }

    /// <summary>
    /// Gets all component names that have documentation available.
    /// </summary>
    /// <returns>A sorted list of component names.</returns>
    public IReadOnlyList<string> GetAvailableComponents()
    {
        return [.. _componentDocs.Keys.OrderBy(k => k, StringComparer.OrdinalIgnoreCase)];
    }

    /// <summary>
    /// Finds documentation entries for a component name, trying multiple name variants.
    /// </summary>
    private List<ComponentDocumentation>? FindDocumentation(string componentName)
    {
        // Try exact match first
        if (_componentDocs.TryGetValue(componentName, out var docs))
        {
            return docs;
        }

        // Try with "Fluent" prefix
        if (_componentDocs.TryGetValue($"Fluent{componentName}", out docs))
        {
            return docs;
        }

        // Try without "Fluent" prefix
        if (componentName.StartsWith("Fluent", StringComparison.OrdinalIgnoreCase))
        {
            var withoutPrefix = componentName["Fluent".Length..];
            if (_componentDocs.TryGetValue(withoutPrefix, out docs))
            {
                return docs;
            }
        }

        // Try partial match (e.g. "DataGrid" matching "FluentDataGrid")
        var match = _componentDocs.Keys
            .FirstOrDefault(k => k.EndsWith(componentName, StringComparison.OrdinalIgnoreCase));

        if (match != null)
        {
            return _componentDocs[match];
        }

        return null;
    }

    /// <summary>
    /// Loads all embedded resources matching component documentation patterns.
    /// </summary>
    private void LoadEmbeddedResources()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceNames = assembly.GetManifestResourceNames();

        // Load examples first (both .razor and .razor.cs) so they are available for directive resolution
        LoadExamples(assembly, resourceNames);

        // Then load markdown documentation
        LoadMarkdownDocs(assembly, resourceNames);

        Console.Error.WriteLine($"{McpServerConstants.LogPrefix} Loaded component documentation for {_componentDocs.Count} components with {_examples.Count} examples.");
    }

    /// <summary>
    /// Loads all Razor example files and code-behind files from embedded resources.
    /// </summary>
    private void LoadExamples(Assembly assembly, string[] resourceNames)
    {
        foreach (var resourceName in resourceNames)
        {
            // Match .razor.cs code-behind files (check before .razor to avoid substring confusion)
            if (resourceName.Contains(".Examples.", StringComparison.OrdinalIgnoreCase) &&
                resourceName.EndsWith(".razor.cs", StringComparison.OrdinalIgnoreCase))
            {
                var content = ReadResource(assembly, resourceName);
                if (content != null)
                {
                    var exampleName = ExtractExampleName(resourceName, isCodeBehind: true);
                    if (!string.IsNullOrEmpty(exampleName))
                    {
                        _codeBehind[exampleName] = content;
                    }
                }
            }
            // Match .razor example files (but not .razor.cs or .razor.css)
            else if (resourceName.Contains(".Examples.", StringComparison.OrdinalIgnoreCase) &&
                     resourceName.EndsWith(".razor", StringComparison.OrdinalIgnoreCase))
            {
                var content = ReadResource(assembly, resourceName);
                if (content != null)
                {
                    var exampleName = ExtractExampleName(resourceName, isCodeBehind: false);
                    if (!string.IsNullOrEmpty(exampleName))
                    {
                        _examples[exampleName] = content;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Loads all markdown documentation files from embedded resources.
    /// </summary>
    private void LoadMarkdownDocs(Assembly assembly, string[] resourceNames)
    {
        foreach (var resourceName in resourceNames)
        {
            if (!resourceName.Contains(".Components.", StringComparison.OrdinalIgnoreCase) ||
                !resourceName.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var content = ReadResource(assembly, resourceName);
            if (string.IsNullOrEmpty(content))
            {
                continue;
            }

            var componentName = ExtractComponentNameFromResource(resourceName);
            if (string.IsNullOrEmpty(componentName))
            {
                continue;
            }

            // Parse the title from YAML front matter
            var title = ExtractTitle(content);

            // Remove YAML front matter from the content
            var cleanContent = RemoveFrontMatter(content);

            // Resolve {{ }} directives
            var resolvedContent = ResolveDirectives(cleanContent);

            var doc = new ComponentDocumentation(
                ComponentName: componentName,
                Title: title ?? componentName,
                RawMarkdown: cleanContent,
                ResolvedMarkdown: resolvedContent);

            if (!_componentDocs.TryGetValue(componentName, out var docList))
            {
                docList = [];
                _componentDocs[componentName] = docList;
            }

            docList.Add(doc);
        }
    }

    /// <summary>
    /// Resolves {{ }} directives in markdown content.
    /// - {{ ExampleName }} → replaces with the example Razor source code in a fenced code block.
    /// - {{ ExampleName Files=Code:file1.razor;Label:file2.razor }} → replaces with multiple code blocks.
    /// - {{ API Type=... }} → stripped.
    /// </summary>
    internal string ResolveDirectives(string content)
    {
        // First, strip {{ API ... }} directives
        var result = ApiDirectiveRegex().Replace(content, string.Empty);

        // Then, resolve {{ ExampleName }} and {{ ExampleName Files=... }} directives
        result = ExampleDirectiveRegex().Replace(result, match =>
        {
            var exampleName = match.Groups["name"].Value;
            var filesSpec = match.Groups["files"].Success ? match.Groups["files"].Value : null;

            if (!string.IsNullOrEmpty(filesSpec))
            {
                return ResolveFilesDirective(exampleName, filesSpec);
            }

            return ResolveSingleExample(exampleName);
        });

        // Clean up excessive blank lines (more than 2 consecutive)
        result = ExcessiveNewlinesRegex().Replace(result, "\n\n");

        return result.Trim();
    }

    /// <summary>
    /// Resolves a single {{ ExampleName }} directive.
    /// </summary>
    private string ResolveSingleExample(string exampleName)
    {
        var sb = new StringBuilder();

        if (_examples.TryGetValue(exampleName, out var razorContent))
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"### Example: {exampleName}");
            sb.AppendLine();
            sb.AppendLine("```razor");
            sb.AppendLine(razorContent.TrimEnd());
            sb.AppendLine("```");

            // Also include code-behind if it exists
            if (_codeBehind.TryGetValue(exampleName, out var codeBehindContent))
            {
                sb.AppendLine();
                sb.AppendLine(CultureInfo.InvariantCulture, $"**Code-behind ({exampleName}.razor.cs):**");
                sb.AppendLine();
                sb.AppendLine("```csharp");
                sb.AppendLine(codeBehindContent.TrimEnd());
                sb.AppendLine("```");
            }
        }
        else
        {
            // Example not found - leave a note
            sb.AppendLine(CultureInfo.InvariantCulture, $"<!-- Example '{exampleName}' not found -->");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Resolves a {{ ExampleName Files=Code:file1.razor;Label:file2.razor }} directive.
    /// </summary>
    private string ResolveFilesDirective(string mainExampleName, string filesSpec)
    {
        var sb = new StringBuilder();

        // Parse the files spec: "Code:file1.razor;Label:file2.razor"
        var fileEntries = filesSpec.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var entry in fileEntries)
        {
            var parts = entry.Split(':', 2, StringSplitOptions.TrimEntries);
            if (parts.Length != 2)
            {
                continue;
            }

            var label = parts[0];
            var fileName = parts[1];

            // Derive the example name from the filename (remove extension)
            var exName = Path.GetFileNameWithoutExtension(fileName);
            if (exName.EndsWith(".razor", StringComparison.OrdinalIgnoreCase))
            {
                // Handle .razor.cs files where GetFileNameWithoutExtension leaves .razor
                exName = Path.GetFileNameWithoutExtension(exName);
            }

            // Determine language from extension
            var language = fileName.EndsWith(".cs", StringComparison.OrdinalIgnoreCase) ? "csharp" : "razor";

            if (_examples.TryGetValue(exName, out var content) || _codeBehind.TryGetValue(exName, out content))
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"**{label} ({fileName}):**");
                sb.AppendLine();
                sb.AppendLine(CultureInfo.InvariantCulture, $"```{language}");
                sb.AppendLine(content.TrimEnd());
                sb.AppendLine("```");
                sb.AppendLine();
            }
            else
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"<!-- File '{fileName}' ({label}) not found -->");
            }
        }

        if (sb.Length == 0)
        {
            // Fall back to resolving as a single example
            return ResolveSingleExample(mainExampleName);
        }

        return sb.ToString();
    }

    /// <summary>
    /// Extracts the component name from an embedded resource name.
    /// Resource names follow the pattern: *.Components.{Category}.{SubFolder}.{FileName}.md
    /// We extract the FileName (without .md) as the component name (e.g., "FluentButton").
    /// For overview files, we use the file name as-is.
    /// </summary>
    private static string? ExtractComponentNameFromResource(string resourceName)
    {
        // Resource name format: Microsoft.FluentUI.AspNetCore.McpServer.Documentation.Components.{path}.{FileName}.md
        // We want the FileName part (the part before .md)
        var parts = resourceName.Split('.');
        if (parts.Length < 3)
        {
            return null;
        }

        // The last part is "md", the second-to-last is the filename
        var fileName = parts[^2];

        // Skip overview/default files that are category summaries
        if (fileName.EndsWith("Default", StringComparison.OrdinalIgnoreCase) ||
            fileName.EndsWith("Overview", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return fileName;
    }

    /// <summary>
    /// Extracts the example name from an embedded resource name.
    /// Example resource: *.Examples.ButtonDefault.razor → "ButtonDefault"
    /// Code-behind: *.Examples.BadgeAttached.razor.cs → "BadgeAttached"
    /// </summary>
    private static string? ExtractExampleName(string resourceName, bool isCodeBehind)
    {
        var parts = resourceName.Split('.');
        if (parts.Length < 3)
        {
            return null;
        }

        if (isCodeBehind)
        {
            // Format: *.ExampleName.razor.cs  → parts[^3] = "ExampleName", parts[^2] = "razor", parts[^1] = "cs"
            if (parts.Length >= 4)
            {
                return parts[^3];
            }
        }
        else
        {
            // Format: *.ExampleName.razor → parts[^2] = "ExampleName", parts[^1] = "razor"
            return parts[^2];
        }

        return null;
    }

    /// <summary>
    /// Extracts the title from YAML front matter.
    /// </summary>
    private static string? ExtractTitle(string content)
    {
        var match = FrontMatterRegex().Match(content);
        if (!match.Success)
        {
            return null;
        }

        var frontMatter = match.Groups["frontmatter"].Value;
        var titleMatch = YamlTitleRegex().Match(frontMatter);
        return titleMatch.Success ? titleMatch.Groups["title"].Value.Trim() : null;
    }

    /// <summary>
    /// Removes YAML front matter from the markdown content.
    /// </summary>
    private static string RemoveFrontMatter(string content)
    {
        var match = FrontMatterRegex().Match(content);
        if (!match.Success)
        {
            return content;
        }

        return content[(match.Index + match.Length)..].TrimStart('\r', '\n');
    }

    /// <summary>
    /// Reads an embedded resource as a string.
    /// </summary>
    private static string? ReadResource(Assembly assembly, string resourceName)
    {
        try
        {
            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                return null;
            }

            using var reader = new StreamReader(stream, Encoding.UTF8);
            return reader.ReadToEnd();
        }
        catch
        {
            return null;
        }
    }

    [GeneratedRegex(@"^---\s*\n(?<frontmatter>.*?)\n---", RegexOptions.Singleline | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex FrontMatterRegex();

    [GeneratedRegex(@"^title:\s*(?<title>.+)$", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex YamlTitleRegex();

    [GeneratedRegex(DirectivePattern, RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex ExampleDirectiveRegex();

    [GeneratedRegex(ApiDirectivePattern, RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex ApiDirectiveRegex();

    [GeneratedRegex(@"\n{3,}", RegexOptions.None, matchTimeoutMilliseconds: 1000)]
    private static partial Regex ExcessiveNewlinesRegex();
}
