using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Fast.Components.FluentUI
{
    public enum Orientation
    {
        Horizontal,
        Vertical
    }

    internal static class OrientationExtensions
    {
        private static readonly Dictionary<Orientation, string> _orientationValues =
            Enum.GetValues<Orientation>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

        public static string? ToAttributeValue(this Orientation? value) => value == null ? null : _orientationValues[value.Value];
    }
}
