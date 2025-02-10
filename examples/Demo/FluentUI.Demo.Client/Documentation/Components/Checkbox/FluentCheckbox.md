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

### Size

The size of the checkbox can be adjusted using the `Size` property. The available sizes are:

- `Medium`: The default size.
- `Large`: A larger size for the checkbox.

{{ CheckboxSizes }}

### Indeterminate

The `FluentCheckbox` component supports an indeterminate state, which can be useful for scenarios where a checkbox represents a mixed or partial selection.
The indeterminate state is visually distinct from the checked and unchecked states.

To set the checkbox to the indeterminate state, use the `Indeterminate` property.

{{ CheckboxIndeterminate }}

## Three-State Checkbox

The `FluentCheckbox` component supports a three-state mode, which allows the checkbox to have an additional indeterminate state. This can be useful for scenarios where a checkbox represents a mixed or partial selection.

To enable the three-state mode, set the `ThreeState` property to `true`. You can also control the order of the states using the `ThreeStateOrderUncheckToIntermediate` property.

- `ThreeState`: Enables the three-state mode.
- `ThreeStateOrderUncheckToIntermediate`: Controls the order of the states. If set to `true`, the order will be Unchecked -> Intermediate -> Checked. If set to `false` (default), the order will be Unchecked -> Checked -> Intermediate.

{{ CheckboxThreeStates }}

## API FluentCheckbox

{{ API Type=FluentCheckbox }}
