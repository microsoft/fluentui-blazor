namespace Microsoft.FluentUI.AspNetCore.Components;

public class SplitterResizedEventArgs : EventArgs
{
    /// <summary>
    /// Gets the size of panel 1 (top/left) after a resize operation.
    /// </summary>
    public int Panel1Size { get; set; }

    /// <summary>
    /// Gets the size of the panel 2 (bottom/right) after a resize operation.
    /// </summary>  
    public int Panel2Size { get; set; }
}
