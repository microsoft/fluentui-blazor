using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// A component implementing this interface can be used as toast content.
/// </summary>
public interface IToastComponent
{
    /// <summary>
    /// Gets or sets the intent of the notification. See <see cref="ToastIntent"/>
    /// </summary>
    [Parameter]
    public ToastIntent Intent { get; set; }

    /// <summary>
    /// Gets or sets the main message of the notification.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public ToastEndContentType EndContentType { get; set; }

    /// <summary>
    /// Notification instance specific settings
    /// </summary>
    [Parameter]
    public ToastSettings Settings { get; set; }

}
