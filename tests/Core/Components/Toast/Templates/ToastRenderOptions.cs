// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Toast.Templates;

public class ToastRenderOptions
{
    public bool AutoClose { get; set; }

    public int AutoCloseDelay { get; set; }

    public ToastResult AutoCloseResult { get; set; } = ToastResult.Ok(true);

    public int OnInitializedCount { get; set; }
}
