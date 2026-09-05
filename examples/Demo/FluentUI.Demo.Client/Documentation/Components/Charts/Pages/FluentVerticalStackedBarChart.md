---
title: Vertical Stacked Bar Chart
route: /Charts/VerticalStackedBarChart
---

# Vertical Stacked Bar Chart

Vertical stacked bar chart displays multiple series of data as stacked bars, with each bar representing a category. The bars are stacked on top of each other, with the height of each bar representing the value of the category of the series.

Categories and their count are shown on the horizontal axis.

## Layout

Stacked bar charts are ideal for comparing values across two or more categories. They can easily show multiple categories on the same chart.

Refer to Vertical Bar Chart page for common layout guidance.

## Content

Refer to Vertical Bar Chart page for common content guidance.

## Accessibility

Refer to Vertical Bar Chart page for common accessibility guidance.

## Customizing the chart

Here are some commonly used properties to customize the bar chart.

- `BarGapMax` sets the maximum gap between bars in a stack. See the prop for more details.

- `BarCornerRadius` sets the corner radius of the bars.

- `BarMinimumHeight` provides the minimum height of a bar. Bars below this height will be displayed at this height.

- Use `IsCalloutForStack` to configure callout to be at stack level or individual datapoint level.

- Define a custom callout rendered per datapoint using `OnRenderCalloutPerDataPoint` and per stack using `OnRenderCalloutPerStack`

- Use `OnBarClick` handler for callback on click of bars

- The bar labels are shown by default. Set the `HideLabels` prop to hide them.

- Use the `BarWidth` prop to customize the width of each bar in the chart. When set to undefined or 'default', the bar width defaults to 16px, which may decrease to prevent overlap. When set to 'auto', the bar width is calculated from padding values. For a fixed bar width, specify an absolute pixel value like 40.

- Use the `MaxBarWidth` prop to limit the width of bars to a specified number of pixels.

- Use the `XAxisInnerPadding` and `XAxisOuterPadding` props to adjust the padding between bars and the padding before the first bar and after the last bar, respectively. These props accept values between 0 and 1, representing a fraction of the step, which is the interval between the start of a bar and the start of the next bar. These props are particularly relevant when using a string x-axis. By default, the inner padding is set to 2/3, maintaining a 2:1 spacing ratio. This default value is calculated using the formula:

> `innerPadding = spaceBetweenBars / (spaceBetweenBars + barWidth)`

## Do's

Refer to Vertical Bar Chart page for common dos.

## Don'ts

Refer to Vertical Bar Chart page for common don'ts.
