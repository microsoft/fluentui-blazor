using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public class FocusManager : IFocusManager
{
    private readonly IJSRuntime _jsRuntime;
    private IJSObjectReference? _jsModule;

    public FocusManager(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    /// <inheritdoc />
    public async Task SetFocusParametersAsync(
        ElementReference element,
        ArrowNavigationGroupParameters? arrowNavigationGroup = null,
        FocusableElementParameters? focusableElement = null,
        FocusableGroupParameters? focusableGroup = null,
        FocusRestoreParameters? focusRestore = null,
        ModalParameters? modal = null,
        FocusTrackingParameters? tracking = null,
        bool update = false)
    {
        if (modal is not null &&
            modal.GroupName is null)
        {
            modal.GroupName = $"modal-{Identifier.NewId()}";
        }

        var parameters =
            new
            {
                arrowNavigationGroup,
                focusableElement,
                focusableGroup,
                focusRestore,
                modal,
                tracking,
                update
            };

        await EnsureJsModuleInitialized();
        await _jsModule!.InvokeVoidAsync("setFocusManagementParameters", element, parameters);
    }

    /// <summary />
    private async Task EnsureJsModuleInitialized()
    {
        _jsModule ??= await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Microsoft.FluentUI.AspNetCore.Components/Microsoft.FluentUI.AspNetCore.Components.lib.module.js");
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_jsModule is not null)
        {
            await _jsModule.DisposeAsync();
            _jsModule = null;
        }
    }
}
