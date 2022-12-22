﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Fast.Components.FluentUI.DesignTokens;
using Microsoft.Fast.Components.FluentUI.Infrastructure;

namespace Microsoft.Fast.Components.FluentUI;

public static class ServiceCollectionExtensions
{
    public static void AddFluentUIComponents(this IServiceCollection services)
    {
        services.AddScoped<GlobalState>();
        services.AddScoped<CacheStorageAccessor>();
        services.AddScoped<StaticAssetService>();
        services.AddHttpClient("staticassetservice");
        services.AddDesignTokens();
    }
}