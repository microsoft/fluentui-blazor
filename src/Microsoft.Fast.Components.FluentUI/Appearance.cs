using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Fast.Components.FluentUI;
public enum Appearance
{
    Neutral,
    Accent,
    Hypertext,
    Lightweight,
    Outline,
    Stealth,
    Filled
}

internal static class AppearanceExtensions
{
    private static readonly Dictionary<Appearance, string> _appearanceValues =
        Enum.GetValues<Appearance>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this Appearance? value) => value == null ? null : _appearanceValues[value.Value];
}