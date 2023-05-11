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
    public string? Id { get; set; }

    /// <summary>
    /// Notification specific settings
    /// </summary>
    [Parameter, EditorRequired]
    public ToastSettings Settings { get; set; } = default!;

    /// <summary>
    /// Gets or sets the intent of the notification. See <see cref="ToastIntent"/>
    /// </summary>
    [Parameter, EditorRequired]
    public ToastIntent Intent { get; set; }

    /// <summary>
    /// Gets or sets the main message of the notification.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public ToastAction? PrimaryAction { get; set; }

    [Parameter]
    public ToastAction? SecondaryAction { get; set; }

    [Parameter]
    public ToastEndContent EndContent { get; set; } = ToastEndContent.Dismiss;

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
        if (Settings.PrimaryAction is not null || Settings.SecondaryAction is not null)
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

    public FluentToast()
    {
        Id = Identifier.NewId();
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
        => ToastsContainer.RemoveToast(Id!);

    public void ToastClick()
        => Settings.OnClick?.Invoke();

    public void HandlePrimaryActionClick()
    {
        Settings.PrimaryAction?.OnClick?.Invoke();
        Close();
    }
        
    public void HandleSecondaryActionClick()
    {
        Settings.SecondaryAction?.OnClick?.Invoke();
        Close();
    }

    public void Dispose()
    {
        _countdownTimer?.Dispose();
        _countdownTimer = null;

        GC.SuppressFinalize(this);
    }
}