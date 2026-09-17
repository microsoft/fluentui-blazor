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

    defineOnce('fluentui:chart-components:horizontal-bar-chart', () => {
      FluentUIComponents.HorizontalBarChart.define(FluentUIComponents.HorizontalBarChartDefinition);
    });

    defineOnce('fluentui:chart-components:horizontal-bar-chart-with-axis', () => {
      FluentUIComponents.HorizontalBarChartWithAxis.define(FluentUIComponents.HorizontalBarChartWithAxisDefinition);
    });

    defineOnce('fluentui:chart-components:funnel-chart', () => {
      FluentUIComponents.FunnelChart.define(FluentUIComponents.FunnelChartDefinition);
    });

    defineOnce('fluentui:chart-components:gantt-chart', () => {
      FluentUIComponents.GanttChart.define(FluentUIComponents.GanttChartDefinition);
    });

  }
}
