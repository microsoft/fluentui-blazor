using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;


namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public class ToastService : IToastService, IDisposable
{
    private readonly NavigationManager _navigationManager;

    public ToastService(NavigationManager navigationManager)
        : this(navigationManager, new ToastGlobalOptions())
    {
    }

    public ToastService(NavigationManager navigationManager, ToastGlobalOptions commonOptions)
    {
        _navigationManager = navigationManager;
        Configuration = commonOptions;

        _navigationManager.LocationChanged += NavigationManager_LocationChanged;
    }

    public event Action? OnToastUpdated;

    private ReaderWriterLockSlim ToastLock { get; } = new ReaderWriterLockSlim();

    private IList<Toast> ToastList { get; } = new List<Toast>();

    public ToastGlobalOptions Configuration { get; }

    public virtual IEnumerable<Toast> ShownToasts
    {
        get
        {
            ToastLock.EnterReadLock();
            try
            {
                return ToastList.Take(Configuration.MaxToasts);
            }
            finally
            {
                ToastLock.ExitReadLock();
            }
        }
    }

    public virtual Toast Add(string message)
    {
        return Add(options =>
        {
            options.Message = message;
        });
    }

    public virtual Toast Add(string message, ToastIntent severity)
    {
        return Add(options =>
        {
            options.Message = message;
            options.Intent = severity;
        });
    }

    public virtual Toast Add(Action<ToastOptions> options)
    {
        var configuration = new ToastOptions(Configuration);
        options.Invoke(configuration);

        var toast = new Toast(configuration);

        ToastLock.EnterWriteLock();
        try
        {
            toast.OnClose += RemoveToast;
            ToastList.Add(toast);
        }
        finally
        {
            ToastLock.ExitWriteLock();
        }

        OnToastUpdated?.Invoke();

        return toast;
    }

    public virtual void Clear()
    {
        ToastLock.EnterWriteLock();
        try
        {
            RemoveAllToasts(ToastList);
        }
        finally
        {
            ToastLock.ExitWriteLock();
        }

        OnToastUpdated?.Invoke();
    }

    public virtual void RemoveToast(Toast toast)
    {
        toast.Dispose();
        toast.OnClose -= RemoveToast;
        toast.State.Options.OnClose?.Invoke(toast);

        ToastLock.EnterWriteLock();
        try
        {
            var index = ToastList.IndexOf(toast);
            if (index < 0)
            {
                return;
            }

            ToastList.RemoveAt(index);
        }
        finally
        {
            ToastLock.ExitWriteLock();
        }

        OnToastUpdated?.Invoke();
    }

    //private void ConfigurationUpdated()
    //{
    //    OnToastUpdated?.Invoke();
    //}

    private void NavigationManager_LocationChanged(object? sender, LocationChangedEventArgs e)
    {
        if (Configuration.ClearAfterNavigation)
        {
            Clear();
        }
        else
        {
            ShownToasts.Where(s => s.State.Options.ClearAfterNavigation).ToList().ForEach(s => RemoveToast(s));
        }
    }

    public virtual void Dispose()
    {
        _navigationManager.LocationChanged -= NavigationManager_LocationChanged;
        RemoveAllToasts(ToastList);
    }

    private void RemoveAllToasts(IEnumerable<Toast> toasts)
    {
        if (ToastList.Count == 0)
        {
            return;
        }

        foreach (var toast in toasts)
        {
            toast.OnClose -= RemoveToast;
            toast.Dispose();
        }

        ToastList.Clear();
    }
}
