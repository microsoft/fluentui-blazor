// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentKeyCodeProvider : IDisposable
{
    [Inject]
    private IKeyCodeService KeyCodeService { get; set; } = default!;

    /// <summary>
    /// Gets or sets a way to tells the user agent that if the event does not get explicitly handled, its default action should not be taken as it normally would be.
    /// </summary>
    [Parameter]
    public bool PreventDefault { get; set; } = false;

    private void KeyDownHandler(FluentKeyCodeEventArgs args)
    {
        foreach (var listener in KeyCodeService.Listeners)
        {
            listener.OnKeyDownAsync(args);
        }
    }

    private void KeyUpHandler(FluentKeyCodeEventArgs args)
    {
        foreach (var listener in KeyCodeService.Listeners)
        {
            listener.OnKeyUpAsync(args);
        }
    }

    public void Dispose()
    {
        KeyCodeService.Clear();
    }
}
