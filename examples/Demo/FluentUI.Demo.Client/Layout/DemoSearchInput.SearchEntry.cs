// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer.Models;

public partial class DemoSearchInput
{
    private sealed record SearchEntry
    {
        public SearchEntry(Page page)
        {
            var lines = GetSearchableLines(page.Content, page.Title);
            var description = lines.FirstOrDefault() ?? string.Empty;

            Title = page.Title;
            Route = page.Route;
            Description = Truncate(description, MaximumDescriptionLength);
            Content = string.Join(' ', lines);
        }

        public string Title { get; }

        public string Route { get; }

        public string Description { get; }

        public string Content { get; }

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
    }
}