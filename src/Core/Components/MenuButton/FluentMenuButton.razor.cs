using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentMenuButton : FluentComponentBase
{
    private bool _visible;

    [Parameter]
    public FluentButton? Button { get; set; }

    [Parameter]
    public FluentMenu? Menu { get; set; }

    [Parameter]
    public string? Text { get; set; }

    [Parameter]
    public string? ButtonStyle { get; set; }

    [Parameter]
    public string? MenuStyle { get; set; }

    [Parameter]
    public Dictionary<string, string> Items { get; set; } = [];

    [Parameter]

    public EventCallback<MenuChangeEventArgs> OnMenuChanged { get; set; }

    private void ToggleMenu()
    {
        _visible = !_visible;
    }

    private async Task OnMenuChangeAsync(MenuChangeEventArgs args)
    {
        if (args is not null && args.Id is not null)
        {
            await OnMenuChanged.InvokeAsync(args);
        }
        _visible = false;
    }

    private void OnKeyDown(KeyboardEventArgs args)
    {
        if (args is not null && args.Key == "Escape")
        {
            _visible = false;
        }
    }
}
