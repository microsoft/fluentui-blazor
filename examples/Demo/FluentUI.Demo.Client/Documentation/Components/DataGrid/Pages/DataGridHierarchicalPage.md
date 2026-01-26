---
title: Hierarchical data
route: /DataGrid/Hierarchical
---

# Hierarchical data

## Single-level hierarchy

The `FluentDataGrid` supports displaying hierarchical data.

By using the `HierarchicalGridItem` class (or a class derived from it) as the grid item type, 
you can define parent-child relationships between items.

Set the `HierarchicalToggle` parameter to `true` on one of the columns to display the expand/collapse button.

{{ DataGridHierarchical Files=Code:DataGridHierarchical.razor;CSS:DataGridHierarchical.razor.css}}

## Multi-level hierarchy

The hierarchical data grid can display multiple levels of nesting. In this example, we show an organization chart with three levels: CEO, Directors, and Employees.

This example also shows how to programmatically expand and collapse rows by manually setting the `IsCollapsed` property on a `HierarchicalGridItem` instance. 

{{ DataGridHierarchicalOrgChart Files=Code:DataGridHierarchicalOrgChart.razor}}

