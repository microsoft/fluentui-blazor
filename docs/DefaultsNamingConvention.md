# Externalized Defaults Naming Convention

This document explains the critical naming convention required for externalized defaults classes in FluentUI Blazor components.

## Overview

When creating static defaults classes for FluentUI components using the `[FluentDefault("ComponentName")]` attribute, the property names in the defaults class **MUST** exactly match the component's parameter names.

## Critical Requirement

**The property names must match the parameter names exactly for externalized defaults to work as intended.**

## Examples

### ✅ Correct Pattern

For `FluentButton` component with parameter `Appearance`:

```csharp
[FluentDefault("FluentButton")]
public static class FluentButtonDefaults
{
    // ✅ CORRECT: Property name "Appearance" matches FluentButton parameter name exactly
    public static Appearance Appearance { get; set; } = AspNetCore.Components.Appearance.Neutral;
    
    // ✅ CORRECT: Property name "Disabled" matches FluentButton parameter name exactly  
    public static bool Disabled { get; set; } = false;
}
```

### ❌ Incorrect Patterns

```csharp
[FluentDefault("FluentButton")]  
public static class FluentButtonDefaults
{
    // ❌ WRONG: Property name "DefaultButtonAppearance" does not match parameter name
    public static Appearance DefaultButtonAppearance { get; set; } = AspNetCore.Components.Appearance.Neutral;
    
    // ❌ WRONG: Property name "ButtonAppearance" does not match parameter name  
    public static Appearance ButtonAppearance { get; set; } = AspNetCore.Components.Appearance.Neutral;
    
    // ❌ WRONG: Property name "DefaultAppearance" does not match parameter name
    public static Appearance DefaultAppearance { get; set; } = AspNetCore.Components.Appearance.Neutral;
}
```

## Component Parameter Reference

### FluentButton Parameters

The `FluentButton` component defines these parameters (among others):

- `Appearance` (not `DefaultButtonAppearance` or `ButtonAppearance`)
- `Disabled` (not `DefaultDisabled`)
- `Loading` (not `DefaultLoading`)
- `BackgroundColor` (not `DefaultBackgroundColor`)
- `Color` (not `DefaultColor`)

## Why This Matters

The externalized defaults system uses reflection to match property names from defaults classes to component parameters. If the names don't match exactly:

1. The defaults won't be applied correctly
2. Runtime errors may occur
3. Component behavior will be unpredictable

## Validation

Use the included tests (`FluentButtonDefaultsTests`) to validate that your defaults classes follow the correct naming convention.

## Summary

- ✅ Use exact parameter names: `Appearance`
- ❌ Don't use prefixed names: `DefaultButtonAppearance`, `ButtonAppearance`
- ✅ Follow the `[FluentDefault("ComponentName")]` attribute pattern
- ✅ Validate with unit tests