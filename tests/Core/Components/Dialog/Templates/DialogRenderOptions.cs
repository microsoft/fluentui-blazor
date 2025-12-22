// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Dialog.Templates;

public class DialogRenderOptions
{
    public bool AutoClose { get; set; }

    public int AutoCloseDelay { get; set; }

    public DialogResult AutoCloseResult { get; set; } = DialogResult.Ok(true);

    public int OnInitializedCount { get; set; }

    public int OnParametersSetCount { get; set; }
}
