using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Reflection.Metadata;
using System.Xml;

namespace BcdLib.Components;

public partial class PullUp
{
    [Inject]
    public DocumentJsInterop DocumentJs { get; set; } = default!;

    #region Parameter

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
        builder.AddContent(0, "上拉刷新");
    };

    [Parameter]
    public RenderFragment LoosingTip { get; set; } = builder =>
    {
        builder.AddContent(0, "释放更新");
    };

    [Parameter]
    public RenderFragment LoadingTip { get; set; } = builder =>
    {
        builder.AddContent(0, "更新中...");
    };

    [Parameter]
    public RenderFragment CompletedTip { get; set; } = builder =>
    {
        builder.AddContent(0, "更新完成");
    };

    [Parameter]
    public RenderFragment NoDataTip { get; set; } = builder =>
    {
        builder.AddContent(0, "暂无更多数据");
    };

    [Parameter]
    public int MaxDistance { get; set; } = 50;
    
    #endregion

    private PullUpStatus pullStatus = PullUpStatus.Awaiting;

    private RenderFragment GetTipHtml()
    {
        var renderFragment = pullStatus switch
        {
            PullUpStatus.Loosing => LoosingTip,
            PullUpStatus.Loading => LoadingTip,
            PullUpStatus.Completed => CompletedTip,
            PullUpStatus.NoData => NoDataTip,
            _ => PullingTip,
        };
        return renderFragment;
    }


    private string GetWrapperStyle()
    {
        return pullStatus switch
        {
            PullUpStatus.Awaiting => "",
            _ => wrapperStyle,
        };
    }



    private double startY = 0;
    private int moveDistance = 0;
    private string wrapperStyle = "";

    private void OnTouchStart(TouchEventArgs e)
    {
        if (this.pullStatus == PullUpStatus.NoData)
        {
            return;
        }
        if (this.pullStatus == PullUpStatus.Awaiting
            || this.pullStatus == PullUpStatus.Completed
        )
        {
            this.SetPullStatus(PullUpStatus.Pulling);
            // 获取初始y轴位置
            this.startY = e.TargetTouches[0].ClientY;
            // 触摸开始时，动画时间，移动距离归0
            this.moveDistance = 0;
        }
    }


    private async Task OnTouchMove(TouchEventArgs e)
    {
        if (this.pullStatus == PullUpStatus.Pulling || this.pullStatus == PullUpStatus.Loosing)
        {
            var distToBottom = await DocumentJs.GetScrollDistToBottomAsync();
            if (distToBottom > 0)
            {
                return;
            }

            var move =  this.startY - e.TargetTouches[0].ClientY;
            // Only a positive number means that the user has pulled down.
            if (move > 0)
            {
                this.SetDistance(CalcMoveDistance(move));
            }
        }
    }

    private async Task OnTouchEnd(TouchEventArgs e)
    {
        if (this.pullStatus == PullUpStatus.Loosing)
        {
            this.SetPullStatus(PullUpStatus.Loading);

            #region This part cannot be placed in SetPullStatus, otherwise async will lead to state confusion
            bool hasMoreData = true;
            if (OnRefreshing != null)
            {
                try
                {
                    hasMoreData = await OnRefreshing(); 
                }
                catch (Exception)
                {
                    this.SetDistance(-1);
                    throw;
                }
            }
#if DEBUG
            else
            {
                await Task.Delay(1000);
            }
#endif
            #endregion

            this.wrapperStyle = $"transform: translate3d(0, 0, 0);";
            if (!hasMoreData)
            {
                this.SetPullStatus(PullUpStatus.NoData);
            }
            else
            {
                this.SetPullStatus(PullUpStatus.Completed);
            }
            StateHasChanged();
            await Task.Delay(1000);
            this.SetDistance(-1);

        }
        else if (this.pullStatus == PullUpStatus.Awaiting || this.pullStatus == PullUpStatus.Pulling)
        {
            this.SetDistance(-1);
        }
    }

    private int CalcMoveDistance(double moveDist)
    {
        // Simulated resistance
        return (int)Math.Pow(moveDist, 0.8);
    }

    private void SetDistance(int moveDist)
    {
        if (moveDist < 0)
        {
            this.SetPullStatus(PullUpStatus.Awaiting);
            this.moveDistance = 0;
            wrapperStyle = "";
            StateHasChanged();
        }
        else
        {
            if (moveDist < MaxDistance)
            {
                this.SetPullStatus(PullUpStatus.Pulling);
            }
            else
            {
                this.SetPullStatus(PullUpStatus.Loosing);
                moveDist = MaxDistance;
            }
            if (this.moveDistance != moveDist)
            {
                this.moveDistance = moveDist;
                wrapperStyle = $"transform: translate3d(0, -{moveDist}px, 0);";
                StateHasChanged();
            }
        }
    }

    private void SetPullStatus(PullUpStatus newPullStatus)
    {
        if (this.pullStatus != newPullStatus)
        {
            this.pullStatus = newPullStatus;
        }
    }
}
