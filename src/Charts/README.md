# Microsoft Fluent UI Chart Components for Blazor

[![NuGet](https://img.shields.io/nuget/v/Microsoft.FluentUI.AspNetCore.Components.Charts?label=NuGet%20Charts)](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components.Charts)

## About

This package provides Blazor chart components that bring the Fluent Design System's charting capabilities to .NET Blazor applications. The components are based on the [Fluent UI V9 React Charts package (`@fluentui/react-charts`)](https://react.fluentui.dev/?path=/docs/charts-introduction--docs) and offer the same look, feel, and behavior as their React counterparts.

> **Note:** Currently, only a subset of the chart types available in `@fluentui/react-charts` are implemented. Additional chart types will be added in future releases.

### Available chart types

| Chart type | Blazor component |
|---|---|
| Donut Chart | `<FluentDonutChart>` |
| Funnel Chart | `<FluentFunnelChart>` |
| Gantt Chart | `<FluentGanttChart>` |
| Horizontal Bar Chart | `<FluentHorizontalBarChart>` |
| Horizontal Bar Chart with Axis | `<FluentHorizontalBarChartWithAxis>` |

### Coming soon

The following chart types from the React Charts package are planned for future releases:

- Area Chart
- Gauge Chart
- Grouped Vertical Bar Chart
- Heat Map Chart
- Line Chart
- Polar Chart
- Sankey Chart
- Scatter Chart
- Sparkline
- Vertical Bar Chart
- Vertical Stacked Bar Chart
- And more...

## Prerequisites

This package is an **extension** to the [Microsoft Fluent UI Blazor library](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components) and **cannot be used as a standalone package**. The main `Microsoft.FluentUI.AspNetCore.Components` package must be installed and configured in your project first.

For instructions on how to set up the Fluent UI Blazor library, see the [Getting Started](https://www.fluentui-blazor.net/GetStarted) documentation.

## Installation

Install the package using the .NET CLI:

```shell
dotnet add package Microsoft.FluentUI.AspNetCore.Components.Charts
```

Or add the package using the NuGet Package Manager in Visual Studio.

> No additional setup required. Once the package is installed, the Core library will automatically detect and register everything it needs. There is no need to manually add services or script references.

## Samples and documentation

Live samples and full documentation for all chart components are available on the demo site:

[https://v5.fluentui-blazor.net](https://v5.fluentui-blazor.net)

## Support

The Microsoft Fluent UI Blazor library is an open source project and is **not** an official part of ASP.NET Core, which means it's **not** officially
supported and isn't committed to ship updates as part of any official .NET updates. It is built and maintained by Microsoft employees (**and** other contributors)
and offers support, like most other open source projects, on a best effort base through the GitHub repository **only**.
