// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.MessageBar;

public class MessageBarResultTests
{
    [Fact]
    public void MessageBarResult_OfDismissed_NoData_SetsReason()
    {
        // Act
        var result = MessageBarResult.OfDismissed();

        // Assert
        Assert.Equal(MessageBarCloseReason.Dismissed, result.Reason);
        Assert.Null(result.Data);
    }

    [Fact]
    public void MessageBarResult_OfDismissed_WithData_SetsData()
    {
        // Arrange
        var data = new { Key = "value" };

        // Act
        var result = MessageBarResult.OfDismissed(data);

        // Assert
        Assert.Equal(MessageBarCloseReason.Dismissed, result.Reason);
        Assert.Same(data, result.Data);
    }

    [Fact]
    public void MessageBarResult_OfProgrammatic_NoData_SetsReason()
    {
        // Act
        var result = MessageBarResult.OfProgrammatic();

        // Assert
        Assert.Equal(MessageBarCloseReason.Programmatic, result.Reason);
        Assert.Null(result.Data);
    }

    [Fact]
    public void MessageBarResult_OfProgrammatic_WithData_SetsData()
    {
        // Act
        var result = MessageBarResult.OfProgrammatic("hello");

        // Assert
        Assert.Equal(MessageBarCloseReason.Programmatic, result.Reason);
        Assert.Equal("hello", result.Data);
    }

    [Fact]
    public void MessageBarResult_OfTimedOut_NoData_SetsReason()
    {
        // Act
        var result = MessageBarResult.OfTimedOut();

        // Assert
        Assert.Equal(MessageBarCloseReason.TimedOut, result.Reason);
        Assert.Null(result.Data);
    }

    [Fact]
    public void MessageBarResult_OfTimedOut_WithData_SetsData()
    {
        // Act
        var result = MessageBarResult.OfTimedOut(42);

        // Assert
        Assert.Equal(MessageBarCloseReason.TimedOut, result.Reason);
        Assert.Equal(42, result.Data);
    }

    [Fact]
    public void MessageBarResult_OfVisible_NoData_SetsReason()
    {
        // Act
        var result = MessageBarResult.OfVisible();

        // Assert
        Assert.Equal(MessageBarCloseReason.Programmatic, result.Reason);
        Assert.Null(result.Data);
    }

    [Fact]
    public void MessageBarResult_OfVisible_WithData_SetsData()
    {
        // Act
        var result = MessageBarResult.OfVisible("shown");

        // Assert
        Assert.Equal(MessageBarCloseReason.Programmatic, result.Reason);
        Assert.Equal("shown", result.Data);
    }
}
