// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components.Charts;

/// <summary>
/// Represents a value that can be plotted on any axis of any chart type.
/// Holds either a numeric value (e.g. a Unix timestamp in milliseconds or an
/// arbitrary scalar) or a <see cref="DateTimeOffset"/> that is serialized as an
/// ISO 8601 string so chart web components can consume it directly.
/// </summary>
/// <remarks>
/// Use the implicit conversions from <see cref="double"/>, <see cref="DateTime"/>,
/// or <see cref="DateTimeOffset"/> to create an instance:
/// <code>
/// ChartAxisValue numeric = 42.0;
/// ChartAxisValue date    = new DateTime(2024, 1, 1);
/// ChartAxisValue offset  = DateTimeOffset.UtcNow;
/// </code>
/// </remarks>
[JsonConverter(typeof(ChartAxisValueJsonConverter))]
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct ChartAxisValue : IEquatable<ChartAxisValue>
{
    private readonly double? _number;
    private readonly DateTimeOffset? _date;

    private ChartAxisValue(double number)
    {
        _number = number;
        _date = null;
    }

    private ChartAxisValue(DateTimeOffset date)
    {
        _number = null;
        _date = date;
    }

    internal bool IsDate => _date.HasValue;
    internal double NumberValue => _number ?? 0.0;
    internal DateTimeOffset DateValue => _date ?? default;

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

    /// <summary>Determines whether this instance equals another <see cref="ChartAxisValue"/>.</summary>
    public bool Equals(ChartAxisValue other) => _number == other._number && _date == other._date;

    /// <summary>Determines whether this instance equals another object.</summary>
    public override bool Equals(object? obj) => obj is ChartAxisValue other && Equals(other);

    /// <summary>Returns the hash code for this instance.</summary>
    public override int GetHashCode() => HashCode.Combine(_number, _date);

    /// <summary>Returns <see langword="true"/> when both values are equal.</summary>
    public static bool operator ==(ChartAxisValue left, ChartAxisValue right) => left.Equals(right);

    /// <summary>Returns <see langword="true"/> when the values are not equal.</summary>
    public static bool operator !=(ChartAxisValue left, ChartAxisValue right) => !left.Equals(right);
}
