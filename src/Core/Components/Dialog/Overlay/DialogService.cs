// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class DialogService : IDialogService
{
    private bool _isGlobalOverlayRegistered;

    /// <summary>
    /// Identifier of the global overlay rendered by the <see cref="FluentDialogProvider"/>.
    /// </summary>
    internal const string GlobalOverlayId = "global-overlay";

    /// <summary>
    /// Key used to store the <see cref="OverlayOptions"/> of the global overlay in the Parameters of the dialog instance.
    /// </summary>
    internal const string GlobalOverlayOptionsKey = "Options";

    /// <see cref="IDialogService.ShowOverlayAsync(Action{OverlayOptions})"/>
    public virtual async Task ShowOverlayAsync(Action<OverlayOptions>? options = null)
    {
        if (this.ProviderNotAvailable())
        {
            throw new FluentServiceProviderException<FluentDialogProvider>();
        }

        if (!_isGlobalOverlayRegistered)
        {
            throw new InvalidOperationException("The global overlay is disabled in the library configuration. To enable it, set the UseGlobalOverlay property to true in the LibraryConfiguration.");
        }

        // Get the current options for the global overlay.
        var overlayOptions = new OverlayOptions();
        options?.Invoke(overlayOptions);
        var overlayInstance = GetGlobalOverlayInstance(overlayOptions);

        // Update the dialog instance for the global overlay in the service provider,
        // so it is rendered by the FluentDialogProvider with the new options.
        ServiceProvider.Items.AddOrUpdate(GlobalOverlayId, overlayInstance, (key, oldValue) => overlayInstance);
        await ServiceProvider.OnUpdatedAsync.Invoke(overlayInstance);

        // Show the global overlay using JS interop.
        await _jsRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Overlay.Show", GlobalOverlayId);
    }

    /// <see cref="IDialogService.HideOverlayAsync()"/>
    public virtual async Task HideOverlayAsync()
    {
        if (this.ProviderNotAvailable())
        {
            throw new FluentServiceProviderException<FluentDialogProvider>();
        }

        if (!_isGlobalOverlayRegistered)
        {
            throw new InvalidOperationException("The global overlay is disabled in the library configuration. To enable it, set the UseGlobalOverlay property to true in the LibraryConfiguration.");
        }

        // Hide the global overlay using JS interop.
        await _jsRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Overlay.Close", GlobalOverlayId);
    }

    /// <summary>
    /// Registers the global overlay component as a dialog instance in the service provider,
    /// so it can be rendered by the <see cref="FluentDialogProvider"/>.
    /// This method is called in the constructor of the DialogService, to ensure the global overlay is available as soon as the service is created.
    /// </summary>
    private void RegisterGlobalOverlayComponent()
    {
        _isGlobalOverlayRegistered = true;

        // Register the global overlay as a dialog instance so it is rendered by the FluentDialogProvider.
        var overlayInstance = GetGlobalOverlayInstance(new OverlayOptions());
        ServiceProvider.Items.TryAdd(overlayInstance.Id, overlayInstance);
    }

    /// <summary>
    /// Gets the global overlay dialog instance for the given options.
    /// </summary>
    private DialogInstance GetGlobalOverlayInstance(OverlayOptions options)
    {
        return new DialogInstance(this, typeof(FluentOverlay), new DialogOptions()
        {
            Id = GlobalOverlayId,
            Parameters = new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                { GlobalOverlayOptionsKey, options },
            },
        });
    }
}
