// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Accessibility data attached to a chart tooltip callout element.
/// Maps to the web component's <c>AccessibilityData</c> interface.
/// </summary>
public sealed record CalloutAccessibilityData
{
    /// <summary>
    /// Gets the accessible label announced by screen readers for the callout element.
    /// </summary>
    [JsonPropertyName("ariaLabel")]
    public string? AriaLabel { get; init; }
}

