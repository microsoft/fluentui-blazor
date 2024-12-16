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
        : base($"{typeof(TProvider).Name} needs to be added to the main layout of your application/site.")
    {

    }
}
