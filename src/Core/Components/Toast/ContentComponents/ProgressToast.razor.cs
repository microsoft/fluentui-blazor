// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

[method: DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(ProgressToast))]
public partial class ProgressToast() : IToastContentComponent<ProgressToastContent>
{
    [Parameter]
    public ProgressToastContent Content { get; set; } = default!;

    [CascadingParameter]
    private FluentToast Toast { get; set; } = default!;

    /// <summary>
    /// Closes the toast
    /// </summary>
    public void Close()
        => Toast.Close();

    public void HandlePrimaryActionClick()
    {
        //Content.PrimaryAction?.OnClick?.Invoke();
        Close();
    }

    public void HandleSecondaryActionClick()
    {
        //Content.SecondaryAction?.OnClick?.Invoke();
        Close();
    }
}
