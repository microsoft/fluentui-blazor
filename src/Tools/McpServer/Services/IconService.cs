// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.FluentUI.AspNetCore.McpServer.Models;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Services;

/// <summary>
/// Service for searching and querying the Fluent UI icon catalog.
/// Loads the icon data from the embedded all-icons.json resource.
/// </summary>
public sealed class IconService
{
    private const string EmbeddedResourceName = "Microsoft.FluentUI.AspNetCore.McpServer.all-icons.json";

    /// <summary>
    /// Known variants in the Fluent UI icon system.
    /// </summary>
    private static readonly string[] KnownVariants = ["Filled", "Regular", "Light", "Color"];

    /// <summary>
    /// Standard sizes in the Fluent UI icon system.
    /// </summary>
    private static readonly int[] KnownSizes = [10, 12, 16, 20, 24, 28, 32, 48];

    private readonly List<IconModel> _icons;
    private readonly IconSynonymService _synonymService;

    /// <summary>
    /// Initializes a new instance of the <see cref="IconService"/> class.
    /// Loads the icon catalog from the embedded resource.
    /// </summary>
    public IconService(IconSynonymService synonymService)
    {
        _synonymService = synonymService;
        _icons = LoadIcons();
    }

    /// <summary>
    /// Gets the total number of icons in the catalog.
    /// </summary>
    public int Count => _icons.Count;

    /// <summary>
    /// Gets all icons in the catalog.
    /// </summary>
    public IReadOnlyList<IconModel> GetAllIcons() => _icons;

