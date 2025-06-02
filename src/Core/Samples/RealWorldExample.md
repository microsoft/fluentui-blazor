# Real-World Usage Example

This example shows how you might use FluentDefault in a real Blazor application.

## 1. Define Application-Wide Defaults

Create a static class in your application to define defaults:

```csharp
// AppDefaults.cs
using Microsoft.FluentUI.AspNetCore.Components;

namespace MyBlazorApp.Configuration;

public static class AppDefaults
{
    // Button defaults
    [FluentDefault("FluentButton")]
    public static Appearance? Appearance => Appearance.Outline;

    [FluentDefault("FluentButton", ParameterName = "Class")]
    public static string? ButtonClass => "app-btn";

    // Text field defaults - using ParameterName to map different property to same parameter
    [FluentDefault("FluentTextField", ParameterName = "Class")]
    public static string? TextFieldClass => "app-input";

    [FluentDefault("FluentTextField")]
    public static FluentInputAppearance? Appearance => FluentInputAppearance.Outline;

    // Card defaults
    [FluentDefault("FluentCard", ParameterName = "Class")]
    public static string? CardClass => "app-card";

    // Design system defaults
    [FluentDefault("FluentDesignSystemProvider")]
    public static LocalizationDirection? Direction => LocalizationDirection.LeftToRight;

    [FluentDefault("FluentDesignSystemProvider")]
    public static int? Density => 0;

    // Dialog defaults
    [FluentDefault("FluentDialog")]
    public static bool? Modal => true;

    [FluentDefault("FluentDialog", ParameterName = "Class")]
    public static string? DialogClass => "app-dialog";
}
```

## 2. Use Components Normally

In your Razor components, use FluentUI components as normal. The defaults will be automatically applied:

```razor
@* MyComponent.razor *@
@using Microsoft.FluentUI.AspNetCore.Components

@* This button will automatically get Appearance.Outline and Class="app-btn" *@
<FluentButton>Save</FluentButton>

@* This button overrides the appearance but keeps the default class *@
<FluentButton Appearance="Appearance.Accent">Submit</FluentButton>

@* This text field gets the default appearance and class "app-input" *@
<FluentTextField @bind-Value="@userName" Placeholder="Enter your name" />

@* This card gets the default class "app-card" *@
<FluentCard>
    <p>Some card content</p>
</FluentCard>

@* This text field overrides the defaults *@
<FluentTextField @bind-Value="@email" 
                Class="special-input" 
                Appearance="FluentInputAppearance.Filled" 
                Placeholder="Enter your email" />

@code {
    private string userName = "";
    private string email = "";
}
```

## 3. Configure in Program.cs (Optional)

For more advanced scenarios, you might want to configure different defaults based on environment:

```csharp
// Program.cs
using MyBlazorApp.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddFluentUIComponents();

var app = builder.Build();

// The FluentDefault system will automatically discover and apply defaults
// No additional configuration needed!

app.Run();
```

## 4. Environment-Specific Defaults

You can even create environment-specific defaults:

```csharp
// DevelopmentDefaults.cs
#if DEBUG
public static class DevelopmentDefaults
{
    [FluentDefault("FluentButton", ParameterName = "Class")]
    public static string? ButtonClass => "app-btn debug-outline";

    [FluentDefault("FluentDialog")]
    public static bool? Modal => false; // Non-modal in development for easier debugging
}
#endif
```

## Benefits

1. **Consistency**: All components use the same defaults across your application
2. **Maintainability**: Change defaults in one place, affects entire application
3. **No Code Changes**: Existing component usage doesn't need to be modified
4. **Flexibility**: Individual components can still override defaults when needed
5. **Type Safety**: Compile-time checking ensures default values match parameter types
6. **Multiple Mappings**: Different components can have different defaults for the same parameter name