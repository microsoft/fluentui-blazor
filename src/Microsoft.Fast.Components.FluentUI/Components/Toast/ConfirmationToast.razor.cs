using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Microsoft.Fast.Components.FluentUI;

public partial class ConfirmationToast : FluentToast, IToastComponent
{
    [CascadingParameter]
    private FluentToast Toast { get; set; } = default!;

    protected override void OnInitialized()
    {
        Id = Toast.Id;
        Settings = Toast.Settings;
    }
}