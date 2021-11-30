using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Fast.Components.FluentUI;
public enum DataGridCellType
{
    Default,
    ColumnHeader,
    RowHeader
}

internal static class CellTypeExtensions
{
    private static readonly Dictionary<DataGridCellType, string> _cellTypeValues =
        Enum.GetValues<DataGridCellType>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this DataGridCellType? value) => value == null ? null : _cellTypeValues[value.Value];
}