using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public partial class MessageBox : IDialogContentComponent<MessageBoxData>
{
    [Parameter]
    public MessageBoxData Data { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;
}
