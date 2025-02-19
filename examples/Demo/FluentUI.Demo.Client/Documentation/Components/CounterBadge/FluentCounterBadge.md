---
title: CounterBadge
route: /CounterBadge
---

# CouterBadge

The `CounterBadge` component is a visual indicator that communicates a numerical count.
It uses numbers, color, and icons for quick recognition and is placed near the relavant content.

## Best practices

The same principles that apply to [badges](/Badge) apply to `CounterBadge` as well.

### Numbers on Badge
CounterBadges are intented to have short, small numerical values or status information. An `OverflowCount` can be set as the maximum number that is shown in the CounterBadge.
It will add a `+` sign to the end of the number to indacate that the number is larger than the `OverflowCount`.

### Content

The CounterBadge does not have a content slot. It can only show a count (through the `Count` parameter) If you need to add content, you can use the `Badge` component.

## Default

- First example shows an empty counter badge. 
- Second example shows a counter badge with a count.
- Third example shows a count higher than the overflow (default is 99)

{{ CounterBadgeDefault }}

## ShowZero
A counter badge can be shown even if the count is zero.

{{ CounterBadgeShowZero }}

## ShowWhen
A counter badge can be shown when a specific condition (lambda expression) is met.
For instance, `ShowWhen=@(Count => Count > 4)` will show the badge only when the count is greater than 4.

- First example count=3. With rule set like above it will not show
- Second example count=5. With rule set like above it will show

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
**Ghost and Tint are not supported in CounterBadge**.

{{ CounterBadgeAppearances }}

## Shape
The counter shape can be set to a value from the `BadgeShape` enumeration.
**Square is not supported in CounterBadge**.

{{ CounterBadgeShapes }}

## Size
The counter badge size be set to a value from  the `BadgeSize` enumeration.

{{ CounterBadgeSizes }}

## API FluentButton

{{ API Type=FluentCounterBadge }}

## Migrating to v5

{{ INCLUDE File=MigrationFluentCounterBadge }}
