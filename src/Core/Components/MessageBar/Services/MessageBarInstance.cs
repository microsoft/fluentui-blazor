// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a message bar instance used with the <see cref="IMessageBarService"/>.
/// </summary>
public class MessageBarInstance : IMessageBarInstance, IDisposable
{
    private static long _counter;
    internal readonly TaskCompletionSource<MessageBarResult> ResultCompletion = new();
    private readonly CancellationTokenSource _lifetimeCts = new();
    private bool _disposed;

    /// <summary />
    internal MessageBarInstance(IMessageBarService messageBarService, MessageBarOptions options)
    {
        Options = options;
        MessageBarService = messageBarService;
        Id = string.IsNullOrEmpty(options.Id) ? Identifier.NewId() : options.Id;
        Index = Interlocked.Increment(ref _counter);
    }

    /// <summary />
    internal IMessageBarService MessageBarService { get; }

    /// <summary>
    /// Gets the cancellation token used to cancel the auto-dismiss timer when the message bar
    /// is closed before its lifetime elapses.
    /// </summary>
    internal CancellationToken LifetimeCancellationToken => _lifetimeCts.Token;

    /// <inheritdoc cref="IMessageBarInstance.Options"/>
    public MessageBarOptions Options { get; internal set; }

    /// <inheritdoc cref="IMessageBarInstance.Result"/>
    public Task<MessageBarResult> Result => ResultCompletion.Task;

    /// <inheritdoc cref="IMessageBarInstance.LifecycleStatus"/>
    public MessageBarLifecycleStatus LifecycleStatus { get; internal set; } = MessageBarLifecycleStatus.Visible;

    /// <inheritdoc cref="IMessageBarInstance.Id"/>
    public string Id { get; }

    /// <inheritdoc cref="IMessageBarInstance.Index"/>
    public long Index { get; }

    /// <inheritdoc cref="IMessageBarInstance.CloseAsync()"/>
    public Task CloseAsync()
        => MessageBarService.CloseAsync(this, MessageBarResult.OfProgrammatic());

    /// <inheritdoc cref="IMessageBarInstance.CloseAsync(MessageBarResult)"/>
    public Task CloseAsync(MessageBarResult result)
        => MessageBarService.CloseAsync(this, result);

    /// <inheritdoc cref="IMessageBarInstance.DismissAsync()"/>
    public Task DismissAsync()
        => MessageBarService.DismissAsync(this);

    /// <inheritdoc cref="IMessageBarInstance.UpdateAsync(Action{MessageBarOptions})"/>
    public Task UpdateAsync(Action<MessageBarOptions> update)
        => MessageBarService.UpdateMessageBarAsync(this, update);

    /// <summary>
    /// Cancels the auto-dismiss timer (if any).
    /// </summary>
    internal void CancelLifetime()
    {
        if (_disposed)
        {
            return;
        }

        try
        {
            if (!_lifetimeCts.IsCancellationRequested)
            {
                _lifetimeCts.Cancel();
            }
        }
        catch (ObjectDisposedException)
        {
            // Already disposed; nothing to do.
        }
    }

    /// <inheritdoc cref="IDisposable.Dispose()"/>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        CancelLifetime();
        _lifetimeCts.Dispose();
        GC.SuppressFinalize(this);
    }
}
