using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// Supplies information about a mouse event that is being raised, including the
/// HTML id of the element clicked
/// </summary>
public class TargetIdMouseEventArgs : MouseEventArgs
{
    /// <summary>
    /// The HTML id of the element clicked.
    /// </summary>
    public string? TargetId { get; set; }
}
