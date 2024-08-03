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
        var id = Identifier.NewId(length);

        Assert.StartsWith("f", id);
        Assert.Equal(length, id.ToString().Length);
    }

    [Fact]
    public void Identifier_LenghtTooLong()
    {
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

        Assert.Equal("0000", id0);
        Assert.Equal("0001", id1);
        Assert.Equal("0002", id2);
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
}
