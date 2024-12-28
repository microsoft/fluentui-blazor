// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
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
            var instance = Instance as DialogInstance;
            if (instance is not null)
            {
                instance.FluentDialog = this;
            }

            return ShowAsync();
        }

        return Task.CompletedTask;
    }

    /// <summary />
    internal async Task OnToggleAsync(DialogToggleEventArgs args)
    {
        // Raise the event received from the Web Component
        var dialogEventArgs = await RaiseOnStateChangeAsync(args);

        if (LaunchedFromService)
        {
            switch (dialogEventArgs.State)
            {
                // Set the result of the dialog
                case DialogState.Closing:
                    (Instance as DialogInstance)?.ResultCompletion.TrySetResult(DialogResult.Cancel());
                    break;

                // Remove the dialog from the DialogProvider
                case DialogState.Closed:
                    (DialogService as DialogService)?.RemoveDialogFromProviderAsync(Instance);
                    break;
            }
        }
    }

    /// <summary />
    private async Task<DialogEventArgs> RaiseOnStateChangeAsync(DialogEventArgs args)
    {
        if (OnStateChange.HasDelegate)
        {
            await InvokeAsync(() => OnStateChange.InvokeAsync(args));
        }

        return args;
    }

    /// <summary />
    private Task<DialogEventArgs> RaiseOnStateChangeAsync(DialogToggleEventArgs args) => RaiseOnStateChangeAsync(new DialogEventArgs(this, args));

    /// <summary />
    internal Task<DialogEventArgs> RaiseOnStateChangeAsync(IDialogInstance instance, DialogState state) => RaiseOnStateChangeAsync(new DialogEventArgs(instance, state));

    /// <summary>
    /// Displays the dialog.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public async Task ShowAsync()
    {
        await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Dialog.Show", Id);
    }

    /// <summary>
    /// Hide the dialog.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public async Task HideAsync()
    {
        await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Dialog.Hide", Id);
    }

    /// <summary />
    private bool LaunchedFromService => Instance is not null;

    /// <summary />
    private async Task OnKeyDownHandlerAsync(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs e)
    {
        if (Instance is null)
        {
            return;
        }

        var shortCut = $"{(e.CtrlKey ? "Ctrl+" : string.Empty)}{(e.AltKey ? "Alt+" : string.Empty)}{(e.ShiftKey ? "Shift+" : string.Empty)}{e.Key}";

        // OK button
        var primaryPressed = await ShortCutPressedAsync(Instance.Options.Footer.PrimaryAction, shortCut, Instance.CloseAsync);
        if (primaryPressed)
        {
            return;
        }

        // Cancel button
        var secondaryPressed = await ShortCutPressedAsync(Instance.Options.Footer.SecondaryAction, shortCut, Instance.CancelAsync);
        if (secondaryPressed)
        {
            return;
        }

        // Call the OnClickAsync or defaultAction if the shortcut is the button.ShortCut.
        async Task<bool> ShortCutPressedAsync(DialogOptionsFooterAction button, string shortCut, Func<Task> defaultAction)
        {
            if (string.IsNullOrEmpty(button.ShortCut) || Instance is null || !button.ToDisplay)
            {
                return false;
            }

            var buttonShortcuts = button.ShortCut.Split(";");
            foreach (var buttonShortcut in buttonShortcuts)
            {

                if (string.Equals(buttonShortcut.Trim(), shortCut, StringComparison.OrdinalIgnoreCase))
                {
                    if (button.OnClickAsync is not null)
                    {
                        await button.OnClickAsync.Invoke(Instance);
                    }
                    else
                    {
                        await defaultAction.Invoke();
                    }

                    return true;
                }
            }

            return false;
        }
    }

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
            // FluentDialog
            case false:
                // TODO: To find a way to catch the click event outside the dialog.
                return "alert";

            // Panels
            case true:
                // TODO: To find a way to catch the click event outside the dialog.
                return "alert";

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
