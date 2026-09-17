// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Services;

/// <summary>
/// Error handling and edge case tests for JsonDocumentationReader.
/// </summary>
public class JsonDocumentationReaderErrorTests
{
    [Fact]
    public void Constructor_WithNonExistentFile_ShouldNotThrow()
    {
        // Arrange
        var nonExistentPath = Path.Combine(Path.GetTempPath(), $"nonexistent_{Guid.NewGuid()}.json");

        // Act
        var exception = Record.Exception(() => new JsonDocumentationReader(nonExistentPath));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_WithEmptyString_ShouldNotThrow()
    {
        // Act
        var exception = Record.Exception(() => new JsonDocumentationReader(string.Empty));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void GetComponent_WithEmptyString_ShouldReturnNull()
    {
        // Arrange
        var tempPath = CreateValidJsonFile();
        try
        {
            var reader = new JsonDocumentationReader(tempPath);

            // Act
            var result = reader.GetComponent("");

            // Assert
            Assert.Null(result);
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Fact]
    public void GetEnum_WithEmptyString_ShouldReturnNull()
    {
        // Arrange
        var tempPath = CreateValidJsonFile();
        try
        {
            var reader = new JsonDocumentationReader(tempPath);

            // Act
            var result = reader.GetEnum("");

            // Assert
            Assert.Null(result);
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Fact]
    public void IsAvailable_WithCorruptedJson_ShouldReturnFalse()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), $"corrupted_{Guid.NewGuid()}.json");
        File.WriteAllText(tempPath, "{ invalid json content here }}}");

        try
        {
            var reader = new JsonDocumentationReader(tempPath);

            // Act & Assert
            Assert.False(reader.IsAvailable);
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Fact]
    public void IsAvailable_WithEmptyFile_ShouldReturnFalse()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), $"empty_{Guid.NewGuid()}.json");
        File.WriteAllText(tempPath, "");

        try
        {
            var reader = new JsonDocumentationReader(tempPath);

            // Act & Assert
            Assert.False(reader.IsAvailable);
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Fact]
    public void Metadata_WhenNotAvailable_ShouldReturnNull()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), $"invalid_{Guid.NewGuid()}.json");
        File.WriteAllText(tempPath, "invalid");

        try
        {
            var reader = new JsonDocumentationReader(tempPath);

            // Act & Assert
            Assert.Null(reader.Metadata);
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Fact]
    public void GetAllComponents_WhenNotAvailable_ShouldReturnEmptyList()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), $"invalid2_{Guid.NewGuid()}.json");
        File.WriteAllText(tempPath, "invalid content");

        try
        {
            var reader = new JsonDocumentationReader(tempPath);

            // Act
            var components = reader.GetAllComponents();

            // Assert
            Assert.Empty(components);
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Fact]
    public void GetAllEnums_WhenNotAvailable_ShouldReturnEmptyList()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), $"invalid3_{Guid.NewGuid()}.json");
        File.WriteAllText(tempPath, "invalid content");

        try
        {
            var reader = new JsonDocumentationReader(tempPath);

            // Act
            var enums = reader.GetAllEnums();

            // Assert
            Assert.Empty(enums);
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Fact]
    public void GetComponent_WhenNotAvailable_ShouldReturnNull()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), $"invalid4_{Guid.NewGuid()}.json");
        File.WriteAllText(tempPath, "invalid content");

        try
        {
            var reader = new JsonDocumentationReader(tempPath);

            // Act
            var component = reader.GetComponent("FluentButton");

            // Assert
            Assert.Null(component);
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Fact]
    public void Constructor_WithJsonArray_ShouldHandleGracefully()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), $"array_{Guid.NewGuid()}.json");
        File.WriteAllText(tempPath, "[1, 2, 3]");

        try
        {
            // Act
            var reader = new JsonDocumentationReader(tempPath);

            // Assert
            Assert.False(reader.IsAvailable);
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Fact]
    public void Constructor_WithJsonNull_ShouldHandleGracefully()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), $"null_{Guid.NewGuid()}.json");
        File.WriteAllText(tempPath, "null");

        try
        {
            // Act
            var reader = new JsonDocumentationReader(tempPath);

            // Assert
            Assert.False(reader.IsAvailable);
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    private static string CreateValidJsonFile()
    {
        var tempPath = Path.Combine(Path.GetTempPath(), $"valid_{Guid.NewGuid()}.json");
        var json = """
        {
            "metadata": { "assemblyVersion": "1.0.0", "generatedDateUtc": "2024-01-01", "componentCount": 1, "enumCount": 1 },
            "components": [{
                "name": "TestComponent",
                "fullName": "Test.TestComponent",
                "summary": "Test",
                "category": "Test",
                "isGeneric": false,
                "properties": [], "events": [], "methods": []
            }],
            "enums": [{
                "name": "TestEnum",
                "fullName": "Test.TestEnum",
                "description": "Test",
                "values": [{ "name": "Value1", "value": 0, "description": "Value 1" }]
            }]
        }
        """;
        File.WriteAllText(tempPath, json);
        return tempPath;
    }
}
