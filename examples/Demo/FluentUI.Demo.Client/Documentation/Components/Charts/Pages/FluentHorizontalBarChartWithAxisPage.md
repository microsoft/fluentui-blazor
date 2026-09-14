---
title: Horizontal Bar Chart With Axis
route: /Charts/HorizontalBarChartWithAxis
---

# Horizontal Bar Chart With Axis

A horizontal bar chart is a chart that presents categorical data with rectangular bars with lengths proportional to the values they represent.
This type of chart is particularly useful when the intention is to show comparisons among various categories and the labels for those categories are long.

Horizontal bar chart with axis is a version of horizontal bar chart that has the x and y axis present. This chart is same as the vertical bar chart except
that the bars are aligned horizontally.

## Layout

The default bar height is 16px. For dense data, it can be as thin as 8px high. Always consider the visual weight of the bars in relationship to the rest
of the app before choosing this type of chart.

The padding around the bar chart is a default of 8px from the x and y-axis container. This gives enough room for additional content like label values to
display properly without overlapping on to the Y-axis ticks. A 2:1 spacing is maintained between all the bars in the graph so that space between two bars
is always two times the bar height. This helps to ensure that the graph is not overpowering other data visualizations. For charts that display monetary values, the dollar symbol should be displayed as part of the total value. Also call out the currency in the chart title to provide additional context. Chart title can be used to communicate currency when the total labels are hidden.

The chart can accommodate unusually long labels by shrinking the bars without distorting the visual layout.

## Content

- **Bar segment** Bar segments make up a bar chart. Standard size options are: 8px, 16px, and 24px with 16px being the default.
- **Value labels** (Optional) - Off by default with the option to toggle on in case the data visualization needs to communicate label values to users.

## Accessibility

- Bar graphs should be flexible to their containers. They will change width and height to fit their environment.

- Type truncation should happen when the total value exceeds one thousand including 1 decimal place for the hundreds.
- For example, display full value for 600, 983, or 19.53. Truncate 6,000 to 6.0K, 9,801 to 9.8K, and 100,900 to 100.9K.

- All the bars of the graph are accessible by screen readers and keyboard navigation using Up and Down arrow keys or Tab.

## Do's

- Try to keep the number of bars in the chart between 3 and 20 to maximize readability.
- Use this chart if the bar labels are very long.

## Dont's

- Don't keep the bar values in random order. Horizontal bar chart is most effective if the bars are sorted in either ascending or descending order.

## Examples

### Default

{{ HorizontalBarChartWithAxisDefault }}

### Axis titles

{{ HorizontalBarChartWithAxisAxisTitles }}

### Rotate X axis labels

{{ HorizontalBarChartWithAxisRotateXAxisLabels }}

### Wrap X axis labels

{{ HorizontalBarChartWithAxisWrapXAxisLabels }}

### Tick format

{{ HorizontalBarChartWithAxisTickFormat }}

### String Y-Axis

{{ HorizontalBarChartWithAxisStringYAxis }}

### Numeric Y-Axis

{{ HorizontalBarChartWithAxisNumericYAxis }}

### Stacked Bars

{{ HorizontalBarChartWithAxisStacked }}

### Negative Values

{{ HorizontalBarChartWithAxisNegative }}

### Category Order

{{ HorizontalBarChartWithAxisCategoryOrder }}

### Single Color

{{ HorizontalBarChartWithAxisSingleColor }}

### Gradient

{{ HorizontalBarChartWithAxisGradient }}

### Hide Labels

{{ HorizontalBarChartWithAxisHideLabels }}

### Hide Legends

{{ HorizontalBarChartWithAxisHideLegends }}

### Multiple Legend Selection

{{ HorizontalBarChartWithAxisMultipleLegendSelection }}

### Rounded Corners

{{ HorizontalBarChartWithAxisRoundedCorners }}

### Culture

{{ HorizontalBarChartWithAxisCulture }}

### Title Align

{{ HorizontalBarChartWithAxisTitleAlign }}

### Title and Legend Positions

{{ HorizontalBarChartWithAxisTitleAndLegendPositions }}

### Hide Tooltip

{{ HorizontalBarChartWithAxisHideTooltip }}

### Custom Tooltip

{{ HorizontalBarChartWithAxisCustomTooltip }}

### RTL

{{ HorizontalBarChartWithAxisRTL }}

### Show Y-Axis Labels

{{ HorizontalBarChartWithAxisShowYAxisLabels }}

### Legend List Label

{{ HorizontalBarChartWithAxisLegendListLabel }}

### Bar Height

{{ HorizontalBarChartWithAxisBarHeight }}

### Axis Tick Counts

{{ HorizontalBarChartWithAxisAxisTickCounts }}

### Y-Axis Padding

{{ HorizontalBarChartWithAxisYAxisPadding }}

### Domain Override

{{ HorizontalBarChartWithAxisDomainOverride }}

### Tick Values

{{ HorizontalBarChartWithAxisTickValues }}

### Stroke Width

{{ HorizontalBarChartWithAxisStrokeWidth }}

### Show X-Axis Labels Tooltip

{{ HorizontalBarChartWithAxisShowXAxisLabelsTooltip }}

## API Fluent Horizontal Bar Chart With Axis

{{ API Type=FluentHorizontalBarChartWithAxis }}

## API Horizontal Bar Chart With Axis Data Point

{{ API Type=HorizontalBarChartWithAxisDataPoint Properties=All }}
