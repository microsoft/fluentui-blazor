// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Serializes a <see cref="DataVizPalette"/> value as its token string
/// (e.g. <c>"color5"</c>, <c>"info"</c>) so the chart web components can
/// resolve it to an actual hex color at runtime.
/// </summary>
[ExcludeFromCodeCoverage(Justification = "This class is used for source-generated JSON serialization and does not contain any logic to be tested.")]
internal sealed class DataVizPaletteJsonConverter : JsonConverter<DataVizPalette>
{
    /// <inheritdoc/>
    public override DataVizPalette Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotSupportedException($"{nameof(DataVizPalette)} deserialization is not supported.");

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, DataVizPalette value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToAttributeValue());
}
