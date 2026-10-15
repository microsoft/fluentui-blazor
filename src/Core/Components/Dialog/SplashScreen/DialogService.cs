// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class DialogService : IDialogService
{
    /// <inheritdoc cref="IDialogService.ShowSplashScreenAsync(Type, DialogOptions)"/>
    public virtual async Task<ISplashScreenInstance> ShowSplashScreenAsync([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type dialogComponent, DialogOptions options)
    {
        ArgumentNullException.ThrowIfNull(dialogComponent);
        ArgumentNullException.ThrowIfNull(options);

        if (!dialogComponent.IsSubclassOf(typeof(ComponentBase)))
        {
            throw new ArgumentException($"{dialogComponent.FullName} must be a Blazor Component", nameof(dialogComponent));
        }

        if (this.ProviderNotAvailable())
        {
            throw new FluentServiceProviderException<FluentDialogProvider>();
        }

        options.IsDrawer ??= false;
        options.PreventDismissOnEscape ??= true;

        var instance = new SplashScreenInstance(this, dialogComponent, options);

        // Add the dialog to the service, and render it, without waiting for the dialog to be closed.
        if (!ServiceProvider.Items.TryAdd(instance.Id, instance))
        {
            throw new InvalidOperationException($"A dialog with the ID '{instance.Id}' is already displayed.");
        }

        await ServiceProvider.OnUpdatedAsync.Invoke(instance);

        return instance;
    }

    /// <inheritdoc cref="IDialogService.ShowSplashScreenAsync{TDialog}(DialogOptions)"/>
    public Task<ISplashScreenInstance> ShowSplashScreenAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TDialog>(DialogOptions options) where TDialog : ComponentBase
    {
        return ShowSplashScreenAsync(typeof(TDialog), options);
    }

    /// <inheritdoc cref="IDialogService.ShowSplashScreenAsync{TDialog}(Action{DialogOptions})"/>
    public Task<ISplashScreenInstance> ShowSplashScreenAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TDialog>(Action<DialogOptions>? options = null) where TDialog : ComponentBase
    {
        return ShowSplashScreenAsync(typeof(TDialog), options is null ? new DialogOptions() : new DialogOptions(options));
    }

    /// <inheritdoc cref="IDialogService.UpdateSplashScreenAsync(ISplashScreenInstance, Action{DialogOptions})"/>
    public virtual async Task UpdateSplashScreenAsync(ISplashScreenInstance splashScreen, Action<DialogOptions> options)
    {
        ArgumentNullException.ThrowIfNull(splashScreen);
        ArgumentNullException.ThrowIfNull(options);

        // The splash screen is already closed: nothing to update.
        if (!ServiceProvider.Items.ContainsKey(splashScreen.Id))
        {
            return;
        }

        options.Invoke(splashScreen.Options);

        await ServiceProvider.OnUpdatedAsync.Invoke(splashScreen);
    }
}
