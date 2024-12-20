// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public class DialogInstance : IDialogInstance
{
    internal readonly TaskCompletionSource<DialogResult> ResultCompletion = new();

    /// <summary />
    internal DialogInstance(IDialogService dialogService, Type componentType, DialogOptions options)
    {
        ComponentType = componentType;
        Options = options;
        DialogService = dialogService;
        Id = string.IsNullOrEmpty(options.Id) ? Identifier.NewId() : options.Id;
    }

    /// <summary />
    internal Type ComponentType { get; }

    /// <summary />
    internal IDialogService DialogService { get; }

    /// <summary />
    public DialogOptions Options { get; internal set; }

    /// <summary />
    public Task<DialogResult> Result => ResultCompletion.Task;

    /// <summary />
    public string Id { get; }

    /// <summary />
    public Task CloseAsync()
    {
        return DialogService.CloseAsync(this, DialogResult.Cancel());
    }

    /// <summary />
    public Task CloseAsync(DialogResult result)
    {
        return DialogService.CloseAsync(this, result);
    }
}
