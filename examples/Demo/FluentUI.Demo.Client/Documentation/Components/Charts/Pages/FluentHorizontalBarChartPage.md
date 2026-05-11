---
title: Horizontal Bar Chart
route: /Charts/HorizontalBarChart
---

# Horizontal Bar Chart

A horizontal bar chart is a chart that presents categorical data with rectangular bars with lengths proportional to the values they represent.
This type of chart is particularly useful when the intention is to show comparisons among various categories and the labels for those categories are long.

## Layout

Use a horizontal bar graph to compare between different values that are hierarchically equivalent. The rectangular bar length is proportional to the values they represent. There will always be a maximum data value (color) representing the total length.

Horizontal bar chart can be of 2 types -

- Absolute scale the length of the bar is proportional to the biggest value for the category.
- n/M scale the length of the bar is determined by the total/target value of the specific bar. As a result, 2 adjacent bars can have different data scales and not be comparable. This aspect should be kept in mind while using this chart type. See HorizontalBarChart benchmark example to see the behavior. Each bar has a different scale - 100, 200 and 50 units.

## Content

- Title/Label The label for the bar. It is displayed above the bar and can represent longer texts.
- Bar segment The bar segment represents the current value of the category. For n/M variant there is a placeholder segment to show the left-over values.
- Bar value The value of the bar is represented on the right side. This can be absolute or percentage format. This can also be in fractional form representing current value out of total value. See the chartDataMode property to use it.
- Benchmark The benchmark value is shown as an inverted triangle in the chart.

## Accessibility

- Bar graphs should be flexible to their containers. They will change widths to fit their environment.
- Each section of the bar chart is readable via screen readers. The user can navigate through the entire bar graph by using the tab keys.
- The chart reflows to accommodate zooming in to 400%.

## Customizing the chart

- Bar chart custom data This property allows customizing the right-side data part of the chart. See the usage of barChartCustomData prop in custom callout variant.
- Custom hover callout See onRenderCalloutPerHorizontalBar prop to customize the hover callout. Set the chartDataMode as number, fraction or percentage to specify how numerical values will be shown on the chart.
- Benchmark data Set the data attribute of IChartDataPoint to specify the benchmark value. The benchmark value is shown as an inverted triangle in the chart.
- AbsoluteScale variant The bar labels are shown by default in the absolute-scale variant. Set the hideLabels prop to hide them.

## Do's

- Use horizontal bar chart if the length of labels is longer.
- Numerical units on labels are represented through abbreviations.

## Don'ts

- Avoid having more than 20 bars in the chart.
- The n/M variant should be used only when a value has to be compared against its target value.

## Examples

### Default horizontal bar chart

The default variant renders multiple series, each as a stacked bar composed of several color-coded segments. The chart automatically distributes the data across the available width.

{{ HorizontalBarChartDefault }}

### Single Bar

The single-bar variant displays one bar per series with a single data segment, making it easy to compare a single value across multiple categories at a glance.

{{ HorizontalBarChartSingleBar }}

### Single Bar NM Variant

The n/M variant shows each bar scaled relative to its own target value rather than a shared maximum, allowing per-category comparison against individual goals.

{{ HorizontalBarChartSingleBarNMVariant }}

### Benchmark

The benchmark example adds an inverted-triangle indicator to the bar, visually marking a reference or target value within each series.

{{ HorizontalBarChartBenchmark }}

### Single Data Point

This example shows how the chart renders when each series contains only a single data point, useful for simple value comparisons without stacking.

{{ HorizontalBarChartSingleDataPoint }}

### Hide labels

Setting `HideLabels` removes the numeric value labels shown at the end of each bar, producing a cleaner visual when precise values are not required.

{{ HorizontalBarChartHideLabels }}

### Hide ratio

When `HideRatio` is set, the fractional value shown to the right of the bar (e.g. 1543/15000) is suppressed, leaving only the bar itself.

{{ HorizontalBarChartHideRatio }}

### Hide legends

Setting `HideLegends` removes the legend list below the chart, useful when the bar titles alone provide sufficient context.

{{ HorizontalBarChartHideLegends }}

### Hide tooltip

Setting `HideTooltip` disables the hover tooltip so that hovering over a bar segment no longer shows a callout with the data value.

{{ HorizontalBarChartHideTooltip }}

### Rounded corners

Enabling `RoundedCorners` applies a small border-radius to each bar segment, giving the chart a softer, more modern appearance.

{{ HorizontalBarChartRoundedCorners }}

### Gradient

Enabling `EnableGradient` fills each bar segment with a gradient that transitions from a lighter tint to the segment's full color.

{{ HorizontalBarChartGradient }}

### Fraction mode

Setting `ChartDataMode` to `fraction` displays the value label as a numerator/denominator pair (e.g. 1543/15000) so the viewer can see both the current value and the total at a glance.

{{ HorizontalBarChartChartDataModeFraction }}

### Percentage mode

Setting `ChartDataMode` to `percentage` replaces the raw numeric label with a rounded percentage of the total, giving a proportional view of each bar's value.

{{ HorizontalBarChartChartDataModePercentage }}

### Data mode (interactive)

Use the selector to switch between `default`, `fraction`, and `percentage` display modes at runtime, illustrating how `ChartDataMode` changes the value label without altering the bar lengths.

{{ HorizontalBarChartChartDataModeInteractive }}

### Legend list label

The `LegendListLabel` property sets an accessible heading for the legend list, useful when the chart is embedded in a larger page that requires descriptive landmark text.

{{ HorizontalBarChartLegendListLabel }}

### Culture

Setting the `Culture` property to a specific locale (here `de-DE`) formats all numeric values and separators according to that culture's conventions.

{{ HorizontalBarChartCulture }}

### Multiple legend selection

When `AllowMultipleLegendSelection` is enabled, clicking a legend item highlights only the corresponding bar segments; multiple items can be selected simultaneously.

{{ HorizontalBarChartMultipleLegendSelection }}

### RTL

Demonstrates the right-to-left layout mode, where bars grow from the right edge and all labels are mirrored for RTL language support.

{{ HorizontalBarChartDefaultRTL }}

## API Fluent Horizontal Bar Chart

{{ API Type=FluentHorizontalBarChart }}

## API Horizontal Bar Chart Series

{{ API Type=HorizontalBarChartSeries Properties=All }}

## API Horizontal Bar Chart Data Point

{{ API Type=HorizontalBarChartDataPoint Properties=All }}
