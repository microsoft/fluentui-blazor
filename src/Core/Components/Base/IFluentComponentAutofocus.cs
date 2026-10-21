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
    ///  Determines if the element should receive the focus automatically
    /// when the component is rendered.
    /// </summary>
    bool Autofocus { get; set; }

    /// <summary>
    /// Sets the focus to the element, optionally preventing scrolling.
    /// </summary>
    /// <param name="preventScroll">If true, prevents the page from scrolling to the element.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task FocusAsync(bool preventScroll = false);
}
