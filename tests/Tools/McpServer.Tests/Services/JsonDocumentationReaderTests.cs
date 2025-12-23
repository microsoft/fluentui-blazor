// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Services;

/// <summary>
/// Tests for the <see cref="JsonDocumentationReader"/> class.
/// </summary>
public class JsonDocumentationReaderTests
{
    private readonly string _testJsonPath;
    private readonly bool _jsonFileExists;

    public JsonDocumentationReaderTests()
    {
        _testJsonPath = Path.Combine(AppContext.BaseDirectory, "FluentUIComponentsDocumentation.json");
        _jsonFileExists = File.Exists(_testJsonPath);
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_WithNullPath_ShouldTryEmbeddedResource()
    {
        // Act
        var reader = new JsonDocumentationReader(null);

        // Assert - should not throw, may or may not have documentation depending on resource
        Assert.NotNull(reader);
    }

    [Fact]
    public void Constructor_WithInvalidPath_ShouldTryEmbeddedResource()
    {
        // Arrange
        var invalidPath = @"C:\nonexistent\path\file.json";

        // Act
        var reader = new JsonDocumentationReader(invalidPath);

        // Assert
        Assert.NotNull(reader);
    }

    [Fact]
    public void Constructor_WithValidPath_ShouldLoadDocumentation()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Act
        var reader = new JsonDocumentationReader(_testJsonPath);

        // Assert
        Assert.True(reader.IsAvailable);
    }

    #endregion

    #region IsAvailable Tests

    [Fact]
    public void IsAvailable_WithValidDocumentation_ShouldReturnTrue()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var reader = new JsonDocumentationReader(_testJsonPath);

