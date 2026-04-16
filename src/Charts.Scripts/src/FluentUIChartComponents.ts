import * as FluentUIWebComponents from '@fluentui/web-components';
import * as FluentUIComponents from '@fluentui/chart-web-components';
import { defineOnce } from '@core/RegistrationState';



export namespace Microsoft.FluentUI.Blazor.FluentUIChartComponents {

  export function defineComponents() {
    const registry = FluentUIWebComponents.FluentDesignSystem.registry;

    // Register Chart Web Components
    defineOnce('fluentui:chart-components:donut-chart', () => {
      FluentUIComponents.DonutChartDefinition.define(registry);
    });

    defineOnce('fluentui:chart-components:horizontal-bar-chart', () => {
      FluentUIComponents.HorizontalBarChartDefinition.define(registry);
    });

    defineOnce('fluentui:chart-components:horizontal-bar-chart-with-axis', () => {
      FluentUIComponents.HorizontalBarChartWithAxisDefinition.define(registry);
    });
  }
}
