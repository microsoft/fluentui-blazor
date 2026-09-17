// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Serializes and deserializes a <see cref="ChartAxisValue"/> as either a JSON
/// number or an ISO 8601 string, matching the format expected by the chart web components.
/// </summary>
internal sealed class ChartAxisValueJsonConverter : JsonConverter<ChartAxisValue>
{
    /// <inheritdoc/>
    public override ChartAxisValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            return (ChartAxisValue)reader.GetDouble();
        }

        var str = reader.GetString();
        return (ChartAxisValue)DateTimeOffset.Parse(str!, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, ChartAxisValue value, JsonSerializerOptions options)
    {
        if (value.IsDate)
        {
            writer.WriteStringValue(value.DateValue.ToString("O", CultureInfo.InvariantCulture));
        }
        else
        {
            writer.WriteNumberValue(value.NumberValue);
        }
    }
}

#pragma warning restore MA0048 // File name must match type name
