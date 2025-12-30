// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocApiGen.Models.McpMode;
using Xunit;

namespace FluentUI.Demo.DocApiGen.Tests.Models.McpMode;

/// <summary>
/// Unit tests for <see cref="McpPromptInfo"/>.
/// </summary>
public class McpPromptInfoTests
{
    [Fact]
    public void Constructor_ShouldRequireNameAndClassName()
    {
        // Arrange & Act
        var prompt = new McpPromptInfo
        {
            Name = "CreateComponent",
            ClassName = "ComponentPrompts"
        };

        // Assert
        Assert.Equal("CreateComponent", prompt.Name);
        Assert.Equal("ComponentPrompts", prompt.ClassName);
    }

    [Fact]
    public void OptionalProperties_ShouldBeNullByDefault()
    {
        // Arrange & Act
        var prompt = new McpPromptInfo
        {
            Name = "TestPrompt",
            ClassName = "TestClass"
        };

        // Assert
        Assert.Null(prompt.Description);
        Assert.Null(prompt.Summary);
        Assert.Null(prompt.Parameters);
    }

    [Fact]
    public void Description_ShouldBeSettable()
    {
        // Arrange & Act
        var prompt = new McpPromptInfo
        {
            Name = "CreateComponent",
            ClassName = "ComponentPrompts",
            Description = "Guides the creation of a new component"
        };

        // Assert
        Assert.Equal("Guides the creation of a new component", prompt.Description);
    }

    [Fact]
    public void Summary_ShouldBeSettable()
    {
        // Arrange & Act
        var prompt = new McpPromptInfo
        {
            Name = "CreateComponent",
            ClassName = "ComponentPrompts",
            Summary = "XML documentation summary for the prompt"
        };

        // Assert
        Assert.Equal("XML documentation summary for the prompt", prompt.Summary);
    }

    [Fact]
    public void Parameters_ShouldBeSettable()
    {
        // Arrange
        var parameters = new List<McpParameterInfo>
        {
            new()
            {
                Name = "componentName",
                Type = "string",
                Description = "Name of the component to create",
                IsRequired = true
            }
        };

        // Act
        var prompt = new McpPromptInfo
        {
            Name = "CreateComponent",
            ClassName = "ComponentPrompts",
            Parameters = parameters
        };

        // Assert
        Assert.NotNull(prompt.Parameters);
        Assert.Single(prompt.Parameters);
        Assert.Equal("componentName", prompt.Parameters[0].Name);
    }

    [Fact]
    public void CompletePrompt_ShouldBeConstructedProperly()
    {
        // Arrange & Act
        var prompt = new McpPromptInfo
        {
            Name = "CreateForm",
            ClassName = "FormPrompts",
            Description = "Guides creation of a form with validation",
            Summary = "Helps create a complete form using Fluent UI Blazor components.",
            Parameters =
            [
                new McpParameterInfo
                {
                    Name = "formName",
                    Type = "string",
                    Description = "The name of the form",
                    IsRequired = true
                },
                new McpParameterInfo
                {
                    Name = "includeValidation",
                    Type = "bool",
                    Description = "Whether to include validation",
                    IsRequired = false,
                    DefaultValue = "true"
                }
            ]
        };

        // Assert
        Assert.Equal("CreateForm", prompt.Name);
        Assert.Equal("FormPrompts", prompt.ClassName);
        Assert.Equal("Guides creation of a form with validation", prompt.Description);
        Assert.NotNull(prompt.Summary);
        Assert.NotNull(prompt.Parameters);
        Assert.Equal(2, prompt.Parameters.Count);
        Assert.True(prompt.Parameters[0].IsRequired);
        Assert.False(prompt.Parameters[1].IsRequired);
    }
}
