---
title: Presence Badge
route: /Badges/PresenceBadge
icon: PresenceAvailable
---


A presence badge is a badge that displays a status indicator such as available, away, or busy.

## Change strings used in the UI

The AppBar uses a string in the UI. It's value can be changed by leveraging the built-in [localization](/localization) functionality.
The following values can be localized (default value in brackets) :

- PresenceStatus_Available ("available")
- PresenceStatus_Away ("away")
- PresenceStatus_Blocked ("blocked")
- PresenceStatus_Busy ("busy")
- PresenceStatus_DoNotDisturb ("do not disturb")
- PresenceStatus_Offline ("offline")
- PresenceStatus_OutOfOffice ("out of office")
- PresenceStatus_Unknown ("unknown")

## Examples

### Default

{{ PresenceBadgeDefault }}

### Sizes

A presence badge supports all `BadgeSize` values. The default value is `BadgeSize.Medium`.

{{ PresenceBadgeSize }}

### Status

A presence badge status is set by mean of the `PresenceStatus` enumeration. See the documentation below for the possible values. The default is `PresenceStatus.Available`.

{{ PresenceBadgeStatus }}

### Out Of Office

A presence badge can indicate if a user is out of office by setting the `OutOfOffice` parameter to `true`. This will change the appearance of the badge to indicate the out of office status.

{{ PresenceBadgeStatusOOO }}

### On Avatar

The Avatar component is tailored to work with the PresenceBadge component. The PresenceBadge needs to be supplied as the `ChildContent` of the Avatar component and the value of the `Slot` parameter needs to be set to `FluentSlot.Badge`.

{{ PresenceBadgeOnAvatar }}

_Due to an issue in the Avatar Component the initials are not show at the moment. A fix is being worked on._

### On Avatar with OutOfOffice

The Avatar component is tailored to work with the PresenceBadge component. The PresenceBadge needs to be supplied as the `ChildContent` of the Avatar component and the value of the `Slot` parameter needs to be set to `FluentSlot.Badge`.

{{ PresenceBadgeOnAvatarOOO }}

_Due to an issue in the Avatar Component the initials are not show at the moment. A fix is being worked on._

## API FluentPresenceBadge

The PresenceBadge is a specialized Badge component and does **not** support the following parameters:

-  Appearance
-  BackgroundColor
-  Color
-  Content
-  IconEnd
-  IconLabel
-  IconStart
-  Shape

{{ API Type=FluentPresenceBadge }}
