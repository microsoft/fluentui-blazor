// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Models;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Models;

/// <summary>
/// Tests for the <see cref="EventInfo"/> record.
/// </summary>
public class EventInfoTests
{
    [Fact]
    public void EventInfo_WithRequiredProperties_ShouldBeCreatedSuccessfully()
    {
        // Arrange & Act
        var eventInfo = new EventInfo
        {
            Name = "OnClick",
            Type = "EventCallback<MouseEventArgs>"
        };

        // Assert
        Assert.Equal("OnClick", eventInfo.Name);
        Assert.Equal("EventCallback<MouseEventArgs>", eventInfo.Type);
    }

    [Fact]
    public void EventInfo_DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var eventInfo = new EventInfo
        {
            Name = "OnClick",
            Type = "EventCallback<MouseEventArgs>"
        };

        // Assert
        Assert.Equal(string.Empty, eventInfo.Description);
        Assert.False(eventInfo.IsInherited);
    }

    [Fact]
    public void EventInfo_WithAllProperties_ShouldBeCreatedSuccessfully()
    {
        // Arrange & Act
        var eventInfo = new EventInfo
        {
            Name = "OnClick",
            Type = "EventCallback<MouseEventArgs>",
            Description = "Raised when the button is clicked",
            IsInherited = false
        };

        // Assert
        Assert.Equal("OnClick", eventInfo.Name);
        Assert.Equal("EventCallback<MouseEventArgs>", eventInfo.Type);
        Assert.Equal("Raised when the button is clicked", eventInfo.Description);
        Assert.False(eventInfo.IsInherited);
    }

    [Fact]
    public void EventInfo_WithIsInherited_ShouldBeCreatedSuccessfully()
    {
        // Arrange & Act
        var eventInfo = new EventInfo
        {
            Name = "OnFocus",
            Type = "EventCallback<FocusEventArgs>",
            IsInherited = true
        };

        // Assert
        Assert.True(eventInfo.IsInherited);
    }

    [Fact]
    public void EventInfo_RecordEquality_ShouldWorkCorrectly()
    {
        // Arrange
        var eventInfo1 = new EventInfo
        {
            Name = "OnClick",
            Type = "EventCallback<MouseEventArgs>",
            Description = "Click event"
        };

        var eventInfo2 = new EventInfo
        {
            Name = "OnClick",
            Type = "EventCallback<MouseEventArgs>",
            Description = "Click event"
        };

        // Act & Assert
        Assert.Equal(eventInfo1, eventInfo2);
    }

    [Fact]
    public void EventInfo_RecordInequality_ShouldWorkCorrectly()
    {
        // Arrange
        var eventInfo1 = new EventInfo
        {
            Name = "OnClick",
            Type = "EventCallback<MouseEventArgs>"
        };

        var eventInfo2 = new EventInfo
        {
            Name = "OnChange",
            Type = "EventCallback<ChangeEventArgs>"
        };

        // Act & Assert
        Assert.NotEqual(eventInfo1, eventInfo2);
    }
}
