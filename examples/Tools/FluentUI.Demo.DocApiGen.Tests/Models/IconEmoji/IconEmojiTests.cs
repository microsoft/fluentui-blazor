// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using System.Text.Json;
using FluentUI.Demo.DocApiGen.Abstractions;
using FluentUI.Demo.DocApiGen.Formatters;
using FluentUI.Demo.DocApiGen.Generators;
using Xunit;

namespace FluentUI.Demo.DocApiGen.Tests.Models.IconEmoji;

/// <summary>
/// Unit tests for Icons and Emojis generation modes using <see cref="IconsEmojisGenerator"/>.
/// </summary>
public class IconEmojiTests
{
    private readonly Assembly _testAssembly;
    private readonly FileInfo _testXmlFile;

    public IconEmojiTests()
    {
        // Use the FluentUI assembly which contains icons and emojis
        var fluentUiAssemblyPath = typeof(Microsoft.FluentUI.AspNetCore.Components.IconsExtensions).Assembly.Location;
        _testAssembly = Assembly.LoadFrom(fluentUiAssemblyPath);

        // Create a temporary XML file (not used for icons/emojis but required by constructor)
        var tempPath = Path.Combine(Path.GetTempPath(), "test.xml");
        File.WriteAllText(tempPath, "<?xml version=\"1.0\"?><doc></doc>");
        _testXmlFile = new FileInfo(tempPath);
    }

    [Fact]
    public void IconsMode_Constructor_ShouldSetCorrectMode()
    {
        // Arrange & Act
        var generator = new IconsEmojisGenerator(_testAssembly, _testXmlFile, GenerationMode.Icons);

        // Assert
        Assert.Equal(GenerationMode.Icons, generator.Mode);
    }

    [Fact]
    public void EmojisMode_Constructor_ShouldSetCorrectMode()
    {
        // Arrange & Act
        var generator = new IconsEmojisGenerator(_testAssembly, _testXmlFile, GenerationMode.Emojis);

        // Assert
        Assert.Equal(GenerationMode.Emojis, generator.Mode);
    }

    [Fact]
    public void Factory_CreateIconsGenerator_ShouldReturnIconsEmojisGenerator()
    {
        // Arrange & Act
        var generator = DocumentationGeneratorFactory.Create(GenerationMode.Icons, _testAssembly, _testXmlFile);

        // Assert
        Assert.IsType<IconsEmojisGenerator>(generator);
        Assert.Equal(GenerationMode.Icons, generator.Mode);
    }

    [Fact]
    public void Factory_CreateEmojisGenerator_ShouldReturnIconsEmojisGenerator()
    {
        // Arrange & Act
        var generator = DocumentationGeneratorFactory.Create(GenerationMode.Emojis, _testAssembly, _testXmlFile);

        // Assert
        Assert.IsType<IconsEmojisGenerator>(generator);
        Assert.Equal(GenerationMode.Emojis, generator.Mode);
    }

    [Fact]
    public void IconsMode_Generate_ShouldProduceValidJson()
    {
        // Arrange
        var generator = new IconsEmojisGenerator(_testAssembly, _testXmlFile, GenerationMode.Icons);
        var formatter = new JsonOutputFormatter();

        // Act
        var output = generator.Generate(formatter);

        // Assert
        Assert.NotNull(output);
        Assert.NotEmpty(output);

        // Verify it's valid JSON
        var jsonDoc = JsonDocument.Parse(output);
        Assert.NotNull(jsonDoc);
    }

    [Fact]
    public void EmojisMode_Generate_ShouldProduceValidJson()
    {
        // Arrange
        var generator = new IconsEmojisGenerator(_testAssembly, _testXmlFile, GenerationMode.Emojis);
        var formatter = new JsonOutputFormatter();

        // Act
        var output = generator.Generate(formatter);

        // Assert
        Assert.NotNull(output);
        Assert.NotEmpty(output);

        // Verify it's valid JSON
        var jsonDoc = JsonDocument.Parse(output);
        Assert.NotNull(jsonDoc);
    }

