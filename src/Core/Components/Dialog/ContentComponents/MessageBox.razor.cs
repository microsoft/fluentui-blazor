using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class MessageBox : IDialogContentComponent<MessageBoxContent>
{
    [Parameter]
    public MessageBoxContent Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; }

    public MessageBox()
    {
        Dialog = new FluentDialog();
    }
}
