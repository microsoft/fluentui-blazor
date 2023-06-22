using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentDialogContent : FluentComponentBase, IDialogParameters, IDialogContentComponent
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

    /// <summary>
    /// Width of the panel. Must be a valid CSS width value like "600px" or "3em"
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Height of the panel. Must be a valid CSS height value like "600px" or "3em"
    /// Only used if Alignment is set to "HorizotalAlignment.Center"
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    [Parameter]
    public DialogSettings Settings { get; set; } = new();

    [Parameter]
    public EventCallback<DialogResult> OnDialogResult { get; set; }

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    public FluentDialogContent()
    {
        Id = Identifier.NewId();
    }

    protected override void OnParametersSet()
    {
        if (Alignment is not HorizontalAlignment.Center && !string.IsNullOrWhiteSpace(Height))
        {
            throw new ArgumentException($"Height can only be set when Alignment is set to {nameof(HorizontalAlignment.Center)}");
        }
    }
}
