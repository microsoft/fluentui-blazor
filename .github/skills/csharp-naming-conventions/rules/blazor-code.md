---
title: Blazor Methods and Properties
impact: MEDIUM-HIGH
impactDescription: Inconsistent property ordering and nullable handling causes warnings and confusion
tags: blazor, properties, inject, parameter, nullable
---

## Methods and Properties

### Member Order in Blazor Components

1. Private **variables** (using `readonly` if possible) and **constants**
2. The **constructors**
3. The **[Inject]** properties
4. The **[Parameter]** properties
5. The **Public** properties
6. The other properties (**Internal**, **Protected** and **Private**)
7. The **Public** methods
8. The **Internal**, **Protected** methods 
9. The **Private** methods

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
