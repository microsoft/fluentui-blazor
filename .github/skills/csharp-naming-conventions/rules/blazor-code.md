---
title: Blazor Methods and Properties
impact: MEDIUM-HIGH
impactDescription: Inconsistent property ordering and nullable handling causes warnings and confusion
tags: blazor, properties, inject, parameter, nullable
---

## Methods and Properties

### Member Order in Blazor Components

1. Private **variables** (using `readonly` if possible) and **constants**
2. The **[Inject]** attribute
3. **[Parameter]** attribute
4. **Public** methods and properties
5. **Internal** and **Protected** methods/properties
6. **Private** methods/properties

### Attributes Placement

Place attributes (`Inject`, `Parameter`, ...) **above** the property:

```csharp
[Inject]
public IJSRuntime JsRuntime { get; set; } = default!;
```

### Nullable Handling

When nullable reference types are enabled, initialize non-null properties with `default!` or the `required` keyword to avoid compiler warnings:

```csharp
public required IConnectionManager Connection { get; set; }
```

For properties that accept null values, use `?`:

```csharp
public string? Name { get; set; }
public int? Count { get; set; }
public object? Data { get; set; }
```
