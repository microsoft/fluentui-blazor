// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Client.Documentation.Components.Layout.DebugPages;

public partial class DebugLayout
{
    private bool GlobalScrollbar = true;
    private bool MenuDeferredLoading;
    private string HamburgerContent = string.Empty;
    private readonly RenderFragment _renderOptions;
    private readonly Option Header = new() { Visible = true, Sticky = false };
    private readonly Option Menu = new() { Visible = true, Sticky = false };
    private readonly Option Content = new() { Visible = true, Sticky = false };
    private readonly Option Aside = new() { Visible = true, Sticky = false };
    private readonly Option Footer = new() { Visible = true, Sticky = false };

    public bool MobileView { get; set; }
    public bool Opened { get; set; }

    private class Option
    {
        public bool Visible { get; set; }
        public bool Sticky { get; set; }
    }

    public DebugLayout()
    {
        _renderOptions = RenderOptions;
    }

    private static readonly MarkupString NavigationContent = SampleData.Text.Titles.Take(20).ToMarkupList("li");
    private static readonly MarkupString BodyContent = SampleData.Text.LoremIpsum.Take(10).ToMarkupList("p");
    private static readonly MarkupString AsideContent = (MarkupString)SampleData.Text.LoremIpsum.ElementAt(2)[..200];
}
