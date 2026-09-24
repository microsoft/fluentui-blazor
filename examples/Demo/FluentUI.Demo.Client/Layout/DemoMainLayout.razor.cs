// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Demo.Client.Layout;

public partial class DemoMainLayout
{
    private bool _consoleLogOpened;

    [Inject]
    public required NavigationManager Navigation { get; set; }

    [Parameter]
    public RenderFragment? Body { get; set; }

    private string? LayoutStyleHeight => new StyleBuilder()
        .AddStyle("--layout-footer-height", "calc(150px + 36px)", when: _consoleLogOpened == true)
        .AddStyle("--layout-footer-height", "36px", when: _consoleLogOpened == false)
        .Build();

  
    /// <summary />
    private bool IsHomePage() => Navigation.Uri == Navigation.BaseUri;

    /// <summary />
    private string GetLayoutKey() => IsHomePage() ? "Home" : string.Empty;
}
