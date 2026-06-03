// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Defines the border display options for the <see cref="FluentWizard"/> component.
/// </summary>
[Flags]
public enum WizardBorder
{
    /// <summary>
    /// No border.
    /// </summary>
    None = 0,

    /// <summary>
    /// Border inside (between sections).
    /// </summary>
    Inside = 1,

    /// <summary>
    /// Border outside (around the wizard).
    /// </summary>
    Outside = 2,

    /// <summary>
    /// Both inside and outside borders.
    /// </summary>
    All = Inside | Outside,
}
