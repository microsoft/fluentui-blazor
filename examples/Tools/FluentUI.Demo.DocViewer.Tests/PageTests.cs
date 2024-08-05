// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer.Models;
using Xunit;

namespace FluentUI.Demo.DocViewer.Tests;

public class PageTests
{
    [Fact]
    public void Page_HeaderContent()
    {
        var fileContent = @"---
                           title: Button
                           route: /Button
                           ---

                           My content";

        var page = new Page(fileContent);

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

        var page = new Page(fileContent);

        Assert.Equal(2, page.Headers.Count);
        Assert.Equal("Value1", page.Headers["Header1"]);
        Assert.Equal("Value2", page.Headers["Header2"]);
    }

    [Fact]
    public void Page_Content()
    {
        var fileContent = @"My content";

        var page = new Page(fileContent);

        Assert.Empty(page.Headers);
        Assert.Equal("My content", page.Content);
    }

    [Fact]
    public void Page_Empty()
    {
        var fileContent = @"";

        var page = new Page(fileContent);

        Assert.Empty(page.Headers);
        Assert.Empty(page.Content);
    }
}
