---
title: ToggleButton
route: /ToggleButton
---

# ToggleButton

The `ToggleButton` component allows users to commit a change or trigger a toggle action via a single click
or tap and is often found inside forms, dialogs, drawers (panels) and/or pages.

View the [Usage Guidance](https://fluent2.microsoft.design/components/web/react/button/behavior).

## Appearance

Only use one **primary** button in a layout for the most important action.
If there are more than two buttons with equal priority, all buttons should have neutral backgrounds.

When there are many available minor actions, use the **outline**, **subtle**, or **transparent** appearances
on all buttons to avoid a busy layout.

{{ ToggleButtonDefault }}

## Pressed

{{ ToggleButtonPressed }}

## Icons

You can add icons to a button to help identify the action it triggers.
To do this, you can use an `IconStart` or `IconEnd` property to add an icon
to the beginning or end of the button text. When using `IconStart` or `IconEnd`
without supplying any content, the button will be displayed in a smaller form.

By putting an icon in the content, it is possible to specify
a `Color` for the icon.

By setting the `IconOnly` parameter to true, you can use an icon as the button's content but still have it display in a smaller form. 

{{ ToggleButtonIcon }}

## Shape

Toggle buttons can be square, rounded, or circular.

{{ ToggleButtonShapes }}

## Size

{{ ToggleButtonSizes }}

## Disabled

{{ ToggleButtonDisabled }}

## With long text

{{ ToggleButtonLongText }}

## API FluentButton

{{ API Type=FluentToggleButton }}


