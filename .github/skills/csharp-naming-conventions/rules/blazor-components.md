---
title: Blazor Component Naming
impact: MEDIUM-HIGH
impactDescription: Inconsistent component naming breaks routing and discoverability
tags: blazor, components, naming, routing
---

## Component Name

Blazor Components use a combination of C# and HTML markups. HTML tags must be in `[Component].razor` and C# code in `[Component].razor.cs` — don't use `@code { }` section in razor file.
With the exception of the demonstration example (in `/examples` folder), which uses a single file containing both HTML/Razor and code in `@code` section.

### File Naming
- Component file paths use **Pascal case**: `Pages/ProductDetailPage.razor`.
- Component file paths for routable components match their **URLs with hyphens**: a `ProductDetailPage` with `@page "/product-detail"`.

### Module Routing
- For multiple apps in the same project, consider each app as a "_module_": `Pages/Studio/ProductDetailPage.razor` with `@page "/studio/product-detail"`.
- If apps are in different projects, the app name is global: `Pages/ProductDetailPage.razor` with `@page "/studio/product-detail"`.

Reference: [Razor Components in Blazor](https://docs.microsoft.com/en-us/aspnet/core/blazor/components)
