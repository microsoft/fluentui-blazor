using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentToast : FluentComponentBase, IDisposable
{
    private CountdownTimer? _countdownTimer;
    private ToastParameters _parameters = default!;

    [CascadingParameter]
    private InternalToastContext ToastContext { get; set; } = default!;

    /// <summary>
    /// Gets or sets the instance containing the programmatic API for the toast.
    /// </summary>
    [Parameter]
    public ToastInstance Instance { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _parameters = Instance.Parameters;
        ToastContext!.Register(this);

        if (_parameters.Timeout.HasValue && _parameters.Timeout == 0)
        {
            return;
        }
        _countdownTimer = new CountdownTimer(_parameters.Timeout ?? ToastContext!.ToastProvider.Timeout).OnElapsed(Close);
        await _countdownTimer.StartAsync();
    }

    protected override void OnParametersSet()
    {
        if (Instance.ContentType == typeof(CommunicationToast) && _parameters.TopCTAType == ToastTopCTAType.Action)
        {
            throw new InvalidOperationException("ToastTopCTAType.Action is not supported for a CommunicationToast  ");
        }
        if (Instance.ContentType != typeof(CommunicationToast) && _parameters.TopCTAType == ToastTopCTAType.Timestamp)
        {
            throw new InvalidOperationException("ToastTopCTAType.Timestamp is not supported for a this type of toast");
        }
    }

    /// <summary>
    /// Closes the toast
    /// </summary>
    public void Close()
        => ToastContext?.ToastProvider.RemoveToast(Id!);

    public void HandleTopActionClick()
    {
        _parameters.OnTopAction?.InvokeAsync(ToastResult.Ok<object?>(null));
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
        _parameters.OnPrimaryAction?.InvokeAsync();
        Close();
    }

    public void HandleSecondaryActionClick()
    {
        _parameters.OnSecondaryAction?.InvokeAsync();
        Close();
    }

    public void Dispose()
    {
        _countdownTimer?.Dispose();
        _countdownTimer = null;

        ToastContext?.Unregister(this);
    }
}
