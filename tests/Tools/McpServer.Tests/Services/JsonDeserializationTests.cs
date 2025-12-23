// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
using Microsoft.FluentUI.AspNetCore.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Services;

/// <summary>
/// Tests for JSON deserialization of documentation models.
/// </summary>
public class JsonDeserializationTests
{
    [Fact]
    public void JsonComponentInfo_ShouldDeserializeCorrectly()
    {
        // Arrange
        var json = """
        {
            "name": "FluentButton",
            "fullName": "Microsoft.FluentUI.AspNetCore.Components.FluentButton",
            "summary": "A button component",
            "category": "Button",
            "isGeneric": false,
            "baseClass": "FluentComponentBase",
            "properties": [],
            "events": [],
            "methods": []
        }
        """;

        // Act
        var component = JsonSerializer.Deserialize<JsonComponentInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Assert
        Assert.NotNull(component);
        Assert.Equal("FluentButton", component!.Name);
        Assert.Equal("Microsoft.FluentUI.AspNetCore.Components.FluentButton", component.FullName);
        Assert.Equal("A button component", component.Summary);
        Assert.Equal("Button", component.Category);
        Assert.False(component.IsGeneric);
        Assert.Equal("FluentComponentBase", component.BaseClass);
    }

    [Fact]
    public void JsonPropertyInfo_ShouldDeserializeCorrectly()
    {
        // Arrange
        var json = """
        {
            "name": "Appearance",
            "type": "Appearance?",
            "description": "The appearance of the button",
            "isParameter": true,
            "isInherited": false,
            "defaultValue": "Appearance.Neutral",
            "enumValues": ["Accent", "Lightweight", "Neutral"]
        }
        """;

        // Act
        var property = JsonSerializer.Deserialize<JsonPropertyInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Assert
        Assert.NotNull(property);
        Assert.Equal("Appearance", property!.Name);
        Assert.Equal("Appearance?", property.Type);
        Assert.Equal("The appearance of the button", property.Description);
        Assert.True(property.IsParameter);
        Assert.False(property.IsInherited);
        Assert.Equal("Appearance.Neutral", property.DefaultValue);
        Assert.Equal(3, property.EnumValues.Length);
    }

    [Fact]
    public void JsonEventInfo_ShouldDeserializeCorrectly()
    {
        // Arrange
        var json = """
        {
            "name": "OnClick",
            "type": "EventCallback<MouseEventArgs>",
            "description": "Raised when the button is clicked",
            "isInherited": false
        }
        """;

        // Act
        var eventInfo = JsonSerializer.Deserialize<JsonEventInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Assert
        Assert.NotNull(eventInfo);
        Assert.Equal("OnClick", eventInfo!.Name);
        Assert.Equal("EventCallback<MouseEventArgs>", eventInfo.Type);
        Assert.Equal("Raised when the button is clicked", eventInfo.Description);
        Assert.False(eventInfo.IsInherited);
    }

    [Fact]
    public void JsonMethodInfo_ShouldDeserializeCorrectly()
    {
        // Arrange
        var json = """
        {
            "name": "FocusAsync",
            "returnType": "ValueTask",
            "description": "Sets focus to the button",
            "parameters": ["bool preventScroll"],
            "isInherited": false
        }
        """;

        // Act
        var methodInfo = JsonSerializer.Deserialize<JsonMethodInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Assert
        Assert.NotNull(methodInfo);
        Assert.Equal("FocusAsync", methodInfo!.Name);
        Assert.Equal("ValueTask", methodInfo.ReturnType);
        Assert.Equal("Sets focus to the button", methodInfo.Description);
        Assert.Single(methodInfo.Parameters);
        Assert.Equal("bool preventScroll", methodInfo.Parameters[0]);
        Assert.False(methodInfo.IsInherited);
    }

