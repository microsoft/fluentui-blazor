// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

#pragma warning disable CS1591

public partial class FluentProgressToastContent
{
    [Parameter, EditorRequired]
    public FluentProgressToast Owner { get; set; } = default!;

    private Task OnActionClickedAsync(bool primary)
    {
        return Owner.OnActionClickedAsync(primary);
    }

#pragma warning restore CS1591
}
