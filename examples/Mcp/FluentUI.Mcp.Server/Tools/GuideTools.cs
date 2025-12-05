// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Text;
using FluentUI.Mcp.Server.Services;
using ModelContextProtocol.Server;

namespace FluentUI.Mcp.Server.Tools;

/// <summary>
/// MCP Tools for accessing documentation guides.
/// </summary>
[McpServerToolType]
public class GuideTools
{
    private readonly DocumentationGuideService _guideService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GuideTools"/> class.
    /// </summary>
    public GuideTools(DocumentationGuideService guideService)
    {
        _guideService = guideService;
    }

    [McpServerTool(Name = "GetGuide")]
    [Description("Gets a specific documentation guide by name. Available guides: installation, defaultvalues, whatsnew, migration, localization, styles.")]
    public string GetGuide(
        [Description("The guide name: 'installation', 'defaultvalues', 'whatsnew', 'migration', 'localization', or 'styles'")] 
        string guideName)
    {
        var content = _guideService.GetGuideContent(guideName);

        if (content == null)
        {
            var guides = _guideService.GetAllGuides();
            var available = guides.Count > 0
                ? string.Join(", ", guides.Select(g => g.Key))
                : "installation, defaultvalues, whatsnew, migration, localization, styles";

            return $"Guide '{guideName}' not found.\n\nAvailable guides: {available}";
        }

        return content;
    }

    [McpServerTool(Name = "SearchGuides")]
    [Description("Searches documentation guides for specific content or topics.")]
    public string SearchGuides(
        [Description("The search term to find in documentation guides")]
        string searchTerm)
    {
        var guides = _guideService.GetAllGuides();
        var results = new StringBuilder();
        var matchCount = 0;

        results.AppendLine($"# Search Results for: \"{searchTerm}\"");
        results.AppendLine();

        foreach (var guide in guides)
        {
            var content = _guideService.GetGuideContent(guide.Key);
            if (content == null)
            {
                continue;
            }

            // Case-insensitive search
            var index = content.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                matchCount++;
                results.AppendLine($"## Found in: {guide.Title}");
                results.AppendLine();
                results.AppendLine($"**Guide:** `fluentui://guide/{guide.Key}`");
                results.AppendLine();

                // Extract context around the match (up to 200 chars before and after)
                var start = Math.Max(0, index - 100);
                var length = Math.Min(300, content.Length - start);
                var excerpt = content.Substring(start, length);

                // Clean up excerpt
                if (start > 0)
                {
                    excerpt = "..." + excerpt;
                }

                if (start + length < content.Length)
                {
                    excerpt += "...";
                }

                results.AppendLine("**Excerpt:**");
                results.AppendLine();
                results.AppendLine($"> {excerpt.Replace("\n", " ").Replace("\r", "")}");
                results.AppendLine();
            }
        }

        if (matchCount == 0)
        {
            results.AppendLine("No matches found in documentation guides.");
            results.AppendLine();
            results.AppendLine("**Available guides:**");
            foreach (var guide in guides)
            {
                results.AppendLine($"- {guide.Title} (`{guide.Key}`)");
            }
        }
        else
        {
            results.AppendLine($"---");
            results.AppendLine($"Found {matchCount} guide(s) containing \"{searchTerm}\".");
        }

        return results.ToString();
    }

    [McpServerTool(Name = "ListGuides")]
    [Description("Lists all available documentation guides with descriptions.")]
    public string ListGuides()
    {
        var guides = _guideService.GetAllGuides();

        var sb = new StringBuilder();
        sb.AppendLine("# Available Documentation Guides");
        sb.AppendLine();

        if (guides.Count == 0)
        {
            sb.AppendLine("No guides currently available.");
            sb.AppendLine();
            sb.AppendLine("Standard guides include:");
            sb.AppendLine("- **installation** - NuGet packages and project setup");
            sb.AppendLine("- **defaultvalues** - Configure default component values");
            sb.AppendLine("- **whatsnew** - Release notes and changes");
            sb.AppendLine("- **migration** - Migration guide from v4 to v5");
            sb.AppendLine("- **localization** - Translate component texts");
            sb.AppendLine("- **styles** - CSS, design tokens, and theming");
            return sb.ToString();
        }

        sb.AppendLine("| Guide | Title | Description |");
        sb.AppendLine("|-------|-------|-------------|");
        sb.AppendLine("| `installation` | Installation | NuGet packages, setup, and configuration |");
        sb.AppendLine("| `defaultvalues` | Default Values | Configure default component values globally |");
        sb.AppendLine("| `whatsnew` | What's New | Release notes and latest changes |");
        sb.AppendLine("| `migration` | Migration to v5 | Breaking changes and migration steps |");
        sb.AppendLine("| `localization` | Localization | Translate and localize component texts |");
        sb.AppendLine("| `styles` | Styles | CSS styling, design tokens, theming |");
        sb.AppendLine();
        sb.AppendLine("Use `GetGuide(guideName)` to retrieve the full content of a guide.");

        return sb.ToString();
    }
}
