// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Client;

public static class ComponentBaseExtensions
{
    public static async Task TimerWaitAsync(
        this ComponentBase component,
        int milliseconds,
        Action action,
        CancellationToken cancellationToken = default)
    {
        var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(milliseconds));
        while (await timer.WaitForNextTickAsync(cancellationToken))
        {
            timer.Dispose();
            action.Invoke();
        }
    }
}
