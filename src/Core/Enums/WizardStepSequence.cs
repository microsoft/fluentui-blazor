// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

public enum WizardStepSequence
{
    /// <summary>
    /// The user can go to the next/previous step only, using the Next/Previous button.
    /// </summary>
    Linear,

    /// <summary>
    /// The use can go to any steps (not disabled) clicking on an item.
    /// </summary>
    Any,

    /// <summary>
    /// The user can go to the next step using the Next button,
    /// or go to any previous step, already visited.
    /// </summary>
    Visited,
}
