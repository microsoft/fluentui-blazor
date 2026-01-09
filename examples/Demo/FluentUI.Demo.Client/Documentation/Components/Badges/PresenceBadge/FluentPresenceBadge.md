---
title: Presence Badge
route: /Badges/PresenceBadge
icon: PresenceAvailable
---


A presence badge is a badge that displays a status indicator such as available, away, or busy.

## Change strings used in the UI

The AppBar uses a string in the UI. It's value can be changed by leveraging the built-in [localization](/localization) functionality.
The following values can be localized (the default value is shown between brackets) :

- PresenceStatus_Available ("Available")
- PresenceStatus_Away ("Away")
- PresenceStatus_Busy ("Busy")
- PresenceStatus_DoNotDisturb ("Do not disturb")
- PresenceStatus_Offline ("Offline")
- PresenceStatus_Unknown ("Unknown")
- PresenceStatus_OutOfOffice ("Out of office")
- PresenceStatus_Blocked ("Blocked")


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

Using a presence badges with posible statusses used on an avatar component.

{{ PresenceBadgeOnAvatar }}

_Due to a bug in the Avatar Web Component the initials are not show at the moment. A fix is under review._

### On Avatar with OutOfOffice

Using a presence badges with `OutOfOffice="true"` and all posible statusses used on an avatar component.

{{ PresenceBadgeOnAvatarOOO }}

_Due to a bug in the Avatar Web Component the initials are not show at the moment. A fix is under review._

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
