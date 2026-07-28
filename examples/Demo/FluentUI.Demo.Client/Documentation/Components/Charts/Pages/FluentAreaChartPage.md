---
title: Area Chart
route: /Charts/AreaChart
---

# Area Chart

Area charts are graphical representations of data that display quantitative data points connected by lines and filled with colors to create a visual representation of trends and patterns. The area between the line and the x-axis is colored, which helps in emphasizing the cumulative total or the overall magnitude of the data. They are a slight variation of single line charts, and generally can be used interchangeably.

Stacked area charts are great at communicating how multiple data series relate to the total value. It illustrates how each series compares to the other in their contributions to the total. The baseline is moving in stacked area charts, rather than sharing a common baseline in overlapping areas.

## Layout

Padding on the left and right of the chart is determined by the x-axis labels - it should start and end at or close to the first and last tick mark. The minimum padding is 8px.

Area charts support both stacked (`mode="tonexty"`) and non-stacked (`mode="tozeroy"`) fill modes.

## Content

- Area line An area line represents a set of values from the same data set. Each line takes on a new swatch in the data visualization library to distinguish it from others. 2px wide. There is no rounding of joints to avoid data misrepresentation.
- Area fill Uses the same color family as the area line, but applies a 50% opacity. Note: the implemented stacked area components use transparency fills, but we cannot apply transparency in the Figma guidance

## Accessibility

Users "Enter" into the graph and can use both arrow and tab keys to navigate through.
The first tab stop will stop on the graph and give a description of what type of graph it is.
Each section of the graph is readable via screen readers. The user can navigate through the entire area plot by using Left and Right arrow keys.

## Interaction

The area chart is a highly performant visual. It uses a path-based rendering mechanism to render the area component. On hovering, the nearest x datapoint is identified and the corresponding point is hovered.

## Customizing the chart

Stacked area chart In stacked area chart, two or more data series are stacked vertically. It helps in easy comparison across different dimensions. The callout on hover for stacked chart displays multiple values from the stack. The callout can be customized to show single values or stacked values. Refer to the props onRenderCalloutPerDataPoint and onRenderCalloutPerStack using which custom content for the callout can be defined.
Custom accessibility Area chart provides a bunch of props to enable custom accessibility messages. Use xAxisCalloutAccessibilityData and callOutAccessibilityData to configure x axis and y axis accessibility messages, respectively.

## Axis localization

The chart axes support 2 ways of localization.

1) JavaScript provided inbuilt localization for numeric and date axis. Specify the culture and dateLocalizeOptions for date axis to define target localization. Refer the Javascript localization guide for usage.
2) Custom locale definition: The consumer of the library can specify a custom locale definition as supported by d3 like this. The date axis will use the date range and the multiformat specified in the definition to determine the correct labels to show in the ticks. For example - If the date range is in days, then the axis will show hourly ticks. If the date range spans across months, then the axis will show months in tick labels and so on. Specify the custom locale definition in the timeFormatLocale prop. Refer to the Custom Locale Date Axis example in line chart for sample usage.

## Do's

- Remain consistent with one chart style if there are multiple instances of it on a page rather than using area and line charts interchangeably.

## Dont's

- Prefer line charts to plot trends.
- No more than 9 lines on a chart; fewer are better.
- Do not remove axis titles unless it is clear to the user what is being visualized.

## Examples

### Basic example

The default example renders an area chart with 3 data series and a legend showing each category's data value.

{{ AreaChartDefault }}

### Multiple legend selection

When `AllowMultipleLegendSelection` is enabled, multiple legend items can be selected at the same time.

{{ AreaChartMultipleLegendSelection }}

### Enable gradient

Setting `EnableGradient` applies a gradient fill to the area series.

{{ AreaChartEnableGradient }}

### Negative Y values

This example demonstrates how to handle negative values in the area chart. The area fill will extend below the x-axis for negative values.

{{ AreaChartNegativeYValues }}

### Multiple Series Negative Y values

This example demonstrates how to handle multiple series with negative values in the area chart. The area fill will extend below the x-axis for negative values.

{{ AreaChartMultipleSeriesNegativeYValues }}

### All negative Y values

This example demonstrates how to handle all negative values in the area chart. The area fill will extend below the x-axis for negative values.

{{ AreaChartAllNegativeYValues }}

### Zero Y (Non stacked)

Non-stacked mode: each series fills independently from y=0 (equivalent to React's mode="tozeroy").

{{ AreaChartZeroYValues }}

### Secondary axis

Use `UseSecondaryYScale` on a series and configure the secondary axis visibility and label width.

{{ AreaChartSecondaryAxis }}

### Hide legend

This example removes the legend list below the chart.

{{ AreaChartHideLegends }}

### Culture

This example uses a specific culture to format the axis labels and tooltips.

{{ AreaChartCulture }}

### Rounded corners

Enabling `RoundedCorners` applies a softer visual style to the legend indicators.

{{ AreaChartRoundedCorners }}

### With custom sizing

Use the sliders to adjust the chart width and height at runtime.

{{ AreaChartSizing }}

### Hide tooltip

Setting `HideTooltip` disables the hover callout.

{{ AreaChartHideTooltip }}

### Legend list label

The `LegendListLabel` property sets the accessible heading for the legend list.

{{ AreaChartLegendListLabel }}

### Custom tooltip

Use `CartesianTooltipTemplate` (or `TooltipTemplate`) to replace the default hover callout with custom Blazor markup.

{{ AreaChartCustomTooltip }}

### Axis titles

The `XAxisTitle` and `YAxisTitle` properties label the chart axes.

{{ AreaChartAxisTitles }}

### RTL

This example demonstrates the chart inside a right-to-left container.

{{ AreaChartDefaultRTL }}

## API Fluent Area Chart

{{ API Type=FluentAreaChart }}

## API Area Chart Data

{{ API Type=AreaChartSeries Properties=All }}

## API Area Chart Data Point

{{ API Type=AreaChartDataPoint Properties=All }}
