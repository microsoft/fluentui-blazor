// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.DocViewer.Models;

public record Section
{
    private const string DEFAULT_LANGUAGE = "text";
    private static readonly Random _rnd = new();

    public const string ARGUMENT_LANGUAGE = "Language";

    public Section(string content)
    {
        // Code section
        if (content.StartsWith("<pre><code", StringComparison.InvariantCultureIgnoreCase))
        {
            var regex = new System.Text.RegularExpressions.Regex(@"language-(\w+)"); // Extract the Language name
            var match = regex.Match(content);

            var language = match.Success ? match.Groups[1].Value : DEFAULT_LANGUAGE;

            Value = System.Text.RegularExpressions.Regex.Replace(content, @"<\/?(pre|code)[^>]*>", string.Empty);
            Type = SectionType.Code;
            Arguments = new Dictionary<string, string>
            {
                { ARGUMENT_LANGUAGE, language }
            };
        }

        // API section
        else if (content.StartsWith("{{ API") && content.EndsWith("}}"))
        {
            var arguments = content[6..^4].Trim().Split('=', ' ', StringSplitOptions.RemoveEmptyEntries);
            Value = string.Join(';', arguments);
            Type = SectionType.Api;
            Arguments = arguments.ToDictionary(i => i);
        }

        // Component section
        else if (content.StartsWith("{{") && content.EndsWith("}}"))
        {
            Value = content[2..^2].Trim();
            Type = SectionType.Component;
        }

        // Text / HTML section
        else
        {
            Value = content;
            Type = SectionType.Html;
        }
    }

    public string Id { get; } = _rnd.Next().ToString("x", System.Globalization.CultureInfo.InvariantCulture);

    public string Value { get; }

    public IDictionary<string, string>? Arguments { get; }

    public SectionType Type { get; } = SectionType.Html;
}
