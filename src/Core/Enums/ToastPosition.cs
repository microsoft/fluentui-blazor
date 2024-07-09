using System.ComponentModel.DataAnnotations;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public enum ToastPosition
{
    /// <summary>
    /// Toasts are displayed in the bottom-left corner of the screen
    /// </summary>
    [Display(Name = "fluent-toast-bottom-left")]
    BottomLeft,

    /// <summary>
    /// Toasts are displayed in the bottom-start corner of the screen
    /// </summary>
    [Display(Name = "fluent-toast-bottom-start")]
    BottomStart,
    /// <summary>
    /// Toasts are displayed in the bottom-center of the screen
    /// </summary>
    [Display(Name = "fluent-toast-bottom-center")]
    BottomCenter,

    /// <summary>
    /// Toasts are displayed in the bottom-right corner of the screen
    /// </summary>
    [Display(Name = "fluent-toast-bottom-right")]
    BottomRight,

    /// <summary>
    /// Toasts are displayed in the bottom-end corner of the screen
    /// </summary>
    [Display(Name = "fluent-toast-bottom-end")]
    BottomEnd,

    /// <summary>
    /// Toasts are displayed in the top-left corner of the screen
    /// </summary>
    [Display(Name = "fluent-toast-top-left")]
    TopLeft,

    /// <summary>
    /// Toasts are displayed in the top-left corner of the screen
    /// </summary>
    [Display(Name = "fluent-toast-top-start")]
    TopStart,

    /// <summary>
    /// Toasts are displayed in the top-center of the screen
    /// </summary>
    [Display(Name = "fluent-toast-top-center")]
    TopCenter,

    /// <summary>
    /// Toasts are displayed in the top-right corner of the screen
    /// </summary>
    [Display(Name = "fluent-toast-top-right")]
    TopRight,

    /// <summary>
    /// Toasts are displayed in the top-end corner of the screen
    /// </summary>
    [Display(Name = "fluent-toast-top-end")]
    TopEnd,
}
