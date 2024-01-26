using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;
public partial class FluentCollapsibleRegion : FluentComponentBase
{
    private bool _expanded;

    protected string? StyleValue =>
        new StyleBuilder(Style)
            .AddStyle("max-height", MaxHeight, !string.IsNullOrEmpty(MaxHeight))
            .AddStyle("height", "auto", Expanded)
            .AddStyle("height", "0", !Expanded)
            .Build();

    protected string? ClassValue =>
        new CssBuilder(Class)
            .AddClass("fluent-collapsible-region-container")
            .Build();

    /// <summary>
    /// If true, the region is expaned, otherwise it is collapsed.
    /// </summary>
    [Parameter]
    public bool Expanded
    {
        get => _expanded;
        set
        {
            if (_expanded == value)
            {
                return;
            }
            _expanded = value;
            _ = ExpandedChanged.InvokeAsync(_expanded);
        }
    }

    /// <summary>
    /// Explicitly sets the height for the Collapse element to override the css default.
    /// </summary>
    [Parameter]
    public string? MaxHeight { get; set; }

    /// <summary>
    /// Gets or sets the child content of component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Callback for when the Expanded property changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> ExpandedChanged { get; set; }
}
