// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Prompts;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tests.Prompts;

public class CreateFormPromptTests
{
    private readonly CreateFormPrompt _prompt;
    private readonly FluentUIDocumentationService _documentationService;

    public CreateFormPromptTests()
    {
        var jsonPath = JsonDocumentationFinder.Find();
        _documentationService = new FluentUIDocumentationService(jsonPath);
        _prompt = new CreateFormPrompt(_documentationService);
    }

    [Fact]
    public void CreateForm_WithFormFields_ReturnsNonEmptyMessage()
    {
        // Arrange
        var formFields = "name, email, phone";

        // Act
        var result = _prompt.CreateForm(formFields);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.NotEmpty(result.Text);
        Assert.Contains(formFields, result.Text);
    }

    [Fact]
    public void CreateForm_WithModelName_IncludesModelInMessage()
    {
        // Arrange
        var formFields = "name, email";
        var modelName = "ContactModel";

        // Act
        var result = _prompt.CreateForm(formFields, modelName);

        // Assert
        Assert.NotNull(result);
        Assert.Contains(modelName, result.Text);
        Assert.Contains("Model Class", result.Text);
    }

    [Fact]
    public void CreateForm_WithValidation_IncludesValidationInMessage()
    {
        // Arrange
        var formFields = "name, email";
        var validation = "email required and valid";

        // Act
        var result = _prompt.CreateForm(formFields, null, validation);

        // Assert
        Assert.NotNull(result);
        Assert.Contains(validation, result.Text);
        Assert.Contains("Validation", result.Text);
    }

    [Fact]
    public void CreateForm_IncludesFormComponents()
    {
        // Arrange
        var formFields = "name, email, phone";

        // Act
        var result = _prompt.CreateForm(formFields);

        // Assert
        Assert.Contains("Available Form Components", result.Text);
        Assert.Contains("FluentTextField", result.Text);
        Assert.Contains("EditForm", result.Text);
    }

    [Theory]
    [InlineData("name, email")]
    [InlineData("first name, last name, address")]
    [InlineData("username, password, confirm password")]
    public void CreateForm_WithVariousFields_GeneratesValidPrompt(string formFields)
    {
        // Act
        var result = _prompt.CreateForm(formFields);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Text);
        Assert.Contains("Task", result.Text);
        Assert.Contains("data annotations", result.Text);
    }
}
