---
title: SettingsCard
route: /SettingsCard
icon: Settings
---

# SettingsCard

A **FluentSettingsCard** looks like the tiles in the Windows 11 settings app.
It shows an icon, a header, a description and an action area (for example a dropdown, switch or slider).

## Default

{{ SettingsCardDefault }}

## Expandable

When `ChildContent` is provided, a chevron is shown to expand the card.
Nested `FluentSettingsCard` components are rendered as rows inside the expanded area.

{{ SettingsCardExpandable }}

## Checkboxes and dropdowns

Use `HeaderTemplate` to replace the header text with custom content, such as a checkbox.

{{ SettingsCardVerbosity }}

## Switches, buttons and links

Cards can host any action, like a button or a switch. When `Href` is set, the whole card becomes a link
and an "open" icon is displayed at the end.

{{ SettingsCardManageData }}
