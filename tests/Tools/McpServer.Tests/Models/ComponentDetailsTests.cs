// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Models;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Models;

/// <summary>
/// Tests for the <see cref="ComponentDetails"/> record.
/// </summary>
public class ComponentDetailsTests
{
    [Fact]
    public void ComponentDetails_WithRequiredProperties_ShouldBeCreatedSuccessfully()
    {
        // Arrange
        var componentInfo = new ComponentInfo
        {
            Name = "FluentButton",
            FullName = "Microsoft.FluentUI.AspNetCore.Components.FluentButton"
        };

        // Act
        var componentDetails = new ComponentDetails
        {
            Component = componentInfo
        };

        // Assert
        Assert.Equal(componentInfo, componentDetails.Component);
    }

    [Fact]
    public void ComponentDetails_DefaultValues_ShouldBeEmptyLists()
    {
        // Arrange
        var componentInfo = new ComponentInfo
        {
            Name = "FluentButton",
            FullName = "Microsoft.FluentUI.AspNetCore.Components.FluentButton"
        };

        // Act
        var componentDetails = new ComponentDetails
        {
            Component = componentInfo
        };

        // Assert
        Assert.Empty(componentDetails.Parameters);
        Assert.Empty(componentDetails.Properties);
        Assert.Empty(componentDetails.Events);
        Assert.Empty(componentDetails.Methods);
    }

    [Fact]
    public void ComponentDetails_WithParameters_ShouldStoreCorrectly()
    {
        // Arrange
        var componentInfo = new ComponentInfo
        {
            Name = "FluentButton",
            FullName = "Microsoft.FluentUI.AspNetCore.Components.FluentButton"
        };

        var parameters = new List<PropertyInfo>
        {
            new()
            {
                Name = "Appearance",
                Type = "Appearance?",
                Description = "The appearance of the button",
                IsParameter = true,
                EnumValues = ["Accent", "Lightweight", "Neutral"]
            },
            new()
            {
                Name = "Disabled",
                Type = "bool",
                Description = "Whether the button is disabled",
                IsParameter = true
            }
        };

        // Act
        var componentDetails = new ComponentDetails
        {
            Component = componentInfo,
            Parameters = parameters
        };

        // Assert
        Assert.Equal(2, componentDetails.Parameters.Count);
        Assert.Equal("Appearance", componentDetails.Parameters[0].Name);
        Assert.Equal("Disabled", componentDetails.Parameters[1].Name);
    }

    [Fact]
    public void ComponentDetails_WithAllCollections_ShouldStoreCorrectly()
    {
        // Arrange
        var componentInfo = new ComponentInfo
        {
            Name = "FluentButton",
            FullName = "Microsoft.FluentUI.AspNetCore.Components.FluentButton"
        };

        var parameters = new List<PropertyInfo>
        {
            new() { Name = "Appearance", Type = "Appearance?", IsParameter = true }
        };

        var properties = new List<PropertyInfo>
        {
            new() { Name = "Element", Type = "ElementReference", IsParameter = false }
        };

        var events = new List<EventInfo>
        {
            new() { Name = "OnClick", Type = "EventCallback<MouseEventArgs>" }
        };

        var methods = new List<MethodInfo>
        {
            new() { Name = "FocusAsync", ReturnType = "ValueTask" }
        };

        // Act
        var componentDetails = new ComponentDetails
        {
            Component = componentInfo,
            Parameters = parameters,
            Properties = properties,
            Events = events,
            Methods = methods
        };

        // Assert
        Assert.Single(componentDetails.Parameters);
        Assert.Single(componentDetails.Properties);
        Assert.Single(componentDetails.Events);
        Assert.Single(componentDetails.Methods);
    }
}
