---
title: Migration FluentProfileMenu
route: /Migration/ProfileMenu
hidden: true
---

- ### Reimplemented with V5 components

  `FluentProfileMenu` now uses `FluentPopover`, `FluentAvatar`, and
  `FluentPresenceBadge`. The profile menu retains its V4 parameter names and
  template sections.

- ### Changed size properties

  `ButtonSize` and `ImageSize` now use the `AvatarSize` enum instead of CSS strings.

  ```xml
  <!-- V4 -->
  <FluentProfileMenu ButtonSize="32px" ImageSize="64px" />

  <!-- V5 -->
  <FluentProfileMenu ButtonSize="AvatarSize.Size32"
                     ImageSize="AvatarSize.Size64" />
  ```

- ### Updated supporting components

  The rendered markup and styling hooks differ because `FluentPersona` and the
  V4 popover sections are no longer used internally. Prefer the
  `HeaderTemplate`, `ChildContent`, and `FooterTemplate` parameters instead of
  targeting internal elements.

See the [FluentProfileMenu documentation](/ProfileMenu) for examples and the current API.
