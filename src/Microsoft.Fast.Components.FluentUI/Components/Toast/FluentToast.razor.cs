using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentToast : FluentComponentBase, IDisposable
{
    private CountdownTimer? _countdownTimer;

    [CascadingParameter]
    private InternalToastContext ToastContext { get; set; } = default!;

    /// <summary>
    /// Notification specific settings
    /// </summary>
    [Parameter]
    public ToastSettings Settings { get; set; } = default!;

    /// <summary>
    /// Gets or sets the the icon to display in the notification
    /// Use a constant from the <see cref="FluentIcons" /> class for the <c>Name</c> value
    /// The <c>Color</c> value determines the display color of the icon.
    /// It is based on either the <see cref="ToastIntent"/> or the active Accent color 
    /// The <c>Variant</c> value determines the variant of the icon.
    /// For the intents Success, Warning, Error and Information the defualt is IconVariant.Filled.
    /// For all other intents the default is IconVariant.Regular.
    /// </summary>
    [Parameter]
    public (string Name, Color Color, IconVariant Variant)? Icon { get; set; }

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
        //Settings.PrimaryAction?.OnClick?.Invoke();
        Close();
    }

    public void HandleSecondaryActionClick()
    {
        //Settings.SecondaryAction?.OnClick?.Invoke();
        Close();
    }

    public void Dispose()
    {
        _countdownTimer?.Dispose();
        _countdownTimer = null;

        GC.SuppressFinalize(this);
    }
}