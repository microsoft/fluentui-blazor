// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents a value that can be plotted on any axis of any chart type.
/// Holds either a numeric value, a <see cref="DateTimeOffset"/>, or a string label,
/// matching the value shapes accepted by the chart web components.
/// </summary>
/// <remarks>
/// Use the implicit conversions from <see cref="double"/>, <see cref="DateTime"/>,
/// <see cref="DateTimeOffset"/>, or <see cref="string"/> to create an instance:
/// <code>
/// ChartAxisValue numeric = 42.0;
/// ChartAxisValue date    = new DateTime(2024, 1, 1);
/// ChartAxisValue offset  = DateTimeOffset.UtcNow;
/// ChartAxisValue label   = "Monday";
/// </code>
/// </remarks>
[JsonConverter(typeof(ChartAxisValueJsonConverter))]
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct ChartAxisValue : IEquatable<ChartAxisValue>
{
    private readonly double? _number;
    private readonly DateTimeOffset? _date;
    private readonly string? _text;

    private ChartAxisValue(double number)
    {
        _number = number;
        _date = null;
        _text = null;
    }

    private ChartAxisValue(DateTimeOffset date)
    {
        _number = null;
        _date = date;
        _text = null;
    }

    private ChartAxisValue(string text)
    {
        _number = null;
        _date = null;
        _text = text;
    }

    internal bool IsDate => _date.HasValue;
    internal bool IsString => _text is not null;
    internal double NumberValue => _number ?? 0.0;
    internal DateTimeOffset DateValue => _date ?? default;
    internal string StringValue => _text ?? string.Empty;

    /// <summary>Creates a <see cref="ChartAxisValue"/> from a numeric value.</summary>
    public static implicit operator ChartAxisValue(double value) => new(value);

    /// <summary>
    /// Creates a <see cref="ChartAxisValue"/> from a <see cref="DateTime"/>.
    /// Unspecified <see cref="DateTimeKind"/> is treated as UTC.
    /// </summary>
    public static implicit operator ChartAxisValue(DateTime value) =>
        new(new DateTimeOffset(value.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(value, DateTimeKind.Utc)
            : value));

    /// <summary>Creates a <see cref="ChartAxisValue"/> from a <see cref="DateTimeOffset"/>.</summary>
    public static implicit operator ChartAxisValue(DateTimeOffset value) => new(value);

    /// <summary>Creates a <see cref="ChartAxisValue"/> from a string label.</summary>
    public static implicit operator ChartAxisValue(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new(value);
    }

    /// <summary>Determines whether this instance equals another <see cref="ChartAxisValue"/>.</summary>
    public bool Equals(ChartAxisValue other) =>
        _number == other._number &&
        _date == other._date &&
        string.Equals(_text, other._text, StringComparison.Ordinal);

    /// <summary>Determines whether this instance equals another object.</summary>
    public override bool Equals(object? obj) => obj is ChartAxisValue other && Equals(other);

    /// <summary>Returns the hash code for this instance.</summary>
    public override int GetHashCode() => HashCode.Combine(_number, _date, _text);

    /// <summary>Returns <see langword="true"/> when both values are equal.</summary>
    public static bool operator ==(ChartAxisValue left, ChartAxisValue right) => left.Equals(right);

    /// <summary>Returns <see langword="true"/> when the values are not equal.</summary>
    public static bool operator !=(ChartAxisValue left, ChartAxisValue right) => !left.Equals(right);
}
