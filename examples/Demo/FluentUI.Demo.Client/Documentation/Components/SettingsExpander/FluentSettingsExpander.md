---
title: SettingsExpander
route: /SettingsExpander
icon: Settings
---

# SettingsExpander

A **FluentSettingsExpander** presents a setting like the tiles in the Windows 11 settings app.
It separates the informational content (icon, title and description), the main action and an optional expandable panel.

## Default

{{ SettingsExpanderDefault }}

## Expandable

When `ChildContent` is provided, an expand/collapse button is displayed.
Nested `FluentSettingsExpander` components are rendered as rows inside the panel.
Use `@bind-Expanded` to control the state.

{{ SettingsExpanderExpandable }}

## Dropdowns and sliders

{{ SettingsExpanderVerbosity }}

## Switches, buttons and links

Settings can host any action, like a button or a switch. When `Href` is set, the whole header becomes a link
and an "open" icon is displayed at the end.

{{ SettingsExpanderManageData }}

## Disabled

{{ SettingsExpanderDisabled }}

## API FluentSettingsExpander

{{ API Type=FluentSettingsExpander }}
