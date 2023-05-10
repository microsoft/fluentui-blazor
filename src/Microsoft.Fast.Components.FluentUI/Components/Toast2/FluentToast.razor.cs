using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentToast : IDisposable
{
    private bool _showCloseButton = true;
    private bool _showBody;
    private CountdownTimer? _countdownTimer;

    [CascadingParameter]
    private FluentToasts ToastsContainer { get; set; } = default!;

    /// <summary>
    /// Gets or sets the id that identiefies a specific notification
    /// </summary>
    [Parameter, EditorRequired]
    public string? ToastId { get; set; }

    /// <summary>
    /// Notification specific settings
    /// </summary>
    [Parameter, EditorRequired]
    public ToastSettings Settings { get; set; } = default!;

    /// <summary>
    /// Gets or sets the intent of the notification. See <see cref="ToastIntent"/>
    /// </summary>
    [Parameter]
    public ToastIntent? Intent { get; set; }

    /// <summary>
    /// Gets or sets the main message of the notification.
    /// </summary>
    [Parameter]
    public RenderFragment? Title { get; set; }

   

    /// <summary>
    /// Use a custom component in the notfication
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _countdownTimer = new CountdownTimer(Settings.Timeout)
            .OnElapsed(Close);

        await _countdownTimer.StartAsync();
    }

    protected override void OnParametersSet()
    {
        if (Settings.PercentageComplete is not null && (Settings.PercentageComplete < 0 || Settings.PercentageComplete > 100))
        {
            throw new ArgumentOutOfRangeException(nameof(Settings.PercentageComplete), "PercentageComplete must be between 0 and 100");
        }
        else
        {
            _showBody = true;
        }
        if (Settings.PrimaryCallToAction is not null || Settings.SecondaryCallToAction is not null)
        {
            _showBody = true;
        }
        if (!string.IsNullOrWhiteSpace(Settings.Subtitle))
        {
            _showBody = true;
        }
        if (!string.IsNullOrWhiteSpace(Settings.Details))
        {
            _showBody = true;
        }
    }

    /// <summary>
    /// Closes the toast
    /// </summary>
    public void Close()
        => ToastsContainer.RemoveToast(ToastId!);

    private void ToastClick()
        => Settings.OnClick?.Invoke();

    private void HandleCta1Click()
        => Settings.PrimaryCallToAction?.OnClick?.Invoke();

    private void HandleCta2Click()
        => Settings.SecondaryCallToAction?.OnClick?.Invoke();

    public void Dispose()
    {
        _countdownTimer?.Dispose();
        _countdownTimer = null;

        GC.SuppressFinalize(this);
    }
}