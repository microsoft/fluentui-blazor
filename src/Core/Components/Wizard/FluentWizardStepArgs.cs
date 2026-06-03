// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Arguments passed to the <see cref="FluentWizardStep.StepTemplate"/> render fragment.
/// </summary>
public class FluentWizardStepArgs
{
    internal FluentWizardStepArgs(int index, int active)
    {
        Index = index;
        Active = index == active;
    }

    /// <summary>
    /// Gets the index of the step.
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// Gets a value indicating whether the step is the currently active step.
    /// </summary>
    public bool Active { get; }
}
