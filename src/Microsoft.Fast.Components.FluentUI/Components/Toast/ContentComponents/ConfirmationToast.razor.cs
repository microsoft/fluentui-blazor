using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class ConfirmationToast : IToastComponent
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
    public ToastAction? PrimaryAction { get; set; }


    protected override void OnInitialized()
    {
        _toastId = Toast.Id;
        Settings = Toast.Settings;
    }

    protected override void OnParametersSet()
    {
        if (EndContentType == ToastEndContentType.Timestamp)
            throw new InvalidOperationException("EndContentType.Timestamp is not supported for a ConfirmationToast  ");
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


}