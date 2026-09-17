// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;
using System.Text;
using Microsoft.FluentUI.AspNetCore.McpServer.Models;
using Microsoft.FluentUI.AspNetCore.McpServer.Services;
using ModelContextProtocol.Server;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Resources;

/// <summary>
/// MCP Resources providing icon catalog context for Fluent UI Blazor.
/// These resources are user-selected and provide the LLM with icon discovery information.
/// </summary>
[McpServerResourceType]
public class IconResources
{
    private readonly IconService _iconService;

    /// <summary>
    /// Initializes a new instance of the <see cref="IconResources"/> class.
    /// </summary>
    public IconResources(IconService iconService)
    {
        _iconService = iconService;
    }

    /// <summary>
    /// Gets the icon catalog overview with statistics and a sample of available icons.
    /// </summary>
    [McpServerResource(
        UriTemplate = "fluentui://icons",
        Name = "icons",
        Title = "Fluent UI Icon Catalog",
        MimeType = "text/markdown")]
    [Description("Overview of the Fluent UI icon catalog with statistics, available variants, sizes, and a sample of icon names. Use the SearchIcons tool for targeted lookups.")]
    public string GetIconCatalog()
    {
        var allIcons = _iconService.GetAllIcons();
        var variantSummary = _iconService.GetVariantSummary();
        var sizeSummary = _iconService.GetSizeSummary();

        var sb = new StringBuilder();
        sb.AppendLine("# Fluent UI Icon Catalog");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"**Total icons:** {_iconService.Count}");
        sb.AppendLine();

        AppendVariantTable(sb, variantSummary);
        AppendSizeTable(sb, sizeSummary);
        AppendUsageGuidance(sb);
        AppendSampleIcons(sb, allIcons);

