using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;
public partial class CommunicationToast : FluentToast, IToastComponent
{
    [CascadingParameter]
    private FluentToast Toast { get; set; } = default!;

    [Parameter]
    public string? Subtitle { get; set; }

    [Parameter]
    public string? Details { get; set; }

    [Parameter]
    public ToastAction? SecondaryAction { get; set; }

    protected override void OnInitialized()
    {
        Id = Toast.Id;
        Settings = Toast.Settings;
    }

    protected override void OnParametersSet()
    {
        if (EndContentType == ToastEndContentType.Action)
            throw new InvalidOperationException("EndContentType.Action is not supported for a CommunicationToast  ");
    }

    public void HandleSecondaryActionClick()
    {
        SecondaryAction?.OnClick?.Invoke();
        Close();
    }

}
