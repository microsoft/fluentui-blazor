// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Client.Layout;

public partial class DemoMainLayout
{
    private bool _consoleLogOpened;
    private bool _useReboot;

    [Inject]
    public required IJSRuntime JSRuntime { get; set; }

    [Inject]
    public required NavigationManager Navigation { get; set; }

    [Parameter]
    public RenderFragment? Body { get; set; }

    protected override void OnInitialized()
    {
        _useReboot = new Uri(Navigation.Uri).Query.Contains("reboot", StringComparison.InvariantCultureIgnoreCase);
    }

    private string? LayoutStyleHeight => new StyleBuilder()
        .AddStyle("--layout-footer-height", "calc(150px + 36px)", when: _consoleLogOpened == true)
        .AddStyle("--layout-footer-height", "36px", when: _consoleLogOpened == false)
        .Build();

    private async Task SwitchThemeAsync()
    {
        await JSRuntime.InvokeVoidAsync("Blazor.theme.switchTheme");
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

    /// <summary>
    /// Opens a URL in a new browser tab.
    /// </summary>
    private async Task OpenUrlInNewTabAsync(string url)
    {
        await JSRuntime.InvokeVoidAsync("open", url, "_blank");
    }

    internal class GitHubIcon : Icon
    {
        public GitHubIcon() : base(
            "GitHub",
            IconVariant.Regular,
            IconSize.Size20,
            "<path fill-rule=\"evenodd\" clip-rule=\"evenodd\" d=\"M10.178 0C4.55 0 0 4.583 0 10.254c0 4.533 2.915 8.369 6.959 9.727 0.506 0.102 0.691 -0.221 0.691 -0.492 0 -0.238 -0.017 -1.053 -0.017 -1.901 -2.831 0.611 -3.421 -1.222 -3.421 -1.222 -0.455 -1.188 -1.129 -1.494 -1.129 -1.494 -0.927 -0.628 0.068 -0.628 0.068 -0.628 1.028 0.068 1.567 1.053 1.567 1.053 0.91 1.562 2.376 1.12 2.966 0.849 0.084 -0.662 0.354 -1.12 0.64 -1.375 -2.258 -0.238 -4.634 -1.12 -4.634 -5.059 0 -1.12 0.404 -2.037 1.045 -2.75 -0.101 -0.255 -0.455 -1.307 0.101 -2.716 0 0 0.859 -0.272 2.797 1.053a9.786 9.786 0 0 1 2.545 -0.34c0.859 0 1.735 0.119 2.544 0.34 1.938 -1.324 2.797 -1.053 2.797 -1.053 0.556 1.409 0.202 2.462 0.101 2.716 0.657 0.713 1.045 1.63 1.045 2.75 0 3.939 -2.376 4.804 -4.651 5.059 0.371 0.323 0.691 0.934 0.691 1.901 0 1.375 -0.017 2.479 -0.017 2.818 0 0.272 0.185 0.594 0.691 0.493 4.044 -1.358 6.959 -5.195 6.959 -9.727C20.356 4.583 15.789 0 10.178 0z\"></path>")
        { }
    }
}
