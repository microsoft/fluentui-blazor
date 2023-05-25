using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;
public partial class CommunicationToast : IToastComponent
{
    private string? _toastId;

    [CascadingParameter]
    private FluentToast Toast { get; set; } = default!;

    /// <inheritdoc/>
    [Parameter]
    public ToastIntent Intent { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public string? Title { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public ToastEndContentType EndContentType { get; set; } = ToastEndContentType.Dismiss;

    /// <inheritdoc/>
    [Parameter]
    public ToastSettings Settings { get; set; } = default!;

    /// <summary>
    /// The primary action of the notification. Will be shown after title or at bottom of the toast.
    /// </summary>
    [Parameter]
    public DateTime TimeStamp { get; set; } = DateTime.Now;

    /// <summary>
    /// The Subtitle will be shown below the title in a slightly smaller and lighter font. 
    /// </summary>
    [Parameter]
    public string? Subtitle { get; set; }

    /// <summary>
    /// Used to show additional details about the notification.
    /// </summary>
    [Parameter]
    public string? Details { get; set; }

    /// <summary>
    /// The primary action of the notification. Will be shown after title or at bottom of the toast.
    /// </summary>
    [Parameter]
    public ToastAction? PrimaryAction { get; set; }

    /// <summary>
    /// The secondary action of the notification. Will be shown after primary action at bottom of the toast.
    /// </summary>
    [Parameter]
    public ToastAction? SecondaryAction { get; set; }

    protected override void OnInitialized()
    {
        _toastId = Toast.Id;
        Settings = Toast.Settings;
    }

    protected override void OnParametersSet()
    {
        if (EndContentType == ToastEndContentType.Action)
            throw new InvalidOperationException("EndContentType.Action is not supported for a CommunicationToast  ");
    }

    /// <summary>
    /// Closes the toast
    /// </summary>
    public void Close()
        => Toast.Close();

    public void HandlePrimaryActionClick()
    {
        PrimaryAction?.OnClick?.Invoke();
        Close();
    }

    public void HandleSecondaryActionClick()
    {
        SecondaryAction?.OnClick?.Invoke();
        Close();
    }
}
