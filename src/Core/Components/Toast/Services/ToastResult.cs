namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public class ToastResult
{
    /// <summary />
    protected internal ToastResult(object? data, bool cancelled)
    {
        Data = data;
        Cancelled = cancelled;
    }

    /// <summary />
    public object? Data { get; set; }

    /// <summary />
    public bool Cancelled { get; }

    /// <summary />
    public static ToastResult Ok<T>(T result) => new(result, false);

    /// <summary />
    public static ToastResult Cancel(object? data = null) => new(data ?? default, true);
}
