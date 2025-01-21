---
title: Checkbox
route: /Checkbox
---

# Checkbox

A **FluentCheckbox** component enables a user to select or deselect an option.
It's typically used to capture a boolean value.

## Appearance

The apparent style of a checkbox can be changed by setting the `Shape` property, but also by setting the `Size` property.

You can also add a label to the checkbox by setting the `Label` property.
The label will be automatically positioned next to the checkbox.

We recommend using a spacing of 24px between checkboxes and other components.

{{ CheckboxAppearances }}

Although not recommended by FluentUI, a checkbox can be rendered inline with text using a style attribute.

```
<div>
    Name:
    <Checkbox Style="display: inline-block;" />
</div>
```

## Binding with ImmediateDelay

In some cases, you may want to bind the value of the checkbox to a property of a model
and update the model immediately after the user changes the state. This can be achieved by setting the `Immediate` and the optional `ImmediateDelay` properties.

{{ CheckboxImmediate }}

## States

A checkbox can be in different states, such as `Disabled`, `Indeterminate`, and `Required`.

{{ CheckboxState }}

## Prefix and Suffix

You can use the `StartTemplate` and `EndTemplate` properties to add content before or after the checkbox.

These templates are automatically positioned with a small margin between the checkbox and the prefix/suffix.

{{ CheckboxPrefixSuffix }}

## API FluentCheckbox

{{ API Type=FluentCheckbox }}
