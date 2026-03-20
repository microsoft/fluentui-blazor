// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Defines the status of a wizard step.
/// </summary>
[Flags]
public enum WizardStepStatus
{
    /// <summary>
    /// No status.
    /// </summary>
    None = 0,

    /// <summary>
    /// The step has been completed.
    /// </summary>
    [Description("previous")]
    Previous = 1,

    /// <summary>
    /// The step is the current active step.
    /// </summary>
    [Description("current")]
    Current = 2,

    /// <summary>
    /// The step has not been visited yet.
    /// </summary>
    [Description("next")]
    Next = 4,

    /// <summary>
    /// All statuses.
    /// </summary>
    [Description("all")]
    All = Previous | Current | Next
}
