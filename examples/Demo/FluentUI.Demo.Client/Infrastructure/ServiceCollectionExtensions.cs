// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer;

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
        // Add HttpClient for the "CatchAll.razor" page
        services.AddHttpClient();

        // Documentation services
        services.AddDocViewer(options =>
        {
            options.PageTitle = "{0} - FluentUI Blazor Components";
            options.ComponentsAssembly = typeof(Client._Imports).Assembly;
            options.ResourcesAssembly = typeof(Client._Imports).Assembly;
            options.ApiAssembly = typeof(Microsoft.FluentUI.AspNetCore.Components._Imports).Assembly;
            options.SourceCodeUrl = "/sources/{0}.razor.txt";
        });

        return new DemoServices(services);
    }
}
