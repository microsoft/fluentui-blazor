using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Fast.Components.FluentUI
{
    public enum HorizontalPosition
    {
        Unset,
        Start,
        End,
        Left,
        Right
    }

    internal static class HorizontalDefaultPositionExtensions
    {
        private static readonly Dictionary<HorizontalPosition, string> _positionValues =
            Enum.GetValues<HorizontalPosition>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

        public static string? ToAttributeValue(this HorizontalPosition? value) => value == null ? null : _positionValues[value.Value];
    }
}
