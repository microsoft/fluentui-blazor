// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Event arguments for the <see cref="FluentWizardStep.OnChange"/> event.
/// </summary>
public class FluentWizardStepChangeEventArgs
{
    /// <summary />
    internal FluentWizardStepChangeEventArgs(int targetIndex, string targetLabel)
    {
        TargetIndex = targetIndex;
        TargetLabel = targetLabel;
    }

    /// <summary>
    /// Gets the index of the target step.
    /// </summary>
    public int TargetIndex { get; }

    /// <summary>
    /// Gets the label of the target step.
    /// </summary>
    public string TargetLabel { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the step change should be cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }
}
