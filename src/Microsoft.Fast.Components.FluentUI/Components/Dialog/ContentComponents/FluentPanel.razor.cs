using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentPanel : FluentComponentBase, IPanelParameters, IDialogContentComponent
{

    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public HorizontalAlignment Alignment { get; set; }

    [Parameter]
    public bool Modal { get; set; }

    [Parameter]
    public bool ShowDismiss { get; set; }

    [Parameter]
    public string? PrimaryButton { get; set; }

    [Parameter]
    public string? SecondaryButton { get; set; }

    [Parameter]
    public string? Width { get; set; }

    [Parameter]
    public DialogSettings Settings { get; set; } = new();

    [Parameter]
    public EventCallback<DialogResult> OnDialogResult { get; set; }

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    public FluentPanel()
    {
        Id = Identifier.NewId();
    }
}