    [Fact]
    public void IconsMode_Generate_ShouldContainExpectedStructure()
    {
        // Arrange
        var generator = new IconsEmojisGenerator(_testAssembly, _testXmlFile, GenerationMode.Icons);
        var formatter = new JsonOutputFormatter();

        // Act
        var output = generator.Generate(formatter);
        var jsonDoc = JsonDocument.Parse(output);

        // Assert
        Assert.Equal(JsonValueKind.Object, jsonDoc.RootElement.ValueKind);

        // Verify structure: Icon Name -> Variant (Filled/Regular) -> Size array
        if (jsonDoc.RootElement.EnumerateObject().Any())
        {
            var firstIcon = jsonDoc.RootElement.EnumerateObject().First();
            Assert.Equal(JsonValueKind.Object, firstIcon.Value.ValueKind);

            var variants = firstIcon.Value.EnumerateObject().ToList();
            Assert.NotEmpty(variants);

            // Each variant should have an array of sizes
            foreach (var variant in variants)
            {
                Assert.Equal(JsonValueKind.Array, variant.Value.ValueKind);

                // Sizes should be numbers
                foreach (var size in variant.Value.EnumerateArray())
                {
                    Assert.Equal(JsonValueKind.Number, size.ValueKind);
                }
            }
        }
    }

    [Fact]
    public void EmojisMode_Generate_ShouldContainExpectedStructure()
    {
        // Arrange
        var generator = new IconsEmojisGenerator(_testAssembly, _testXmlFile, GenerationMode.Emojis);
        var formatter = new JsonOutputFormatter();

        // Act
        var output = generator.Generate(formatter);
        var jsonDoc = JsonDocument.Parse(output);

        // Assert
        Assert.Equal(JsonValueKind.Object, jsonDoc.RootElement.ValueKind);

        // Verify structure: Emoji Name -> Properties (Group, Style, Skintone, Size, KeyWords)
        if (jsonDoc.RootElement.EnumerateObject().Any())
        {
            var firstEmoji = jsonDoc.RootElement.EnumerateObject().First();
            Assert.Equal(JsonValueKind.Object, firstEmoji.Value.ValueKind);

            // Verify expected properties exist
            Assert.True(firstEmoji.Value.TryGetProperty("Group", out var group));
            Assert.Equal(JsonValueKind.String, group.ValueKind);

            Assert.True(firstEmoji.Value.TryGetProperty("Style", out var style));
            Assert.Equal(JsonValueKind.Array, style.ValueKind);

            Assert.True(firstEmoji.Value.TryGetProperty("Skintone", out var skintone));
            Assert.Equal(JsonValueKind.Array, skintone.ValueKind);

            Assert.True(firstEmoji.Value.TryGetProperty("Size", out var size));
            Assert.Equal(JsonValueKind.Array, size.ValueKind);

            Assert.True(firstEmoji.Value.TryGetProperty("KeyWords", out var keyWords));
            Assert.True(keyWords.ValueKind == JsonValueKind.String || keyWords.ValueKind == JsonValueKind.Null);
        }
    }

    [Fact]
    public void IconsMode_Generate_ShouldExcludeCustomSize()
    {
        // Arrange
        var generator = new IconsEmojisGenerator(_testAssembly, _testXmlFile, GenerationMode.Icons);
        var formatter = new JsonOutputFormatter();

        // Act
        var output = generator.Generate(formatter);
        var jsonDoc = JsonDocument.Parse(output);

        // Assert - verify no size equals 0 (Custom size)
        foreach (var icon in jsonDoc.RootElement.EnumerateObject())
        {
            foreach (var variant in icon.Value.EnumerateObject())
            {
                foreach (var size in variant.Value.EnumerateArray())
                {
                    Assert.True(size.GetInt32() > 0);
                }
            }
        }
    }

