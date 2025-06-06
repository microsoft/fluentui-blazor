// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Infrastructure;

/// <summary>
/// Service for managing and applying externalized default values for component parameters.
/// Supports both static defaults and instance-based defaults for multi-tenancy scenarios.
/// </summary>
public sealed class FluentDefaultValuesService
{
    private static readonly ConcurrentDictionary<string, Dictionary<string, object?>> _defaultValues = new();
    private static readonly ConcurrentDictionary<string, ComponentDefaultsInfo> _componentCache = new();
    private static readonly HashSet<string> _componentTypesWithoutDefaults = new();
    private static volatile bool _initialized = false;
    private static readonly object _lock = new();
    
    /// <summary>
    /// Configuration for controlling which assemblies/namespaces to scan for defaults
    /// </summary>
    public static FluentDefaultsScanConfiguration ScanConfiguration { get; } = new();

    /// <summary>
    /// Cached information about a component's default-applicable properties.
    /// </summary>
    private sealed class ComponentDefaultsInfo
    {
        public required Dictionary<string, PropertyInfo> ParameterProperties { get; init; }
        public required Dictionary<string, object?> DefaultValues { get; init; }
    }

    /// <summary>
    /// Configuration class for controlling default values scanning behavior
    /// </summary>
    public sealed class FluentDefaultsScanConfiguration
    {
        /// <summary>
        /// Gets or sets the specific assemblies to scan for defaults. If empty, scans all loaded assemblies.
        /// Use this to optimize performance by limiting scanning to specific assemblies.
        /// </summary>
        public List<Assembly> TargetAssemblies { get; set; } = new();

        /// <summary>
        /// Gets or sets the namespace prefixes to scan within assemblies. If empty, scans all namespaces.
        /// Use this to optimize performance by limiting scanning to specific namespaces.
        /// </summary>
        public List<string> TargetNamespaces { get; set; } = new();

        /// <summary>
        /// Gets or sets whether to scan for static defaults classes. Default is true.
        /// </summary>
        public bool ScanStaticDefaults { get; set; } = true;
        
        /// <summary>
        /// Gets or sets instance-based default providers for multi-tenancy scenarios.
        /// These providers are called at runtime to get current default values.
        /// </summary>
        public List<IFluentDefaultProvider> InstanceProviders { get; set; } = new();
    }

    /// <summary>
    /// Interface for providing instance-based default values, useful for multi-tenancy scenarios
    /// where defaults may vary per tenant or context.
    /// </summary>
    public interface IFluentDefaultProvider
    {
        /// <summary>
        /// Gets the default value for the specified component type and parameter name.
        /// </summary>
        /// <param name="componentTypeName">The component type name</param>
        /// <param name="parameterName">The parameter name</param>
        /// <returns>The default value, or null if no default is provided</returns>
        object? GetDefaultValue(string componentTypeName, string parameterName);

        /// <summary>
        /// Determines if this provider has a default value for the specified component and parameter.
        /// </summary>
        /// <param name="componentTypeName">The component type name</param>
        /// <param name="parameterName">The parameter name</param>
        /// <returns>True if a default value is available</returns>
        bool HasDefaultValue(string componentTypeName, string parameterName);
    }

    /// <summary>
    /// Initializes the default values service by scanning for FluentDefault attributes.
    /// </summary>
    public static void Initialize()
    {
        if (_initialized)
            return;

        lock (_lock)
        {
            if (_initialized)
                return;

            ScanForDefaultValues();
            _initialized = true;
        }
    }

    /// <summary>
    /// Applies default values to the specified component if any matching defaults are found.
    /// Optimized to minimize reflection overhead and supports both static and instance-based defaults.
    /// </summary>
    /// <param name="component">The component to apply defaults to.</param>
    [UnconditionalSuppressMessage("Trimming", "IL2070", Justification = "FluentDefault properties are preserved via DynamicDependency attributes")]
    public static void ApplyDefaults(ComponentBase component)
    {
        if (!_initialized)
            Initialize();

        var componentTypeName = component.GetType().Name;

        // Fast path: if we know this component type has no defaults, skip entirely
        if (_componentTypesWithoutDefaults.Contains(componentTypeName))
            return;

        // Check if we have cached info for this component type
        if (!_componentCache.TryGetValue(componentTypeName, out var componentInfo))
        {
            // Build cache for this component type
            componentInfo = BuildComponentCache(component.GetType(), componentTypeName);
            if (componentInfo == null)
            {
                // No defaults for this component type
                _componentTypesWithoutDefaults.Add(componentTypeName);
                return;
            }
        }

        // Apply static defaults using cached reflection info
        ApplyStaticDefaults(component, componentInfo);
        
        // Apply instance-based defaults if any are configured
        ApplyInstanceDefaults(component, componentInfo, componentTypeName);
    }

