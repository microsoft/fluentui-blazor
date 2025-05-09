// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components.Forms;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class DialogService : IDialogService
{
    internal const string InputFileElementId = "ElementId";
    internal const string InputFileOnCompleted = "OnCompleted";
    internal const string InputFileOptions = "Options";

    /// <see cref="IDialogService.RegisterInputFileAsync(string, Func{IEnumerable{FluentInputFileEventArgs}, Task}, Action{InputFileOptions}?)"/>
    public virtual async Task RegisterInputFileAsync(string elementId, Func<IEnumerable<FluentInputFileEventArgs>, Task> onCompletedAsync, Action<InputFileOptions>? options = null)
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
            Parameters = new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                { InputFileElementId, elementId },
                { InputFileOnCompleted, onCompletedAsync },
                { InputFileOptions, config},
            },
        });

        // Add the dialog to the service, and render it.
        ServiceProvider.Items.TryAdd(instance?.Id ?? "", instance ?? throw new InvalidOperationException("Failed to register an InputFile."));
        await ServiceProvider.OnUpdatedAsync.Invoke(instance);
    }
}
