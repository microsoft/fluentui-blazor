// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocApiGen;
using FluentUI.Demo.DocApiGen.Abstractions;
using FluentUI.Demo.DocApiGen.Formatters;
using FluentUI.Demo.DocApiGen.Generators;
using System.Reflection;
using System.Text.Json;
using Xunit;

namespace FluentUI.Demo.DocApiGen.IntegrationTests;

/// <summary>
/// Integration tests using the real Microsoft.FluentUI.AspNetCore.Components.xml file.
/// These tests validate documentation generation for actual FluentUI components.
/// </summary>
public class FluentUIComponentsIntegrationTests : IDisposable
{
    private readonly FileInfo _xmlDocumentation;
    private readonly string _tempOutputDirectory;
    private readonly string _xmlPath;
    private readonly Assembly _fluentUIAssembly;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentUIComponentsIntegrationTests"/> class.
    /// </summary>
    public FluentUIComponentsIntegrationTests()
    {
        _tempOutputDirectory = Path.Combine(Path.GetTempPath(), $"DocApiGen_FluentUI_Tests_{Guid.NewGuid()}");
        Directory.CreateDirectory(_tempOutputDirectory);

        // Get project root directory
        var projectRoot = GetProjectRootDirectory();
        _xmlPath = Path.Combine(projectRoot, "examples", "Tools", "FluentUI.Demo.DocApiGen", "Microsoft.FluentUI.AspNetCore.Components.xml");

        if (!File.Exists(_xmlPath))
        {
            throw new FileNotFoundException($"XML documentation file not found at: {_xmlPath}");
        }

        _xmlDocumentation = new FileInfo(_xmlPath);

        // Load the FluentUI assembly dynamically
        var fluentUIAssemblyPath = Path.Combine(projectRoot, "src", "Core", "bin", "Debug", "net9.0", "Microsoft.FluentUI.AspNetCore.Components.dll");

        if (!File.Exists(fluentUIAssemblyPath))
        {
            // Try alternative path (Release build)
            fluentUIAssemblyPath = Path.Combine(projectRoot, "src", "Core", "bin", "Release", "net9.0", "Microsoft.FluentUI.AspNetCore.Components.dll");

            if (!File.Exists(fluentUIAssemblyPath))
            {
                throw new FileNotFoundException($"FluentUI assembly not found. Please build the Core project first. Looked for: {fluentUIAssemblyPath}");
            }
        }

        _fluentUIAssembly = Assembly.LoadFrom(fluentUIAssemblyPath);
    }

    /// <summary>
    /// Gets the project root directory by walking up from the current directory.
    /// </summary>
    private static string GetProjectRootDirectory()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

        // Look for solution file
        while (directory != null)
        {
            var solutionFiles = directory.GetFiles("*.sln");
            if (solutionFiles.Length > 0)
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new InvalidOperationException($"Could not find project root directory. Current directory: {Directory.GetCurrentDirectory()}");
    }

    /// <summary>
    /// Cleanup temporary files and directories.
    /// </summary>
    public void Dispose()
    {
        if (Directory.Exists(_tempOutputDirectory))
        {
            try
            {
                Directory.Delete(_tempOutputDirectory, recursive: true);
            }
            catch
            {
                // Ignore cleanup errors
            }
        }
    }

    #region Summary Mode Tests (New Architecture)

    [Fact]
    public void SummaryGenerator_ShouldGenerateJsonSuccessfully()
    {
        // Arrange
        var generator = DocumentationGeneratorFactory.CreateSummaryGenerator(_fluentUIAssembly, _xmlDocumentation);
        var formatter = OutputFormatterFactory.CreateJsonFormatter(useCompactFormat: true);

        // Act
        var json = generator.Generate(formatter);

        // Assert
        Assert.NotNull(json);
        Assert.NotEmpty(json);
        Assert.Contains("__Generated__", json);
        Assert.Contains("AssemblyVersion", json);
    }

    [Fact]
    public void SummaryGenerator_ShouldGenerateCSharpSuccessfully()
    {
        // Arrange
        var generator = DocumentationGeneratorFactory.CreateSummaryGenerator(_fluentUIAssembly, _xmlDocumentation);
        var formatter = OutputFormatterFactory.CreateCSharpFormatter();

        // Act
        var code = generator.Generate(formatter);

        // Assert
        Assert.NotNull(code);
        Assert.NotEmpty(code);
        Assert.Contains("public static class CodeComments", code);
        Assert.Contains("Mode: Summary", code);
        Assert.Contains("SummaryData", code);
    }

