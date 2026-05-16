---
title: Funnel Chart
route: /Charts/FunnelChart
---

# Funnel Chart

A funnel chart is a type of chart that shows the flow of data through a process. It is often used to visualize the conversion rates
of a sales process, where the width of each section represents the number of people at that stage of the process.

## Orientation

The chart can be shown in a vertical or horizontal orientation. The default orientation is horizontal, where the widest section is at the left
and the narrowest section is at the right (reversed in RTL layout). In vertical orientation, the widest section is at the top and the narrowest
section is at the bottom.

## Examples

### Basic example

{{ FunnelChartDefault }}

### Vertical orientation

{{ FunnelChartVertical }}

### Stacked Funnel chart

{{ FunnelChartStacked }}

### RTL

Demonstrates the right-to-left layout mode, where segment labels and the legend are mirrored for RTL language support.

{{ FunnelChartDefaultRTL }}

## API Fluent Funnel Chart

{{ API Type=FluentFunnelChart }}

## API Funnel Chart Data Point

{{ API Type=FunnelDataPoint Properties=All }}

## API Funnel Sub Value

{{ API Type=FunnelSubValue Properties=All }}
