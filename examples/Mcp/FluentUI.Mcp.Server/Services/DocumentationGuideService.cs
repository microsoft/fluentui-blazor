// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text;
using System.Text.RegularExpressions;

namespace FluentUI.Mcp.Server.Services;

/// <summary>
/// Service for reading and processing documentation guide files (Markdown).
/// </summary>
public partial class DocumentationGuideService
{
    private readonly string _documentationBasePath;
    private readonly Dictionary<string, DocumentationGuide> _guides;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentationGuideService"/> class.
    /// </summary>
    public DocumentationGuideService()
    {
        _documentationBasePath = FindDocumentationPath();
        _guides = LoadGuides();
    }

    /// <summary>
    /// Gets all available documentation guides.
    /// </summary>
    public IReadOnlyList<DocumentationGuide> GetAllGuides()
    {
        return _guides.Values.OrderBy(g => g.Order).ToList();
    }

    /// <summary>
    /// Gets a specific guide by its key.
    /// </summary>
    public DocumentationGuide? GetGuide(string key)
    {
        return _guides.TryGetValue(key.ToLowerInvariant(), out var guide) ? guide : null;
    }

    /// <summary>
    /// Gets the content of a guide, resolving any includes.
    /// </summary>
    public string? GetGuideContent(string key)
    {
        var guide = GetGuide(key);
        if (guide == null)
        {
            return null;
        }

        return ResolveIncludes(guide.RawContent);
    }

    /// <summary>
    /// Gets the migration guide with all component-specific sections.
    /// </summary>
    public string GetFullMigrationGuide()
    {
        var guide = GetGuide("migration");
        if (guide == null)
        {
            return "Migration guide not found.";
        }

        return ResolveIncludes(guide.RawContent);
    }

    private static string FindDocumentationPath()
    {
        // Try to find documentation path relative to execution
        var possiblePaths = new[]
        {
            // When running from source
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Demo", "FluentUI.Demo.Client", "Documentation", "GetStarted"),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "Demo", "FluentUI.Demo.Client", "Documentation", "GetStarted"),
            // When running as tool (embedded or packaged)
            Path.Combine(AppContext.BaseDirectory, "Documentation", "GetStarted"),
            // Development paths
            @"D:\gh\fluentui-blazor\examples\Demo\FluentUI.Demo.Client\Documentation\GetStarted",
        };

        foreach (var path in possiblePaths)
        {
            var fullPath = Path.GetFullPath(path);
            if (Directory.Exists(fullPath))
            {
                return fullPath;
            }
        }

        return string.Empty;
    }

    private Dictionary<string, DocumentationGuide> LoadGuides()
    {
        var guides = new Dictionary<string, DocumentationGuide>(StringComparer.OrdinalIgnoreCase);

        if (string.IsNullOrEmpty(_documentationBasePath) || !Directory.Exists(_documentationBasePath))
        {
            // Return empty guides with informative message
            return guides;
        }

        var guideFiles = new Dictionary<string, string>
        {
            ["installation"] = "Installation.md",
            ["defaultvalues"] = "DefaultValues.md",
            ["whatsnew"] = "ReleaseNotes.md",
            ["migration"] = "MigrationVersion5.md",
            ["localization"] = "Localization.md",
            ["styles"] = "Styles.md",
        };

        foreach (var (key, fileName) in guideFiles)
        {
            var filePath = Path.Combine(_documentationBasePath, fileName);
            if (File.Exists(filePath))
            {
                var content = File.ReadAllText(filePath);
                var metadata = ParseFrontMatter(content);
                guides[key] = new DocumentationGuide
                {
                    Key = key,
                    Title = metadata.Title,
                    Order = metadata.Order,
                    Route = metadata.Route,
                    RawContent = content,
                    FilePath = filePath
                };
            }
        }

        return guides;
    }

    private string ResolveIncludes(string content)
    {
        // Pattern: {{ INCLUDE FileName }}
        var includePattern = IncludeRegex();
        var migrationPath = Path.Combine(_documentationBasePath, "Migration");

        return includePattern.Replace(content, match =>
        {
            var includeName = match.Groups[1].Value.Trim();
            var includeFile = Path.Combine(migrationPath, $"{includeName}.md");

            if (File.Exists(includeFile))
            {
                var includeContent = File.ReadAllText(includeFile);
                // Remove front matter from included content
                return RemoveFrontMatter(includeContent);
            }

            return $"<!-- Include not found: {includeName} -->";
        });
    }

    private static string RemoveFrontMatter(string content)
    {
        if (content.StartsWith("---"))
        {
            var endIndex = content.IndexOf("---", 3);
            if (endIndex > 0)
            {
                return content[(endIndex + 3)..].TrimStart();
            }
        }

        return content;
    }

    private static (string Title, int Order, string Route) ParseFrontMatter(string content)
    {
        var title = "Unknown";
        var order = 0;
        var route = "";

        if (content.StartsWith("---"))
        {
            var endIndex = content.IndexOf("---", 3);
            if (endIndex > 0)
            {
                var frontMatter = content[3..endIndex];
                var lines = frontMatter.Split('\n');

                foreach (var line in lines)
                {
                    var parts = line.Split(':', 2);
                    if (parts.Length == 2)
                    {
                        var key = parts[0].Trim().ToLowerInvariant();
                        var value = parts[1].Trim();

                        switch (key)
                        {
                            case "title":
                                title = value;
                                break;
                            case "order":
                                int.TryParse(value, out order);
                                break;
                            case "route":
                                route = value;
                                break;
                        }
                    }
                }
            }
        }

        return (title, order, route);
    }

    [GeneratedRegex(@"\{\{\s*INCLUDE\s+(\w+)\s*\}\}", RegexOptions.IgnoreCase)]
    private static partial Regex IncludeRegex();
}

/// <summary>
/// Represents a documentation guide.
/// </summary>
public record DocumentationGuide
{
    /// <summary>Gets the unique key for this guide.</summary>
    public required string Key { get; init; }

    /// <summary>Gets the title of the guide.</summary>
    public required string Title { get; init; }

    /// <summary>Gets the sort order.</summary>
    public int Order { get; init; }

    /// <summary>Gets the route path.</summary>
    public string Route { get; init; } = "";

    /// <summary>Gets the raw Markdown content.</summary>
    public required string RawContent { get; init; }

    /// <summary>Gets the file path.</summary>
    public string FilePath { get; init; } = "";
}
