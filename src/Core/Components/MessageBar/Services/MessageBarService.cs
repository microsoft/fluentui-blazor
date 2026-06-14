// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Service for showing message bars.
/// </summary>
public partial class MessageBarService : FluentServiceBase<IMessageBarInstance>, IMessageBarService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageBarService"/> class.
    /// </summary>
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(MessageBarEventArgs))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(MessageBarInstance))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(IMessageBarInstance))]
    public MessageBarService()
    {
    }

    /// <inheritdoc cref="IMessageBarService.CloseAsync(IMessageBarInstance, MessageBarResult)"/>
    public async Task CloseAsync(IMessageBarInstance messageBar, MessageBarResult result)
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

    /// <inheritdoc cref="IMessageBarService.DismissAsync(IMessageBarInstance)"/>
    public Task DismissAsync(IMessageBarInstance messageBar)
        => CloseAsync(messageBar, MessageBarResult.OfDismissed());

    /// <inheritdoc cref="IMessageBarService.DismissAsync(string)"/>
    public async Task<bool> DismissAsync(string messageBarId)
    {
        if (string.IsNullOrWhiteSpace(messageBarId) || !ServiceProvider.Items.TryGetValue(messageBarId, out var messageBar))
        {
            return false;
        }

        await CloseAsync(messageBar, MessageBarResult.OfDismissed());
        return true;
    }

    /// <inheritdoc cref="IMessageBarService.DismissAllAsync()"/>
    public async Task<int> DismissAllAsync()
    {
        var messageBars = ServiceProvider.Items.Values.ToList();

        foreach (var messageBar in messageBars)
        {
            await CloseAsync(messageBar, MessageBarResult.OfDismissed());
        }

        return messageBars.Count;
    }

    /// <inheritdoc cref="IMessageBarService.ShowMessageAsync(MessageBarOptions)"/>
    public async Task<MessageBarResult> ShowMessageAsync(MessageBarOptions? options = null)
    {
        var instance = ShowMessageInstanceCore(componentType: null, options ?? new MessageBarOptions());
        await ServiceProvider.OnUpdatedAsync.Invoke(instance);
        return await instance.Result;
    }

    /// <inheritdoc cref="IMessageBarService.ShowMessageAsync(Action{MessageBarOptions})"/>
    public Task<MessageBarResult> ShowMessageAsync(Action<MessageBarOptions> options)
        => ShowMessageAsync(new MessageBarOptions(options));

    /// <inheritdoc cref="IMessageBarService.ShowMessageAsync{TMessageBar}(MessageBarOptions)"/>
    public async Task<MessageBarResult> ShowMessageAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TMessageBar>(MessageBarOptions? options = null)
        where TMessageBar : ComponentBase
    {
        var instance = ShowMessageInstanceCore(typeof(TMessageBar), options ?? new MessageBarOptions());
        await ServiceProvider.OnUpdatedAsync.Invoke(instance);
        return await instance.Result;
    }

    /// <inheritdoc cref="IMessageBarService.ShowMessageAsync{TMessageBar}(Action{MessageBarOptions})"/>
    public Task<MessageBarResult> ShowMessageAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TMessageBar>(Action<MessageBarOptions> options)
        where TMessageBar : ComponentBase
        => ShowMessageAsync<TMessageBar>(new MessageBarOptions(options));

    /// <inheritdoc cref="IMessageBarService.UpdateMessageBarAsync(IMessageBarInstance, Action{MessageBarOptions})"/>
    public async Task UpdateMessageBarAsync(IMessageBarInstance messageBar, Action<MessageBarOptions> update)
    {
        if (messageBar is not MessageBarInstance instance)
        {
            throw new ArgumentException($"{nameof(messageBar)} must be a {nameof(MessageBarInstance)}.", nameof(messageBar));
        }

        update(instance.Options);
        await ServiceProvider.OnUpdatedAsync.Invoke(instance);
    }

    /// <summary />
    private MessageBarInstance ShowMessageInstanceCore(Type? componentType, MessageBarOptions options)
    {
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

        // Schedule the auto-dismiss when a lifetime is configured.
        if (options.Lifetime is TimeSpan lifetime && lifetime > TimeSpan.Zero)
        {
            ScheduleAutoDismiss(instance, lifetime);
        }

        return instance;
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
                await CloseAsync(instance, MessageBarResult.OfTimedOut());
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
    internal async Task RemoveMessageBarFromProviderAsync(IMessageBarInstance? messageBar)
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
