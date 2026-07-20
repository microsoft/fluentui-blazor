---
title: Master / Detail
route: /DataGrid/MasterDetail
---

# Master / Detail

The `FluentDataGrid` supports a master/detail view. Set the `RowDetails` template parameter to make each row
expandable through a chevron button shown in the first column. The template content is rendered in an extra row,
spanning all columns, directly below the expanded row.

The template's `context` is the master row's item, so you can place a child `FluentDataGrid` inside it and filter
its items based on the master row. In this example, expanding a customer shows a child grid with that customer's
orders. Not every row needs to have detail data: Tailspin Toys has no orders yet, so expanding it shows the child
grid's `EmptyContent` instead.

Unlike the [hierarchical view](/DataGrid/Hierarchical) — where parent and child rows share the same item type and
the same columns within a single grid — the master/detail view displays data of a completely different structure:
the child grid defines its own columns, sorting and content, and is simply filtered by the master row's item.

Rows can also be expanded and collapsed programmatically with the `ToggleRowDetailsAsync`, `ExpandRowDetailsAsync`,
`CollapseRowDetailsAsync`, `ExpandAllRowDetailsAsync` and `CollapseAllRowDetailsAsync` methods. The
`OnRowDetailsToggle` event callback is raised for every row whose expansion state actually changes — including once
per affected row when using `ExpandAllRowDetailsAsync`/`CollapseAllRowDetailsAsync` — but not for rows that were
already in the requested state.

>[!WARNING] The `RowDetails` parameter cannot be used when the `Virtualize` parameter is set to `true`.

{{ DataGridMasterDetail Files=Code:DataGridMasterDetail.razor }}

## Any content as detail

The detail template is not limited to a child `FluentDataGrid`: it can host any content. Because the template's
`context` is the master row's item, you can build a fully custom detail panel — cards, stacks, icons, links,
progress bars, buttons or any other component — bound to that item. In this example, expanding a customer shows a
contact card built from standard Fluent components instead of a nested grid.

{{ DataGridMasterDetailCustom Files=Code:DataGridMasterDetailCustom.razor }}

## Two levels of detail

Since the detail template can contain any content, a child `FluentDataGrid` can itself define a `RowDetails`
template, giving as many nested levels as needed. In this example, expanding a customer shows its orders, and
expanding an order shows its order lines.

Each level filters its own child grid with its own row item (the `Context` of each template is named explicitly —
`customer`, then `order` — to avoid ambiguity between the nested templates).

{{ DataGridMasterDetailTwoLevels Files=Code:DataGridMasterDetailTwoLevels.razor }}
