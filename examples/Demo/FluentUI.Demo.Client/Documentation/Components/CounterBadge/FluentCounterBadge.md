---
title: CounterBadge
route: /CounterBadge
---

# CouterBadge

The `CounterBadge` component is a visual indicator that communicates a numerical count.
It uses numbers, color, and icons for quick recognition and is placed near the relavant content.

Typically a `CounterBadge` 'wraps' a component, such as a `FluentButton`, to indicate a count (>0) that represents a status.

There are 9 possible positions for a badge to attach to the content it wraps:

{{ BadgePositions SourceCode=false }}

## Best practices

The same principles that apply to [badges](/Badge) apply to `CounterBadge` as well.

### Numbers on Badge
CounterBadges are intented to have short, small numerical values or status information.
An `OverflowCount` can be set as the maximum number that is shown in the CounterBadge.
It will add a `+` sign to the end of the number to indacate that the number is larger than the `OverflowCount`.

### Content

The CounterBadge can only show a count (through the `Count` parameter).
If you need to add textual content, you can use the `Badge` component.

## Default

- First example shows an empty counter badge. 
- Second example shows a counter badge with a count.
- Third example shows a count higher than the overflow (default is 99)

{{ CounterBadgeDefault }}


## Attached to content
A counter badge can be attached to any content. The default position is above-end. The position can be changed using the `Positioning` parameter.
The `OffestX` and `OffsetY` allow for tuning the positioning further.

{{ CounterBadgeAttached SourceCode=false}}

## ShowZero
A counter badge can be shown even if the count is zero.

{{ CounterBadgeShowZero }}

## ShowWhen
A counter badge can be shown when a specific condition (lambda expression) is met.
For instance, `ShowWhen=@(c => c > 4)` will show the badge only when the count is greater than 4.

- First example `Count="3"`. With rule set like above it will not show
- Second example `Count="5"`. With rule set like above it will show

{{ CounterBadgeShowWhen }}

## Dot
A counter badge can be shown as a dot. This can be used, for example, to indicate there are new items but the actual number is not really important.

{{ CounterBadgeDot }}

## Colors

The counter badge colors be set to a value from the `BadgeColor` enumeration. The last badge shows how a custom background color can be used.

{{ CounterBadgeColors }}

## Icons
An `Icon` can be shown in the start or end slot (or both). The color of the icon is coupled to the color of the text in the badge.

{{ CounterBadgeIcons }}

## Appearance

The counter badge appearance can be set to a value from the `BadgeAppearance` enumeration.
**CounterBadge does not support Ghost and Tint appearances**.

{{ CounterBadgeAppearances }}

## Shape
The counter shape can be set to a value from the `BadgeShape` enumeration.
**CounterBadge does not support the Square shape**.

{{ CounterBadgeShapes }}

## Size
The counter badge size be set to a value from  the `BadgeSize` enumeration.

{{ CounterBadgeSizes }}

## API FluentButton

{{ API Type=FluentCounterBadge }}

## Migrating to v5

{{ INCLUDE File=MigrationFluentCounterBadge }}
