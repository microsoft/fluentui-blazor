// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The dialog component is a window overlaid on either the primary window or another dialog window.
/// Windows under a modal dialog are inert.
/// </summary>
public partial class FluentDialog : FluentComponentBase, IHandleEvent
{
    private string? _shownInstanceId;

    /// <summary />
    [DynamicDependency(nameof(OnToggleAsync))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(DialogToggleEventArgs))]
    public FluentDialog(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("height", Instance?.Options.Height, when: IsDialog())
        .AddStyle("width", Instance?.Options.Width, when: !string.IsNullOrEmpty(Instance?.Options.Width))
        .AddStyle("max-width", "calc(-48px + 100vw)", when: !string.IsNullOrEmpty(Instance?.Options.Width)) // By default the fluent-dialog.max-width is "600px".
        .AddStyle("width", "100%", when: IsDrawer() && string.IsNullOrEmpty(Instance?.Options.Width))
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
    /// Gets or sets the child content rendered directly inside the dialog.
    /// Use this when displaying the dialog declaratively in Razor without the <see cref="DialogService"/>.
    /// When using <see cref="DialogService"/> to show dialogs, content is provided through the dialog component class, not via this parameter.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the alignment of the dialog (center, left, right).
    /// </summary>
    [Parameter]
    public DialogAlignment Alignment { get; set; } = DialogAlignment.Default;

    /// <summary>
    /// Gets or sets a value indicating whether this dialog is displayed modally.
    /// By default, the dialog is displayed modally (Modal = true).
    /// </summary>
    /// <remarks>
    /// When a dialog is displayed modally, no input (keyboard or mouse click) can occur except to objects on the modal dialog.
    /// The program must hide or close a modal dialog (usually in response to some user action) before input to another dialog can occur.
    /// </remarks>
    [Parameter]
    public bool Modal { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether pressing the ESC key should be prevented from closing the dialog.
    /// By default, the ESC key closes the dialog (<langword>false</langword>).
    /// When using the <see cref="IDialogService"/>, set <see cref="DialogOptions.PreventDismissOnEscape"/> instead.
    /// </summary>
    [Parameter]
    public bool PreventDismissOnEscape { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the dialog state changes (e.g., opening or closing).
    /// </summary>
    [Parameter]
    public EventCallback<DialogEventArgs> OnStateChange { get; set; }

    /// <summary />
    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        var shouldShowDialog = string.CompareOrdinal(_shownInstanceId, Instance?.Id) != 0;
        if (shouldShowDialog && LaunchedFromService)
        {
            _shownInstanceId = Instance?.Id;
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
        // The 'beforetoggle'/'toggle' DOM events are shared by the native <dialog> element and the
        // Popover API. Any popover rendered inside the dialog/drawer content (e.g. fluent-menu-list,
        // select listbox, tooltip) also raises these events. Blazor's event delegation attributes
        // them to this dialog's @ondialogtoggle handler. We must ignore events that don't target
        // this dialog instance; otherwise the IHandleEvent implementation below would re-render the
        // whole dialog subtree and detach any open popover content.
        if (string.CompareOrdinal(args.Id, Instance?.Id) != 0)
        {
            return;
        }

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

    /// <summary>
    /// Handles UI events for this component.
    /// </summary>
    /// <remarks>
    /// The dialog's content is supplied by the consumer (declaratively or through the
    /// <see cref="DialogService"/>) and is re-rendered on its own. The dialog's own event handlers
    /// (<see cref="OnKeyDownHandlerAsync"/> and <see cref="OnToggleAsync"/>) only forward to dialog
    /// actions/state callbacks that already request their own renders, so they don't need the
    /// automatic <c>StateHasChanged</c> that the default <see cref="IHandleEvent"/> implementation
    /// performs after every callback.
    /// <para>
    /// Suppressing that automatic render is important because the 'beforetoggle'/'toggle' and
    /// 'keydown' DOM events also bubble from content rendered inside the dialog/drawer (for example a
    /// <c>fluent-menu-list</c> popover, a select listbox or a DataGrid header). Blazor's event
    /// delegation attributes those to this dialog's handlers, and an unnecessary re-render of the
    /// dialog subtree would recreate keyed child content (e.g. DataGrid header cells) and detach any
    /// open popover.
    /// </para>
    /// </remarks>
    [ExcludeFromCodeCoverage(Justification = "Tested in aspnetcore code")]
    Task IHandleEvent.HandleEventAsync(EventCallbackWorkItem callback, object? arg)
        => callback.InvokeAsync(arg);

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
        var preventEscape = Instance?.Options.PreventDismissOnEscape ?? PreventDismissOnEscape;
        if (preventEscape)
        {
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Dialog.SetPreventEscapeClose", Id, preventEscape);
        }
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

        var shouldHandleShortcut = await JSRuntime.InvokeAsync<bool>("Microsoft.FluentUI.Blazor.Components.Dialog.ShouldHandleShortcut", Id);
        if (!shouldHandleShortcut)
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
        // Get the alignment from the DialogService (if used) or the Alignment property.
        var alignment = Instance?.Options.Alignment ?? Alignment;

        return alignment switch
        {
            DialogAlignment.Start => FluentSlot.Start,
            DialogAlignment.End => FluentSlot.End,
            _ => null,
        };
    }

    /// <summary />
    private string? GetModalAttribute()
    {
        // In Web Components, the type="modal" has the opposite function to that generally used by Windows (WPP or WinForms).
        // See https://learn.microsoft.com/en-us/windows/apps/design/controls/dialogs-and-flyouts/dialogs
        // See https://www.telerik.com/blazor-ui/documentation/components/window/modal

        var isModal = Instance?.Options?.Modal ?? Modal;

        switch (IsDrawer())
        {
            // Dialog
            case false:
                return isModal ? "alert" : "modal";

            // Drawers / Panels
            case true:
                return isModal ? "modal" : "non-modal";

        }
    }

    /// <summary />
    private string? GetSizeAttribute()
    {
        return Instance?.Options?.Size.ToAttributeValue();
    }

    /// <summary />
    private bool IsDrawer() => IsDrawer(Instance, this);

    /// <summary />
    private bool IsDialog() => !IsDrawer();

    /// <summary />
    private MarkupStringSanitized? GetDialogStyle()
    {
        if (string.IsNullOrEmpty(StyleValue))
        {
            return null;
        }

        return new MarkupStringSanitized($"<style>#{Id}::part(dialog) {{ {StyleValue} }}</style>", LibraryConfiguration);
    }

    /// <summary>
    /// Returns true if the dialog is a drawer (panel).
    /// </summary>
    /// <param name="instance">The dialog instance.</param>
    /// <param name="dialog">The fluent dialog.</param>
    /// <returns>True if the dialog is a drawer, otherwise false.</returns>
    internal static bool IsDrawer(IDialogInstance? instance, FluentDialog? dialog = null)
    {
        var alignment = instance?.Options.Alignment ?? dialog?.Alignment;
        var isDrawer = instance?.Options.IsDrawer ?? dialog?.Instance?.Options.IsDrawer;

        if (isDrawer.HasValue)
        {
            return isDrawer.Value;
        }

        return alignment == DialogAlignment.Start || alignment == DialogAlignment.End;
    }
}
