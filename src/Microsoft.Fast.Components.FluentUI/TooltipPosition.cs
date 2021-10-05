using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Fast.Components.FluentUI
{
    public enum TooltipPosition
    {
        Top,
        Bottom,
        Left,
        Right,
        Start,
        End
    }

    internal static class TooltipPositionExtensions
    {
        private static readonly Dictionary<TooltipPosition, string> _tooltipPositionValues =
            Enum.GetValues<TooltipPosition>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

        public static string? ToAttributeValue(this TooltipPosition? value) => value == null ? null : _tooltipPositionValues[value.Value];
    }
}
