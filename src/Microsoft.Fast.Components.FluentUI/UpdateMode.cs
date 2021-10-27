using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Fast.Components.FluentUI
{
    public enum UpdateMode
    {
        Anchor,
        Auto
    }

    internal static class UpdateModeExtensions
    {
        private static readonly Dictionary<UpdateMode, string> _updateModeValues =
            Enum.GetValues<UpdateMode>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

        public static string? ToAttributeValue(this UpdateMode? value) => value == null ? null : _updateModeValues[value.Value];
    }
}
