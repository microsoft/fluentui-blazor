// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// An actionable link that can be used in a message bar.
/// </summary>
/// <typeparam name="T">The type of the parameter passed to the click event.</typeparam>
public class ActionLink<T>
{
    /// <summary>
    /// Gets or sets the text to show for the link.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the address to navigate to when the link is clicked.
    /// </summary>
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets the target window or frame to open the link in.
    /// </summary>
    public LinkTarget? Target { get; set; }

    /// <summary>
    /// Gets or sets the function to call when the link is clicked.
    /// </summary>
    public Func<T, Task>? OnClick { get; set; }
}
