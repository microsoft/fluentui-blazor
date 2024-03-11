using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Xml;
using static Microsoft.FluentUI.AspNetCore.Components.CoreIcons.Regular.Size24;

namespace BcdLib.Components;

public partial class PullDown
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/SortableList/FluentSortableList.razor.js";

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public EventCallback OnRefreshing { get; set; }

    [Parameter]
    public RenderFragment PullingTip { get; set; } = builder =>
    {
        builder.AddContent(0, "下拉刷新");
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
    public int MaxDistance { get; set; } = 50;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            
            IJSObjectReference? module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            await module.InvokeAsync<string>("init", Id, Group, Clone ? "clone" : null, Drop, Sort, Handle ? ".sortable-grab" : null, Filter, Fallback, _selfReference);
        }
    }
    private PullDownStatus pullStatus = PullDownStatus.Awaiting;

    private RenderFragment GetTipHtml()
    {
        var renderFragment = pullStatus switch
        {
            PullDownStatus.Loosing => LoosingTip,
            PullDownStatus.Loading => LoadingTip,
            PullDownStatus.Completed => CompletedTip,
            _ => PullingTip,
        };
        return renderFragment;
    }

    private string GetWrapperStyle()
    {
        return pullStatus switch
        {
            PullDownStatus.Awaiting => "",
            _ => wrapperStyle,
        };
    }

    private double startY = 0;
    private int moveDistance = 0;
    private string wrapperStyle = "";
    
    private void OnTouchStart(TouchEventArgs e)
    {
        if (this.pullStatus == PullDownStatus.Awaiting
            || this.pullStatus == PullDownStatus.Completed
        )
        {
            this.SetPullStatus(PullDownStatus.Pulling);
            // 获取初始y轴位置
            this.startY = e.TargetTouches[0].ClientY;
            // 触摸开始时，动画时间，移动距离归0
            this.moveDistance = 0;
        }
    }


    private async Task OnTouchMove(TouchEventArgs e)
    {
        if (this.pullStatus == PullDownStatus.Pulling || this.pullStatus == PullDownStatus.Loosing)
        {
            // If document is a scroll bar, touch sliding is a simple way to scroll up and down the page
            var distToTop = await DocumentJs.GetScrollDistToTopAsync();
            if (distToTop > 0)
            {
                return;
            }

            var move = e.TargetTouches[0].ClientY - this.startY;
            // Only a positive number means that the user has pulled down.
            if (move > 0)
            {
                this.SetDistance(CalcMoveDistance(move));
            }
        }
    }

    private async Task OnTouchEnd(TouchEventArgs e)
    {
        if (this.pullStatus == PullDownStatus.Loosing)
        {
            this.SetPullStatus(PullDownStatus.Loading);

            #region This part cannot be placed in SetPullStatus, otherwise async will lead to state confusion

            if (OnRefreshing.HasDelegate)
            {
                try
                {
                    await OnRefreshing.InvokeAsync();
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


            this.SetPullStatus(PullDownStatus.Completed);
            await Task.Delay(800);
            this.SetDistance(-1);
        }
        else if (this.pullStatus == PullDownStatus.Awaiting || this.pullStatus == PullDownStatus.Pulling)
        {
            this.SetDistance(-1);
        }
    }

    private int CalcMoveDistance(double moveDist)
    {
        // Simulated resistance
        return (int) Math.Pow(moveDist, 0.8);
    }

    private void SetDistance(int moveDist)
    {
        if (moveDist < 0)
        {
            this.SetPullStatus(PullDownStatus.Awaiting);
            this.moveDistance = 0;
            wrapperStyle = "";
            StateHasChanged();
        }
        else
        {
            if (moveDist < MaxDistance)
            {
                this.SetPullStatus(PullDownStatus.Pulling);
            }
            else
            {
                this.SetPullStatus(PullDownStatus.Loosing);
                moveDist = MaxDistance;
            }
            if (this.moveDistance != moveDist)
            {
                this.moveDistance = moveDist;
                wrapperStyle = $"transform: translate3d(0, {moveDist}px, 0)";
                StateHasChanged();
            }
        }
    }

    private void SetPullStatus(PullDownStatus newPullStatus)
    {
        if (this.pullStatus != newPullStatus)
        {
            this.pullStatus = newPullStatus;
        }
    }
}
