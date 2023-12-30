using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public interface IFocusManager : IAsyncDisposable
{
    /// <summary>
    /// Sets focus management parameters on target <see cref="ElementReference"/>
    /// </summary>
    /// <param name="element">Target <see cref="ElementReference"/></param>
    /// <param name="arrowNavigationGroup">parameters of arrow navigation inside group</param>
    /// <param name="focusableElement">general focus-related parameters</param>
    /// <param name="focusableGroup">focusable group parameters</param>
    /// <param name="focusRestore">focus restore parameters</param>
    /// <param name="modal">parameters for creation of modal dialog like experience</param>
    /// <param name="tracking">parameters for tracking focus using data attributes</param>
    /// <param name="update">defines whether applied parameters will update any existing parameters or replace them</param>
    /// <returns></returns>
    Task SetFocusParametersAsync(
        ElementReference element,
        ArrowNavigationGroupParameters? arrowNavigationGroup = null,
        FocusableElementParameters? focusableElement = null,
        FocusableGroupParameters? focusableGroup = null,
        FocusRestoreParameters? focusRestore = null,
        ModalParameters? modal = null,
        FocusTrackingParameters? tracking = null,
        bool update = false);
}