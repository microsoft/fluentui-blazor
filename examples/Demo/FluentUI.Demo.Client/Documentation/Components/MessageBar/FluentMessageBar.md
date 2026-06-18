---
title: MessageBar
route: /MessageBar
icon: WindowHeaderHorizontal
---

# MessageBar

Communicates important information about the state of the entire application or surface.
For example, the status of a page, panel, dialog or card.
The information shouldn't require someone to take immediate action, but should persist until
the user performs one of the required actions.

## Appearance

**FluentMessageBar** components come built-in with preset intents that determine the design and aria live announcement.

{{ MessageBarAppearance }}

You can also use the `Shape` parameter to change the shape of the corners of the message bar: square or rounded.

## Actions

The **FluentMessageBar** can have links and different actions.

Add your buttons actions using the `ActionsTemplate` parameter.
To keep a coherent design, use the `FluentButton` component with the `Size` parameter set to `ButtonSize.Small`.

{{ MessageBarActions }}

## Layout

The `Layout` parameter allows you to choose the position of the actions:
  - **SingleLine**: Next to the message content, allowing for a compact layout.
  - **MultiLine**: On a new line, allowing for more space for the message content.
  - **Notification**: The title, message, and actions are displayed on separate lines, providing a clear and structured layout.

When no action is defined, you can set the `TimeStamp` parameter to display the time when the message was created.
This parameter is ignored if at least one action is defined.
If you want to display an Action and a TimeStamp, you can use the `ActionsTemplate` parameter and customize the content.

{{ MessageBarLayouts }}

## Notification Service

Use the `NotificationService` to display message bars from C# code (for example, from an event handler, a service, or after an API call).

The service is registered automatically when you call `AddFluentUIComponents()` in your `Program.cs`:

You can then inject it into any component or service:

```csharp
@inject INotificationService NotificationService
```

**FluentMessageBarProvider**

For the message bars created by the service to be rendered, you **must** add at least one `FluentMessageBarProvider`
component in your application (typically in `MainLayout.razor`, or in any page where you want the messages to appear).

```razor
<FluentMessageBarProvider Section="MAIN" />
```

The `Section` parameter is **required** and used to identify the provider.
When you call one of the `ShowMessageAsync` / `ShowSuccessMessageAsync` / `ShowWarningMessageAsync` / `ShowErrorMessageAsync` / `ShowInfoMessageAsync`
methods, the first argument (or the `MessageBarOptions.Section` property) specifies in which provider the message will be displayed.

This allows you to have several independent provider zones on the same page (for example, one global provider in the layout
and a local provider scoped to a specific panel or dialog), and to route each message to the appropriate one.

>[!Note] A message published to a section that has no matching `FluentMessageBarProvider` will not be visible.
> Make sure the `Section` value passed to the service matches the `Section` value of the provider.

**Showing a message bar**

The simplest way to display a message is to use one of the typed helpers, passing the target section and the message text:

- `NotificationService.ShowSuccessMessageAsync("SECTION", "Title", "Message")`
- `NotificationService.ShowWarningMessageAsync("SECTION", "Title", "Message")`
- `NotificationService.ShowErrorMessageAsync("SECTION", "Title", "Message")`
- `NotificationService.ShowInfoMessageAsync("SECTION", "Title", "Message")`

{{ MessageBarServiceDefault }}

**Configuring the message bar**

For more control (title, intent, layout, lifetime, dismiss button, ...), use the `ShowMessageAsync` overload that accepts
an `Action<MessageBarOptions>`. The returned `MessageBarResult` indicates how the message bar was closed.

{{ MessageBarServiceOptions }}

**Using a custom component**

You can also display a fully custom component inside the message bar by using the generic `ShowMessageAsync<TComponent>` overload.
This is useful when the default layout is not enough and you need to render rich content.

{{ MessageBarServiceCustomized Files=Code:MessageBarServiceCustomized.razor;CustomizedMessageBar:CustomizedMessageBar.razor;CustomizedComponent:CustomizedComponent.razor }}

## API FluentMessageBar

{{ API Type=FluentMessageBar }}

## API NotificationService

{{ API Type=NotificationService }}

## API MessageBarOptions

{{ API Type=MessageBarOptions Properties=All }}

## API FluentMessageBarProvider

{{ API Type=FluentMessageBarProvider }}

## Migrating to v5

{{ INCLUDE File=MigrationFluentMessageBar }}
