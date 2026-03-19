// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// 
/// </summary>
public abstract class FluentToastInstance : ComponentBase
{
    /// <summary>
    /// Gets or sets the Toast instance.
    /// </summary>
    [CascadingParameter]
    public virtual required IToastInstance ToastInstance { get; set; }

    /// <summary>
    /// Gets or sets the localizer.
    /// </summary>
    [Inject]
    public virtual required IFluentLocalizer Localizer { get; set; }

    /// <summary>
    /// Method invoked when an action is clicked.
    ///
    /// Override this method if you will perform an asynchronous operation
    /// when the user clicks an action button.
    /// </summary>
    /// <returns></returns>
    protected abstract Task OnActionClickedAsync(bool primary);
}
