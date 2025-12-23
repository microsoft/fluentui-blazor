// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Services;

/// <summary>
/// Tests for the <see cref="FluentUIDocumentationService"/> class.
/// </summary>
public class FluentUIDocumentationServiceTests
{
    private readonly string _testJsonPath;
    private readonly bool _jsonFileExists;

    public FluentUIDocumentationServiceTests()
    {
        _testJsonPath = Path.Combine(AppContext.BaseDirectory, "FluentUIComponentsDocumentation.json");
        _jsonFileExists = File.Exists(_testJsonPath);
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_WithNullPath_ShouldNotThrow()
    {
        // Act
        var exception = Record.Exception(() => new FluentUIDocumentationService(null));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_WithValidPath_ShouldLoadDocumentation()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Act
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Assert
        Assert.NotEmpty(service.GetAllComponents());
    }

    #endregion

    #region GetAllComponents Tests

    [Fact]
    public void GetAllComponents_ShouldReturnOrderedList()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var components = service.GetAllComponents();

        // Assert
        Assert.NotEmpty(components);
        var names = components.Select(c => c.Name).ToList();
        var sortedNames = names.OrderBy(n => n).ToList();
        Assert.Equal(sortedNames, names);
    }

    [Fact]
    public void GetAllComponents_ShouldContainExpectedComponents()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var components = service.GetAllComponents();

