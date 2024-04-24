namespace Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip;

/// <inheritdoc cref="ITooltipService"/>
public class TooltipService : ITooltipService, IDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TooltipService"/> class.
    /// </summary>
    public TooltipService()
        : this(new TooltipGlobalOptions())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TooltipService"/> class.
    /// </summary>
    /// <param name="options">Default global options</param>
    public TooltipService(TooltipGlobalOptions options)
    {
        GlobalOptions = options;
    }

    /// <inheritdoc cref="ITooltipService.OnTooltipUpdated"/>
    public event Action? OnTooltipUpdated;

    /// <inheritdoc cref="ITooltipService.GlobalOptions"/>
    public TooltipGlobalOptions GlobalOptions { get; }

    /// <inheritdoc cref="ITooltipService.Tooltips"/>
    public IEnumerable<TooltipOptions> Tooltips
    {
        get
        {
            TooltipLock.EnterReadLock();
            try
            {
                return TooltipList;
            }
            finally
            {
                TooltipLock.ExitReadLock();
            }
        }
    }

    /// <summary />
    private IList<TooltipOptions> TooltipList { get; } = new List<TooltipOptions>();

    /// <summary />
    private ReaderWriterLockSlim TooltipLock { get; } = new ReaderWriterLockSlim();

    /// <inheritdoc cref="ITooltipService.Add(TooltipOptions)"/>
    public void Add(TooltipOptions options)
    {
        TooltipLock.EnterWriteLock();
        try
        {
            TooltipList.Add(options);
        }
        finally
        {
            TooltipLock.ExitWriteLock();
        }

        OnTooltipUpdated?.Invoke();
    }

    /// <inheritdoc cref="ITooltipService.Clear"/>
    public void Clear()
    {
        TooltipLock.EnterWriteLock();
        try
        {
            TooltipList.Clear();
        }
        finally
        {
            TooltipLock.ExitWriteLock();
        }

        OnTooltipUpdated?.Invoke();
    }

    /// <inheritdoc cref="ITooltipService.Refresh"/>
    public void Refresh()
    {
        OnTooltipUpdated?.Invoke();
    }

    /// <summary>
    /// Clears all tooltips from the service.
    /// </summary>
    public void Dispose()
    {
        Clear();
    }

    /// <inheritdoc cref="ITooltipService.Remove(Guid)"/>
    public void Remove(Guid value)
    {
        TooltipLock.EnterWriteLock();
        try
        {
            var item = TooltipList.FirstOrDefault(i => i.Id == value);
            if (item != null)
            {
                TooltipList.Remove(item);
            }
        }
        finally
        {
            TooltipLock.ExitWriteLock();
        }

        OnTooltipUpdated?.Invoke();
    }
}
