using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class ProgressToast : IToastContentComponent<ProgressToastData>
{


    [Parameter]
    public ProgressToastData Data { get; set; } = default!;


    [CascadingParameter]
    private FluentToast Toast { get; set; } = default!;

    /// <summary>
    /// Closes the toast
    /// </summary>
    public void Close()
        => Toast.Close();

    public void HandlePrimaryActionClick()
    {
        //Data.PrimaryAction?.OnClick?.Invoke();
        Close();
    }

    public void HandleSecondaryActionClick()
    {
        //Data.SecondaryAction?.OnClick?.Invoke();
        Close();
    }
}