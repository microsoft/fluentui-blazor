namespace Microsoft.FluentUI.AspNetCore.Components;

public class ActionLink<T>
{
    /// <summary>
    /// The text to show for the link
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// The address to navigate to when the link is clicked
    /// </summary>
    public string? Href { get; set; }

    /// <summary>
    /// The target window or frame to open the link in
    /// </summary>
    public string? Target { get; set; }

    /// <summary>
    /// The function to call when the link is clicked
    /// </summary>
    public Func<T, Task>? OnClick { get; set; }
}
