// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentKeyCodeProvider : IDisposable
{
    [Inject]
    private IKeyCodeService KeyCodeService { get; set; } = default!;

    private void KeyDownHandler(FluentKeyCodeEventArgs args)
    {
        foreach (var listener in KeyCodeService.Listeners)
        {
            listener.OnKeyDownAsync(args);
        }
    }

    public void Dispose()
    {
        KeyCodeService.Clear();
    }
}