        return sb.ToString();
    }

    private static void AppendVariantTable(StringBuilder sb, IReadOnlyDictionary<string, int> variantSummary)
    {
        sb.AppendLine("## Variants");
        sb.AppendLine();
        sb.AppendLine("| Variant | Description | Icon Count |");
        sb.AppendLine("|---------|-------------|------------|");
        sb.AppendLine(CultureInfo.InvariantCulture, $"| **Regular** | Outlined/line-drawn icons. Best for most UI contexts. | {variantSummary.GetValueOrDefault("Regular")} |");
        sb.AppendLine(CultureInfo.InvariantCulture, $"| **Filled** | Solid/filled icons. Use for emphasis or active states. | {variantSummary.GetValueOrDefault("Filled")} |");
        sb.AppendLine(CultureInfo.InvariantCulture, $"| **Light** | Thin/lightweight icons. Typically only Size32. | {variantSummary.GetValueOrDefault("Light")} |");
        sb.AppendLine(CultureInfo.InvariantCulture, $"| **Color** | Multi-color icons. Not all available via standard class hierarchy. | {variantSummary.GetValueOrDefault("Color")} |");
        sb.AppendLine();
    }

    private static void AppendSizeTable(StringBuilder sb, IReadOnlyDictionary<int, int> sizeSummary)
    {
        sb.AppendLine("## Sizes");
        sb.AppendLine();
        sb.AppendLine("| Size | Typical Use | Icon Count |");
        sb.AppendLine("|------|-------------|------------|");
        sb.AppendLine(CultureInfo.InvariantCulture, $"| **10** | Micro indicators | {sizeSummary.GetValueOrDefault(10)} |");
        sb.AppendLine(CultureInfo.InvariantCulture, $"| **12** | Compact UI, badges | {sizeSummary.GetValueOrDefault(12)} |");
        sb.AppendLine(CultureInfo.InvariantCulture, $"| **16** | Small buttons, inline text | {sizeSummary.GetValueOrDefault(16)} |");
        sb.AppendLine(CultureInfo.InvariantCulture, $"| **20** | Default for most components | {sizeSummary.GetValueOrDefault(20)} |");
        sb.AppendLine(CultureInfo.InvariantCulture, $"| **24** | Buttons, toolbars, navigation | {sizeSummary.GetValueOrDefault(24)} |");
        sb.AppendLine(CultureInfo.InvariantCulture, $"| **28** | Medium emphasis areas | {sizeSummary.GetValueOrDefault(28)} |");
        sb.AppendLine(CultureInfo.InvariantCulture, $"| **32** | Headers, hero sections | {sizeSummary.GetValueOrDefault(32)} |");
        sb.AppendLine(CultureInfo.InvariantCulture, $"| **48** | Large displays, empty states | {sizeSummary.GetValueOrDefault(48)} |");
        sb.AppendLine();
    }

    private static void AppendUsageGuidance(StringBuilder sb)
    {
        sb.AppendLine("## Usage");
        sb.AppendLine();
        sb.AppendLine("```razor");
        sb.AppendLine("<FluentIcon Value=\"@(new Icons.Regular.Size20.Bookmark())\" />");
        sb.AppendLine("<FluentIcon Value=\"@(new Icons.Filled.Size24.Alert())\" Color=\"@Color.Warning\" />");
        sb.AppendLine("```");
        sb.AppendLine();
        sb.AppendLine("> Use the **SearchIcons** tool to find icons by name or keyword.");
        sb.AppendLine("> Use the **GetIconDetails** tool to see all variants and sizes for a specific icon.");
        sb.AppendLine("> Use the **GetIconUsage** tool to generate ready-to-paste Blazor code.");
        sb.AppendLine();
    }

    private void AppendSampleIcons(StringBuilder sb, IReadOnlyList<IconModel> allIcons)
    {
        sb.AppendLine("## Sample Icon Names (first 100)");
        sb.AppendLine();
        var sampleIcons = allIcons.Take(100);
        foreach (var icon in sampleIcons)
        {
            var variants = string.Join("/", icon.VariantNames);
            sb.AppendLine(CultureInfo.InvariantCulture, $"- {icon.Name} ({variants})");
        }

        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"*... and {_iconService.Count - 100} more. Use SearchIcons to discover icons by keyword.*");
    }

    /// <summary>
    /// Gets detailed information about icon variants with usage guidance.
    /// </summary>
    [McpServerResource(
        UriTemplate = "fluentui://icons/variants",
        Name = "icon_variants",
        Title = "Icon Variants Overview",
        MimeType = "text/markdown")]
    [Description("Detailed guide to Fluent UI icon variants (Filled, Regular, Light, Color) with usage recommendations and size availability.")]
    public string GetIconVariants()
    {
        var variantSummary = _iconService.GetVariantSummary();

        var sb = new StringBuilder();
        sb.AppendLine("# Fluent UI Icon Variants");
        sb.AppendLine();

        AppendRegularVariant(sb, variantSummary);
        AppendFilledVariant(sb, variantSummary);
        AppendLightVariant(sb, variantSummary);
        AppendColorVariant(sb, variantSummary);
        AppendChoosingGuide(sb);

        return sb.ToString();
    }

    private static void AppendRegularVariant(StringBuilder sb, IReadOnlyDictionary<string, int> summary)
    {
        sb.AppendLine("## Regular (Outlined)");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"**{summary.GetValueOrDefault("Regular")} icons** available.");
        sb.AppendLine();
        sb.AppendLine("- Line-drawn, outlined icons");
        sb.AppendLine("- **Recommended for most UI contexts** — navigation, toolbars, forms");
        sb.AppendLine("- Available in most sizes: 10, 12, 16, 20, 24, 28, 32, 48");
        sb.AppendLine("- Use as the default choice unless you have a specific reason to use another variant");
        sb.AppendLine();
        sb.AppendLine("```razor");
        sb.AppendLine("<FluentIcon Value=\"@(new Icons.Regular.Size20.Settings())\" />");
        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendFilledVariant(StringBuilder sb, IReadOnlyDictionary<string, int> summary)
    {
        sb.AppendLine("## Filled (Solid)");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"**{summary.GetValueOrDefault("Filled")} icons** available.");
        sb.AppendLine();
        sb.AppendLine("- Solid, filled icons");
        sb.AppendLine("- Use for **emphasis**, **active/selected states**, or **high-contrast** needs");
        sb.AppendLine("- Pair with Regular variant: Regular for inactive, Filled for active");
        sb.AppendLine("- Available in most sizes: 10, 12, 16, 20, 24, 28, 32, 48");
        sb.AppendLine();
        sb.AppendLine("```razor");
        sb.AppendLine("<FluentIcon Value=\"@(new Icons.Filled.Size20.Heart())\" Color=\"@Color.Error\" />");
        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendLightVariant(StringBuilder sb, IReadOnlyDictionary<string, int> summary)
    {
        sb.AppendLine("## Light (Thin)");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"**{summary.GetValueOrDefault("Light")} icons** available.");
        sb.AppendLine();
        sb.AppendLine("- Thin, lightweight line icons");
        sb.AppendLine("- Typically **only available in Size32**");
        sb.AppendLine("- Best for decorative use, large displays, or low-emphasis contexts");
        sb.AppendLine("- Limited availability — not all icons have a Light variant");
        sb.AppendLine();
        sb.AppendLine("```razor");
        sb.AppendLine("<FluentIcon Value=\"@(new Icons.Light.Size32.Star())\" />");
        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendColorVariant(StringBuilder sb, IReadOnlyDictionary<string, int> summary)
    {
        sb.AppendLine("## Color (Multi-color)");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"**{summary.GetValueOrDefault("Color")} icons** available.");
        sb.AppendLine();
        sb.AppendLine("- Multi-color icons with pre-defined color schemes");
        sb.AppendLine("- ⚠️ **Not available through the standard `Icons.Color.SizeNN` class hierarchy**");
        sb.AppendLine("- Limited availability — only select icons have a Color variant");
        sb.AppendLine("- Typically used for product/brand icons");
        sb.AppendLine();
    }

    private static void AppendChoosingGuide(StringBuilder sb)
    {
        sb.AppendLine("## Choosing the Right Variant");
        sb.AppendLine();
        sb.AppendLine("| Context | Recommended Variant | Size |");
        sb.AppendLine("|---------|-------------------|------|");
        sb.AppendLine("| Inline text, compact UI | Regular | 16 |");
        sb.AppendLine("| Default component usage | Regular | 20 |");
        sb.AppendLine("| Buttons, toolbars | Regular | 20 or 24 |");
        sb.AppendLine("| Navigation items | Regular | 20 or 24 |");
        sb.AppendLine("| Selected/active state | Filled | Match the Regular size |");
        sb.AppendLine("| Toggle icons (like/unlike) | Regular → Filled on active | 20 or 24 |");
        sb.AppendLine("| Empty state illustrations | Regular or Light | 32 or 48 |");
        sb.AppendLine("| Header/hero areas | Regular | 32 or 48 |");
    }

    /// <summary>
    /// Gets full details for a specific icon by name.
    /// </summary>
    [McpServerResource(
        UriTemplate = "fluentui://icon/{name}",
        Name = "icon_details",
        Title = "Icon Details",
        MimeType = "text/markdown")]
    [Description("Full details for a specific Fluent UI icon including all variants, sizes, and Blazor code snippets.")]
    public string GetIconByName(string name)
    {
        var icon = _iconService.GetIconByName(name);
        if (icon is null)
        {
            return $"Icon '{name}' not found. Use the SearchIcons tool to discover available icons.";
        }

        var (defaultVariant, defaultSize) = _iconService.GetRecommendedDefault(icon);

        var sb = new StringBuilder();
        sb.AppendLine(CultureInfo.InvariantCulture, $"# Icon: {icon.Name}");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"**Recommended:** `Icons.{defaultVariant}.Size{defaultSize}.{icon.Name}`");
        sb.AppendLine();

        // Variant/Size matrix
        sb.AppendLine("## Availability Matrix");
        sb.AppendLine();
        foreach (var kvp in icon.Variants.OrderBy(v => v.Key, StringComparer.OrdinalIgnoreCase))
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"- **{kvp.Key}**: Size {string.Join(", ", kvp.Value)}");
        }

        sb.AppendLine();

        // Code examples
        sb.AppendLine("## Code Examples");
        sb.AppendLine();
        sb.AppendLine("```razor");
        sb.AppendLine(CultureInfo.InvariantCulture, $"@* Basic usage *@");
        sb.AppendLine(CultureInfo.InvariantCulture, $"<FluentIcon Value=\"@(new Icons.{defaultVariant}.Size{defaultSize}.{icon.Name}())\" />");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"@* With color *@");
        sb.AppendLine(CultureInfo.InvariantCulture, $"<FluentIcon Value=\"@(new Icons.{defaultVariant}.Size{defaultSize}.{icon.Name}())\" Color=\"@Color.Primary\" />");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"@* In a button *@");
        sb.AppendLine("<FluentButton>");
        sb.AppendLine(CultureInfo.InvariantCulture, $"    <FluentIcon Value=\"@(new Icons.{defaultVariant}.Size{defaultSize}.{icon.Name}())\" Slot=\"@FluentSlot.Start\" />");
        sb.AppendLine(CultureInfo.InvariantCulture, $"    {icon.Name}");
        sb.AppendLine("</FluentButton>");
        sb.AppendLine("```");

        return sb.ToString();
    }
}
