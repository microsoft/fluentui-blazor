// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Toast;

public class ToastEventArgsTests : Bunit.BunitContext
{
    public ToastEventArgsTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddFluentUIComponents();
    }

    [Fact]
    public void ToastEventArgs_SetsAllProperties()
    {
        // Arrange
        var service = Services.GetRequiredService<INotificationService>();
        var instance = new ToastInstance(service, new ToastOptions { Id = "my-id" });

        // Act
        var args = new ToastEventArgs(instance, ToastLifecycleStatus.Dismissed);

        // Assert
        Assert.Equal("my-id", args.Id);
        Assert.Equal(ToastLifecycleStatus.Dismissed, args.Status);
        Assert.Same(instance, args.Instance);
    }

    [Theory]
    [InlineData(ToastLifecycleStatus.Queued)]
    [InlineData(ToastLifecycleStatus.Visible)]
    [InlineData(ToastLifecycleStatus.Dismissed)]
    [InlineData(ToastLifecycleStatus.Unmounted)]
    public void ToastEventArgs_KeepsStatus(ToastLifecycleStatus status)
    {
        // Arrange
        var service = Services.GetRequiredService<INotificationService>();
        var instance = new ToastInstance(service, new ToastOptions());

        // Act
        var args = new ToastEventArgs(instance, status);

        // Assert
        Assert.Equal(status, args.Status);
    }
}
