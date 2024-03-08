// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Component used to register the EditContext with the WizardStep
/// </summary>
public class FluentWizardStepFormValidator : ComponentBase
{
    /// <summary>
    /// Wizard Step to register the <see cref="EditContext"/> with.
    /// </summary>
    [CascadingParameter]
    public FluentWizardStep? WizardStep { get; set; }

    /// <summary>
    /// EditContext that should be registered to the <see cref="FluentWizardStep"/>
    /// </summary>
    [CascadingParameter]
    public EditContext? EditContext { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (WizardStep is not null && EditContext is not null)
        {
            WizardStep.RegisterEditContext(EditContext);
        }
    }
}
