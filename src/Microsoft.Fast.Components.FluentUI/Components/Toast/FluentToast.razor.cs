using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentToast : FluentComponentBase, IDisposable
{
    private CountdownTimer? _countdownTimer;

    [CascadingParameter]
    private FluentToastContainer ToastContainer { get; set; } = default!;

    /// <inheritdoc/>
    [Parameter]
    public ToastIntent Intent { get; set; }


    /// <inheritdoc/>
    [Parameter]
    public ToastSettings Settings { get; set; } = default!;

    /// <summary>
    /// The primary action of the notification. Will be shown after title or at bottom of the toast.
    /// </summary>
    [Parameter]
    public ToastAction? PrimaryAction { get; set; } = default;

    /// <summary>
    /// Record a timestamp of when the toast was created.
    /// </summary>
    [Parameter]
    public DateTime Timestamp { get; set; } = DateTime.Now;


    [Parameter]
    public ToastInstance Instance { get; set; } = default!;

    /// <summary>
    /// Use a custom component in the notification
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }


    protected override async Task OnInitializedAsync()
    {
        _countdownTimer = new CountdownTimer(Settings.Timeout)
            .OnElapsed(Close);

        await _countdownTimer.StartAsync();
    }

    public FluentToast()
    {
    }

    /// <summary>
    /// Closes the toast
    /// </summary>
    public void Close()
        => ToastContainer.RemoveToast(Id!);

    public void HandlePrimaryActionClick()
    {
        PrimaryAction?.OnClick?.Invoke();
        Close();
    }

    public void PauseTimeout()
    {
        _countdownTimer?.Pause();
    }

    public void ResumeTimeout()
    {
        _countdownTimer?.Resume();
    }

    public void Dispose()
    {
        _countdownTimer?.Dispose();
        _countdownTimer = null;

        GC.SuppressFinalize(this);
    }
}