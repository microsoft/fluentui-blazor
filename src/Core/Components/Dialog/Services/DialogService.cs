// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Service for showing dialogs.
/// </summary>
public partial class DialogService : FluentServiceBase<IDialogInstance>, IDialogService
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DialogService"/> class.
    /// </summary>
    /// <param name="serviceProvider">List of services available in the application.</param>
    public DialogService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc cref="IDialogService.CloseAsync(IDialogInstance, DialogResult)"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "MA0004:Use Task.ConfigureAwait", Justification = "<Pending>")]
    public async Task CloseAsync(IDialogInstance dialog, DialogResult result)
    {
        // Remove the HTML code from the DialogProvider
        if (!ServiceProvider.Items.TryRemove(dialog.Id, out _))
        {
            throw new InvalidOperationException($"Failed to remove dialog from DialogProvider: the ID '{dialog.Id}' doesn't exist in the DialogServiceProvider.");
        }

        await ServiceProvider.OnUpdatedAsync.Invoke(dialog);

        // Set the result of the dialog
        (dialog as DialogInstance)?.ResultCompletion.SetResult(result);
    }

    /// <inheritdoc cref="IDialogService.ShowDialogAsync(Type, DialogOptions)"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "MA0004:Use Task.ConfigureAwait", Justification = "TODO")]
    public virtual async Task<IDialogInstance> ShowDialogAsync(Type componentType, DialogOptions options)
    {
        if (!componentType.IsSubclassOf(typeof(ComponentBase)))
        {
            throw new ArgumentException($"{componentType.FullName} must be a Blazor Component", nameof(componentType));
        }

        if (this.ProviderNotAvailable())
        {
            throw new FluentServiceProviderException<FluentDialogProvider>();
        }

        var instance = new DialogInstance(this, componentType, options);

        // Add the dialog to the service, and render it.
        ServiceProvider.Items.TryAdd(instance?.Id ?? "", instance ?? throw new InvalidOperationException("Failed to create FluentDialog."));
        await ServiceProvider.OnUpdatedAsync.Invoke(instance);

        return instance;
    }

    /// <inheritdoc cref="IDialogService.ShowDialogAsync{TDialog}(DialogOptions)"/>
    public Task<IDialogInstance> ShowDialogAsync<TDialog>(DialogOptions options) where TDialog : ComponentBase
    {
        return ShowDialogAsync(typeof(TDialog), options);
    }

    /// <inheritdoc cref="IDialogService.ShowDialogAsync{TDialog}(Action{DialogOptions})"/>
    public Task<IDialogInstance> ShowDialogAsync<TDialog>(Action<DialogOptions> options) where TDialog : ComponentBase
    {
        return ShowDialogAsync(typeof(TDialog), new DialogOptions(options));
    }
}
