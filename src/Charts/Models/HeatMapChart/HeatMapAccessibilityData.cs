// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Accessibility data attached to a heat map callout element.
/// </summary>
public sealed record HeatMapAccessibilityData
{
    /// <summary>
    /// Gets the accessible label announced by screen readers for the callout element.
    /// </summary>
    [JsonPropertyName("ariaLabel")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AriaLabel { get; init; }
}
