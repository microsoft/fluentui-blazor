// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.McpServer.Models;

/// <summary>
/// Represents a Fluent UI icon with its available variants and sizes.
/// </summary>
/// <param name="Name">The icon name (e.g., "Bookmark", "Alert", "Add").</param>
/// <param name="Variants">Dictionary of variant names (e.g., "Filled", "Regular", "Light", "Color") to available sizes.</param>
public sealed record IconModel(string Name, IDictionary<string, IList<int>> Variants)
{
    /// <summary>
    /// Gets all variant names available for this icon.
    /// </summary>
    public IEnumerable<string> VariantNames => Variants.Keys;

    /// <summary>
    /// Gets all unique sizes available across all variants.
    /// </summary>
    public IEnumerable<int> AllSizes => Variants.Values.SelectMany(s => s).Distinct().OrderBy(s => s);

    /// <summary>
    /// Checks whether a specific variant and size combination is available.
    /// </summary>
    public bool HasVariantAndSize(string variant, int size)
    {
        return Variants.TryGetValue(variant, out var sizes) && sizes.Contains(size);
    }

    /// <summary>
    /// Gets the sizes available for a specific variant.
    /// </summary>
    public IReadOnlyList<int> GetSizesForVariant(string variant)
    {
        return Variants.TryGetValue(variant, out var sizes) ? sizes.ToList() : [];
    }
}
