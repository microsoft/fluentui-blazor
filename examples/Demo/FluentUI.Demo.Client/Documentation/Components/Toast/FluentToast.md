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

## Types

Toasts generally fall into three categories: confirmation, progress, and communication.
The ideal configuration and usage of each toast type is described below:

**Confirmation toast**

Confirmation toasts are shown to someone as a direct result of their action. 
A confirmation toast’s state can be success, error, warning, informational, or progress.

**Progress toast**

Progress toasts inform someone about the status of an operation they initiated.

**Communication toast**

Communication toasts inform someone of messages from the system or another person’s actions. 
These messages can include mentions, event reminders, replies, and system updates.
They include a call to action directly linking to a solution or the content that they reference. 
They can be either temporary or persistent. They’re dismissible only if there is another surface, 
like a notification center, where the customer can find this content again later.

## Behavior

### Dismissal

Toasts can have timed, conditional, or express dismissals, dependent on their use case.

**Timed dismissal**

If there is no action to take, toast will time out after seven seconds. 
Timed dismissal is best when there is no further action to take, like for a successful confirmation toast.

People who navigate via mouse can pause the timer by hovering over the toast. 
However, toasts that don’t include actions won’t receive keyboard focus for people who navigate primarily by keyboard.

**Conditional dismissal**

Use conditional dismissal for toasts that should persist until a condition is met, like a progress toast that dismisses once a task is complete.

Don’t use toasts for necessary actions. If you need the encourage people to take an action before moving forward, 
try a more forceful surface like a message bar or a dialog.

**Express dismissal**

Include the "Close" button to allow people to expressly dismiss toasts only if they can find that information again elsewhere, 
like in a notification center.

>[!Note] We do not have a way yet to facilitate showing toast messages on other surfaces like a notification center, so use the express dismissal option with caution.

### Determinate and indeterminate progress

Progress toasts can be either determinate or indeterminate, depending on the needs of your app and the 
capabilities of the technology you’re building on.

When the completion time can be predicted, show a determinate progress bar and percentage of completion. 
Determinate progress bars offer a reliable user experience since they communicate status and assure people things are still working.

If the completion time is unknown or its accuracy is unreliable, show an indeterminate spinner icon instead.

Although a specific type of toast needs to be specified through the `ToastOptions`, the library does not prevent you 
from showing both a spinner icon and a progress bar in the same toast, but we recommend strongly against doing this.

## Accessibility

Toasts are announced with an alert role and live region behavior based on intent. Use the `Intent` value in `ToastOptions` to apply semantic styling and announcement priority.

Use assertive intents carefully, because too many interruptions can disrupt screen reader users.

## Default values

Global default values (used for all instances) can be set using the `LibraryConfiguration.Toast` member. 
The type of this member is `LibraryToastOptions`, and has the following properties (and default values):

- `MaxToastCount = 4`
- `Lifetime = 7 seconds`
- `Position = ToastPosition.BottomEnd`
- `VerticalOffset = 16`
- `HorizontalOffset = 20`
- `PauseOnHover = true`
- `PauseOnWindowBlur = true`
- `AllowDismiss = true`
- `Inverted = false`
- `Width = null (290px)`

The preferred defaults can be set in the `AddFluentUIComponents` method when configuring services.

> [!NOTE] By default, toasts stay on the screen for seven seconds. On hover or focus, the toast’s timer will pause 
> and resume when the person navigates away from it.
> When a toast contains at least one quick action and no explicit `Lifetime` is set, the default lifetime
> is automatically set to `0` to disable automatic closing.

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

### Fastest helper methods

This example shows the fastest helper methods to display **success**, **info**, **warning**, **error** and **progress** toasts 
by using a required title plus optional message and dismiss button details.

In this example, **Success**, **Warning**, **Error**, **Info**, and **Progress** toasts are shown for 7 seconds (`lifetime = 7`) 
and then close automatically.

These five helper methods are non-blocking. Because they use `ToastResultTiming.Queued`, the awaited call completes 
as soon as the toast is queued, so the code after `await` continues to run immediately, without waiting for the toast 
to be rendered or closed:

```csharp
await NotificationService.ShowSuccessToastAsync("Saved");
```

The **Progress** toast follows the same pattern, returning the `ToastResult` instance immediately so you can keep a 
reference to it:

```csharp
// Show the Progress Toast
var ProgressResult = await NotificationService.ShowProgressToastAsync("Working...");

// Use the kept reference later to interact with the toast
await ProgressResult.Instance.CloseAsync();
```

When `ProgressResult` is not `null`, the **Close Progress** button is enabled so the user can close that toast manually.


{{ FluentToastDefault }}

### Default

This example shows the standard toast setup with default behavior and intent. Use it as the baseline pattern for simple status feedback.

**Notes**: 
- By default, `ResultTiming = ToastResultTiming.Queued`. The code after `await` resumes as soon as the toast is queued (just before it becomes visible). 
- Set this property to `ToastResultTiming.Visible` to block execution until the toast is visible.
- Set this property to `ToastResultTiming.Closed` to block execution until the toast is dismissed.

{{ FluentToastDefaultOptions }}

### Custom dismissal

This example shows a toast that uses a custom dismiss action instead of the default dismiss button, useful when you need a tailored close flow.

{{ FluentToastCustomDismiss }}

### Indeterminate progress

This example shows an indeterminate progress toast for operations where completion time is unknown.

{{ FluentToastIndeterminateProgress }}

### Determinate progress

This example shows a determinate progress toast that updates as the operation advances toward completion.

It uses `ShowToastAsync<TToast>(...)`, where the generic type `TToast` is the Razor component **dynamically** rendered inside the toast body.
With this approach, you can open any Razor component in the toast and fully customize its content and behavior.

{{ FluentToastDeterminateProgress }}

### Quick actions

This example shows quick action links inside the toast so people can immediately respond to the notification.

> [!NOTE] When a toast contains at least one quick action and no explicit `Lifetime` is set, the default lifetime
> is automatically set to `0` to disable automatic closing. This keeps the toast on screen so the user has time
> to interact with the action. To override this behavior, set an explicit `Lifetime` value in the `ToastOptions`.

{{ FluentToastQuickActions }}

### Result Timing

This example shows when your application should consider an interaction with a context notification to be complete and when the .NET code should continue executing
`var result = await NotificationService.ShowToastAsync()`.

By default, `await NotificationService.ShowToastAsync(...)` resumes only when the toast is closed.

If you do not want to wait for the result, start the call without waiting for completion:
`_ = NotificationService.ShowToastAsync(...);`

You can also control when the awaited result is completed with `ResultTiming`:
- `ResultTiming = Closed` (default): code after `await` runs after the toast is closed.
- `ResultTiming = Visible`: code after `await` runs as soon as the toast is visible.

When using `Visible`, keep the returned `result.Instance` if you need to interact with that toast later (for example, close it programmatically).

In the sample, both toasts stay visible for 7 seconds. **Show Lifetime On Close** reports the result after the toast closes, 
while **Show Lifetime On Visible** reports it immediately when the toast appears.

{{ FluentToastResultTiming }}

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
