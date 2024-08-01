// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Client;

public static class ComponentBaseExtensions
{
    public static async Task TimerWaitAsync(this ComponentBase component, int milliseconds, Action action)
    {
        var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(milliseconds));
        while (await timer.WaitForNextTickAsync())
        {
            timer.Dispose();
            action.Invoke();
        }
    }
}
