namespace Microsoft.FluentUI.AspNetCore.Components;

public class FluentWizardStepChangeEventArgs
{
    /// <summary />
    internal FluentWizardStepChangeEventArgs(int targetIndex, string targetLabel)
    {
        TargetIndex = targetIndex;
        TargetLabel = targetLabel;
    }

    /// <summary />
    public int TargetIndex { get; }

    /// <summary />
    public string TargetLabel { get; }

    /// <summary />
    public bool IsCancelled { get; set; } = false;
}
