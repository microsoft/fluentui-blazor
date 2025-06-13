// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Component to display a hamburger icon that can be used to open and close a menu, on mobile device only.
/// </summary>
public partial class FluentLayoutHamburger : FluentComponentBase
{
    /// <summary />
    public FluentLayoutHamburger(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// <inheritdoc cref="FluentComponentBase.Class"/>
    /// </summary>
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-layout-hamburger")
        .Build();

    /// <summary>
    /// <inheritdoc cref="FluentComponentBase.Style"/>
    /// </summary>
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("display", "flex", when: Visible == true)
        .AddStyle("display", "none", when: Visible == false)
        .Build();

    /// <summary>
    /// Gets or sets the parent layout component.
    /// </summary>
    [CascadingParameter]
    private FluentLayout? LayoutContainer { get; set; }

    /// <summary>
    /// Gets or sets the icon to display.
    /// By default, this icon is a hamburger icon.
    /// </summary>
    [Parameter]
    public Icon Icon { get; set; } = new CoreIcons.Regular.Size20.LineHorizontal3();

    /// <summary>
    /// Gets or sets the title to display when the user hovers over the hamburger icon.
    /// </summary>
    [Parameter]
    public string? IconTitle { get; set; }

    /// <summary>
    /// Gets or sets the header to display when the hamburger menu is open.
    /// </summary>
    [Parameter]
    public string? PanelHeader { get; set; }

    /// <summary>
    /// Gets or sets the header template to display when the hamburger menu is open.
    /// </summary>
    [Parameter]
    public RenderFragment? PanelHeaderTemplate { get; set; }

    /// <summary>
    /// Gets or sets the panel position to display when the hamburger menu is open.
    /// Only <see cref="DialogAlignment.Start"/> and <see cref="DialogAlignment.End"/> are supported."/>.
    /// The default value is <see cref="DialogAlignment.Start"/>.
    /// </summary>
    [Parameter]
    public DialogAlignment PanelPosition { get; set; } = DialogAlignment.Start;

    /// <summary>
    /// Gets or sets the size of the panel to display when the hamburger menu is open.
    /// Default value is <see cref="DialogSize.Medium"/>.
    /// </summary>
    [Parameter]
    public DialogSize PanelSize { get; set; } = DialogSize.Medium;

    /// <summary>
    /// Gets or sets the content to display when the hamburger menu is open.
    /// if not set, the content of the menu area is used.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Event that is triggered when the hamburger menu is opened or closed.
    /// </summary>
    [Parameter]
    public EventCallback<LayoutHamburgerEventArgs> OnOpened { get; set; }

    /// <summary>
    /// Gets or sets whether the hamburger menu is visible or not.
    /// Default is null to display the hamburger icon only when the layout is in mobile mode.
    /// </summary>
    [Parameter]
    public bool? Visible { get; set; }

    /// <summary />
    private RenderFragment? MenuContent => ChildContent ?? LayoutContainer?.Areas.Find(i => i.Area == LayoutArea.Menu)?.ChildContent;

    /// <summary />
    protected override void OnInitialized()
    {
        IconTitle = Localizer[Localization.LanguageResource.LayoutHamburger_Title];
        LayoutContainer?.AddItem(this);
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var dotNetHelper = DotNetObjectReference.Create(this);
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Layout.HamburgerInitialize", dotNetHelper, Id);
        }
    }

    /// <summary />
    [JSInvokable]
    public async Task FluentLayout_HamburgerClickAsync(bool isExpanded)
    {
        if (OnOpened.HasDelegate)
        {
            await OnOpened.InvokeAsync(new LayoutHamburgerEventArgs(Id ?? "", isExpanded));
        }
    }

    /// <summary>
    /// Displays the dialog.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public async Task ShowAsync()
    {
        await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Dialog.Show", $"{Id}-drawer");
    }

    /// <summary>
    /// Asynchronously refreshes the current state of the component.
    /// </summary>
    public Task RefreshAsync()
    {
        return InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Hide the dialog.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public async Task HideAsync()
    {
        await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Dialog.Hide", $"{Id}-drawer");
    }

    /// <summary />
    private bool RenderDrawer()
    {
        if (LayoutContainer == null || Visible == true)
        {
            return true;
        }

        // If the Desktop view is active
        if (LayoutContainer.MenuDeferredLoading && !LayoutContainer.IsMobile)
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc />
    public override ValueTask DisposeAsync()
    {
        LayoutContainer?.RemoveItem(this);
        return base.DisposeAsync();
    }
}
