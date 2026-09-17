// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Models;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Models;

/// <summary>
/// Tests for the <see cref="MethodInfo"/> record.
/// </summary>
public class MethodInfoTests
{
    [Fact]
    public void MethodInfo_WithRequiredProperties_ShouldBeCreatedSuccessfully()
    {
        // Arrange & Act
        var methodInfo = new MethodInfo
        {
            Name = "FocusAsync",
            ReturnType = "ValueTask"
        };

        // Assert
        Assert.Equal("FocusAsync", methodInfo.Name);
        Assert.Equal("ValueTask", methodInfo.ReturnType);
    }

    [Fact]
    public void MethodInfo_DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var methodInfo = new MethodInfo
        {
            Name = "FocusAsync",
            ReturnType = "ValueTask"
        };

        // Assert
        Assert.Equal(string.Empty, methodInfo.Description);
        Assert.Empty(methodInfo.Parameters);
        Assert.False(methodInfo.IsInherited);
    }

    [Fact]
    public void MethodInfo_WithAllProperties_ShouldBeCreatedSuccessfully()
    {
        // Arrange & Act
        var methodInfo = new MethodInfo
        {
            Name = "SetValueAsync",
            ReturnType = "ValueTask",
            Description = "Sets the value asynchronously",
            Parameters = ["string value", "bool notify"],
            IsInherited = false
        };

        // Assert
        Assert.Equal("SetValueAsync", methodInfo.Name);
        Assert.Equal("ValueTask", methodInfo.ReturnType);
        Assert.Equal("Sets the value asynchronously", methodInfo.Description);
        Assert.Equal(2, methodInfo.Parameters.Length);
        Assert.False(methodInfo.IsInherited);
    }

    [Fact]
    public void MethodInfo_Signature_ShouldBeGeneratedCorrectly_WithNoParameters()
    {
        // Arrange
        var methodInfo = new MethodInfo
        {
            Name = "FocusAsync",
            ReturnType = "ValueTask"
        };

        // Act
        var signature = methodInfo.Signature;

        // Assert
        Assert.Equal("ValueTask FocusAsync()", signature);
    }

    [Fact]
    public void MethodInfo_Signature_ShouldBeGeneratedCorrectly_WithParameters()
    {
        // Arrange
        var methodInfo = new MethodInfo
        {
            Name = "SetValueAsync",
            ReturnType = "ValueTask",
            Parameters = ["string value", "bool notify"]
        };

        // Act
        var signature = methodInfo.Signature;

        // Assert
        Assert.Equal("ValueTask SetValueAsync(string value, bool notify)", signature);
    }

    [Fact]
    public void MethodInfo_Signature_ShouldBeGeneratedCorrectly_WithSingleParameter()
    {
        // Arrange
        var methodInfo = new MethodInfo
        {
            Name = "ScrollToItemAsync",
            ReturnType = "Task",
            Parameters = ["int index"]
        };

        // Act
        var signature = methodInfo.Signature;

        // Assert
        Assert.Equal("Task ScrollToItemAsync(int index)", signature);
    }

    [Fact]
    public void MethodInfo_WithIsInherited_ShouldBeCreatedSuccessfully()
    {
        // Arrange & Act
        var methodInfo = new MethodInfo
        {
            Name = "Dispose",
            ReturnType = "void",
            IsInherited = true
        };

        // Assert
        Assert.True(methodInfo.IsInherited);
    }

    [Fact]
    public void MethodInfo_RecordEquality_ShouldWorkCorrectly()
    {
        // Arrange
        var methodInfo1 = new MethodInfo
        {
            Name = "FocusAsync",
            ReturnType = "ValueTask",
            Parameters = ["bool preventScroll"]
        };

        var methodInfo2 = new MethodInfo
        {
            Name = "FocusAsync",
            ReturnType = "ValueTask",
            Parameters = ["bool preventScroll"]
        };

        // Act & Assert
        Assert.Equal(methodInfo1, methodInfo2);
    }

    [Fact]
    public void MethodInfo_Signature_WithVoidReturnType_ShouldBeCorrect()
    {
        // Arrange
        var methodInfo = new MethodInfo
        {
            Name = "Clear",
            ReturnType = "void"
        };

        // Act
        var signature = methodInfo.Signature;

        // Assert
        Assert.Equal("void Clear()", signature);
    }

    [Fact]
    public void MethodInfo_Signature_WithGenericReturnType_ShouldBeCorrect()
    {
        // Arrange
        var methodInfo = new MethodInfo
        {
            Name = "GetItemsAsync",
            ReturnType = "Task<IEnumerable<TItem>>",
            Parameters = ["int page", "int pageSize"]
        };

        // Act
        var signature = methodInfo.Signature;

        // Assert
        Assert.Equal("Task<IEnumerable<TItem>> GetItemsAsync(int page, int pageSize)", signature);
    }
}
