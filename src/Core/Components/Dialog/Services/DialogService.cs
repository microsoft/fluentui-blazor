// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class DialogService : FluentServiceBase<FluentDialog>, IDialogService
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider"></param>
    public DialogService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary />
    public Task CloseAsync(DialogReference dialog, DialogResult result)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IDialogService.ShowDialogAsync(Type, DialogParameters)"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "MA0004:Use Task.ConfigureAwait", Justification = "<Pending>")]
    public virtual async Task<IDialogReference> ShowDialogAsync(Type dialogComponent, DialogParameters parameters)
    {
        if (!typeof(IDialogContentComponent).IsAssignableFrom(dialogComponent))
        {
            throw new ArgumentException($"{dialogComponent.FullName} must be a Dialog Component", nameof(dialogComponent));
        }

        if (this.ProviderNotAvailable())
        {
            throw new FluentServiceProviderException<FluentDialogProvider>();
        }

        var dialogInstance = new DialogInstance(dialogComponent, parameters);
        var dialogReference = new DialogReference(dialogInstance, this);
        var dialog = new FluentDialog(_serviceProvider, this, dialogInstance);

        InternalService.Items.TryAdd(dialog?.Id ?? "", dialog ?? throw new InvalidOperationException("Failed to create FluentDialog."));

        await InternalService.OnUpdatedAsync.Invoke(dialog);

        //await dialog.ShowAsync();

        return dialogReference;
        //throw new InvalidOperationException("Hello");
        //IDialogReference? dialogReference = new DialogReference(parameters.Id, this);
        //return await OnShowAsync.Invoke(dialogReference, dialogComponent, parameters, data);
    }

    /// <summary />
    public Task<IDialogReference> ShowDialogAsync<TDialog>(DialogParameters parameters) where TDialog : IDialogContentComponent
    {
        return ShowDialogAsync(typeof(TDialog), parameters);
    }
}
