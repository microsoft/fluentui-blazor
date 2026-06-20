// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------
namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Options for the Fluent UI Blazor component library.
/// </summary>
public class LibraryToastOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LibraryToastOptions"/> class.
    /// </summary>
    internal LibraryToastOptions()
    {
    }

    /// <summary>
    /// Gets or sets the maximum number of toasts displayed at the same time.
    /// Default is 4 toasts, which is the recommended maximum number of toasts to be displayed according to Fluent UI design guidelines.
    /// When the maximum count is reached, the oldest toast is dismissed when a new toast is added. 
    /// Setting this value to 0 allows an unlimited number of toasts to be displayed, which can lead to a poor user experience and is not recommended.
    /// </summary>
    public int MaxToastCount { get; set; } = 4;

    /// <summary>
    /// Gets or sets the lifetime of the toast.
    /// When set to a positive value, the toast is automatically removed after this duration elapses.
    /// When `null`, the toast stays visible until it is dismissed programmatically or by the user.
    /// </summary>
    public TimeSpan? Lifetime { get; set; }

    /// <summary>
    /// Gets or sets the default toast position.
    /// </summary>
    public ToastPosition Position { get; set; } = ToastPosition.BottomEnd;

    /// <summary>
    /// Gets or sets the default vertical offset in pixels.
    /// Default is 16px, which is the recommended offset according to Fluent UI design guidelines.
    /// </summary>
    public int VerticalOffset { get; set; } = 16;

    /// <summary>
    /// Gets or sets the default horizontal offset in pixels.
    /// Default is 20px, which is the recommended offset according to Fluent UI design guidelines.
    /// </summary>
    public int HorizontalOffset { get; set; } = 20;

    /// <summary>
    /// Gets or sets a value indicating whether visible toasts pause timeout while hovered.
    /// Default is `true`, which is the recommended behavior according to Fluent UI design guidelines. 
    /// </summary>
    public bool PauseOnHover { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether visible toasts pause timeout while the window is blurred.
    /// Default is `true`, which is the recommended behavior according to Fluent UI design guidelines.
    /// </summary>
    public bool PauseOnWindowBlur { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether visible toasts can be dismissed by the user.
    /// Default is `false`, which is the recommended behavior according to Fluent UI design guidelines.
    /// </summary>
    public bool AllowDismiss { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the toast uses inverted colors.
    /// Default is `false`, which is the recommended behavior according to Fluent UI design guidelines.
    /// </summary>
    public bool Inverted { get; set; }
}
