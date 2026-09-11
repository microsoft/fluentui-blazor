import * as FluentUIComponents from './components';
import { defineOnce } from '@core/RegistrationState';

export namespace Microsoft.FluentUI.Blazor.FluentUIChartComponents {

  export function defineComponents() {
    // Register Chart Web Components
    defineOnce('fluentui:chart-components:area-chart', () => {
      FluentUIComponents.AreaChart.define(FluentUIComponents.AreaChartDefinition);
    });

    defineOnce('fluentui:chart-components:chart-legend', () => {
      FluentUIComponents.ChartLegend.define(FluentUIComponents.ChartLegendDefinition);
    });

    defineOnce('fluentui:chart-components:donut-chart', () => {
      FluentUIComponents.DonutChart.define(FluentUIComponents.DonutChartDefinition);
    });

    defineOnce('fluentui:chart-components:funnel-chart', () => {
      FluentUIComponents.FunnelChart.define(FluentUIComponents.FunnelChartDefinition);
    });

    defineOnce('fluentui:chart-components:gantt-chart', () => {
      FluentUIComponents.GanttChart.define(FluentUIComponents.GanttChartDefinition);
    });

    defineOnce('fluentui:chart-components:gauge-chart', () => {
      FluentUIComponents.GaugeChart.define(FluentUIComponents.GaugeChartDefinition);
    });

    defineOnce('fluentui:chart-components:grouped-vertical-bar-chart', () => {
      FluentUIComponents.GroupedVerticalBarChart.define(FluentUIComponents.GroupedVerticalBarChartDefinition);
    });

    defineOnce('fluentui:chart-components:heat-map-chart', () => {
      FluentUIComponents.HeatMapChart.define(FluentUIComponents.HeatMapChartDefinition);
    });

    defineOnce('fluentui:chart-components:horizontal-bar-chart', () => {
      FluentUIComponents.HorizontalBarChart.define(FluentUIComponents.HorizontalBarChartDefinition);
    });

    defineOnce('fluentui:chart-components:horizontal-bar-chart-with-axis', () => {
      FluentUIComponents.HorizontalBarChartWithAxis.define(FluentUIComponents.HorizontalBarChartWithAxisDefinition);
    });

    defineOnce('fluentui:chart-components:line-chart', () => {
      FluentUIComponents.LineChart.define(FluentUIComponents.LineChartDefinition);
    });

    defineOnce('fluentui:chart-components:polar-chart', () => {
      FluentUIComponents.PolarChart.define(FluentUIComponents.PolarChartDefinition);
    });

    defineOnce('fluentui:chart-components:sankey-chart', () => {
      FluentUIComponents.SankeyChart.define(FluentUIComponents.SankeyChartDefinition);
    });

    defineOnce('fluentui:chart-components:scatter-chart', () => {
      FluentUIComponents.ScatterChart.define(FluentUIComponents.ScatterChartDefinition);
    });

    defineOnce('fluentui:chart-components:sparkline-chart', () => {
      FluentUIComponents.SparklineChart.define(FluentUIComponents.SparklineChartDefinition);
    });

    defineOnce('fluentui:chart-components:vertical-bar-chart', () => {
      FluentUIComponents.VerticalBarChart.define(FluentUIComponents.VerticalBarChartDefinition);
    });

    defineOnce('fluentui:chart-components:vertical-stacked-bar-chart', () => {
      FluentUIComponents.VerticalStackedBarChart.define(FluentUIComponents.VerticalStackedBarChartDefinition);
    });
  }
}
