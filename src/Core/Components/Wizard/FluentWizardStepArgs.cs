namespace Microsoft.FluentUI.AspNetCore.Components;

public class FluentWizardStepArgs
{
    internal FluentWizardStepArgs(int index, int active)
    {
        Index = index;
        Active = index == active;
    }

    public int Index { get; }

    public bool Active { get; }
}
