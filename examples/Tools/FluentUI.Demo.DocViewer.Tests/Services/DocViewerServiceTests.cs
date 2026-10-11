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
    [InlineData("MigrationFluentButton.md")]
    [InlineData("MigrationFluentAutocomplete.md")]
    public void LoadResource_MigrationWithoutHeaders_RendersTable(string resourceName)
    {
        var service = new DocViewerService(new DocViewerOptions { ResourcesAssembly = typeof(DocViewerServiceTests).Assembly });

        var content = service.LoadResource(resourceName, isFullName: false, removeHeaders: true);
        var html = Markdown.ToHtml(content, MarkdownPipeline);

        Assert.DoesNotContain("title:", content);
        Assert.Contains("<table>", html);
        Assert.Contains("<thead>", html);
        Assert.Contains("<tbody>", html);
    }

    [Fact]
    public void LoadResource_WithHeaders_PreservesMarkdownBody()
    {
        var service = new DocViewerService(new DocViewerOptions { ResourcesAssembly = typeof(DocViewerServiceTests).Assembly });
        var original = service.LoadResource("WithHeaders.md", isFullName: false);

        var content = service.LoadResource("WithHeaders.md", isFullName: false, removeHeaders: true);

        Assert.StartsWith("---", original);
        Assert.Equal(original[original.IndexOf("# Content", StringComparison.Ordinal)..], content);
    }

    [Theory]
    [InlineData("WithoutHeaders.md")]
    [InlineData("UnclosedHeaders.md")]
    public void LoadResource_WithoutCompleteLeadingHeaders_PreservesContent(string resourceName)
    {
        var service = new DocViewerService(new DocViewerOptions { ResourcesAssembly = typeof(DocViewerServiceTests).Assembly });
        var original = service.LoadResource(resourceName, isFullName: false);

        var content = service.LoadResource(resourceName, isFullName: false, removeHeaders: true);

        Assert.NotEmpty(original);
        Assert.Equal(original, content);
    }
}
