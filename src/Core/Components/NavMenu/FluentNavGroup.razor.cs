// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentNavGroup : FluentNavBase
{
    private readonly RenderFragment _renderContent;
    private readonly RenderFragment _renderButton;

    protected string? ClassValue =>
        new CssBuilder("fluent-nav-group")
            .AddClass("fluent-nav-item")
            .AddClass("expanded", Expanded)
            .AddClass("disabled", Disabled)
            .AddClass(Class)
            .Build();

    internal string? StyleValue => new StyleBuilder(Style)
       .AddStyle("margin", $"{Gap} 0", !string.IsNullOrEmpty(Gap))
       .Build();

    protected string? ButtonClassValue =>
        new CssBuilder("expand-collapse-button")
            .AddClass("rotate", Expanded)
            .Build();

    protected static string? ExpandIconClassValue =>
        new CssBuilder("fluent-nav-expand-icon")
            .Build();

    internal Dictionary<string, object?> Attributes
    {
        get => Disabled ? [] : new Dictionary<string, object?>
        {
            { "href", Href },
            { "target", Target },
            { "rel", !string.IsNullOrWhiteSpace(Target) ? "noopener noreferrer" : string.Empty }
        };
    }
    /// <summary>
    /// Gets or sets the text to display for the group.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// If true, expands the nav group, otherwise collapse it. 
    /// Two-way bindable
    /// </summary>
    [Parameter]
    public bool Expanded { get; set; }

    /// <summary>
    /// If true, hides expand button at the end of the NavGroup.
    /// </summary>
    [Parameter]
    public bool HideExpander { get; set; }

    /// <summary>
    /// Explicitly sets the height for the Collapse element to override the css default.
    /// </summary>
    [Parameter]
    public string? MaxHeight { get; set; }

    /// <summary>
    /// Defines the vertical spacing between the NavGroup and adjecent items. 
    /// Needs to be a valid CSS value.
    /// </summary>
    [Parameter]
    public string? Gap { get; set; }

    /// <summary>
    /// If set, overrides the default expand icon.
    /// </summary>
    [Parameter]
    public Icon ExpandIcon { get; set; } = new CoreIcons.Regular.Size12.ChevronRight();

    /// <summary>
    /// Allows for specific markup and styling to be applied for the group title 
    /// When using this, the containded <see cref="FluentNavLink"/>s and <see cref="FluentNavGroup"/>s need to be placed in a ChildContent tag.
    /// When specifying both Title and TitleTemplate, both will be rendered.
    /// </summary>
    [Parameter]
    public RenderFragment? TitleTemplate { get; set; }

    /// <summary>
    /// Gets or sets a callback that is triggered whenever <see cref="Expanded"/> changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> ExpandedChanged { get; set; }

    public FluentNavGroup()
    {
        Id = Identifier.NewId();
        _renderContent = RenderContent;
        _renderButton = RenderButton;
    }

    private Task ToggleExpandedAsync() => SetExpandedAsync(!Expanded);

    private async Task HandleExpanderKeyDownAsync(FluentKeyCodeEventArgs args)
    {
        if (args.TargetId != Id)
        {
            return;
        }
        Task handler = args.Key switch
        {
            KeyCode.Enter => SetExpandedAsync(!Expanded),
            KeyCode.Right => SetExpandedAsync(true),
            KeyCode.Left => SetExpandedAsync(false),
            _ => Task.CompletedTask
        };
        await handler;
    }

    private async Task SetExpandedAsync(bool value)
    {
        if (value == Expanded)
        {
            return;
        }

        if (!Owner.Expanded)
        {
            await Owner.ExpandedChanged.InvokeAsync(true);
        }
        else
        {
            Expanded = value;

            if (ExpandedChanged.HasDelegate)
            {
                await ExpandedChanged.InvokeAsync(value);
            }
        }
    }
}
