# FluentDefault Attribute Usage

The `FluentDefaultAttribute` allows you to define externalized default values for component parameters using static classes. This provides a clean way to establish application-wide defaults without modifying component code.

## Basic Usage

### 1. Define Defaults in a Static Class

```csharp
using Microsoft.FluentUI.AspNetCore.Components;

public static class MyAppDefaults
{
    // Default appearance for all FluentButton components
    [FluentDefault("FluentButton")]
    public static Appearance Appearance => Appearance.Outline;

    // Default CSS class for all FluentButton components  
    [FluentDefault("FluentButton")]
    public static string? Class => "my-app-button";

    // Default direction for FluentDesignSystemProvider
    [FluentDefault("FluentDesignSystemProvider")]
    public static LocalizationDirection? Direction => LocalizationDirection.LeftToRight;
}
```

### 2. Multiple Components with Same Parameter

When you need to set the same parameter (like `Class`) for different components with different values, use the `ParameterName` property:

```csharp
public static class MyAppDefaults
{
    // Different default classes for different components
    [FluentDefault("FluentButton", ParameterName = "Class")]
    public static string? ButtonClass => "my-app-button";

    [FluentDefault("FluentTextInput", ParameterName = "Class")]
    public static string? InputClass => "my-app-input";

    [FluentDefault("FluentCard", ParameterName = "Class")]
    public static string? CardClass => "my-app-card";
}
```

### 3. Use Components Normally

```razor
@* This button will automatically get Appearance.Outline and Class="my-app-button" *@
<FluentButton>Click Me</FluentButton>

@* This input will automatically get Class="my-app-input" *@
<FluentTextInput />

@* This button will override the default appearance but keep the default class *@
<FluentButton Appearance="Appearance.Accent">Special Button</FluentButton>

@* This button will override both defaults *@
<FluentButton Appearance="Appearance.Neutral" Class="custom-button">Custom Button</FluentButton>
```

## How It Works

1. The `FluentDefaultAttribute` is applied to static properties in static classes
2. The attribute specifies which component type the default applies to (by name)
3. Optionally, the `ParameterName` property can specify which component parameter to map to
4. During component initialization, `FluentComponentBase.OnInitialized()` calls `FluentDefaultValuesService.ApplyDefaults()`
5. The service scans for matching defaults and applies them only if the property is currently unset
6. Explicitly provided parameter values always take precedence over defaults

## Best Practices

- Use a single static class per application or feature area for defaults
- Use descriptive property names with `ParameterName` when mapping multiple properties to the same parameter
- When `ParameterName` is not specified, the property name must exactly match the component's parameter name
- Only properties marked with `[Parameter]` will receive default values
- Default values are applied before `OnInitialized()` and `OnParametersSet()` lifecycle methods

## Limitations

- **Value Types**: If a value type parameter is explicitly set to its language default value (e.g., `false` for `bool`, `0` for `int`), it will be overridden by the external default. This is because the system cannot distinguish between "not set" and "explicitly set to default value".
- **Nullable Value Types**: Nullable types set to `null` are treated as "not set" and will receive the external default.
- **Reference Types**: Reference types set to `null` are treated as "not set" and will receive the external default.