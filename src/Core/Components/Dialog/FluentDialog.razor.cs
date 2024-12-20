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
    /// <param name="instance"></param>
    internal FluentDialog(IServiceProvider serviceProvider, DialogInstance instance)
    {
        Id = instance.Id;
        DialogService = instance.DialogService;
        Instance = instance;
        JSRuntime = serviceProvider.GetRequiredService<IJSRuntime>();
    }

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("height", Instance?.Options.Height, when: IsDialog())
        .AddStyle("width", Instance?.Options.Width)
        .Build();

    /// <summary />
    [Inject]
    private IDialogService? DialogService { get; set; }

    /// <summary>
    /// Gets or sets the instance used by the <see cref="DialogService" />.
    /// </summary>
    [Parameter]
    public IDialogInstance? Instance { get; set; }

    /// <summary>
    /// Used when not calling the <see cref="DialogService" /> to show a dialog.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the alignment of the dialog (center, left, right).
    /// </summary>
    [Parameter]
    public DialogAlignment Alignment { get; set; } = DialogAlignment.Default;

    ///// <summary>
    ///// Gets or sets a value indicating whether this dialog is displayed modally.
    ///// </summary>
    ///// <remarks>
    ///// When a dialog is displayed modally, no input (keyboard or mouse click) can occur except to objects on the modal dialog.
    ///// The program must hide or close a modal dialog (usually in response to some user action) before input to another dialog can occur.
    ///// </remarks>
    //[Parameter]
    //public bool Modal { get; set; }

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
    private async Task OnToggleAsync(DialogToggleEventArgs args)
    {
        var dialogEventArgs = new DialogEventArgs(this, args);
        var dialogId = dialogEventArgs?.Id ?? string.Empty;

        if (OnStateChange.HasDelegate)
        {
            await OnStateChange.InvokeAsync(dialogEventArgs);
        }

        // Remove the HTML code from the DialogProvider
        if (LaunchedFromService && Instance is not null && dialogEventArgs?.State == DialogState.Closed && !string.IsNullOrEmpty(dialogId))
        {
            var service = DialogService as DialogService;
            service?.CloseAsync(Instance, DialogResult.Cancel());
        }
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

    /// <summary />
    private string? GetAlignmentAttribute()
    {
        // Alignment is only used when the dialog is a panel.
        if (IsPanel())
        {
            // Get the alignment from the DialogService (if used) or the Alignment property.
            var alignment = Instance?.Options.Alignment ?? Alignment;

            return alignment switch
            {
                DialogAlignment.Start => "start",
                DialogAlignment.End => "end",
                _ => null
            };
        }

        return null;
    }

    /// <summary />
    private string? GetModalAttribute()
    {
        switch (IsPanel())
        {
            // Dialog
            case false:
                // TODO: To find a way to catch the click event outside the dialog.
                return "modal";

            // Panels
            case true:
                // TODO: To find a way to catch the click event outside the dialog.
                return "modal";

        }
    }

    /// <summary />
    private bool IsPanel() => (Instance?.Options.Alignment ?? Alignment) != DialogAlignment.Default;

    /// <summary />
    private bool IsDialog() => !IsPanel();

    /// <summary />
    private MarkupString? GetDialogStyle()
    {
        if (string.IsNullOrEmpty(StyleValue))
        {
            return null;
        }

        return (MarkupString)$"<style>#{Id}::part(dialog) {{ {StyleValue} }}</style>";
    }
}
