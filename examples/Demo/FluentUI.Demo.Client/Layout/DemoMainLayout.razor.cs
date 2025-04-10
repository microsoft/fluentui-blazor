// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Client.Layout;

public partial class DemoMainLayout
{
    private FluentLayout? _layout;
    private bool _menuOpened;
    private bool _consoleLogOpened;
    private bool _useReboot;
    private bool _darkTheme;

    [Inject]
    public required IJSRuntime JSRuntime { get; set; }

    [Inject]
    public required NavigationManager Navigation { get; set; }

    [Parameter]
    public RenderFragment? Body { get; set; }

    protected override void OnInitialized()
    {
        _useReboot = new Uri(Navigation.Uri).Query.Contains("reboot", StringComparison.InvariantCultureIgnoreCase);

        // Reset the menu when the location changes
        Navigation.RegisterLocationChangingHandler((e) =>
        {
            _menuOpened = false;
            return ValueTask.CompletedTask;
        });
    }

    private string? LayoutStyleHeight => new StyleBuilder()
        .AddStyle("--layout-footer-height", "calc(150px + 36px)", when: _consoleLogOpened == true)
        .AddStyle("--layout-footer-height", "36px", when: _consoleLogOpened == false)
        .Build();

    private async Task SwitchThemeAsync()
    {
        _darkTheme = !_darkTheme;
        await JSRuntime.InvokeVoidAsync(_darkTheme ? "Blazor.theme.setDarkTheme" : "Blazor.theme.setLightTheme");
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

    /// <summary />
    private bool IsHomePage() => Navigation.Uri == Navigation.BaseUri;

    /// <summary />
    private string GetLayoutKey() => IsHomePage() ? "Home" : string.Empty;    
}
