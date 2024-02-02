namespace Microsoft.FluentUI.AspNetCore.Components;

internal class CountdownTimer : IDisposable
{
    private readonly PeriodicTimer _timer;
    private readonly int _ticksToTimeout;
    private readonly CancellationToken _cancellationToken;
    private int _percentComplete;
    private bool _paused;
    private Action? _elapsedDelegate;

    internal CountdownTimer(int timeout, CancellationToken cancellationToken = default)
    {
        _ticksToTimeout = 100;
        _timer = new PeriodicTimer(TimeSpan.FromMilliseconds(timeout / 100));
        _cancellationToken = cancellationToken;
    }

    internal CountdownTimer OnElapsed(Action elapsedDelegate)
    {
        _elapsedDelegate = elapsedDelegate;
        return this;
    }
    internal async Task StartAsync()
    {
        _percentComplete = 0;
        await DoWorkAsync();
    }

    private async Task DoWorkAsync()
    {
        while (await _timer.WaitForNextTickAsync(_cancellationToken) && !_cancellationToken.IsCancellationRequested)
        {
            if (!_paused)
            {
                _percentComplete++;

                if (_percentComplete == _ticksToTimeout)
                {
                    _elapsedDelegate?.Invoke();
                }
            }
        }
    }

    internal void Pause() => _paused = true;
    internal void Resume() => _paused = false;

    public void Dispose() => _timer.Dispose();
}
