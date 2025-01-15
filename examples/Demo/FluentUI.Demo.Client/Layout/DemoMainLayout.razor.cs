// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Client.Layout;

public partial class DemoMainLayout
{
    private string? _version;
    private FluentLayout? _layout;
    private bool _menuOpened;
    private bool _consoleLogOpened;
    private bool _useReboot;

    [Inject]
    public required NavigationManager Navigation { get; set; }

    [Parameter]
    public RenderFragment? Body { get; set; }

    protected override void OnInitialized()
    {
        _version = AppVersion.Version;
        _useReboot = new Uri(Navigation.Uri).Query.Contains("reboot", StringComparison.InvariantCultureIgnoreCase);

        // Reset the menu when the location changes
        Navigation.RegisterLocationChangingHandler((e) =>
        {
            _menuOpened = false;
            return ValueTask.CompletedTask;
        });
    }

    private void ReloadReboot()
    {
        var uri = new Uri(Navigation.Uri);
        var baseUri = uri.GetLeftPart(UriPartial.Path);

        _useReboot = uri.Query.Contains("reboot", StringComparison.InvariantCultureIgnoreCase);

        // Toggle the reboot flag
        _useReboot = !_useReboot;

        if (_useReboot)
        {
            Navigation.NavigateTo($"{baseUri}?reboot", true);
        }
        else
        {
            Navigation.NavigateTo(baseUri, true);
        }
    }

    private bool IsHomePage()
    {
        if (Navigation.Uri == Navigation.BaseUri)
        {
            return true;
        }

        return false;
    }

    private string GetLayoutKey() => IsHomePage() ? "Home" : string.Empty;
}
