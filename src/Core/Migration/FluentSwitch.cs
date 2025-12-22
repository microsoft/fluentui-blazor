// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentSwitch
{
    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    [Obsolete("This property is not supported anymore and will be removed in a future release.")]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the checked message
    /// </summary>
    [Parameter]
    [Obsolete("This property is not supported anymore and will be removed in a future release.")]
    public string? CheckedMessage { get; set; }

    /// <summary>
    /// Gets or sets the unchecked message
    /// </summary>
    [Parameter]
    [Obsolete("This property is not supported anymore and will be removed in a future release.")]
    public string? UncheckedMessage { get; set; }
}
