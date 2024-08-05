// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer.Models;
using FluentUI.Demo.DocViewer.Tests.Services;
using Xunit;

namespace FluentUI.Demo.DocViewer.Tests;

public class SectionTests
{
    private readonly FactoryServiceTests FactoryService = new();

    [Fact]
    public async Task Section_CodeSection()
    {
        var section = new Section(FactoryService);
        var content = @"<pre><code class=""language-razor"">Example</code></pre>";

        await section.ReadAsync(content);

        Assert.Equal(SectionType.Code, section.Type);
        Assert.Equal("razor", section.Arguments[Section.ARGUMENT_LANGUAGE]);
        Assert.Equal("Example", section.Value);
    }

    [Fact]
    public async Task Section_APISection()
    {
        var section = new Section(FactoryService);
        var content = @"{{ API Arg1=Value1   Arg2 = Value2 Arg3=""Value 3""  }}";

        await section.ReadAsync(content);

        Assert.Equal(SectionType.Api, section.Type);
        Assert.Equal("Value1", section.Arguments["arg1"]);
        Assert.Equal("Value2", section.Arguments["arg2"]);
        Assert.Equal("Value 3", section.Arguments["arg3"]);
        Assert.Equal("arg1=Value1;arg2=Value2;arg3=Value 3", section.Value);
    }

    [Fact]
    public async Task Section_ComponentSection()
    {
        var section = new Section(FactoryService);
        var content = @"{{ MY_COMPONENT }}";

        await section.ReadAsync(content);

        Assert.Equal(SectionType.Component, section.Type);
        Assert.Equal("MY_COMPONENT", section.Value);
        Assert.Empty(section.Arguments);
    }

    [Fact]
    public async Task Section_ComponentWithArguments()
    {
        var section = new Section(FactoryService);
        var content = @"{{ MY_COMPONENT Arg1=Value1   Arg2 = Value2 Arg3=""Value 3""  }}";

        await section.ReadAsync(content);

        Assert.Equal(SectionType.Component, section.Type);
        Assert.Equal("Value1", section.Arguments["arg1"]);
        Assert.Equal("Value2", section.Arguments["arg2"]);
        Assert.Equal("Value 3", section.Arguments["arg3"]);
        Assert.Equal("MY_COMPONENT", section.Value);
    }
}
