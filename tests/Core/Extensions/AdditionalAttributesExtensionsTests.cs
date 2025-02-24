// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Extensions;

public class AdditionalAttributesExtensionsTests
{
    [Fact]
    public void AdditionalAttributesExtensions_GetValueOrDefault_ExistingAttribute()
    {
        // Arrange
        var attributes = new Dictionary<string, object>
        {
            { "attribute1", "value1" },
            { "attribute2", "value2" }
        };
       
        // Act
        var result = attributes.GetValueOrDefault("attribute1");

        // Assert
        Assert.Equal("value1", result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("expected-value")]
    public void AdditionalAttributesExtensions_GetValueOrDefault_NotExistingAttribute(object? defaultValue)
    {
        // Arrange
        var attributes = new Dictionary<string, object>
        {
            { "attribute1", "value1" },
            { "attribute2", "value2" }
        };

        // Act
        var result = attributes.GetValueOrDefault("InvalidAttribute", defaultValue);

        // Assert
        Assert.Equal(defaultValue, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("expected-value")]
    public void AdditionalAttributesExtensions_GetValueOrDefault_NullDictionary(object? defaultValue)
    {
        // Arrange
        Dictionary<string, object>? attributes = null;

        // Act
        var result = attributes.GetValueOrDefault("attribute1", defaultValue);

        // Assert
        Assert.Equal(defaultValue, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("expected-value")]
    public void AdditionalAttributesExtensions_GetValueOrDefault_EmptyDictionary(object? defaultValue)
    {
        // Arrange
        var attributes = new Dictionary<string, object>();

        // Act
        var result = attributes.GetValueOrDefault("attribute1", defaultValue);

        // Assert
        Assert.Equal(defaultValue, result);
    }
}
