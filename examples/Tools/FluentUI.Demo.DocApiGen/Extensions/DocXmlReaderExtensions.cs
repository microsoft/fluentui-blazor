// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Reflection;
using System.Text.RegularExpressions;

namespace FluentUI.Demo.DocApiGen.Extensions;

/// <summary>
/// Reads the summary comments from the XML documentation file.
/// </summary>
public static class DocXmlReaderExtensions
{
    /// <summary>
    /// Gets the summary comments for the component.
    /// </summary>
    /// <param name="docReader"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetComponentSummary(this LoxSmoke.DocXml.DocXmlReader docReader, Type type)
    {
        var comments = docReader.GetTypeComments(type);

        var summary = $"{comments.Summary}" +
                      $"{(string.IsNullOrEmpty(comments.Remarks) ? "" : $" _{comments.Remarks}_")}" +
                      $"{(string.IsNullOrEmpty(comments.Example) ? "" : $" Example: `{comments.Example}`")}";

        return RemoveCrLf(ConvertSeeHref(ConvertSeeCref(summary)));
    }

    /// <summary>
    /// Gets the summary comments for the member.
    /// </summary>
    /// <param name="docReader"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    public static string GetMemberSummary(this LoxSmoke.DocXml.DocXmlReader docReader, MemberInfo member)
    {
        var comments = docReader.GetMemberComments(member);

        var summary = $"{comments.Summary}" +
                      $"{(string.IsNullOrEmpty(comments.Remarks) ? "" : $" _{comments.Remarks}_")}" +
                      $"{(string.IsNullOrEmpty(comments.Example) ? "" : $" Example: `{comments.Example}`")}";

        return RemoveCrLf(ConvertSeeHref(ConvertSeeCref(summary)));
    }

    private static string ConvertSeeCref(string input)
    {
        const string pattern = @"<see(also)* cref=""[\w]:Microsoft\.FluentUI\.AspNetCore\.Components\.([\w.]+)"" />";
        const string replacement = "`$2`";

        return Regex.Replace(input, pattern, replacement);
    }

    private static string ConvertSeeHref(string input)
    {
        const string pattern = @"<see href=""([^""]+)"">([^<]+)</see>";
        const string replacement = "[$2]($1)";

        return Regex.Replace(input, pattern, replacement);
    }

    private static string RemoveCrLf(string input)
    {
        return input.Replace("\r", "").Replace("\n", "");
    }
}
