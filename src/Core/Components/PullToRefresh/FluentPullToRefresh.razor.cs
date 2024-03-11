// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

//using System.Reflection.Metadata;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentPullToRefresh : FluentComponentBase
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/PullToRefresh/FluentPullToRefresh.razor.js";
    private IJSObjectReference _jsModule = default!;

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
        builder.AddContent(0, "Pull to refresh");
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

    private PullStatus _pullStatus = PullStatus.Awaiting;

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
            _ => wrapperStyle,
        };
    }

    private double startY = 0;
    private int moveDistance = 0;
    private string wrapperStyle = "";

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
            startY = e.TargetTouches[0].ClientY;
            // When the touch starts, the animation time and movement distance are set to 0
            moveDistance = 0;
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

        var move = e.TargetTouches[0].ClientY - startY;
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

        var move = startY - e.TargetTouches[0].ClientY;
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
            wrapperStyle = $"transform: translate3d(0, 0, 0);";
            if (!hasMoreData)
            {
                SetPullStatus(PullStatus.NoData);
            }
            else
            {
                SetPullStatus(PullStatus.Completed);
            }
            StateHasChanged();
            await Task.Delay(800);
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
            moveDistance = 0;
            wrapperStyle = "";
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
            if (moveDistance != moveDist)
            {
                moveDistance = moveDist;
                if (Direction == PullDirection.Down)
                {
                    wrapperStyle = $"transform: translate3d(0, {moveDist}px, 0);";
                }
                else
                {
                    wrapperStyle = $"transform: translate3d(0, -{moveDist}px, 0);";
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
