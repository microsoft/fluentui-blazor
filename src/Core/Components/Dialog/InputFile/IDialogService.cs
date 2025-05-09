// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial interface IDialogService
{
    /// <summary>
    /// Registers a new <see cref="FluentInputFile">element</see> based on the trigger element.
    /// </summary>
    /// <param name="elementId">HTML element identifier</param>
    /// <param name="onCompletedAsync">Callback to be invoked when the file upload is completed</param>
    /// <param name="options">Options for the <see cref="FluentInputFile">element</see></param>
    /// <returns></returns>
    Task RegisterInputFileAsync(string elementId, Func<IEnumerable<FluentInputFileEventArgs>, Task> onCompletedAsync, Action<InputFileOptions>? options = null);
}
