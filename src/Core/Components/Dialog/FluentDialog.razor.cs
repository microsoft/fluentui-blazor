// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The dialog component is a window overlaid on either the primary window or another dialog window.
/// Windows under a modal dialog are inert. 
/// </summary>
public partial class FluentDialog : FluentComponentBase
{
    /// <summary />
    public FluentDialog()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// internal constructor used by the <see cref="DialogService" />
    /// and the <see cref="FluentDialogProvider"/>.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="dialogService"></param>
    /// <param name="instance"></param>
    internal FluentDialog(IServiceProvider serviceProvider, IDialogService? dialogService, DialogInstance? instance)
        : this()
    {
        DialogService = dialogService;
        Instance = instance;
        JSRuntime = serviceProvider.GetRequiredService<IJSRuntime>();
    }

    /// <summary />
    [Inject]
    private IDialogService? DialogService { get; set; }

    /// <summary>
    /// Gets or sets the instance used by the <see cref="DialogService" />.
    /// </summary>
    [Parameter]
    public DialogInstance? Instance { get; set; }

    /// <summary>
    /// Used when not calling the <see cref="DialogService" /> to show a dialog.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Command executed when the user clicks on the button.
    /// </summary>
    [Parameter]
    public EventCallback<DialogEventArgs> OnStateChange { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="firstRender"></param>
    /// <returns></returns>
    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && LaunchedFromService)
        {
            return ShowAsync();
        }

        return Task.CompletedTask;
    }

    /// <summary />
    private Task OnToggleAsync(DialogToggleEventArgs args)
    {
        var dialogEventArgs = new DialogEventArgs(this, args);
        var dialogId = dialogEventArgs?.Id ?? string.Empty;

        if (OnStateChange.HasDelegate)
        {
            return OnStateChange.InvokeAsync(dialogEventArgs);
        }

        // Remove the HTML code from the Provider
        if (LaunchedFromService && dialogEventArgs?.State == DialogState.Closed && !string.IsNullOrEmpty(dialogId))
        {
            (DialogService as DialogService)?.InternalService.Items.TryRemove(dialogId, out _);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Displays the dialog.
    /// </summary>
    public async Task ShowAsync()
    {
        await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Dialog.Show", Id);
    }

    /// <summary>
    /// Hide the dialog.
    /// </summary>
    public async Task HideAsync()
    {
        await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Dialog.Hide", Id);
    }

    /// <summary />
    private bool LaunchedFromService => Instance is not null;
}
