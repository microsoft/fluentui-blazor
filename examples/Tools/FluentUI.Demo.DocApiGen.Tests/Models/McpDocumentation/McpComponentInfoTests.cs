// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;
using FluentUI.Demo.DocApiGen.Models.McpDocumentation;

namespace FluentUI.Demo.DocApiGen.Tests.Models.McpDocumentation;

/// <summary>
/// Unit tests for <see cref="McpComponentInfo"/>.
/// </summary>
public class McpComponentInfoTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaults()
    {
        // Arrange & Act
        var component = new McpComponentInfo();

        // Assert
        Assert.Equal(string.Empty, component.Name);
        Assert.Equal(string.Empty, component.FullName);
        Assert.Equal(string.Empty, component.Summary);
        Assert.Equal(string.Empty, component.Category);
        Assert.False(component.IsGeneric);
        Assert.Null(component.BaseClass);
        Assert.NotNull(component.Properties);
        Assert.NotNull(component.Events);
        Assert.NotNull(component.Methods);
        Assert.Empty(component.Properties);
        Assert.Empty(component.Events);
        Assert.Empty(component.Methods);
    }

    [Fact]
    public void Name_ShouldBeSettable()
    {
        // Arrange
        var component = new McpComponentInfo();

        // Act
        component.Name = "FluentButton";

        // Assert
        Assert.Equal("FluentButton", component.Name);
    }

    [Fact]
    public void FullName_ShouldBeSettable()
    {
        // Arrange
        var component = new McpComponentInfo();

        // Act
        component.FullName = "Microsoft.FluentUI.AspNetCore.Components.FluentButton";

        // Assert
        Assert.Equal("Microsoft.FluentUI.AspNetCore.Components.FluentButton", component.FullName);
    }

    [Fact]
    public void Summary_ShouldBeSettable()
    {
        // Arrange
        var component = new McpComponentInfo();

        // Act
        component.Summary = "A button component";

        // Assert
        Assert.Equal("A button component", component.Summary);
    }

    [Fact]
    public void Category_ShouldBeSettable()
    {
        // Arrange
        var component = new McpComponentInfo();

        // Act
        component.Category = "Forms";

        // Assert
        Assert.Equal("Forms", component.Category);
    }

    [Fact]
    public void IsGeneric_ShouldBeSettable()
    {
        // Arrange
        var component = new McpComponentInfo();

        // Act
        component.IsGeneric = true;

        // Assert
        Assert.True(component.IsGeneric);
    }

    [Fact]
    public void BaseClass_ShouldBeSettableToNull()
    {
        // Arrange
        var component = new McpComponentInfo { BaseClass = "SomeBase" };

        // Act
        component.BaseClass = null;

        // Assert
        Assert.Null(component.BaseClass);
    }

    [Fact]
    public void BaseClass_ShouldBeSettableToValue()
    {
        // Arrange
        var component = new McpComponentInfo();

        // Act
        component.BaseClass = "FluentComponentBase";

        // Assert
        Assert.Equal("FluentComponentBase", component.BaseClass);
    }

    [Fact]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var component = new McpComponentInfo();
        var properties = new List<McpPropertyInfo>
        {
            new() { Name = "Appearance", Type = "Appearance?" },
            new() { Name = "Disabled", Type = "bool" }
        };

        // Act
        component.Properties = properties;

        // Assert
        Assert.Same(properties, component.Properties);
        Assert.Equal(2, component.Properties.Count);
    }

    [Fact]
    public void Events_ShouldBeSettable()
    {
        // Arrange
        var component = new McpComponentInfo();
        var events = new List<McpEventInfo>
        {
            new() { Name = "OnClick", Type = "EventCallback<MouseEventArgs>" }
        };

        // Act
        component.Events = events;

        // Assert
        Assert.Same(events, component.Events);
        Assert.Single(component.Events);
    }

    [Fact]
    public void Methods_ShouldBeSettable()
    {
        // Arrange
        var component = new McpComponentInfo();
        var methods = new List<McpMethodInfo>
        {
            new() { Name = "Focus", ReturnType = "Task" }
        };

        // Act
        component.Methods = methods;

        // Assert
        Assert.Same(methods, component.Methods);
        Assert.Single(component.Methods);
    }

    [Fact]
    public void CompleteObject_ShouldBeConstructedProperly()
    {
        // Arrange & Act
        var component = new McpComponentInfo
        {
            Name = "FluentButton",
            FullName = "Microsoft.FluentUI.AspNetCore.Components.FluentButton",
            Summary = "A button component",
            Category = "Forms",
            IsGeneric = false,
            BaseClass = "FluentComponentBase",
            Properties =
            [
                new McpPropertyInfo { Name = "Appearance", Type = "Appearance?" }
            ],
            Events =
            [
                new McpEventInfo { Name = "OnClick", Type = "EventCallback<MouseEventArgs>" }
            ],
            Methods =
            [
                new McpMethodInfo { Name = "Focus", ReturnType = "Task" }
            ]
        };

        // Assert
        Assert.Equal("FluentButton", component.Name);
        Assert.Equal("Microsoft.FluentUI.AspNetCore.Components.FluentButton", component.FullName);
        Assert.Equal("A button component", component.Summary);
        Assert.Equal("Forms", component.Category);
        Assert.False(component.IsGeneric);
        Assert.Equal("FluentComponentBase", component.BaseClass);
        Assert.Single(component.Properties);
        Assert.Single(component.Events);
        Assert.Single(component.Methods);
    }

    [Fact]
    public void Collections_CanBeModifiedAfterConstruction()
    {
        // Arrange
        var component = new McpComponentInfo();

        // Act
        component.Properties.Add(new McpPropertyInfo { Name = "Prop1" });
        component.Events.Add(new McpEventInfo { Name = "Event1" });
        component.Methods.Add(new McpMethodInfo { Name = "Method1" });

        // Assert
        Assert.Single(component.Properties);
        Assert.Single(component.Events);
        Assert.Single(component.Methods);
        Assert.Equal("Prop1", component.Properties[0].Name);
        Assert.Equal("Event1", component.Events[0].Name);
        Assert.Equal("Method1", component.Methods[0].Name);
    }
}
