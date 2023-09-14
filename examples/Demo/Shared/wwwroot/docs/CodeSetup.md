## Configuring the `AddFluentUIComponents()` service collection extension

The following service collection extension needs to be added to the `Program.cs` file:

```csharp
builder.Services.AddFluentUIComponents(options =>
{
    options.HostingModel = {see remark below};
});
```

*If you're running your application on Blazor Server, make sure a default HttpClient is available by adding the following:*

```csharp
builder.Services.AddHttpClient();
```
*before adding the `AddFluentUIComponents()` code*

**The `options.HostingModel` setting is used to determine the type of project you are building. Choose the approprate value form the `BlazorHostingModel` enumeration**

## Scripts 
The heart of this library is formed by the Fluent UI Web Components and the accompanying `web-components.min.js` file. From version 2.3 onwards, the 
script is included in the library itself and no longer needs to be added to your `index.html` or `_Layout.cshtml`. In fact, doing this might lead to 
unpredictable results. 

> **If you are upgrading from an earlier version please remove the script from your `index.html` or `_Layout.cshtml` file.**

The script is added to the application automatically. This way we can safeguard that the you are always using the best matching script version.
