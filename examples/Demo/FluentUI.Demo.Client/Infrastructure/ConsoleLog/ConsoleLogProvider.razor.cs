// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Client.Infrastructure.ConsoleLog;

public partial class ConsoleLogProvider
{
    [Inject]
    public required ConsoleLogService ConsoleService { get; set; }

    private void ClearConsole()
    {
        ConsoleService.Clear();
    }

    protected override void OnInitialized()
    {
        ConsoleService.OnTraceLogged = (message) => StateHasChanged();
    }
}
