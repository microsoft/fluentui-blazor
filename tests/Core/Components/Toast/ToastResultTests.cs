// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Toast;

public class ToastResultTests : Bunit.BunitContext
{
    public ToastResultTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddFluentUIComponents();
    }

    private IToastInstance CreateInstance(string? id = null)
    {
        var service = Services.GetRequiredService<INotificationService>();
        return new ToastInstance(service, new ToastOptions { Id = id });
    }

    [Fact]
    public void ToastResult_Constructor_ThrowsOnNullInstance()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new ToastResult(null!, ToastCloseReason.Dismissed, data: null));
    }

    [Fact]
    public void ToastResult_Constructor_SetsProperties()
    {
        // Arrange
        var instance = CreateInstance();
        var data = new object();

        // Act
        var result = new ToastResult(instance, ToastCloseReason.QuickAction, data);

        // Assert
        Assert.Equal(ToastCloseReason.QuickAction, result.Reason);
        Assert.Same(data, result.Data);
        Assert.Same(instance, result.Instance);
    }

    [Fact]
    public void ToastResult_OfDismissed_SetsReason()
    {
        // Arrange
        var instance = CreateInstance();

        // Act
        var result = ToastResult.OfDismissed(instance, "payload");

        // Assert
        Assert.Equal(ToastCloseReason.Dismissed, result.Reason);
        Assert.Equal("payload", result.Data);
        Assert.Same(instance, result.Instance);
    }

    [Fact]
    public void ToastResult_OfQuickAction_SetsReason()
    {
        // Arrange
        var instance = CreateInstance();

        // Act
        var result = ToastResult.OfQuickAction(instance);

        // Assert
        Assert.Equal(ToastCloseReason.QuickAction, result.Reason);
    }

    [Fact]
    public void ToastResult_OfProgrammatic_SetsReason()
    {
        // Arrange
        var instance = CreateInstance();

        // Act
        var result = ToastResult.OfProgrammatic(instance, 123);

        // Assert
        Assert.Equal(ToastCloseReason.Programmatic, result.Reason);
        Assert.Equal(123, result.Data);
    }

    [Fact]
    public void ToastResult_OfTimedOut_SetsReason()
    {
        // Arrange
        var instance = CreateInstance();

        // Act
        var result = ToastResult.OfTimedOut(instance);

        // Assert
        Assert.Equal(ToastCloseReason.TimedOut, result.Reason);
    }

    [Fact]
    public void ToastResult_OfVisible_SetsProgrammaticReasonAndNoData()
    {
        // Arrange
        var instance = CreateInstance();

        // Act
        var result = ToastResult.OfVisible(instance);

        // Assert
        Assert.Equal(ToastCloseReason.Programmatic, result.Reason);
        Assert.Null(result.Data);
    }

    [Fact]
    public void ToastResult_OfQueued_SetsProgrammaticReasonAndNoData()
    {
        // Arrange
        var instance = CreateInstance();

        // Act
        var result = ToastResult.OfQueued(instance);

        // Assert
        Assert.Equal(ToastCloseReason.Programmatic, result.Reason);
        Assert.Null(result.Data);
    }
}
