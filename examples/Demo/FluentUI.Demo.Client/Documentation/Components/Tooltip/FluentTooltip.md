---
title: Tooltip
route: /Tooltip
---

# Tooltip

A `FluentTooltip` displays additional information about another component. The information is displayed above and near the target component.

**Tooltip** is not expected to handle interactive content. If this is necessary behavior, an expand/collapse button + popover should be used instead.

Hover or focus the buttons in the examples to see their tooltips.

## Default

{{ TooltipDefault }}

## Customized
The tooltip can be customized with a custom template,
adapting the delay to `700 ms`, the position of the tooltip to `BelowStart` (including spacings to `20px` and `10px`)
and the style "Inverted".

Each time the tooltip is shown or hidden, a message is logged in the console. Click on the top/right button to open the **Console**.

{{ TooltipCustomized }}

## API FluentTooltip

{{ API Type=FluentTooltip }}

