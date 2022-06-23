using System.Text.RegularExpressions;

namespace Microsoft.Fast.Components.FluentUI;

public enum DataGridRowType
{
    Default,
    Header,
    StickyHeader
}

public static class DataGridRowTypeExtensions
{
    private static readonly Dictionary<DataGridRowType, string> _dataGridRowTypeValues =
        Enum.GetValues<DataGridRowType>().ToDictionary(id => id, id => string.Join("-", Regex.Split(Enum.GetName(id)!, @"(?<!^)(?=[A-Z](?![A-Z]|$))")).ToLowerInvariant());

    public static string? ToAttributeValue(this DataGridRowType? value) => value == null ? null : _dataGridRowTypeValues[value.Value];
}