using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Components.FluentUI
{
    public enum Autocomplete
    {
        Inline,
        List,
        Both
    }

    internal static class AutocompleteExtensions
    {
        private static Dictionary<Autocomplete, string> _autocompleteValues =
            Enum.GetValues<Autocomplete>().ToDictionary(id => id, id => Enum.GetName(id).ToLowerInvariant());

        public static string ToAttributeValue(this Autocomplete? value) => value == null ? null : _autocompleteValues[value.Value];
    }

    public enum Position
    {
        Above,
        Below
    }

    internal static class PositionExtensions
    {
        private static Dictionary<Position, string> _positionValues =
            Enum.GetValues<Position>().ToDictionary(id => id, id => Enum.GetName(id).ToLowerInvariant());

        public static string ToAttributeValue(this Position? value) => value == null ? null : _positionValues[value.Value];
    }
}
