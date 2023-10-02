using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;
using Microsoft.JSInterop;


namespace Microsoft.Fast.Components.FluentUI;
public partial class FluentCollapsibleRegion : FluentComponentBase
{

    internal enum CollapseState
    {
        Entering, Entered, Exiting, Exited
    }

    //internal double _height;
    private bool _expanded, _isRendered, _updateHeight;
    private ElementReference _wrapper;
    internal CollapseState _state = CollapseState.Exited;

    protected string? StyleValue =>
        new StyleBuilder()
            .AddStyle("max-height", MaxHeight, MaxHeight is not null)
            .AddStyle("height", "auto", Expanded)
            .AddStyle("height", "0", !Expanded)
            .AddStyle(Style)
            .Build();

    protected string? ClassValue =>
        new CssBuilder("fluent-collapsible-region-container")
            .AddClass($"fluent-collapsible-region-entering", _state == CollapseState.Entering)
            .AddClass($"fluent-collapsible-region-entered", _state == CollapseState.Entered)
            .AddClass($"fluent-collapsible-region-exiting", _state == CollapseState.Exiting)
            .AddClass(Class)
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
                return;
            _expanded = value;

            if (_isRendered)
            {
                _state = _expanded ? CollapseState.Entering : CollapseState.Exiting;
                //_ = UpdateHeight();
                _updateHeight = true;
            }
            else if (_expanded)
            {
                _state = CollapseState.Entered;
            }

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
    public EventCallback OnAnimationEnd { get; set; }

    [Parameter]
    public EventCallback<bool> ExpandedChanged { get; set; }

    
    //internal async Task UpdateHeight()
    //{
    //    try
    //    {
    //        _height = (await _wrapper.MudGetBoundingClientRectAsync())?.Height ?? 0;
    //    }
    //    catch (Exception ex) when (ex is JSDisconnectedException or TaskCanceledException)
    //    {
    //        _height = 0;
    //    }

    //    if (_height > MaxHeight)
    //    {
    //        _height = MaxHeight.Value;
    //    }
    //}

    //protected override async Task OnAfterRenderAsync(bool firstRender)
    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            _isRendered = true;
            //await UpdateHeight();
        }
        else if (_updateHeight && _state is CollapseState.Entering or CollapseState.Exiting)
        {
            _updateHeight = false;
            //await UpdateHeight();
            StateHasChanged();
        }
    }
}
