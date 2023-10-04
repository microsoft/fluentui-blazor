using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

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
       .AddStyle("margin", $"{Gap} 0" , !string.IsNullOrEmpty(Gap))
       .Build();


    protected string? ButtonClassValue =>
        new CssBuilder("expand-collapse-button")
            .AddClass("rotate", Expanded)
            .Build();

    protected string? ExpandIconClassValue =>
        new CssBuilder("fluent-nav-expand-icon")
            .Build();

    internal Dictionary<string, object?> Attributes
    {
        get => Disabled ? new Dictionary<string, object?>() : new Dictionary<string, object?>
        {
            { "href", Href },
            { "target", Target },
            { "rel", !string.IsNullOrWhiteSpace(Target) ? "noopener noreferrer" : string.Empty }
        };
    }

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
    /// Needs to be a valid CSS value. Defaults to 10px.
    /// </summary>
    [Parameter]
    public string? Gap { get; set; } = "10px";

    /// <summary>
    /// If set, overrides the default expand icon.
    /// </summary>
    [Parameter]
    public Icon ExpandIcon { get; set; } = new CoreIcons.Regular.Size12.ChevronRight();

    [Parameter]
    public EventCallback<bool> ExpandedChanged { get; set; }

    public FluentNavGroup()
    {
        _renderContent = RenderContent;
        _renderButton = RenderButton;
    }

    private Task ToggleExpandedAsync() => SetExpandedAsync(!Expanded);
   
    private async Task HandleExpanderKeyDownAsync(KeyboardEventArgs args)
    {
        Task handler = args.Code switch
        {
            "Enter" => SetExpandedAsync(!Expanded),
            "ArrowRight" => SetExpandedAsync(true),
            "ArrowLeft" => SetExpandedAsync(false),
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

        Expanded = value;
        if (ExpandedChanged.HasDelegate)
        {
            await ExpandedChanged.InvokeAsync(value);
        }
    }
}