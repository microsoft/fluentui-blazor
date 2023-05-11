namespace Microsoft.Fast.Components.FluentUI;

internal class CountdownTimer : IDisposable
{
    private readonly PeriodicTimer _timer;
    private readonly int _ticksToTimeout;
    private readonly CancellationToken _cancellationToken;
    private int _percentComplete;
    
    private Func<int, Task>? _tickDelegate;
    private Action? _elapsedDelegate;
    internal CountdownTimer(int timeout, CancellationToken cancellationToken = default)
    {
        _ticksToTimeout = 100;
        _timer = new PeriodicTimer(TimeSpan.FromMilliseconds(timeout * 10));
        _cancellationToken = cancellationToken;
    }
    internal CountdownTimer OnTick(Func<int, Task> updateProgressDelegate)
    {
        _tickDelegate = updateProgressDelegate;
        return this;
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

            _percentComplete++;
            
            if (_tickDelegate != null)
            {
                await _tickDelegate(_percentComplete);
            }

            if (_percentComplete == _ticksToTimeout)
            {
                _elapsedDelegate?.Invoke();
            }
        }
    }
    public void Dispose() => _timer.Dispose();
}
