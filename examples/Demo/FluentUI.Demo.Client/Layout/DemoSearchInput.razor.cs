// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.RegularExpressions;
using FluentUI.Demo.DocViewer.Services;
using Markdig;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Client.Layout;

public partial class DemoSearchInput
{
    private const int MaximumDescriptionLength = 200;
    private const int MaximumResults = 9;

    private readonly List<SearchEntry> _searchEntries = [];
    private string _searchText = string.Empty;
    private SearchResult? _selectedItem;

    [Inject]
    public required DocViewerService DocViewerService { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    protected override void OnInitialized()
    {
        foreach (var page in DocViewerService.Pages.Where(page => !page.Hidden))
        {
            var lines = GetSearchableLines(page.Content, page.Title);
            var description = lines.FirstOrDefault() ?? string.Empty;

            _searchEntries.Add(new SearchEntry(
                page.Title,
                page.Route,
                Truncate(description, MaximumDescriptionLength),
                string.Join(' ', lines)));
        }
    }

    private void Search(OptionsSearchEventArgs<SearchResult> args)
    {
        _searchText = args.Text.Trim();
        if (string.IsNullOrEmpty(_searchText))
        {
            args.Items = [];
            return;
        }

        args.Items = _searchEntries
            .Select(entry => CreateSearchResult(entry, _searchText))
            .Where(result => result is not null)
            .OrderBy(result => result!.MatchKind)
            .ThenBy(result => result!.MatchIndex)
            .ThenBy(result => result!.Title, StringComparer.OrdinalIgnoreCase)
            .Take(MaximumResults)
            .Select(result => result!);
    }

    private void NavigateToSelectedPage(SearchResult? result)
    {
        if (result is null)
        {
            return;
        }

        _selectedItem = null;
        _searchText = string.Empty;
        NavigationManager.NavigateTo(result.Route);
    }

    private static SearchResult? CreateSearchResult(SearchEntry entry, string searchText)
    {
        var titleIndex = entry.Title.IndexOf(searchText, StringComparison.OrdinalIgnoreCase);
        if (titleIndex >= 0)
        {
            return new SearchResult(entry.Title, entry.Route, entry.Description, null, SearchMatchKind.Title, titleIndex);
        }

        var descriptionIndex = entry.Description.IndexOf(searchText, StringComparison.OrdinalIgnoreCase);
        if (descriptionIndex >= 0)
        {
            return new SearchResult(entry.Title, entry.Route, entry.Description, null, SearchMatchKind.Description, descriptionIndex);
        }

        var contentIndex = entry.Content.IndexOf(searchText, StringComparison.OrdinalIgnoreCase);
        if (contentIndex < 0)
        {
            return null;
        }

        return new SearchResult(
            entry.Title,
            entry.Route,
            entry.Description,
            CreateContextSnippet(entry.Content, contentIndex, searchText.Length),
            SearchMatchKind.Content,
            contentIndex);
    }

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

    private static string CreateContextSnippet(string content, int matchIndex, int matchLength)
    {
        if (content.Length <= MaximumDescriptionLength)
        {
            return content;
        }

        var omittedPrefix = matchIndex > 0;
        var omittedSuffix = matchIndex + matchLength < content.Length;
        var ellipsisLength = (omittedPrefix ? 3 : 0) + (omittedSuffix ? 3 : 0);
        var contentLength = MaximumDescriptionLength - ellipsisLength;
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

    private static string Truncate(string text, int maximumLength)
    {
        if (text.Length <= maximumLength)
        {
            return text;
        }

        var end = MoveToWordStart(text, maximumLength);
        return $"{text[..end].TrimEnd()}...";
    }

    private static int MoveToWordStart(string text, int index)
    {
        while (index > 0 && index < text.Length && !char.IsWhiteSpace(text[index - 1]))
        {
            index--;
        }

        return index;
    }

    private static string NormalizeWhitespace(string text) => Regex.Replace(text, @"\s+", " ").Trim();

    private static bool IsDocViewerDirective(string text) => text.StartsWith("{{", StringComparison.Ordinal) &&
                                                              text.EndsWith("}}", StringComparison.Ordinal);

    private sealed record SearchEntry(string Title, string Route, string Description, string Content);

    private sealed record SearchResult(
        string Title,
        string Route,
        string Description,
        string? Context,
        SearchMatchKind MatchKind,
        int MatchIndex);

    private enum SearchMatchKind
    {
        Title,
        Description,
        Content
    }
}