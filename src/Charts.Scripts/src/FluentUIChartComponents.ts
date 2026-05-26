import { FluentDesignSystem } from '@fluentui/web-components';
import * as FluentUIComponents from './components';
import { defineOnce } from '@core/RegistrationState';

export namespace Microsoft.FluentUI.Blazor.FluentUIChartComponents {

  export function defineComponents() {
    const registry = FluentDesignSystem.registry;

    // Register Chart Web Components
    defineOnce('fluentui:chart-components:chart-legend', () => {
      FluentUIComponents.ChartLegendDefinition.define(registry);
    });

    defineOnce('fluentui:chart-components:donut-chart', () => {
      FluentUIComponents.DonutChartDefinition.define(registry);
    });

    defineOnce('fluentui:chart-components:horizontal-bar-chart', () => {
      FluentUIComponents.HorizontalBarChartDefinition.define(registry);
    });

    defineOnce('fluentui:chart-components:horizontal-bar-chart-with-axis', () => {
      FluentUIComponents.HorizontalBarChartWithAxisDefinition.define(registry);
    });

    defineOnce('fluentui:chart-components:funnel-chart', () => {
      FluentUIComponents.FunnelChartDefinition.define(registry);
    });

    defineOnce('fluentui:chart-components:gantt-chart', () => {
      FluentUIComponents.GanttChartDefinition.define(registry);
    });

  }
}
