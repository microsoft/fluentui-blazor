---
title: Pinned columns
order: 0095
route: /DataGrid/PinnedColumns
---

# Pinned columns

Columns can be pinned (frozen) to the left or right edge of the grid so that they remain visible
while the user scrolls horizontally through wider datasets.

## Parameters

Set the `Pin` parameter on any `PropertyColumn` or `TemplateColumn`:

| Value | Behavior |
|---|---|
| `DataGridColumnPin.None` | Default — column scrolls normally |
| `DataGridColumnPin.Left` | Column stays anchored to the left edge |
| `DataGridColumnPin.Right` | Column stays anchored to the right edge |

## Rules

* **Explicit pixel width required.** Every pinned column must declare a `Width` in pixels
  (e.g. `Width="150px"`). Relative units (`fr`, `%`) are not supported because the browser cannot
  determine a fixed sticky offset from them at render time.
* **Left-pinned columns must be contiguous at the start.** Each left-pinned column must
  immediately follow another left-pinned column, or be the very first column.
* **Right-pinned columns must be contiguous at the end.** Each right-pinned column must
  immediately precede another right-pinned column, or be the very last column.
* Violating any of these rules throws an `ArgumentException` with a descriptive message.

## Scrollable container

Sticky positioning only activates inside a scrollable ancestor. Wrap the grid in a container with
`overflow-x: auto` and give the grid `Style="min-width: max-content;"` so that a horizontal scroll
bar appears when columns overflow the container:

```razor
<div style="overflow-x: auto;">
    <FluentDataGrid Items="@employees" Style="min-width: max-content;">
        <PropertyColumn Title="ID"       Property="@(e => e.Id)"     Width="60px"  Pin="DataGridColumnPin.Left" />
        <PropertyColumn Title="Name"     Property="@(e => e.Name)"   Width="160px" Pin="DataGridColumnPin.Left" />
        <PropertyColumn Title="City"     Property="@(e => e.City)" />
        <TemplateColumn Title="Actions"  Width="120px" Pin="DataGridColumnPin.Right">
            ...
        </TemplateColumn>
    </FluentDataGrid>
</div>
```

## Theming the pinned background

Pinned cells receive a solid background to prevent scrolling content from showing through. The
color defaults to `--colorNeutralBackground1` and can be overridden per-grid with the CSS custom
property `--fluent-data-grid-pinned-background`:

```css
.my-grid {
    --fluent-data-grid-pinned-background: var(--colorNeutralBackground3);
}
```

## Notes

* Column resizing interacts correctly with sticky offsets — the JavaScript in
  `FluentDataGrid.razor.ts` recalculates `left` / `right` values after every resize step via
  `UpdatePinnedColumnOffsets`.
* Virtualization and paging are fully compatible because each rendered row's cells carry the
  same `position: sticky` styling regardless of which page or scroll position is active.
* In RTL layouts the browser interprets `left` / `right` according to the document direction, so
  pinned columns behave correctly without additional configuration.

{{ DataGridPinnedColumns }}
