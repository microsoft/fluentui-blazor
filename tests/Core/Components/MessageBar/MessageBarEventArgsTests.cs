// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.MessageBar;

public class MessageBarEventArgsTests
{
    [Fact]
    public void MessageBarEventArgs_SetsAllProperties()
    {
        // Arrange
        var service = new NotificationService();
        var options = new MessageBarOptions { Section = "section-A", Id = "my-id" };
        var instance = new MessageBarInstance(service, options);

        // Act
        var args = new MessageBarEventArgs(instance, MessageBarLifecycleStatus.Dismissed);

        // Assert
        Assert.Equal("my-id", args.Id);
        Assert.Equal(MessageBarLifecycleStatus.Dismissed, args.Status);
        Assert.Same(instance, args.Instance);
    }

    [Theory]
    [InlineData(MessageBarLifecycleStatus.Visible)]
    [InlineData(MessageBarLifecycleStatus.Dismissed)]
    [InlineData(MessageBarLifecycleStatus.Unmounted)]
    public void MessageBarEventArgs_KeepsStatus(MessageBarLifecycleStatus status)
    {
        // Arrange
        var service = new NotificationService();
        var instance = new MessageBarInstance(service, new MessageBarOptions { Section = "section" });

        // Act
        var args = new MessageBarEventArgs(instance, status);

        // Assert
        Assert.Equal(status, args.Status);
    }
}