    [Fact]
    public void JsonEnumInfo_ShouldDeserializeCorrectly()
    {
        // Arrange
        var json = """
        {
            "name": "Appearance",
            "fullName": "Microsoft.FluentUI.AspNetCore.Components.Appearance",
            "description": "Defines the appearance of a button",
            "values": [
                { "name": "Accent", "value": 0, "description": "The accent appearance" },
                { "name": "Neutral", "value": 1, "description": "The neutral appearance" }
            ]
        }
        """;

        // Act
        var enumInfo = JsonSerializer.Deserialize<JsonEnumInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Assert
        Assert.NotNull(enumInfo);
        Assert.Equal("Appearance", enumInfo!.Name);
        Assert.Equal("Microsoft.FluentUI.AspNetCore.Components.Appearance", enumInfo.FullName);
        Assert.Equal("Defines the appearance of a button", enumInfo.Description);
        Assert.Equal(2, enumInfo.Values.Count);
    }

    [Fact]
    public void JsonEnumValueInfo_ShouldDeserializeCorrectly()
    {
        // Arrange
        var json = """
        {
            "name": "Accent",
            "value": 0,
            "description": "The accent color style"
        }
        """;

        // Act
        var enumValue = JsonSerializer.Deserialize<JsonEnumValueInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Assert
        Assert.NotNull(enumValue);
        Assert.Equal("Accent", enumValue!.Name);
        Assert.Equal(0, enumValue.Value);
        Assert.Equal("The accent color style", enumValue.Description);
    }

    [Fact]
    public void McpDocumentationRoot_ShouldDeserializeCorrectly()
    {
        // Arrange
        var json = """
        {
            "metadata": {
                "assemblyVersion": "5.0.0",
                "generatedDateUtc": "2024-01-01 12:00",
                "componentCount": 142,
                "enumCount": 111
            },
            "components": [],
            "enums": []
        }
        """;

        // Act
        var root = JsonSerializer.Deserialize<McpDocumentationRoot>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Assert
        Assert.NotNull(root);
        Assert.NotNull(root!.Metadata);
        Assert.Equal("5.0.0", root.Metadata.AssemblyVersion);
        Assert.Equal("2024-01-01 12:00", root.Metadata.GeneratedDateUtc);
        Assert.Equal(142, root.Metadata.ComponentCount);
        Assert.Equal(111, root.Metadata.EnumCount);
        Assert.Empty(root.Components);
        Assert.Empty(root.Enums);
    }

    [Fact]
    public void McpDocumentationMetadata_ShouldDeserializeCorrectly()
    {
        // Arrange
        var json = """
        {
            "assemblyVersion": "5.0.0-preview.1",
            "generatedDateUtc": "2024-12-16 20:33",
            "componentCount": 150,
            "enumCount": 120
        }
        """;

        // Act
        var metadata = JsonSerializer.Deserialize<McpDocumentationMetadata>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Assert
        Assert.NotNull(metadata);
        Assert.Equal("5.0.0-preview.1", metadata!.AssemblyVersion);
        Assert.Equal("2024-12-16 20:33", metadata.GeneratedDateUtc);
        Assert.Equal(150, metadata.ComponentCount);
        Assert.Equal(120, metadata.EnumCount);
    }

    [Fact]
    public void JsonPropertyInfo_DefaultValues_ShouldBeCorrect()
    {
        // Arrange
        var json = """
        {
            "name": "TestProp",
            "type": "string"
        }
        """;

        // Act
        var property = JsonSerializer.Deserialize<JsonPropertyInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Assert
        Assert.NotNull(property);
        Assert.Equal(string.Empty, property!.Description);
        Assert.False(property.IsParameter);
        Assert.False(property.IsInherited);
        Assert.Null(property.DefaultValue);
        Assert.Empty(property.EnumValues);
    }

    [Fact]
    public void JsonMethodInfo_DefaultValues_ShouldBeCorrect()
    {
        // Arrange
        var json = """
        {
            "name": "TestMethod",
            "returnType": "void"
        }
        """;

        // Act
        var method = JsonSerializer.Deserialize<JsonMethodInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Assert
        Assert.NotNull(method);
        Assert.Equal(string.Empty, method!.Description);
        Assert.Empty(method.Parameters);
        Assert.False(method.IsInherited);
    }
}
