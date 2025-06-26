// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Splits the text into fragments, according to the text to be highlighted
/// </summary>
/// <remarks>Inspired from https://github.com/MudBlazor</remarks>
internal sealed class HighlighterSplitter
{
    private static readonly TimeSpan _regExMatchTimeout = TimeSpan.FromMilliseconds(100);
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

        var builder = GetStringBuilder();
        regex = BuildRegexPattern(builder, highlightedTexts, untilNextBoundary);

        if (string.IsNullOrEmpty(regex))
        {
            return new string[] { text };
        }

        var splits = SplitText(text, regex, caseSensitive);
        return FilterEmptyFragments(splits);
    }

    /// <summary />
    private static StringBuilder GetStringBuilder()
    {
        return Interlocked.Exchange(ref _stringBuilderCached, value: null) ?? new StringBuilder();
    }

    /// <summary />
    private static string BuildRegexPattern(StringBuilder builder, IEnumerable<string> highlightedTexts, bool untilNextBoundary)
    {
        builder.Append("((?:");
        var hasAtLeastOnePattern = false;

        if (highlightedTexts is not null)
        {
            foreach (var substring in highlightedTexts)
            {
                if (string.IsNullOrEmpty(substring))
                {
                    continue;
                }

                if (hasAtLeastOnePattern)
                {
                    builder.Append(")|(?:");
                }

                AppendPattern(builder, substring, untilNextBoundary);
                hasAtLeastOnePattern = true;
            }
        }

        if (hasAtLeastOnePattern)
        {
            builder.Append("))");
        }
        else
        {
            builder.Clear();
            _stringBuilderCached = builder;
            return string.Empty;
        }

        var regex = builder.ToString();
        builder.Clear();
        _stringBuilderCached = builder;
        return regex;
    }

    /// <summary />
    private static void AppendPattern(StringBuilder builder, string value, bool untilNextBoundary)
    {
        value = Regex.Escape(value);
        builder.Append(value);
        if (untilNextBoundary)
        {
            builder.Append(NextBoundary);
        }
    }

    /// <summary />
    private static string[] SplitText(string text, string regex, bool caseSensitive)
    {
        return Regex.Split(text, regex, caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase, _regExMatchTimeout);
    }

    /// <summary />
    private static Memory<string> FilterEmptyFragments(string[] splits)
    {
        var length = 0;
        for (var i = 0; i < splits.Length; i++)
        {
            if (!string.IsNullOrEmpty(splits[i]))
            {
                splits[length++] = splits[i];
            }
        }

        Array.Clear(splits, length, splits.Length - length);
        return splits.AsMemory(0, length);
    }
}
