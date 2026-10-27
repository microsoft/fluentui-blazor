// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a component that supports automatic focus when rendered.
/// </summary>
public interface IFluentComponentAutofocus
{
    /// <summary>
    /// Determines if the element should receive the focus automatically
    /// when the component is rendered.
    /// </summary>
    bool Autofocus { get; set; }

    /// <summary>
    /// Sets the focus to the element, optionally preventing scrolling.
    /// The component's ID must be set before calling this method.
    /// </summary>
    /// <param name="preventScroll">If true, prevents the page from scrolling to the element.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">The component's ID is null. The exception message is "Component ID cannot be null.".</exception>
    Task FocusAsync(bool preventScroll = false);
}
