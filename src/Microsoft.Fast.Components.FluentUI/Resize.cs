using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Fast.Components.FluentUI
{
    public enum Resize
    {
        Horizontal,
        Vertical,
        Both
    }

    internal static class ResizeExtensions
    {
        private static Dictionary<Resize, string> _resizeValues =
            Enum.GetValues<Resize>().ToDictionary(id => id, id => Enum.GetName(id).ToLowerInvariant());

        public static string ToAttributeValue(this Resize? value) => value == null ? null : _resizeValues[value.Value];
    }
}
