---
title: Data Type Conversion
impact: MEDIUM
impactDescription: Using ToString() on null objects causes NullReferenceException
tags: code, conversion, cast, convert
---

## Data Conversion

Always use one of these methods to convert between data types:

- `Convert` class
- Explicit casting via `(...)`
- `as` operator
- `$"{x}"` (string interpolation)

**Correct:**

```csharp
var a = Convert.ToString(myVariable);
var b = (string)myVariable;
var c = myVariable as string;
var d = $"{myVariable}";
```

**Incorrect:**

```csharp
var a = myVariable.ToString();  // Throws NullReferenceException if myVariable is null
```

> `Convert.ToString()` handles `null` (returns `string.Empty`), while `ToString()` doesn't.
