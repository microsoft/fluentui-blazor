---
title: JavaScript Interop in Blazor
impact: MEDIUM
impactDescription: Excessive JS interop calls cause significant performance overhead
tags: blazor, javascript, interop, collocation
---

## JavaScript Collocated with a Component

> Avoid too many calls between .NET and JavaScript — calls are asynchronous, parameters are JSON-serialized, and on Blazor Server they cross the network.

Use [collocated JavaScript files](https://docs.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/?view=aspnetcore-6.0#load-a-script-from-an-external-javascript-file-js-collocated-with-a-component) for component-specific JS. 
This centralizes JS code, avoids global functions, and loads code only when needed.

> If a JavaScript function is used by at least 3 components, include it in a global file (`wwwroot/js`).

### Steps

1. **Create a `[Component].razor.js` file:**

```javascript
export function myModule_myJsMethod() {
    // ...
}
```

2. **Add constants and properties in your component:**

```csharp
private const string JAVASCRIPT_FILE = "./Pages/[Path]/[Component].razor.js";

[Inject]
private required IJSRuntime JsRuntime { get; set; }

private IJSObjectReference JsModule { get; set; } = default!;
```

3. **Call the JavaScript function from C#:**

```csharp
if (JsModule == null)
{
    JsModule = await JsRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
}
await JsModule.InvokeVoidAsync("myModule_myJsMethod");
```
