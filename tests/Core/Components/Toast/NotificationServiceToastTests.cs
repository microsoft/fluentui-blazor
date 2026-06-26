// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Toast;

public class NotificationServiceToastTests : Bunit.BunitContext
{
    private const string TEST_PROVIDER = "toast-provider";

    public NotificationServiceToastTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddFluentUIComponents();
    }

    private NotificationService GetService()
        => (NotificationService)Services.GetRequiredService<INotificationService>();

    /// <summary>
    /// Subscribes a fake provider so that <see cref="NotificationService"/> considers a provider available.
    /// </summary>
    private NotificationService GetServiceWithProvider()
    {
        var service = GetService();
        service.Subscribe(TEST_PROVIDER, _ => Task.CompletedTask);
        return service;
    }

    private static IToastInstance SingleToast(NotificationService service)
        => ((IFluentServiceBase<INotificationInstance>)service).Items.Values.OfType<IToastInstance>().Single();

    [Fact]
    public async Task ShowToastAsync_WithoutProvider_Throws()
    {
        // Arrange
        var service = GetService();
        var options = new ToastOptions { Title = "Title" };

        // Act & Assert
        await Assert.ThrowsAsync<FluentServiceProviderException<FluentToastProvider>>(
            () => service.ShowToastAsync(options));
    }

    [Fact]
    public async Task ShowToastAsync_NullOptions_Throws()
    {
        // Arrange
        var service = GetServiceWithProvider();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => service.ShowToastAsync((ToastOptions)null!));
    }

    [Fact]
    public void ShowSuccessToastAsync_SetsSuccessIntent()
    {
        // Arrange
        var service = GetServiceWithProvider();

        // Act
        _ = service.ShowSuccessToastAsync("Title", "Message", lifetime: null);

        // Assert
        var instance = SingleToast(service);
        Assert.Equal(ToastResultTiming.Queued, instance.Options.ResultTiming);
        Assert.Equal(ToastIntent.Success, instance.Options.Intent);
        Assert.Equal("Title", instance.Options.Title);
        Assert.Equal("Message", instance.Options.Message);
    }

    [Fact]
    public void ShowWarningToastAsync_SetsWarningIntent()
    {
        // Arrange
        var service = GetServiceWithProvider();

        // Act
        _ = service.ShowWarningToastAsync("Title", lifetime: null);

        // Assert
        Assert.Equal(ToastIntent.Warning, SingleToast(service).Options.Intent);
    }

    [Fact]
    public void ShowErrorToastAsync_SetsErrorIntent()
    {
        // Arrange
        var service = GetServiceWithProvider();

        // Act
        _ = service.ShowErrorToastAsync("Title", lifetime: null);

        // Assert
        Assert.Equal(ToastIntent.Error, SingleToast(service).Options.Intent);
    }

    [Fact]
    public void ShowInfoToastAsync_SetsInfoIntent()
    {
        // Arrange
        var service = GetServiceWithProvider();

        // Act
        _ = service.ShowInfoToastAsync("Title", lifetime: null);

        // Assert
        Assert.Equal(ToastIntent.Info, SingleToast(service).Options.Intent);
    }

    [Fact]
    public void ShowProgressToastAsync_SetsProgressIntentAndVisibleTiming()
    {
        // Arrange
        var service = GetServiceWithProvider();

        // Act
        _ = service.ShowProgressToastAsync("Title", lifetime: null);

        // Assert
        var instance = SingleToast(service);
        Assert.Equal(ToastIntent.Progress, instance.Options.Intent);
        Assert.Equal(ToastResultTiming.Queued, instance.Options.ResultTiming);
    }

    [Fact]
    public void ShowSimpleToastAsync_WithLifetime_SetsLifetime()
    {
        // Arrange
        var service = GetServiceWithProvider();

        // Act
        _ = service.ShowInfoToastAsync("Title", lifetime: 7);

        // Assert
        Assert.Equal(TimeSpan.FromSeconds(7), SingleToast(service).Options.Lifetime);
    }

    [Fact]
    public void ShowSimpleToastAsync_WithDismissLabel_SetsAllowDismissAndLabel()
    {
        // Arrange
        var service = GetServiceWithProvider();

        // Act
        _ = service.ShowInfoToastAsync("Title", dismissLabel: "Close", lifetime: null);

        // Assert
        var instance = SingleToast(service);
        Assert.True(instance.Options.AllowDismiss);
        Assert.Equal("Close", instance.Options.DismissAction.Label);
        Assert.NotNull(instance.Options.DismissAction.OnClickAsync);
    }

    [Fact]
    public void ShowToastAsync_AddsInstanceAndRaisesQueuedStatus()
    {
        // Arrange
        var service = GetServiceWithProvider();

        // Act
        _ = service.ShowToastAsync(new ToastOptions { Title = "Title" });

        // Assert
        var instance = SingleToast(service);
        Assert.NotNull(instance);
        Assert.NotEqual(ToastLifecycleStatus.Unmounted, instance.LifecycleStatus);
    }

    [Fact]
    public void ShowToastAsync_WithAction_BuildsOptions()
    {
        // Arrange
        var service = GetServiceWithProvider();

        // Act
        _ = service.ShowToastAsync(o => o.Title = "Configured");

        // Assert
        Assert.Equal("Configured", SingleToast(service).Options.Title);
    }

    [Fact]
    public void ShowToastAsync_OfComponent_SetsComponentType()
    {
        // Arrange
        var service = GetServiceWithProvider();

        // Act
        _ = service.ShowToastAsync<FluentToast>(new ToastOptions { Title = "Custom" });

        // Assert
        var instance = (INotificationInstance)SingleToast(service);
        Assert.Equal(typeof(FluentToast), instance.ComponentType);
    }

    [Fact]
    public async Task ShowToastAsync_DuplicateId_Throws()
    {
        // Arrange
        var service = GetServiceWithProvider();
        _ = service.ShowToastAsync(new ToastOptions { Id = "dup", Title = "First" });

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.ShowToastAsync(new ToastOptions { Id = "dup", Title = "Second" }));
    }

    [Fact]
    public void GetToastInstance_ReturnsInstance()
    {
        // Arrange
        var service = GetServiceWithProvider();
        _ = service.ShowToastAsync(new ToastOptions { Id = "get-id", Title = "Title" });

        // Act
        var instance = service.GetToastInstance("get-id");

        // Assert
        Assert.NotNull(instance);
        Assert.Equal("get-id", instance!.Id);
    }

    [Fact]
    public void GetToastInstance_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var service = GetServiceWithProvider();

        // Act
        var instance = service.GetToastInstance("missing");

        // Assert
        Assert.Null(instance);
    }

    [Fact]
    public async Task CloseAsync_ByInstance_SetsDismissedStatus()
    {
        // Arrange
        var service = GetServiceWithProvider();
        _ = service.ShowToastAsync(new ToastOptions { Id = "close-id", Title = "Title" });
        var instance = service.GetToastInstance("close-id")!;

        // Act
        await service.CloseAsync(instance);

        // Assert
        Assert.Equal(ToastLifecycleStatus.Dismissed, instance.LifecycleStatus);
    }

    [Fact]
    public async Task CloseAsync_WithToastResult_UsesProvidedResult()
    {
        // Arrange
        var service = GetServiceWithProvider();
        _ = service.ShowToastAsync(new ToastOptions { Id = "close-id", Title = "Title", ResultTiming = ToastResultTiming.Closed });
        var instance = service.GetToastInstance("close-id")!;
        var providedResult = ToastResult.OfQuickAction(instance, "payload");

        // Act
        await service.CloseAsync(instance, providedResult);
        var result = await instance.Result;

        // Assert
        Assert.Equal(ToastCloseReason.QuickAction, result.Reason);
        Assert.Equal("payload", result.Data);
    }

    [Fact]
    public async Task CloseAsync_ById_ReturnsTrue_AndClosesToast()
    {
        // Arrange
        var service = GetServiceWithProvider();
        _ = service.ShowToastAsync(new ToastOptions { Id = "by-id", Title = "Title" });

        // Act
        var closed = await service.CloseAsync("by-id");

        // Assert
        Assert.True(closed);
    }

    [Fact]
    public async Task CloseAsync_ById_ReturnsFalse_WhenMissing()
    {
        // Arrange
        var service = GetServiceWithProvider();

        // Act
        var closed = await service.CloseAsync("missing");

        // Assert
        Assert.False(closed);
    }

    [Fact]
    public async Task CloseAllToastsAsync_ClosesAll_AndReturnsCount()
    {
        // Arrange
        var service = GetServiceWithProvider();
        _ = service.ShowToastAsync(new ToastOptions { Id = "a", Title = "A" });
        _ = service.ShowToastAsync(new ToastOptions { Id = "b", Title = "B" });

        // Act
        var count = await service.CloseAllToastsAsync();

        // Assert
        Assert.Equal(2, count);
    }

    [Fact]
    public void ShowToastAsync_OfCustomComponent_SetsComponentTypeAndParameters()
    {
        // Arrange
        var service = GetServiceWithProvider();

        // Act
        _ = service.ShowToastAsync<Templates.CustomToastTemplate>(o =>
        {
            o.Parameters[nameof(Templates.CustomToastTemplate.Text)] = "from-test";
        });

        // Assert
        var instance = SingleToast(service);
        Assert.Equal(typeof(Templates.CustomToastTemplate), ((INotificationInstance)instance).ComponentType);
        Assert.Equal("from-test", instance.Options.Parameters[nameof(Templates.CustomToastTemplate.Text)]);
    }

    [Fact]
    public void ShowToastAsync_OfCustomComponent_WithOptions_SetsComponentTypeAndParameters()
    {
        // Arrange
        var service = GetServiceWithProvider();
        var options = new ToastOptions();
        options.Parameters[nameof(Templates.CustomToastTemplate.Text)] = "options-text";

        // Act
        _ = service.ShowToastAsync<Templates.CustomToastTemplate>(options);

        // Assert
        var instance = SingleToast(service);
        Assert.Equal(typeof(Templates.CustomToastTemplate), ((INotificationInstance)instance).ComponentType);
        Assert.Equal("options-text", instance.Options.Parameters[nameof(Templates.CustomToastTemplate.Text)]);
    }

    [Fact]
    public void ShowToastAsync_OfCustomComponent_RendersTemplateInProvider()
    {
        // Arrange: render the real provider so the toast is displayed.
        var cut = Render<FluentToastProvider>();
        var service = (NotificationService)Services.GetRequiredService<INotificationService>();

        // Act
        _ = service.ShowToastAsync<Templates.CustomToastTemplate>(o =>
        {
            o.Parameters[nameof(Templates.CustomToastTemplate.Text)] = "from-test";
        });

        // Assert
        cut.WaitForAssertion(() =>
        {
            Assert.Contains("custom-toast", cut.Markup);
            Assert.Contains("from-test", cut.Markup);
        });
    }

    [Fact]
    public async Task ShowSimpleToastAsync_DismissOnClick_InvokesCallbackAndClosesToast()
    {
        // Arrange
        var service = GetServiceWithProvider();
        var callbackInvoked = false;

        _ = service.ShowInfoToastAsync(
            "Title",
            dismissLabel: "Close",
            lifetime: null,
            dismissOnClickAsync: _ =>
            {
                callbackInvoked = true;
                return Task.CompletedTask;
            });

        var instance = SingleToast(service);
        var args = new ToastEventArgs(instance, ToastLifecycleStatus.Visible);

        // Act
        await instance.Options.DismissAction.OnClickAsync!.Invoke(args);

        // Assert
        Assert.True(callbackInvoked);
        Assert.Equal(ToastLifecycleStatus.Dismissed, instance.LifecycleStatus);
    }

    [Fact]
    public async Task ShowSimpleToastAsync_DismissOnClick_WithoutCallback_ClosesToast()
    {
        // Arrange
        var service = GetServiceWithProvider();

        _ = service.ShowInfoToastAsync("Title", dismissLabel: "Close", lifetime: null);

        var instance = SingleToast(service);
        var args = new ToastEventArgs(instance, ToastLifecycleStatus.Visible);

        // Act
        await instance.Options.DismissAction.OnClickAsync!.Invoke(args);

        // Assert
        Assert.Equal(ToastLifecycleStatus.Dismissed, instance.LifecycleStatus);
    }

    [Fact]
    public async Task CloseCoreAsync_NonToastInstance_DoesNothing()
    {
        // Arrange
        var service = GetServiceWithProvider();
        var fake = new FakeToastInstance();

        // Act & Assert (does not throw and returns)
        await service.CloseAsync(fake);
    }

    [Fact]
    public async Task CloseCoreAsync_AlreadyUnmounted_DoesNotSetResult()
    {
        // Arrange
        var service = GetServiceWithProvider();

        // A freshly created instance defaults to the Unmounted status.
        var instance = new ToastInstance(service, new ToastOptions { Id = "unmounted" });
        Assert.Equal(ToastLifecycleStatus.Unmounted, instance.LifecycleStatus);

        // Act
        await service.CloseAsync(instance);

        // Assert
        Assert.False(instance.Result.IsCompleted);
    }

    [Fact]
    public async Task RemoveToastFromProviderAsync_Null_DoesNothing()
    {
        // Arrange
        var service = GetServiceWithProvider();

        // Act & Assert (does not throw)
        await service.RemoveToastFromProviderAsync(null);
    }

    [Fact]
    public async Task RemoveToastFromProviderAsync_NonToastInstance_DoesNothing()
    {
        // Arrange
        var service = GetServiceWithProvider();

        // Act & Assert (does not throw)
        await service.RemoveToastFromProviderAsync(new FakeToastInstance());
    }

    [Fact]
    public async Task RemoveToastFromProviderAsync_RemovesToast_AndRaisesUnmountedStatus()
    {
        // Arrange
        var service = GetService();
        INotificationInstance? notified = null;
        service.Subscribe(TEST_PROVIDER, instance =>
        {
            notified = instance;
            return Task.CompletedTask;
        });

        _ = service.ShowToastAsync(new ToastOptions { Id = "remove-id", Title = "Title" });
        var instance = service.GetToastInstance("remove-id")!;

        // Act
        await service.CloseAsync(instance);

        // Assert: the fire-and-forget removal completes after the closing animation delay.
        await WaitForAsync(() => service.GetToastInstance("remove-id") is null);
        Assert.Null(service.GetToastInstance("remove-id"));
        Assert.Equal(ToastLifecycleStatus.Unmounted, instance.LifecycleStatus);
        Assert.NotNull(notified);
        Assert.Equal("remove-id", notified!.Id);
    }

    private static async Task WaitForAsync(Func<bool> condition, int timeoutMilliseconds = 5000)
    {
        var elapsed = 0;
        const int interval = 50;

        while (!condition() && elapsed < timeoutMilliseconds)
        {
            await Task.Delay(interval);
            elapsed += interval;
        }
    }

    /// <summary>
    /// A test double implementing <see cref="IToastInstance"/> that is not a <see cref="ToastInstance"/>,
    /// used to exercise the type-guard branches of the service.
    /// </summary>
    private sealed class FakeToastInstance : IToastInstance
    {
        public ToastOptions Options { get; } = new();

        public Task<ToastResult> Result => Task.FromResult(ToastResult.OfProgrammatic(this));

        public ToastLifecycleStatus LifecycleStatus => ToastLifecycleStatus.Queued;

        public string Id { get; } = "fake-id";

        public long Index => 0;

        Type? INotificationInstance.ComponentType => null;

        public Task CloseAsync() => Task.CompletedTask;

        public Task CloseAsync(ToastCloseReason reason, object? data = null) => Task.CompletedTask;
    }
}

