using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;
public partial class CommunicationToast : IToastContentComponent<CommunicationToastContent>
{

    [Parameter]
    public CommunicationToastContent Content { get; set; } = default!;


    [CascadingParameter]
    private FluentToast Toast { get; set; } = default!;

    protected override void OnParametersSet()
    {
        //if (TopCTAType == ToastTopCTAType.Action)
        //    throw new InvalidOperationException("ToastTopCTAType.Action is not supported for a CommunicationToast  ");
    }

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
