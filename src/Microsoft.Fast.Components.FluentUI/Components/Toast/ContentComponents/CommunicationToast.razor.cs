using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;
public partial class CommunicationToast : IToastContentComponent<CommunicationToastData>
{

    [Parameter]
    public CommunicationToastData Data { get; set; } = default!;


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
        //Data.PrimaryAction?.OnClick?.Invoke();
        Close();
    }

    public void HandleSecondaryActionClick()
    {
        //Data.SecondaryAction?.OnClick?.Invoke();
        Close();
    }
}