        // Assert
        Assert.Contains(components, c => c.Name == "FluentButton");
        Assert.Contains(components, c => c.Name == "FluentLink");
    }

    #endregion

    #region GetComponentsByCategory Tests

    [Fact]
    public void GetComponentsByCategory_WithValidCategory_ShouldReturnComponents()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var components = service.GetComponentsByCategory("Components");

        // Assert
        Assert.NotEmpty(components);
        Assert.All(components, c => Assert.Equal("Components", c.Category, ignoreCase: true));
    }

    [Fact]
    public void GetComponentsByCategory_CaseInsensitive_ShouldReturnComponents()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var componentsLower = service.GetComponentsByCategory("components");
        var componentsUpper = service.GetComponentsByCategory("COMPONENTS");

        // Assert
        Assert.Equal(componentsLower.Count, componentsUpper.Count);
    }

    [Fact]
    public void GetComponentsByCategory_WithInvalidCategory_ShouldReturnEmpty()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var components = service.GetComponentsByCategory("NonExistentCategory");

        // Assert
        Assert.Empty(components);
    }

    [Fact]
    public void GetComponentsByCategory_ShouldReturnOrderedList()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var components = service.GetComponentsByCategory("Components");

        // Assert
        var names = components.Select(c => c.Name).ToList();
        var sortedNames = names.OrderBy(n => n).ToList();
        Assert.Equal(sortedNames, names);
    }

    #endregion

    #region SearchComponents Tests

    [Fact]
    public void SearchComponents_ByName_ShouldReturnMatches()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var components = service.SearchComponents("Button");

        // Assert
        Assert.NotEmpty(components);
        Assert.Contains(components, c => c.Name.Contains("Button", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void SearchComponents_ByDescription_ShouldReturnMatches()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var components = service.SearchComponents("input");

        // Assert
        Assert.NotEmpty(components);
    }

    [Fact]
    public void SearchComponents_CaseInsensitive_ShouldReturnMatches()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var componentsLower = service.SearchComponents("button");
        var componentsUpper = service.SearchComponents("BUTTON");

        // Assert
        Assert.Equal(componentsLower.Count, componentsUpper.Count);
    }

    [Fact]
    public void SearchComponents_WithNoMatches_ShouldReturnEmpty()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var components = service.SearchComponents("XyzNonExistentTerm123");

        // Assert
        Assert.Empty(components);
    }

    [Fact]
    public void SearchComponents_ShouldReturnOrderedList()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var components = service.SearchComponents("Button");

        // Assert
        var names = components.Select(c => c.Name).ToList();
        var sortedNames = names.OrderBy(n => n).ToList();
        Assert.Equal(sortedNames, names);
    }

    #endregion

    #region GetComponentDetails Tests

    [Fact]
    public void GetComponentDetails_WithExactName_ShouldReturnDetails()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var details = service.GetComponentDetails("FluentButton");

        // Assert
        Assert.NotNull(details);
        Assert.Equal("FluentButton", details!.Component.Name);
    }

    [Fact]
    public void GetComponentDetails_WithoutFluentPrefix_ShouldReturnDetails()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var details = service.GetComponentDetails("Button");

        // Assert
        Assert.NotNull(details);
        Assert.Equal("FluentButton", details!.Component.Name);
    }

    [Fact]
    public void GetComponentDetails_CaseInsensitive_ShouldReturnDetails()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var details = service.GetComponentDetails("fluentbutton");

        // Assert
        Assert.NotNull(details);
    }

    [Fact]
    public void GetComponentDetails_WithNonExistentName_ShouldReturnNull()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var details = service.GetComponentDetails("NonExistentComponent");

        // Assert
        Assert.Null(details);
    }

    [Fact]
    public void GetComponentDetails_ShouldHaveParameters()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var details = service.GetComponentDetails("FluentButton");

        // Assert
        Assert.NotNull(details);
        Assert.NotEmpty(details!.Parameters);
    }

    [Fact]
    public void GetComponentDetails_ShouldHaveProperties()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var details = service.GetComponentDetails("FluentButton");

        // Assert
        Assert.NotNull(details);
        Assert.NotEmpty(details!.Properties);
    }

    [Fact]
    public void GetComponentDetails_ParametersShouldBeOrdered()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var details = service.GetComponentDetails("FluentButton");

        // Assert
        Assert.NotNull(details);
        var names = details!.Parameters.Select(p => p.Name).ToList();
        var sortedNames = names.OrderBy(n => n).ToList();
        Assert.Equal(sortedNames, names);
    }

    #endregion

    #region GetAllEnums Tests

    [Fact]
    public void GetAllEnums_ShouldReturnOrderedList()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var enums = service.GetAllEnums();

        // Assert
        Assert.NotEmpty(enums);
        var names = enums.Select(e => e.Name).ToList();
        var sortedNames = names.OrderBy(n => n).ToList();
        Assert.Equal(sortedNames, names);
    }

    [Fact]
    public void GetAllEnums_ShouldContainExpectedEnums()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var enums = service.GetAllEnums();

        // Assert
        Assert.Contains(enums, e => e.Name == "Appearance");
    }

    #endregion

    #region GetEnumDetails Tests

    [Fact]
    public void GetEnumDetails_WithExactName_ShouldReturnEnum()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var enumInfo = service.GetEnumDetails("Appearance");

        // Assert
        Assert.NotNull(enumInfo);
        Assert.Equal("Appearance", enumInfo!.Name);
    }

    [Fact]
    public void GetEnumDetails_CaseInsensitive_ShouldReturnEnum()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var enumInfo = service.GetEnumDetails("appearance");

        // Assert
        Assert.NotNull(enumInfo);
    }

    [Fact]
    public void GetEnumDetails_WithNonExistentName_ShouldReturnNull()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var enumInfo = service.GetEnumDetails("NonExistentEnum");

        // Assert
        Assert.Null(enumInfo);
    }

    [Fact]
    public void GetEnumDetails_ShouldHaveValues()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var enumInfo = service.GetEnumDetails("Appearance");

        // Assert
        Assert.NotNull(enumInfo);
        Assert.NotEmpty(enumInfo!.Values);
    }

    #endregion

    #region GetEnumsForComponent Tests

    [Fact]
    public void GetEnumsForComponent_WithValidComponent_ShouldReturnEnums()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var enums = service.GetEnumsForComponent("FluentButton");

        // Assert
        Assert.NotEmpty(enums);
    }

    [Fact]
    public void GetEnumsForComponent_WithNonExistentComponent_ShouldReturnEmpty()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var enums = service.GetEnumsForComponent("NonExistentComponent");

        // Assert
        Assert.Empty(enums);
    }

    [Fact]
    public void GetEnumsForComponent_ShouldContainAppearance()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var enums = service.GetEnumsForComponent("FluentButton");

        // Assert
        Assert.True(enums.ContainsKey("Appearance"));
    }

    [Fact]
    public void GetEnumsForComponent_EnumsShouldHaveValues()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act
        var enums = service.GetEnumsForComponent("FluentButton");

        // Assert
        foreach (var kvp in enums)
        {
            Assert.NotEmpty(kvp.Value.Values);
        }
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void Service_ShouldWorkEndToEnd()
    {
        Skip.IfNot(_jsonFileExists, "JSON documentation file not found");

        // Arrange
        var service = new FluentUIDocumentationService(_testJsonPath);

        // Act - Get all components
        var allComponents = service.GetAllComponents();
        Assert.NotEmpty(allComponents);

        // Act - Search for specific component
        var searchResults = service.SearchComponents("Button");
        Assert.NotEmpty(searchResults);

        // Act - Get component details
        var details = service.GetComponentDetails("FluentButton");
        Assert.NotNull(details);

        // Act - Get enums
        var allEnums = service.GetAllEnums();
        Assert.NotEmpty(allEnums);

        // Act - Get specific enum
        var enumInfo = service.GetEnumDetails("Appearance");
        Assert.NotNull(enumInfo);

        // Act - Get enums for component
        var componentEnums = service.GetEnumsForComponent("FluentButton");
        Assert.NotEmpty(componentEnums);
    }

    #endregion
}
