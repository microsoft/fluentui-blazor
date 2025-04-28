// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Client.Documentation.Components.Layout.Debug;

public partial class DebugLayout
{
    private bool GlobalScrollbar = true;
    private readonly RenderFragment _renderOptions;
    private readonly Option Header = new Option() { Visible = true, Sticky = false };
    private readonly Option Menu = new Option() { Visible = true, Sticky = false };
    private readonly Option Content = new Option() { Visible = true, Sticky = false };
    private readonly Option Aside = new Option() { Visible = true, Sticky = false };
    private readonly Option Footer = new Option() { Visible = true, Sticky = false };

    private class Option
    {
        public bool Visible { get; set; }
        public bool Sticky { get; set; }
    }

    public DebugLayout()
    {
        _renderOptions = RenderOptions;
    }
}
