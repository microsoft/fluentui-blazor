// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace FluentUI.Demo.Client.Documentation.Theme;

internal class TokenItem
{
    private const string DEFAULT_SECTION = "Default";
    private string? _mainSection;
    private string? _subSection;

    public string Name { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public object Value { get; set; } = string.Empty;

    public string ValueAsString
    {
        get => Value?.ToString() ?? string.Empty;
        set => Value = value;
    }

    public string MainSection => _mainSection ??= GetMainSectionName(Name);

    public string SubSection => _subSection ??= GetSubSectionName(Name);

    private static string GetMainSectionName(string value)
    {
        var pattern = @"^[a-z]+";
        var match = Regex.Match(value, pattern);

        return match.Success
            ? CapitalizeFirstLetter(match.Value)
            : DEFAULT_SECTION;
    }

    private static string GetSubSectionName(string value)
    {
        var pattern = @"^[a-z]+([A-Z][a-z]+)";
        var match = Regex.Match(value, pattern);

        return match.Success && match.Groups.Count > 1
            ? CapitalizeFirstLetter(match.Groups[1].Value)
            : DEFAULT_SECTION;
    }

    public static string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        var firstChar = char.ToUpper(input[0]);
        var restOfString = input.Substring(1);

        return firstChar + restOfString;
    }
}
