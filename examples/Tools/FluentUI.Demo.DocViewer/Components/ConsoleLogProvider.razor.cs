// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer.Components.ConsoleLog;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.DocViewer.Components;

/// <summary>
/// Provider for the console log.
/// </summary>
public partial class ConsoleLogProvider
{
    /// <summary>
    /// Gets or sets the injected console log service.
    /// </summary>
    [Inject]
    public required ConsoleLogService ConsoleService { get; set; }

    /// <summary />
    private void ClearConsole()
    {
        ConsoleService.Clear();
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD110:Observe result of async calls", Justification = "Allowing Blazor async infrastructure to handle the state updates without forcing synchronous execution")]
    protected override void OnInitialized()
    {
        ConsoleService.OnTraceLogged = (message) => InvokeAsync(StateHasChanged);
    }
}
