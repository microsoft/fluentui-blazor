namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public class DialogResult
{
    /// <summary />
    protected internal DialogResult(object? data, bool cancelled)
    {
        Data = data;
        Cancelled = cancelled;
    }

    /// <summary />
    public object? Data { get; set; }

    /// <summary />
    public bool Cancelled { get; }

    /// <summary />
    public static DialogResult Ok<T>(T result) => new(result, false);

    /// <summary />
    public static DialogResult Cancel(object? data = null) => new(data ?? default, true);
}
