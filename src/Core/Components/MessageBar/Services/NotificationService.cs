// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Service for showing message bars and toasts.
/// </summary>
public partial class NotificationService : FluentServiceBase<INotificationInstance>, INotificationService
{
    /// <inheritdoc cref="INotificationService.ShowSuccessBarAsync(string, string?, string?)"/>
    public Task<MessageBarResult> ShowSuccessBarAsync(string section, string? title = null, string? message = null)
    {
        return ShowMessageBarAsync(options =>
        {
            options.Section = section;
            options.Intent = MessageBarIntent.Success;
            options.Title = title;
            options.Message = message;
        });
    }

    /// <inheritdoc cref="INotificationService.ShowWarningBarAsync(string, string?, string?)"/>
    public Task<MessageBarResult> ShowWarningBarAsync(string section, string? title = null, string? message = null)
    {
        return ShowMessageBarAsync(options =>
        {
            options.Section = section;
            options.Intent = MessageBarIntent.Warning;
            options.Title = title;
            options.Message = message;
        });
    }

    /// <inheritdoc cref="INotificationService.ShowErrorBarAsync(string, string?, string?)"/>
    public Task<MessageBarResult> ShowErrorBarAsync(string section, string? title = null, string? message = null)
    {
        return ShowMessageBarAsync(options =>
        {
            options.Section = section;
            options.Intent = MessageBarIntent.Error;
            options.Title = title;
            options.Message = message;
        });
    }

    /// <inheritdoc cref="INotificationService.ShowInfoBarAsync(string, string?, string?)"/>
    public Task<MessageBarResult> ShowInfoBarAsync(string section, string? title = null, string? message = null)
    {
        return ShowMessageBarAsync(options =>
        {
            options.Section = section;
            options.Intent = MessageBarIntent.Info;
            options.Title = title;
            options.Message = message;
        });
    }

    /// <inheritdoc cref="INotificationService.ShowMessageBarAsync(MessageBarOptions)"/>
    public async Task<MessageBarResult> ShowMessageBarAsync(MessageBarOptions options)
    {
        var instance = ShowMessageInstanceCore(componentType: null, options);
        await ServiceProvider.OnUpdatedAsync.Invoke(instance);
        return await instance.Result;
    }

    /// <inheritdoc cref="INotificationService.ShowMessageBarAsync(Action{MessageBarOptions})"/>
    public Task<MessageBarResult> ShowMessageBarAsync(Action<MessageBarOptions> options)
        => ShowMessageBarAsync(new MessageBarOptions(options));

    /// <inheritdoc cref="INotificationService.ShowMessageBarAsync{TMessageBar}(MessageBarOptions)"/>
    public async Task<MessageBarResult> ShowMessageBarAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TMessageBar>(MessageBarOptions options)
        where TMessageBar : ComponentBase
    {
        var instance = ShowMessageInstanceCore(typeof(TMessageBar), options);
        await ServiceProvider.OnUpdatedAsync.Invoke(instance);
        return await instance.Result;
    }

    /// <inheritdoc cref="INotificationService.ShowMessageBarAsync{TMessageBar}(Action{MessageBarOptions})"/>
    public Task<MessageBarResult> ShowMessageBarAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TMessageBar>(Action<MessageBarOptions> options)
        where TMessageBar : ComponentBase
        => ShowMessageBarAsync<TMessageBar>(new MessageBarOptions(options));

    /// <inheritdoc cref="INotificationService.CloseAsync(IMessageBarInstance, object?)"/>
    public Task CloseAsync(IMessageBarInstance messageBar, object? data = null)
    {
        if (data is not null and MessageBarResult result)
        {
            return CloseCoreAsync(messageBar, result);
        }

        return CloseCoreAsync(messageBar, MessageBarResult.OfProgrammatic(data));
    }

