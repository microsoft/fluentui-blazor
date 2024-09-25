// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer.Models;
using FluentUI.Demo.DocViewer.Tests.Extensions;
using FluentUI.Demo.DocViewer.Tests.Services;
using Xunit;

namespace FluentUI.Demo.DocViewer.Tests;

public class PageTests
{
    private readonly DocViewerServiceTests DocViewerService = new();

    [Fact]
    public void Page_HeaderContent()
    {
        var fileContent = @"---
                           title: Button
                           route: /Button
                           ---

                           My content";

        var page = new Page(DocViewerService, "file.md", fileContent.RemoveLeadingBlanks());

        Assert.Equal("file.md", page.ResourceName);
        Assert.Equal("Button", page.Title);
        Assert.Equal("/Button", page.Route);
        Assert.Equal("My content", page.Content);
    }

    [Fact]
    public void Page_Headers()
    {
        var fileContent = @"---
                           Header1: Value1
                           Header2: Value2
                           ---";

        var page = new Page(DocViewerService, "file.md", fileContent.RemoveLeadingBlanks());

        Assert.Equal(2, page.Headers.Count);
        Assert.Equal("Value1", page.Headers["Header1"]);
        Assert.Equal("Value2", page.Headers["Header2"]);
    }

    [Fact]
    public void Page_Content()
    {
        var fileContent = @"My content";

        var page = new Page(DocViewerService, "file.md", fileContent.RemoveLeadingBlanks());

        Assert.Empty(page.Headers);
        Assert.Equal("My content", page.Content);
    }

    [Fact]
    public void Page_Empty()
    {
        var fileContent = @"";

        var page = new Page(DocViewerService, "file.md", fileContent.RemoveLeadingBlanks());

        Assert.Empty(page.Headers);
        Assert.Empty(page.Content);
    }

    [Fact]
    public void Page_HtmlHeaders()
    {
        var fileContent = @"---
                           title: Button
                           route: /Button
                           ---

                           # Level 1

                           ## Level 2

                           ### Level 3

                           ## Level 2

                           # Level 1

                           My content";

        var page = new Page(DocViewerService, "file.md", fileContent.RemoveLeadingBlanks());
        var htmlHeaders = page.GetHtmlHeaders()?.ToArray() ?? [];

        Assert.Equal(5, htmlHeaders.Length);

        Assert.Equal("Level 1", htmlHeaders[0].Title);
        Assert.Equal("level-1", htmlHeaders[0].Id);
        Assert.Equal("/Button#level-1", htmlHeaders[0].AnchorId);

        Assert.Equal("Level 3", htmlHeaders[2].Title);
        Assert.Equal("level-3", htmlHeaders[2].Id);
        Assert.Equal("/Button#level-3", htmlHeaders[2].AnchorId);
    }

    [Fact]
    public async Task Page_Sections()
    {
        var fileContent = @"---
                           title: Button
                           route: /Button
                           ---

                           # Section 1
                           My content

                           My content

                           ```csharp
                           My code
                           ```";

        var page = new Page(DocViewerService, "file.md", fileContent.RemoveLeadingBlanks());
        var sections = await page.ExtractSectionsAsync();

        Assert.Equal(SectionType.Html, sections.ElementAt(0).Type);
        Assert.Equal(SectionType.Code, sections.ElementAt(1).Type);
    }
}
