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

    /// <inheritdoc cref="IDialogService.ShowDialogAsync(Type, object, DialogParameters)"/>
    public virtual Task<IDialogReference> ShowDialogAsync(Type dialogComponent, object data, DialogParameters parameters)
    {
        if (!typeof(IDialogContentComponent).IsAssignableFrom(dialogComponent))
        {
            throw new ArgumentException($"{dialogComponent.FullName} must be a Dialog Component", nameof(dialogComponent));
        }

        if (this.ProviderNotAvailable())
        {
            throw new FluentServiceProviderException<FluentDialogProvider>();
        }

        throw new InvalidOperationException("Hello");
        //IDialogReference? dialogReference = new DialogReference(parameters.Id, this);
        //return await OnShowAsync.Invoke(dialogReference, dialogComponent, parameters, data);
    }

    /// <summary />
    public Task<IDialogReference> ShowDialogAsync<TDialog>(object data, DialogParameters parameters) where TDialog : IDialogContentComponent
    {
        return ShowDialogAsync(typeof(TDialog), data, parameters);
    }
}
