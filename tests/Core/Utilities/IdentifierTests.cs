// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Utilities;

public class IdentifierTests
{
    [Theory]
    [InlineData(4)]
    [InlineData(8)]
    [InlineData(12)]
    [InlineData(16)]
    public void Identifier_NewId(int length)
    {
        WaitUntilContextIsNull();
        var id = Identifier.NewId(length);

        Assert.StartsWith("f", id);
        Assert.Equal(length, id.ToString().Length);
    }

    [Fact]
    public void Identifier_LengthTooLong()
    {
        WaitUntilContextIsNull();
        Assert.Throws<ArgumentOutOfRangeException>(() => Identifier.NewId(17));
    }

    [Fact]
    public void Identifier_Context()
    {
        var idGenerator = (uint i) => $"{i:0000}";

        // Apply the ID Context until the end of the block
        using var context = new IdentifierContext(idGenerator);
        
        var id0 = Identifier.NewId();
        var id1 = Identifier.NewId();
        var id2 = Identifier.NewId();
        var id3 = Identifier.NewId(16);

        Assert.Equal("0000", id0);
        Assert.Equal("0001", id1);
        Assert.Equal("0002", id2);
        Assert.Equal("0003", id3);
    }

    [Fact]
    public void Identifier_SequentialContext()
    {
        // Apply the ID Context until the end of the block
        using var context = Identifier.SequentialContext();

        var id0 = Identifier.NewId();
        var id1 = Identifier.NewId();
        var id2 = Identifier.NewId();
        Assert.Equal("f0000", id0);
        Assert.Equal("f0001", id1);
        Assert.Equal("f0002", id2);
    }

    [Fact]
    public void Identifier_CurrentIndex_Reset()
    {
        // Apply the ID Context until the end of the block
        using var context = Identifier.SequentialContext();

        context.CurrentIndex = 99999999;

        var id0 = Identifier.NewId();
        var id1 = Identifier.NewId();

        Assert.Equal("f99999999", id0);
        Assert.Equal("f0000", id1);
    }

    private void WaitUntilContextIsNull()
    {
        // Wait until IdentifierContext.Current is null
        while (IdentifierContext.Current != null)
        {
            Thread.Sleep(10);
        }
    }
}