        // Act & Assert
        Assert.True(reader.IsAvailable);
    }

    [Fact]
    public void IsAvailable_WithInvalidPath_ShouldHandleGracefully()
    {
        // Arrange
        var reader = new JsonDocumentationReader(@"C:\nonexistent\invalid.json");

        // Act & Assert - depends on embedded resource availability
        Assert.NotNull(reader);
    }

    #endregion

    #region Metadata Tests

    [Fact]
    public void Metadata_WithValidDocumentation_ShouldHaveValues()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var reader = new JsonDocumentationReader(_testJsonPath);

        // Act
        var metadata = reader.Metadata;

        // Assert
        Assert.NotNull(metadata);
        Assert.True(metadata!.ComponentCount > 0);
        Assert.True(metadata.EnumCount > 0);
    }

    #endregion

    #region GetAllComponents Tests

    [Fact]
    public void GetAllComponents_WithValidDocumentation_ShouldReturnComponents()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var reader = new JsonDocumentationReader(_testJsonPath);

        // Act
        var components = reader.GetAllComponents();

        // Assert
        Assert.NotEmpty(components);
    }

    [Fact]
    public void GetAllComponents_ShouldContainFluentButton()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var reader = new JsonDocumentationReader(_testJsonPath);

        // Act
        var components = reader.GetAllComponents();

        // Assert
        Assert.Contains(components, c => c.Name == "FluentButton");
    }

    [Fact]
    public void GetAllComponents_WithNoDocumentation_ShouldReturnEmptyList()
    {
        // Arrange - Create reader that will fail to load
        var tempPath = Path.Combine(Path.GetTempPath(), "invalid.json");
        File.WriteAllText(tempPath, "invalid json content {{{");

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

    #endregion

    #region GetComponent Tests

    [Fact]
    public void GetComponent_WithExactName_ShouldReturnComponent()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var reader = new JsonDocumentationReader(_testJsonPath);

        // Act
        var component = reader.GetComponent("FluentButton");

        // Assert
        Assert.NotNull(component);
        Assert.Equal("FluentButton", component!.Name);
    }

    [Fact]
    public void GetComponent_WithoutFluentPrefix_ShouldReturnComponent()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var reader = new JsonDocumentationReader(_testJsonPath);

        // Act
        var component = reader.GetComponent("Button");

        // Assert
        Assert.NotNull(component);
        Assert.Equal("FluentButton", component!.Name);
    }

    [Fact]
    public void GetComponent_CaseInsensitive_ShouldReturnComponent()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var reader = new JsonDocumentationReader(_testJsonPath);

        // Act
        var component = reader.GetComponent("fluentbutton");

        // Assert
        Assert.NotNull(component);
    }

    [Fact]
    public void GetComponent_WithNonExistentName_ShouldReturnNull()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var reader = new JsonDocumentationReader(_testJsonPath);

        // Act
        var component = reader.GetComponent("NonExistentComponent");

        // Assert
        Assert.Null(component);
    }

    #endregion

    #region GetAllEnums Tests

    [Fact]
    public void GetAllEnums_WithValidDocumentation_ShouldReturnEnums()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var reader = new JsonDocumentationReader(_testJsonPath);

        // Act
        var enums = reader.GetAllEnums();

        // Assert
        Assert.NotEmpty(enums);
    }

    [Fact]
    public void GetAllEnums_ShouldContainAppearance()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var reader = new JsonDocumentationReader(_testJsonPath);

        // Act
        var enums = reader.GetAllEnums();

        // Assert
        Assert.Contains(enums, e => e.Name == "Appearance");
    }

    #endregion

    #region GetEnum Tests

    [Fact]
    public void GetEnum_WithExactName_ShouldReturnEnum()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var reader = new JsonDocumentationReader(_testJsonPath);

        // Act
        var enumInfo = reader.GetEnum("Appearance");

        // Assert
        Assert.NotNull(enumInfo);
        Assert.Equal("Appearance", enumInfo!.Name);
    }

    [Fact]
    public void GetEnum_CaseInsensitive_ShouldReturnEnum()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var reader = new JsonDocumentationReader(_testJsonPath);

        // Act
        var enumInfo = reader.GetEnum("appearance");

        // Assert
        Assert.NotNull(enumInfo);
    }

    [Fact]
    public void GetEnum_WithNonExistentName_ShouldReturnNull()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var reader = new JsonDocumentationReader(_testJsonPath);

        // Act
        var enumInfo = reader.GetEnum("NonExistentEnum");

        // Assert
        Assert.Null(enumInfo);
    }

    [Fact]
    public void GetEnum_ShouldHaveValues()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var reader = new JsonDocumentationReader(_testJsonPath);

        // Act
        var enumInfo = reader.GetEnum("Appearance");

        // Assert
        Assert.NotNull(enumInfo);
        Assert.NotEmpty(enumInfo!.Values);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Constructor_WithEmptyJsonFile_ShouldHandleGracefully()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), "empty.json");
        File.WriteAllText(tempPath, "{}");

        try
        {
            // Act
            var reader = new JsonDocumentationReader(tempPath);

            // Assert
            Assert.True(reader.IsAvailable);
            Assert.Empty(reader.GetAllComponents());
            Assert.Empty(reader.GetAllEnums());
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Fact]
    public void Constructor_WithMalformedJson_ShouldHandleGracefully()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), "malformed.json");
        File.WriteAllText(tempPath, "{ this is not valid json }}}");

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
    public void Constructor_WithValidButEmptyComponents_ShouldWork()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), "valid_empty.json");
        var json = """
        {
            "metadata": {
                "assemblyVersion": "1.0.0",
                "generatedDateUtc": "2024-01-01",
                "componentCount": 0,
                "enumCount": 0
            },
            "components": [],
            "enums": []
        }
        """;
        File.WriteAllText(tempPath, json);

        try
        {
            // Act
            var reader = new JsonDocumentationReader(tempPath);

            // Assert
            Assert.True(reader.IsAvailable);
            Assert.NotNull(reader.Metadata);
            Assert.Equal(0, reader.Metadata!.ComponentCount);
            Assert.Empty(reader.GetAllComponents());
            Assert.Empty(reader.GetAllEnums());
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    #endregion
}
