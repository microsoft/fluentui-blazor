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
orders.

Unlike the [hierarchical view](/DataGrid/Hierarchical) — where parent and child rows share the same item type and
the same columns within a single grid — the master/detail view displays data of a completely different structure:
the child grid defines its own columns, sorting and content, and is simply filtered by the master row's item.

Rows can also be expanded and collapsed programmatically with the `ToggleRowDetailsAsync`, `ExpandRowDetailsAsync`,
`CollapseRowDetailsAsync`, `ExpandAllRowDetailsAsync` and `CollapseAllRowDetailsAsync` methods. The
`OnRowDetailsToggle` event callback is raised for every row whose expansion state actually changes — including once
per affected row when using `ExpandAllRowDetailsAsync`/`CollapseAllRowDetailsAsync` — but not for rows that were
already in the requested state.

*Note: `RowDetails` cannot be combined with `Virtualize`.*

{{ DataGridMasterDetail Files=Code:DataGridMasterDetail.razor }}

## Two levels of detail

Since the detail template can contain any content, a child `FluentDataGrid` can itself define a `RowDetails`
template, giving as many nested levels as needed. In this example, expanding a customer shows its orders, and
expanding an order shows its order lines.

Each level filters its own child grid with its own row item (the `Context` of each template is named explicitly —
`customer`, then `order` — to avoid ambiguity between the nested templates).

{{ DataGridMasterDetailTwoLevels Files=Code:DataGridMasterDetailTwoLevels.razor }}
