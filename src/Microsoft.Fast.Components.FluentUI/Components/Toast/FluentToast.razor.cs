using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentToast : FluentComponentBase, IToastComponent, IDisposable
{
    private CountdownTimer? _countdownTimer;

    [CascadingParameter]
    private InternalToastContext ToastContext { get; set; } = default!;

    [Parameter]
    public ToastIntent Intent { get; set; }

    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public ToastEndContentType EndContentType { get; set; } = ToastEndContentType.Dismiss;

    [Parameter]
    public ToastSettings Settings { get; set; } = default!;

    [Parameter]
    public ToastAction? PrimaryAction { get; set; } = default;

    [Parameter]
    public DateTime TimeStamp { get; set; } = DateTime.Now;

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

    public FluentToast(ToastIntent intent, string title, ToastSettings settings) : this()
    {
        Title = title;
        Intent = intent;
        Settings = settings;
    }

    public FluentToast(RenderFragment renderFragment, ToastSettings settings) : this()
    {
        ChildContent = renderFragment;
        Settings = settings;
    }

    /// <summary>
    /// Closes the toast
    /// </summary>
    public void Close()
        => ToastContext.ToastsContainer.RemoveToast(Id!);


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