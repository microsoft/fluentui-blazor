// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Component that defines a layout for a page, using a grid composed of a Header, a Footer and 3 columns: Menu, Value and Aside Pane.
/// For mobile devices (&lt; 768px), the layout is a single column with the Menu, Value and Footer panes stacked vertically.
/// </summary>
public partial class FluentLayout
{
    private const string DEFAULT_HEADER_HEIGHT = "44px";
    private const string DEFAULT_FOOTER_HEIGHT = "36px";
    private const string DEFAULT_CONTENT_HEIGHT = "calc(var(--layout-height) - var(--layout-header-height) - var(--layout-footer-height))";

    internal FluentLayoutHamburger? Hamburger { get; private set; }

    /// <summary>
    /// Gets the list of items that are part of the layout.
    /// </summary>
    internal List<FluentLayoutItem> Items { get; } = [];

    /// <summary>
    /// <inheritdoc cref="FluentComponentBase.Class"/>
    /// </summary>
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-layout")
        .Build();

    /// <summary>
    /// <inheritdoc cref="FluentComponentBase.Style"/>
    /// </summary>
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("width", Width, when: !string.IsNullOrEmpty(Width))
        .AddStyle("height", "var(--layout-height)")
        .AddStyle("--layout-height", string.IsNullOrEmpty(Height) ? "100vh" : Height)
        .AddStyle("--layout-header-height", HeaderHeight)
        .AddStyle("--layout-footer-height", FooterHeight)
        .AddStyle("--layout-body-height", ContentHeight)
        .Build();

    /// <summary>
    /// Gets or sets the vertical scrollbar position:
    /// global to the entire Layout (true), or inside the content area (false).
    /// </summary>
    [Parameter]
    public bool GlobalScrollbar { get; set; }

    /// <summary>
    /// Gets ot sets the width of the Layout.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the Layout.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    internal void AddHamburger(FluentLayoutHamburger hamburger)
    {
        if (Hamburger != null)
        {
            throw new InvalidOperationException("Only one Hamburger can be added to a FluentLayout.");
        }

        Hamburger = hamburger;
    }

    internal Task RefreshAsync()
    {
        StateHasChanged();
        return Task.CompletedTask;
    }

    /// <summary />
    internal void AddItem(FluentLayoutItem item)
    {
        Items.Add(item);
    }

    /// <summary />
    internal bool HasHeader => Items.Exists(i => i.Area == LayoutArea.Header);

    /// <summary />
    internal string HeaderHeight => Items.Find(i => i.Area == LayoutArea.Header)?.Height ?? DEFAULT_HEADER_HEIGHT;

    /// <summary />
    internal bool HeaderSticky => Items.Find(i => i.Area == LayoutArea.Header)?.Sticky ?? false;

    /// <summary />
    internal bool HasFooter => Items.Exists(i => i.Area == LayoutArea.Footer);

    /// <summary />
    internal string FooterHeight => Items.Find(i => i.Area == LayoutArea.Footer)?.Height ?? DEFAULT_FOOTER_HEIGHT;

    /// <summary />
    internal bool FooterSticky => Items.Find(i => i.Area == LayoutArea.Footer)?.Sticky ?? false;

    /// <summary />
    internal string ContentHeight => Items.Find(i => i.Area == LayoutArea.Content)?.Height ?? DEFAULT_CONTENT_HEIGHT;
}
