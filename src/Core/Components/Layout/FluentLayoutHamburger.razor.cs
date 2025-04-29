// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

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
    private RenderFragment? MenuContent => LayoutContainer?.Items.FirstOrDefault(i => i.Area == LayoutArea.Menu)?.ChildContent;

    /// <summary />
    protected override void OnInitialized()
    {
        Title = Localizer[Localization.LanguageResource.FluentLayoutHamburger_Title];

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
        Console.WriteLine(isExpanded);

        if (OnOpened.HasDelegate)
        {
            await OnOpened.InvokeAsync(new LayoutHamburgerEventArgs(Id ?? "", isExpanded));
        }

        await Task.CompletedTask;
    }

    /// <inheritdoc />
    public override ValueTask DisposeAsync()
    {
        LayoutContainer?.RemoveItem(this);
        return base.DisposeAsync();
    }
}
