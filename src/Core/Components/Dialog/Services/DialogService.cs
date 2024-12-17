// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

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
    public Task CloseAsync(DialogInstance dialog, DialogResult result)
    {
        // Remove the HTML code from the DialogProvider
        ServiceProvider.Items.TryRemove(dialog.Id, out _);

        // Set the result of the dialog
        dialog.ResultCompletion.SetResult(result);

        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IDialogService.ShowDialogAsync(Type, DialogParameters)"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "MA0004:Use Task.ConfigureAwait", Justification = "TODO")]
    public virtual async Task<IDialogInstance> ShowDialogAsync(Type componentType, DialogParameters parameters)
    {
        if (!componentType.IsSubclassOf(typeof(ComponentBase)))
        {
            throw new ArgumentException($"{componentType.FullName} must be a Blazor Component", nameof(componentType));
        }

        if (this.ProviderNotAvailable())
        {
            throw new FluentServiceProviderException<FluentDialogProvider>();
        }

        var instance = new DialogInstance(this, componentType, parameters);
        var dialog = new FluentDialog(_serviceProvider, instance);

        // Add the dialog to the service, and render it.
        ServiceProvider.Items.TryAdd(dialog?.Id ?? "", dialog ?? throw new InvalidOperationException("Failed to create FluentDialog."));
        await ServiceProvider.OnUpdatedAsync.Invoke(dialog);

        return instance;
    }

    /// <summary />
    public Task<IDialogInstance> ShowDialogAsync<TDialog>(DialogParameters parameters) where TDialog : ComponentBase
    {
        return ShowDialogAsync(typeof(TDialog), parameters);
    }
}
