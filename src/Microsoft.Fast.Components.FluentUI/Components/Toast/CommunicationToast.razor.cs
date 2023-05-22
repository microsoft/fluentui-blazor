using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;
public partial class CommunicationToast
{
    [Parameter]
    public string? Subtitle { get; set; }

    [Parameter]
    public string? Details { get; set; }

    [Parameter]
    public ToastAction? SecondaryAction { get; set; }


    protected override void OnParametersSet()
    {
        if (SecondaryAction is not null && EndContentType == ToastEndContentType.Action)
            throw new InvalidOperationException("SecondaryAction is not supported when EndContentType is set to Action");
    }
}
