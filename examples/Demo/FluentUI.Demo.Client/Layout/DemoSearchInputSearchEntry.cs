// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.RegularExpressions;
using FluentUI.Demo.DocViewer.Models;
using Markdig;

namespace FluentUI.Demo.Client.Layout;

/// <summary>
/// Represents searchable content extracted from a documentation page.
/// </summary>
internal sealed record DemoSearchInputSearchEntry
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DemoSearchInputSearchEntry"/> class.
    /// </summary>
    /// <param name="page">The documentation page to index.</param>
    public DemoSearchInputSearchEntry(Page page)
    {
        var lines = GetSearchableLines(page.Content, page.Title);
        var description = lines.FirstOrDefault() ?? string.Empty;

        Title = page.Title;
        Route = page.Route;
        Description = Truncate(description, DemoSearchInputSearchResult.MaximumDescriptionLength);
        Content = string.Join(' ', lines);

        if (page.Category.Key == DemoSearchInputSearchResult.DefaultCategory)
        {
            var route = page.Route.Replace("/[Default]", "", StringComparison.InvariantCultureIgnoreCase);
            var depth = route.Count(c => c == '/'); // Number of "/" in the route

            if (depth == 1)
            {
                DefaultOrder = page.Order;
            }
        }
    }

    /// <summary>
    /// Gets the title of the documentation page.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the default order of the documentation page when it is in the default category and at the top level.
    /// Used to display the pages when no criteria is entered in the search input.
    /// </summary>
    public string? DefaultOrder { get; }

    /// <summary>
    /// Gets the route of the documentation page.
    /// </summary>
    public string Route { get; }

    /// <summary>
    /// Gets the shortened description of the documentation page.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the searchable plain-text content of the documentation page.
    /// </summary>
    public string Content { get; }

    /// <summary>
    /// Creates a shortened content snippet centered around a search match.
    /// </summary>
    /// <param name="content">The content containing the search match.</param>
    /// <param name="matchIndex">The zero-based index of the search match.</param>
    /// <param name="matchLength">The length of the search match.</param>
    /// <returns>A context snippet containing the search match.</returns>
    public static string CreateContextSnippet(string content, int matchIndex, int matchLength)
    {
        if (content.Length <= DemoSearchInputSearchResult.MaximumDescriptionLength)
        {
            return content;
        }

        var omittedPrefix = matchIndex > 0;
        var omittedSuffix = matchIndex + matchLength < content.Length;
        var ellipsisLength = (omittedPrefix ? 3 : 0) + (omittedSuffix ? 3 : 0);
        var contentLength = DemoSearchInputSearchResult.MaximumDescriptionLength - ellipsisLength;
        var start = Math.Max(0, matchIndex - ((contentLength - matchLength) / 2));
        var end = Math.Min(content.Length, start + contentLength);

        if (end == content.Length)
        {
            start = Math.Max(0, end - contentLength);
        }

        start = MoveToWordStart(content, start);
        if (end - start > contentLength)
        {
            end = start + contentLength;
        }

        end = MoveToWordStart(content, end);
        if (end <= matchIndex + matchLength)
        {
            end = Math.Min(content.Length, start + contentLength);
        }

        var snippet = content[start..end].Trim();
        return $"{(start > 0 ? "..." : string.Empty)}{snippet}{(end < content.Length ? "..." : string.Empty)}";
    }

    /// <summary>
    /// Gets the searchable lines of the documentation page content, excluding the title and any DocViewer directives.
    /// </summary>
    /// <param name="markdown">The documentation page content in Markdown format.</param>
    /// <param name="title">The documentation page title to exclude from the searchable content.</param>
    /// <returns>The normalized lines that can be searched.</returns>
    private static IEnumerable<string> GetSearchableLines(string markdown, string title)
    {
        var plainText = Markdown.ToPlainText(markdown);
        var lines = plainText
            .ReplaceLineEndings("\n")
            .Split('\n')
            .Select(NormalizeWhitespace)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Where(line => !IsDocViewerDirective(line))
            .ToList();

        if (lines.Count > 0 && string.Equals(lines[0], title, StringComparison.OrdinalIgnoreCase))
        {
            lines.RemoveAt(0);
        }

        return lines;
    }

    /// <summary>
    /// Truncates text to the specified maximum length without splitting a word.
    /// </summary>
    /// <param name="text">The text to truncate.</param>
    /// <param name="maximumLength">The maximum length of the returned text.</param>
    /// <returns>The original text or a truncated version followed by an ellipsis.</returns>
    private static string Truncate(string text, int maximumLength)
    {
        if (text.Length <= maximumLength)
        {
            return text;
        }

        var end = MoveToWordStart(text, maximumLength);
        return $"{text[..end].TrimEnd()}...";
    }

    /// <summary>
    /// Moves an index backward to the beginning of the current word.
    /// </summary>
    /// <param name="text">The text containing the index.</param>
    /// <param name="index">The index from which to move backward.</param>
    /// <returns>The index at the beginning of the current word.</returns>
    private static int MoveToWordStart(string text, int index)
    {
        while (index > 0 && index < text.Length && !char.IsWhiteSpace(text[index - 1]))
        {
            index--;
        }

        return index;
    }

    /// <summary>
    /// Replaces consecutive whitespace characters with a single space and trims the result.
    /// </summary>
    /// <param name="text">The text to normalize.</param>
    /// <returns>The normalized text.</returns>
    private static string NormalizeWhitespace(string text) => Regex.Replace(text, @"\s+", " ").Trim();

    /// <summary>
    /// Determines whether text contains a DocViewer directive.
    /// </summary>
    /// <param name="text">The text to inspect.</param>
    /// <returns><see langword="true"/> when the text is a DocViewer directive; otherwise, <see langword="false"/>.</returns>
    private static bool IsDocViewerDirective(string text) => text.StartsWith("{{", StringComparison.Ordinal) &&
                                                             text.EndsWith("}}", StringComparison.Ordinal);

}