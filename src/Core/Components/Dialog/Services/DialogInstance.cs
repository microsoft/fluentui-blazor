// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public class DialogInstance : IDialogInstance
{
    private readonly TaskCompletionSource<DialogResult> _resultCompletion = new();

    /// <summary />
    public DialogInstance(IDialogService dialogService, Type componentType, DialogParameters parameters)
    {
        ComponentType = componentType;
        Parameters = parameters;
        DialogService = dialogService;
    }

    /// <summary />
    internal Type ComponentType { get; }

    /// <summary />
    internal IDialogService DialogService { get; }

    /// <summary />
    public DialogParameters Parameters { get; internal set; }

    /// <summary />
    public Task<DialogResult> Result => _resultCompletion.Task;

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
