// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Reflection;
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
        // Add HttpClient for the "CatchAll.razor" or "DocViewer.razor" page
        services.AddHttpClient();

        // Documentation services
        services.AddDocViewer(options =>
        {
            options.PageTitle = "{0} - FluentUI Blazor Components";
            options.ComponentsAssembly = typeof(Client._Imports).Assembly;
            options.ResourcesAssembly = typeof(Client._Imports).Assembly;
            options.ApiAssembly = typeof(Microsoft.FluentUI.AspNetCore.Components._Imports).Assembly;
            options.ApiCommentSummary = (data, component, member) =>
            {
                if (member is null && (data is null || data?.Items?.Count <= 1))
                {
                    return "⚠️ The file `api-comments.json` was not found. " +
                           "Run the project `FluentUI.Demo.DocApiGen` to generate the file. ";
                }

                var sectionName = component.Name;

                // Component summary
                if (member is null)
                {
                    return "TODO";
                }

                // Member summary
                else
                {
                    var signature = member.GetSignature();
                    if (data?.Items?.TryGetValue(sectionName, out var section) == true)
                    {
                        if (section.TryGetValue(signature, out var summary))
                        {
                            return summary;
                        }
                    }
                }

                return "";
            };
            options.SourceCodeUrl = "/sources/{0}.txt";
        });

        return new DemoServices(services);
    }

    /// <summary>
    /// Get the signature of the method.
    /// </summary>
    /// <param name="member"></param>
    /// <remarks>⚠️ Must be identical to FluentUI.Demo.DocApiGen.Extensions.ReflectionExtensions.GetSignature</remarks>
    public static string GetSignature(this MemberInfo member)
    {
        return member.MemberType == MemberTypes.Method
             ? $"{member.Name}({string.Join(", ", ((MethodInfo)member).GetParameters().Select(p => p.ParameterType.Name))})"
             : member.Name;
    }
}
