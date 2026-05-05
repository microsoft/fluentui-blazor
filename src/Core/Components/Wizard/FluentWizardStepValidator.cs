// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A component that automatically registers an <see cref="EditContext"/> with a parent <see cref="FluentWizardStep"/>
/// for validation when navigating between wizard steps.
/// Place this component inside an <see cref="EditForm"/> within a <see cref="FluentWizardStep"/>.
/// </summary>
public class FluentWizardStepValidator : ComponentBase, IDisposable
{
    [CascadingParameter]
    private FluentWizardStep? WizardStep { get; set; }

    [CascadingParameter]
    private EditContext? EditContext { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (WizardStep is null)
        {
            throw new InvalidOperationException(
                $"{nameof(FluentWizardStepValidator)} must be used inside a {nameof(FluentWizardStep)}.");
        }

        if (EditContext is null)
        {
            throw new InvalidOperationException(
                $"{nameof(FluentWizardStepValidator)} must be used inside an {nameof(EditForm)}.");
        }

        WizardStep.RegisterEditContext(EditContext);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (WizardStep is not null && EditContext is not null)
        {
            WizardStep.UnregisterEditContext(EditContext);
        }
    }
}
