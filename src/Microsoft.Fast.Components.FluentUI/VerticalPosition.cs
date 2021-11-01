using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Fast.Components.FluentUI
{
    public enum VerticalPosition
    {
        Unset,
        Top,
        Bottom
    }

    internal static class VerticalDefaultPositionExtensions
    {
        private static readonly Dictionary<VerticalPosition, string> _positionValues =
            Enum.GetValues<VerticalPosition>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

        public static string? ToAttributeValue(this VerticalPosition? value) => value == null ? null : _positionValues[value.Value];
    }
}
