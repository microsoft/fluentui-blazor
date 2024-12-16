// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class DialogService : FluentServiceBase<FluentDialog>, IDialogService
{
    /// <summary />
    public Task CloseAsync(DialogReference dialog, DialogResult result)
    {
        return Task.CompletedTask;
    }

    /// <summary />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0025:Implement the functionality instead of throwing NotImplementedException", Justification = "Under development")]
    public Task<IDialogReference> ShowDialogAsync<TData>(Type dialogComponent, TData data, DialogParameters parameters) where TData : class
    {
        throw new NotImplementedException();
    }
}
