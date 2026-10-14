---
title: Profile menu
route: /ProfileMenu
icon: PersonAccounts
---

# Profile menu

A profile menu gives signed-in users quick access to their identity, account details, settings, and sign-out action.
It is commonly placed in an application header, usually near the top-right corner, where it remains easy to find without 
taking space from the main content.

This feature is composed of two components:

- `FluentProfileMenu` displays the avatar button, an optional presence badge, and a popover when the button is selected.
- `FluentProfilePopover` provides a ready-to-use layout for profile information and account actions inside the menu.

Use the components together for a standard profile menu, or provide custom content to `FluentProfileMenu` when your application needs a different layout.

## When to use

Use a profile menu to group actions that apply to the current user, such as:

- Viewing or editing an account.
- Changing personal settings or preferences.
- Displaying the current availability or presence status.
- Switching accounts or signing out.

Keep frequently used application navigation outside the profile menu.
The menu should contain user-specific information and actions rather than general navigation or large amounts of content.

## Button

`FluentProfileMenu` is the interactive container. Its avatar acts as the menu button and opens the content supplied through `ChildContent`.

Set `ImageUrl` to display a profile image, or use `ImageInitials` as a fallback. 
Use `ImageSize` to change the avatar size. 
To show a presence indicator, set `Status`, and optionally customize its tooltip and size with `StatusTitle` and `StatusSize`.

The `Title` value is used as the accessible label for the avatar button and its popover. It defaults to a localized
"Profile menu" label, but a more specific description such as the user's name or "Open profile menu" is recommended.

Content can be added around the avatar with:

- `StartTemplate` for content before the avatar, such as the user's display name.
- `EndTemplate` for content after the avatar, such as a chevron or another visual indicator.
- `ChildContent` for everything displayed in the popover.

Use `@bind-Open` when the application needs to observe or control whether the menu is open.

{{ FluentProfileMenuDefault }}

## Popover

`FluentProfilePopover` is an optional content layout designed for use inside `FluentProfileMenu`. 
It divides the profile content into header, main, and footer sections.

For a standard layout, set:

- `HeaderLabel` and `HeaderLinkLabel` for a heading and an action such as "Sign out".
- `ImageUrl`, `ImageInitials`, `ImageSize`, `FullName`, and `EMail` for the user's identity.
- `FooterLabel` and `FooterLinkLabel` for additional account information or navigation.
- `OnHeaderLinkClick` and `OnFooterLinkClick` to handle the built-in actions.
- `Width` and `Height` to control the size of the content.

## Customizing the popover content

Each section can be replaced with custom Razor content:

- `HeaderTemplate` replaces the default header label and link.
- `ChildContent` replaces the default avatar, name, and email layout.
- `FooterTemplate` replaces the default footer label and link.

When a template is supplied, it takes precedence over the corresponding label and identity properties.
This makes it possible to add components such as menus, links, status details, or application-specific actions while retaining the profile popover structure.

For complete control, place any Razor content directly inside `FluentProfileMenu` instead of using `FluentProfilePopover`.

{{ FluentProfilePopoverDefault }}

## API FluentProfileMenu

{{ API Type=FluentProfileMenu }}

## API FluentProfilePopover

{{ API Type=FluentProfilePopover }}
