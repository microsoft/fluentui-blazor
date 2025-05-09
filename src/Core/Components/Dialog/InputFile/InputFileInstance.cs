// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Instance of an input file.
/// </summary>
public class InputFileInstance : IAsyncDisposable
{
    private readonly IFluentServiceBase<IDialogInstance> _serviceProvider;
    private readonly IDialogInstance _dialogInstance;

    /// <summary />
    internal InputFileInstance(IFluentServiceBase<IDialogInstance> serviceProvider, IDialogInstance instance, string anchorId)
    {
        _serviceProvider = serviceProvider;
        _dialogInstance = instance;
        AnchorId = anchorId;

        Id = _dialogInstance.Id;
    }

    /// <summary>
    /// Gets the identifier of the input file instance.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the identifier of the anchor element.
    /// </summary>
    public string AnchorId { get; }

    /// <summary>
    /// Removes the <see cref="FluentInputFile"/> from the <see cref="FluentDialogProvider"/>.
    /// </summary>
    public async ValueTask UnregisterAsync()
    {
        _serviceProvider.Items.TryRemove(Id, out _);
        await _serviceProvider.OnUpdatedAsync.Invoke(_dialogInstance);
    }

    /// <summary>
    /// Disposes the <see cref="FluentInputFile"/> from the <see cref="FluentDialogProvider"/>.
    /// </summary>
    /// <returns></returns>
    public async ValueTask DisposeAsync()
    {
        await UnregisterAsync();
    }
}
