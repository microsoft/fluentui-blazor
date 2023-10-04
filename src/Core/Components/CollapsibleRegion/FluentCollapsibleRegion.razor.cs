using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;
using Microsoft.JSInterop;


namespace Microsoft.Fast.Components.FluentUI;
public partial class FluentCollapsibleRegion : FluentComponentBase
{
    private bool _expanded;  

    protected string? StyleValue =>
        new StyleBuilder(Style)
            .AddStyle("max-height", MaxHeight, MaxHeight is not null)
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
    /// Child content of component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public EventCallback<bool> ExpandedChanged { get; set; }
   
}
