// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Dialog.SplashScreen;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class DialogService : IDialogService
{
    /// <inheritdoc cref="IDialogService.ShowSplashScreenAsync(Type, SplashScreenOptions)"/>
    public virtual async Task<ISplashScreenInstance> ShowSplashScreenAsync([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type dialogComponent, SplashScreenOptions options)
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
        options.Parameters["SplashScreen"] = options.SplashScreen;

        var instance = new SplashScreenInstance(this, dialogComponent, options);

        // Add the dialog to the service, and render it, without waiting for the dialog to be closed.
        if (!ServiceProvider.Items.TryAdd(instance.Id, instance))
        {
            throw new InvalidOperationException($"A dialog with the ID '{instance.Id}' is already displayed.");
        }

        await ServiceProvider.OnUpdatedAsync.Invoke(instance);

        return instance;
    }

    /// <inheritdoc cref="IDialogService.ShowSplashScreenAsync{TDialog}(SplashScreenOptions)"/>
    public Task<ISplashScreenInstance> ShowSplashScreenAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TDialog>(SplashScreenOptions options) where TDialog : ComponentBase
    {
        return ShowSplashScreenAsync(typeof(TDialog), options);
    }

    /// <inheritdoc cref="IDialogService.ShowSplashScreenAsync{TDialog}(Action{SplashScreenOptions})"/>
    public Task<ISplashScreenInstance> ShowSplashScreenAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TDialog>(Action<SplashScreenOptions>? options = null) where TDialog : ComponentBase
    {
        return ShowSplashScreenAsync(typeof(TDialog), options is null ? new SplashScreenOptions() : new SplashScreenOptions(options));
    }

    /// <inheritdoc cref="IDialogService.ShowSplashScreenAsync(Action{SplashScreenOptions})"/>
    public Task<ISplashScreenInstance> ShowSplashScreenAsync(Action<SplashScreenContent> content)
    {
        ArgumentNullException.ThrowIfNull(content);

        return ShowSplashScreenAsync<DefaultSplashScreenDialog>(new SplashScreenOptions(content));
    }

    /// <inheritdoc cref="IDialogService.UpdateSplashScreenAsync(ISplashScreenInstance, Action{SplashScreenContent})"/>
    Task IDialogService.UpdateSplashScreenAsync(ISplashScreenInstance splashScreen, Action<SplashScreenContent> content)
    {
        ArgumentNullException.ThrowIfNull(splashScreen);
        ArgumentNullException.ThrowIfNull(content);

        // The splash screen is already closed: nothing to update.
        if (!ServiceProvider.Items.ContainsKey(splashScreen.Id))
        {
            return Task.CompletedTask;
        }

        var splashScreenOptions = (SplashScreenOptions)splashScreen.Options;
        content.Invoke(splashScreenOptions.SplashScreen);

        return ServiceProvider.OnUpdatedAsync.Invoke(splashScreen);
    }
}
