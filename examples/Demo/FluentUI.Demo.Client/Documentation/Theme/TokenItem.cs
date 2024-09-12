// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace FluentUI.Demo.Client.Documentation.Theme;

internal class TokenItem
{
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

        return match.Success ? match.Value : string.Empty;
    }

    private static string GetSubSectionName(string value)
    {
        var pattern = @"^[a-z]+([A-Z][a-z]+)";
        var match = Regex.Match(value, pattern);

        return match.Success && match.Groups.Count > 1
            ? match.Groups[1].Value
            : string.Empty;
    }
}
