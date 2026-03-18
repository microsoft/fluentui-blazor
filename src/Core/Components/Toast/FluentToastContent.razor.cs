// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

#pragma warning disable CS1591

public partial class FluentToastContent
{
    [Parameter, EditorRequired]
    public FluentToast Owner { get; set; } = default!;

    private Task OnActionClickedAsync(bool primary)
    {
        return Owner.OnActionClickedAsync(primary);
    }

#pragma warning restore CS1591
}
