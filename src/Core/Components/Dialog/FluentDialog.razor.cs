using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentDialog : FluentComponentBase //, IDisposable
{
    private const string DEFAULT_DIALOG_WIDTH = "500px";
    private const string DEFAULT_PANEL_WIDTH = "340px";
    private const string DEFAULT_HEIGHT = "unset";
    private DialogParameters _parameters = default!;
    private bool _hidden;
    private FluentDialogHeader? _dialogHeader;
    private FluentDialogFooter? _dialogFooter;

    [CascadingParameter]
    private InternalDialogContext? DialogContext { get; set; } = default!;

    /// <summary>
    /// Indicates the element is modal. When modal, user mouse interaction will be limited to the contents of the element by a modal
    /// overlay.  Clicks on the overlay will cause the dialog to emit a "dismiss" event.
    /// </summary>
    [Parameter]
    public bool? Modal { get; set; }

    /// <summary>
    /// Gets or sets if the dialog is hidden
    /// </summary>
    [Parameter]
    public bool Hidden
    {
        get => _hidden;
        set
        {
            if (value == _hidden)
                return;
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
    /// Indicates that the dialog should trap focus.
    /// </summary>
    [Parameter]
    public bool? TrapFocus { get; set; }

    /// <summary>
    /// The id of the element describing the dialog.
    /// </summary>
    [Parameter]
    public string? AriaDescribedby { get; set; }

    /// <summary>
    /// The id of the element labeling the dialog.
    /// </summary>
    [Parameter]
    public string? AriaLabelledby { get; set; }

    /// <summary>
    /// The label surfaced to assistive technologies.
    /// </summary>
    [Parameter]
    public string? AriaLabel { get; set; }

    /// <summary>
    /// The instance containing the programmatic API for the dialog.
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

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-dialog-main")
        .AddClass("right", () => _parameters.DialogType == DialogType.Panel && _parameters.Alignment == HorizontalAlignment.Right)
        .AddClass("left", () => _parameters.DialogType == DialogType.Panel && _parameters.Alignment == HorizontalAlignment.Left)
        .Build();

    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("position", "absolute")
        .AddStyle("top", "50%", () => _parameters.Alignment == HorizontalAlignment.Center)
        .AddStyle("left", "50%", () => _parameters.Alignment == HorizontalAlignment.Center)
        .AddStyle("--dialog-width", _parameters.Width ?? DEFAULT_DIALOG_WIDTH, () => _parameters.Alignment == HorizontalAlignment.Center)
        .AddStyle("--dialog-width", _parameters.Width ?? DEFAULT_PANEL_WIDTH, () => _parameters.DialogType == DialogType.Panel)
        .AddStyle("--dialog-height", _parameters.Height ?? DEFAULT_HEIGHT, () => _parameters.Alignment == HorizontalAlignment.Center)
        .Build();

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

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Element.FocusAsync();
        }
    }

    private bool HasButtons => _parameters.ShowPrimaryAction || _parameters.ShowSecondaryAction;

    public void Show()
    {
        Hidden = false;
        StateHasChanged();
    }

    public void Hide()
    {
        Hidden = true;
        StateHasChanged();
    }

    public void TogglePrimaryActionButton(bool isEnabled)
    {
        _parameters.PrimaryActionEnabled = isEnabled;
        StateHasChanged();
    }

    public void ToggleSecondaryActionButton(bool isEnabled)
    {
        _parameters.SecondaryActionEnabled = isEnabled;
        StateHasChanged();
    }

    public async Task CancelAsync() => await CloseAsync(DialogResult.Cancel());

    public async Task CancelAsync<T>(T returnValue) => await CloseAsync(DialogResult.Cancel(returnValue));

    public async Task CloseAsync() => await CloseAsync(DialogResult.Ok<object?>(null));

    public async Task CloseAsync<T>(T returnValue) => await CloseAsync(DialogResult.Ok(returnValue));

    /// <summary>
    /// Closes the dialog
    /// </summary>
    public async Task CloseAsync(DialogResult dialogResult)
    {
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

    internal void SetDialogHeader(FluentDialogHeader header)
    {
        if (_dialogHeader != null && !HasDefaultDialogHeader)
        {
            throw new InvalidOperationException($"This {nameof(FluentDialog)} already contains a {nameof(FluentDialogHeader)}");
        }

        _dialogHeader = header;
        StateHasChanged();
    }

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
    private bool HasDefaultDialogHeader => _dialogHeader == null ||
                                           _dialogHeader?.Data?.ToString() == FluentDialogHeader.DefaultDialogHeaderIdentifier;

    /// <summary />
    private bool HasDefaultDialogFooter => _dialogFooter == null ||
                                           _dialogFooter?.Data?.ToString() == FluentDialogFooter.DefaultDialogFooterIdentifier;

    /// <summary />
    private bool IsCustomized => !HasDefaultDialogFooter && !HasDefaultDialogHeader;
}