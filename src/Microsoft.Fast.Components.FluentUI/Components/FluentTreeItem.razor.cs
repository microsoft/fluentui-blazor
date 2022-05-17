using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentTreeItem : FluentComponentBase
{
    /// <summary>
    /// Gets or sets if the tree item is disabled
    /// </summary>
    [Parameter]
    public bool? Disabled { get; set; }

    /// <summary>
    /// Gets or sets if the tree item is selected
    /// </summary>
    [Parameter]
    public bool? Selected { get; set; }

    /// <summary>
    /// Gets or sets if the tree item is expanded
    /// </summary>
    [Parameter]
    public bool? Expanded { get; set; }
}