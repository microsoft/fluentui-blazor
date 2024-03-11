// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace Microsoft.FluentUI.AspNetCore.Components;
public class FluentEditForm : EditForm
{
    [CascadingParameter]
    public FluentWizardStep? WizardStep { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        base.BuildRenderTree(builder);

        if (WizardStep is not null && EditContext is not null)
        {
            WizardStep.RegisterEditFormAndContext(this, EditContext);
        }
    }
}
