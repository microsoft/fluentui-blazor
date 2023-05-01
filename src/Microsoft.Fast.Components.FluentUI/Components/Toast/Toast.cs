
namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public class Toast : IDisposable
{
    internal Toast(ToastOptions options)
    {
        Message = options.Message;
        State = new ToastMessageState(options);
        Timer = new Timer(TimerElapsed, null, Timeout.Infinite, Timeout.Infinite);
    }

    internal event Action<Toast>? OnClose;

    internal event Action? OnUpdate;

    public string Message { get; }

    public ToastIntent Intent => State.Options.Intent;

    internal ToastMessageState State { get; }

    private Timer? Timer { get; set; }

    public void Close()
    {
        TransitionTo(ToastState.Hiding);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    internal async Task ClickedAsync(bool fromCloseIcon)
    {
        if (fromCloseIcon)
        {
            TransitionTo(ToastState.Hiding);
        }
        else
        {
            if (State.Options.OnClick != null)
            {
                await State.Options.OnClick.Invoke(this);
            }
        }
    }

    internal void Initialize() => TransitionTo(ToastState.Showing);

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        StopTimer();

        var timer = Timer;
        Timer = null;

        timer?.Dispose();
    }

    private void StartTimer(int duration)
    {
        State.Stopwatch.Restart();
        Timer?.Change(duration, Timeout.Infinite);
    }

    private void StopTimer()
    {
        State.Stopwatch.Stop();
        Timer?.Change(Timeout.Infinite, Timeout.Infinite);
    }

    private void TimerElapsed(object? timerState)
    {
        switch (State.ToastState)
        {
            case ToastState.Showing:
                TransitionTo(ToastState.Visible);
                break;
            case ToastState.Visible:
                TransitionTo(ToastState.Hiding);
                break;
            case ToastState.Hiding:
                OnClose?.Invoke(this);
                break;
        }
    }

    private void TransitionTo(ToastState state)
    {
        StopTimer();
        State.ToastState = state;

        switch (state)
        {
            // Showing to Visible
            case ToastState.Showing:
                if (State.Options.Global.ShowTransitionDuration <= 0)
                {
                    TransitionTo(ToastState.Visible);
                }
                else
                {
                    StartTimer(State.Options.Global.ShowTransitionDuration);
                }

                break;

            // Visible to Hiding
            case ToastState.Visible:
                if (State.Options.Global.AutoHide)
                {
                    if (State.Options.Global.VisibleStateDuration <= 0)
                    {
                        TransitionTo(ToastState.Hiding);
                    }
                    else
                    {
                        StartTimer(State.Options.Global.VisibleStateDuration);
                    }
                }

                break;

            // Hiding to Closed
            case ToastState.Hiding:
                if (State.Options.Global.HideTransitionDuration <= 0)
                {
                    OnClose?.Invoke(this);
                }
                else
                {
                    StartTimer(State.Options.Global.HideTransitionDuration);
                }

                break;
        }

        OnUpdate?.Invoke();
    }
}
