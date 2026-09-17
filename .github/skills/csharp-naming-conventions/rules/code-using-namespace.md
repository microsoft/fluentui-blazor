---
title: Using Directives and Namespaces
impact: HIGH
impactDescription: Inconsistent namespace usage causes merge conflicts and confusion
tags: code, using, namespace, file-scoped
---

## Using and Namespace

Follow these rules for consistency with the .NET team and ASP.NET Core team:

1. **[IDE0065](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0065)** — Prefer using directives to be placed **outside** the namespace.
2. **[SA1210](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1210.md)** — Using directives must be **sorted alphabetically** by namespace.
3. **[IDE0161](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0160-ide0161)** — Namespace declarations should be **file scoped**.

**Correct:**

```csharp
using Microsoft.AspNetCore.Components;
using System;

namespace MyNamespace;

public class Foo
{
}
```

**Incorrect:**

```csharp
namespace MyNamespace
{
    using System;
    using Microsoft.AspNetCore.Components;

    public class Foo
    {
    }
}
```
