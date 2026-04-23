import { html } from '@microsoft/fast-element';
import {
  FieldDefinition,
  FluentDesignSystem,
  LabelDefinition,
  SliderDefinition,
  SwitchDefinition,
} from '@fluentui/web-components';
import type { Meta, Story, StoryArgs } from '../helpers.stories.js';
import { renderComponent } from '../helpers.stories.js';
import { DonutChart as FluentDonutChart } from './donut-chart.js';
import type { ChartDataPoint, ChartProps } from './donut-chart.options.js';

type FluentSliderElement = HTMLElement & { value: string };
type FluentSwitchElement = HTMLElement & { checked: boolean };

const ensureDefinition = (tagName: string, define: () => void) => {
  if (!customElements.get(tagName)) {
    define();
  }
};

ensureDefinition('fluent-field', () => FieldDefinition.define(FluentDesignSystem.registry));
ensureDefinition('fluent-label', () => LabelDefinition.define(FluentDesignSystem.registry));
ensureDefinition('fluent-slider', () => SliderDefinition.define(FluentDesignSystem.registry));
ensureDefinition('fluent-switch', () => SwitchDefinition.define(FluentDesignSystem.registry));

const controlsRowStyle = 'display:flex;flex-wrap:wrap;gap:16px 24px;align-items:end;';
const sliderFieldStyle = 'min-width:220px;flex:1 1 220px;';
const toggleFieldStyle = 'min-width:220px;';

const createSliderField = (
  labelText: string,
  id: string,
  value: number,
  min: number,
  max: number,
  onChange: (nextValue: number) => void,
) => {
  const field = document.createElement('fluent-field');
  field.setAttribute('label-position', 'above');
  field.setAttribute('style', sliderFieldStyle);

  const label = document.createElement('label');
  label.slot = 'label';
  label.htmlFor = id;
  label.textContent = labelText;
  field.appendChild(label);

  const slider = document.createElement('fluent-slider') as FluentSliderElement;
  slider.slot = 'input';
  slider.id = id;
  slider.setAttribute('min', `${min}`);
  slider.setAttribute('max', `${max}`);
  slider.value = `${value}`;
  slider.setAttribute('value', `${value}`);
  field.appendChild(slider);

  const message = document.createElement('fluent-label');
  message.slot = 'message';
  message.textContent = `${value}`;
  field.appendChild(message);

  slider.addEventListener('change', () => onChange(Number(slider.value)));

  return {
    element: field,
    setValue: (nextValue: number) => {
      slider.value = `${nextValue}`;
      slider.setAttribute('value', `${nextValue}`);
      message.textContent = `${nextValue}`;
    },
  };
};

const createSwitchField = (
  labelText: string,
  id: string,
  checked: boolean,
  onChange: (nextChecked: boolean) => void,
) => {
  const field = document.createElement('fluent-field');
  field.setAttribute('label-position', 'after');
  field.setAttribute('style', toggleFieldStyle);

  const label = document.createElement('label');
  label.slot = 'label';
  label.htmlFor = id;
  label.textContent = labelText;
  field.appendChild(label);

  const control = document.createElement('fluent-switch') as FluentSwitchElement;
  control.slot = 'input';
  control.id = id;
  control.checked = checked;
  control.toggleAttribute('checked', checked);
  control.addEventListener('change', () => onChange(control.checked));
  field.appendChild(control);

  return {
    element: field,
    setValue: (nextChecked: boolean) => {
      control.checked = nextChecked;
      control.toggleAttribute('checked', nextChecked);
    },
  };
};

const basicTitle = 'Donut chart basic example';
const sortedTitle = 'Sorted donut chart example';

const points: ChartDataPoint[] = [
  {
    legend: 'first',
    data: 20000,
  },
  {
    legend: 'second',
    data: 39000,
  },
];

const data: ChartProps = {
  chartData: points,
};

const sortedPoints: ChartDataPoint[] = [
  {
    legend: 'small',
    data: 5000,
  },
  {
    legend: 'large',
    data: 39000,
  },
  {
    legend: 'medium',
    data: 15000,
  },
];

const sortedData: ChartProps = {
  chartData: sortedPoints,
};

const storyTemplate = html<StoryArgs<FluentDonutChart>>`
  <fluent-donut-chart
    chart-title="${basicTitle}"
    data="${JSON.stringify(data)}"
    value-inside-donut="39,000"
    inner-radius="55"
  >
  </fluent-donut-chart>
`;

export default {
  title: 'Components/DonutChart',
} as Meta<FluentDonutChart>;

export const Basic: Story<FluentDonutChart> = renderComponent(storyTemplate).bind({});

export const OutsideLabels: Story<FluentDonutChart> = () => {
  const chart = document.createElement('fluent-donut-chart') as FluentDonutChart;
  chart.setAttribute('chart-title', 'Donut chart outside labels example');
  chart.setAttribute('data', JSON.stringify(data));
  chart.setAttribute('value-inside-donut', '39,000');
  chart.setAttribute('inner-radius', '85');
  chart.setAttribute('width', '320');
  chart.setAttribute('height', '320');
  chart.setAttribute('style', 'width:320px;height:320px');
  chart.hideLabels = false;

  return chart;
};

