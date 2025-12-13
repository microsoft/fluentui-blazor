// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocApiGen.Models;
using System.Reflection;
using System.Text.Json;
using Xunit;

namespace FluentUI.Demo.DocApiGen.IntegrationTests;

/// <summary>
/// Integration tests for ApiClassGenerator output formats and generation modes.
/// Tests focus on input/output validation for different generation modes:
/// - Default (Summary mode)
/// - JSON Summary
/// - C# Summary
/// - JSON All
/// </summary>
public class OutputFormatIntegrationTests : IDisposable
{
    private readonly Assembly _testAssembly;
    private readonly FileInfo _xmlDocumentation;
    private readonly string _tempXmlPath;
    private readonly string _tempOutputDirectory;

    /// <summary>
    /// Initializes a new instance of the <see cref="OutputFormatIntegrationTests"/> class.
    /// </summary>
    public OutputFormatIntegrationTests()
    {
        _testAssembly = typeof(OutputFormatIntegrationTests).Assembly;
        _tempOutputDirectory = Path.Combine(Path.GetTempPath(), $"DocApiGen_Tests_{Guid.NewGuid()}");
        Directory.CreateDirectory(_tempOutputDirectory);

        // Create XML documentation path
        _tempXmlPath = Path.Combine(_tempOutputDirectory, "test.xml");
        
        // Create a minimal XML documentation file
        File.WriteAllText(_tempXmlPath, @"<?xml version=""1.0""?>
<doc>
    <assembly>
        <name>FluentUI.Demo.DocApiGen.IntegrationTests</name>
    </assembly>
    <members>
        <member name=""T:FluentUI.Demo.DocApiGen.IntegrationTests.OutputFormatIntegrationTests"">
            <summary>Test class for documentation generation</summary>
        </member>
    </members>
</doc>");
        
        _xmlDocumentation = new FileInfo(_tempXmlPath);
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

    #region Default Mode Tests (Summary)

    [Fact]
    public void Default_GenerateJson_ShouldUseSummaryMode()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);

        // Act
        var json = generator.GenerateJson();

        // Assert
        Assert.NotNull(json);
        Assert.NotEmpty(json);
        Assert.Contains("__Generated__", json);
        Assert.Contains("\"Mode\": \"Summary\"", json);
    }

