---
title: MessageBox
route: /MessageBox
---

# MessageBox

A **MessageBox** is a dialog that is used to display information with a specific intent to the user.
It uses the `DialogService` to display the dialog.

The `DialogService` is an injected service that can be used into any component.
It exposes methods to show a dialog. For working with a **MessageBox**, the following methods are available:

- ShowSuccessAsync
- ShowWarningAsync
- ShowInfoAsync
- ShowErrorAsync
- ShowConfirmationAsync
- ShowMessageBoxAsync

The **MessageBox** is displayed as a modal dialog box. This means that the user has to click one of the buttons to close the dialog.
It cannot be closed by clicking outside of the dialog. 

Clicking **Primary** button (OK) will return `true` and clicking **Secondary** button (Cancel) will return `false` as the dialog result.
See the Console log for these return values.

Internally, the `ShowMessageBox` methods call the `ShowDialog` methods. The ShowMessageBox variants are just convenience methods that make ite easier to work.

## Example

{{ DialogMessageBox }}