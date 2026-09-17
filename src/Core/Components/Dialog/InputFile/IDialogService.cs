// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial interface IDialogService
{
    /// <summary>
    /// Registers a new <see cref="FluentInputFile">element</see> based on the trigger element id.
    /// </summary>
    /// <param name="elementId">HTML element identifier.</param>
    /// <param name="onCompletedAsync">Callback to be invoked when the file upload is completed. Call `StateHasChanged` in your method to refresh your UI.</param>
    /// <param name="options">Options for the <see cref="FluentInputFile">element</see>.</param>
    /// <returns></returns>
    Task<InputFileInstance> RegisterInputFileAsync(string elementId, Func<IEnumerable<FluentInputFileEventArgs>, Task> onCompletedAsync, Action<InputFileOptions>? options = null);

    /// <summary>
    /// Unregisters the <see cref="FluentInputFile">element</see> based on the trigger element id.
    /// </summary>
    /// <param name="elementId"></param>
    Task UnregisterInputFileAsync(string elementId);
}
