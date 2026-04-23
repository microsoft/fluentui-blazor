import { FluentDesignSystem } from '@fluentui/web-components';
import { definition } from './horizontal-bar-chart-with-axis.definition.js';

definition.define(FluentDesignSystem.registry);

const itemRenderer = () => {
  const chart = document.createElement('fluent-horizontal-bar-chart-with-axis');
  return chart;
};

export default itemRenderer;
export { tests } from '../utils/benchmark-wrapper.js';
