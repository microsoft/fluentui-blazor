// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer.Services;
using Markdig;
using Xunit;

namespace FluentUI.Demo.DocViewer.Tests.Services;

public class DocViewerServiceTests : DocViewerService
{
    public DocViewerServiceTests() : base(new DocViewerOptions())
    {
    }

    [Theory]
    [InlineData("\n")]
    [InlineData("\r\n")]
    public void RemoveHeaders_WithLeadingHeaders_PreservesMarkdownBody(string newLine)
    {
        var body = """
            # Content

            | Before | After |
            |---|---|
            | One | Two |

            ---

            A horizontal rule and a fenced example:

            ```yaml
            ---
            title: Example
            ---
            ```
            """.ReplaceLineEndings(newLine);
        var original = $"---{newLine}title: Before --- After{newLine}---{newLine}{body}";

        var content = RemoveHeaders(original);
        var html = Markdown.ToHtml(content, MarkdownPipeline);

        Assert.Equal(body, content);
        Assert.Contains("<table>", html);
        Assert.Contains("<thead>", html);
        Assert.Contains("<tbody>", html);
    }

    [Theory]
    [InlineData("| Before | After |\n|---|---|\n| One | Two |")]
    [InlineData("# Content\n\n---\n\nMore content\n\n---")]
    [InlineData("---\ntitle: Before --- After\n\n| Before | After |\n|---|---|\n| One | Two |")]
    public void RemoveHeaders_WithoutCompleteLeadingHeaders_PreservesContent(string content)
    {
        Assert.Equal(content, RemoveHeaders(content));
    }

    [Fact]
    public void RemoveHeaders_WithClosingDelimiterAtEnd_ReturnsEmpty()
    {
        var content = "---\ntitle: Example\n---";

        Assert.Empty(RemoveHeaders(content));
    }
}
