using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Fast.Components.FluentUI
{
    public enum NumberFieldAppearance
    {
        Filled,
        Outline
    }

    internal static class NumberFieldAppearanceExtensions
    {
        private static Dictionary<NumberFieldAppearance, string> _numberFieldAppearanceValues =
            Enum.GetValues<NumberFieldAppearance>().ToDictionary(id => id, id => Enum.GetName(id).ToLowerInvariant());

        public static string ToAttributeValue(this NumberFieldAppearance? value) => value == null ? null : _numberFieldAppearanceValues[value.Value];
    }
}
