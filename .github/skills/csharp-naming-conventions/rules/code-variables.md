---
title: Variable Conventions
impact: HIGH
impactDescription: Poor variable practices lead to bugs and reduced readability
tags: code, variables, var, access-modifiers
---

## Variables

- Write only **one declaration per line**.
- Use only an **explicit name** without abbreviation.
- Use **implicit typing** (`var`) for local variables when the type is obvious from the right side of the assignment.

**Correct:**

```csharp
var fullname = "Scott Guthrie";
var age = 27;
```

- **Don't use `var`** when the type is not apparent from the right side. A variable type is considered clear if it's a `new` operator, an explicit cast, or when the variable name clearly conveys the type.
- **Don't use `dynamic`**, except for justified interop scenarios (e.g., COM interop).
- Use the correct **access modifier** (`private`, `public`, `protected`, `internal`) explicitly on all members.
- **Do not** provide instance fields that are `public` or `protected`. Provide properties instead.
- You can use **insignificant names** `i, j, k, x, y, z` with local loops or very short lambda expressions. Use `_` (discard) when the variable is not consumed.
- **Declare local variables as close as possible to their first use**, with a meaningful initial value. Avoid placeholder initialization (e.g., `string.Empty` or `0`) if the real value is assigned immediately after. 
- Use `string.Empty` or `""` to represent an empty string. Be consistent across the codebase.

**Incorrect:**

```csharp
string x = "";                      // Avoid abbreviations; use an explicit, meaningful name
public int counter;                 // Don't expose public/protected fields; use a property instead
dynamic value = GetSomething();     // Don't use dynamic; it bypasses compile-time type checking
```

**Correct:**

```csharp
var name = "ABC";                   // Declared close to first use with a meaningful default
public int Counter { get; set; }    // Use a property instead of a public field
var customers = GetCustomers();     // var is acceptable when the variable name conveys the type
```

Reference: [Microsoft Field Design Guidelines](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/field)
