// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public sealed class DialogInstance
{
    /// <summary />
    public DialogInstance(Type? type, DialogParameters parameters)
    {
        ContentType = type;
        Parameters = parameters;
    }

    /// <summary />
    public Type? ContentType { get; }

    /// <summary />
    public DialogParameters Parameters { get; internal set; }  
}
