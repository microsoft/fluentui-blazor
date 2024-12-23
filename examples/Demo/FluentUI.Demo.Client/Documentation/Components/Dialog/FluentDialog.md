---
title: Dialog
route: /Dialog
---

# Dialog

`FluentDialog` is a window overlaid on either the primary window or another dialog window.
Windows under a modal dialog are inert. That is, users cannot interact with content outside an active dialog window.
Inert content outside an active dialog is typically visually obscured or dimmed so it is difficult to discern, and in some implementations,
attempts to interact with the inert content cause the dialog to close.

## Best practices

### Do
 - Dialog boxes consist of a header (`TitleTemplate`), content (`ChildContent`), and footer (`ActionTemplate`),
   which should all be included inside a body (`FluentDialogBody`).
 - Validate that people’s entries are acceptable before closing the dialog. Show an inline validation error near the field they must correct.
 - Modal dialogs should be used very sparingly—only when it's critical that people make a choice or provide information before they can proceed.
   Thee dialogs are generally used for irreversible or potentially destructive tasks. They're typically paired with an backdrop without a light dismiss.

### Don't
 - Don't use more than three buttons between Dialog'`ActionTemplate`.
 - Don't open a `FluentDialog` from a `FluentDialog`.
 - Don't use a `FluentDialog` with no focusable elements

## Usage

{{ DialogServiceDefault Files=Code:DialogServiceDefault.razor;SimpleDialog.razor:SimpleDialog.razor }}

## Customized

{{ DialogServiceCustomized Files=Code:DialogServiceCustomized.razor;SimpleDialogCustomized.razor:SimpleDialogCustomized.razor }}


## Without the DialogService

{{ DialogBodyDefault }}

## API FluentDialogBody

{{ API Type=FluentDialogBody }}
