using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;
public partial class CommunicationToast : IToastContentComponent<CommunicationToastContent>
{

    [Parameter]
    public CommunicationToastContent Content { get; set; } = default!;

    [CascadingParameter]
    private FluentToast Toast { get; set; } = default!;

    /// <summary>
    /// Closes the toast
    /// </summary>
    public void Close()
        => Toast.Close();

    public void HandlePrimaryActionClick()
    {
        Close();
    }

    public void HandleSecondaryActionClick()
    {
        Close();
    }
}
