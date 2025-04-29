// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Component to display a hamburger icon that can be used to open and close a menu, on mobile device only.
/// </summary>
public partial class FluentLayoutHamburger : FluentComponentBase
{
    /// <summary />
    public FluentLayoutHamburger()
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
    protected string? StyleValue => DefaultStyleBuilder.Build();

    /// <summary>
    /// Gets or sets the parent layout component.
    /// </summary>
    [CascadingParameter]
    private FluentLayout? LayoutContainer { get; set; }

    /// <summary>
    /// Gets or sets the layout to which this hamburger belongs.
    /// If not set, the parent layout is used.
    /// </summary>
    [Parameter]
    public FluentLayout? Layout { get; set; }

    /// <summary>
    /// Gets or sets the title to display when the user hovers over the hamburger icon.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the icon to display.
    /// By default, this icon is a hamburger icon.
    /// </summary>
    [Parameter]
    public Icon Icon { get; set; } = new CoreIcons.Regular.Size20.LineHorizontal3();

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public EventCallback<LayoutHamburgerEventArgs> OnOpened { get; set; }

    /// <summary />
    protected override void OnInitialized()
    {
        Title = Localizer[Localization.LanguageResource.FluentLayoutHamburger_Title];

        var layout = Layout ?? LayoutContainer;
        layout?.AddItem(this);
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
        Console.WriteLine(isExpanded);

        if (OnOpened.HasDelegate)
        {
            await OnOpened.InvokeAsync(new LayoutHamburgerEventArgs(Id ?? "", isExpanded));
        }

        await Task.CompletedTask;
    }

    /// <summary />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "This method is required to have a clickable button")]
    private Task HamburgerClickAsync(MouseEventArgs e)
    {
        // This method is required to have a clickable button
        // But the click event is handled by the JavaScript function in `Microsoft.FluentUI.Blazor.Components.Layout`.
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override ValueTask DisposeAsync()
    {
        var layout = Layout ?? LayoutContainer;
        layout?.RemoveItem(this);

        return base.DisposeAsync();
    }
}
