// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocApiGen.Models;
using System.Reflection;
using System.Text.Json;
using System.Linq;
using Xunit;

namespace FluentUI.Demo.DocApiGen.IntegrationTests;

/// <summary>
/// Integration tests using the real Microsoft.FluentUI.AspNetCore.Components.xml file.
/// These tests validate documentation generation for actual FluentUI components with
/// the ApiClassGenerator (Summary mode focus).
/// Note: MCP (All mode) tests are skipped for now - will be implemented later.
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

    #region ApiClassGenerator (Summary Mode) Tests

    [Fact]
    public void ApiGenerator_ShouldGenerateJsonSuccessfully()
    {
        // Arrange
        var generator = new ApiClassGenerator(_fluentUIAssembly, _xmlDocumentation);

        // Act
        var json = generator.GenerateJson(GenerationMode.Summary);

        // Assert
        Assert.NotNull(json);
        Assert.NotEmpty(json);
        Assert.Contains("__Generated__", json);
        Assert.Contains("\"Mode\": \"Summary\"", json);
    }

    [Fact]
    public void ApiGenerator_ShouldGenerateCSharpSuccessfully()
    {
        // Arrange
        var generator = new ApiClassGenerator(_fluentUIAssembly, _xmlDocumentation);

        // Act
        var code = generator.GenerateCSharp(GenerationMode.Summary);

        // Assert
        Assert.NotNull(code);
        Assert.NotEmpty(code);
        Assert.Contains("public class CodeComments", code);
        Assert.Contains("Mode: Summary", code);
        Assert.Contains("SummaryData", code);
    }

    [Fact]
    public void ApiGenerator_JsonOutput_ShouldContainFluentUIComponents()
    {
        // Arrange
        var generator = new ApiClassGenerator(_fluentUIAssembly, _xmlDocumentation);

        // Act
        var json = generator.GenerateJson(GenerationMode.Summary);

        // Assert
        // Should contain common FluentUI component names
        Assert.Contains("FluentButton", json);
    }

    [Fact]
    public void ApiGenerator_CSharpOutput_ShouldContainFluentUIComponents()
    {
        // Arrange
        var generator = new ApiClassGenerator(_fluentUIAssembly, _xmlDocumentation);

        // Act
        var code = generator.GenerateCSharp(GenerationMode.Summary);

        // Assert
        // Should contain common FluentUI component names
        Assert.Contains("FluentButton", code);
    }

    [Fact]
    public void ApiGenerator_JsonOutput_ShouldBeValidJson()
    {
        // Arrange
        var generator = new ApiClassGenerator(_fluentUIAssembly, _xmlDocumentation);

        // Act
        var json = generator.GenerateJson(GenerationMode.Summary);

        // Assert - Verify it's valid JSON
        var exception = Record.Exception(() => JsonDocument.Parse(json));
        Assert.Null(exception);
    }

    [Fact]
    public void ApiGenerator_SaveToFile_JsonShouldSucceed()
    {
        // Arrange
        var generator = new ApiClassGenerator(_fluentUIAssembly, _xmlDocumentation);
        var outputPath = Path.Combine(_tempOutputDirectory, "fluentui_summary.json");

        // Act
        generator.SaveToFile(outputPath, "json", GenerationMode.Summary);

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
    public void ApiGenerator_SaveToFile_CSharpShouldSucceed()
    {
        // Arrange
        var generator = new ApiClassGenerator(_fluentUIAssembly, _xmlDocumentation);
        var outputPath = Path.Combine(_tempOutputDirectory, "fluentui_summary.cs");

        // Act
        generator.SaveToFile(outputPath, "csharp", GenerationMode.Summary);

        // Assert
        Assert.True(File.Exists(outputPath));
        
        var content = File.ReadAllText(outputPath);
        Assert.NotEmpty(content);
        Assert.Contains("public class CodeComments", content);
    }

    [Fact]
    public void ApiGenerator_LargeScale_ShouldCompleteWithoutErrors()
    {
        // Arrange
        var generator = new ApiClassGenerator(_fluentUIAssembly, _xmlDocumentation);

        // Act & Assert - Should complete without throwing
        var exception = Record.Exception(() =>
        {
            var json = generator.GenerateJson(GenerationMode.Summary);
            Assert.NotNull(json);
            Assert.NotEmpty(json);
            
            var code = generator.GenerateCSharp(GenerationMode.Summary);
            Assert.NotNull(code);
            Assert.NotEmpty(code);
        });
        
        Assert.Null(exception);
    }

    [Fact]
    public void ApiGenerator_OutputSize_ShouldBeReasonable()
    {
        // Arrange
        var generator = new ApiClassGenerator(_fluentUIAssembly, _xmlDocumentation);

        // Act
        var json = generator.GenerateJson(GenerationMode.Summary);
        var code = generator.GenerateCSharp(GenerationMode.Summary);

        // Assert - Output should be substantial but not excessive
        Assert.True(json.Length > 1000, "JSON output should be substantial");
        Assert.True(json.Length < 50_000_000, "JSON output should not be excessive");
        
        Assert.True(code.Length > 1000, "C# output should be substantial");
        Assert.True(code.Length < 50_000_000, "C# output should not be excessive");
    }

    [Fact]
    public void ApiGenerator_JsonMetadata_ShouldBePresent()
    {
        // Arrange
        var generator = new ApiClassGenerator(_fluentUIAssembly, _xmlDocumentation);

        // Act
        var json = generator.GenerateJson(GenerationMode.Summary);
        using var doc = JsonDocument.Parse(json);

        // Assert
        var root = doc.RootElement;
        Assert.True(root.TryGetProperty("__Generated__", out var generated));
        
        var generatedObj = generated;
        Assert.True(generatedObj.TryGetProperty("AssemblyVersion", out _));
        Assert.True(generatedObj.TryGetProperty("DateUtc", out _));
        Assert.True(generatedObj.TryGetProperty("Mode", out var mode));
        Assert.Equal("Summary", mode.GetString());
    }

    #endregion

    #region MCP Tests (Skipped - Future Implementation)

    // Note: MCP/All mode tests are commented out for now
    // These will be implemented in a future phase
    
    /*
    [Fact(Skip = "MCP implementation deferred")]
    public void McpGenerator_WillBeImplementedLater()
    {
        // MCP tests will be added when ready to work on All mode
        Assert.True(true);
    }
    */

    #endregion
}
