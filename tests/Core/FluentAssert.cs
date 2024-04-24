using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using Bunit;
using Bunit.Rendering;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests;

/* Add this configuration in Unit Tests .csproj file.
   to display all .verified and .received json files under the test class.

	<ItemGroup>

		<!-- Dependent Test Result HTML files -->
		<None Include="**\*.verified.html" />
		<None Update="**\*.verified.html">
			<ParentFile>$([System.String]::Copy('%(FileName)').Split('.')[0])</ParentFile>
			<DependentUpon>%(ParentFile).cs</DependentUpon>
		</None>
		<None Include="**\*.received.html" />
		<None Update="**\*.received.html">
			<ParentFile>$([System.String]::Copy('%(FileName)').Split('.')[0])</ParentFile>
			<DependentUpon>%(ParentFile).cs</DependentUpon>
		</None>

	</ItemGroup>
 */

/// <summary>
/// Fluent Blazor Unit Tests methods
/// </summary>
public static class FluentAssert
{
    private const string UndefinedSuffix = "[#~UNDEFINED~#]";
    public static readonly FluentAssertOptions Options = new();

    /// <summary>
    /// Verifies that the rendered markup from the <paramref name="actual"/> <see cref="IRenderedFragment"/> matches
    /// the ".verified.html" file content, using the <see cref="Bunit.Diffing.HtmlComparer"/> type.
    /// If the contents are not the same, a new ".received.html" file is created to allow comparison.
    /// ".received.html" and ".verified.html" extension is configurable using <see cref="Options"/>.
    /// </summary>
    /// <param name="actual"></param>
    /// <param name="received"></param>
    /// <param name="filename"></param>
    /// <param name="memberName"></param>
    /// <param name="suffix"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void Verify(this IRenderedFragment? actual,
        Func<string, string>? received = null,
        [CallerFilePath] string? filename = "",
        [CallerMemberName] string? memberName = "",
        string? suffix = UndefinedSuffix)
    {
        if (actual is null)
        {
            return;
        }

        // Valid?
        ArgumentNullException.ThrowIfNull(filename, nameof(filename));
        ArgumentNullException.ThrowIfNull(memberName, nameof(memberName));

        // Files
        var file = new FileInfo(filename);
        var isRazor = file.Extension == ".razor";
        var memberFullName = GetMemberFullName(memberName, suffix);
        var expectedFile = file.GetTargetFile(memberFullName, isRazor ? Options.VerifiedRazorExtension : Options.VerifiedCSharpExtension);
        var receivedFile = file.GetTargetFile(memberFullName, isRazor ? Options.ReceivedRazorExtension : Options.ReceivedCSharpExtension);
        var htmlParser = actual.Services.GetRequiredService<BunitHtmlParser>();

        // Load "verified.html" file
        var expectedHtml = expectedFile.Exists
                         ? File.ReadAllText(expectedFile.FullName)
                         : string.Empty;
        expectedHtml = Options.ScrubLinesWithReplace(expectedHtml);
        var expectedNodes = expectedHtml.ToNodeList(htmlParser);

        // Get actual Markup
        var receivedHtml = received == null
                         ? actual.Markup
                         : received.Invoke(actual.Markup);
        receivedHtml = Options.ScrubLinesWithReplace(receivedHtml);
        var receivedNodes = receivedHtml.ToNodeList(htmlParser);

        // Difference?
        var diffs = receivedNodes.CompareTo(expectedNodes)
                                 .Where(i => !Options.IsExcluded(i));

        // Delete a previous "received.html" file
        if (!diffs.Any())
        {
            if (receivedFile.Exists)
            {
                receivedFile.Delete();
            }
        }

        // Create a "received.json" file
        else if (Options.UpdateVerifiedFiles)
        {
            var formattedReceivedHtml = NodePrintExtensions.ToMarkup((IEnumerable<INode>)receivedNodes);
            File.WriteAllText(expectedFile.FullName, formattedReceivedHtml);
        }

        // Create a "received.json" file
        else
        {
            var formattedReceivedHtml = NodePrintExtensions.ToMarkup((IEnumerable<INode>)receivedNodes);
            File.WriteAllText(receivedFile.FullName, formattedReceivedHtml);
            throw new HtmlEqualException(diffs, expectedNodes, receivedNodes, null);
        }
    }

    private static string GetMemberFullName(string memberName, string? suffix)
    {
        if (suffix == UndefinedSuffix)
        {
            return memberName;
        }
        else if (suffix == null)
        {
            return $"{memberName}-[null]";
        }
        else if (suffix == string.Empty)
        {
            return $"{memberName}-[empty]";
        }
        else
        {
            var suffixFormatted = suffix.Replace(" ", "[space]")
                                        .Replace("/", "-")
                                        .Replace("\\", "-");
            return $"{memberName}-{suffixFormatted}";
        }
    }

    private static INodeList ToNodeList(this string markup, BunitHtmlParser? htmlParser)
    {
        if (htmlParser is null)
        {
            using var newHtmlParser = new BunitHtmlParser();
            return newHtmlParser.Parse(markup);
        }

        return htmlParser.Parse(markup);
    }

    private static FileInfo GetTargetFile(this FileInfo file, string memberName, string extension)
    {
        return new FileInfo(Path.Combine(file.DirectoryName ?? string.Empty, $"{file.NameWithoutExtension()}.{memberName}{extension}"));
    }

    private static string NameWithoutExtension(this FileInfo file)
    {
        var nameLength = (int)file.Name.Length - file.Extension.Length;

        if (nameLength > 0)
        {
            return file.Name.Substring(0, nameLength);
        }

        return file.Name;
    }

    public static string RemoveEmptyComments(this string value)
    {
        return value.Replace("<!--!-->", string.Empty);
    }

    public static string RemoveAttribute(this string value, string attribute)
    {
        return ReplaceAttribute(value, attribute, string.Empty);
    }

    public static string ReplaceAttribute(this string value, string attribute, string? newValue = "")
    {
        var newAttributeValue = string.IsNullOrEmpty(newValue) ? string.Empty : $" {attribute}=\"{newValue}\"";
        value = Regex.Replace(value, $" {attribute}='[\\w-]+'", newAttributeValue, RegexOptions.IgnoreCase);
        value = Regex.Replace(value, $" {attribute}=\"[\\w-]+\"", newAttributeValue, RegexOptions.IgnoreCase);
        return value;
    }
}
