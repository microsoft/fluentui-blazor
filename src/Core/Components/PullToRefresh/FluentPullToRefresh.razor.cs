// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------
// Based on https://github.com/BcdLib/PullComponent

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentPullToRefresh : FluentComponentBase
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/PullToRefresh/FluentPullToRefresh.razor.js";
    private IJSObjectReference _jsModule = default!;
    private PullStatus _pullStatus = PullStatus.Awaiting;
    private double _startY = 0;
    private int _moveDistance = 0;
    private string _wrapperStyle = "";

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-pull-container")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .Build();

    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    [Parameter]
    public PullDirection Direction { get; set; } = PullDirection.Down;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// returns whether there is more data
    /// </summary>
    [Parameter]
    public Func<Task<bool>>? OnRefreshing { get; set; }

    [Parameter]
    public RenderFragment PullingTip { get; set; } = builder =>
    {
        builder.AddContent(0, "Pull down to refresh");
    };

    [Parameter]
    public RenderFragment WaitingTip { get; set; } = builder =>
    {
        builder.AddContent(0, "Release to update");
    };

    [Parameter]
    public RenderFragment LoadingTip { get; set; } = builder =>
    {
        builder.AddContent(0, "Updating...");
    };

    [Parameter]
    public RenderFragment CompletedTip { get; set; } = builder =>
    {
        builder.AddContent(0, "The update is complete");
    };

    [Parameter]
    public RenderFragment NoDataTip { get; set; } = builder =>
    {
        builder.AddContent(0, "No more data");
};

    [Parameter]
    public int MaxDistance { get; set; } = 50;

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
        }
    }
    private RenderFragment GetTipHtml()
    {
        var renderFragment = _pullStatus switch
        {
            PullStatus.WaitingForRelease => WaitingTip,
            PullStatus.Loading => LoadingTip,
            PullStatus.Completed => CompletedTip,
            PullStatus.NoData => NoDataTip,
            _ => PullingTip,
        };
        return renderFragment;
    }

    private string GetWrapperStyle()
    {
        return _pullStatus switch
        {
            PullStatus.Awaiting => "",
            _ => _wrapperStyle,
        };
    }

    private void OnTouchStart(TouchEventArgs e)
    {
        if (_pullStatus == PullStatus.NoData)
        {
            return;
        }
        if (_pullStatus == PullStatus.Awaiting || _pullStatus == PullStatus.Completed
        )
        {
            SetPullStatus(PullStatus.Pulling);
            // Gets the initial y-axis position
            _startY = e.TargetTouches[0].ClientY;
            // When the touch starts, the animation time and movement distance are set to 0
            _moveDistance = 0;
        }
    }

    private async Task OnTouchMoveAsync(TouchEventArgs e)
    {
        if (_pullStatus == PullStatus.Pulling || _pullStatus == PullStatus.WaitingForRelease)
        {
            if (Direction == PullDirection.Down)
            {
                await OnTouchMoveDownAsync(e);
            }
            else
            {
                await OnTouchMoveUpAsync(e);
            }

        }
    }

    private async Task OnTouchMoveDownAsync(TouchEventArgs e)
    {
        // If document is a scroll bar, touch sliding is a simple way to scroll up and down the page
        var distToTop = await _jsModule.InvokeAsync<int>("getScrollDistToTop");
        ;
        if (distToTop > 0)
        {
            return;
        }

        var move = e.TargetTouches[0].ClientY - _startY;
        // Only a positive number means that the user has pulled down.
        if (move > 0)
        {
            SetDistance(CalcMoveDistance(move));
        }
    }

    private async Task OnTouchMoveUpAsync(TouchEventArgs e)
    {
        // If document is a scroll bar, touch sliding is a simple way to scroll up and down the page
        var distToBottom = await _jsModule.InvokeAsync<int>("getScrollDistToBottom");
        ;
        if (distToBottom > 0)
        {
            return;
        }

        var move = _startY - e.TargetTouches[0].ClientY;
        // Only a positive number means that the user has pulled down.
        if (move > 0)
        {
            SetDistance(CalcMoveDistance(move));
        }
    }

    private async Task OnTouchEndAsync(TouchEventArgs e)
    {
        if (_pullStatus == PullStatus.WaitingForRelease)
        {
            SetPullStatus(PullStatus.Loading);

            var hasMoreData = true;
            if (OnRefreshing is not null)
            {
                try
                {
                    hasMoreData = await OnRefreshing.Invoke();
                }
                catch (Exception)
                {
                    SetDistance(-1);
                    throw;
                }
            }
#if DEBUG
            else
            {
                await Task.Delay(1000);
            }
#endif
            _wrapperStyle = $"transform: translate3d(0, 0, 0);";
            if (!hasMoreData)
            {
                SetPullStatus(PullStatus.NoData);
                await Task.Delay(800);
            }
            else
            {
                SetPullStatus(PullStatus.Completed);
            }
            //StateHasChanged();

            SetDistance(-1);
        }
        else if (_pullStatus == PullStatus.Awaiting || _pullStatus == PullStatus.Pulling)
        {
            SetDistance(-1);
        }
    }

    private static int CalcMoveDistance(double moveDist)
    {
        // Simulated resistance
        return (int)Math.Pow(moveDist, 0.8);
    }

    private void SetDistance(int moveDist)
    {
        if (moveDist < 0)
        {
            SetPullStatus(PullStatus.Awaiting);
            _moveDistance = 0;
            _wrapperStyle = "";
            StateHasChanged();
        }
        else
        {
            if (moveDist < MaxDistance)
            {
                SetPullStatus(PullStatus.Pulling);
            }
            else
            {
                SetPullStatus(PullStatus.WaitingForRelease);
                moveDist = MaxDistance;
            }
            if (_moveDistance != moveDist)
            {
                _moveDistance = moveDist;
                if (Direction == PullDirection.Down)
                {
                    _wrapperStyle = $"transform: translate3d(0, {moveDist}px, 0);";
                }
                else
                {
                    _wrapperStyle = $"transform: translate3d(0, -{moveDist}px, 0);";
                }
                StateHasChanged();
            }
        }
    }

    private void SetPullStatus(PullStatus newPullStatus)
    {
        if (_pullStatus != newPullStatus)
        {
            _pullStatus = newPullStatus;
        }
    }
}
