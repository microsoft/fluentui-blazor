using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentDialogHeader : FluentComponentBase
{
    [CascadingParameter]
    private FluentDialog? Dialog { get; set; }

    /// <summary>
    /// Title of the dialog
    /// </summary>
    [Parameter]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// When true, shows the dismiss button in the header.
    /// </summary>
    [Parameter]
    public bool ShowDismiss { get; set; }

    protected override void OnParametersSet()
    {
        if (Dialog is null)
        {
            throw new ArgumentNullException(nameof(Dialog), "FluentDialogHeader must be used inside FluentDialog");
        }
    }
}