using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentDialog : FluentComponentBase
{
    /// <summary>
    /// Gets or sets if the dialog is shown modal
    /// </summary>
    [Parameter]
    public bool? Modal { get; set; }

    /// <summary>
    /// Gets or sets if the dialog is hidden
    /// </summary>
    [Parameter]
    public bool Hidden { get; set; } = false;

    public void Show() => Hidden = false;
    public void Hide() => Hidden = true;
}