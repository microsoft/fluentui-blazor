// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.Client.Layout;

/// <summary>
/// Represents a documentation search result and its match location.
/// </summary>
internal sealed record DemoSearchInputSearchResult
{
    public const string DefaultCategory = "10";
    public const int MaximumDescriptionLength = 200;
    public const int MaximumResults = 9;

    /// <summary>
    /// Initializes a new instance of the <see cref="DemoSearchInputSearchResult"/> class.
    /// </summary>
    /// <param name="title">The title of the matched documentation page.</param>
    /// <param name="route">The route of the matched documentation page.</param>
    /// <param name="description">The shortened description of the matched documentation page.</param>
    /// <param name="context">The optional content snippet containing the search match.</param>
    /// <param name="matchKind">The part of the documentation page containing the search match.</param>
    /// <param name="matchIndex">The zero-based index of the search match.</param>
    public DemoSearchInputSearchResult(string title, string route, string description, string? context, SearchMatchKind matchKind, int matchIndex)
    {
        Title = title;
        Route = route;
        Description = description;
        Context = context;
        MatchKind = matchKind;
        MatchIndex = matchIndex;
    }

    /// <summary>
    /// Gets the title of the matched documentation page.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the route of the matched documentation page.
    /// </summary>
    public string Route { get; }

    /// <summary>
    /// Gets the shortened description of the matched documentation page.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the optional content snippet containing the search match.
    /// </summary>
    public string? Context { get; }

    /// <summary>
    /// Gets the part of the documentation page containing the search match.
    /// </summary>
    public SearchMatchKind MatchKind { get; }

    /// <summary>
    /// Gets the zero-based index of the search match.
    /// </summary>
    public int MatchIndex { get; }

    /// <summary>
    /// Creates the default search results for the specified documentation entries.
    /// </summary>
    /// <param name="entries">The documentation entries to search.</param>
    /// <returns>The default documentation entries ordered by category, order, and title.</returns>
    public static IEnumerable<DemoSearchInputSearchResult> CreateDefaultResults(IEnumerable<DemoSearchInputSearchEntry> entries)
    {
        return entries.Where(i => i.DefaultOrder is not null)
                              .OrderBy(entry => entry.DefaultOrder)
                              .Select(entry => CreateSearchResult(entry, " "))
                              .OfType<DemoSearchInputSearchResult>()
                              .Take(MaximumResults);
    }

    /// <summary>
    /// Creates the ordered search results for the specified search text.
    /// </summary>
    /// <param name="entries">The documentation entries to search.</param>
    /// <param name="searchText">The text to find in the documentation entries.</param>
    /// <returns>The matching documentation entries ordered by match kind, index, and title.</returns>
    public static IEnumerable<DemoSearchInputSearchResult> CreateSearchResults(IEnumerable<DemoSearchInputSearchEntry> entries, string searchText)
    {
        return entries.Select(entry => CreateSearchResult(entry, searchText))
                      .OfType<DemoSearchInputSearchResult>()
                      .OrderBy(result => result?.MatchKind)
                      .ThenBy(result => result.MatchIndex)
                      .ThenBy(result => result.Title, StringComparer.OrdinalIgnoreCase)
                      .Take(MaximumResults);
    }

    /// <summary>
    /// Creates a search result for the first matching field of a documentation entry.
    /// </summary>
    /// <param name="entry">The documentation entry to search.</param>
    /// <param name="searchText">The text to find in the documentation entry.</param>
    /// <returns>The search result when a match is found; otherwise, <see langword="null"/>.</returns>
    private static DemoSearchInputSearchResult? CreateSearchResult(DemoSearchInputSearchEntry entry, string searchText)
    {
        var titleIndex = entry.Title.IndexOf(searchText, StringComparison.OrdinalIgnoreCase);
        if (titleIndex >= 0)
        {
            return new DemoSearchInputSearchResult(entry.Title, entry.Route, entry.Description, null, SearchMatchKind.Title, titleIndex);
        }

        var descriptionIndex = entry.Description.IndexOf(searchText, StringComparison.OrdinalIgnoreCase);
        if (descriptionIndex >= 0)
        {
            return new DemoSearchInputSearchResult(entry.Title, entry.Route, entry.Description, null, SearchMatchKind.Description, descriptionIndex);
        }

        var contentIndex = entry.Content.IndexOf(searchText, StringComparison.OrdinalIgnoreCase);
        if (contentIndex < 0)
        {
            return null;
        }

        return new DemoSearchInputSearchResult(
            entry.Title,
            entry.Route,
            entry.Description,
            DemoSearchInputSearchEntry.CreateContextSnippet(entry.Content, contentIndex, searchText.Length),
            SearchMatchKind.Content,
            contentIndex);
    }

    /// <summary>
    /// Specifies the part of a documentation page containing a search match.
    /// </summary>
    public enum SearchMatchKind
    {
        /// <summary>
        /// The search text matches the page title.
        /// </summary>
        Title,

        /// <summary>
        /// The search text matches the page description.
        /// </summary>
        Description,

        /// <summary>
        /// The search text matches the page content.
        /// </summary>
        Content
    }
}