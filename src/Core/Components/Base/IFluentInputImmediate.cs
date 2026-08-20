// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Defines a component that can update its text value after a configurable input delay.
/// </summary>
public interface IFluentInputImmediate
{
    /// <summary>
    /// Gets or sets whether the value changes while the user writes text.
    /// </summary>
    bool Immediate { get; set; }

    /// <summary>
    /// Gets or sets the delay, in milliseconds, before raising the change event.
    /// </summary>
    int ImmediateDelay { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the field gains focus.
    /// </summary>
    EventCallback<FocusEventArgs> OnFocusIn { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the field loses focus.
    /// </summary>
    EventCallback<FocusEventArgs> OnFocusOut { get; set; }
}