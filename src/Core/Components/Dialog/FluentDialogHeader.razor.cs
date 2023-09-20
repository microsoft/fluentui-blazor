using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentDialogHeader : FluentComponentBase
{
    [CascadingParameter]
    private FluentDialog Dialog { get; set; } = default!;

    /// <summary>
    /// Title of the dialog
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// When true, shows the dismiss button in the header.
    /// </summary>
    [Parameter]
    public bool? ShowDismiss { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected override void OnInitialized()
    {
        if (Dialog is null)
        {
            throw new ArgumentNullException(nameof(Dialog), $"{nameof(FluentDialogHeader)} must be used inside {nameof(FluentDialog)}");
        }

        Dialog.SetContainsHeader(true);

        if (Dialog.Instance is not null)
        {
            ShowDismiss ??= Dialog.Instance.Parameters.ShowDismiss;
            Title ??= Dialog.Instance.Parameters.Title;
        }
    }
}