using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Fast.Components.FluentUI
{
    public enum Positioning
    {
        Uncontrolled,
        Locktodefault,
        Dynamic
    }

    internal static class PositioningExtensions
    {
        private static Dictionary<Positioning, string> _positioningValues =
            Enum.GetValues<Positioning>().ToDictionary(id => id, id => Enum.GetName(id).ToLowerInvariant());

        public static string ToAttributeValue(this Positioning? value) => value == null ? null : _positioningValues[value.Value];
    }
}
