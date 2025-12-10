// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using LoxSmoke.DocXml;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

/// <summary>
/// Service for reading XML documentation comments.
/// </summary>
internal sealed class XmlDocumentationReader
{
    private readonly DocXmlReader? _docXmlReader;

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlDocumentationReader"/> class.
    /// </summary>
    /// <param name="xmlDocumentationPath">Path to the XML documentation file.</param>
    public XmlDocumentationReader(string? xmlDocumentationPath)
    {
        if (!string.IsNullOrEmpty(xmlDocumentationPath) && File.Exists(xmlDocumentationPath))
        {
            _docXmlReader = new DocXmlReader(xmlDocumentationPath);
        }
    }

    /// <summary>
    /// Gets whether documentation is available.
    /// </summary>
    public bool IsAvailable => _docXmlReader != null;

    /// <summary>
    /// Gets the summary for a type from XML documentation.
    /// </summary>
    public string GetTypeSummary(Type type)
    {
        if (_docXmlReader == null)
        {
            return string.Empty;
        }

        try
        {
            var comments = _docXmlReader.GetTypeComments(type);
            return TypeHelper.CleanSummary(comments.Summary);
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Gets the summary for a member from XML documentation.
    /// </summary>
    public string GetMemberSummary(MemberInfo member)
    {
        if (_docXmlReader == null)
        {
            return string.Empty;
        }

        try
        {
            var comments = _docXmlReader.GetMemberComments(member);
            return TypeHelper.CleanSummary(comments.Summary);
        }
        catch
        {
            return string.Empty;
        }
    }
}
