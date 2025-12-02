// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
//using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a navigation menu component that displays a hamburger icon and provides navigation functionality within
/// a Fluent UI application.
/// </summary>
/// <remarks>
/// <para>Use the FluentNav to present a collapsible navigation menu, typically accessed via a hamburger icon.
/// The component supports customization of the displayed icon and its tooltip text and is designed for integration
/// into Fluent UI layouts.</para>
///
/// <para><strong>Allowed Child Components:</strong></para>
/// <para>Only components implementing the <see cref="INavItem"/> interface are allowed as direct children
/// of FluentNav. Attempting to use other components will result in an <see cref="InvalidOperationException"/>.</para>
///
/// <para>Valid direct children include:</para>
/// <list type="bullet">
/// <item><description><see cref="FluentNavItem"/> - A simple navigation item</description></item>
/// <item><description><see cref="FluentNavCategory"/> - A grouped set of navigation items</description></item>
/// <item><description><see cref="FluentNavSectionHeader"/> - A section header to organize navigation</description></item>
/// <item><description><see cref="FluentNavDivider"/> - A visual divider between sections</description></item>
/// </list>
///
/// <para><strong>Nested Navigation:</strong></para>
/// <para><see cref="FluentNavSubItem"/> components can only be used as direct children of <see cref="FluentNavCategory"/>,
/// not directly in the drawer.</para>
/// </remarks>
public partial class FluentNav : FluentComponentBase
{
    //private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "Nav/FluentNav.razor.js";

    /// <summary />
    public FluentNav(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-nav")
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary>
    /// Gets or sets the icon to display besides the app title.
    /// </summary>
    [Parameter]
    public Icon? AppIcon { get; set; }

    /// <summary>
    /// Gets or sets the title to display at the top of the menu .
    /// </summary>
    [Parameter]
    public string? AppTitle { get; set; }

    /// <summary>
    /// Gets or sets the link to use for the app item.
    /// Defaults to homepage ("/").
    /// </summary>
    [Parameter]
    public string AppLink { get; set; } = "/";

    // This is for when the header with the hamburger icon is implemented
    ///// <summary>
    ///// Gets or sets the icon to display for collapsing/expanding the nav menu.
    ///// By default, this icon is a hamburger icon.
    ///// </summary>
    //[Parameter]
    //public Icon ToggleIcon { get; set; } = new CoreIcons.Regular.Size20.LineHorizontal3();

    ///// <summary>
    ///// Gets or sets the title to display when the user hovers over the hamburger icon.
    ///// </summary>
    //[Parameter]
    //public string? ToggleIconTitle { get; set; }

    /// <summary>
    /// Gets or sets wether to enable using icons in the nav items.
    /// </summary>
    [Parameter]
    public bool UseIcons { get; set; } = true;

    /// <summary>
    /// Gets or sets wether to allowjust one expanded category or multiple
    /// </summary>
    [Parameter]
    public bool UseSingleExpanded { get; set; } = true;

    /// <summary>
    /// Gets or sets the density of the nav menu item.
    /// </summary>
    [Parameter]
    public NavDrawerDensity? Density { get; set; }

    /// <summary>
    /// Gets or sets the content of the nav menu item.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Import the JavaScript module
            //var jsModule = await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);

            // Call a function from the JavaScript module
            //await jsModule.InvokeVoidAsync("Microsoft.FluentUI.Blazor.NavDrawer.InitializeCategoryToggle",
            //    UseSingleExpanded);
        }
    }
}
