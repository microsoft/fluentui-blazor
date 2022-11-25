using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace FluentUI.Demo.Shared.Shared
{
    public partial class DemoNavMenu : IDisposable
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
            if (uri.Segments.Length > 1)
                target = uri.Segments[1];
            else
                target = "";
            //StateHasChanged();
        }

        //private Appearance SetAppearance(string location) => (location == target) ? Appearance.Accent : Appearance.Neutral;

        void IDisposable.Dispose()
        {
            // Unsubscribe from the event when our component is disposed
            NavigationManager.LocationChanged -= LocationChanged;
        }
    }
}