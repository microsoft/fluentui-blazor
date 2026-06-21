---
title: Toast
route: /Toast
category: 20|Components
icon: FoodToast
---

# Toast

A toast is an elevated, temporary notification that gives people feedback about an action they just took or informs them about a timely event.

Use toast notifications for information that is useful and relevant, but not critical. If people must take immediate action, use a dialog instead. If the message is not tied to an immediate user action, consider a message bar.

The library provides a `FluentToast` component that can be used to display these notifications.
To display a toast, you **must** use the `INotificationService`. 
You use the `ToastOptions` class to configure the toast's content and behavior.

## Before you start

Add a `FluentProviders` (containing the `FluentToastProvider`) to your layout and inject `INotificationService` where you trigger notifications.

Use these guidance points to keep toast usage consistent:

- Show toasts in a non-blocking area, such as an app corner, and keep placement consistent.
- Do not place toasts in the center of the experience.
- In duplicate or multi-window scenarios, show the toast only in the focused window where the action occurs.
- Keep toast content concise: one-line title, short supporting text, and clear action labels.
- Prefer no more than four visible toasts in the same toaster.
- Use timed dismissal for informational success feedback, and persistent behavior for active progress.

## Accessibility

Toasts are announced with an alert role and live region behavior based on intent. Use the `Intent` value in `ToastOptions` to apply semantic styling and announcement priority.

Use assertive intents carefully, because too many interruptions can disrupt screen reader users.

## Default values

Global default values (used for all instances) can be set using the `LibraryConfiguration.Toast` member. 
The type of this member is `LibraryToastOptions`, and has the following properties (and default values):

- `MaxToastCount = 4`
- `Lifetime = null`
- `Position = ToastPosition.BottomEnd`
- `VerticalOffset = 16`
- `HorizontalOffset = 20`
- `PauseOnHover = true`
- `PauseOnWindowBlur = true`
- `AllowDismiss = true`
- `Inverted = false`
- `Width = null (290px)`

The preferred defaults can be set in the `AddFluentUIComponents` method when configuring services.


**Example**

```csharp
// Add FluentUI services
builder.Services.AddFluentUIComponents(config =>
{
    config.Toast.AllowDismiss = false;
    config.Toast.Position = ToastPosition.TopEnd;   
});
```

## Examples

### Default

This example shows the standard toast setup with default behavior and intent. Use it as the baseline pattern for simple status feedback.

{{ FluentToastDefault }}

### Custom dismissal

This example shows a toast that uses a custom dismiss action instead of the default dismiss button, useful when you need a tailored close flow.

{{ FluentToastCustomDismiss }}

### Indeterminate progress

This example shows an indeterminate progress toast for operations where completion time is unknown.

{{ FluentToastIndeterminateProgress }}

### Determinate progress

This example shows a determinate progress toast that updates as the operation advances toward completion.

{{ FluentToastDeterminateProgress }}

### Quick actions

This example shows quick action links inside the toast so people can immediately respond to the notification.

{{ FluentToastQuickActions }}

### Inverted toast

This example shows an inverted toast style for surfaces that need stronger contrast against the current background.

{{ FluentToastInverted }}

## API NotificationService

{{ API Type=NotificationService }}

## API FluentToast

{{ API Type=FluentToast }}

## API ToastOptions

{{ API Type=ToastOptions Properties=All }}

## API FluentToastProvider

{{ API Type=FluentToastProvider }}
