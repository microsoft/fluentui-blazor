using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

#nullable enable
public partial class FluentNavGroup : FluentComponentBase
{
    private bool _expanded;
    private readonly RenderFragment _renderButton;

    protected string? Classname =>
        new CssBuilder("fluent-nav-group")
            .AddClass("fluent-nav-item")
            .AddClass(Class)
            .AddClass("expanded", Expanded)
            .AddClass($"disabled", Disabled)
            .Build();

    protected string? ButtonClassname =>
        new CssBuilder("expand-collapse-button")
            .AddClass("rotate", Expanded)
            .Build();

    protected string? IconClassname =>
        new CssBuilder("fluent-nav-icon")
            .AddClass($"fluent-nav-icon-default", IconColor == Color.Accent)
            .Build();

    protected string? ExpandIconClassname =>
        new CssBuilder("fluent-nav-expand-icon")
            .AddClass($"fluent-transform", Expanded && !Disabled)
            .AddClass($"fluent-transform-disabled", Expanded && Disabled)
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

    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// URL for the link.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// The target attribute specifies where to open the link, if Href is specified. 
    /// Possible values: _blank | _self | _parent | _top.
    /// </summary>
    [Parameter]
    public string? Target { get; set; }

    /// <summary>
    /// Class names to use to indicate the item is active, separated by space.
    /// </summary>
    [Parameter]
    public string ActiveClass { get; set; } = "active";

    /// <summary>
    /// Icon to use if set.
    /// </summary>
    [Parameter]
    public Icon? Icon { get; set; }

    /// <summary>
    /// The color of the icon. Default value uses the accent color.
    /// </summary>
    [Parameter]
    public Color IconColor { get; set; } = Color.Accent;

    /// <summary>
    /// If true, the button will be disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// If true, expands the nav group, otherwise collapse it. 
    /// Two-way bindable
    /// </summary>
    [Parameter]
    public bool Expanded
    {
        get => _expanded;
        set
        {
            if (_expanded == value)
                return;

            _expanded = value;
            ExpandedChanged.InvokeAsync(_expanded);
        }
    }

    /// <summary>
    /// If true, hides expand-icon at the end of the NavGroup.
    /// </summary>
    [Parameter]
    public bool HideExpandIcon { get; set; }

    /// <summary>
    /// Explicitly sets the height for the Collapse element to override the css default.
    /// </summary>
    [Parameter]
    public string? MaxHeight { get; set; }

    /// <summary>
    /// If set, overrides the default expand icon.
    /// </summary>
    [Parameter]
    public Icon ExpandIcon { get; set; } = new CoreIcons.Regular.Size12.ChevronDown();

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public EventCallback<bool> ExpandedChanged { get; set; }

    public FluentNavGroup()
    {
        _renderButton = RenderButton;
    }

    protected Task ExpandedToggleAsync()
    {
        _expanded = !Expanded;

        return ExpandedChanged.InvokeAsync(_expanded);
    }
}