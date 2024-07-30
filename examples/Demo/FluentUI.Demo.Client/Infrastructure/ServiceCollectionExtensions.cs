// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.Client;

/// <summary />
public static class ServiceCollectionExtensions
{
    /// <summary />
    public class DemoServices
    {
        private readonly IServiceCollection _services;

        /// <summary />
        internal DemoServices(IServiceCollection services)
        {
            _services = services;
        }

        /// <summary>
        /// Add common client services required by the Fluent UI Web Components for Blazor library
        /// </summary>
        public IServiceCollection ForClient()
        {
            _services.AddHttpClient<IStaticAssetService, HttpBasedStaticAssetService>();
            
            // _services.AddSingleton<CacheStorageAccessor>();
            // _services.AddSingleton<DemoNavProvider>();

            return _services;
        }

        /// <summary>
        /// Add common server services required by the Fluent UI Web Components for Blazor library
        /// </summary>
        public IServiceCollection ForServer()
        {
            _services.AddHttpClient<IStaticAssetService, ServerStaticAssetService>();

            // _services.AddSingleton<DemoNavProvider>();
            // _services.AddScoped<CacheStorageAccessor>();

            return _services;
        }
    }

    /// <summary>
    /// Add common client services required by the Fluent UI Web Components for Blazor library
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns></returns>
    public static DemoServices AddFluentUIDemoServices(this IServiceCollection services)
    {
        return new DemoServices(services);
    }
}
