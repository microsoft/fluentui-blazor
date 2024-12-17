// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public class DialogResult
{
    /// <summary />
    protected internal DialogResult(object? content, bool cancelled)
    {
        Value = content;
        Cancelled = cancelled;
    }

    /// <summary />
    public object? Value { get; set; }

    /// <summary />
    public bool Cancelled { get; }

    /// <summary />
    public static DialogResult Ok<T>(T result) => new(result, cancelled: false);

    /// <summary />
    public static DialogResult Cancel(object? content = null) => new(content ?? default, cancelled: true);
}
