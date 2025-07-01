// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.DocViewer.Tests.Extensions;

internal static class StringExtensions
{
    /// <summary>
    /// Remove all leading blanks from each line in the input string.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string RemoveLeadingBlanks(this string input)
    {
        var lines = input.Split(new[] { Environment.NewLine, "\r", "\n" }, StringSplitOptions.None);
        for (var i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].TrimStart();
        }

        return string.Join(Environment.NewLine, lines);
    }
}
