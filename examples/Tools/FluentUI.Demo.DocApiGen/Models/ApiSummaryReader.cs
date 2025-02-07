// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Reflection;
using System.Text.RegularExpressions;

namespace FluentUI.Demo.DocApiGen.Models;

/// <summary>
/// Reads the summary comments from the XML documentation file.
/// </summary>
public class ApiSummaryReader
{
    private readonly LoxSmoke.DocXml.DocXmlReader _docReader;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiSummaryReader"/> class.
    /// </summary>
    /// <param name="xmlDocumentation"></param>
    public ApiSummaryReader(FileInfo xmlDocumentation)
    {
        _docReader = new LoxSmoke.DocXml.DocXmlReader(xmlDocumentation.FullName);
    }

    /// <summary>
    /// Gets the summary comments for the member.
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    public string GetMemberSummary(MemberInfo member)
    {
        var comments = _docReader.GetMemberComments(member);

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