export const Sizing: Story<FluentDonutChart> = () => {
  const container = document.createElement('div');
  const controls = document.createElement('div');
  controls.setAttribute('style', controlsRowStyle);
  container.appendChild(controls);
  const chartHost = document.createElement('div');
  chartHost.setAttribute('style', 'margin-top:20px;');
  container.appendChild(chartHost);

  let width = 320;
  let height = 320;
  let innerRadius = 55;

  const renderChart = () => {
    const chart = document.createElement('fluent-donut-chart') as FluentDonutChart;
    chart.setAttribute('chart-title', 'Donut chart sizing example');
    chart.setAttribute('data', JSON.stringify(data));
    chart.setAttribute('value-inside-donut', '39,000');
    chart.setAttribute('inner-radius', `${innerRadius}`);
    chart.width = width;
    chart.height = height;
    chart.setAttribute('width', `${width}`);
    chart.setAttribute('height', `${height}`);
    chart.setAttribute('style', `width:${width}px;height:${height}px`);

    chartHost.replaceChildren(chart);
  };

  const widthControl = createSliderField('Width', 'donut-width', width, 200, 640, nextWidth => {
    width = nextWidth;
    widthControl.setValue(nextWidth);
    renderChart();
  });
  controls.appendChild(widthControl.element);

  const heightControl = createSliderField('Height', 'donut-height', height, 200, 640, nextHeight => {
    height = nextHeight;
    heightControl.setValue(nextHeight);
    renderChart();
  });
  controls.appendChild(heightControl.element);

  const innerRadiusControl = createSliderField(
    'Inner radius',
    'donut-inner-radius',
    innerRadius,
    1,
    120,
    nextRadius => {
      innerRadius = nextRadius;
      innerRadiusControl.setValue(nextRadius);
      renderChart();
    },
  );
  controls.appendChild(innerRadiusControl.element);

  renderChart();

  return container;
};

export const RoundedCorners: Story<FluentDonutChart> = () => {
  const container = document.createElement('div');
  const controls = document.createElement('div');
  controls.setAttribute('style', controlsRowStyle);
  container.appendChild(controls);

  let roundCorners = false;
  let hideLabels = false;

  const chart = document.createElement('fluent-donut-chart') as FluentDonutChart;
  chart.setAttribute('chart-title', 'Donut chart rounded corners example');
  chart.setAttribute('data', JSON.stringify(data));
  chart.setAttribute('value-inside-donut', '39,000');
  chart.setAttribute('inner-radius', '55');
  chart.setAttribute('style', 'width:320px;height:320px;margin-top:20px;');

  const renderChart = () => {
    chart.hideLabels = hideLabels;
    chart.roundCorners = roundCorners;
    chart.toggleAttribute('hide-labels', hideLabels);
    chart.toggleAttribute('round-corners', roundCorners);

    if (!chart.isConnected) {
      container.appendChild(chart);
    }
  };

  const roundedCornersControl = createSwitchField(
    'Rounded corners',
    'donut-rounded-corners',
    roundCorners,
    nextChecked => {
      roundCorners = nextChecked;
      roundedCornersControl.setValue(nextChecked);
      renderChart();
    },
  );
  controls.appendChild(roundedCornersControl.element);

  const hideLabelsControl = createSwitchField('Hide labels', 'donut-rounded-hide-labels', hideLabels, nextChecked => {
    hideLabels = nextChecked;
    hideLabelsControl.setValue(nextChecked);
    renderChart();
  });
  controls.appendChild(hideLabelsControl.element);

  renderChart();

  return container;
};

export const HideLegends: Story<FluentDonutChart> = renderComponent(html<StoryArgs<FluentDonutChart>>`
  <fluent-donut-chart
    chart-title="Donut chart hide legends example"
    data="${JSON.stringify(data)}"
    value-inside-donut="39,000"
    inner-radius="55"
    hide-legends
  >
  </fluent-donut-chart>
`);

export const ShowLabelsInPercent: Story<FluentDonutChart> = () => {
  const chart = document.createElement('fluent-donut-chart') as FluentDonutChart;
  chart.setAttribute('chart-title', 'Donut chart percent labels example');
  chart.setAttribute('data', JSON.stringify(data));
  chart.setAttribute('inner-radius', '55');
  chart.toggleAttribute('show-labels-in-percent', true);
  chart.hideLabels = false;

  return chart;
};

export const RTL: Story<FluentDonutChart> = renderComponent(html<StoryArgs<FluentDonutChart>>`
  <div dir="rtl">
    <fluent-donut-chart
      chart-title="Donut chart RTL example"
      data="${JSON.stringify(data)}"
      value-inside-donut="39,000"
      inner-radius="55"
    >
    </fluent-donut-chart>
  </div>
`);
