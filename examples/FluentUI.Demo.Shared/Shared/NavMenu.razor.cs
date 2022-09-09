using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Fast.Components.FluentUI;

namespace FluentUI.Demo.Shared.Shared
{
    public partial class NavMenu
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        private string? target;
        protected override void OnInitialized()
        {
            NavigationManager.LocationChanged += LocationChanged;
            base.OnInitialized();
        }

        private void LocationChanged(object? sender, LocationChangedEventArgs e)
        {
            Uri uri = new(e.Location);
            if (uri.Segments.Count() > 1)
                target = uri.Segments[1];
            else
                target = "";
            StateHasChanged();
        }

        private Appearance SetAppearance(string location) => (location == target) ? Appearance.Neutral : Appearance.Stealth;
    }
}