    [Fact]
    public void SummaryGenerator_JsonOutput_ShouldContainFluentUIComponents()
    {
        // Arrange
        var generator = DocumentationGeneratorFactory.CreateSummaryGenerator(_fluentUIAssembly, _xmlDocumentation);
        var formatter = OutputFormatterFactory.CreateJsonFormatter(useCompactFormat: true);

        // Act
        var json = generator.Generate(formatter);

        // Assert
        // Should contain common FluentUI component names
        Assert.Contains("FluentButton", json);
    }

    [Fact]
    public void SummaryGenerator_CSharpOutput_ShouldContainFluentUIComponents()
    {
        // Arrange
        var generator = DocumentationGeneratorFactory.CreateSummaryGenerator(_fluentUIAssembly, _xmlDocumentation);
        var formatter = OutputFormatterFactory.CreateCSharpFormatter();

        // Act
        var code = generator.Generate(formatter);

        // Assert
        // Should contain common FluentUI component names
        Assert.Contains("FluentButton", code);
    }

    [Fact]
    public void SummaryGenerator_JsonOutput_ShouldBeValidJson()
    {
        // Arrange
        var generator = DocumentationGeneratorFactory.CreateSummaryGenerator(_fluentUIAssembly, _xmlDocumentation);
        var formatter = OutputFormatterFactory.CreateJsonFormatter(useCompactFormat: true);

        // Act
        var json = generator.Generate(formatter);

        // Assert - Verify it's valid JSON
        var exception = Record.Exception(() => JsonDocument.Parse(json));
        Assert.Null(exception);
    }

    [Fact]
    public void SummaryGenerator_SaveToFile_JsonShouldSucceed()
    {
        // Arrange
        var generator = DocumentationGeneratorFactory.CreateSummaryGenerator(_fluentUIAssembly, _xmlDocumentation);
        var formatter = OutputFormatterFactory.CreateJsonFormatter(useCompactFormat: true);
        var outputPath = Path.Combine(_tempOutputDirectory, "fluentui_summary.json");

        // Act
        generator.SaveToFile(outputPath, formatter);

        // Assert
        Assert.True(File.Exists(outputPath));

        var content = File.ReadAllText(outputPath);
        Assert.NotEmpty(content);
        Assert.Contains("__Generated__", content);

        // Verify valid JSON
        var exception = Record.Exception(() => JsonDocument.Parse(content));
        Assert.Null(exception);
    }

    [Fact]
    public void SummaryGenerator_SaveToFile_CSharpShouldSucceed()
    {
        // Arrange
        var generator = DocumentationGeneratorFactory.CreateSummaryGenerator(_fluentUIAssembly, _xmlDocumentation);
        var formatter = OutputFormatterFactory.CreateCSharpFormatter();
        var outputPath = Path.Combine(_tempOutputDirectory, "fluentui_summary.cs");

        // Act
        generator.SaveToFile(outputPath, formatter);

        // Assert
        Assert.True(File.Exists(outputPath));

        var content = File.ReadAllText(outputPath);
        Assert.NotEmpty(content);
        Assert.Contains("public static class CodeComments", content);
    }

    [Fact]
    public void SummaryGenerator_LargeScale_ShouldCompleteWithoutErrors()
    {
        // Arrange
        var generator = DocumentationGeneratorFactory.CreateSummaryGenerator(_fluentUIAssembly, _xmlDocumentation);
        var jsonFormatter = OutputFormatterFactory.CreateJsonFormatter(useCompactFormat: true);
        var csharpFormatter = OutputFormatterFactory.CreateCSharpFormatter();

        // Act & Assert - Should complete without throwing
        var exception = Record.Exception(() =>
        {
            var json = generator.Generate(jsonFormatter);
            Assert.NotNull(json);
            Assert.NotEmpty(json);

            var code = generator.Generate(csharpFormatter);
            Assert.NotNull(code);
            Assert.NotEmpty(code);
        });

        Assert.Null(exception);
    }

