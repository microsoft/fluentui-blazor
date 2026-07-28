---
title: Donut Chart
route: /Charts/DonutChart
---

# Donut Chart

Donut charts are used to show proportion, which expresses a partial value in comparison to a total value. These types of charts are best to show percentage of individual parts in comparison to a whole, where the change over time is not important to visualize. They are circular statistical graphics divided into slices to illustrate numerical proportion.

## Layout

- The donut chart’s behavior is simple in application. The data is ordered from largest to smallest in clockwise direction and users can single out individual segments for clarity.
- For high cardinality scenarios where the slices are very small, they can be grouped together to form a bigger slice to improve readability.
- The chart is centered in the available screen space. The default chart diameter is 140px and bar width is 16px. This matches the width of bars in bar charts to achieve balanced scale. The size can be adjusted with responsive chart behavior, where the size of the chart and bar diameter grows proportionally in units of 4px.
- Always try to balance the visual weight of the bars in relationship to the rest of the app.
- Segments are separated by a 2px gap to maximize readability. Segment labels should be always displayed for easier chart comprehension.
- Minimum padding around the chart is 16px. It also applies to the version with labels to accommodate space for labels. There is a 2px space between the chart and the label. The label is centered in relationship to the slice it describes. That can be offset if an overlap happens between 2 labels.

## Content

- The donut chart consists of segments arranged clockwise from large to small. The total circle equates to 100% of the data. The segments can use custom formatting, but all values must add up to 100%. Tiny segments may be grouped and shown visually as 'Others'.
- The label string inside the donut should be concise and contain numerical information with limited or no explanation.

## Accessibility

- Users "Enter" into the graph and can use both arrowing and tabbing to navigate through.
- The first tab stop will stop on the graph and give a description of what type of graph it is.
- Each segment can define its own accessibility label to help the user understand the data better.

## Do's

- For scenarios with lots of categories, consider changing the type of graph to a stacked horizontal bar chart.
- We recommend donut charts over pie charts as they are more readable.

## Don'ts

- Don't overuse donuts charts. They require a lot of space on the page and using more than one next to each other dilutes the intended message.

## Examples

### Basic example

The default example renders a donut chart with color-coded segments, a center label, and a legend showing each category's data value.

{{ DonutChartDefault }}

### With labels as percentages

> [!NOTE] There is no value shown in the center of the Donut Chart because the 'ValueInsideDonut' property is not set.

Setting `ShowLabelsInPercent` displays each segment's label as a percentage of the total rather than as a raw numeric value.

{{ DonutChartShowLabelsInPercent }}

### Hide labels

Setting `HideLabels` suppresses the callout labels around the chart segments, leaving only the legend to identify each slice.

{{ DonutChartHideLabels }}

### Without legends

Setting `HideLegends` removes the legend list below the chart, useful when the segment colors and callout labels provide sufficient identification.

{{ DonutChartHideLegends }}

### Rounded corners

Enabling `RoundedCorners` applies a small border-radius to each segment arc, giving the chart a softer, more modern appearance.

{{ DonutChartRoundedCorners }}

<<<<<<< HEAD
=======
### Value inside donut

The `ValueInsideDonut` property sets the text displayed in the center of the ring; use the text input to change the value and see the chart update in real time.

{{ DonutChartValueInsideDonut }}

>>>>>>> users/vnbaaij/dev-v5/add-areachart
### With custom sizing

Use the sliders to adjust the chart's `Width`, `Height`, and `InnerRadius` at runtime, demonstrating how the donut ring scales with the available space.

{{ DonutChartSizing }}

<<<<<<< HEAD
### Outside labels

Enabling `ShowOutsideLabels` moves each segment's label outside the ring, avoiding overlap for charts with many small segments.

{{ DonutChartOutsideLabels }}

=======
>>>>>>> users/vnbaaij/dev-v5/add-areachart
### Hide tooltip

Setting `HideTooltip` disables the hover callout so that mousing over a segment no longer shows a data tooltip.

{{ DonutChartHideTooltip }}

### Legend list label

The `LegendListLabel` property sets an accessible heading for the legend list, useful when the chart is embedded in a larger page that requires descriptive landmark text.

{{ DonutChartLegendListLabel }}

### Culture

Setting the `Culture` property to a specific locale (here `de-DE`) formats all numeric values in labels and tooltips according to that culture's conventions.

{{ DonutChartCulture }}

### Multiple legend selection

When `AllowMultipleLegendSelection` is enabled, clicking a legend item highlights only the corresponding segment; multiple legend items can be selected at the same time.

{{ DonutChartMultipleLegendSelection }}

<<<<<<< HEAD
### Value inside donut

The `ValueInsideDonut` property sets the text displayed in the center of the ring; use the text input to change the value and see the chart update in real time.

{{ DonutChartValueInsideDonut }}

=======
>>>>>>> users/vnbaaij/dev-v5/add-areachart
### RTL

Demonstrates the right-to-left layout mode, where segment labels and the legend are mirrored for RTL language support.

{{ DonutChartDefaultRTL }}

### Custom tooltip

Use the `TooltipTemplate` parameter to replace the default hover callout with fully custom Blazor markup. The `Context` parameter exposes a `TooltipContext` with `Legend`, `YValue`, and `Color` so the template can render any content you need.

{{ DonutChartCustomTooltip }}

## API Fluent Donut Chart

{{ API Type=FluentDonutChart }}

## API Donut Chart Data

{{ API Type=DonutChartData Properties=All }}

## API Donut Chart Data Point

{{ API Type=DonutChartDataPoint Properties=All }}
