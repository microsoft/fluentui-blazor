using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

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

    /// <summary>
    /// In Debug mode, you can set this to true to display the status message on the page (on right, in yellow).
    /// </summary>
    [Parameter]
    public bool DebugDisplay { get; set; } = false;

    /// <summary />
    private string Content => string.IsNullOrEmpty(Message) ? string.Empty : Message;

    /// <summary />
    private bool AriaHidden => string.IsNullOrEmpty(Message);

    /// <summary />
    private string Style => DebugDisplay
                           ? "position: absolute; right: 0px; background: yellow; color: black; min-width: 40px; min-height: 15px;"
                           : "position: absolute; left: -9999px; width: 1px; height: 1px;";

}
