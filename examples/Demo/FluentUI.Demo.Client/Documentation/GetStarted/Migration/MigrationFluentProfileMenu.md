---
title: Migration FluentProfileMenu
route: /Migration/ProfileMenu
hidden: true
---

# FluentProfileMenu

In V4, `FluentProfileMenu` combined the avatar button, profile details, header,
footer, and popover in one component. In V5, these responsibilities are split
between two components:

- `FluentProfileMenu` provides the avatar button, presence badge, open state,
  and popover.
- `FluentProfilePopover` provides the optional header, profile details, and
  footer layout displayed inside the menu.

Nest `FluentProfilePopover` inside `FluentProfileMenu` to reproduce the
standard V4 profile menu.

## Basic migration

```razor
<!-- V4 -->
<FluentProfileMenu FullName="@user.Name"
                   EMail="@user.Email"
                   Image="@user.ImageUrl"
                   Initials="@user.Initials"
                   ButtonSize="32px"
                   ImageSize="64px"
                   HeaderLabel="@user.Company"
                   HeaderButton="Sign out"
                   OnHeaderButtonClick="@SignOut"
                   FooterLink="View account"
                   OnFooterLinkClick="@ViewAccount"
                   Status="@PresenceStatus.Available"
                   @bind-Open="@isOpen" />
```

```razor
<!-- V5 -->
<FluentProfileMenu Title="@user.Name"
                   ImageUrl="@user.ImageUrl"
                   ImageInitials="@user.Initials"
                   ImageSize="@AvatarSize.Size32"
                   Status="@PresenceStatus.Available"
                   @bind-Open="@isOpen">
    <FluentProfilePopover FullName="@user.Name"
                          EMail="@user.Email"
                          ImageUrl="@user.ImageUrl"
                          ImageInitials="@user.Initials"
                          ImageSize="@AvatarSize.Size64"
                          HeaderLabel="@user.Company"
                          HeaderButtonLabel="Sign out"
                          OnHeaderButtonClick="@SignOut"
                          FooterLinkLabel="View account"
                          OnFooterLinkClick="@ViewAccount" />
</FluentProfileMenu>
```

Set `Title` on `FluentProfileMenu` to provide an accessible label for the
avatar button.

## Parameter mapping

| V4 `FluentProfileMenu` | V5 component | V5 parameter or action |
| --- | --- | --- |
| `Open` / `OpenChanged` | `FluentProfileMenu` | Unchanged; `@bind-Open` is still supported. |
| `StartTemplate` | `FluentProfileMenu` | Unchanged. |
| `EndTemplate` | `FluentProfileMenu` | Unchanged. |
| `Status` | `FluentProfileMenu` | Unchanged. |
| `StatusTitle` | `FluentProfileMenu` | Unchanged. |
| `ButtonSize` | `FluentProfileMenu` | Rename to `ImageSize` and use an `AvatarSize` value. |
| `Image` | Both components | Rename to `ImageUrl` and set it on the menu and popover. |
| `Initials` | Both components | Rename to `ImageInitials` and set it on the menu and popover. |
| `ImageSize` | `FluentProfilePopover` | Keep the name, but replace the CSS string with an `AvatarSize` value. |
| `FullName` | `FluentProfilePopover` | Move to the nested popover. |
| `EMail` | `FluentProfilePopover` | Move to the nested popover. |
| `HeaderLabel` | `FluentProfilePopover` | Move to the nested popover. |
| `HeaderButton` | `FluentProfilePopover` | Rename to `HeaderButtonLabel`. |
| `OnHeaderButtonClick` | `FluentProfilePopover` | Move to the nested popover. The callback can receive `MouseEventArgs`. |
| `FooterLabel` | `FluentProfilePopover` | Move to the nested popover. |
| `FooterLink` | `FluentProfilePopover` | Rename to `FooterLinkLabel`. |
| `OnFooterLinkClick` | `FluentProfilePopover` | Move to the nested popover. The callback can receive `MouseEventArgs`. |
| `HeaderTemplate` | `FluentProfilePopover` | Move to the nested popover. |
| `FooterTemplate` | `FluentProfilePopover` | Move to the nested popover. |
| `ChildContent` | `FluentProfilePopover` | Move it into the nested popover to replace only the profile details. |
| `PopoverClass` / `PopoverStyle` | — | Removed. Style the nested `FluentProfilePopover`, or compose `FluentPopover` directly when its container must be customized. |
| `TopCorner` | — | Removed. The V5 popover positions itself relative to the avatar and available viewport space. |

V5 also adds `StatusSize` to `FluentProfileMenu`, and `Width` and `Height` to
`FluentProfilePopover`.

## Migrating custom content

In V4, `ChildContent` replaced only the body of the built-in profile layout.
In V5, direct child content of `FluentProfileMenu` represents the complete
popover content. To keep separate header, body, and footer sections, move the
existing templates into a nested `FluentProfilePopover`.

```razor
<!-- V4 -->
<FluentProfileMenu Image="@user.ImageUrl">
    <HeaderTemplate>
        <MyProfileHeader />
    </HeaderTemplate>
    <ChildContent>
        <MyProfileDetails />
    </ChildContent>
    <FooterTemplate>
        <MyProfileActions />
    </FooterTemplate>
</FluentProfileMenu>
```

```razor
<!-- V5 -->
<FluentProfileMenu ImageUrl="@user.ImageUrl"
                   Title="@user.Name">
    <FluentProfilePopover>
        <HeaderTemplate>
            <MyProfileHeader />
        </HeaderTemplate>
        <ChildContent>
            <MyProfileDetails />
        </ChildContent>
        <FooterTemplate>
            <MyProfileActions />
        </FooterTemplate>
    </FluentProfilePopover>
</FluentProfileMenu>
```

If the existing V4 `ChildContent` already renders the complete menu, place it
directly inside `FluentProfileMenu` and omit `FluentProfilePopover`.

## Changed defaults and markup

- `HeaderButton` defaulted to `"Sign out"` and `FooterLink` defaulted to
  `"View account"` in V4. Their V5 replacements have no default text, so set
  `HeaderButtonLabel` and `FooterLinkLabel` explicitly when those actions are
  required.
- The V4 implementation used `FluentPersona`. V5 uses `FluentAvatar` and
  `FluentPresenceBadge`, so CSS selectors that target the old generated markup
  or its `part` attributes must be updated.
- V4 size parameters accepted CSS strings such as `"32px"`. V5 avatar sizes use
  the `AvatarSize` enum.

See the [FluentProfileMenu documentation](/ProfileMenu) for examples and the
current APIs.
