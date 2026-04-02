// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents an individual step within a <see cref="FluentWizard"/> component.
/// </summary>
public partial class FluentWizardStep : FluentComponentBase
{
    private readonly Dictionary<EditForm, EditContext> _editForms = [];
    private readonly List<EditContext> _editContexts = [];

    /// <summary />
    public FluentWizardStep(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder.Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("position", "relative")
        .AddStyle("display", "flex")
        .AddStyle("gap", "10px", when: FluentWizard.StepperPosition == StepperPosition.Left)
        .AddStyle("flex-direction", "column", when: FluentWizard.StepperPosition == StepperPosition.Top)
        .AddStyle("align-items", "center", when: FluentWizard.StepperPosition == StepperPosition.Top)
        .AddStyle("flex", "1", when: FluentWizard.StepperPosition == StepperPosition.Top)
        .AddStyle("text-align", "center", when: FluentWizard.StepperPosition == StepperPosition.Top)
        .AddStyle("max-width", FluentWizard.StepperBulletSpace ?? "100%", when: FluentWizard.StepperPosition == StepperPosition.Top)
        .AddStyle("height", IsLastStep ? "auto" : (FluentWizard.StepperBulletSpace ?? "100%"), when: FluentWizard.StepperPosition == StepperPosition.Left)
        .AddStyle("cursor", "pointer", when: IsStepClickable)
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
    public int Index { get; private set; }

    /// <summary>
    /// Gets or sets whether the step is disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Render the Wizard Step content only when the Step is selected.
    /// </summary>
    [Parameter]
    public bool DeferredLoading { get; set; }

    /// <summary>
    /// Gets or sets the label of the step.
    /// </summary>
    [Parameter]
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Display a number on the step icon.
    /// By default, this is the <see cref="FluentWizard.DisplayStepNumber"/> value.
    /// </summary>
    [Parameter]
    public bool? DisplayStepNumber { get; set; }

    /// <summary>
    /// The OnChange event fires before the current step has changed.
    /// The EventArgs contains a field of the targeted new step and a field to cancel the built-in action.
    /// </summary>
    [Parameter]
    public EventCallback<FluentWizardStepChangeEventArgs> OnChange { get; set; }

    /// <summary>
    /// Reference to the parent <see cref="FluentWizard"/> component.
    /// For internal use only.
    /// </summary>
    [CascadingParameter]
    internal FluentWizard FluentWizard { get; set; } = default!;

    /// <summary>
    /// Gets or sets the summary of the step, to display near the label.
    /// </summary>
    [Parameter]
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the icon to display for the past/previous step.
    /// By default, it is a checkmark circle.
    /// </summary>
    [Parameter]
    public Icon IconPrevious { get; set; } = new CoreIcons.Filled.Size20.CheckmarkCircle();

    /// <summary>
    /// Gets or sets the icon to display for the current/active step.
    /// By default, it is a filled circle.
    /// </summary>
    [Parameter]
    public Icon IconCurrent { get; set; } = new CoreIcons.Filled.Size20.Circle();

    /// <summary>
    /// Gets or sets the icon to display for the future/next step.
    /// By default, it is a regular circle.
    /// </summary>
    [Parameter]
    public Icon IconNext { get; set; } = new CoreIcons.Regular.Size20.Circle();

    internal WizardStepStatus Status { get; set; } = WizardStepStatus.Next;

    private bool IsLastStep => Index >= FluentWizard.StepCount - 1;

    private string IconStyle => "width: var(--fluent-wizard-circle-size);" +
                                (Disabled ? " fill-opacity: 0.4;" : string.Empty);

    private Icon StepIcon
    {
        get
        {
            return Status switch
            {
                WizardStepStatus.Previous => IconPrevious,
                WizardStepStatus.Current => IconCurrent,
                WizardStepStatus.Next => IconNext,
                _ => new CoreIcons.Regular.Size20.Circle(),
            };
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

    /// <summary />
    public override async ValueTask DisposeAsync()
    {
        FluentWizard?.RemoveStep(this);
        await base.DisposeAsync();
    }

    /// <summary>
    /// Registers an EditForm and its EditContext for validation tracking.
    /// </summary>
    public void RegisterEditFormAndContext(EditForm editForm, EditContext editContext)
    {
        _editForms.TryAdd(editForm, editContext);
    }

    /// <summary>
    /// Clears all registered EditForm and EditContext pairs.
    /// </summary>
    public void ClearEditFormAndContext()
    {
        _editForms.Clear();
    }

    /// <summary>
    /// Registers an <see cref="EditContext"/> for validation tracking.
    /// This is typically called by the <see cref="FluentWizardStepValidator"/> component.
    /// </summary>
    public void RegisterEditContext(EditContext editContext)
    {
        if (!_editContexts.Contains(editContext))
        {
            _editContexts.Add(editContext);
        }
    }

    /// <summary>
    /// Unregisters an <see cref="EditContext"/> from validation tracking.
    /// </summary>
    public void UnregisterEditContext(EditContext editContext)
    {
        _editContexts.Remove(editContext);
    }

    /// <summary>
    /// Validates all registered EditContexts.
    /// </summary>
    public bool ValidateEditContexts()
    {
        var isValid = true;
        foreach (var editForm in _editForms)
        {
            var contextIsValid = editForm.Value.Validate();
            if (!contextIsValid)
            {
                isValid = false;
            }
        }

        foreach (var editContext in _editContexts)
        {
            var contextIsValid = editContext.Validate();
            if (!contextIsValid)
            {
                isValid = false;
            }
        }

        return isValid;
    }

    internal async Task InvokeOnValidSubmitForEditFormsAsync()
    {
        foreach (var editForm in _editForms)
        {
            await editForm.Key.OnValidSubmit.InvokeAsync(editForm.Value);
        }
    }

    internal async Task InvokeOnInValidSubmitForEditFormsAsync()
    {
        foreach (var editForm in _editForms)
        {
            await editForm.Key.OnInvalidSubmit.InvokeAsync(editForm.Value);
        }
    }

    internal async Task InvokeOnSubmitForEditFormsAsync()
    {
        foreach (var editForm in _editForms)
        {
            await editForm.Key.OnSubmit.InvokeAsync(editForm.Value);
        }
    }

    private async Task OnClickHandlerAsync()
    {
        if (!IsStepClickable)
        {
            return;
        }

        await FluentWizard.ValidateAndGoToStepAsync(Index, validateEditContexts: Index > FluentWizard.Value);
    }

    private bool IsStepClickable
    {
        get
        {
            if (Disabled)
            {
                return false;
            }

            if (FluentWizard.Value == Index)
            {
                return false;
            }

            if (FluentWizard.StepSequence == WizardStepSequence.Linear)
            {
                return false;
            }

            if (FluentWizard.StepSequence == WizardStepSequence.Visited &&
                Index > FluentWizard._maxStepVisited)
            {
                return false;
            }

            return true;
        }
    }
}
