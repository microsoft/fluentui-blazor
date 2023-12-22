using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentWizardStep : FluentComponentBase
{
    /// <summary />
    protected string? ClassValue => new CssBuilder(Class).Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style).Build();

    /// <summary>
    /// Gets or sets the content of the step.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets the step index.
    /// </summary>
    public int Index { get; private set; } = 0;

    /// <summary>
    /// Gets or sets whether the step is disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; } = false;

    /// <summary>
    /// Gets or sets the label of the step.
    /// </summary>
    [Parameter]
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// The OnChange event fires before the current step has changed.
    /// The EventArgs contains a field of the targeted new step and a field to cancel the build-in action.
    /// </summary>
    [Parameter]
    public EventCallback<FluentWizardStepChangeEventArgs> OnChange { get; set; }

    /// <summary />
    [CascadingParameter]
    public FluentWizard FluentWizard { get; set; } = default!;

    /// <summary>
    /// Gets or sets the summary of the step, to diplay near the label.
    /// </summary>
    [Parameter]
    public string Summary { get; set; } = string.Empty;

    internal WizardStepStatus Status { get; set; } = WizardStepStatus.Next;

    /// <summary />
    private string StepStyle
    {
        get
        {
            string spaceSize = FluentWizard.StepperBulletSpace ?? "100%";
            switch (FluentWizard.StepperPosition)
            {
                case StepperPosition.Top:
                    return $"max-width: {spaceSize};";

                case StepperPosition.Left:
                    return $"height: {spaceSize};";

                default:
                    return string.Empty;
            }
        }
    }

    private Icon StepIcon
    {
        get
        {
            switch (Status)
            {
                case WizardStepStatus.Previous:
                    return new CoreIcons.Filled.Size24.CheckmarkCircle();

                case WizardStepStatus.Current:
                    return new CoreIcons.Filled.Size24.Circle();

                case WizardStepStatus.Next:
                    return new CoreIcons.Regular.Size24.Circle();

                default:
                    return new CoreIcons.Regular.Size24.Circle();
            }
        }
    }

    /// <summary />
    protected override void OnInitialized()
    {
        if (FluentWizard == null)
        {
            throw new ArgumentException("The FluentWizardStep must be included in the FluentWizard component.");
        }

        Index = FluentWizard.AddStep(this);
        base.OnInitialized();
    }
}
