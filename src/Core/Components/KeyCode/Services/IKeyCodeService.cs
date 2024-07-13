// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Service registered by <see cref="ServiceCollectionExtensions.AddFluentUIComponents(Microsoft.Extensions.DependencyInjection.IServiceCollection, Microsoft.FluentUI.AspNetCore.Components.LibraryConfiguration?)"/>
/// to catch all keys events and notify all registered listeners.
/// </summary>
public interface IKeyCodeService
{
    /// <summary>
    /// Gets the list of all active keystroke listeners.
    /// </summary>
    IEnumerable<IKeyCodeListener> Listeners { get; }

    /// <summary>
    /// Register the listener component or page, and returns a unique identifier.
    /// </summary>
    /// <param name="listener"></param>
    /// <returns></returns>
    Guid RegisterListener(IKeyCodeListener listener);

    /// <summary>
    /// Register the <paramref name="handler"/> method as a listener, and returns a unique identifier.
    /// </summary>
    /// <param name="handler"></param>
    /// <returns></returns>
    Guid RegisterListener(Func<FluentKeyCodeEventArgs, Task> handler);

    /// <summary>
    /// Register the <paramref name="handlerKeyDown"/> and <paramref name="handlerKeyUp"/> methods as a listener, and returns a unique identifier.
    /// </summary>
    /// <param name="handlerKeyDown"></param>
    /// <param name="handlerKeyUp"></param>
    /// <returns></returns>
    Guid RegisterListener(Func<FluentKeyCodeEventArgs, Task> handlerKeyDown, Func<FluentKeyCodeEventArgs, Task> handlerKeyUp);

    /// <summary>
    /// Unregister the listener component or page.
    /// </summary>
    /// <param name="listener"></param>
    void UnregisterListener(IKeyCodeListener listener);

    /// <summary>
    /// Unregister the listener method.
    /// </summary>
    /// <param name="handler"></param>
    void UnregisterListener(Func<FluentKeyCodeEventArgs, Task> handler);

    /// <summary>
    /// Unregister the listener method.
    /// </summary>
    /// <param name="handlerKeyDown"></param>
    /// <param name="handlerKeyUp"></param>
    void UnregisterListener(Func<FluentKeyCodeEventArgs, Task> handlerKeyDown, Func<FluentKeyCodeEventArgs, Task> handlerKeyUp);

    /// <summary>
    /// Unregister all listeners.
    /// </summary>
    void Clear();
}
