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
    internal int _maxStepVisited = 0;

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

            _maxStepVisited = Math.Max(_value, _maxStepVisited);

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
    /// The OnChange event can be triggered using the <see cref="GoToStepAsync(int, bool)"/> method from your code.
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

    /// <summary>
    /// Gets or sets the way to navigate in the Wizard Steps.
    /// Default is <see cref="WizardStepSequence.Linear"/>.
    /// </summary>
    [Parameter]
    public WizardStepSequence StepSequence { get; set; } = WizardStepSequence.Linear;

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
        var stepChangeArgs = await OnStepChangeHandlerAsync(targetIndex, true);
        var isCanceled = stepChangeArgs?.IsCancelled ?? false;

        if (!isCanceled)
        {
            Value = targetIndex;
            await ValueChanged.InvokeAsync(targetIndex);
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
        var stepChangeArgs = await OnStepChangeHandlerAsync(targetIndex, false);
        var isCanceled = stepChangeArgs?.IsCancelled ?? false;

        if (!isCanceled)
        {
            Value = targetIndex;
            await ValueChanged.InvokeAsync(targetIndex);
            StateHasChanged();
        }
    }

    /// <summary />
    protected virtual async Task<FluentWizardStepChangeEventArgs> OnStepChangeHandlerAsync(int targetIndex, bool validateEditContexts)
    {
        var stepChangeArgs = new FluentWizardStepChangeEventArgs(targetIndex, _steps[targetIndex].Label);

        if (validateEditContexts)
        {
            var allEditContextsAreValid = _steps[Value].ValidateEditContexts();
            stepChangeArgs.IsCancelled = !allEditContextsAreValid;

            if (!allEditContextsAreValid)
            {
                await _steps[Value].InvokeOnInValidSubmitForEditFormsAsync();
            }
            if (!stepChangeArgs.IsCancelled && allEditContextsAreValid)
            {
                // Invoke the 'OnValidSubmit' handlers for the Edit Forms
                await _steps[Value].InvokeOnValidSubmitForEditFormsAsync();
            }

            await _steps[Value].InvokeOnSubmitForEditFormsAsync();
        }

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
    protected virtual async Task OnFinishHandlerAsync(MouseEventArgs e)
    {
        // Validate any form edit contexts
        var allEditContextsAreValid = _steps[Value].ValidateEditContexts();
        if (!allEditContextsAreValid)
        {
            // Invoke the 'OnInvalidSubmit' handlers for the edit forms.
            await _steps[Value].InvokeOnInValidSubmitForEditFormsAsync();
            return;
        }

        // Invoke the 'OnValidSubmit' handlers for the edit forms.
        await _steps[Value].InvokeOnValidSubmitForEditFormsAsync();
        await _steps[Value].InvokeOnSubmitForEditFormsAsync();

        _steps[Value].Status = WizardStepStatus.Previous;

        if (OnFinish.HasDelegate)
        {
            await OnFinish.InvokeAsync();
        }
    }

    /// <summary>
    /// Navigate to the specified step, with or without validate the current EditContexts.
    /// </summary>
    /// <param name="step">Index number of the step to display</param>
    /// <param name="validateEditContexts">Validate the EditContext. Default is false.</param>
    /// <returns></returns>
    public Task GoToStepAsync(int step, bool validateEditContexts = false)
    {
        Value = step;
        return ValidateAndGoToStepAsync(step, validateEditContexts);
    }

    internal async Task ValidateAndGoToStepAsync(int targetIndex, bool validateEditContexts)
    {
        var stepChangeArgs = await OnStepChangeHandlerAsync(targetIndex, validateEditContexts);
        var isCanceled = stepChangeArgs?.IsCancelled ?? false;

        if (!isCanceled)
        {
            Value = targetIndex;
            await ValueChanged.InvokeAsync(targetIndex);
            StateHasChanged();
        }
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

    private bool DisplayPreviousButton => Value > 0 && _steps[..Value].Any(i => !i.Disabled);

    private bool DisplayNextButton => Value < _steps.Count - 1 && _steps[(Value + 1)..].Any(i => !i.Disabled);
}
