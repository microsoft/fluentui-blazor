// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.FluentUI.AspNetCore.McpServer.Models;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Services;

/// <summary>
/// Service for providing GetStarted documentation content.
/// Reads markdown files embedded in the assembly.
/// </summary>
public partial class GetStartedDocumentationService
{
    private readonly Dictionary<string, GetStartedInfo> _documentationCache = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<string> _excludedFolders;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetStartedDocumentationService"/> class.
    /// </summary>
    /// <param name="excludedFolders">Optional list of folder names to exclude (e.g., "mcp").</param>
    public GetStartedDocumentationService(IEnumerable<string>? excludedFolders = null)
    {
        _excludedFolders = excludedFolders?.ToList() ?? ["mcp"];
        LoadDocumentation();
    }

    /// <summary>
    /// Gets all available GetStarted documentation pages.
    /// </summary>
    /// <returns>A list of all documentation pages ordered by their order value.</returns>
    public IReadOnlyList<GetStartedInfo> GetAllDocumentation()
    {
        return [.. _documentationCache.Values
            .Where(d => !d.Hidden)
            .OrderBy(d => d.Order),
        ];
    }

    /// <summary>
    /// Gets a specific documentation page by its title or route.
    /// </summary>
    /// <param name="identifier">The title or route of the documentation page.</param>
    /// <returns>The documentation page, or null if not found.</returns>
    public GetStartedInfo? GetDocumentation(string identifier)
    {
        // Try to find by title first
        if (_documentationCache.TryGetValue(identifier, out var doc))
        {
            return doc;
        }

        // Try to find by route
        return _documentationCache.Values.FirstOrDefault(d =>
            d.Route.Equals(identifier, StringComparison.OrdinalIgnoreCase) ||
            d.Route.Equals("/" + identifier, StringComparison.OrdinalIgnoreCase) ||
            d.FileName.Equals(identifier, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Searches for documentation pages by search term.
    /// </summary>
    /// <param name="searchTerm">The term to search for in titles and content.</param>
    /// <returns>A list of matching documentation pages.</returns>
    public IReadOnlyList<GetStartedInfo> SearchDocumentation(string searchTerm)
    {
        var lowerSearch = searchTerm.ToLowerInvariant();
        return [.. _documentationCache.Values
            .Where(d => !d.Hidden &&
                       (d.Title.Contains(lowerSearch, StringComparison.OrdinalIgnoreCase) ||
                        d.Content.Contains(lowerSearch, StringComparison.OrdinalIgnoreCase) ||
                        d.Summary.Contains(lowerSearch, StringComparison.OrdinalIgnoreCase)))
            .OrderBy(d => d.Order),
        ];
    }

    /// <summary>
    /// Gets all migration-related documentation pages.
    /// </summary>
    /// <returns>A list of migration documentation pages.</returns>
    public IReadOnlyList<GetStartedInfo> GetMigrationDocumentation()
    {
        return [.. _documentationCache.Values
            .Where(d => d.FileName.StartsWith("Migration", StringComparison.OrdinalIgnoreCase) ||
                       d.Title.Contains("Migration", StringComparison.OrdinalIgnoreCase) ||
                       d.Title.Contains("Migrating", StringComparison.OrdinalIgnoreCase))
            .OrderBy(d => d.Order),
        ];
    }

    /// <summary>
    /// Gets available documentation topics (non-hidden, non-migration main pages).
    /// </summary>
    /// <returns>A list of main documentation topics.</returns>
    public IReadOnlyList<string> GetTopics()
    {
        return [.. _documentationCache.Values
            .Where(d => !d.Hidden)
            .Select(d => d.Title)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Order(StringComparer.OrdinalIgnoreCase),
        ];
    }

    private void LoadDocumentation()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceNames = assembly.GetManifestResourceNames()
            .Where(name => name.Contains("GetStarted", StringComparison.OrdinalIgnoreCase) &&
                          name.EndsWith(".md", StringComparison.OrdinalIgnoreCase));

        foreach (var resourceName in resourceNames)
        {
            // Check if the resource is in an excluded folder
            if (IsExcluded(resourceName))
            {
                continue;
            }

            try
            {
                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null)
                {
                    continue;
                }

                using var reader = new StreamReader(stream, Encoding.UTF8);
                var content = reader.ReadToEnd();

                var docInfo = ParseMarkdownFile(content, resourceName);
                if (docInfo != null && !string.IsNullOrEmpty(docInfo.Title))
                {
                    _documentationCache[docInfo.Title] = docInfo;
                }
            }
            catch
            {
                // Skip files that cannot be read
            }
        }
    }

    private bool IsExcluded(string resourceName)
    {
        var lowerResourceName = resourceName.ToLowerInvariant();
        return _excludedFolders.Exists(folder =>
        {
            var lowerFolder = folder.ToLowerInvariant();
            return lowerResourceName.Contains($".{lowerFolder}.", StringComparison.Ordinal) ||
                   lowerResourceName.Contains($".{lowerFolder}_", StringComparison.Ordinal);
        });
    }

    private static GetStartedInfo? ParseMarkdownFile(string content, string resourceName)
    {
        var info = new GetStartedInfo
        {
            FileName = ExtractFileName(resourceName),
            Content = content,
        };

        // Parse YAML front matter
        var frontMatterMatch = FrontMatterRegex().Match(content);
        if (frontMatterMatch.Success)
        {
            var frontMatter = frontMatterMatch.Groups[1].Value;
            info.Title = ExtractYamlValue(frontMatter, "title");
            info.Route = ExtractYamlValue(frontMatter, "route");
            info.Icon = ExtractYamlValue(frontMatter, "icon");
            info.Category = ExtractYamlValue(frontMatter, "category");
            info.Hidden = ExtractYamlValue(frontMatter, "hidden").Equals("true", StringComparison.OrdinalIgnoreCase);

            if (int.TryParse(ExtractYamlValue(frontMatter, "order"), CultureInfo.InvariantCulture, out var order))
            {
                info.Order = order;
            }

            // Remove front matter from content for summary extraction
            var contentWithoutFrontMatter = content[(frontMatterMatch.Index + frontMatterMatch.Length)..].Trim();
            info.Summary = ExtractSummary(contentWithoutFrontMatter);
        }
        else
        {
            info.Summary = ExtractSummary(content);
        }

        return info;
    }

    private static string ExtractFileName(string resourceName)
    {
        var parts = resourceName.Split('.');
        if (parts.Length >= 2)
        {
            return parts[^2]; // Get the part before .md
        }

        return resourceName;
    }

    private static string ExtractYamlValue(string frontMatter, string key)
    {
        var regex = YamlKeyValueRegex(key);
        var match = regex.Match(frontMatter);
        return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
    }

    private static string ExtractSummary(string content)
    {
        // Remove headers and get first meaningful paragraph
        var lines = content.Split('\n')
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrEmpty(l) && !l.StartsWith('#'))
            .Take(3);

        var summary = string.Join(' ', lines);

        // Limit summary length
        if (summary.Length > 200)
        {
            summary = summary[..197] + "...";
        }

        return summary;
    }

    [GeneratedRegex(@"^---\s*\n(.*?)\n---", RegexOptions.Singleline | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex FrontMatterRegex();

    private static Regex YamlKeyValueRegex(string key) =>
        new($@"^{Regex.Escape(key)}:\s*(.+)$", RegexOptions.Multiline | RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1));
}
