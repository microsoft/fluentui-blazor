---
title: MessageBox
route: /MessageBox
---

# MessageBox

A **MessageBox** is a dialog that is used to display information with a specific intent to the user.
It uses the `DialogService` to display the dialog.

The `DialogService` is an injected service that can be used into any component.
It exposes methods to show a dialog. For working with a **MessageBox**, the following methods are available:

- **ShowSuccessAsync**: Shows a dialog with a success (green) icon, a message and an OK button.

- **ShowWarningAsync**: Shows a dialog with a warning (orange) icon, a message and an OK button.

- **ShowInfoAsync**: Shows a dialog with an information (gray) icon, a message and an OK button.

- **ShowErrorAsync**: Shows a dialog with an error (red) icon, a message and an OK button.

- **ShowConfirmationAsync**: Shows a dialog with a confirmation icon, a message and a Yes/No buttons.
  This method returns a `DialogResult` object where `Cancelled = false` if **Yes** is pressed and `Cancelled = true` if **No** is pressed.

- **ShowMessageBoxAsync**: Shows a dialog with the specified options.

The **MessageBox** is displayed as a modal dialog box. This means that the user has to click one of the buttons to close the dialog.
It cannot be closed by clicking outside of the dialog. 

Clicking **Primary** button (OK) will return `true` and clicking **Secondary** button (Cancel) will return `false` as the dialog result.
See the Console log for these return values.

Internally, the `ShowMessageBox` methods call the `ShowDialog` methods. The ShowMessageBox variants are just convenience methods that make ite easier to work.

## Example

```csharp
await DialogService.ShowSuccessAsync("The action was a success");
```

```csharp
await DialogService.ShowSuccessAsync(message: "The action was a <strong>success</strong>",
                                     title: "Success",
                                     button: "Okay");
```

{{ DialogMessageBox }}
