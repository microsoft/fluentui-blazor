// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Exception thrown when a service provider is not available.
/// </summary>
public class FluentServiceProviderException<TProvider> : Exception
{
    /// <summary>
    /// Creates a new instance of the <see cref="FluentServiceProviderException{TProvider}"/> class.
    /// </summary>
    public FluentServiceProviderException()
        : base($"{typeof(TProvider).Name} needs to be added to the page/component hierarchy of your application/site. Usually this will be 'MainLayout' but depending on your setup it could be at a different location.")
    {

    }
}
