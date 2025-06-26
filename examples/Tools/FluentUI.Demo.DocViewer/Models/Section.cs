// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
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
            Value = ReplaceMarkupKeyWord(content);
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

    /// <summary />
    private static string ReplaceMarkupKeyWord(string content)
    {
        const string NOTE_ICON = "<svg viewBox=\"0 0 16 16\"><path d=\"M8 2a6 6 0 1 1 0 12A6 6 0 0 1 8 2Zm0 1a5 5 0 1 0 0 10A5 5 0 0 0 8 3Zm0 7a.75.75 0 1 1 0 1.5.75.75 0 0 1 0-1.5Zm0-5.5a.5.5 0 0 1 .5.41V8.5a.5.5 0 0 1-1 .09V5c0-.28.22-.5.5-.5Z\" /></svg>";
        const string WARN_ICON = NOTE_ICON;
        const string TIP_ICON = "<svg viewBox=\"0 0 16 16\"><path d=\"M4.5 6.5A3.5 3.5 0 1 1 10.45 9c-.19.19-.36.43-.44.73L9.66 11H6.34l-.35-1.27c-.08-.3-.25-.54-.44-.73A3.49 3.49 0 0 1 4.5 6.5ZM6.6 12h2.8l-.18.63a.5.5 0 0 1-.48.37H7.26a.5.5 0 0 1-.48-.37L6.61 12ZM8 2a4.5 4.5 0 0 0-3.16 7.7c.1.1.16.2.19.3l.79 2.9c.17.65.77 1.1 1.44 1.1h1.48a1.5 1.5 0 0 0 1.44-1.1l.8-2.9c.02-.1.08-.2.18-.3A4.49 4.49 0 0 0 8 2Z\" /></svg>";

        content = Regex.Replace(content, @"\[!NOTE\]", $"<span keyword=\"note\">{NOTE_ICON}Note</span>", RegexOptions.IgnoreCase);
        content = Regex.Replace(content, @"\[!NOTE_ICON\]", $"<span keyword=\"note\">{NOTE_ICON}</span>", RegexOptions.IgnoreCase);

        content = Regex.Replace(content, @"\[!WARNING\]", $"<span keyword=\"warn\">{WARN_ICON}Warning</span>", RegexOptions.IgnoreCase);
        content = Regex.Replace(content, @"\[!WARNING_ICON\]", $"<span keyword=\"warn\">{WARN_ICON}</span>", RegexOptions.IgnoreCase);

        content = Regex.Replace(content, @"\[!TIP\]", $"<span keyword=\"tip\">{TIP_ICON}Tip</span>", RegexOptions.IgnoreCase);
        content = Regex.Replace(content, @"\[!TIP_ICON\]", $"<span keyword=\"tip\">{TIP_ICON}</span>", RegexOptions.IgnoreCase);

        return content;
    }
}
