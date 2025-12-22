---
title: Default Values
order: 0006
category: 10|Get Started
route: /default-values
---

# Default Values

**Default values** are used to set the initial state of components when they are rendered.

It is possible to set default values for components globally, so that you don't have to specify them every time you use a component.
For example, you can set the default appearance of a button to `Primary` and its shape to `Circular`.
In this case the button will always have these values unless you override them in a specific instance.

## Example

{{ DefaultValuesSample SourceCode=false }}

```csharp
// Add FluentUI services
builder.Services.AddFluentUIComponents(config =>
{
    // Set default values for FluentButton component
    config.DefaultValues.For<FluentButton>().Set(p => p.Appearance, ButtonAppearance.Primary);
    config.DefaultValues.For<FluentButton>().Set(p => p.Shape, ButtonShape.Circular);
});
```