    private static void ApplyStaticDefaults(ComponentBase component, ComponentDefaultsInfo componentInfo)
    {
        foreach (var (propertyName, defaultValue) in componentInfo.DefaultValues)
        {
            if (!componentInfo.ParameterProperties.TryGetValue(propertyName, out var property))
                continue;

            // Only set default if the current value is null or the default value for the type
            var currentValue = property.GetValue(component);
            if (IsDefaultValue(currentValue, property.PropertyType))
            {
                try
                {
                    property.SetValue(component, defaultValue);
                }
                catch
                {
                    // Silently ignore type conversion errors
                }
            }
        }
    }

    private static void ApplyInstanceDefaults(ComponentBase component, ComponentDefaultsInfo componentInfo, string componentTypeName)
    {
        if (ScanConfiguration.InstanceProviders.Count == 0)
            return;

        foreach (var provider in ScanConfiguration.InstanceProviders)
        {
            foreach (var propertyName in componentInfo.ParameterProperties.Keys)
            {
                if (!provider.HasDefaultValue(componentTypeName, propertyName))
                    continue;

                var property = componentInfo.ParameterProperties[propertyName];
                var currentValue = property.GetValue(component);
                
                if (IsDefaultValue(currentValue, property.PropertyType))
                {
                    try
                    {
                        var defaultValue = provider.GetDefaultValue(componentTypeName, propertyName);
                        property.SetValue(component, defaultValue);
                    }
                    catch
                    {
                        // Silently ignore type conversion errors
                    }
                }
            }
        }
    }

    [UnconditionalSuppressMessage("Trimming", "IL2070", Justification = "Component parameter properties are preserved via component usage")]
    private static ComponentDefaultsInfo? BuildComponentCache(Type componentType, string componentTypeName)
    {
        // Check if we have any static defaults for this component type
        var hasStaticDefaults = _defaultValues.TryGetValue(componentTypeName, out var defaults) && defaults?.Count > 0;
        
        // Check if we have any instance providers that might have defaults
        var hasInstanceDefaults = ScanConfiguration.InstanceProviders.Any(p => 
            componentType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(prop => prop.CanWrite && prop.GetCustomAttributes<ParameterAttribute>().Any())
                        .Any(prop => p.HasDefaultValue(componentTypeName, prop.Name)));

        if (!hasStaticDefaults && !hasInstanceDefaults)
            return null;

        // Build cache of parameter properties for this component type
        var parameterProperties = new Dictionary<string, PropertyInfo>();
        var properties = componentType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        foreach (var property in properties)
        {
            if (property.CanWrite && property.GetCustomAttributes<ParameterAttribute>().Any())
            {
                parameterProperties[property.Name] = property;
            }
        }

        var componentInfo = new ComponentDefaultsInfo
        {
            ParameterProperties = parameterProperties,
            DefaultValues = defaults ?? new Dictionary<string, object?>()
        };

        _componentCache.TryAdd(componentTypeName, componentInfo);
        return componentInfo;
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Assembly scanning for FluentDefault attributes is controlled via configuration")]
    [UnconditionalSuppressMessage("Trimming", "IL2070", Justification = "Types with FluentDefault attributes should be preserved via DynamicDependency")]
    private static void ScanForDefaultValues()
    {
        if (!ScanConfiguration.ScanStaticDefaults)
            return;

        var assembliesToScan = ScanConfiguration.TargetAssemblies.Count > 0 
            ? ScanConfiguration.TargetAssemblies 
            : AppDomain.CurrentDomain.GetAssemblies();
        
        foreach (var assembly in assembliesToScan)
        {
            try
            {
                ScanAssemblyForDefaults(assembly);
            }
            catch
            {
                // Silently ignore assemblies that can't be scanned
            }
        }
    }

    [UnconditionalSuppressMessage("Trimming", "IL2070", Justification = "Static classes with FluentDefault attributes should be preserved")]
    private static void ScanAssemblyForDefaults(Assembly assembly)
    {
        var types = assembly.GetTypes().Where(t => t.IsClass && t.IsAbstract && t.IsSealed); // static classes

        foreach (var type in types)
        {
            // If target namespaces are specified, filter by namespace
            if (ScanConfiguration.TargetNamespaces.Count > 0 && 
                !ScanConfiguration.TargetNamespaces.Any(ns => type.Namespace?.StartsWith(ns, StringComparison.Ordinal) == true))
                continue;

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Static);
            
            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<FluentDefaultAttribute>();
                if (attribute == null)
                    continue;

                var componentTypeName = attribute.ComponentTypeName;
                var propertyName = attribute.ParameterName ?? property.Name;
                var defaultValue = property.GetValue(null);

                _defaultValues.AddOrUpdate(
                    componentTypeName,
                    _ => new Dictionary<string, object?> { { propertyName, defaultValue } },
                    (_, existing) =>
                    {
                        existing[propertyName] = defaultValue;
                        return existing;
                    }
                );
            }
        }
    }

    private static bool IsDefaultValue(object? value, Type propertyType)
    {
        if (value == null)
            return true;

        // Handle nullable value types
        var underlyingType = Nullable.GetUnderlyingType(propertyType);
        if (underlyingType != null)
        {
            // For nullable types, null is considered the default
            return false; // If we got here, value is not null, so it's been explicitly set
        }

        if (!propertyType.IsValueType)
            return false; // Reference types (non-null) are considered explicitly set

        var defaultValue = Activator.CreateInstance(propertyType);
        return Equals(value, defaultValue);
    }

    /// <summary>
    /// Gets all registered default values for debugging purposes.
    /// </summary>
    internal static IReadOnlyDictionary<string, Dictionary<string, object?>> GetAllDefaults()
    {
        if (!_initialized)
            Initialize();

        return _defaultValues.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    /// <summary>
    /// Clears all cached default values. Primarily for testing purposes.
    /// </summary>
    internal static void Reset()
    {
        lock (_lock)
        {
            _defaultValues.Clear();
            _componentCache.Clear();
            _componentTypesWithoutDefaults.Clear();
            _initialized = false;
        }
    }
}

