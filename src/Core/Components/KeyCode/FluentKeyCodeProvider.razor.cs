// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentKeyCodeProvider : IDisposable
{
    private bool _disposedValue;

    [Inject]
    private IKeyCodeService KeyCodeService { get; set; } = default!;

    /// <summary>
    /// Gets or sets a way to tells the user agent that if the event does not get explicitly handled, its default action should not be taken as it normally would be.
    /// </summary>
    [Parameter]
    public bool PreventDefault { get; set; } = false;

    /// <summary />
    internal async Task KeyDownHandlerAsync(FluentKeyCodeEventArgs args)
    {
        foreach (var listener in KeyCodeService.Listeners)
        {
            await listener.OnKeyDownAsync(args);
        }
    }

    /// <summary />
    internal async Task KeyUpHandlerAsync(FluentKeyCodeEventArgs args)
    {
        foreach (var listener in KeyCodeService.Listeners)
        {
            await listener.OnKeyUpAsync(args);
        }
    }

    /// <summary>
    /// Releases the resources used by the current instance of the class.
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                KeyCodeService.Clear();
            }

            _disposedValue = true;
        }
    }

    /// <summary>
    /// Releases the resources used by the current instance of the class.
    /// </summary>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
