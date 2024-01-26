using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentWizardStep : FluentComponentBase
{
    /// <summary />
    protected string? ClassValue => new CssBuilder(Class).Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("max-width", FluentWizard.StepperBulletSpace ?? "100%", when: FluentWizard.StepperPosition == StepperPosition.Top)
        .AddStyle("height", FluentWizard.StepperBulletSpace ?? "100%", when: FluentWizard.StepperPosition == StepperPosition.Left)
        .Build();

    /// <summary>
    /// Gets or sets the content of the step.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the template of the step icon.
    /// </summary>
    [Parameter]
    public RenderFragment<FluentWizardStepArgs>? StepTemplate { get; set; }

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
    /// Render the Wizard Step content only when the Step is selected.
    /// </summary>
    [Parameter]
    public bool DeferredLoading { get; set; } = false;

    /// <summary>
    /// Gets or sets the label of the step.
    /// </summary>
    [Parameter]
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Display a number the step icon.
    /// By default, this is the <see cref="FluentWizard.DisplayStepNumber"/> value.
    /// </summary>
    [Parameter]
    public bool? DisplayStepNumber { get; set; }

    /// <summary>
    /// The OnChange event fires before the current step has changed.
    /// The EventArgs contains a field of the targeted new step and a field to cancel the build-in action.
    /// </summary>
    [Parameter]
    public EventCallback<FluentWizardStepChangeEventArgs> OnChange { get; set; }

    /// <summary>
    /// Reference to the parent <see cref="FluentWizard"/> component.
    /// For internal use only
    /// </summary>
    [CascadingParameter]
    public FluentWizard FluentWizard { get; set; } = default!;

    /// <summary>
    /// Gets or sets the summary of the step, to diplay near the label.
    /// </summary>
    [Parameter]
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the icon to display for the past/previous step.
    /// By default, it is a checkmark circle.
    /// </summary>
    [Parameter]
    public Icon IconPrevious { get; set; } = new CoreIcons.Filled.Size24.CheckmarkCircle();

    /// <summary>
    /// Gets or sets the icon to display for the current/active step.
    /// By default, it is a checkmark circle.
    /// </summary>
    [Parameter]
    public Icon IconCurrent { get; set; } = new CoreIcons.Filled.Size24.Circle();

    /// <summary>
    /// Gets or sets the icon to display for the future/next step.
    /// By default, it is a checkmark circle.
    /// </summary>
    [Parameter]
    public Icon IconNext { get; set; } = new CoreIcons.Regular.Size24.Circle();

    internal WizardStepStatus Status { get; set; } = WizardStepStatus.Next;

    private string IconStyle => "width: var(--fluent-wizard-circle-size);" +
                                (Disabled ? " fill-opacity: var(--disabled-opacity);" : string.Empty);

    private Icon StepIcon
    {
        get
        {
            switch (Status)
            {
                case WizardStepStatus.Previous:
                    return IconPrevious;

                case WizardStepStatus.Current:
                    return IconCurrent;

                case WizardStepStatus.Next:
                    return IconNext;

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
