using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentWizard : FluentComponentBase
{
    public static string LabelButtonPrevious = "Previous";
    public static string LabelButtonNext = "Next";
    public static string LabelButtonDone = "Done";

    private readonly List<FluentWizardStep> _steps = new();
    private int _value = 0;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-wizard")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("width", Width)
        .AddStyle("height", Height)
        .Build();

    /// <summary>
    /// Gets or sets the height of the wizard.
    /// </summary>
    [Parameter]
    public string Height { get; set; } = "400px";

    /// <summary>
    /// Gets or sets the width of the wizard.
    /// </summary>
    [Parameter]
    public string Width { get; set; } = "100%";

    /// <summary>
    /// Triggers when the done button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnFinish { get; set; }

    /// <summary>
    /// Gets or sets the stepper position in the wizard (Top or Left).
    /// </summary>
    [Parameter]
    public StepperPosition StepperPosition { get; set; } = StepperPosition.Left;

    /// <summary>
    /// Gets or sets the stepper width (if position is Left)
    /// or the stepper height (if position is Top).
    /// </summary>
    [Parameter]
    public string? StepperSize { get; set; }

    /// <summary>
    /// Gets or sets the space between two bullets (ex. 120px).
    /// </summary>
    [Parameter]
    public string? StepperBulletSpace { get; set; }

    /// <summary>
    /// Display a border of the Wizard.
    /// </summary>
    [Parameter]
    public WizardBorder Border { get; set; } = WizardBorder.None;

    /// <summary>
    /// Display a number on each step icon. Can be overridden by the step <see cref="FluentWizardStep.DisplayStepNumber"/> property.
    /// </summary>
    [Parameter]
    public WizardStepStatus DisplayStepNumber { get; set; } = WizardStepStatus.None;

    /// <summary>
    /// Gets or sets the step index of the current step.
    /// This value is bindable.
    /// </summary>
    [Parameter]
    public int Value
    {
        get
        {
            return _value;
        }

        set
        {
            if (value < 0 || _steps.Count <= 0)
            {
                _value = 0;
            }
            else if (value > _steps.Count - 1)
            {
                _value = _steps.Count - 1;
            }
            else
            {
                _value = value;
            }

            SetCurrentStatusToStep(_value);
        }
    }

    /// <summary>
    /// Triggers when the value has changed.
    /// </summary>
    [Parameter]
    public EventCallback<int> ValueChanged { get; set; }

    /// <summary>
    /// Gets or sets the buttons section of the wizard.
    /// This configuration overrides the whole rendering of the bottom-right section of the Wizard,
    /// including the built-in buttons and thus provides a full control over it.
    /// Custom Wizard buttons do not trigger the component OnChange and OnFinish events.
    /// </summary>
    [Parameter]
    public RenderFragment<int>? ButtonTemplate { get; set; }

    /// <summary>
    /// Gets or sets the wizard steps. Add WizardStep tags inside this tag.
    /// </summary>
    [Parameter]
    public RenderFragment? Steps { get; set; }

    /// <summary>
    /// Hide step titles and summaries on specified sizes (you can combine several values: GridItemHidden.Sm | GridItemHidden.Xl).
    /// The default value is <see cref="GridItemHidden.XsAndDown"/> to adapt to mobile devices.
    /// </summary>
    [Parameter]
    public GridItemHidden? StepTitleHiddenWhen { get; set; } = GridItemHidden.XsAndDown;

    /// <summary />
    protected virtual async Task OnNextHandlerAsync(MouseEventArgs e)
    {
        // Target step index
        var targetIndex = Value;
        do
        {
            targetIndex++;
        }
        while (_steps[targetIndex].Disabled && targetIndex < _steps.Count - 1);

        // StepChange event
        var stepChangeArgs = await OnStepChangeHandlerAsync(targetIndex);
        var isCanceled = stepChangeArgs?.IsCancelled ?? false;

        if (!isCanceled)
        {
            Value = targetIndex;
            StateHasChanged();
        }
    }

    /// <summary />
    protected virtual async Task OnPreviousHandlerAsync(MouseEventArgs e)
    {
        // Target step index
        var targetIndex = Value;
        do
        {
            targetIndex--;
        }
        while (_steps[targetIndex].Disabled && targetIndex > 0);

        // StepChange event
        var stepChangeArgs = await OnStepChangeHandlerAsync(targetIndex);
        var isCanceled = stepChangeArgs?.IsCancelled ?? false;

        if (!isCanceled)
        {
            Value = targetIndex;
            StateHasChanged();
        }
    }

    /// <summary />
    protected virtual async Task<FluentWizardStepChangeEventArgs> OnStepChangeHandlerAsync(int targetIndex)
    {
        var stepChangeArgs = new FluentWizardStepChangeEventArgs(targetIndex, _steps[targetIndex].Label);
        return await OnStepChangeHandlerAsync(stepChangeArgs);
    }

    /// <summary />
    protected virtual async Task<FluentWizardStepChangeEventArgs> OnStepChangeHandlerAsync(FluentWizardStepChangeEventArgs args)
    {
        if (_steps[Value].OnChange.HasDelegate)
        {
            await _steps[Value].OnChange.InvokeAsync(args);
        }

        return args;
    }

    /// <summary />
    protected virtual Task OnFinishHandlerAsync(MouseEventArgs e)
    {
        _steps[Value].Status = WizardStepStatus.Previous;

        if (OnFinish.HasDelegate)
        {
            return OnFinish.InvokeAsync();
        }

        return Task.CompletedTask;
    }

    internal int AddStep(FluentWizardStep step)
    {
        _steps.Add(step);
        var index = _steps.Count - 1;

        if (index == Value)
        {
            SetCurrentStatusToStep(index);
        }

        StateHasChanged();

        return index;
    }

    private void SetCurrentStatusToStep(int stepIndex)
    {
        for (var i = 0; i < _steps.Count; i++)
        {
            // Step disabled
            if (_steps[i].Disabled)
            {
                _steps[i].Status = WizardStepStatus.Next;
            }

            // Step enabled
            else
            {
                if (i < stepIndex)
                {
                    _steps[i].Status = WizardStepStatus.Previous;
                }
                else if (i == stepIndex)
                {
                    _steps[i].Status = WizardStepStatus.Current;
                }
                else if (i > stepIndex)
                {
                    _steps[i].Status = WizardStepStatus.Next;
                }
                else
                {
                    _steps[i].Status = WizardStepStatus.Next;
                }
            }
        }
    }

    private string? GetStepperWidthOrHeight()
    {
        if (string.IsNullOrEmpty(StepperSize))
        {
            return null;
        }

        switch (StepperPosition)
        {
            case StepperPosition.Top:
                return $"height: {StepperSize}";

            case StepperPosition.Left:
                return $"width: {StepperSize}";
        }

        return null;
    }
}
