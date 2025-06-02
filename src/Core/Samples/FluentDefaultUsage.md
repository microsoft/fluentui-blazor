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
    public static Appearance DefaultButtonAppearance => Appearance.Outline;

    // Default CSS class for all FluentButton components  
    [FluentDefault("FluentButton")]
    public static string? Class => "my-app-button";

    // Default direction for FluentDesignSystemProvider
    [FluentDefault("FluentDesignSystemProvider")]
    public static LocalizationDirection? Direction => LocalizationDirection.LeftToRight;
}
```

### 2. Use Components Normally

```razor
@* This button will automatically get Appearance.Outline and Class="my-app-button" *@
<FluentButton>Click Me</FluentButton>

@* This button will override the default appearance but keep the default class *@
<FluentButton Appearance="Appearance.Accent">Special Button</FluentButton>

@* This button will override both defaults *@
<FluentButton Appearance="Appearance.Neutral" Class="custom-button">Custom Button</FluentButton>
```

## How It Works

1. The `FluentDefaultAttribute` is applied to static properties in static classes
2. The attribute specifies which component type the default applies to (by name)
3. During component initialization, `FluentComponentBase.OnInitialized()` calls `FluentDefaultValuesService.ApplyDefaults()`
4. The service scans for matching defaults and applies them only if the property is currently unset
5. Explicitly provided parameter values always take precedence over defaults

## Best Practices

- Use a single static class per application or feature area for defaults
- Property names in the defaults class must exactly match the component's parameter names
- Only properties marked with `[Parameter]` will receive default values
- Default values are applied before `OnInitialized()` and `OnParametersSet()` lifecycle methods