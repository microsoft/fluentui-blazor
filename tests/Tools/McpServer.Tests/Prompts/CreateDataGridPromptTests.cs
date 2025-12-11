// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Prompts;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Prompts;

/// <summary>
/// Tests for <see cref="CreateDataGridPrompt"/>.
/// </summary>
public class CreateDataGridPromptTests
{
    private readonly CreateDataGridPrompt _prompt;
    private readonly FluentUIDocumentationService _documentationService;

    public CreateDataGridPromptTests()
    {
        var jsonPath = JsonDocumentationFinder.Find();
        _documentationService = new FluentUIDocumentationService(jsonPath);
        _prompt = new CreateDataGridPrompt(_documentationService);
    }

    [Fact]
    public void CreateDataGrid_WithDataDescription_ReturnsNonEmptyMessage()
    {
        // Arrange
        var dataDescription = "products with name, price, category";

        // Act
        var result = _prompt.CreateDataGrid(dataDescription);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.NotEmpty(result.Text);
        Assert.Contains(dataDescription, result.Text);
    }

    [Fact]
    public void CreateDataGrid_ContainsFluentDataGridInfo()
    {
        // Act
        var result = _prompt.CreateDataGrid("users with name and email");

        // Assert
        Assert.Contains("FluentDataGrid", result.Text);
    }

    [Fact]
    public void CreateDataGrid_WithSortingFeature_IncludesSorting()
    {
        // Act
        var result = _prompt.CreateDataGrid("products", "sorting");

        // Assert
        Assert.Contains("sort", result.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CreateDataGrid_WithFilteringFeature_IncludesFiltering()
    {
        // Act
        var result = _prompt.CreateDataGrid("products", "filtering");

        // Assert
        Assert.Contains("filter", result.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CreateDataGrid_WithPaginationFeature_IncludesPagination()
    {
        // Act
        var result = _prompt.CreateDataGrid("products", "pagination");

        // Assert
        Assert.Contains("pagination", result.Text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("FluentPaginator", result.Text);
    }

    [Fact]
    public void CreateDataGrid_WithSelectionFeature_IncludesSelection()
    {
        // Act
        var result = _prompt.CreateDataGrid("products", "selection");

        // Assert
        Assert.Contains("selection", result.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CreateDataGrid_WithItemType_IncludesItemType()
    {
        // Arrange
        var itemType = "Product";

        // Act
        var result = _prompt.CreateDataGrid("products", null, itemType);

        // Assert
        Assert.Contains(itemType, result.Text);
        Assert.Contains("Item Type", result.Text);
    }

    [Fact]
    public void CreateDataGrid_ContainsTaskSection()
    {
        // Act
        var result = _prompt.CreateDataGrid("users");

        // Assert
        Assert.Contains("Task", result.Text);
        Assert.Contains("model class", result.Text);
        Assert.Contains("PropertyColumn", result.Text);
    }

    [Theory]
    [InlineData("users with id, name, email")]
    [InlineData("orders with date, total, status")]
    [InlineData("customers with name, address, phone")]
    public void CreateDataGrid_WithVariousData_GeneratesValidPrompt(string dataDescription)
    {
        // Act
        var result = _prompt.CreateDataGrid(dataDescription);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.Contains("FluentDataGrid", result.Text);
    }
}
