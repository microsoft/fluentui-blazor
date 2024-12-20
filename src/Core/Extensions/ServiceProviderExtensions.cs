// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

internal static class ServiceProviderExtensions
{
    /// <summary>
    /// Gets a value indicating whether the provider was added by the user and is available.
    /// </summary>
    public static bool ProviderNotAvailable<TComponent>(this IFluentServiceBase<TComponent> provider)
    {
        return string.IsNullOrEmpty(provider.ProviderId);
    }
}
