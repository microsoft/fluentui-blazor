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

    /// <summary />
    internal InputFileInstance(IFluentServiceBase<IDialogInstance> serviceProvider, IDialogInstance instance, string anchorId)
    {
        _serviceProvider = serviceProvider;
        AnchorId = anchorId;

        Id = instance.Id;
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
        if (_serviceProvider.Items.TryRemove(Id, out var dialogInstance))
        {
            await _serviceProvider.OnUpdatedAsync.Invoke(dialogInstance);
        }
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
