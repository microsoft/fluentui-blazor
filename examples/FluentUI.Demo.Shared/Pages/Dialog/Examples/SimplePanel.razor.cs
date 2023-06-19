using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;

namespace FluentUI.Demo.Shared.Pages.Dialog.Examples;

public partial class SimplePanel : FluentPanel
{

    [Parameter]
    public string Name { get; set; } = "";

    public override async Task CloseAsync(DialogResult result)
    {
        //await Task.CompletedTask;   // To avoid a warning
        if (result.Cancelled)
        {
            result.Data = "Cancel";
        }
        else
        {
            result.Data = $"OK {Name}";
        }

        await Dialog.CloseAsync(result);
    }


}