namespace Microsoft.FluentUI.AspNetCore.Components;
public class ActionButton<T>
{
    /// <summary>
    /// Gets or sets the text to show for the button.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the function to call when the link is clicked.
    /// </summary>
    public Func<T, Task>? OnClick { get; set; }
}
