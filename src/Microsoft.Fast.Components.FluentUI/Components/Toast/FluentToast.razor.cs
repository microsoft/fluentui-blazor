using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentToast : FluentComponentBase, IDisposable
{
    private CountdownTimer? _countdownTimer;
    private ToastSettings _settings = default!;

    [CascadingParameter]
    private InternalToastContext ToastContext { get; set; } = default!;

    /// <summary>
    /// The instance containing the programmatic API for the toast.
    /// </summary>
    [Parameter]
    public ToastInstance Instance { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _settings = Instance.Settings;
        ToastContext!.Register(this);

        if (_settings.Timeout.HasValue && _settings.Timeout == 0)
        {
            return;
        }
        _countdownTimer = new CountdownTimer(_settings.Timeout ?? ToastContext!.ToastContainer.Timeout).OnElapsed(Close);
        await _countdownTimer.StartAsync();
    }

    protected override void OnParametersSet()
    {
        if (Instance.ContentType == typeof(CommunicationToast) && _settings.TopCTAType == ToastTopCTAType.Action)
        {
            throw new InvalidOperationException("ToastTopCTAType.Action is not supported for a CommunicationToast  ");
        }
        if (Instance.ContentType != typeof(CommunicationToast) && _settings.TopCTAType == ToastTopCTAType.Timestamp)
        {
            throw new InvalidOperationException("ToastTopCTAType.Timestamp is not supported for a this type of toast");
        }
    }

    /// <summary>
    /// Closes the toast
    /// </summary>
    public void Close()
        => ToastContext?.ToastContainer.RemoveToast(Id!);

    public void HandleTopActionClick()
    {
        _settings.TopAction?.OnClick?.Invoke();
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
        _settings.PrimaryAction?.OnClick?.Invoke();
        Close();
    }

    public void HandleSecondaryActionClick()
    {
        _settings.SecondaryAction?.OnClick?.Invoke();
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