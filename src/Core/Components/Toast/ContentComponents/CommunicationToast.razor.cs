// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

[method: DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(CommunicationToast))]
public partial class CommunicationToast() : IToastContentComponent<CommunicationToastContent>
{

    [Parameter]
    public CommunicationToastContent Content { get; set; } = default!;

    [CascadingParameter]
    private FluentToast Toast { get; set; } = default!;

    /// <summary>
    /// Closes the toast
    /// </summary>
    public void Close()
        => Toast.Close();

    public void HandlePrimaryActionClick()
    {
        Close();
    }

    public void HandleSecondaryActionClick()
    {
        Close();
    }
}