    /// <inheritdoc cref="INotificationService.CloseAsync(string, object?)"/>
    public async Task<bool> CloseAsync(string id, object? data = null)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return false;
        }

        if (ServiceProvider.Items.TryGetValue(id, out var notification))
        {
            if (notification is IMessageBarInstance messageBar)
            {
                await CloseCoreAsync((IMessageBarInstance)messageBar, MessageBarResult.OfProgrammatic(data));
                return true;
            }

            if (notification is IToastInstance toast)
            {
                await CloseCoreAsync((IToastInstance)toast, ToastResult.OfProgrammatic(toast, data));
                return true;
            }
        }

        return false;
    }

    /// <inheritdoc cref="INotificationService.CloseAllMessageBarsAsync()"/>
    public async Task<int> CloseAllMessageBarsAsync()
    {
        var messageBars = ServiceProvider.Items.Values.Where(item => item is IMessageBarInstance).Cast<IMessageBarInstance>().ToList();

        foreach (var messageBar in messageBars)
        {
            await CloseCoreAsync(messageBar, MessageBarResult.OfProgrammatic());
        }

        return messageBars.Count;
    }

    /// <summary />
    private MessageBarInstance ShowMessageInstanceCore([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type? componentType, MessageBarOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (string.IsNullOrWhiteSpace(options.Section))
        {
            throw new ArgumentException(
                $"{nameof(MessageBarOptions)}.{nameof(MessageBarOptions.Section)} must be set to the Section of a {nameof(FluentMessageBarProvider)} that will render the message bar.",
                nameof(options));
        }

        if (this.ProviderNotAvailable())
        {
            throw new FluentServiceProviderException<FluentMessageBarProvider>();
        }

        var instance = new MessageBarInstance(this, componentType, options);

        // Add the MessageBar to the service.
        if (!ServiceProvider.Items.TryAdd(instance.Id, instance))
        {
            throw new InvalidOperationException($"A MessageBar with the ID '{instance.Id}' is already registered.");
        }

        options.OnStatusChange?.Invoke(new MessageBarEventArgs(instance, MessageBarLifecycleStatus.Visible));

        // Complete the result now if the caller requested completion when the message bar becomes visible.
        instance.TryCompleteResultOnVisible();

        // Schedule the auto-dismiss when a lifetime is configured.
        if (options.Lifetime is TimeSpan lifetime && lifetime > TimeSpan.Zero)
        {
            ScheduleAutoDismiss(instance, lifetime);
        }

        return instance;
    }

    /// <summary />
    private async Task CloseCoreAsync(IMessageBarInstance messageBar, MessageBarResult result)
    {
        if (messageBar is not MessageBarInstance instance)
        {
            return;
        }

        if (instance.LifecycleStatus == MessageBarLifecycleStatus.Unmounted)
        {
            return;
        }

        // Cancel the auto-dismiss timer (if it is still pending).
        instance.CancelLifetime();

        instance.LifecycleStatus = MessageBarLifecycleStatus.Dismissed;
        instance.Options.OnStatusChange?.Invoke(new MessageBarEventArgs(instance, MessageBarLifecycleStatus.Dismissed));

        // Remove the MessageBar from the MessageBarProvider.
        await RemoveMessageBarFromProviderAsync(instance);

        instance.LifecycleStatus = MessageBarLifecycleStatus.Unmounted;

        // Set the result of the MessageBar.
        instance.ResultCompletion.TrySetResult(result);

        // Raise the final MessageBarLifecycleStatus.Unmounted event.
        instance.Options.OnStatusChange?.Invoke(new MessageBarEventArgs(instance, MessageBarLifecycleStatus.Unmounted));
    }

    /// <summary>
    /// Schedules a delayed automatic dismissal of the message bar after the configured lifetime.
    /// </summary>
    private void ScheduleAutoDismiss(MessageBarInstance instance, TimeSpan lifetime)
    {
        var token = instance.LifetimeCancellationToken;

        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(lifetime, token);
            }
            catch (TaskCanceledException)
            {
                // The MessageBar was closed before the lifetime elapsed.
                return;
            }

            if (token.IsCancellationRequested)
            {
                return;
            }

            try
            {
                await CloseCoreAsync(instance, MessageBarResult.OfTimedOut());
            }
            catch
            {
                // The provider may have been disposed. Swallow exceptions in the background task.
            }
        });
    }

    /// <summary>
    /// Removes the MessageBar from the MessageBarProvider.
    /// </summary>
    private async Task RemoveMessageBarFromProviderAsync(IMessageBarInstance? messageBar)
    {
        if (messageBar is null)
        {
            return;
        }

        if (!ServiceProvider.Items.TryRemove(messageBar.Id, out _))
        {
            return;
        }

        await ServiceProvider.OnUpdatedAsync.Invoke(messageBar);
    }
}
