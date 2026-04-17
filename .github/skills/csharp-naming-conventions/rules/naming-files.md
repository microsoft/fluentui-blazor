---
title: File Organization
impact: CRITICAL
impactDescription: Poor file organization makes navigation and maintenance difficult
tags: naming, files, organization, namespace
---

## File Organization

Organize your files in separate folders to respect these rules:

- **One class = one file.**
- **Class name = file name.**
- **Each namespace into folders with same names.**
- **No more than a few dozen lines per method**, otherwise, break it down into several submethods
- **No more than a few hundred lines per file**, otherwise, split into methods or partial classes.

**Correct:**

```csharp
// File: C:\Dev\MyCompany\MyProject\Blazor\Foo.cs

namespace MyCompany.MyProject.Blazor;

public class Foo
{
}
```

**Incorrect:**

```csharp
// File: C:\Dev\MyCompany\MyProject\Helpers.cs

namespace MyCompany.MyProject.Blazor  // Namespace does not match folder
{
    public class Foo { }      // Class name does not match file name
    public class Bar { }      // Multiple classes in same file
}
```
