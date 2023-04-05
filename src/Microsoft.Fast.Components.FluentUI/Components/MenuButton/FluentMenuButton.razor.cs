using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI
{
    public partial class FluentMenuButton : FluentComponentBase, IAsyncDisposable
    {
        
        
        bool? toggle = null;
        string menuClass = "menu";
        string idButton = Identifier.NewId();
        string idMenu = Identifier.NewId();
        IJSObjectReference? jsModule;

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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // Remeber to replace the path to the colocated JS file with your own project's path
                // or Razor Class Library's path.
                jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Microsoft.Fast.Components.FluentUI/Components/MenuButton/FluentMenuButton.razor.js");
                await jsModule.InvokeAsync<object>("clickOutsideHandler", idButton, idMenu, DotNetObjectReference.Create(this));
            }
        }

        private void ToggleMenu()
        {
            if (toggle is null || toggle == false)
                ShowMenu();
            else
                HideMenu();
        }

        public void ShowMenu()
        {
            toggle = true;
            menuClass = "menu visible";
            Menu!.Element.FocusAsync();
            StateHasChanged();
        }

        [JSInvokable]
        public void HideMenu()
        {
            toggle = null;
            Button!.Element.FocusAsync();
            menuClass = "menu";
            StateHasChanged();
        }

        private async Task OnMenuChange(MenuChangeEventArgs args)
        {
            if (args is not null && args.Id is not null)
            {
                await OnMenuChanged.InvokeAsync(args);
                
                ToggleMenu();
            }
        }

        private void OnKeyDown(KeyboardEventArgs args)
        {
            if (args is not null && args.Key == "Escape")
            {
                ToggleMenu();
            }
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (jsModule is not null)
                {
                    await jsModule.DisposeAsync();
                }
            }
            catch (JSDisconnectedException)
            {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
            }
        }
    }
}