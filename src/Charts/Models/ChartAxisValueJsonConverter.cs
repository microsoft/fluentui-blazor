// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Serializes and deserializes a <see cref="ChartAxisValue"/> as either a JSON number
/// or string, matching the format expected by the chart web components. Non-finite
/// numeric values are serialized as JSON <see langword="null"/> so chart renderers
/// can treat them as gaps instead of receiving invalid numeric tokens.
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

        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString() ?? string.Empty;

            return DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var dateValue)
                ? (ChartAxisValue)dateValue
                : (ChartAxisValue)value;
        }

        throw new JsonException($"Unexpected token {reader.TokenType} when reading {nameof(ChartAxisValue)}.");
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, ChartAxisValue value, JsonSerializerOptions options)
    {
        if (value.IsString)
        {
            writer.WriteStringValue(value.StringValue);
            return;
        }

        if (value.IsDate)
        {
            writer.WriteStringValue(value.DateValue.ToString("O", CultureInfo.InvariantCulture));
            return;
        }

        if (double.IsFinite(value.NumberValue))
        {
            writer.WriteNumberValue(value.NumberValue);
            return;
        }

        writer.WriteNullValue();
    }
}
