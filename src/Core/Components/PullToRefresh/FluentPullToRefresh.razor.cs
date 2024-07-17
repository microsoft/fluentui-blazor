// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
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
    private bool _originalShowStaticTip;
    //private bool _showStaticTip = true;
    private bool _internalShowStaticTip = true;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-pull-container")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .Build();

    /// <summary />
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the direction to pull the <see cref="ChildContent"/>.
    /// </summary>
    [Parameter]
    public PullDirection Direction { get; set; } = PullDirection.Down;

    /// <summary>
    /// Gets or sets if the pull action is disabled.
    /// Deaults to false.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets if the component should work on non-touch devices (by using an emulation script).
    /// Deaults to true.
    /// </summary>
    [Parameter]
    public bool EmulateTouch { get; set; } = true;

    /// <summary>
    /// Gets or sets if a tip is shown when <see cref="ChildContent"/> is not being pulled.
    /// </summary>
    [Parameter]
    public bool ShowStaticTip { get; set; } = true;

    /// <summary>
    /// Gets or set the content to show.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Returns whether there is more data available.
    /// </summary>
    [Parameter]
    public Func<Task<bool>>? OnRefreshAsync { get; set; }

    /// <summary>
    /// Gets or sets the the content to indicate the <see cref="ChildContent"/> can be refreshed by a pull down/up action.
    /// </summary>
    [Parameter]
    public RenderFragment? PullingTemplate { get; set; }

    /// <summary>
    /// Gets or sets the the content to indicate the pulled <see cref="ChildContent"/> must be released to start the refresh action.
    /// </summary>
    [Parameter]
    public RenderFragment ReleaseTemplate { get; set; } = builder =>
    {
        builder.OpenComponent(0, typeof(FluentIcon<Icon>));
        builder.AddAttribute(1, "Value", new CoreIcons.Regular.Size24.ArrowSyncCircle());
        builder.CloseComponent();
    };

    /// <summary>
    /// Gets or sets the the content to indicate the <see cref="ChildContent"/> is being refreshed.
    /// </summary>
    [Parameter]
    public RenderFragment LoadingTemplate { get; set; } = builder =>
    {
        builder.OpenComponent(0, typeof(FluentProgressRing));
        builder.AddAttribute(1, "Stroke", ProgressStroke.Small);
        builder.AddAttribute(2, "Width", "20px");
        builder.CloseComponent();
    };

    /// <summary>
    /// Gets or sets the the content to indicate the <see cref="ChildContent"/> has been refreshed.
    /// </summary>
    [Parameter]
    public RenderFragment CompletedTemplate { get; set; } = builder =>
    {
        builder.OpenComponent(0, typeof(FluentIcon<Icon>));
        builder.AddAttribute(1, "Value", new CoreIcons.Regular.Size24.CheckmarkCircle());
        builder.CloseComponent();
    };

    /// <summary>
    /// Gets or sets the the content to indicate the <see cref="ChildContent"/> can not be refreshed anymore.
    /// </summary>
    [Parameter]
    public RenderFragment NoDataTemplate { get; set; } = builder =>
    {
        builder.AddContent(0, "No more data");
    };

    /// <summary>
    /// Gets or sets the distance the <see cref="ChildContent"/> needs to be pulled (in pixels) to initiate a refresh action.
    /// </summary>
    [Parameter]
    public int DragDistance { get; set; } = 32;

    /// <summary>
    /// Gets or sets the height (in pixels) of the tip fragment (if shown).
    /// </summary>
    [Parameter]
    public int TipHeight { get; set; } = 32;

    /// <summary>
    /// Gets or sets the amount of time (in milliseconds) a status update message will be displayed.
    /// Default is 750
    /// </summary>
    [Parameter]
    public int StatusUpdateMessageTimeout { get; set; } = 750;

    /// <summary>
    /// Gets or sets the threshold distance the <see cref="ChildContent"/> needs to be pulled (in pixels) to start the tip pull action.
    /// </summary>
    [Parameter]
    public int DragThreshold { get; set; } = 0;

    protected override void OnInitialized()
    {
        _originalShowStaticTip = _internalShowStaticTip = ShowStaticTip;

        PullingTemplate ??= builder =>
        {
            builder.OpenComponent(0, typeof(FluentIcon<Icon>));
            if (Direction == PullDirection.Down)
            {
                builder.AddAttribute(1, "Value", new CoreIcons.Regular.Size24.ArrowCircleDown());
            }
            else
            {
                builder.AddAttribute(1, "Value", new CoreIcons.Regular.Size24.ArrowCircleUp());
            }
            builder.CloseComponent();

        };

    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && EmulateTouch)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));

            await _jsModule.InvokeVoidAsync("initTouchEmulator");
        }
    }
    private RenderFragment? GetTipContent()
    {
        var renderFragment = _pullStatus switch
        {
            PullStatus.WaitingForRelease => ReleaseTemplate,
            PullStatus.Loading => LoadingTemplate,
            PullStatus.Completed => CompletedTemplate,
            PullStatus.NoData => NoDataTemplate,
            _ => PullingTemplate,
        };
        return renderFragment;
    }

    protected virtual string? WrapperStyle => new StyleBuilder()
         .AddStyle("position", "relative")
         .AddStyle("user-select", "none")
         .AddStyle("height", "100%")
         .AddStyle(_pullStatus == PullStatus.Awaiting ? null : _wrapperStyle)
         .Build();

    private Task OnTouchStartAsync(TouchEventArgs e)
    {
        if (Disabled) { return Task.CompletedTask; }

        if (_pullStatus == PullStatus.Awaiting || _pullStatus == PullStatus.Completed)
        {
            SetPullStatus(PullStatus.Pulling);
            _startY = e.TargetTouches[0].ClientY;
            _moveDistance = 0;
        }
        return Task.CompletedTask;
    }

    private async Task OnTouchMoveAsync(TouchEventArgs e)
    {
        if (Disabled) { return; }

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
        if (_jsModule is not null)
        {
            var distToTop = await _jsModule.InvokeAsync<int>("getScrollDistToTop");

            if (distToTop > 0)
            {
                return;
            }

            var move = e.TargetTouches[0].ClientY - (_startY + DragThreshold);

            if (move > 0)
            {
                _internalShowStaticTip = true;
                SetDistance(CalcMoveDistance(move));
            }
        }
    }

    private Task OnTouchMoveUpAsync(TouchEventArgs e)
    {
        var move = _startY - (e.TargetTouches[0].ClientY + DragThreshold);

        if (move > 0)
        {
            _internalShowStaticTip = true;
            SetDistance(CalcMoveDistance(move));
        }
        return Task.CompletedTask;

    }

    private async Task OnTouchEndAsync(TouchEventArgs e)
    {
        if (Disabled) { return; }

        if (_pullStatus == PullStatus.WaitingForRelease)
        {
            SetPullStatus(PullStatus.Loading);

            var hasMoreData = true;
            if (OnRefreshAsync is not null)
            {
                try
                {
                    hasMoreData = await OnRefreshAsync.Invoke();
                }
                catch (Exception)
                {
                    SetDistance(-1);
                    throw;
                }
            }

            _wrapperStyle = $"transform: translate3d(0, 0, 0);";
            if (!hasMoreData)
            {
                SetPullStatus(PullStatus.NoData);
                await Task.Delay(StatusUpdateMessageTimeout);
            }
            else
            {
                SetPullStatus(PullStatus.Completed);
                StateHasChanged();
                await Task.Delay(StatusUpdateMessageTimeout);
                _internalShowStaticTip = _originalShowStaticTip;
            }

            SetDistance(-1);
        }
        else if (_pullStatus == PullStatus.Awaiting || _pullStatus == PullStatus.Pulling)
        {
            SetDistance(-1);
            _internalShowStaticTip = _originalShowStaticTip;
        }
    }

    private static int CalcMoveDistance(double moveDist)
    {
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
            if (moveDist < DragDistance)
            {
                SetPullStatus(PullStatus.Pulling);
            }
            else
            {
                SetPullStatus(PullStatus.WaitingForRelease);
                moveDist = DragDistance;
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
