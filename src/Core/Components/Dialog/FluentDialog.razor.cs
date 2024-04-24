using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentDialog : FluentComponentBase
{
    private const string DEFAULT_DIALOG_WIDTH = "500px";
    private const string DEFAULT_PANEL_WIDTH = "340px";
    private const string DEFAULT_HEIGHT = "unset";
    private DialogParameters _parameters = default!;
    private bool _hidden;
    private FluentDialogHeader? _dialogHeader;
    private FluentDialogFooter? _dialogFooter;

    /// <summary />
    [CascadingParameter]
    private InternalDialogContext? DialogContext { get; set; } = default!;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-dialog-main")
        .AddClass("right", () => _parameters.DialogType == DialogType.Panel && _parameters.Alignment == HorizontalAlignment.Right)
        .AddClass("left", () => _parameters.DialogType == DialogType.Panel && _parameters.Alignment == HorizontalAlignment.Left)
        .AddClass("prevent-scroll", () => Instance is null ? (PreventScroll && !Hidden) : _parameters.PreventScroll)
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("position", "absolute")
        .AddStyle("z-index", $"{ZIndex.Dialog}")
        .AddStyle("top", "50%", () => _parameters.Alignment == HorizontalAlignment.Center)
        .AddStyle("left", "50%", () => _parameters.Alignment == HorizontalAlignment.Center)
        .AddStyle("--dialog-width", _parameters.Width ?? DEFAULT_DIALOG_WIDTH, () => _parameters.Alignment == HorizontalAlignment.Center)
        .AddStyle("--dialog-width", _parameters.Width ?? DEFAULT_PANEL_WIDTH, () => _parameters.DialogType == DialogType.Panel)
        .AddStyle("--dialog-height", _parameters.Height ?? DEFAULT_HEIGHT, () => _parameters.Alignment == HorizontalAlignment.Center)
        .Build();

    /// <summary>
    /// Prevents scrolling outside of the dialog while it is shown.
    /// </summary>
    [Parameter]
    public bool PreventScroll { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the element is modal. When modal, user mouse interaction will be limited to the contents of the element by a modal
    /// overlay. Clicks on the overlay will cause the dialog to emit a "dismiss" event.
    /// </summary>
    [Parameter]
    public bool? Modal { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the dialog is hidden.
    /// </summary>
    [Parameter]
    public bool Hidden
    {
        get => _hidden;
        set
        {
            if (value == _hidden)
            {
                return;
            }

            _hidden = value;
            HiddenChanged.InvokeAsync(value);
        }
    }

    /// <summary>
    /// The event callback invoked when <see cref="Hidden"/> change.
    /// </summary>
    [Parameter]
    public EventCallback<bool> HiddenChanged { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether that the dialog should trap focus.
    /// </summary>
    [Parameter]
    public bool? TrapFocus { get; set; }

    /// <summary>
    /// Gets or sets the id of the element describing the dialog.
    /// </summary>
    [Parameter]
    public string? AriaDescribedby { get; set; }

    /// <summary>
    /// Gets or sets the id of the element labeling the dialog.
    /// </summary>
    [Parameter]
    public string? AriaLabelledby { get; set; }

    /// <summary>
    /// Gets or sets the label surfaced to assistive technologies.
    /// </summary>
    [Parameter]
    public string? AriaLabel { get; set; }

    /// <summary>
    /// Gets or sets the instance containing the programmatic API for the dialog.
    /// </summary>
    [Parameter]
    public DialogInstance Instance { get; set; } = default!;

    /// <summary>
    /// Used when not calling the <see cref="DialogService" /> to show a dialog.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// The event callback invoked to return the dialog result.
    /// </summary>
    [Parameter]
    public EventCallback<DialogResult> OnDialogResult { get; set; }

    /// <summary>
    /// Gets True if the Dialog was called from the DialogService.
    /// </summary>
    private bool CallingFromDialogService => ChildContent is null;

    /// <summary />
    protected override void OnInitialized()
    {
        if (Instance is null)
        {
            _parameters = new()
            {
                Alignment = HorizontalAlignment.Center,
                ShowTitle = false,
                PrimaryAction = string.Empty,
                SecondaryAction = string.Empty
            };
            Modal = true;
            TrapFocus = true;
        }
        else
        {
            _parameters = Instance.Parameters;
        }
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Element.FocusAsync();

            if (Instance is not null)
            {
                if (Instance.Parameters.OnDialogOpened.HasDelegate)
                {
                    await Instance.Parameters.OnDialogOpened.InvokeAsync(Instance);
                }
            }
        }
    }

    /// <summary>
    /// Shows the dialog
    /// </summary>
    public void Show()
    {
        Hidden = false;
        RefreshHeaderFooter();
    }

    /// <summary>
    /// Hides the dialog
    /// </summary>
    public void Hide()
    {
        Hidden = true;
        RefreshHeaderFooter();
    }

    /// <summary>
    /// Toggle the primary action button
    /// </summary>
    /// <param name="isEnabled"></param>
    public void TogglePrimaryActionButton(bool isEnabled)
    {
        _parameters.PrimaryActionEnabled = isEnabled;
        RefreshHeaderFooter();
    }

    /// <summary>
    /// Toggle the secondary action button
    /// </summary>
    /// <param name="isEnabled"></param>
    public void ToggleSecondaryActionButton(bool isEnabled)
    {
        _parameters.SecondaryActionEnabled = isEnabled;
        RefreshHeaderFooter();
    }

    /// <summary>
    /// Closes the dialog with a cancel result.
    /// </summary>
    /// <returns></returns>
    public async Task CancelAsync() => await CloseAsync(DialogResult.Cancel());

    /// <summary>
    /// Closes the dialog with a cancel result.
    /// </summary>
    /// <param name="returnValue"></param>
    /// <returns></returns>
    public async Task CancelAsync<T>(T returnValue) => await CloseAsync(DialogResult.Cancel(returnValue));

    /// <summary>
    /// Closes the dialog with a OK result.
    /// </summary>
    /// <returns></returns>
    public async Task CloseAsync() => await CloseAsync(DialogResult.Ok<object?>(null));

    /// <summary>
    /// Closes the dialog with a OK result.
    /// </summary>
    /// <param name="returnValue"></param>
    /// <returns></returns>
    public async Task CloseAsync<T>(T returnValue) => await CloseAsync(DialogResult.Ok(returnValue));

    /// <summary>
    /// Closes the dialog
    /// </summary>
    public async Task CloseAsync(DialogResult dialogResult)
    {
        if (Instance is not null)
        {
            if (Instance.Parameters.OnDialogClosing.HasDelegate)
            {
                await Instance.Parameters.OnDialogClosing.InvokeAsync(Instance);
            }
        }
        DialogContext?.DialogContainer.DismissInstance(Id!, dialogResult);
        if (Instance is not null)
        {
            if (Instance.Parameters.OnDialogResult.HasDelegate)
            {
                await Instance.Parameters.OnDialogResult.InvokeAsync(dialogResult);
            }
        }
        else
        {
            Hide();
        }
    }

    /// <summary />
    internal void SetDialogHeader(FluentDialogHeader header)
    {
        if (_dialogHeader != null && !HasDefaultDialogHeader)
        {
            throw new InvalidOperationException($"This {nameof(FluentDialog)} already contains a {nameof(FluentDialogHeader)}");
        }

        _dialogHeader = header;
        StateHasChanged();
    }

    /// <summary />
    internal void SetDialogFooter(FluentDialogFooter footer)
    {
        if (_dialogFooter != null && !HasDefaultDialogFooter)
        {
            throw new InvalidOperationException($"This {nameof(FluentDialog)} already contains a {nameof(FluentDialogFooter)}");
        }

        _dialogFooter = footer;
        StateHasChanged();
    }

    /// <summary />
    private void RefreshHeaderFooter()
    {
        StateHasChanged();

        _dialogHeader?.Refresh();

        _dialogFooter?.Refresh();
    }

    /// <summary />
    private bool HasDefaultDialogHeader => (_dialogHeader == null && CallingFromDialogService) ||
                                           _dialogHeader?.Data?.ToString() == FluentDialogHeader.DefaultDialogHeaderIdentifier;

    /// <summary />
    private bool HasDefaultDialogFooter => (_dialogFooter == null && CallingFromDialogService) ||
                                           _dialogFooter?.Data?.ToString() == FluentDialogFooter.DefaultDialogFooterIdentifier;

    /// <summary />
    private bool IsCustomized => !HasDefaultDialogFooter && !HasDefaultDialogHeader;
}
