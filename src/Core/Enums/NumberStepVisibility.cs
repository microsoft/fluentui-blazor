// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Defines the visibility behavior of the step buttons in the <see cref="FluentNumber{TValue}" /> component.
/// </summary>
public enum NumberStepVisibility
{
    /// <summary>
    /// The step buttons are always visible.
    /// </summary>
    Visible,

    /// <summary>
    /// The step buttons are always hidden.
    /// </summary>
    Hidden,

    /// <summary>
    /// The step buttons are shown only when the user hovers over the input or when the input has focus.
    /// </summary>
    Auto,
}
