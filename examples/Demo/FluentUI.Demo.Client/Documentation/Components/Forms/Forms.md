---
title: Forms
route: /Forms
icon: Form
---

## Validation
The Fluent UI Razor components work with validation messages in the same way the standard Blazor (input) components do. Two extra components are provided to make it possible to show validation messages that follow the Fluent Design guidelines:

- FluentValidationSummary
- FluentValidationMessage

See the [documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/forms/validation?view=aspnetcore-10.0#validation-summary-and-validation-message-components) on the Learn site for more information on the standard components. As the Fluent components are based on the standard components, the same documentation applies

## Example form with validation

This is a copy of the example from the standard [Blazor input components documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/forms/input-components?view=aspnetcore-10.0#example-form), implemented with the Fluent UI Blazor components. It uses the <code>FluentValidationSummary</code> and <code>FluentValidationMessage</code> to give feedback on the state of the form. It
uses the same <code>Starship</code> model as the standard docs and a DataAnnotationsValidator to use the data annotations set in the model.

Not all of the library's input components are used in this form. No data is actually being stored or saved.

{{ BasicForm }}


## API FluentValidationSummary

{{ API Type=FluentValidationSummary }}

## API FluentValidationMessage

{{ API Type=FluentValidationMessage }}