    [Fact]
    public void Default_GenerateCSharp_ShouldUseSummaryMode()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);

        // Act
        var code = generator.GenerateCSharp();

        // Assert
        Assert.NotNull(code);
        Assert.NotEmpty(code);
        Assert.Contains("public class CodeComments", code);
        Assert.Contains("Mode: Summary", code);
    }

    #endregion

    #region JSON Summary Mode Tests

    [Fact]
    public void JsonSummary_GenerateJson_ShouldProduceValidJson()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);

        // Act
        var json = generator.GenerateJson(GenerationMode.Summary);

        // Assert
        Assert.NotNull(json);
        Assert.NotEmpty(json);
        
        // Verify it's valid JSON
        var exception = Record.Exception(() => JsonDocument.Parse(json));
        Assert.Null(exception);
    }

    [Fact]
    public void JsonSummary_Output_ShouldContainMetadata()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);

        // Act
        var json = generator.GenerateJson(GenerationMode.Summary);

        // Assert
        Assert.Contains("__Generated__", json);
        Assert.Contains("AssemblyVersion", json);
        Assert.Contains("DateUtc", json);
        Assert.Contains("\"Mode\": \"Summary\"", json);
    }

    [Fact]
    public void JsonSummary_SaveToFile_ShouldCreateValidFile()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);
        var outputPath = Path.Combine(_tempOutputDirectory, "output_summary.json");

        // Act
        generator.SaveToFile(outputPath, "json", GenerationMode.Summary);

        // Assert
        Assert.True(File.Exists(outputPath));
        
        var content = File.ReadAllText(outputPath);
        Assert.NotNull(content);
        Assert.NotEmpty(content);
        Assert.Contains("__Generated__", content);
        Assert.Contains("\"Mode\": \"Summary\"", content);
        
        // Verify valid JSON
        var exception = Record.Exception(() => JsonDocument.Parse(content));
        Assert.Null(exception);
    }

    [Fact]
    public void JsonSummary_Output_ShouldBeWellFormatted()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);

        // Act
        var json = generator.GenerateJson(GenerationMode.Summary);

        // Assert
        Assert.StartsWith("{", json);
        Assert.True(json.TrimEnd().EndsWith("}"), "JSON should end with }");
        Assert.Contains("\n", json); // Should have line breaks
        Assert.Contains("  ", json); // Should have indentation
    }

    #endregion

    #region C# Summary Mode Tests

    [Fact]
    public void CSharpSummary_GenerateCSharp_ShouldProduceValidCode()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);

        // Act
        var code = generator.GenerateCSharp(GenerationMode.Summary);

        // Assert
        Assert.NotNull(code);
        Assert.NotEmpty(code);
        Assert.Contains("public class CodeComments", code);
        Assert.Contains("SummaryData", code);
        Assert.Contains("Mode: Summary", code);
    }

    [Fact]
    public void CSharpSummary_Output_ShouldContainLicenseHeader()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);

        // Act
        var code = generator.GenerateCSharp(GenerationMode.Summary);

        // Assert
        Assert.StartsWith("//", code);
        Assert.Contains("This file is licensed to you under the MIT License", code);
    }

    [Fact]
    public void CSharpSummary_Output_ShouldContainAutoGeneratedComment()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);

        // Act
        var code = generator.GenerateCSharp(GenerationMode.Summary);

        // Assert
        Assert.Contains("<auto-generated>", code);
        Assert.Contains("This code was generated by a tool", code);
        Assert.Contains("Mode: Summary", code);
    }

    [Fact]
    public void CSharpSummary_Output_ShouldContainUsings()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);

        // Act
        var code = generator.GenerateCSharp(GenerationMode.Summary);

        // Assert
        Assert.Contains("using System.Reflection;", code);
    }

    [Fact]
    public void CSharpSummary_Output_ShouldContainStaticMethods()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);

        // Act
        var code = generator.GenerateCSharp(GenerationMode.Summary);

        // Assert
        Assert.Contains("public static string GetSignature(MemberInfo member)", code);
        Assert.Contains("public static string GetSummary(MemberInfo member)", code);
    }

    [Fact]
    public void CSharpSummary_SaveToFile_ShouldCreateValidFile()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);
        var outputPath = Path.Combine(_tempOutputDirectory, "output_summary.cs");

        // Act
        generator.SaveToFile(outputPath, "csharp", GenerationMode.Summary);

        // Assert
        Assert.True(File.Exists(outputPath));
        
        var content = File.ReadAllText(outputPath);
        Assert.NotNull(content);
        Assert.NotEmpty(content);
        Assert.Contains("public class CodeComments", content);
        Assert.Contains("Mode: Summary", content);
    }

    #endregion

    #region JSON All Mode Tests

    [Fact]
    public void JsonAll_GenerateJson_ShouldProduceValidJson()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);

        // Act
        var json = generator.GenerateJson(GenerationMode.All);

        // Assert
        Assert.NotNull(json);
        Assert.NotEmpty(json);
        
        // Verify it's valid JSON
        var exception = Record.Exception(() => JsonDocument.Parse(json));
        Assert.Null(exception);
    }

    [Fact]
    public void JsonAll_Output_ShouldContainMetadata()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);

        // Act
        var json = generator.GenerateJson(GenerationMode.All);

        // Assert
        Assert.Contains("__Generated__", json);
        Assert.Contains("AssemblyVersion", json);
        Assert.Contains("DateUtc", json);
        Assert.Contains("\"Mode\": \"All\"", json);
    }

    [Fact]
    public void JsonAll_SaveToFile_ShouldCreateValidFile()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);
        var outputPath = Path.Combine(_tempOutputDirectory, "output_all.json");

        // Act
        generator.SaveToFile(outputPath, "json", GenerationMode.All);

        // Assert
        Assert.True(File.Exists(outputPath));
        
        var content = File.ReadAllText(outputPath);
        Assert.NotNull(content);
        Assert.NotEmpty(content);
        Assert.Contains("__Generated__", content);
        Assert.Contains("\"Mode\": \"All\"", content);
        
        // Verify valid JSON
        var exception = Record.Exception(() => JsonDocument.Parse(content));
        Assert.Null(exception);
    }

    [Fact]
    public void JsonAll_Output_ShouldBeWellFormatted()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);

        // Act
        var json = generator.GenerateJson(GenerationMode.All);

        // Assert
        Assert.StartsWith("{", json);
        Assert.True(json.TrimEnd().EndsWith("}"), "JSON should end with }");
        Assert.Contains("\n", json); // Should have line breaks
        Assert.Contains("  ", json); // Should have indentation
    }

    #endregion

    #region Mode Comparison Tests

    [Fact]
    public void Comparison_SummaryAndAllModes_ShouldHaveDifferentModeInOutput()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);

        // Act
        var jsonSummary = generator.GenerateJson(GenerationMode.Summary);
        var jsonAll = generator.GenerateJson(GenerationMode.All);

        // Assert
        Assert.Contains("\"Mode\": \"Summary\"", jsonSummary);
        Assert.Contains("\"Mode\": \"All\"", jsonAll);
        Assert.DoesNotContain("\"Mode\": \"All\"", jsonSummary);
        Assert.DoesNotContain("\"Mode\": \"Summary\"", jsonAll);
    }

    [Fact]
    public void Comparison_JsonAndCSharp_ShouldHaveSameMode()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);

        // Act
        var json = generator.GenerateJson(GenerationMode.Summary);
        var csharp = generator.GenerateCSharp(GenerationMode.Summary);

        // Assert
        Assert.Contains("\"Mode\": \"Summary\"", json);
        Assert.Contains("Mode: Summary", csharp);
    }

    [Fact]
    public void Comparison_SaveToFile_ShouldOverwriteExistingFile()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);
        var outputPath = Path.Combine(_tempOutputDirectory, "overwrite_test.json");
        
        // Create initial file with old content
        File.WriteAllText(outputPath, "old content");
        var initialWriteTime = File.GetLastWriteTimeUtc(outputPath);
        
        // Wait a bit to ensure different timestamp
        System.Threading.Thread.Sleep(100);

        // Act
        generator.SaveToFile(outputPath, "json", GenerationMode.Summary);

        // Assert
        Assert.True(File.Exists(outputPath));
        var content = File.ReadAllText(outputPath);
        Assert.DoesNotContain("old content", content);
        Assert.Contains("__Generated__", content);
        
        var newWriteTime = File.GetLastWriteTimeUtc(outputPath);
        Assert.True(newWriteTime > initialWriteTime);
    }

    #endregion

    #region File Extension Tests

    [Fact]
    public void SaveToFile_WithJsonFormat_ShouldWorkWithAnyExtension()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);
        var outputPath = Path.Combine(_tempOutputDirectory, "output.txt");

        // Act
        generator.SaveToFile(outputPath, "json", GenerationMode.Summary);

        // Assert
        Assert.True(File.Exists(outputPath));
        var content = File.ReadAllText(outputPath);
        
        // Should still be valid JSON despite .txt extension
        var exception = Record.Exception(() => JsonDocument.Parse(content));
        Assert.Null(exception);
    }

    [Fact]
    public void SaveToFile_WithCSharpFormat_ShouldWorkWithAnyExtension()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);
        var outputPath = Path.Combine(_tempOutputDirectory, "output.txt");

        // Act
        generator.SaveToFile(outputPath, "csharp", GenerationMode.Summary);

        // Assert
        Assert.True(File.Exists(outputPath));
        var content = File.ReadAllText(outputPath);
        
        // Should still be valid C# code despite .txt extension
        Assert.Contains("public class CodeComments", content);
    }

    #endregion

    #region Output Structure Tests

    [Fact]
    public void JsonOutput_Structure_ShouldHaveRootObject()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);

        // Act
        var json = generator.GenerateJson(GenerationMode.Summary);
        using var doc = JsonDocument.Parse(json);

        // Assert
        Assert.Equal(JsonValueKind.Object, doc.RootElement.ValueKind);
        Assert.True(doc.RootElement.TryGetProperty("__Generated__", out var generatedProp));
        Assert.Equal(JsonValueKind.Object, generatedProp.ValueKind);
    }

    [Fact]
    public void JsonOutput_Metadata_ShouldHaveRequiredFields()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);

        // Act
        var json = generator.GenerateJson(GenerationMode.Summary);
        using var doc = JsonDocument.Parse(json);

        // Assert
        var generated = doc.RootElement.GetProperty("__Generated__");
        Assert.True(generated.TryGetProperty("AssemblyVersion", out _));
        Assert.True(generated.TryGetProperty("DateUtc", out _));
        Assert.True(generated.TryGetProperty("Mode", out var modeProp));
        Assert.Equal("Summary", modeProp.GetString());
    }

    [Fact]
    public void CSharpOutput_Structure_ShouldHaveRequiredElements()
    {
        // Arrange
        var generator = new ApiClassGenerator(_testAssembly, _xmlDocumentation);

        // Act
        var code = generator.GenerateCSharp(GenerationMode.Summary);

        // Assert
        Assert.Matches(@"public\s+class\s+CodeComments", code);
        Assert.Matches(@"public\s+static\s+readonly\s+IDictionary", code);
        Assert.Matches(@"public\s+static\s+string\s+GetSignature", code);
        Assert.Matches(@"public\s+static\s+string\s+GetSummary", code);
    }

    #endregion
}
