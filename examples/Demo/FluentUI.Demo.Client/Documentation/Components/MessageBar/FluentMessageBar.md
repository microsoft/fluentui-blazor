---
title: MessageBar
route: /MessageBar
---

# MessageBar

Communicates important information about the state of the entire application or surface.
For example, the status of a page, panel, dialog or card.
The information shouldn't require someone to take immediate action, but should persist until
the user performs one of the required actions.

## Appearance

**FluentMessageBar** components come built-in with preset intents that determine the design and aria live announcement.

{{ MessageBarAppearance }}

You can also use the `Shape` parameter to change the shape of the corners of the message bar: square or rounded.

## Actions

The **FluentMessageBar** can have different actions.
The `Layout` parameter allows you to choose the position of the actions:
  - **SingleLine**: Next to the message content, allowing for a compact layout.
  - **MultiLine**: On a new line, allowing for more space for the message content.

{{ MessageBarActions }}
