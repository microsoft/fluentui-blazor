---
title: Badge
route: /Badge
---

# Badge

The `Badge` component is a visual indicator that communicates a status or description of an associated component.
It uses short text, color, and icons for quick recognition and is placed near the relavant content.

## Best practices

### Badges should not receive focus
- Badge information should be surfaced as part of the control that it is associated with, because, badges themselves do not receive focus meaning they are not directly accessible by screen readers. If the combination of icon and badge communicates some meaningful information, that information should be surfaced in another way through screenreader or tooltip on the component the badge is associated with.

### Screen Readers
Badge content is exposed as text, and is treated by screen readers as if it were inline content of the control it is associated with.
This should provide a reasonable default for most badges that contain plain text, such as the `CounterBadge`.

There are two actions authors should consider taking when using Badge to improve this experience:

1. If the badge contains a custom icon, that icon must be given alternative text with aria-label, unless it is purely presentational:
   ```
   <FluentBadge IconLabel="paste" />
   ```

1. If the text of the badge itself is not sufficient to convey its meaning, it can either be given additional hidden text, or the parent element given an explicit label:
    ```
    <FluentButton>
        Inbox
        <FluentBadge>6<span class="visuallyHidden"> unread messages</span></FluentBadge>
    </FluentButton>

    Or

    <FluentButton aria-label="Inbox, 6 unread messages">
        Inbox
        <FluentBadge>6</FluentBadge>
    </FluentButton>
    ```     

### Badge shouldn't rely only on color information
Include meaningful descriptions when using color to represent meaning in a badge. If relying on color only, ensure that non-visual information is included in the parent's label or description. 

### Text on Badge
Badges are intented to have short text, small numerical values or status information. Long text is not supported and should not be used within a Badge.

## Default

First example shows an empty badge. Second example shows a badge with content.

{{ BadgeDefault }}


## Colors

The badge colors be set to a value from the `BadgeColor` enumeration. The last badge shows how a custom background color can be used.

{{ BadgeColors }}

## Icons
An `Icon` can be shown in the start or end slot (or both). The color of the icon is coupled to the color of the text in the badge.

{{ BadgeIcons }}

## Appearance

The badge appearance can be set to a value from the `BadgeAppearance` enumeration.

{{ BadgeAppearances }}

## Shape
The badge badge shape can be set to a value from the `BadgeShape` enumeration.

{{ BadgeShapes }}

## Size
The badge size be set to a value from  the `BadgeSize` enumeration.

{{ BadgeSizes }}

## API FluentButton

{{ API Type=FluentBadge }}

## Migrating to v5

{{ INCLUDE File=MigrationFluentBadge }}
