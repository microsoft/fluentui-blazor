---
title: Blazor Project Structure
impact: MEDIUM-HIGH
impactDescription: Poor structure makes code hard to find and maintain
tags: blazor, structure, folders, solid
---

## Component Structure

Organize files following [SOLID](https://en.wikipedia.org/wiki/SOLID) principles — create autonomous and extensible components.

### Folder Rules

- Group pages and dialogs of the **same module** in a sub-folder of **Pages** folder, with the module name (pluralized): `Pages\Calendars\`.
- Suffix pages with **Page.razor** and dialogs with **Dialog.razor** (singular): `CalendarMainPage.razor`, `CalendarEditDialog.razor`.
- Dialog box logic (saving data, etc.) must be in the dialog class itself (e.g., `OnCloseAsync`). Don't write dialog logic in the calling page.
- Shared components go in **Components** folder, suffixed with `Component`.

### Example Structure

```
|-- App.razor
|-- Components
|   |-- TimeComponent.razor
|   |-- TimeComponent.razor.cs
|-- Pages
|   |-- Calendars
|   |   |-- CalendarMainPage.razor
|   |   |-- CalendarMainPage.razor.cs
|   |   |-- CalendarMainPage.razor.css
|   |   |-- CalendarMainPage.razor.js
|   |   |-- CalendarEditDialog.razor
|   |   |-- CalendarEditDialog.razor.cs
|   |   |-- CalendarEditDialog.razor.css
```

### File Nesting

In VS Code: `"explorer.fileNesting.patterns": { "$(capture).razor": "$(capture).razor.*" }`
