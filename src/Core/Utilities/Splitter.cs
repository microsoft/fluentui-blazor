using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.FluentUI.AspNetCore.Components.Utilities;

/// <summary>
/// Splits the text into fragments, according to the text to be highlighted
/// </summary>
/// <remarks>Inspired from https://github.com/MudBlazor</remarks>
internal class Splitter
{
    private const string NextBoundary = ".*?\\b";

    private static StringBuilder? _stringBuilderCached;

    /// <summary>
    /// Splits the text into fragments, according to the
    /// text to be highlighted
    /// </summary>
    /// <param name="text">The whole text</param>
    /// <param name="highlightedTexts">The texts to be highlighted</param>
    /// <param name="regex">Regex expression that was used to split fragments.</param>
    /// <param name="caseSensitive">Whether it's case sensitive or not</param>
    /// <param name="untilNextBoundary">If true, splits until the next regex boundary</param>
    /// <returns></returns>
    internal static Memory<string> GetFragments(
        string text,
        IEnumerable<string> highlightedTexts,
        out string regex,
        bool caseSensitive = false,
        bool untilNextBoundary = false)
    {
        if (string.IsNullOrEmpty(text))
        {
            regex = string.Empty;
            return Memory<string>.Empty;
        }

        var builder = Interlocked.Exchange(ref _stringBuilderCached, null) ?? new();

        // the first brace in the pattern is to keep the patten when splitting,
        // the `(?:` in the pattern is to accept multiple highlightedTexts but not capture them.
        builder.Append("((?:");

        // this becomes true if `AppendPattern` was called at least once.
        var hasAtLeastOnePattern = false;
        if (highlightedTexts is not null)
        {
            foreach (var substring in highlightedTexts)
            {
                if (string.IsNullOrEmpty(substring))
                {
                    continue;
                }

                // split pattern if we already added an string to search.
                if (hasAtLeastOnePattern)
                {
                    builder.Append(")|(?:");
                }

                AppendPattern(substring);
            }
        }

        if (hasAtLeastOnePattern)
        {
            // close the last pattern group and the capture group.
            builder.Append("))");
        }
        else
        {
            builder.Clear();
            _stringBuilderCached = builder;

            // all patterns were empty or null.
            regex = string.Empty;
            return new string[] { text };
        }

        regex = builder.ToString();
        builder.Clear();
        _stringBuilderCached = builder;

        var splits = Regex.Split(text, regex, caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase);

        var length = 0;
        for (var i = 0; i < splits.Length; i++)
        {
            var s = splits[i];
            if (!string.IsNullOrEmpty(s))
            {
                splits[length++] = s;
            }
        }

        Array.Clear(splits, length, splits.Length - length);
        return splits.AsMemory(0, length);

        void AppendPattern(string value)
        {
            hasAtLeastOnePattern = true;

            // escapes the text for regex
            value = Regex.Escape(value);
            builder.Append(value);
            if (untilNextBoundary)
            {
                builder.Append(NextBoundary);
            }
        }
    }
}
