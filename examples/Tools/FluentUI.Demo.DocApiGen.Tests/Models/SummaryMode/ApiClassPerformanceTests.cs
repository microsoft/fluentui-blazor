// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;
using FluentUI.Demo.DocApiGen.Models.SummaryMode;

namespace FluentUI.Demo.DocApiGen.Tests.Models.SummaryMode;

/// <summary>
/// Performance tests for <see cref="ApiClass"/> optimizations.
/// </summary>
public class ApiClassPerformanceTests
{
    /// <summary>
    /// Test that abstract types don't cause exceptions during processing.
    /// </summary>
    [Fact]
    public void ApiClass_ShouldHandleAbstractTypes_WithoutExceptions()
    {
        // Arrange
        var assembly = typeof(TestAbstractClass).Assembly;
        var docReader = CreateMockDocReader();
        var options = new ApiClassOptions(assembly, docReader);

        // Act
        var exception = Record.Exception(() =>
        {
            var apiClass = new ApiClass(typeof(TestAbstractClass), options);
            var dictionary = apiClass.ToDictionary();
        });

        // Assert
        Assert.Null(exception); // Should not throw
    }

    /// <summary>
    /// Test that interface types don't cause exceptions during processing.
    /// </summary>
    [Fact]
    public void ApiClass_ShouldHandleInterfaces_WithoutExceptions()
    {
        // Arrange
        var assembly = typeof(ITestInterface).Assembly;
        var docReader = CreateMockDocReader();
        var options = new ApiClassOptions(assembly, docReader);

        // Act
        var exception = Record.Exception(() =>
        {
            var apiClass = new ApiClass(typeof(ITestInterface), options);
            var dictionary = apiClass.ToDictionary();
        });

        // Assert
        Assert.Null(exception); // Should not throw
    }

    /// <summary>
    /// Test that types with complex constructors don't cause exceptions.
    /// </summary>
    [Fact]
    public void ApiClass_ShouldHandleComplexConstructors_WithoutExceptions()
    {
        // Arrange
        var assembly = typeof(TestClassWithComplexConstructor).Assembly;
        var docReader = CreateMockDocReader();
        var options = new ApiClassOptions(assembly, docReader);

        // Act
        var exception = Record.Exception(() =>
        {
            var apiClass = new ApiClass(typeof(TestClassWithComplexConstructor), options);
            var dictionary = apiClass.ToDictionary();
        });

        // Assert
        Assert.Null(exception); // Should not throw
    }

    /// <summary>
    /// Test that concrete types with parameterless constructors work correctly.
    /// </summary>
    [Fact]
    public void ApiClass_ShouldHandleConcreteTypes_Successfully()
    {
        // Arrange
        var assembly = typeof(TestConcreteClass).Assembly;
        var docReader = CreateMockDocReader();
        var options = new ApiClassOptions(assembly, docReader);

        // Act
        var apiClass = new ApiClass(typeof(TestConcreteClass), options);
        var dictionary = apiClass.ToDictionary();

        // Assert
        Assert.NotNull(dictionary);
        // Should have at least the public properties
        Assert.Contains("TestProperty", dictionary.Keys);
    }

    /// <summary>
    /// Test that processing multiple types doesn't accumulate exceptions.
    /// </summary>
    [Fact]
    public void ApiClass_ShouldProcessMultipleTypes_Efficiently()
    {
        // Arrange
        var assembly = typeof(TestAbstractClass).Assembly;
        var docReader = CreateMockDocReader();
        var options = new ApiClassOptions(assembly, docReader);

        var types = new[]
        {
            typeof(TestAbstractClass),
            typeof(ITestInterface),
            typeof(TestConcreteClass),
            typeof(TestClassWithComplexConstructor)
        };

        // Act
        var startTime = DateTime.UtcNow;
        var processedCount = 0;

        foreach (var type in types)
        {
            var exception = Record.Exception(() =>
            {
                var apiClass = new ApiClass(type, options);
                var dictionary = apiClass.ToDictionary();
                processedCount++;
            });

            Assert.Null(exception); // Should not throw
        }

        var elapsed = DateTime.UtcNow - startTime;

        // Assert
        Assert.Equal(types.Length, processedCount);
        Assert.True(elapsed.TotalSeconds < 5, $"Processing took {elapsed.TotalSeconds} seconds, expected < 5 seconds");
    }

    /// <summary>
    /// Creates a mock DocXmlReader for testing.
    /// </summary>
    private static LoxSmoke.DocXml.DocXmlReader CreateMockDocReader()
    {
        // Create a minimal XML documentation file for testing
        var xmlContent = @"<?xml version=""1.0""?>
<doc>
    <assembly>
        <name>FluentUI.Demo.DocApiGen.Tests</name>
    </assembly>
    <members>
    </members>
</doc>";
        
        var tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, xmlContent);
        
        return new LoxSmoke.DocXml.DocXmlReader(tempFile);
    }

    #region Test Types

    /// <summary>
    /// Test abstract class for validation.
    /// </summary>
    public abstract class TestAbstractClass
    {
        /// <summary>
        /// Test property.
        /// </summary>
        public string? TestProperty { get; set; }
    }

    /// <summary>
    /// Test interface for validation.
    /// </summary>
    public interface ITestInterface
    {
        /// <summary>
        /// Test property.
        /// </summary>
        string? TestProperty { get; set; }
    }

    /// <summary>
    /// Test class with complex constructor.
    /// </summary>
    public class TestClassWithComplexConstructor
    {
        /// <summary>
        /// Test property.
        /// </summary>
        public string? TestProperty { get; set; }

        /// <summary>
        /// Constructor with required dependencies.
        /// </summary>
        public TestClassWithComplexConstructor(string requiredParam, int anotherParam)
        {
            TestProperty = requiredParam;
        }
    }

    /// <summary>
    /// Test concrete class with parameterless constructor.
    /// </summary>
    public class TestConcreteClass
    {
        /// <summary>
        /// Test property.
        /// </summary>
        public string? TestProperty { get; set; }

        /// <summary>
        /// Test integer property.
        /// </summary>
        public int TestIntProperty { get; set; }
    }

    #endregion
}
