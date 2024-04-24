namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Step Status
/// </summary>
[Flags]
public enum WizardStepStatus
{
    None = 0,
    Previous = 1,
    Current = 2,
    Next = 4,
    All = Previous | Current | Next
}
