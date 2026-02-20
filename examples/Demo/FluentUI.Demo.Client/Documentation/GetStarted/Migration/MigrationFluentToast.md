---
title: Migration FluentToast
route: /Migration/Toast
hidden: true
---

- ### Entire toast system removed 💥

  The full toast notification system has been **removed** in V5:
  - `FluentToast` component
  - `FluentToastProvider` component
  - `IToastService` / `ToastService`
  - `ToastInstance`, `ToastResult`
  - Content components: `CommunicationToast`, `ConfirmationToast`, `ProgressToast`
  - Content types: `CommunicationToastContent`, `ConfirmationToastContent`, `ProgressToastContent`
  - `ToastParameters`
  - Enums: `ToastIntent`, `ToastPosition`, `ToastTopCTAType`

- ### V4 FluentToastProvider parameters (removed)

  | Parameter | Type | Default |
  |-----------|------|---------|
  | `Position` | `ToastPosition` | `TopRight` |
  | `Timeout` | `int` | `7000` |
  | `MaxToastCount` | `int` | `4` |
  | `RemoveToastsOnNavigation` | `bool` | `true` |

- ### Migration strategy

  Use `FluentMessageBar` for persistent notification messages, or implement a custom
  toast/notification pattern using the V5 component set.

  ```csharp
  // V4
  @inject IToastService ToastService

  ToastService.ShowCommunicationToast(new ToastParameters<CommunicationToastContent>
  {
      Intent = ToastIntent.Success,
      Title = "Saved!",
      Content = new CommunicationToastContent { Subtitle = "Changes saved." }
  });
  ```

  ```xml
  <!-- V5: Use FluentMessageBar for visible notifications -->
  <FluentMessageBar Intent="MessageBarIntent.Success"
                    Title="Saved!"
                    Animation="MessageBarAnimation.FadeIn">
      Changes saved.
  </FluentMessageBar>
  ```
