---
title: Charts
route: /Charts/[Default]
icon: ChartMultiple
---

# Charts

The Fluent UI Charts are a set of Razor components that allow you to easily use charts in your Blazor applications. The Charts are not part of the core
Fluent UI Blazor package, but are available as a separate package (`Microsoft.FluentUI.AspNetCore.Components.Charts`). This allows us to keep the core
package lightweight and focused on the most commonly used components, while still providing a rich set of charting options for those who need them.

In the future, the package will be extended with more chart types (based on the Fluent UI React v9 Charts package).

Currently, the following chart types are available:

- [Donut Chart](/Charts/DonutChart)
- [Horizontal Bar Chart](/Charts/HorizontalBarChart)
- [Horizontal Bar Chart with Axis](/Charts/HorizontalBarChartWithAxis)

## Fluent Chart Base

All charts in the Fluent UI Charts package inherit from the `FluentChartBase` component, which provides common parameters for all chart types.
