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
- [Funnel Chart](/Charts/FunnelChart)
- [Horizontal Bar Chart](/Charts/HorizontalBarChart)
- [Horizontal Bar Chart with Axis](/Charts/HorizontalBarChartWithAxis)

## Fluent Chart Base

All charts in the Fluent UI Charts package inherit from the `FluentChartBase` component, which provides common parameters for all chart types.

There are common parameters defined that do not apply to all chart types

## Accessibility

All charts in the Fluent UI Charts package are designed with accessibility in mind. They include appropriate ARIA attributes and support
keyboard navigation to ensure that they are usable by all users, including those with disabilities.

For the chart legends, you can navigate through the items with the arrow keys. The corresponding chart element (arc, bar, etc.) will be highlighted and all
other elements will appear dimmed. It is also possible to select one (or more, depending on the `EnableMultipleSelection`parameter) legend items and
corresponding chart elements by using the space bar/enter key when focused. Press space bar/enter key again to toggle all items to an unselected state.

## Colors

The charts use the Fluent UI DataViz color palette by default, but you can customize the colors of the chart segments by providing your own color palette.
You can also specify a custom color for each segment in the data points.

{{ API Type=DataVizPalette Properties=all }}
