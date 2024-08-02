// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer.Services;

namespace FluentUI.Demo.DocViewer.Models;

public record Section
{
    private const string DEFAULT_LANGUAGE = "text";
    private static readonly Random _rnd = new();

    public const string ARGUMENT_LANGUAGE = "Language";

    /// <summary>
    /// Initializes a new instance of the <see cref="Section"/> class.
    /// </summary>
    /// <param name="content"></param>
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
            Arguments = new Dictionary<string, string>();
        }

        // Text / HTML section
        else
        {
            Value = content;
            Type = SectionType.Html;
            Arguments = new Dictionary<string, string>();
        }
    }

    /// <summary>
    /// Get the unique identifier of the section.
    /// </summary>
    public string Id { get; private set; } = _rnd.Next().ToString("x", System.Globalization.CultureInfo.InvariantCulture);

    /// <summary>
    /// Gets the content of the section or the name of the component.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Gets the source code of the component (if <see langword="type"/> is Component).
    /// </summary>
    public string SourceCode { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the parameters of the section.
    /// </summary>
    public IDictionary<string, string> Arguments { get; }

    /// <summary>
    /// Gets the type of the section.
    /// </summary>
    public SectionType Type { get; } = SectionType.Html;

    internal async Task LoadStaticAssetsAsync(StaticAssetService assetService, DocViewerService viewerService)
    {
        switch (Type)
        {
            case SectionType.Component:

                var url = string.Format(System.Globalization.CultureInfo.InvariantCulture, viewerService.Options.SourceCodeUrl, Value);
                var code = await assetService.GetAsync(url);

                if (!string.IsNullOrEmpty(code))
                {
                    SourceCode = code;
                    Id += "-src";
                }

                break;

            default:
                break;
        }
    }
}
