// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Component that defines a layout for a page, using a grid composed of a PanelHeader, a Footer and 3 columns: Menu, Value and Aside Pane.
/// For mobile devices (&lt; 768px), the layout is a single column with the Menu, Value and Footer panes stacked vertically.
/// </summary>
public partial class FluentLayout : FluentComponentBase
{
    private const string DEFAULT_HEADER_HEIGHT = "44px";
    private const string DEFAULT_FOOTER_HEIGHT = "36px";
    private const string DEFAULT_CONTENT_HEIGHT = "calc(var(--layout-height) - var(--layout-header-height) - var(--layout-footer-height))";

    internal bool IsMobile { get; set; }

    /// <summary />
    public FluentLayout(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    internal List<FluentLayoutHamburger> Hamburgers { get; } = [];

    /// <summary>
    /// Gets the list of items that are part of the layout.
    /// </summary>
    internal List<FluentLayoutItem> Areas { get; } = [];

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
        .AddStyle("--layout-height", string.IsNullOrEmpty(Height) ? "100dvh" : Height)
        .AddStyle("--layout-header-height", HeaderHeight)
        .AddStyle("--layout-footer-height", FooterHeight)
        .AddStyle("--layout-body-height", ContentHeight)
        .Build();

    /// <summary>
    /// Gets or sets the vertical scrollbar position:
    /// global to the entire LayoutContainer (true), or inside the content area (false).
    /// </summary>
    [Parameter]
    public bool GlobalScrollbar { get; set; }

    /// <summary>
    /// Gets ot sets the width of the LayoutContainer.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the LayoutContainer.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the width, in pixels, at which the container switches to a mobile layout.
    /// </summary>
    [Parameter]
    public int MobileBreakdownWidth { get; set; } = 768;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Event raised when layout changes to a Mobile layout (true) or Desktop layout (false).
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnBreakpointEnter { get; set; }

    /// <summary>
    /// Gets or sets whether the <see cref="LayoutArea.Menu"/> content is rendered only when this area is visible.
    /// To reduce the HTML page size, the <see cref="FluentLayoutHamburger" /> is not rendered when the layout is in desktop mode.
    /// To use this feature, make sure to enable the Blazor interactive mode.
    /// </summary>
    [Parameter]
    public bool MenuDeferredLoading { get; set; } = false;

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var dotNetHelper = DotNetObjectReference.Create(this);
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Layout.Initialize", dotNetHelper, Id, MobileBreakdownWidth);
        }
    }

    /// <summary />
    [JSInvokable]
    public async Task FluentLayout_MediaChangedAsync(string size)
    {
        var mobile = string.Equals(size, "mobile", StringComparison.Ordinal);
        if (IsMobile == mobile)
        {
            return;
        }

        IsMobile = mobile;

        // Raise the event
        if (OnBreakpointEnter.HasDelegate)
        {
            await OnBreakpointEnter.InvokeAsync(IsMobile);
        }

        // Update the layout (Menu and Hamburger)
        if (MenuDeferredLoading)
        {
            foreach (var item in Hamburgers)
            {
                await item.RefreshAsync();
            }

            foreach (var item in Areas.Where(i => i.Area == LayoutArea.Menu))
            {
                await item.RefreshAsync();
            }
        }
    }

    /// <summary />
    internal void AddItem(FluentComponentBase item)
    {
        switch (item)
        {
            case FluentLayoutItem layoutItem:
                Areas.Add(layoutItem);
                break;

            case FluentLayoutHamburger layoutHamburger:
                Hamburgers.Add(layoutHamburger);
                break;

            default:
                throw new ArgumentException("Item must be of type FluentLayoutItem or FluentLayoutHamburger.", nameof(item));
        }
    }

    /// <summary />
    internal void RemoveItem(FluentComponentBase item)
    {
        switch (item)
        {
            case FluentLayoutItem layoutItem:
                Areas.Remove(layoutItem);
                break;

            case FluentLayoutHamburger layoutHamburger:
                Hamburgers.Remove(layoutHamburger);
                break;

            default:
                throw new ArgumentException("Item must be of type FluentLayoutItem or FluentLayoutHamburger.", nameof(item));
        }
    }

    /// <summary />
    internal bool HasHeader => Areas.Exists(i => i.Area == LayoutArea.Header);

    /// <summary />
    internal string HeaderHeight => Areas.Find(i => i.Area == LayoutArea.Header)?.Height ?? DEFAULT_HEADER_HEIGHT;

    /// <summary />
    internal bool HeaderSticky => Areas.Find(i => i.Area == LayoutArea.Header)?.Sticky ?? false;

    /// <summary />
    internal bool HasFooter => Areas.Exists(i => i.Area == LayoutArea.Footer);

    /// <summary />
    internal string FooterHeight => Areas.Find(i => i.Area == LayoutArea.Footer)?.Height ?? DEFAULT_FOOTER_HEIGHT;

    /// <summary />
    internal bool FooterSticky => Areas.Find(i => i.Area == LayoutArea.Footer)?.Sticky ?? false;

    /// <summary />
    internal string ContentHeight => Areas.Find(i => i.Area == LayoutArea.Content)?.Height ?? DEFAULT_CONTENT_HEIGHT;

    /// <summary />
    internal string GetContainerMobileStyles()
    {
        return CssBuilder.MinifyCss(@$"
            #{Id}-container {{
                container-type: inline-size;
                container-name: layout-{Id};
            }}

            @container layout-{Id} (max-width: {Convert.ToString(MobileBreakdownWidth, CultureInfo.InvariantCulture)}px) {{
                .fluent-layout {{
                    grid-template-areas:
                        ""header""
                        ""content""
                        ""footer"";
                    grid-template-columns: 1fr;
                    grid-template-rows: auto 1fr auto;
                    overflow-x: auto;
                }}

                .fluent-layout .fluent-layout-item[area=""menu""],
                .fluent-layout .fluent-layout-item[area=""aside""] {{
                  display: none;
                }}

                .fluent-layout .fluent-layout-item[area=""header""] .fluent-layout-hamburger {{
                    display: flex;
                }}
            }}
        ");
    }
}
