using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class ProgressToast : IToastContentComponent<ProgressToastContent>
{


    [Parameter]
    public ProgressToastContent Content { get; set; } = default!;


    [CascadingParameter]
    private FluentToast Toast { get; set; } = default!;

    /// <summary>
    /// Closes the toast
    /// </summary>
    public void Close()
        => Toast.Close();

    public void HandlePrimaryActionClick()
    {
        //Content.PrimaryAction?.OnClick?.Invoke();
        Close();
    }

    public void HandleSecondaryActionClick()
    {
        //Content.SecondaryAction?.OnClick?.Invoke();
        Close();
    }
}