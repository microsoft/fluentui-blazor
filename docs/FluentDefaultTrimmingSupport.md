# FluentDefault Blazor WASM and Trimming Support

This document explains how to use the FluentDefault system in Blazor WebAssembly applications with trimming enabled.

## Key Considerations for Blazor WASM + Trimming

### 1. Reflection Usage
The FluentDefault system uses reflection to:
- Scan assemblies for static classes with `[FluentDefault]` attributes
- Discover component parameter properties
- Set default values on component instances

### 2. Trimming Challenges
When trimming is enabled, the following can be removed by the trimmer:
- Static classes that appear unused
- Properties marked with `[FluentDefault]` that aren't directly referenced
- Component parameter properties accessed only through reflection

## Recommended Approaches

### Option 1: Targeted Assembly Scanning (Recommended)
Instead of scanning all loaded assemblies, specify exactly which assemblies to scan:

```csharp
// In Program.cs or startup code
FluentDefaultValuesService.ScanConfiguration
    .WithTargetAssemblies(typeof(MyAppDefaults).Assembly)
    .WithTargetNamespaces("MyApp.Defaults");
```

**Benefits:**
- Faster startup (reduced reflection)
- More predictable for trimming
- Better performance

### Option 2: Instance-Based Defaults (Trimming-Safe)
Use instance providers instead of static properties:

```csharp
public class MyDefaultProvider : FluentDefaultValuesService.IFluentDefaultProvider
{
    public object? GetDefaultValue(string componentTypeName, string parameterName)
    {
        return (componentTypeName, parameterName) switch
        {
            ("FluentButton", "Appearance") => Appearance.Outline,
            ("FluentButton", "Class") => "my-button-class",
            _ => null
        };
    }

    public bool HasDefaultValue(string componentTypeName, string parameterName)
    {
        return (componentTypeName, parameterName) switch
        {
            ("FluentButton", "Appearance") => true,
            ("FluentButton", "Class") => true,
            _ => false
        };
    }
}

// Configure to use only instance providers
FluentDefaultValuesService.ScanConfiguration
    .WithoutStaticDefaults()
    .WithInstanceProvider(new MyDefaultProvider());
```

### Option 3: Preserve Static Defaults with Attributes
If you must use static defaults, preserve them with `[DynamicDependency]`:

```csharp
[DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(MyAppDefaults))]
public static class PreserveDefaultsHelper
{
    // This class exists solely to preserve MyAppDefaults from trimming
}

public static class MyAppDefaults
{
    [FluentDefault("FluentButton")]
    public static Appearance? ButtonAppearance => Appearance.Outline;
    
    [FluentDefault("FluentButton")]
    public static string? ButtonClass => "my-button";
}
```

## Project Configuration for Trimming

### 1. Enable Trimming Warnings
Add to your `.csproj`:

```xml
<PropertyGroup>
    <PublishTrimmed>true</PublishTrimmed>
    <TrimMode>link</TrimMode>
    <SuppressTrimAnalysisWarnings>false</SuppressTrimAnalysisWarnings>
</PropertyGroup>
```

### 2. Preserve FluentUI Components
If you get trimming warnings about FluentUI components, preserve them:

```xml
<ItemGroup>
    <TrimmerRootAssembly Include="Microsoft.FluentUI.AspNetCore.Components" />
</ItemGroup>
```

### 3. Custom Trim Configuration
Create a `Linker.xml` file in your project root to preserve specific types:

```xml
<linker>
    <assembly fullname="YourApp">
        <!-- Preserve your defaults classes -->
        <type fullname="YourApp.Defaults.*" preserve="all" />
        
        <!-- Preserve specific defaults class -->
        <type fullname="YourApp.Defaults.MyAppDefaults" preserve="all">
            <method name="get_*" />
        </type>
    </assembly>
</linker>
```

## Performance Optimization Examples

### Minimal Scanning Configuration
```csharp
// Only scan your specific defaults assembly and namespace
FluentDefaultValuesService.ScanConfiguration
    .WithTargetAssemblies(typeof(Program).Assembly)
    .WithTargetNamespaces("MyApp.UI.Defaults");
```

### Multi-Assembly Targeted Scanning
```csharp
// Scan multiple specific assemblies
FluentDefaultValuesService.ScanConfiguration
    .WithTargetAssemblies(
        typeof(MyApp.Defaults.AppDefaults).Assembly,
        typeof(MyApp.Theming.ThemeDefaults).Assembly
    )
    .WithTargetNamespaces("MyApp.Defaults", "MyApp.Theming");
```

### Hybrid Approach
```csharp
// Use both static and instance providers
FluentDefaultValuesService.ScanConfiguration
    .WithTargetAssemblies(typeof(StaticDefaults).Assembly)
    .WithTargetNamespaces("MyApp.Defaults")
    .WithInstanceProvider(new TenantDefaultProvider(tenantService))
    .WithInstanceProvider(new UserPreferencesDefaultProvider(userService));
```

## Troubleshooting Trimming Issues

### 1. Defaults Not Applied
- Check if your defaults classes are being trimmed
- Add `[DynamicDependency]` attributes
- Use targeted assembly scanning
- Switch to instance providers

### 2. Component Parameters Not Found
- Ensure component types are preserved
- Add component assemblies to `TrimmerRootAssembly`
- Use explicit component references in your code

### 3. Performance Issues
- Reduce assembly scanning scope with `WithTargetAssemblies`
- Reduce namespace scanning scope with `WithTargetNamespaces`
- Consider instance providers for complex scenarios

### 4. Runtime Exceptions
- Enable trim analysis warnings: `<SuppressTrimAnalysisWarnings>false</SuppressTrimAnalysisWarnings>`
- Check build output for trim warnings
- Test thoroughly in Release mode with trimming enabled

## Best Practices Summary

1. **Use targeted scanning** instead of full AppDomain scanning
2. **Prefer instance providers** for complex multi-tenant scenarios
3. **Test with trimming enabled** in development
4. **Preserve defaults classes** with `[DynamicDependency]` if needed
5. **Monitor startup performance** and optimize scanning scope
6. **Enable trim warnings** during development to catch issues early