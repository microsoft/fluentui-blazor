// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Focus extension methods for <see cref="IFluentComponentAutofocus"/> and <see cref="FluentComponentBase"/>.
/// </summary>
internal static class FluentComponentAutofocusExtensions
{
    /// <summary>
    /// Applies autofocus on the first render when enabled for the component.
    /// </summary>
    /// <param name="component">The component requesting autofocus.</param>
    /// <param name="firstRender">Whether this is the component's first render.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal static async Task ApplyAutofocusAsync(this IFluentComponentAutofocus component, bool firstRender)
    {
        ArgumentNullException.ThrowIfNull(component);

        if (firstRender && component.Autofocus)
        {
            await component.FocusAsync();
        }
    }

    /// <summary>
    /// Calls the JavaScript focus helper for the component's element.
    /// </summary>
    /// <param name="component">The component requesting focus.</param>
    /// <param name="jsRuntime">The JavaScript runtime.</param>
    /// <param name="id">The ID of the element to focus.</param>
    /// <param name="preventScroll">Whether to prevent scrolling the element into view.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal static Task FocusElementAsync(this IFluentComponentAutofocus component, IJSRuntime jsRuntime, string? id, bool preventScroll)
    {
        ArgumentNullException.ThrowIfNull(component);

        return FocusElementAsync(jsRuntime, id, preventScroll);
    }

    /// <summary>
    /// Focuses the component's element using its JavaScript runtime and ID.
    /// </summary>
    /// <param name="component">The component requesting focus.</param>
    /// <param name="preventScroll">Whether to prevent scrolling the element into view.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal static Task FocusElementAsync(this FluentComponentBase component, bool preventScroll)
    {
        ArgumentNullException.ThrowIfNull(component);

        return FocusElementAsync(component.JSRuntime, component.Id, preventScroll);
    }

    private static Task FocusElementAsync(IJSRuntime jsRuntime, string? id, bool preventScroll)
    {
        ArgumentNullException.ThrowIfNull(jsRuntime);

        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Component ID cannot be null or whitespace.", nameof(id));
        }

        return jsRuntime.InvokeFluentVoidAsync("Microsoft.FluentUI.Blazor.Utilities.DomFunctions.focus", id, preventScroll)
                        .AsTask();
    }
}