    [Fact]
    public void SummaryGenerator_OutputSize_ShouldBeReasonable()
    {
        // Arrange
        var generator = DocumentationGeneratorFactory.CreateSummaryGenerator(_fluentUIAssembly, _xmlDocumentation);
        var jsonFormatter = OutputFormatterFactory.CreateJsonFormatter(useCompactFormat: true);
        var csharpFormatter = OutputFormatterFactory.CreateCSharpFormatter();

        // Act
        var json = generator.Generate(jsonFormatter);
        var code = generator.Generate(csharpFormatter);

        // Assert - Output should be substantial but not excessive
        Assert.True(json.Length > 1000, "JSON output should be substantial");
        Assert.True(json.Length < 50_000_000, "JSON output should not be excessive");

        Assert.True(code.Length > 1000, "C# output should be substantial");
        Assert.True(code.Length < 50_000_000, "C# output should not be excessive");
    }

    [Fact]
    public void SummaryGenerator_JsonMetadata_ShouldBePresent()
    {
        // Arrange
        var generator = DocumentationGeneratorFactory.CreateSummaryGenerator(_fluentUIAssembly, _xmlDocumentation);
        var formatter = OutputFormatterFactory.CreateJsonFormatter(useCompactFormat: true);

        // Act
        var json = generator.Generate(formatter);
        using var doc = JsonDocument.Parse(json);

        // Assert
        var root = doc.RootElement;
        Assert.True(root.TryGetProperty("__Generated__", out var generated));

        Assert.True(generated.TryGetProperty("AssemblyVersion", out _));
        Assert.True(generated.TryGetProperty("DateUtc", out _));
    }

    #endregion

    #region Summary Mode Tests - Compact Format (Standard)

    [Fact]
    public void SummaryGenerator_CompactFormat_ShouldGenerateCorrectStructure()
    {
        // Arrange
        var generator = DocumentationGeneratorFactory.CreateSummaryGenerator(_fluentUIAssembly, _xmlDocumentation);
        var formatter = OutputFormatterFactory.CreateJsonFormatter(useCompactFormat: true);

        // Act
        var json = generator.Generate(formatter);

        // Assert
        Assert.NotNull(json);
        Assert.NotEmpty(json);
        Assert.Contains("__Generated__", json);
        Assert.Contains("AssemblyVersion", json);
        Assert.Contains("DateUtc", json);
        Assert.Contains("FluentButton", json);
    }

    [Fact]
    public void SummaryGenerator_CompactFormat_ShouldBeValidJson()
    {
        // Arrange
        var generator = DocumentationGeneratorFactory.CreateSummaryGenerator(_fluentUIAssembly, _xmlDocumentation);
        var formatter = OutputFormatterFactory.CreateJsonFormatter(useCompactFormat: true);

        // Act
        var json = generator.Generate(formatter);

        // Assert - Verify it's valid JSON
        var exception = Record.Exception(() => JsonDocument.Parse(json));
        Assert.Null(exception);
    }

    [Fact]
    public void SummaryGenerator_CompactFormat_SaveToFile_ShouldSucceed()
    {
        // Arrange
        var generator = DocumentationGeneratorFactory.CreateSummaryGenerator(_fluentUIAssembly, _xmlDocumentation);
        var formatter = OutputFormatterFactory.CreateJsonFormatter(useCompactFormat: true);
        var outputPath = Path.Combine(_tempOutputDirectory, "fluentui_compact.json");

        // Act
        generator.SaveToFile(outputPath, formatter);

        // Assert
        Assert.True(File.Exists(outputPath));

        var content = File.ReadAllText(outputPath);
        Assert.NotEmpty(content);
        Assert.Contains("__Generated__", content);

        // Verify valid JSON
        var exception = Record.Exception(() => JsonDocument.Parse(content));
        Assert.Null(exception);
    }

    #endregion

    #region Summary Mode Tests - Structured Format (Extended)

    [Fact]
    public void SummaryGenerator_StructuredFormat_ShouldContainMetadata()
    {
        // Arrange
        var generator = DocumentationGeneratorFactory.CreateSummaryGenerator(_fluentUIAssembly, _xmlDocumentation);
        var formatter = OutputFormatterFactory.CreateJsonFormatter(useCompactFormat: false);

        // Act
        var json = generator.Generate(formatter);
        using var doc = JsonDocument.Parse(json);

        // Assert
        var root = doc.RootElement;
        Assert.True(root.TryGetProperty("metadata", out var metadata));
        Assert.True(metadata.TryGetProperty("assemblyVersion", out _));
        Assert.True(metadata.TryGetProperty("dateUtc", out _));
        Assert.True(metadata.TryGetProperty("mode", out var mode));
        Assert.Equal("Summary", mode.GetString());
    }

    #endregion

    #region All Mode Tests

