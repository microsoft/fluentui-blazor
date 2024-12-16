// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public class DialogReference : IDialogReference
{
    private readonly IDialogService _dialogService;
    private readonly TaskCompletionSource<DialogResult> _resultCompletion = new();

    /// <summary />
    public DialogReference(Guid dialogInstanceId, DialogInstance dialogInstance, IDialogService dialogService)
    {
        DialogInstanceId = dialogInstanceId;
        Instance = dialogInstance;
        _dialogService = dialogService;
    }

    /// <summary />
    internal Guid DialogInstanceId { get; }

    /// <summary />
    public DialogInstance Instance { get; set; }

    /// <summary />
    public Task<DialogResult> Result => _resultCompletion.Task;

    /// <summary />
    public Task CloseAsync()
    {
        return _dialogService.CloseAsync(this, DialogResult.Cancel());
    }

    /// <summary />
    public Task CloseAsync(DialogResult result)
    {
        return _dialogService.CloseAsync(this, result);
    }
}
