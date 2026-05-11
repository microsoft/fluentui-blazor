---
title: Horizontal Bar Chart With Axis
route: /Charts/HorizontalBarChartWithAxis
---

# Horizontal Bar Chart With Axis

A horizontal bar chart with axis is a chart that presents categorical data with rectangular bars with lengths proportional to the values they represent, along with an axis to provide context for the values. This type of chart is particularly useful when the intention is to show comparisons among various categories and the labels for those categories are long, while also providing a clear reference point for the values being compared.

### Default

The default example renders a grouped horizontal bar chart with a numeric x-axis and string y-axis labels, showing the baseline appearance of the component.

{{ HorizontalBarChartWithAxisDefault }}

### String Y-axis

Demonstrates using string labels on the y-axis, where each category is identified by a text value rather than a numeric position.

{{ HorizontalBarChartWithAxisStringYAxis }}

### Numeric Y-axis

Demonstrates using numeric values on the y-axis, suitable for scenarios where categories are identified by ordered numeric keys.

{{ HorizontalBarChartWithAxisNumericYAxis }}

### Stacked bars

In the stacked variant, multiple data series are layered into a single bar per category, making it easy to see both part-to-whole relationships and totals.

{{ HorizontalBarChartWithAxisStacked }}

### Negative values

Shows how the chart handles data that contains negative values, with bars extending in both directions from the zero baseline.

{{ HorizontalBarChartWithAxisNegative }}

### Category order

Use the selector to change the ordering of the y-axis categories at runtime, illustrating how the `CategoryOrder` property rearranges bars without changing the underlying data.

{{ HorizontalBarChartWithAxisCategoryOrder }}

### Single color

Setting a single color for all series removes per-series color differentiation, useful when the category labels alone carry the semantic meaning.

{{ HorizontalBarChartWithAxisSingleColor }}

### Hide legends

Setting `HideLegends` removes the legend list below the chart, reducing visual clutter when series are already identifiable from the axis labels.

{{ HorizontalBarChartWithAxisHideLegends }}

### Gradient

Enabling `EnableGradient` fills each bar with a gradient that transitions from a lighter tint at the start to the full series color at the end.

{{ HorizontalBarChartWithAxisGradient }}

### Rounded corners

Enabling `RoundedCorners` applies a small border-radius to each bar, giving a softer appearance while retaining the same data layout.

{{ HorizontalBarChartWithAxisRoundedCorners }}

### Show Y-axis labels

Enabling `ShowYAxisLabels` adds text labels directly on the y-axis ticks, making category names visible without requiring the legend.

{{ HorizontalBarChartWithAxisShowYAxisLabels }}

### Culture

Setting the `Culture` property to a specific locale (here `de-DE`) formats all numeric axis tick values and tooltips according to that culture's conventions.

{{ HorizontalBarChartWithAxisCulture }}

### Legend list label

The `LegendListLabel` property sets an accessible heading for the legend list, useful when the chart is embedded in a larger page that requires descriptive landmark text.

{{ HorizontalBarChartWithAxisLegendListLabel }}

### Hide tooltip

Setting `HideTooltip` disables the hover callout so that mousing over a bar no longer shows a data tooltip.

{{ HorizontalBarChartWithAxisHideTooltip }}

### Multiple legend selection

When `AllowMultipleLegendSelection` is enabled, clicking a legend item highlights only the corresponding bars; multiple items can be selected at the same time.

{{ HorizontalBarChartWithAxisMultipleLegendSelection }}

### Hide labels

Setting `HideLabels` suppresses the numeric value labels rendered at the end of each bar, producing a cleaner look when exact values are shown elsewhere.

{{ HorizontalBarChartWithAxisHideLabels }}

### Bar height

Use the slider to adjust the `BarHeight` property at runtime, controlling the pixel thickness of each individual bar in the chart.

{{ HorizontalBarChartWithAxisBarHeight }}

### Axis tick counts

Use the sliders to change the number of ticks rendered on the x-axis and y-axis independently, allowing fine-grained control over axis density.

{{ HorizontalBarChartWithAxisAxisTickCounts }}

### Y-axis padding

Use the slider to adjust the `YAxisPadding` property, which controls the proportional gap between the y-axis labels and the start of the bars.

{{ HorizontalBarChartWithAxisYAxisPadding }}

### Domain override

Use the sliders to override the minimum and maximum values of both axes, demonstrating how `XMinValue`, `XMaxValue`, `YMinValue`, and `YMaxValue` can constrain or expand the visible data range.

{{ HorizontalBarChartWithAxisDomainOverride }}

### RTL

Demonstrates the right-to-left layout mode, where bars grow from the right edge and axis labels are mirrored to support RTL languages.

{{ HorizontalBarChartWithAxisRTL }}

## API Fluent Horizontal Bar Chart With Axis

{{ API Type=FluentHorizontalBarChartWithAxis }}

## API Horizontal Bar Chart With Axis Data Point

{{ API Type=HorizontalBarChartWithAxisDataPoint Properties=All }}
