// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------
namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Options for the Fluent UI Blazor component library.
/// </summary>
public class LibraryToastOptions
{
    private const int _defaultMaxToastCount = 4;
    private const int _defaultTimeout = 7000;
    private const ToastPosition _defaultPosition = ToastPosition.BottomEnd;
    private const int _defaultVerticalOffset = 16;
    private const int _defaultHorizontalOffset = 20;
    private const bool _defaultPauseOnHover = true;
    private const bool _defaultPauseOnWindowBlur = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="LibraryToastOptions"/> class.
    /// </summary>
    internal LibraryToastOptions()
    {
    }

    /// <summary>
    /// Gets or sets the maximum number of toasts displayed at the same time.
    /// </summary>
    public int MaxToastCount { get; set; } = _defaultMaxToastCount;

    /// <summary>
    /// Gets or sets the default timeout duration in milliseconds for visible toasts.
    /// </summary>
    public int Timeout { get; set; } = _defaultTimeout;

    /// <summary>
    /// Gets or sets the default toast position.
    /// </summary>
    public ToastPosition? Position { get; set; } = _defaultPosition;

    /// <summary>
    /// Gets or sets the default vertical offset in pixels.
    /// </summary>
    public int VerticalOffset { get; set; } = _defaultVerticalOffset;

    /// <summary>
    /// Gets or sets the default horizontal offset in pixels.
    /// </summary>
    public int HorizontalOffset { get; set; } = _defaultHorizontalOffset;

    /// <summary>
    /// Gets or sets a value indicating whether visible toasts pause timeout while hovered.
    /// </summary>
    public bool PauseOnHover { get; set; } = _defaultPauseOnHover;

    /// <summary>
    /// Gets or sets a value indicating whether visible toasts pause timeout while the window is blurred.
    /// </summary>
    public bool PauseOnWindowBlur { get; set; } = _defaultPauseOnWindowBlur;
}
