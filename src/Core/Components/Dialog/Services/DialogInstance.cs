// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a dialog instance used with the <see cref="IDialogService"/>.
/// </summary>
public class DialogInstance : IDialogInstance
{
    private static long _counter;
    private readonly Type _componentType;
    internal readonly TaskCompletionSource<DialogResult> ResultCompletion = new();

    /// <summary />
    internal DialogInstance(IDialogService dialogService, Type componentType, DialogOptions options)
    {
        _componentType = componentType;
        Options = options;
        DialogService = dialogService;
        Id = string.IsNullOrEmpty(options.Id) ? Identifier.NewId() : options.Id;
        Index = Interlocked.Increment(ref _counter);
    }

    /// <summary />
    Type IDialogInstance.ComponentType => _componentType;

    /// <summary />
    internal IDialogService DialogService { get; }

    /// <summary />
    internal FluentDialog? FluentDialog { get; set; }

    /// <inheritdoc cref="IDialogInstance.Options"/>
    public DialogOptions Options { get; internal set; }

    /// <inheritdoc cref="IDialogInstance.Result"/>
    public Task<DialogResult> Result => ResultCompletion.Task;

    /// <inheritdoc cref="IDialogInstance.Id"/>"
    public string Id { get; }

    /// <inheritdoc cref="IDialogInstance.Index"/>"
    public long Index { get; }

    /// <inheritdoc cref="IDialogInstance.CancelAsync()"/>
    public Task CancelAsync()
    {
        return DialogService.CloseAsync(this, DialogResult.Cancel());
    }

    /// <inheritdoc cref="IDialogInstance.CloseAsync()"/>
    public Task CloseAsync()
    {
        return DialogService.CloseAsync(this, DialogResult.Ok());
    }

    /// <inheritdoc cref="IDialogInstance.CloseAsync{T}(T)"/>
    public Task CloseAsync<T>(T result)
    {
        return DialogService.CloseAsync(this, DialogResult.Ok(result));
    }

    /// <inheritdoc cref="IDialogInstance.CloseAsync(DialogResult)"/>
    public Task CloseAsync(DialogResult result)
    {
        return DialogService.CloseAsync(this, result);
    }
}
