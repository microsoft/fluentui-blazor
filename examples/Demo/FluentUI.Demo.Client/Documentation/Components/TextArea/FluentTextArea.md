---
title: TextArea
route: /TextArea
---

# TextArea

A **FluentTextArea** component enables a user to enter multiple lines of text.
The component is a wrapper around the FluentUI `TextArea` component.


## Appearance

The apparent style of a textarea can be changed by setting the `Appearance` property, but also by setting the `Size` property.

You can also add a label to the text input by setting the `Label` property and a placeholder by setting the `Placeholder` property.
The label will be automatically positioned above the text input, and the placeholder will be displayed inside the text input.

We recommand to use a spacing of 24px between text fields and other components.

{{ TextAreaAppearances }}

## Resize

The `Resize` property allows you to specify how the textarea can be resized by the user.

{{ TextAreaResizes }}

## Binding with ImmediateDelay

In some cases, you may want to bind the value of the text input to a property of a model
and update the model immediately after the user types a character. But you may also want to delay the update.
This can be achieved by setting the `Immediate` and the optional `ImmediateDelay` properties.

{{ TextAreaImmediate }}

## States

A text input can be in different states, such as `Disabled`, `ReadOnly`, and `Required`.

{{ TextAreaState }}

## API FluentTextArea

{{ API Type=FluentTextArea }}
