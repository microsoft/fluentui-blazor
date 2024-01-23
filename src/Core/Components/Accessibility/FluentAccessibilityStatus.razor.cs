using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// The status message will be read by screen readers.
/// This component must be loaded when the page is rendered (it cannot be displayed or hidden using conditions).
/// </summary>
public partial class FluentAccessibilityStatus
{
    /// <summary>
    /// Gets or sets the status message to be read by screen readers.
    /// </summary>
    [Parameter]
    public string? Message { get; set; }
}