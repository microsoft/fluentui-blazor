---
title: Blazor Unit Tests with bUnit
impact: HIGH
impactDescription: Untested Blazor components break silently on refactoring
tags: testing, blazor, bunit, verifier
---

## Blazor Unit Tests

All Blazor unit tests use [bUnit](https://bunit.dev/) and at least for one test per component: `FluentAssert.Verify()` method.

**IMPORTANT**: 
 1. Test all Blazor components in a Razor file.
 2. You can test simple classes (services or utilities) in standard `.cs` files.

With bUnit, you can:
- Setup and define components under tests using C# or Razor syntax
- Verify outcomes using semantic HTML comparer
- Interact with and inspect components as well as trigger event handlers
- Pass parameters, cascading values and inject services
- Mock `IJSRuntime`, Blazor authentication and authorization

### Example for a class

```csharp
public partial class CssBuilderTests
{
    [Fact]
    public void CssBuilder_AddSingleClasses()
    {
        // Arrange
        var cssBuilder = new CssBuilder();

        // Act
        cssBuilder.AddClass("class1");
        cssBuilder.AddClass("class2");

        // Assert
        Assert.Equal("class1 class2", cssBuilder.Build());
    }
}
```

### Example Using Verify()

```razor
@using Xunit;
@inherits FluentUITestContext

@code
{
    public FluentButtonTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddFluentUIComponents();
    }

    [Fact]
    public void FluentButton_Default()
    {
        // Arrange & Act
        var cut = Render(@<FluentButton>fluent-button</FluentButton>);

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentButton_OnClick()
    {
        bool clicked = false;

        // Arrange
        var cut = Render(@<FluentButton OnClick="@(e => { clicked = true; })">My button</FluentButton>);

        // Act
        cut.Find("fluent-button").Click();

        // Assert
        Assert.True(clicked);
    }
}
```

This generates a `.received.html` file compared to `.verified.html`:
