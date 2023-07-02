using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class ProgressToast : IToastContentComponent<ProgressToastContent>
{


    [Parameter]
    public ProgressToastContent ToastContent { get; set; } = default!;


    [CascadingParameter]
    private FluentToast Toast { get; set; } = default!;

    /// <summary>
    /// Closes the toast
    /// </summary>
    public void Close()
        => Toast.Close();

    public void HandlePrimaryActionClick()
    {
        //ToastContent.PrimaryAction?.OnClick?.Invoke();
        Close();
    }

    public void HandleSecondaryActionClick()
    {
        //ToastContent.SecondaryAction?.OnClick?.Invoke();
        Close();
    }
}