    [Fact]
    public void AllGenerator_ShouldGenerateJsonSuccessfully()
    {
        // Arrange
        var generator = DocumentationGeneratorFactory.CreateAllGenerator(_fluentUIAssembly, _xmlDocumentation);
        var formatter = OutputFormatterFactory.CreateJsonFormatter(useCompactFormat: false);

        // Act
        var json = generator.Generate(formatter);

        // Assert
        Assert.NotNull(json);
        Assert.NotEmpty(json);
        Assert.Contains("metadata", json);
        Assert.Contains("components", json);
    }

    [Fact]
    public void AllGenerator_ShouldNotSupportCSharpFormat()
    {
        // Arrange
        var generator = DocumentationGeneratorFactory.CreateAllGenerator(_fluentUIAssembly, _xmlDocumentation);
        var formatter = OutputFormatterFactory.CreateCSharpFormatter();

        // Act & Assert
        var exception = Assert.Throws<NotSupportedException>(() => generator.Generate(formatter));
        Assert.Contains("only supports JSON format", exception.Message);
    }

    [Fact]
    public void AllGenerator_JsonOutput_ShouldContainComponents()
    {
        // Arrange
        var generator = DocumentationGeneratorFactory.CreateAllGenerator(_fluentUIAssembly, _xmlDocumentation);
        var formatter = OutputFormatterFactory.CreateJsonFormatter(useCompactFormat: false);

        // Act
        var json = generator.Generate(formatter);
        using var doc = JsonDocument.Parse(json);

        // Assert
        var root = doc.RootElement;
        Assert.True(root.TryGetProperty("components", out var components));
        Assert.True(components.GetArrayLength() > 0);
    }

    [Fact]
    public void AllGenerator_SaveToFile_ShouldSucceed()
    {
        // Arrange
        var generator = DocumentationGeneratorFactory.CreateAllGenerator(_fluentUIAssembly, _xmlDocumentation);
        var formatter = OutputFormatterFactory.CreateJsonFormatter(useCompactFormat: false);
        var outputPath = Path.Combine(_tempOutputDirectory, "fluentui_all.json");

        // Act
        generator.SaveToFile(outputPath, formatter);

        // Assert
        Assert.True(File.Exists(outputPath));

        var content = File.ReadAllText(outputPath);
        Assert.NotEmpty(content);

        // Verify valid JSON
        var exception = Record.Exception(() => JsonDocument.Parse(content));
        Assert.Null(exception);
    }

    #endregion

    #region MCP Mode Tests

    [Fact]
    public void McpGenerator_ShouldGenerateJsonSuccessfully()
    {
        // Arrange
        // Load the McpServer assembly
        var projectRoot = GetProjectRootDirectory();
        var mcpAssemblyPath = Path.Combine(projectRoot, "src", "Tools", "McpServer", "bin", "Debug", "net9.0", "Microsoft.FluentUI.AspNetCore.McpServer.dll");

        if (!File.Exists(mcpAssemblyPath))
        {
            // Try Release build
            mcpAssemblyPath = Path.Combine(projectRoot, "src", "Tools", "McpServer", "bin", "Release", "net9.0", "Microsoft.FluentUI.AspNetCore.McpServer.dll");

            if (!File.Exists(mcpAssemblyPath))
            {
                // Skip test if MCP Server is not built
                return;
            }
        }

        var mcpXmlPath = Path.Combine(Path.GetDirectoryName(mcpAssemblyPath)!, "Microsoft.FluentUI.AspNetCore.McpServer.xml");

        if (!File.Exists(mcpXmlPath))
        {
            // Skip test if XML documentation is not available
            return;
        }

        var mcpAssembly = Assembly.LoadFrom(mcpAssemblyPath);
        var mcpXml = new FileInfo(mcpXmlPath);

        var generator = DocumentationGeneratorFactory.Create(GenerationMode.Mcp, mcpAssembly, mcpXml);
        var formatter = OutputFormatterFactory.CreateJsonFormatter(useCompactFormat: false);

        // Act
        var json = generator.Generate(formatter);

        // Assert
        Assert.NotNull(json);
        Assert.NotEmpty(json);
        Assert.Contains("metadata", json);
        Assert.Contains("tools", json);
        Assert.Contains("resources", json);
    }

