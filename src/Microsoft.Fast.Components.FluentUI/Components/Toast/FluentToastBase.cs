using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Microsoft.Fast.Components.FluentUI;

public abstract partial class FluentToastBase : FluentComponentBase, IDisposable
{
    [CascadingParameter] 
    internal InternalToastContext InternalToastContext { get; set; } = default!;
    
    
    [CascadingParameter]
    public FluentToast Toast { get; set; } = default!;


    /// <summary>
    /// Gets or sets the intent of the notification. See <see cref="ToastIntent"/>
    /// </summary>
    [Parameter, EditorRequired]
    public ToastIntent Intent { get; set; }

    /// <summary>
    /// The main message of the notification.
    /// </summary>
    [Parameter, EditorRequired]
    public string? Title { get; set; }

    [Parameter]
    public ToastEndContentType EndContentType { get; set; }

    /// <summary>
    /// Notification specific settings
    /// </summary>
    [Parameter, EditorRequired]
    public ToastSettings Settings { get; set; } = default!;


    [Parameter]
    public ToastAction? PrimaryAction { get; set; }

    [Parameter]
    public ToastAction? SecondaryAction { get; set; }

    [Parameter]
    public DateTime TimeStamp { get; set; } = DateTime.Now;

    /// <summary>
    /// Gets or sets a callback when a toast is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<string> OnClick { get; set; }

    /// <summary>
    /// Use a custom component in the notfication
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private CountdownTimer? _countdownTimer;

    protected override async Task OnInitializedAsync()
    {
        _countdownTimer = new CountdownTimer(Settings.Timeout)
            .OnElapsed(Close);

        await _countdownTimer.StartAsync();
    }

    /// <summary>
    /// Overridden by derived components to provide rendering logic for the toast
    /// </summary>
    /// <param name="builder">The current <see cref="RenderTreeBuilder" />.</param>
    protected internal abstract void ToastContent(RenderTreeBuilder builder);

    /// <summary>
    /// Constructs an instance of <see cref="FluentToastBase" />.
    /// </summary>
    public FluentToastBase()
    {
        Id = Identifier.NewId();
    }

    public async Task HandleOnClickAsync()
    {
        await OnClick.InvokeAsync(Id);
    }

    /// <summary>
    /// Closes the toast
    /// </summary>
    public void Close()
       => InternalToastContext.ToastsContainer.RemoveToast(Id!);

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
