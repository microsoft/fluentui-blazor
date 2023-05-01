using System.ComponentModel;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public enum ToasterPosition
{
    /// <summary>
    /// Toasts are displayed in the bottom-left corner of the screen
    /// </summary>
    [Description("fluent-toasts-bottom-left")]
    BottomLeft,

    /// <summary>
    /// Toasts are displayed in the bottom-start corner of the screen
    /// </summary>
    [Description("fluent-toasts-bottom-start")]
    BottomStart,
    /// <summary>
    /// Toasts are displayed in the bottom-center of the screen
    /// </summary>
    [Description("fluent-toasts-bottom-center")]
    BottomCenter,

    /// <summary>
    /// Toasts are displayed in the bottom-right corner of the screen
    /// </summary>
    [Description("fluent-toasts-bottom-right")]
    BottomRight,

    /// <summary>
    /// Toasts are displayed in the bottom-end corner of the screen
    /// </summary>
    [Description("fluent-toasts-bottom-end")]
    BottomEnd,

    /// <summary>
    /// Toasts are displayed in the top-left corner of the screen
    /// </summary>
    [Description("fluent-toasts-top-left")]
    TopLeft,

    /// <summary>
    /// Toasts are displayed in the top-left corner of the screen
    /// </summary>
    [Description("fluent-toasts-top-start")]
    TopStart,

    /// <summary>
    /// Toasts are displayed in the top-center of the screen
    /// </summary>
    [Description("fluent-toasts-top-center")]
    TopCenter,

    /// <summary>
    /// Toasts are displayed in the top-right corner of the screen
    /// </summary>
    [Description("fluent-toasts-top-right")]
    TopRight,

    /// <summary>
    /// Toasts are displayed in the top-end corner of the screen
    /// </summary>
    [Description("fluent-toasts-top-end")]
    TopEnd,
}