    [Fact]
    public void McpGenerator_ShouldNotSupportCSharpFormat()
    {
        // Arrange
        var projectRoot = GetProjectRootDirectory();
        var mcpAssemblyPath = Path.Combine(projectRoot, "src", "Tools", "McpServer", "bin", "Debug", "net9.0", "Microsoft.FluentUI.AspNetCore.McpServer.dll");

        if (!File.Exists(mcpAssemblyPath))
        {
            mcpAssemblyPath = Path.Combine(projectRoot, "src", "Tools", "McpServer", "bin", "Release", "net9.0", "Microsoft.FluentUI.AspNetCore.McpServer.dll");

            if (!File.Exists(mcpAssemblyPath))
            {
                return;
            }
        }

        var mcpXmlPath = Path.Combine(Path.GetDirectoryName(mcpAssemblyPath)!, "Microsoft.FluentUI.AspNetCore.McpServer.xml");

        if (!File.Exists(mcpXmlPath))
        {
            return;
        }

        var mcpAssembly = Assembly.LoadFrom(mcpAssemblyPath);
        var mcpXml = new FileInfo(mcpXmlPath);

        var generator = DocumentationGeneratorFactory.Create(GenerationMode.Mcp, mcpAssembly, mcpXml);
        var formatter = OutputFormatterFactory.CreateCSharpFormatter();

        // Act & Assert
        var exception = Assert.Throws<NotSupportedException>(() => generator.Generate(formatter));
        Assert.Contains("only supports JSON format", exception.Message);
    }

    [Fact]
    public void McpGenerator_JsonOutput_ShouldContainTools()
    {
        // Arrange
        var projectRoot = GetProjectRootDirectory();
        var mcpAssemblyPath = Path.Combine(projectRoot, "src", "Tools", "McpServer", "bin", "Debug", "net9.0", "Microsoft.FluentUI.AspNetCore.McpServer.dll");

        if (!File.Exists(mcpAssemblyPath))
        {
            mcpAssemblyPath = Path.Combine(projectRoot, "src", "Tools", "McpServer", "bin", "Release", "net9.0", "Microsoft.FluentUI.AspNetCore.McpServer.dll");

            if (!File.Exists(mcpAssemblyPath))
            {
                return;
            }
        }

        var mcpXmlPath = Path.Combine(Path.GetDirectoryName(mcpAssemblyPath)!, "Microsoft.FluentUI.AspNetCore.McpServer.xml");

        if (!File.Exists(mcpXmlPath))
        {
            return;
        }

        var mcpAssembly = Assembly.LoadFrom(mcpAssemblyPath);
        var mcpXml = new FileInfo(mcpXmlPath);

        var generator = DocumentationGeneratorFactory.Create(GenerationMode.Mcp, mcpAssembly, mcpXml);
        var formatter = OutputFormatterFactory.CreateJsonFormatter(useCompactFormat: false);

        // Act
        var json = generator.Generate(formatter);
        using var doc = JsonDocument.Parse(json);

        // Assert
        var root = doc.RootElement;
        Assert.True(root.TryGetProperty("tools", out var tools));
        Assert.True(tools.GetArrayLength() > 0);

        // Should contain known tools
        Assert.Contains("ListComponents", json);
        Assert.Contains("GetComponentDetails", json);
        Assert.Contains("SearchComponents", json);
    }

    [Fact]
    public void McpGenerator_JsonOutput_ShouldContainResources()
    {
        // Arrange
        var projectRoot = GetProjectRootDirectory();
        var mcpAssemblyPath = Path.Combine(projectRoot, "src", "Tools", "McpServer", "bin", "Debug", "net9.0", "Microsoft.FluentUI.AspNetCore.McpServer.dll");

        if (!File.Exists(mcpAssemblyPath))
        {
            mcpAssemblyPath = Path.Combine(projectRoot, "src", "Tools", "McpServer", "bin", "Release", "net9.0", "Microsoft.FluentUI.AspNetCore.McpServer.dll");

            if (!File.Exists(mcpAssemblyPath))
            {
                return;
            }
        }

        var mcpXmlPath = Path.Combine(Path.GetDirectoryName(mcpAssemblyPath)!, "Microsoft.FluentUI.AspNetCore.McpServer.xml");

        if (!File.Exists(mcpXmlPath))
        {
            return;
        }

        var mcpAssembly = Assembly.LoadFrom(mcpAssemblyPath);
        var mcpXml = new FileInfo(mcpXmlPath);

        var generator = DocumentationGeneratorFactory.Create(GenerationMode.Mcp, mcpAssembly, mcpXml);
        var formatter = OutputFormatterFactory.CreateJsonFormatter(useCompactFormat: false);

        // Act
        var json = generator.Generate(formatter);
        using var doc = JsonDocument.Parse(json);

        // Assert
        var root = doc.RootElement;
        Assert.True(root.TryGetProperty("resources", out var resources));
        Assert.True(resources.GetArrayLength() > 0);

        // Should contain known resource URIs
        Assert.Contains("fluentui://components", json);
        Assert.Contains("fluentui://enums", json);
    }

