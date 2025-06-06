// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Infrastructure;

namespace Microsoft.FluentUI.AspNetCore.Components.Samples;

/// <summary>
/// Example implementation of multi-tenant defaults using IFluentDefaultProvider
/// This approach avoids static properties and allows different defaults per tenant/context
/// </summary>
public class MultiTenantDefaultProvider : FluentDefaultValuesService.IFluentDefaultProvider
{
    private readonly ITenantService _tenantService;
    private readonly Dictionary<string, Dictionary<string, Dictionary<string, object?>>> _tenantDefaults;

    public MultiTenantDefaultProvider(ITenantService tenantService)
    {
        _tenantService = tenantService;
        _tenantDefaults = new Dictionary<string, Dictionary<string, Dictionary<string, object?>>>
        {
            ["tenant1"] = new()
            {
                ["FluentButton"] = new()
                {
                    ["Appearance"] = Appearance.Outline,
                    ["Class"] = "tenant1-button"
                },
                ["FluentTextInput"] = new()
                {
                    ["Class"] = "tenant1-input"
                }
            },
            ["tenant2"] = new()
            {
                ["FluentButton"] = new()
                {
                    ["Appearance"] = Appearance.Accent,
                    ["Class"] = "tenant2-button"
                }
            }
        };
    }

    public object? GetDefaultValue(string componentTypeName, string parameterName)
    {
        var tenantId = _tenantService.GetCurrentTenantId();
        
        if (_tenantDefaults.TryGetValue(tenantId, out var componentDefaults) &&
            componentDefaults.TryGetValue(componentTypeName, out var parameterDefaults) &&
            parameterDefaults.TryGetValue(parameterName, out var defaultValue))
        {
            return defaultValue;
        }

        return null;
    }

    public bool HasDefaultValue(string componentTypeName, string parameterName)
    {
        var tenantId = _tenantService.GetCurrentTenantId();
        
        return _tenantDefaults.TryGetValue(tenantId, out var componentDefaults) &&
               componentDefaults.TryGetValue(componentTypeName, out var parameterDefaults) &&
               parameterDefaults.ContainsKey(parameterName);
    }
}

/// <summary>
/// Example tenant service interface - implement based on your tenant identification strategy
/// </summary>
public interface ITenantService
{
    string GetCurrentTenantId();
}

/// <summary>
/// Example configuration for optimized scanning and multi-tenancy
/// </summary>
public static class OptimizedFluentDefaultsConfiguration
{
    /// <summary>
    /// Configure FluentDefaults for optimal performance and multi-tenancy support
    /// </summary>
    /// <param name="tenantService">The tenant service for identifying current tenant</param>
    public static void ConfigureOptimizedDefaults(ITenantService tenantService)
    {
        FluentDefaultValuesService.ScanConfiguration
            // Only scan your application assembly instead of all loaded assemblies
            .WithTargetAssemblies(typeof(OptimizedFluentDefaultsConfiguration).Assembly)
            // Only scan your defaults namespace instead of all namespaces
            .WithTargetNamespaces("YourApp.Defaults", "YourApp.Theming")
            // Add multi-tenant provider
            .WithInstanceProvider(new MultiTenantDefaultProvider(tenantService));
    }

    /// <summary>
    /// Configure for pure instance-based defaults (no static scanning)
    /// Use this in scenarios where static defaults are not desired
    /// </summary>
    /// <param name="tenantService">The tenant service for identifying current tenant</param>
    public static void ConfigureInstanceOnlyDefaults(ITenantService tenantService)
    {
        FluentDefaultValuesService.ScanConfiguration
            .WithoutStaticDefaults()
            .WithInstanceProvider(new MultiTenantDefaultProvider(tenantService));
    }
}