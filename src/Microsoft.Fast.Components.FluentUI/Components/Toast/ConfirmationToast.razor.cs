using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class ConfirmationToast : IToastComponent
{
    private string? _toastId;

    [CascadingParameter]
    private FluentToast Toast { get; set; } = default!;

    [Parameter]
    public ToastIntent Intent { get; set; }

    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public ToastEndContentType EndContentType { get; set; } = ToastEndContentType.Dismiss;

    [Parameter]
    public ToastSettings Settings { get; set; } = default!;

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