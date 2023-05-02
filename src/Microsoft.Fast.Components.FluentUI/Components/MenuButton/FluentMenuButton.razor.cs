using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI
{
    public partial class FluentMenuButton : FluentComponentBase
    {


        private bool _toggle;
        private readonly string _idButton = Identifier.NewId();
        private readonly string _idMenu = Identifier.NewId();

        [Parameter]
        public FluentButton? Button { get; set; }

        [Parameter]
        public FluentMenu? Menu { get; set; }

        [Parameter]
        public string? Text { get; set; }


        [Parameter]
        public Dictionary<string, string> Items { get; set; } = new();

        [Parameter]

        public EventCallback<MenuChangeEventArgs> OnMenuChanged { get; set; }

        private void ToggleMenu()
        {
            _toggle = !_toggle;
        }

        private async Task OnMenuChange(MenuChangeEventArgs args)
        {
            if (args is not null && args.Id is not null)
            {
                await OnMenuChanged.InvokeAsync(args);

                _toggle = !_toggle;
            }
        }

        private void OnKeyDown(KeyboardEventArgs args)
        {
            if (args is not null && args.Key == "Escape")
            {
                _toggle = false;
            }
        }

    }
}