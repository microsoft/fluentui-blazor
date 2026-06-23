// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Collections.Concurrent;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Subscriber-management portion of the <see cref="NotificationService"/>.
/// Allows multiple <see cref="FluentMessageBarProvider"/> instances to receive
/// update notifications from the same service (e.g. when scoped by Section).
/// </summary>
public partial class NotificationService
{
    private readonly ConcurrentDictionary<string, Func<IMessageBarInstance, Task>> _subscribers = new(StringComparer.Ordinal);

    /// <summary>
    /// Registers a provider callback so it gets notified when the items collection changes.
    /// </summary>
    internal void Subscribe(string? providerId, Func<IMessageBarInstance, Task> callback)
    {
        if (string.IsNullOrEmpty(providerId) || callback is null)
        {
            return;
        }

        _subscribers[providerId] = callback;

        // Keep ProviderId non-empty so ProviderNotAvailable() reports the service as available.
        ServiceProvider.ProviderId = providerId;
    }

    /// <summary>
    /// Unregisters a previously registered provider callback.
    /// </summary>
    internal void Unsubscribe(string? providerId)
    {
        if (string.IsNullOrEmpty(providerId))
        {
            return;
        }

        _subscribers.TryRemove(providerId, out _);

        // If no provider is left, mark the service as unavailable.
        if (_subscribers.IsEmpty)
        {
            ServiceProvider.ProviderId = null;
        }
        else if (string.Equals(ServiceProvider.ProviderId, providerId, StringComparison.Ordinal))
        {
            // The provider whose id we were exposing has gone away; switch to any remaining one.
            ServiceProvider.ProviderId = _subscribers.Keys.FirstOrDefault();
        }
    }

    /// <summary>
    /// Invokes every registered subscriber. Each provider decides whether the
    /// instance is relevant to it (typically via the <c>Section</c> filter).
    /// </summary>
    private async Task DispatchOnUpdatedAsync(IMessageBarInstance instance)
    {
        foreach (var callback in _subscribers.Values)
        {
            try
            {
                await callback.Invoke(instance);
            }
            catch
            {
                // A disposed provider may throw; ignore and keep notifying the others.
            }
        }
    }
}
