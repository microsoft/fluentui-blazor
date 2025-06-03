// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Reflection;
using Microsoft.FluentUI.AspNetCore.Components.Infrastructure;

namespace Microsoft.FluentUI.AspNetCore.Components.Samples;

/// <summary>
/// Examples of optimized FluentDefault configurations for different application scenarios
/// </summary>
public static class OptimizedConfigurationExamples
{
    /// <summary>
    /// Configuration for small to medium applications with defaults in a single assembly
    /// </summary>
    public static void ConfigureForSingleAssemblyApp()
    {
        FluentDefaultValuesService.ScanConfiguration
            .WithTargetAssemblies(Assembly.GetExecutingAssembly())
            .WithTargetNamespaces("MyApp.Defaults");
    }

    /// <summary>
    /// Configuration for large enterprise applications with multiple assemblies
    /// </summary>
    public static void ConfigureForEnterpriseApp()
    {
        FluentDefaultValuesService.ScanConfiguration
            .WithTargetAssemblies(
                typeof(CoreComponentDefaults).Assembly,
                typeof(ThemeDefaults).Assembly,
                typeof(FeatureDefaults).Assembly
            )
            .WithTargetNamespaces(
                "MyCompany.UI.Defaults",
                "MyCompany.Theming",
                "MyCompany.Components.Defaults"
            );
    }

    /// <summary>
    /// Configuration for Blazor WASM applications with trimming concerns
    /// Uses instance providers to avoid reflection-based scanning
    /// </summary>
    public static void ConfigureForBlazorWASMWithTrimming()
    {
        FluentDefaultValuesService.ScanConfiguration
            .WithoutStaticDefaults()
            .WithInstanceProvider(new CompileTimeDefaultProvider());
    }

    /// <summary>
    /// Configuration for multi-tenant SaaS applications
    /// Combines base defaults with tenant-specific overrides
    /// </summary>
    /// <param name="tenantService">Service to identify current tenant</param>
    public static void ConfigureForMultiTenantSaaS(ITenantService tenantService)
    {
        FluentDefaultValuesService.ScanConfiguration
            // Base application defaults
            .WithTargetAssemblies(typeof(BaseAppDefaults).Assembly)
            .WithTargetNamespaces("MyApp.Defaults.Base")
            // Tenant-specific overrides
            .WithInstanceProvider(new TenantDefaultProvider(tenantService));
    }

    /// <summary>
    /// Configuration for applications with user customization
    /// Layers: Base App → Tenant → User Preferences
    /// </summary>
    /// <param name="tenantService">Service to identify current tenant</param>
    /// <param name="userService">Service to get user preferences</param>
    public static void ConfigureWithUserCustomization(ITenantService tenantService, IUserService userService)
    {
        FluentDefaultValuesService.ScanConfiguration
            .WithTargetAssemblies(typeof(BaseAppDefaults).Assembly)
            .WithTargetNamespaces("MyApp.Defaults")
            .WithInstanceProvider(new TenantDefaultProvider(tenantService))
            .WithInstanceProvider(new UserPreferencesDefaultProvider(userService));
    }
}

/// <summary>
/// Example compile-time default provider for Blazor WASM trimming scenarios
/// </summary>
public class CompileTimeDefaultProvider : FluentDefaultValuesService.IFluentDefaultProvider
{
    private static readonly Dictionary<(string ComponentType, string Parameter), object?> _defaults = new()
    {
        [("FluentButton", "Appearance")] = Appearance.Outline,
        [("FluentButton", "Class")] = "app-button",
        [("FluentTextInput", "Class")] = "app-input",
        [("FluentCard", "Class")] = "app-card",
        [("FluentDesignSystemProvider", "Direction")] = LocalizationDirection.LeftToRight,
    };

    public object? GetDefaultValue(string componentTypeName, string parameterName)
    {
        return _defaults.TryGetValue((componentTypeName, parameterName), out var value) ? value : null;
    }

    public bool HasDefaultValue(string componentTypeName, string parameterName)
    {
        return _defaults.ContainsKey((componentTypeName, parameterName));
    }
}

/// <summary>
/// Example user preferences provider that could load from user settings, database, etc.
/// </summary>
public class UserPreferencesDefaultProvider : FluentDefaultValuesService.IFluentDefaultProvider
{
    private readonly IUserService _userService;

    public UserPreferencesDefaultProvider(IUserService userService)
    {
        _userService = userService;
    }

    public object? GetDefaultValue(string componentTypeName, string parameterName)
    {
        var preferences = _userService.GetUserPreferences();
        
        return (componentTypeName, parameterName) switch
        {
            ("FluentButton", "Appearance") when preferences.PreferredButtonStyle != null => preferences.PreferredButtonStyle,
            ("FluentDesignSystemProvider", "Direction") when preferences.PreferredDirection != null => preferences.PreferredDirection,
            _ => null
        };
    }

    public bool HasDefaultValue(string componentTypeName, string parameterName)
    {
        var preferences = _userService.GetUserPreferences();
        
        return (componentTypeName, parameterName) switch
        {
            ("FluentButton", "Appearance") => preferences.PreferredButtonStyle != null,
            ("FluentDesignSystemProvider", "Direction") => preferences.PreferredDirection != null,
            _ => false
        };
    }
}

/// <summary>
/// Example interfaces - replace with your actual services
/// </summary>
public interface IUserService
{
    UserPreferences GetUserPreferences();
}

public class UserPreferences
{
    public Appearance? PreferredButtonStyle { get; set; }
    public LocalizationDirection? PreferredDirection { get; set; }
}

// Example placeholder classes for enterprise configuration
public static class CoreComponentDefaults { }
public static class ThemeDefaults { }
public static class FeatureDefaults { }
public static class BaseAppDefaults { }