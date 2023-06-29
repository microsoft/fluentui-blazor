using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentToast : FluentComponentBase, IDisposable
{
    private CountdownTimer? _countdownTimer;

    [CascadingParameter]
    private InternalToastContext ToastContext { get; set; } = default!;

    [Parameter]
    public ToastIntent Intent { get; set; }

    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public ToastTopCTAType TopCTAType { get; set; }

    [Parameter]
    public ToastAction? TopAction { get; set; }

    [Parameter]
    public DateTime Timestamp { get; set; } = DateTime.Now;

    [Parameter]
    public int? Timeout { get; set; } = 7;

    [Parameter]
    public (string Name, Color Color, IconVariant Variant)? Icon { get; set; } = default!;


    /// <summary>
    /// The instance containing the programmatic API for the toast.
    /// </summary>
    [Parameter]
    public ToastInstance Instance { get; set; } = default!;

    [Parameter]
    public ToastSettings Settings { get; set; } = default!;

    /// <summary>
    /// The event callback invoked to return the dialog result.
    /// </summary>
    [Parameter]
    public EventCallback<ToastResult> OnToastResult { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _countdownTimer = new CountdownTimer(Settings.Timeout ?? ToastContext.ToastContainer.Timeout)
            .OnElapsed(Close);
        ToastContext?.Register(this);
        await _countdownTimer.StartAsync();
    }

    protected override void OnParametersSet()
    {
        if (Instance.ContentType == typeof(CommunicationToast) && Settings.TopCTAType == ToastTopCTAType.Action)
        {
            throw new InvalidOperationException("ToastTopCTAType.Action is not supported for a CommunicationToast  ");
        }
        if (Instance.ContentType != typeof(CommunicationToast) && Settings.TopCTAType == ToastTopCTAType.Timestamp)
        {
            throw new InvalidOperationException("ToastTopCTAType.Timestamp is not supported for a this type of toast");
        }
    }

    public FluentToast()
    {
    }

    /// <summary>
    /// Closes the toast
    /// </summary>
    public void Close()
        => ToastContext?.ToastContainer.RemoveToast(Id!);

    public void HandleTopActionClick()
    {
        TopAction?.OnClick?.Invoke();
        Close();
    }

    public void PauseTimeout()
    {
        Console.WriteLine("PauseTimeout");
        _countdownTimer?.Pause();
    }

    public void ResumeTimeout()
    {
        Console.WriteLine("ResumeTimeout");
        _countdownTimer?.Resume();
    }

    public void HandlePrimaryActionClick()
    {
        Settings.PrimaryAction?.OnClick?.Invoke();
        Close();
    }

    public void HandleSecondaryActionClick()
    {
        Settings.SecondaryAction?.OnClick?.Invoke();
        Close();
    }

    public void Dispose()
    {
        _countdownTimer?.Dispose();
        _countdownTimer = null;

        ToastContext?.Unregister(this);

        GC.SuppressFinalize(this);
    }
}