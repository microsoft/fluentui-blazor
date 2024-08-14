// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer.Components.ConsoleLog;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Demo.DocViewer.Components;

/// <summary>
/// Provider for the console log.
/// </summary>
public partial class ConsoleLogProvider
{
    private const string JAVASCRIPT_FILE = "./_content/FluentUI.Demo.DocViewer/Components/ConsoleLogProvider.razor.js";
    private IJSObjectReference _jsModule = default!;

    /// <summary>
    /// Gets or sets the injected console log service.
    /// </summary>
    [Inject]
    internal ConsoleLogService ConsoleService { get; set; } = default!;

    /// <summary />
    [Inject]
    internal IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the class name for the component.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// Gets or sets the style for the component.
    /// </summary>
    [Parameter]
    public string? Style { get; set; }

    /// <summary>
    /// Clear the console
    /// </summary>
    [Parameter]
    public EventCallback OnClear{ get; set; }

    /// <summary>
    /// Close the console
    /// </summary>
    [Parameter]
    public EventCallback OnClose { get; set; }

    /// <summary />
    private async Task ClearConsoleAsync()
    {
        ConsoleService.Clear();

        if (OnClose.HasDelegate)
        {
            await OnClear.InvokeAsync();
        }
    }

    /// <summary />
    private async Task CloseConsoleAsync()
    {
        if (OnClose.HasDelegate)
        {
            await OnClose.InvokeAsync();
        }
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD110:Observe result of async calls", Justification = "Allowing Blazor async infrastructure to handle the state updates without forcing synchronous execution")]
    protected override void OnInitialized()
    {
        ConsoleService.OnTraceLogged = (message) => InvokeAsync(StateHasChanged);
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            await _jsModule.InvokeVoidAsync("scrollToLastConsoleItem");
        }
    }
}
