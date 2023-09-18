using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;

namespace FluentUI.Demo.Shared.Pages.Dialog.Examples;
public partial class CustomHeader : FluentComponentBase
{
    [CascadingParameter]
    private FluentDialog? Dialog { get; set; }

    /// <summary>
    /// Title of the dialog
    /// </summary>
    [Parameter]
    public string Title { get; set; } = string.Empty;

    protected override void OnParametersSet()
    {
        if (Dialog is null)
        {
            throw new ArgumentNullException(nameof(Dialog), "CustomHeader must be used inside FluentDialog");
        }
    }
}