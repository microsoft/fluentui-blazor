// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Text.RegularExpressions;
using FluentUI.Demo.DocViewer.Services;

namespace FluentUI.Demo.DocViewer.Models;

/// <summary>
/// Represents a section of a markdown document.
/// </summary>
public record Section
{
    private const string DEFAULT_LANGUAGE = "text";
    private static readonly Random _rnd = new();

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "For a future usage")]
    private readonly DocViewerService _docViewerService;

    /// <summary>
    /// Key for the language argument, used by the <see cref="Arguments"/> dictionary.
    /// </summary>
    public const string ARGUMENT_LANGUAGE = "language";

    /// <summary>
    /// Key to indicate that the Component should not include the source code (SourceCode="false").
    /// </summary>
    public const string ARGUMENT_SOURCECODE = "sourcecode";

    /// <summary>
    /// Key for the extra files displayed in the Component.
    /// </summary>
    public const string ARGUMENT_EXTRA_FILES = "files";

    /// <summary>
    /// Initializes a new instance of the <see cref="Section"/> class.
    /// </summary>
    /// <param name="docViewerService"></param>
    internal Section(DocViewerService docViewerService)
    {
        _docViewerService = docViewerService;
    }

    /// <summary>
    /// Get the unique identifier of the section.
    /// </summary>
    public string Id { get; private set; } = _rnd.Next().ToString("x", System.Globalization.CultureInfo.InvariantCulture);

    /// <summary>
    /// Gets the content of the section or the name of the component.
    /// </summary>
    public string Value { get; private set; } = string.Empty;

    /// <summary>
    /// Gets True if the contains SourceCode="false" in the arguments.
    /// </summary>
    public bool NoCode => Arguments.TryGetValue(ARGUMENT_SOURCECODE, out var sourceCodeValue) && sourceCodeValue.Equals("false", StringComparison.CurrentCultureIgnoreCase);

    /// <summary>
    /// Gets the files to display in extra tabs in the Component.
    /// The format is "Tab1=File1;Tab2=File2".
    /// </summary>
    public IDictionary<string, string> ExtraFiles
    {
        get
        {
            if (Arguments.TryGetValue(ARGUMENT_EXTRA_FILES, out var extraFileValue))
            {
                var files = extraFileValue.Split(';');
                return files.Select(f => f.Split(':')).ToDictionary(f => f[0].Trim(), f => f[1].Trim());
            }

            return new Dictionary<string, string>();
        }
    }

    /// <summary>
    /// Gets the parameters of the section. All keys are in lowercase.
    /// </summary>
    public IDictionary<string, string> Arguments { get; private set; } = new Dictionary<string, string>();

    /// <summary>
    /// Gets the type of the section.
    /// </summary>
    public SectionType Type { get; private set; } = SectionType.Html;

    /// <summary>
    /// Reads the content of the section asynchronously.
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public Task<Section> ReadAsync(string content)
    {
        // Code section
        if (content.StartsWith("<pre><code", StringComparison.InvariantCultureIgnoreCase))
        {
            var regex = new Regex(@"language-(\w+)"); // Extract the Language name
            var match = regex.Match(content);

            var language = match.Success ? match.Groups[1].Value : DEFAULT_LANGUAGE;

            Value = Regex.Replace(content, @"<\/?(pre|code)[^>]*>", string.Empty);
            Type = SectionType.Code;
            Arguments = new Dictionary<string, string>
            {
                { ARGUMENT_LANGUAGE, language }
            };
        }

        // API or Component section
        else if (content.StartsWith("{{") && content.EndsWith("}}"))
        {
            var component = ParseComponent(content);

            // API
            if (string.Compare(component.Name, "API", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                Arguments = component.Arguments;
                Value = string.Join(';', component.Arguments.Select(i => $"{i.Key}={i.Value}"));
                Type = SectionType.Api;
            }

            // Component
            else
            {
                Arguments = component.Arguments;
                Value = component.Name;
                Type = SectionType.Component;
            }
        }

        // Text / HTML section
        else
        {
            Value = content;
            Type = SectionType.Html;
            Arguments = new Dictionary<string, string>();
        }

        return Task.FromResult(this);
    }

    /// <summary>
    /// Gets the value of the argument with the specified key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string? GetArgumentValue(string key)
    {
        return Arguments.TryGetValue(key.ToLower(), out var value) ? value : null;
    }

    /// <summary />
    private static (string Name, Dictionary<string, string> Arguments) ParseComponent(string input)
    {
        var dict = new Dictionary<string, string>();
        var componentRegex = new Regex(@"\{\{\s*(\w+)\s*(.*?)\s*\}\}");
        var match = componentRegex.Match(input);

        var componentName = match.Groups[1].Value;
        var argumentsPart = match.Groups[2].Value;

        var argRegex = new Regex(@"(\w+)\s*=\s*(""[^""]*""|\S+)");
        var matches = argRegex.Matches(argumentsPart);

        foreach (Match argMatch in matches)
        {
            var key = argMatch.Groups[1].Value.ToLower();
            var value = argMatch.Groups[2].Value.Trim('"');
            dict[key] = value;
        }

        return (componentName, dict);
    }
}