    /// <summary>
    /// Searches for icons by name or keyword, with optional variant and size filters.
    /// Uses synonym mapping for improved discoverability.
    /// </summary>
    /// <param name="searchTerm">The term to search for (icon name or keyword).</param>
    /// <param name="variant">Optional variant filter (Filled, Regular, Light, Color).</param>
    /// <param name="size">Optional size filter (10, 12, 16, 20, 24, 28, 32, 48).</param>
    /// <returns>A list of matching icons.</returns>
    public IReadOnlyList<IconModel> SearchIcons(string searchTerm, string? variant = null, int? size = null)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return [];
        }

        var term = searchTerm.Trim();
        var results = new HashSet<IconModel>();

        CollectDirectMatches(term, results);
        CollectSynonymMatches(term, results);

        return ApplyFilters(results, variant, size);
    }

    private void CollectDirectMatches(string term, HashSet<IconModel> results)
    {
        foreach (var icon in _icons)
        {
            if (icon.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
            {
                results.Add(icon);
            }
        }
    }

    private void CollectSynonymMatches(string term, HashSet<IconModel> results)
    {
        // Exact synonym match
        if (_synonymService.TryGetTargets(term, out var synonymTargets))
        {
            AddIconsMatchingTargets(synonymTargets, results);
        }

        // Partial synonym matching (e.g., "notif" matches "notification" key)
        foreach (var targets in _synonymService.GetPartialMatches(term))
        {
            AddIconsMatchingTargets(targets, results);
        }
    }

    private void AddIconsMatchingTargets(string[] targets, HashSet<IconModel> results)
    {
        foreach (var target in targets)
        {
            foreach (var icon in _icons)
            {
                if (icon.Name.Contains(target, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(icon);
                }
            }
        }
    }

    private static IReadOnlyList<IconModel> ApplyFilters(HashSet<IconModel> results, string? variant, int? size)
    {
        var filtered = results.AsEnumerable();

        if (!string.IsNullOrEmpty(variant))
        {
            filtered = filtered.Where(i => i.Variants.ContainsKey(variant));
        }

        if (size.HasValue)
        {
            filtered = filtered.Where(i => i.Variants.Values.Any(sizes => sizes.Contains(size.Value)));
        }

        return filtered.OrderBy(i => i.Name, StringComparer.OrdinalIgnoreCase).ToList();
    }

    /// <summary>
    /// Gets a specific icon by exact name (case-insensitive).
    /// </summary>
    public IconModel? GetIconByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        return _icons.FirstOrDefault(i => i.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Gets all icons that support a given variant.
    /// </summary>
    public IReadOnlyList<IconModel> GetIconsByVariant(string variant)
    {
        return _icons.Where(i => i.Variants.ContainsKey(variant)).ToList();
    }

    /// <summary>
    /// Gets all icons that support a given size (in any variant).
    /// </summary>
    public IReadOnlyList<IconModel> GetIconsBySize(int size)
    {
        return _icons.Where(i => i.Variants.Values.Any(sizes => sizes.Contains(size))).ToList();
    }

    /// <summary>
    /// Gets a summary of how many icons support each variant.
    /// </summary>
    public IReadOnlyDictionary<string, int> GetVariantSummary()
    {
        var summary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var variant in KnownVariants)
        {
            summary[variant] = _icons.Count(i => i.Variants.ContainsKey(variant));
        }

        return summary;
    }

    /// <summary>
    /// Gets a summary of how many icons support each size.
    /// </summary>
    public IReadOnlyDictionary<int, int> GetSizeSummary()
    {
        var summary = new Dictionary<int, int>();
        foreach (var size in KnownSizes)
        {
            summary[size] = _icons.Count(i => i.Variants.Values.Any(sizes => sizes.Contains(size)));
        }

        return summary;
    }

    /// <summary>
    /// Generates a Blazor code snippet for using the specified icon.
    /// </summary>
    /// <param name="iconName">The icon name.</param>
    /// <param name="variant">The variant (Filled, Regular, Light).</param>
    /// <param name="size">The icon size.</param>
    /// <returns>A Blazor code snippet string, or an error message if the combination is invalid.</returns>
    public string GenerateBlazorCode(string iconName, string variant, int size)
    {
        var icon = GetIconByName(iconName);
        if (icon is null)
        {
            return $"<!-- Icon '{iconName}' not found in the catalog -->";
        }

        // "Color" variant is not available through the standard class hierarchy
        if (variant.Equals("Color", StringComparison.OrdinalIgnoreCase))
        {
            return string.Create(CultureInfo.InvariantCulture,
                $"<!-- The 'Color' variant for '{icon.Name}' is a multi-color icon. -->\n" +
                $"<!-- Color variants are not yet available through the Icons.Color.SizeNN class hierarchy. -->\n" +
                $"<!-- Consider using the 'Filled' or 'Regular' variant instead. -->");
        }

        if (!icon.HasVariantAndSize(variant, size))
        {
            var sb = new StringBuilder();
            sb.AppendLine(CultureInfo.InvariantCulture, $"<!-- '{icon.Name}' is not available in {variant} Size{size}. -->");
            sb.AppendLine("<!-- Available combinations: -->");
            foreach (var v in icon.Variants)
            {
                var sizes = string.Join(", ", v.Value);
                sb.AppendLine(CultureInfo.InvariantCulture, $"<!--   {v.Key}: Size {sizes} -->");
            }

            return sb.ToString().TrimEnd();
        }

        return $"<FluentIcon Value=\"@(new Icons.{variant}.Size{size}.{icon.Name}())\" />";
    }

    /// <summary>
    /// Generates multiple Blazor code snippets showing common usage patterns.
    /// </summary>
    public string GenerateUsageExamples(string iconName, string variant, int size)
    {
        var icon = GetIconByName(iconName);
        if (icon is null)
        {
            return $"Icon '{iconName}' not found in the catalog.";
        }

        if (variant.Equals("Color", StringComparison.OrdinalIgnoreCase))
        {
            return $"The 'Color' variant for '{icon.Name}' is a multi-color icon and is not available through the standard class hierarchy. " +
                   $"Consider using 'Filled' or 'Regular' instead.";
        }

        if (!icon.HasVariantAndSize(variant, size))
        {
            return GenerateBlazorCode(iconName, variant, size);
        }

        var iconClass = $"Icons.{variant}.Size{size}.{icon.Name}";

        var sb = new StringBuilder();
        sb.AppendLine(CultureInfo.InvariantCulture, $"# Usage Examples for {icon.Name} ({variant} Size{size})");
        sb.AppendLine();

        AppendBasicExample(sb, iconClass);
        AppendColorExample(sb, iconClass);
        AppendButtonExample(sb, iconClass, icon.Name);
        AppendClickAndAccessibilityExamples(sb, iconClass, icon.Name);

        return sb.ToString();
    }

    private static void AppendBasicExample(StringBuilder sb, string iconClass)
    {
        sb.AppendLine("## Basic Icon");
        sb.AppendLine("```razor");
        sb.AppendLine(CultureInfo.InvariantCulture, $"<FluentIcon Value=\"@(new {iconClass}())\" />");
        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendColorExample(StringBuilder sb, string iconClass)
    {
        sb.AppendLine("## With Color");
        sb.AppendLine("```razor");
        sb.AppendLine(CultureInfo.InvariantCulture, $"<FluentIcon Value=\"@(new {iconClass}())\" Color=\"@Color.Primary\" />");
        sb.AppendLine(CultureInfo.InvariantCulture, $"<FluentIcon Value=\"@(new {iconClass}())\" Color=\"@Color.Error\" />");
        sb.AppendLine(CultureInfo.InvariantCulture, $"<FluentIcon Value=\"@(new {iconClass}().WithColor(\"#FF5733\"))\" />");
        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendButtonExample(StringBuilder sb, string iconClass, string iconName)
    {
        sb.AppendLine("## Inside a Button (Start Slot)");
        sb.AppendLine("```razor");
        sb.AppendLine("<FluentButton>");
        sb.AppendLine(CultureInfo.InvariantCulture, $"    <FluentIcon Value=\"@(new {iconClass}())\" Slot=\"@FluentSlot.Start\" />");
        sb.AppendLine(CultureInfo.InvariantCulture, $"    {iconName}");
        sb.AppendLine("</FluentButton>");
        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendClickAndAccessibilityExamples(StringBuilder sb, string iconClass, string iconName)
    {
        sb.AppendLine("## With Click Handler");
        sb.AppendLine("```razor");
        sb.AppendLine(CultureInfo.InvariantCulture, $"<FluentIcon Value=\"@(new {iconClass}())\" OnClick=\"@HandleClick\" />");
        sb.AppendLine("```");
        sb.AppendLine();

        sb.AppendLine("## With Tooltip and Accessibility");
        sb.AppendLine("```razor");
        sb.AppendLine(CultureInfo.InvariantCulture, $"<FluentIcon Value=\"@(new {iconClass}())\"");
        sb.AppendLine(CultureInfo.InvariantCulture, $"             Title=\"{iconName}\"");
        sb.AppendLine(CultureInfo.InvariantCulture, $"             Tooltip=\"{iconName} icon\" />");
        sb.AppendLine("```");
        sb.AppendLine();
    }

    /// <summary>
    /// Suggests the best default variant and size for a given icon.
    /// Prefers Regular Size20 if available.
    /// </summary>
    public (string Variant, int Size) GetRecommendedDefault(IconModel icon)
    {
        // Prefer Regular Size20
        if (icon.HasVariantAndSize("Regular", 20))
        {
            return ("Regular", 20);
        }

        // Then Regular Size24
        if (icon.HasVariantAndSize("Regular", 24))
        {
            return ("Regular", 24);
        }

        // Then Filled Size20
        if (icon.HasVariantAndSize("Filled", 20))
        {
            return ("Filled", 20);
        }

        // Fall back to first available combination
        foreach (var kvp in icon.Variants)
        {
            if (kvp.Value.Count > 0)
            {
                return (kvp.Key, kvp.Value[0]);
            }
        }

        return ("Regular", 20);
    }

    private static List<IconModel> LoadIcons()
    {
        var assembly = Assembly.GetExecutingAssembly();

        using var stream = assembly.GetManifestResourceStream(EmbeddedResourceName)
            ?? throw new InvalidOperationException(
                $"Embedded resource '{EmbeddedResourceName}' not found. " +
                $"Available resources: {string.Join(", ", assembly.GetManifestResourceNames())}");

        using var document = JsonDocument.Parse(stream);
        var root = document.RootElement;

        var icons = new List<IconModel>();

        foreach (var property in root.EnumerateObject())
        {
            icons.Add(ParseIconEntry(property));
        }

        return icons;
    }

    private static IconModel ParseIconEntry(JsonProperty property)
    {
        var variants = new Dictionary<string, IList<int>>(StringComparer.OrdinalIgnoreCase);

        foreach (var variantProperty in property.Value.EnumerateObject())
        {
            var sizes = new List<int>();
            foreach (var sizeElement in variantProperty.Value.EnumerateArray())
            {
                sizes.Add(sizeElement.GetInt32());
            }

            variants[variantProperty.Name] = sizes;
        }

        return new IconModel(property.Name, variants);
    }
}
