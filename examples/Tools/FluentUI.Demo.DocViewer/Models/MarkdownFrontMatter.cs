// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace FluentUI.Demo.DocViewer.Models;

internal static partial class MarkdownFrontMatter
{
    /// <summary>
    /// Splits leading front matter from the body without changing its whitespace.
    /// </summary>
    internal static (string? Header, string Body) Split(string content)
    {
        var match = FrontMatterRegex().Match(content);

        return match.Success
            ? (match.Groups["header"].Value, content[match.Length..])
            : (null, content);
    }

    // Delimiters occupy complete lines. Recognize CR as well as LF and CRLF,
    // including an empty header and a closing delimiter at the end of the file.
    [GeneratedRegex(@"\A---[ \t]*(?:\r\n|\n|\r)(?<header>[\s\S]*?)(?<=[\r\n])---[ \t]*(?:\r\n|\n|\r|\z)")]
    private static partial Regex FrontMatterRegex();
}
