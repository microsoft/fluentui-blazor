using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentToast2 : IDisposable
{
    [CascadingParameter] private FluentToasts2 ToastsContainer { get; set; } = default!;

    [Parameter, EditorRequired] public string? ToastId { get; set; }
    [Parameter, EditorRequired] public ToastSettings Settings { get; set; } = default!;
    [Parameter] public ToastIntent? Intent { get; set; }
    [Parameter] public RenderFragment? Message { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    private CountdownTimer? _countdownTimer;

    protected override async Task OnInitializedAsync()
    {
        _countdownTimer = new CountdownTimer(Settings.Timeout)
            .OnElapsed(Close);

        await _countdownTimer.StartAsync();
    }

    /// <summary>
    /// Closes the toast
    /// </summary>
    public void Close()
        => ToastsContainer.RemoveToast(ToastId!);

    private void ToastClick()
        => Settings.OnClick?.Invoke();

    
    public void Dispose()
    {
        _countdownTimer?.Dispose();
        _countdownTimer = null;

        GC.SuppressFinalize(this);
    }
}