    [Fact]
    public void EmojisMode_Generate_ShouldExcludeCustomSize()
    {
        // Arrange
        var generator = new IconsEmojisGenerator(_testAssembly, _testXmlFile, GenerationMode.Emojis);
        var formatter = new JsonOutputFormatter();

        // Act
        var output = generator.Generate(formatter);
        var jsonDoc = JsonDocument.Parse(output);

        // Assert - verify no size equals 0 (Custom size)
        if (jsonDoc.RootElement.EnumerateObject().Any())
        {
            foreach (var emoji in jsonDoc.RootElement.EnumerateObject())
            {
                if (emoji.Value.TryGetProperty("Size", out var sizes))
                {
                    foreach (var size in sizes.EnumerateArray())
                    {
                        Assert.True(size.GetInt32() > 0);
                    }
                }
            }
        }
    }

    [Fact]
    public void IconsMode_SaveToFile_ShouldCreateFile()
    {
        // Arrange
        var generator = new IconsEmojisGenerator(_testAssembly, _testXmlFile, GenerationMode.Icons);
        var formatter = new JsonOutputFormatter();
        var outputPath = Path.Combine(Path.GetTempPath(), $"icons-test-{Guid.NewGuid()}.json");

        try
        {
            // Act
            generator.SaveToFile(outputPath, formatter);

            // Assert
            Assert.True(File.Exists(outputPath));
            var content = File.ReadAllText(outputPath);
            Assert.NotEmpty(content);

            // Verify it's valid JSON
            var jsonDoc = JsonDocument.Parse(content);
            Assert.NotNull(jsonDoc);
        }
        finally
        {
            // Cleanup
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
        }
    }

    [Fact]
    public void EmojisMode_SaveToFile_ShouldCreateFile()
    {
        // Arrange
        var generator = new IconsEmojisGenerator(_testAssembly, _testXmlFile, GenerationMode.Emojis);
        var formatter = new JsonOutputFormatter();
        var outputPath = Path.Combine(Path.GetTempPath(), $"emojis-test-{Guid.NewGuid()}.json");

        try
        {
            // Act
            generator.SaveToFile(outputPath, formatter);

            // Assert
            Assert.True(File.Exists(outputPath));
            var content = File.ReadAllText(outputPath);
            Assert.NotEmpty(content);

            // Verify it's valid JSON
            var jsonDoc = JsonDocument.Parse(content);
            Assert.NotNull(jsonDoc);
        }
        finally
        {
            // Cleanup
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
        }
    }

    [Fact]
    public void IconsMode_Generate_WithInvalidMode_ShouldThrowNotSupportedException()
    {
        // Arrange
        // Create generator with Summary mode (not supported by IconsEmojisGenerator)
        var generator = new IconsEmojisGenerator(_testAssembly, _testXmlFile, GenerationMode.Summary);
        var formatter = new JsonOutputFormatter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => generator.Generate(formatter));
    }

    [Fact]
    public void IconsMode_Generate_ShouldOrderIconsByName()
    {
        // Arrange
        var generator = new IconsEmojisGenerator(_testAssembly, _testXmlFile, GenerationMode.Icons);
        var formatter = new JsonOutputFormatter();

        // Act
        var output = generator.Generate(formatter);
        var jsonDoc = JsonDocument.Parse(output);

        // Assert - verify icons are ordered alphabetically
        var iconNames = jsonDoc.RootElement.EnumerateObject()
            .Select(p => p.Name)
            .ToList();

        var sortedNames = iconNames.OrderBy(n => n).ToList();
        Assert.Equal(sortedNames, iconNames);
    }

    [Fact]
    public void EmojisMode_Generate_ShouldOrderEmojisByName()
    {
        // Arrange
        var generator = new IconsEmojisGenerator(_testAssembly, _testXmlFile, GenerationMode.Emojis);
        var formatter = new JsonOutputFormatter();

        // Act
        var output = generator.Generate(formatter);
        var jsonDoc = JsonDocument.Parse(output);

        // Assert - verify emojis are ordered alphabetically
        if (jsonDoc.RootElement.EnumerateObject().Any())
        {
            var emojiNames = jsonDoc.RootElement.EnumerateObject()
                .Select(p => p.Name)
                .ToList();

            var sortedNames = emojiNames.OrderBy(n => n).ToList();
            Assert.Equal(sortedNames, emojiNames);
        }
    }
}
