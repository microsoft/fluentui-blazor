---
title: Forms
route: /Forms
icon: Form
---

## Validation

The Fluent UI Razor components work with a validation summary in the same way the standard Blazor (input) components do. An extra component is provided to make it possible to show a validation summary that follows the Fluent Design guidelines:

- FluentValidationSummary

### Native constraint validation UI

By default the Fluent UI components render validation feedback using the library's UI. If you prefer the browser's native HTML5 constraint validation UI (the built-in validation bubbles/messages driven by the Constraint Validation API), you can opt in at library registration time.

```csharp
// Program.cs
builder.Services.AddFluentUIComponents(config =>
{
    // Default is false. Set to true to enable the browser's native constraint validation UI.
    config.UseNativeConstraintValidationUI = true;
});
```

The name "constraint" refers to the HTML5 Constraint Validation API (attributes like `required`, `pattern`, `min`, `max`, etc.) and the browser's native UI for reporting those violations. Use this option when you want parity with the browser's built-in validation experience; otherwise keep the library defaults for a consistent Fluent UI look-and-feel.

See the [documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/forms/validation?view=aspnetcore-10.0#validation-summary-and-validation-message-components) on the Learn site for more information on the standard components. As the Fluent component is based on the standard component, the same documentation applies

## Example form with validation

This is a copy of the example from the standard [Blazor input components documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/forms/input-components?view=aspnetcore-10.0#example-form), implemented with the Fluent UI Blazor components. It uses the `FluentValidationSummary` to give feedback on the state of the form. It
uses the same `Starship` model as the standard docs and a DataAnnotationsValidator to use the data annotations set in the model.

Not all of the library's input components are used in this form. No data is actually being stored or saved.

{{ BasicForm }}

## API FluentValidationSummary

{{ API Type=FluentValidationSummary }}
