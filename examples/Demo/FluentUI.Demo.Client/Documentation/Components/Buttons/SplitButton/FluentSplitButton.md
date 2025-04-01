---
title: Button / SplitButton
route: /Button/SplitButton
---

# SplitButton

A SplitButton is a variation of the Menu component formed by combining a SplitButton with a MenuButton. It is used to trigger an action and also show a menu of related actions.


## Appearance

When a MenuButton is placed inside a `FluentMenu` component, it will automatically be used as the
trigger to open/close the menu. See the code tab below.


{{ SplitButtonDefault }}


## Icons

You can add icons to a button to help identify the action it triggers.  To do this, you can use an
`IconStart` or `IconEnd` property to add an icon to the beginning or end of the button text.

For a MenuButton, the `IconEnd` by default uses a chevron down icon. By supplying your own value, this can be overruled.

When using `IconStart` or `IconEnd`without supplying any content, the button will be displayed in a smaller form.
By setting the `IconOnly` parameter to true, you can use an icon as the button's content but still have it display in a smaller form. 

By putting an icon in the content, it is possible to specify a `Color` for the icon.

{{ SplitButtonIcon }}

## Shape

Menu buttons can be square, rounded, or circular.

{{ SplitButtonShapes }}

## Size

{{ SplitButtonSizes }}


## API FluentSplitButton

{{ API Type=FluentSplitButton }}

