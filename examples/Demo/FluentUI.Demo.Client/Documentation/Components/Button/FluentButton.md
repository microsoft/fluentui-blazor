---
title: Button
route: /Button
---

# Button

The `Button` component allows users to commit a change or trigger an action via a single click
or tap and is often found inside forms, dialogs, panels and/or pages.

View the [Usage Guidance](https://fluent2.microsoft.design/components/web/react/button/usage).

## Appearance

Only use one **primary** button in a layout for the most important action.
If there are more than two buttons with equal priority, all buttons should have neutral backgrounds.

When there are many available minor actions, use the **outline**, **subtle**, or **transparent** appearances
on all buttons to avoid a busy layout.

{{ ButtonDefault }}

## Loading

When a button is in a loading state, it should be disabled and show a spinner to indicate
that the action is in progress.

Click on one of these buttons to switch to the `Loading` state.
The button will be disabled and a spinner will be shown.
The button will be enabled again after 2 seconds.

{{ ButtonLoading }}

## Icons

You can add icons to a button to help identify the action it triggers.
To do this, you can use an `IconStart` or `IconEnd` property to add an icon
to the beginning or end of the button text.

With icon in the content. By doing it this way, it is possible to specify
a <code>Color</code> for the icon.

{{ ButtonIcon }}

## Shapes

Buttons can be square, rounded, or circular.

{{ ButtonShapes }}

## API FluentButton

{{ API Type=FluentButton }}

## Migration from v4

{{ INCLUDE File=MigrationFluentButton }}
