---
title: LabelInfo
route: /LabelInfo
icon: Info
---

# LabelInfo

An **InfoLabel** is a Label with an InfoButton at the end, properly handling layout and accessibility properties.
It can be used as a drop-in replacement for Label when an InfoButton is also needed.

The **InfoLabel** pattern is a label followed by a button that exposes additional information about a field or a concept.
To trigger the Popover, the user may activate the button by clicking on it and focusing on it and pressing enter or space. 
To close the Popover, the user may click the button again, click outside the popover, press the escape key, or tab out of the Popover.

**InfoLabel** can not be opened on focus. The pattern where you have an icon and a tooltip that appears on focus is not the InfoLabel pattern. 
The tooltip pattern is meant to have short text and no interaction with the content. We believe that if the content is short or even a few words,
it should be included in the label or a secondary label. If the content is longer and/or has interaction, then it must be an InfoLabel.

## Examples

The simplest usage sets `InfoText` to display plain text inside the popover, and optionally `InfoActionLink` to render a "Learn more" link below it.

{{ LabelInfoDefault }}

Use the `InfoTemplate` to provide fully customized content inside the popover, such as formatted text, links, or any other Razor markup.

{{ LabelInfoCustomized }}

The default info icon can be replaced with any icon (for example a question mark, lightbulb, or warning) 
by setting `InfoIcon` and `InfoIconActive` to suit the context of the field.

{{ LabelInfoIcons }}

A `FluentLabelInfo` can be used as the label of a form field (such as `FluentTextInput`) 
by assigning it to the `LabelTemplate`, keeping accessibility and layout properly handled.

{{ LabelInfoField }}

## API FluentLabelInfo

{{ API Type=FluentLabelInfo }}
