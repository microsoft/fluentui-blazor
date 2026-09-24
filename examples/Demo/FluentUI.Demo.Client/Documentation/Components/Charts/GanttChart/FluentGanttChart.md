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

{{ GanttChartDefault }}

### Grouped

{{ GanttChartGrouped }}

### Numeric Axis

{{ GanttChartNumericAxis }}

### Category Order

{{ GanttChartCategoryOrder }}

### Axis Titles

{{ GanttChartAxisTitles }}

### Tick Format

{{ GanttChartTickFormat }}

### Tick format locale

{{ GanttChartTickFormatLocale }}

### Tick Padding

{{ GanttChartTickPadding }}

### Rotate X-Axis Labels

{{ GanttChartRotateXAxisLabels }}

### Support Negative Data

{{ GanttChartSupportNegativeData }}

### Rounded Ticks

{{ GanttChartRoundedTicks }}

### Tick Values (Numeric Axis)

{{ GanttChartTickValues }}

### Date Tick Values (Date Axis)

{{ GanttChartDateTickValues }}

### Date Tick Format

{{ GanttChartDateTickFormat }}

### Stroke Width

{{ GanttChartStrokeWidth }}

### Show X-Axis Labels Tooltip

{{ GanttChartShowXAxisLabelsTooltip }}

### Date Localize Options

{{ GanttChartDateLocalizeOptions }}

### Use UTC

{{ GanttChartUseUTC }}

### Sizing

{{ GanttChartSizing }}

### Hide Legends

{{ GanttChartHideLegends }}

### Multiple Legend Selection

{{ GanttChartMultipleLegendSelection }}

### Rounded Corners

{{ GanttChartRoundedCorners }}

### Culture

{{ GanttChartCulture }}

### Title Align

{{ GanttChartTitleAlign }}

### Title and Legend Positions

{{ GanttChartTitleAndLegendPositions }}

### Hide Tooltip

{{ GanttChartHideTooltip }}

### Custom Tooltip

{{ GanttChartCustomTooltip }}

### RTL

{{ GanttChartRTL }}

## API Fluent Gantt Chart

{{ API Type=FluentGanttChart }}

## API Gantt Chart Data Point

{{ API Type=GanttChartDataPoint Properties=All }}

## API Gantt Chart X Range

{{ API Type=GanttChartXRange Properties=All }}