/// <summary>
/// Extension methods for configuring FluentDefault behavior.
/// </summary>
public static class FluentDefaultConfigurationExtensions
{
    /// <summary>
    /// Configures the FluentDefault system to scan only the specified assemblies.
    /// This can significantly improve startup performance by limiting the scope of reflection scanning.
    /// </summary>
    /// <param name="configuration">The scan configuration</param>
    /// <param name="assemblies">The assemblies to scan for FluentDefault attributes</param>
    /// <returns>The configuration for method chaining</returns>
    public static FluentDefaultValuesService.FluentDefaultsScanConfiguration WithTargetAssemblies(
        this FluentDefaultValuesService.FluentDefaultsScanConfiguration configuration,
        params Assembly[] assemblies)
    {
        configuration.TargetAssemblies.AddRange(assemblies);
        return configuration;
    }

    /// <summary>
    /// Configures the FluentDefault system to scan only the specified namespaces.
    /// This can improve startup performance by limiting the scope of type scanning within assemblies.
    /// </summary>
    /// <param name="configuration">The scan configuration</param>
    /// <param name="namespaces">The namespace prefixes to scan</param>
    /// <returns>The configuration for method chaining</returns>
    public static FluentDefaultValuesService.FluentDefaultsScanConfiguration WithTargetNamespaces(
        this FluentDefaultValuesService.FluentDefaultsScanConfiguration configuration,
        params string[] namespaces)
    {
        configuration.TargetNamespaces.AddRange(namespaces);
        return configuration;
    }

    /// <summary>
    /// Adds an instance-based default provider for multi-tenancy scenarios.
    /// </summary>
    /// <param name="configuration">The scan configuration</param>
    /// <param name="provider">The default provider to add</param>
    /// <returns>The configuration for method chaining</returns>
    public static FluentDefaultValuesService.FluentDefaultsScanConfiguration WithInstanceProvider(
        this FluentDefaultValuesService.FluentDefaultsScanConfiguration configuration,
        FluentDefaultValuesService.IFluentDefaultProvider provider)
    {
        configuration.InstanceProviders.Add(provider);
        return configuration;
    }

    /// <summary>
    /// Disables static defaults scanning for scenarios where only instance-based defaults are needed.
    /// </summary>
    /// <param name="configuration">The scan configuration</param>
    /// <returns>The configuration for method chaining</returns>
    public static FluentDefaultValuesService.FluentDefaultsScanConfiguration WithoutStaticDefaults(
        this FluentDefaultValuesService.FluentDefaultsScanConfiguration configuration)
    {
        configuration.ScanStaticDefaults = false;
        return configuration;
    }
}