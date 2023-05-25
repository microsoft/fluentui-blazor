using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;
public partial class CommunicationToast : IToastComponent
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
    public DateTime TimeStamp { get; set; } = DateTime.Now;

    [Parameter]
    public string? Subtitle { get; set; }

    [Parameter]
    public string? Details { get; set; }

    [Parameter]
    public ToastAction? PrimaryAction { get; set; }


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
