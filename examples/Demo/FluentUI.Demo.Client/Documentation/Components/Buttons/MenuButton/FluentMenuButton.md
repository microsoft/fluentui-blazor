---
title: Button / MenuButton
route: /Button/MenuButton
---

# MenuButton

A menu button is a button with a chevron icon after the text typically used to trigger a menu.

All practices for the `FluentButton` apply to the `FluentMenuButton` as well.


## Appearance

Only use one **primary** button in a layout for the most important action.
If there are more than two buttons with equal priority, all buttons should have neutral backgrounds.

When there are many available minor actions, use the **outline**, **subtle**, or **transparent** appearances
on all buttons to avoid a busy layout.

{{ MenuButtonDefault }}


## Icons

You can add icons to a button to help identify the action it triggers.  To do this, you can use an
`IconStart` or `IconEnd` property to add an icon to the beginning or end of the button text.

For a MenuButton, the `IconEnd` by default uses a chevron down icon. By supplying your own value, this can be overruled.

When using `IconStart` or `IconEnd`without supplying any content, the button will be displayed in a smaller form.
By setting the `IconOnly` parameter to true, you can use an icon as the button's content but still have it display in a smaller form. 

By putting an icon in the content, it is possible to specify a `Color` for the icon.

{{ MenuButtonIcon }}

## Shape

Buttons can be square, rounded, or circular.

{{ MenuButtonShapes }}

## Size

{{ MenuButtonSizes }}

## Disabled
{{ MenuButtonDisabled }}

## With long text
{{ MenuButtonLongText }}

## API FluentButton

{{ API Type=FluentMenuButton }}

