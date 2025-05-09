// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components.Forms;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class DialogService : IDialogService
{
    /// <see cref="IDialogService.RegisterInputFileAsync(string, Func{IEnumerable{FluentInputFileEventArgs}, Task}, Action{InputFileOptions}?)"/>
    public virtual async Task<InputFileInstance> RegisterInputFileAsync(string elementId, Func<IEnumerable<FluentInputFileEventArgs>, Task> onCompletedAsync, Action<InputFileOptions>? options = null)
    {
        if (this.ProviderNotAvailable())
        {
            throw new FluentServiceProviderException<FluentDialogProvider>();
        }

        // Options
        var config = new InputFileOptions();
        options?.Invoke(config);

        // Register the dialog
        var instance = new DialogInstance(this, typeof(InputFile), new DialogOptions()
        {
            // These parameters are passed to the `FluentDialogProvider` component
            Parameters = new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                { "ElementId", elementId },
                { "OnCompletedAsync", onCompletedAsync },
                { "OnFileUploadedAsync", config.OnFileUploadedAsync },
                { "OnProgressChangeAsync", config.OnProgressChangeAsync },
                { "OnFileErrorAsync", config.OnFileErrorAsync },
                { "Options", config},
            },
        });

        var fileInstance = new InputFileInstance(ServiceProvider, instance, elementId);

        // Add the dialog to the service, and render it.
        ServiceProvider.Items.TryAdd(fileInstance.Id, instance ?? throw new InvalidOperationException("Failed to register an InputFile."));
        await ServiceProvider.OnUpdatedAsync.Invoke(instance);

        return fileInstance;
    }
}
