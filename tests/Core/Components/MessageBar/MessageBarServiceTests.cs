// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.MessageBar;

public class MessageBarServiceTests : Bunit.BunitContext
{
    public MessageBarServiceTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddFluentUIComponents();
    }

    [Fact]
    public void MessageBarService_Subscribe_NullProviderId_NoOp()
    {
        // Arrange
        var service = (NotificationService)Services.GetRequiredService<INotificationService>();
        var serviceBase = (IFluentServiceBase<IMessageBarInstance>)service;

        // Act
        service.Subscribe(null, _ => Task.CompletedTask);

        // Assert: ProviderId remains null and the service is reported as not-available.
        Assert.Null(serviceBase.ProviderId);
        Assert.True(service.ProviderNotAvailable());
    }

    [Fact]
    public void MessageBarService_Subscribe_EmptyProviderId_NoOp()
    {
        // Arrange
        var service = (NotificationService)Services.GetRequiredService<INotificationService>();
        var serviceBase = (IFluentServiceBase<IMessageBarInstance>)service;

        // Act
        service.Subscribe(string.Empty, _ => Task.CompletedTask);

        // Assert
        Assert.Null(serviceBase.ProviderId);
    }

    [Fact]
    public void MessageBarService_Subscribe_NullCallback_NoOp()
    {
        // Arrange
        var service = (NotificationService)Services.GetRequiredService<INotificationService>();
        var serviceBase = (IFluentServiceBase<IMessageBarInstance>)service;

        // Act
        service.Subscribe("provider-1", null!);

        // Assert
        Assert.Null(serviceBase.ProviderId);
    }

    [Fact]
    public void MessageBarService_Unsubscribe_NullProviderId_NoOp()
    {
        // Arrange
        var service = (NotificationService)Services.GetRequiredService<INotificationService>();
        service.Subscribe("provider-1", _ => Task.CompletedTask);

        // Act
        service.Unsubscribe(null);

        // Assert: Existing subscription remains in place.
        var serviceBase = (IFluentServiceBase<IMessageBarInstance>)service;
        Assert.Equal("provider-1", serviceBase.ProviderId);
    }

    [Fact]
    public void MessageBarService_Unsubscribe_EmptyProviderId_NoOp()
    {
        // Arrange
        var service = (NotificationService)Services.GetRequiredService<INotificationService>();
        service.Subscribe("provider-1", _ => Task.CompletedTask);

        // Act
        service.Unsubscribe(string.Empty);

        // Assert: Existing subscription remains in place.
        var serviceBase = (IFluentServiceBase<IMessageBarInstance>)service;
        Assert.Equal("provider-1", serviceBase.ProviderId);
    }

    [Fact]
    public void MessageBarService_Unsubscribe_OnlyProvider_ResetsProviderId()
    {
        // Arrange
        var service = (NotificationService)Services.GetRequiredService<INotificationService>();
        var serviceBase = (IFluentServiceBase<IMessageBarInstance>)service;
        service.Subscribe("provider-1", _ => Task.CompletedTask);

        // Act
        service.Unsubscribe("provider-1");

        // Assert
        Assert.Null(serviceBase.ProviderId);
        Assert.True(service.ProviderNotAvailable());
    }

    [Fact]
    public void MessageBarService_Unsubscribe_FirstOfMany_SwitchesProviderId()
    {
        // Arrange
        var service = (NotificationService)Services.GetRequiredService<INotificationService>();
        var serviceBase = (IFluentServiceBase<IMessageBarInstance>)service;
        service.Subscribe("provider-1", _ => Task.CompletedTask);
        service.Subscribe("provider-2", _ => Task.CompletedTask);

        Assert.Equal("provider-2", serviceBase.ProviderId);

        // Act: unsubscribe the currently-exposed provider
        service.Unsubscribe("provider-2");

        // Assert: ProviderId switches to the remaining one
        Assert.Equal("provider-1", serviceBase.ProviderId);
    }

    [Fact]
    public void MessageBarService_Unsubscribe_NotCurrentProviderId_KeepsCurrent()
    {
        // Arrange
        var service = (NotificationService)Services.GetRequiredService<INotificationService>();
        var serviceBase = (IFluentServiceBase<IMessageBarInstance>)service;
        service.Subscribe("provider-1", _ => Task.CompletedTask);
        service.Subscribe("provider-2", _ => Task.CompletedTask);

        Assert.Equal("provider-2", serviceBase.ProviderId);

        // Act: unsubscribe a provider that is NOT the currently-exposed one
        service.Unsubscribe("provider-1");

        // Assert: the active ProviderId is unchanged
        Assert.Equal("provider-2", serviceBase.ProviderId);
    }

    [Fact]
    public async Task MessageBarService_Dispatch_SwallowsSubscriberExceptions()
    {
        // Arrange
        var service = (NotificationService)Services.GetRequiredService<INotificationService>();
        var goodCalled = false;

        service.Subscribe("bad", _ => throw new InvalidOperationException("boom"));
        service.Subscribe("good", _ =>
        {
            goodCalled = true;
            return Task.CompletedTask;
        });

        // Act: this throws inside one subscriber, but should not propagate to the show call.
        var task = service.ShowInfoBarAsync("section-X", "title");

        await Task.CompletedTask;

        // Assert
        Assert.True(goodCalled);

        // Cleanup
        await service.CloseAllMessageBarsAsync();
        await task;
    }
}