    [Fact]
    public void McpGenerator_JsonOutput_ShouldContainMetadata()
    {
        // Arrange
        var projectRoot = GetProjectRootDirectory();
        var mcpAssemblyPath = Path.Combine(projectRoot, "src", "Tools", "McpServer", "bin", "Debug", "net9.0", "Microsoft.FluentUI.AspNetCore.McpServer.dll");

        if (!File.Exists(mcpAssemblyPath))
        {
            mcpAssemblyPath = Path.Combine(projectRoot, "src", "Tools", "McpServer", "bin", "Release", "net9.0", "Microsoft.FluentUI.AspNetCore.McpServer.dll");

            if (!File.Exists(mcpAssemblyPath))
            {
                return;
            }
        }

        var mcpXmlPath = Path.Combine(Path.GetDirectoryName(mcpAssemblyPath)!, "Microsoft.FluentUI.AspNetCore.McpServer.xml");

        if (!File.Exists(mcpXmlPath))
        {
            return;
        }

        var mcpAssembly = Assembly.LoadFrom(mcpAssemblyPath);
        var mcpXml = new FileInfo(mcpXmlPath);

        var generator = DocumentationGeneratorFactory.Create(GenerationMode.Mcp, mcpAssembly, mcpXml);
        var formatter = OutputFormatterFactory.CreateJsonFormatter(useCompactFormat: false);

        // Act
        var json = generator.Generate(formatter);
        using var doc = JsonDocument.Parse(json);

        // Assert
        var root = doc.RootElement;
        Assert.True(root.TryGetProperty("metadata", out var metadata));
        Assert.True(metadata.TryGetProperty("assemblyVersion", out _));
        Assert.True(metadata.TryGetProperty("generatedDateUtc", out _));
        Assert.True(metadata.TryGetProperty("toolCount", out var toolCount));
        Assert.True(metadata.TryGetProperty("resourceCount", out var resourceCount));
        Assert.True(toolCount.GetInt32() > 0);
        Assert.True(resourceCount.GetInt32() > 0);
    }

    [Fact]
    public void McpGenerator_SaveToFile_ShouldSucceed()
    {
        // Arrange
        var projectRoot = GetProjectRootDirectory();
        var mcpAssemblyPath = Path.Combine(projectRoot, "src", "Tools", "McpServer", "bin", "Debug", "net9.0", "Microsoft.FluentUI.AspNetCore.McpServer.dll");

        if (!File.Exists(mcpAssemblyPath))
        {
            mcpAssemblyPath = Path.Combine(projectRoot, "src", "Tools", "McpServer", "bin", "Release", "net9.0", "Microsoft.FluentUI.AspNetCore.McpServer.dll");

            if (!File.Exists(mcpAssemblyPath))
            {
                return;
            }
        }

        var mcpXmlPath = Path.Combine(Path.GetDirectoryName(mcpAssemblyPath)!, "Microsoft.FluentUI.AspNetCore.McpServer.xml");

        if (!File.Exists(mcpXmlPath))
        {
            return;
        }

        var mcpAssembly = Assembly.LoadFrom(mcpAssemblyPath);
        var mcpXml = new FileInfo(mcpXmlPath);

        var generator = DocumentationGeneratorFactory.Create(GenerationMode.Mcp, mcpAssembly, mcpXml);
        var formatter = OutputFormatterFactory.CreateJsonFormatter(useCompactFormat: false);
        var outputPath = Path.Combine(_tempOutputDirectory, "mcp_documentation.json");

        // Act
        generator.SaveToFile(outputPath, formatter);

        // Assert
        Assert.True(File.Exists(outputPath));

        var content = File.ReadAllText(outputPath);
        Assert.NotEmpty(content);

        // Verify valid JSON
        var exception = Record.Exception(() => JsonDocument.Parse(content));
        Assert.Null(exception);

        // Verify content
        Assert.Contains("tools", content);
        Assert.Contains("resources", content);
        Assert.Contains("prompts", content);
    }

    #endregion
}
