---
title: Presence Badge
route: /Badges/PresenceBadge
icon: PresenceAvailable
---


A presence badge is a badge that displays a status indicator such as available, away, or busy.

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

{{ API Type=FluentPresenceBadge }}
