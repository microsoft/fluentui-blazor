---
title: Gantt Chart
route: /Charts/GanttChart
---

# Gantt Chart

A Gantt chart is a type of bar chart that visualizes a project schedule. Each bar represents a task or activity with a defined start and end point along a time or numeric axis, while the vertical axis shows the categories (tasks or resources).

Gantt charts are especially useful for showing overlapping activities, project timelines, and resource allocation across multiple categories.

## Layout

Each bar spans from its start to its end value on the x-axis. The y-axis lists the categories. When multiple data points share the same y-axis category, they are overlaid in the same row, distinguished by color and legend.

Bar height can be customized via the `BarHeight` property. The default is determined by the component layout.

## Content

- **Bars** — Each bar represents a single data point with a start and end x-value. Bars in the same y-axis category are drawn in the same row.
- **Legends** — Each unique legend value gets a distinct color. Clicking a legend item filters the visible bars.
- **Axis labels** — X-axis ticks show time or numeric values. Y-axis labels show category names when `ShowYAxisLabels` is enabled.
- **Tooltips** — Hovering a bar shows the category, legend, start, and end values.

## Accessibility

- All bars are keyboard-navigable and screen-reader accessible.
- Use `XAxisTitle` and `YAxisTitle` to provide descriptive axis context for assistive technologies.

## Do's

- Use a date x-axis when visualizing project schedules or time-based data.
- Use a numeric x-axis when the range values are scalar (e.g. durations or offsets).
- Set `ShowYAxisLabels` to make category names visible without requiring the legend.

## Don'ts

- Avoid too many overlapping bars per category row — use `AllowMultipleLegendSelection` to let users filter.

## Examples

### Default

The default example shows a simple Gantt chart with a date x-axis and three tasks distributed across two assignees.

{{ GanttChartDefault }}

### Grouped

The grouped example uses the same date axis but includes multiple assignee groups with semantic colors (success, warning, error), demonstrating how overlapping bars in the same category row are distinguished by legend and color.

{{ GanttChartGrouped }}

### Numeric axis

When x-axis values are numbers rather than dates the chart automatically switches to a numeric scale. This is useful for representing durations, offsets, or any scalar range.

{{ GanttChartNumericAxis }}

### Hide legends

Setting `HideLegends` removes the legend list below the chart, reducing visual clutter when the category labels on the y-axis already convey the necessary context.

{{ GanttChartHideLegends }}

### Category order

Use the selector to change the ordering of the y-axis categories at runtime, illustrating how the `YAxisCategoryOrder` property rearranges bars without changing the underlying data.

{{ GanttChartCategoryOrder }}

### Axis titles

Setting `XAxisTitle` and `YAxisTitle` adds descriptive labels to both axes, providing additional context especially when the chart is used in isolation on a page.

{{ GanttChartAxisTitles }}

### Tick format

The `XAxisTickFormat` property accepts a D3 format specifier string. Setting it to `.1f` displays all numeric x-axis tick values with one decimal place.

{{ GanttChartTickFormat }}

### Tick padding

Use the slider to adjust the `TickPadding` property, which controls the pixel gap between axis tick marks and their labels.

{{ GanttChartTickPadding }}

### Rotate x-axis labels

Enabling `RotateXAxisLabels` tilts the x-axis tick labels to prevent overlap when the axis is dense or the labels are long.

{{ GanttChartRotateXAxisLabels }}

### Support negative data

Setting `SupportNegativeData` allows bars to start at or extend into negative x-axis territory. The zero baseline is visible and bars on both sides are rendered correctly.

{{ GanttChartSupportNegativeData }}

### Rounded ticks

Enabling `RoundedTicks` applies D3's `scale.nice()` to the x-axis domain, rounding the outer tick values to clean multiples for a more readable axis.

{{ GanttChartRoundedTicks }}

### Tick values (numeric axis)

Set `TickValues` to an explicit array of doubles to control exactly which values appear as tick marks on a numeric x-axis, overriding the auto-generated ticks.

{{ GanttChartTickValues }}

### Date tick values (date axis)

Set `DateTickValues` to an explicit array of `DateTime` values to control exactly which dates appear as tick marks on a date x-axis. The values are automatically converted to the Unix millisecond timestamps expected by the web component.

{{ GanttChartDateTickValues }}

### Tick format (placeholder)

`TickFormat` accepts a d3-time-format specifier (e.g. `%m/%d`) for date x-axis tick labels. This attribute is **reserved for future d3-time-format support** and currently has no visual effect. Use `DateLocalizeOptions` together with `Culture` to customise date formatting today.

{{ GanttChartDateTickFormat }}

### Stroke width

Set `StrokeWidth` to add an outline stroke to each bar. Use the slider to adjust the width in pixels at runtime.

{{ GanttChartStrokeWidth }}

### X-axis labels tooltip

Setting `ShowXAxisLabelsTooltip` truncates x-axis tick labels that exceed ten characters and shows the full text in a tooltip on hover. In this example, `DateLocalizeOptions` is set to produce full month names (e.g. "September 1") that trigger the truncation.

{{ GanttChartShowXAxisLabelsTooltip }}

### Date localize options

`DateLocalizeOptions` lets you supply an `Intl.DateTimeFormat`-compatible options object to control how date x-axis tick labels are formatted. Keys and values must match the [MDN Intl.DateTimeFormat options](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Intl/DateTimeFormat/DateTimeFormat). When not set the component auto-selects a format based on the visible date range.

{{ GanttChartDateLocalizeOptions }}

## API Fluent Gantt Chart

{{ API Type=FluentGanttChart }}

## API Gantt Chart Data Point

{{ API Type=GanttChartDataPoint Properties=All }}

## API Gantt Chart X Range

{{ API Type=GanttChartXRange Properties=All }}
