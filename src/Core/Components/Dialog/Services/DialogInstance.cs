// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public sealed class DialogInstance
{
    /// <summary />
    public DialogInstance(Type? type, DialogParameters parameters, object content)
    {
        ContentType = type;
        Parameters = parameters;
        Content = content;
    }

    /// <summary />
    public Type? ContentType { get; }

    /// <summary />
    public object Content { get; internal set; } = default!;

    /// <summary />
    public DialogParameters Parameters { get; internal set; }

    /// <summary />
    internal Dictionary<string, object>? GetParameterDictionary()
    {
        if (Content is null)
        {
            return null;
        }

        return new Dictionary<string, object>(StringComparer.Ordinal) { { "Content", Content } };
    }
}
