// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// An actionable button that can be used in a message bar.
/// </summary>
/// <typeparam name="T">The type of the parameter for the button's click action.</typeparam>
public class ActionButton<T>
{
    /// <summary>
    /// Gets or sets the text to show for the button.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the function to call when the button is clicked.
    /// </summary>
    public Func<T, Task>? OnClick { get; set; }
}
