---
title: Exception Handling
impact: HIGH
impactDescription: Swallowing generic exceptions hides bugs and makes debugging impossible
tags: code, exceptions, try-catch, error-handling
---

## Exception Handler

Don't swallow errors by catching generic exceptions. See [AV1210](https://csharpcodingguidelines.com//misc-design-guidelines/#AV1210).

Avoid catching non-specific exceptions (`Exception`, `SystemException`, etc.) in application code. Only in top-level code (last-chance exception handler) should you catch a non-specific exception for logging and graceful shutdown.

> Prefer checking data or environment **before** executing an action instead of using `try ... catch`.
> For example, use `File.Exists` before reading a file, `myArray.Length` before reading an item, `Int32.TryParse` before converting a string.

**Incorrect:**

```csharp
try
{
    var value = Convert.ToInt32(input);
}
catch (Exception ex)
{
    // Swallowed — no one will ever know
}
```

**Correct:**

```csharp
if (Int32.TryParse(input, out var value))
{
    // Use value
}
```

Reference: [TryParse vs TryCatch](https://dvoituron.com/2022/09/02/csharp-tryparse-vs-trycatch/)
