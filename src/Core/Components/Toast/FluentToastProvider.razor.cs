// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentToastProvider : FluentComponentBase
{
    private const int _defaultMaxToastCount = 4;
    private const int _defaultTimeout = 7000;
    private const int _defaultVerticalOffset = 16;
    private const int _defaultHorizontalOffset = 20;

    /// <summary />
    public FluentToastProvider(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the maximum number of toasts displayed at the same time.
    /// </summary>
    [Parameter]
    public int MaxToastCount { get; set; } = _defaultMaxToastCount;

    /// <summary>
    /// Gets or sets the default timeout duration in milliseconds for visible toasts.
    /// </summary>
    [Parameter]
    public int Timeout { get; set; } = _defaultTimeout;

    /// <summary>
    /// Gets or sets the default toast position.
    /// </summary>
    [Parameter]
    public ToastPosition? Position { get; set; }

    /// <summary>
    /// Gets or sets the default vertical offset in pixels.
    /// </summary>
    [Parameter]
    public int VerticalOffset { get; set; } = _defaultVerticalOffset;

    /// <summary>
    /// Gets or sets the default horizontal offset in pixels.
    /// </summary>
    [Parameter]
    public int HorizontalOffset { get; set; } = _defaultHorizontalOffset;

    /// <summary>
    /// Gets or sets a value indicating whether visible toasts pause timeout while hovered.
    /// </summary>
    [Parameter]
    public bool PauseOnHover { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether visible toasts pause timeout while the window is blurred.
    /// </summary>
    [Parameter]
    public bool PauseOnWindowBlur { get; set; }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-toast-provider")
        .Build();

    /// <summary />
    internal string? StyleValue => DefaultStyleBuilder
        .AddStyle("z-index", ZIndex.Toast.ToString(CultureInfo.InvariantCulture))
        .Build();

    /// <summary>
    /// Gets or sets the injected service provider.
    /// </summary>
    [Inject]
    public IServiceProvider? ServiceProvider { get; set; }

    /// <summary />
    protected virtual IToastService? ToastService => GetCachedServiceOrNull<IToastService>();

    /// <summary />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (ToastService is not null)
        {
            ToastService.ProviderId = Id;
            ToastService.OnUpdatedAsync = async (item) =>
            {
                SynchronizeToastQueue();
                await InvokeAsync(StateHasChanged);
            };

            SynchronizeToastQueue();
        }
    }

    /// <summary />
    internal static Action<ToastEventArgs> EmptyOnStatusChange => (_) => { };

    private EventCallback<ToastEventArgs> GetOnStatusChangeCallback(IToastInstance toast)
        => EventCallback.Factory.Create<ToastEventArgs>(this, toast.Options.OnStatusChange ?? EmptyOnStatusChange);

    private int GetTimeout(IToastInstance toast)
        => toast.Options.Timeout ?? Timeout;

    private ToastPosition? GetPosition(IToastInstance toast)
        => toast.Options.Position ?? Position;

    private int GetVerticalOffset(IToastInstance toast)
        => toast.Options.VerticalOffset ?? VerticalOffset;

    private int GetHorizontalOffset(IToastInstance toast)
        => toast.Options.HorizontalOffset ?? HorizontalOffset;

    private bool GetPauseOnHover(IToastInstance toast)
        => toast.Options.PauseOnHover ?? PauseOnHover;

    private bool GetPauseOnWindowBlur(IToastInstance toast)
        => toast.Options.PauseOnWindowBlur ?? PauseOnWindowBlur;

    private IEnumerable<IToastInstance> GetRenderedToasts()
        => ToastService?.Items.Values
            .Where(toast => toast.Status is ToastStatus.Visible or ToastStatus.Dismissed)
            .OrderByDescending(toast => toast.Index)
            ?? Enumerable.Empty<IToastInstance>();

    private void SynchronizeToastQueue()
    {
        if (ToastService is null)
        {
            return;
        }

        var maxToastCount = MaxToastCount <= 0 ? _defaultMaxToastCount : MaxToastCount;
        var activeCount = ToastService.Items.Values.Count(toast => toast.Status is ToastStatus.Visible or ToastStatus.Dismissed);
        var queuedToasts = ToastService.Items.Values
            .Where(toast => toast.Status == ToastStatus.Queued)
            .OrderBy(toast => toast.Index)
            .ToList();

        foreach (var toast in queuedToasts)
        {
            if (activeCount >= maxToastCount)
            {
                break;
            }

            if (toast is ToastInstance instance)
            {
                instance.Status = ToastStatus.Visible;
                toast.Options.OnStatusChange?.Invoke(new ToastEventArgs(instance, ToastStatus.Visible));
                activeCount++;
            }
        }
    }
}
