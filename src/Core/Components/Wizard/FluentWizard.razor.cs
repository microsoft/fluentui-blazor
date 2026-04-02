// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A wizard component that provides a step-by-step user interface.
/// </summary>
public partial class FluentWizard : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the label for the Previous button.
    /// If null or empty, the localized value is used.
    /// </summary>
    public static string? LabelButtonPrevious { get; set; }

    /// <summary>
    /// Gets or sets the label for the Next button.
    /// If null or empty, the localized value is used.
    /// </summary>
    public static string? LabelButtonNext { get; set; }

    /// <summary>
    /// Gets or sets the label for the Done button.
    /// If null or empty, the localized value is used.
    /// </summary>
    public static string? LabelButtonDone { get; set; }

    private string LabelButtonPreviousValue => string.IsNullOrWhiteSpace(LabelButtonPrevious)
        ? Localizer[Localization.LanguageResource.Wizard_LabelButtonPrevious]
        : LabelButtonPrevious;

    private string LabelButtonNextValue => string.IsNullOrWhiteSpace(LabelButtonNext)
        ? Localizer[Localization.LanguageResource.Wizard_LabelButtonNext]
        : LabelButtonNext;

    private string LabelButtonDoneValue => string.IsNullOrWhiteSpace(LabelButtonDone)
        ? Localizer[Localization.LanguageResource.Wizard_LabelButtonDone]
        : LabelButtonDone;

    private readonly List<FluentWizardStep> _steps = new();
    internal int _maxStepVisited;

    /// <summary />
    public FluentWizard(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-wizard")
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
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
    public int Value { get; set; }

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
    protected override void OnParametersSet()
    {
        SetCurrentValue(Value);
        base.OnParametersSet();
    }

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
            SetCurrentValue(targetIndex);
            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync(Value);
            }

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
            SetCurrentValue(targetIndex);
            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync(Value);
            }

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

        if (_steps[Value].DeferredLoading && !args.IsCancelled)
        {
            _steps[Value].ClearEditFormAndContext();
        }

        return args;
    }

    /// <summary />
    protected virtual Task OnFinishHandlerAsync(MouseEventArgs e)
    {
        return FinishAsync(validateEditContexts: true);
    }

    /// <summary>
    /// Optionally validate and invoke the <see cref="OnFinish"/> handler.
    /// </summary>
    /// <param name="validateEditContexts">Validate the EditContext. Default is false.</param>
    /// <returns></returns>
    public async Task FinishAsync(bool validateEditContexts = false)
    {
        if (validateEditContexts)
        {
            // Validate any form edit contexts
            var allEditContextsAreValid = _steps[Value].ValidateEditContexts();
            if (!allEditContextsAreValid)
            {
                // Invoke the 'OnInvalidSubmit' handlers for the edit forms.
                await _steps[Value].InvokeOnInValidSubmitForEditFormsAsync();
                return;
            }
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
        return ValidateAndGoToStepAsync(step, validateEditContexts);
    }

    internal async Task ValidateAndGoToStepAsync(int targetIndex, bool validateEditContexts)
    {
        var stepChangeArgs = await OnStepChangeHandlerAsync(targetIndex, validateEditContexts);
        var isCanceled = stepChangeArgs?.IsCancelled ?? false;

        if (!isCanceled)
        {
            SetCurrentValue(targetIndex);
            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync(Value);
            }

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

        try
        {
            StateHasChanged();
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("render handle is not yet assigned", StringComparison.OrdinalIgnoreCase))
        {
        }

        return index;
    }

    internal int StepCount => _steps.Count;

    internal void RemoveStep(FluentWizardStep step)
    {
        _steps.Remove(step);
    }

    private void SetCurrentValue(int value)
    {
        Value = NormalizeValue(value);
        _maxStepVisited = Math.Max(Value, _maxStepVisited);

        SetCurrentStatusToStep(Value);
    }

    private int NormalizeValue(int value)
    {
        if (value < 0 || _steps.Count <= 0)
        {
            return 0;
        }

        if (value > _steps.Count - 1)
        {
            return _steps.Count - 1;
        }

        return value;
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
