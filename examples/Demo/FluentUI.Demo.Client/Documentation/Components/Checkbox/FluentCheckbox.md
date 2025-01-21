---
title: Checkbox
route: /Checkbox
---

# Checkbox

A **FluentTextInput** component enables a user to enter text into an app.
It's typically used to capture a single line of text.
The text displays on the screen in a simple, uniform format.

## Appearance

The apparent style of a text input can be changed by setting the `Appearance` property, but also by setting the `Size` property.

You can also add a label to the text input by setting the `Label` property and a placeholder by setting the `Placeholder` property.
The label will be automatically positioned above the text input, and the placeholder will be displayed inside the text input.

We recommand to use a spacing of 24px between text fields and other components.

{{ CheckboxAppearances }}

Although not recommended by FluentUI, an input can be rendered inline with text using a style attribute.

```
<div>
    Name:
    <Checkbox Style="display: inline-block;" />
</div>
```

## Binding with ImmediateDelay

In some cases, you may want to bind the value of the text input to a property of a model
and update the model immediately after the user types a character. But you may also want to delay the update.
This can be achieved by setting the `Immediate` and the optional `ImmediateDelay` properties.

{{ TextInputImmediate }}

## States

A text input can be in different states, such as `Disabled`, `ReadOnly`, and `Required`.

{{ TextInputState }}

## Prefix and Suffix

You can use the `StartTemplate` and `EndTemplate` properties to add a prefix or a suffix to the text input
as `https://` and `.com` or an icon.

These templates are automatically positioned with a small margin between the text entered and the prefix/suffix.
You cannot therefore fill the entire background of these templates, with a colour for example.

{{ TextInputPrefixSuffix }}

## API FluentCheckbox

{{ API Type=FluentCheckbox }}
