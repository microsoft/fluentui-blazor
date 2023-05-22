using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentToast : FluentComponentBase, IDisposable
{
    private CountdownTimer? _countdownTimer;

    [CascadingParameter]
    private InternalToastContext ToastContext { get; set; } = default!;

    /// <summary>
    /// Notification instance specific settings
    /// </summary>
    [Parameter]
    public ToastSettings Settings { get; set; } = default!;

    /// <summary>
    /// Gets or sets the intent of the notification. See <see cref="ToastIntent"/>
    /// </summary>
    [Parameter]
    public ToastIntent Intent { get; set; }

    /// <summary>
    /// Gets or sets the main message of the notification.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public ToastEndContentType EndContentType { get; set; } = ToastEndContentType.Dismiss;

    [Parameter]
    public ToastAction? PrimaryAction { get; set; } = default;

    [Parameter]
    public DateTime TimeStamp { get; set; } = DateTime.Now;

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

    //protected override void OnParametersSet()
    //{
    //    if (Settings.PercentageComplete is not null && (Settings.PercentageComplete < 0 || Settings.PercentageComplete > 100))
    //    {
    //        throw new ArgumentOutOfRangeException(nameof(Settings.PercentageComplete), "PercentageComplete must be between 0 and 100");
    //    }
    //    else
    //    {
    //        _showBody = true;
    //    }
    //    if (Settings.PrimaryAction is not null || Settings.SecondaryAction is not null)
    //    {
    //        _showBody = true;
    //    }
    //    if (!string.IsNullOrWhiteSpace(Settings.Subtitle))
    //    {
    //        _showBody = true;
    //    }
    //    if (!string.IsNullOrWhiteSpace(Settings.Details))
    //    {
    //        _showBody = true;
    //    }
    //}

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

    public void ToastClick()
        => Settings.OnClick?.Invoke();

    public void HandlePrimaryActionClick()
    {
        PrimaryAction?.OnClick?.Invoke();
        Close();
    }


    public void Dispose()
    {
        _countdownTimer?.Dispose();
        _countdownTimer = null;

        GC.SuppressFinalize(this);
    }
}