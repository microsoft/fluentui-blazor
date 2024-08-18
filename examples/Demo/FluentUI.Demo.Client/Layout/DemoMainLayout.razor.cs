// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Client.Layout;

public partial class DemoMainLayout
{
    private FluentLayout? Layout;
    private bool MenuOpened;
    private bool ConsoleLogOpened;

    [Parameter]
    public RenderFragment? Body { get; set; }

    protected override void OnInitialized()
    {
        // Reset the menu when the location changes
        Navigation.RegisterLocationChangingHandler((e) =>
        {
            MenuOpened = false;
            return ValueTask.CompletedTask;
        });
    }
}
