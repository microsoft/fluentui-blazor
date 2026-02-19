// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.FluentUI.AspNetCore.McpServer.Models;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Services;

/// <summary>
/// Service for providing migration-specific documentation content.
/// Reads migration markdown files embedded in the assembly from the Migration folder.
/// </summary>
public partial class MigrationService
{
    private GeneralInfo? _generalMigration;
    private readonly Dictionary<string, GeneralInfo> _componentMigrations = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Initializes a new instance of the <see cref="MigrationService"/> class.
    /// </summary>
    public MigrationService()
    {
        LoadMigrationDocumentation();
    }

    /// <summary>
    /// Gets the general migration overview (from MigrationGeneral.md).
    /// </summary>
    /// <returns>The general migration documentation, or null if not found.</returns>
    public GeneralInfo? GetMigrationOverview()
    {
        return _generalMigration;
    }

    /// <summary>
    /// Gets all available component migration guides.
    /// </summary>
    /// <returns>A list of component migration guides ordered by title.</returns>
    public IReadOnlyList<GeneralInfo> GetAllComponentMigrations()
    {
        return [.. _componentMigrations.Values.OrderBy(m => m.Title, StringComparer.OrdinalIgnoreCase)];
    }

    /// <summary>
    /// Gets the migration guide for a specific component.
    /// </summary>
    /// <param name="componentName">
    /// The component name (e.g., "FluentButton", "Button", "FluentDataGrid", "DataGrid").
    /// The search is case-insensitive and supports both prefixed and unprefixed names.
    /// </param>
    /// <returns>The component migration documentation, or null if not found.</returns>
    public GeneralInfo? GetComponentMigration(string? componentName)
    {
        if (string.IsNullOrWhiteSpace(componentName))
        {
            return null;
        }

        var normalized = componentName.Trim();

        // Try exact match first (e.g., "FluentButton")
        if (_componentMigrations.TryGetValue(normalized, out var exactMatch))
        {
            return exactMatch;
        }

        // Try with "Fluent" prefix (e.g., "Button" -> "FluentButton")
        if (!normalized.StartsWith("Fluent", StringComparison.OrdinalIgnoreCase))
        {
            if (_componentMigrations.TryGetValue("Fluent" + normalized, out var prefixedMatch))
            {
                return prefixedMatch;
            }
        }

        // Try without "Fluent" prefix (e.g., "FluentButton" -> "Button")
        if (normalized.StartsWith("Fluent", StringComparison.OrdinalIgnoreCase) && normalized.Length > 6)
        {
            var withoutPrefix = normalized[6..];
            if (_componentMigrations.TryGetValue(withoutPrefix, out var unprefixedMatch))
            {
                return unprefixedMatch;
            }
        }

        // Try partial match on title or filename
        return _componentMigrations.Values.FirstOrDefault(m =>
            m.Title.Contains(normalized, StringComparison.OrdinalIgnoreCase) ||
            m.FileName.Contains(normalized, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Gets a list of available component names that have migration guides.
    /// </summary>
    /// <returns>A sorted list of component names.</returns>
    public IReadOnlyList<string> GetAvailableComponentNames()
    {
        return [.. _componentMigrations.Values
            .Select(m => ExtractComponentName(m.FileName))
            .Where(name => !string.IsNullOrEmpty(name))
            .Order(StringComparer.OrdinalIgnoreCase)];
    }

    private void LoadMigrationDocumentation()
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Only include resources from the Migration folder (segment ".Migration." in the resource name)
        // to avoid capturing unrelated files like MigrationVersion5.md from other folders.
        var resourceNames = assembly.GetManifestResourceNames()
            .Where(name => name.Contains(".Migration.", StringComparison.OrdinalIgnoreCase) &&
                           name.EndsWith(".md", StringComparison.OrdinalIgnoreCase));

        foreach (var resourceName in resourceNames)
        {
            try
            {
                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null)
                {
                    continue;
                }

                using var reader = new StreamReader(stream, Encoding.UTF8);
                var content = reader.ReadToEnd();

                var info = ParseMarkdownFile(content, resourceName);
                if (info == null || string.IsNullOrEmpty(info.Title))
                {
                    continue;
                }

                var fileName = info.FileName;

                // Classify: general vs component-specific
                if (fileName.Equals("MigrationGeneral", StringComparison.OrdinalIgnoreCase))
                {
                    _generalMigration = info;
                }
                else if (fileName.StartsWith("Migration", StringComparison.OrdinalIgnoreCase))
                {
                    // Extract the component name from the filename (e.g., "MigrationFluentButton" -> "FluentButton")
                    var componentName = ExtractComponentName(fileName);
                    if (!string.IsNullOrEmpty(componentName))
                    {
                        _componentMigrations[componentName] = info;
                    }
                }
            }
            catch
            {
                // Skip files that cannot be read
            }
        }
    }

    internal static string ExtractComponentName(string fileName)
    {
        // Remove "Migration" prefix to get the component name
        if (fileName.StartsWith("Migration", StringComparison.OrdinalIgnoreCase) &&
            fileName.Length > "Migration".Length)
        {
            return fileName["Migration".Length..];
        }

        return fileName;
    }

    private static GeneralInfo? ParseMarkdownFile(string content, string resourceName)
    {
        var info = new GeneralInfo
        {
            FileName = ExtractFileName(resourceName),
            Content = content,
        };

        // Parse YAML front matter
        var frontMatterMatch = FrontMatterRegex().Match(content);
        if (frontMatterMatch.Success)
        {
            var frontMatter = frontMatterMatch.Groups[0].Value;
            info.Title = ExtractYamlValue(frontMatter, "title");
            info.Route = ExtractYamlValue(frontMatter, "route");
            info.Hidden = ExtractYamlValue(frontMatter, "hidden").Equals("true", StringComparison.OrdinalIgnoreCase);

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
        var lines = content.Split('\n')
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrEmpty(l) && !l.StartsWith('#'))
            .Take(3);

        var summary = string.Join(' ', lines);

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
