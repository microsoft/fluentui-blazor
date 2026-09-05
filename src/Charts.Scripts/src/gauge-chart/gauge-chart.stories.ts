import { html } from '@microsoft/fast-element';
import type { Meta, Story, StoryArgs } from '../helpers.stories.js';
import {
  controlsRowStyle,
  createDropdownField,
  createSliderField,
  createSwitchField,
  createTextInputField,
  renderComponent,
} from '../helpers.stories.js';
import { GaugeChart as FluentGaugeChart } from './gauge-chart.js';
import type { GaugeChartSegment } from './gauge-chart.options.js';

// ── Sample data ───────────────────────────────────────────────────────────────

const multiSegments: GaugeChartSegment[] = [
  { legend: 'Low', size: 33, color: 'qualitative.1' },
  { legend: 'Medium', size: 34, color: 'qualitative.3' },
  { legend: 'High', size: 33, color: 'qualitative.2' },
];

const singleSegment: GaugeChartSegment[] = [
  { legend: 'Used', size: 55 },
  { legend: 'Available', size: 45, color: 'qualitative.5' },
];

const basicTitle = 'Gauge chart basic example';

const storyTemplate = html<StoryArgs<FluentGaugeChart>>`
  <fluent-gauge-chart
    chart-title="${basicTitle}"
    segments="${JSON.stringify(multiSegments)}"
    chart-value="50"
    max-value="100"
  >
  </fluent-gauge-chart>
`;

export default {
  title: 'Components/GaugeChart',
} as Meta<FluentGaugeChart>;

export const Basic: Story<FluentGaugeChart> = renderComponent(storyTemplate).bind({});
Basic.parameters = { docs: { story: { height: '320px' } } };

export const StandardAttributes: Story<FluentGaugeChart> = () => {
  const container = document.createElement('div');

  let width = 252;
  let height = 173;

  const sliderControls = document.createElement('div');
  sliderControls.setAttribute('style', controlsRowStyle);
  container.appendChild(sliderControls);

  const toggleControls = document.createElement('div');
  toggleControls.setAttribute('style', `margin-top:16px;${controlsRowStyle}`);
  container.appendChild(toggleControls);

  const chart = document.createElement('fluent-gauge-chart') as FluentGaugeChart;
  chart.setAttribute('chart-title', basicTitle);
  chart.setAttribute('segments', JSON.stringify(multiSegments));
  chart.setAttribute('width', `${width}`);
  chart.setAttribute('height', `${height}`);
  chart.setAttribute('style', 'margin-top:20px;');

  const widthControl = createSliderField('Width', 'gauge-sa-width', width, 100, 600, nextValue => {
    width = nextValue;
    widthControl.setValue(nextValue);
    chart.setAttribute('width', `${nextValue}`);
  });
  sliderControls.appendChild(widthControl.element);

  const heightControl = createSliderField('Height', 'gauge-sa-height', height, 100, 400, nextValue => {
    height = nextValue;
    heightControl.setValue(nextValue);
    chart.setAttribute('height', `${nextValue}`);
  });
  sliderControls.appendChild(heightControl.element);

  toggleControls.appendChild(
    createSwitchField('Hide Legends', 'gauge-sa-hide-legends', false, checked => {
      chart.toggleAttribute('hide-legends', checked);
    }).element,
  );

  toggleControls.appendChild(
    createSwitchField('Hide Labels', 'gauge-sa-hide-labels', false, checked => {
      chart.toggleAttribute('hide-labels', checked);
    }).element,
  );

  toggleControls.appendChild(
    createSwitchField('Hide Tooltip', 'gauge-sa-hide-tooltip', false, checked => {
      chart.toggleAttribute('hide-tooltip', checked);
    }).element,
  );

  toggleControls.appendChild(
    createSwitchField('Rounded Corners', 'gauge-sa-round-corners', false, checked => {
      chart.toggleAttribute('round-corners', checked);
    }).element,
  );

  toggleControls.appendChild(
    createSwitchField('Multiple Legend Selection', 'gauge-sa-multi-select', false, checked => {
      chart.toggleAttribute('allow-multiple-legend-selection', checked);
    }).element,
  );

  container.appendChild(chart);
  return container;
};
StandardAttributes.storyName = 'Standard Attributes';
StandardAttributes.parameters = { docs: { story: { height: '420px' } } };

export const ChartAttributes: Story<FluentGaugeChart> = () => {
  const container = document.createElement('div');

  const sliderControls = document.createElement('div');
  sliderControls.setAttribute('style', controlsRowStyle);
  container.appendChild(sliderControls);

  const toggleControls = document.createElement('div');
  toggleControls.setAttribute('style', `margin-top:16px;${controlsRowStyle}`);
  container.appendChild(toggleControls);

  const chart = document.createElement('fluent-gauge-chart') as FluentGaugeChart;
  chart.setAttribute('chart-title', 'Gauge chart chart attributes example');
  chart.setAttribute('segments', JSON.stringify(multiSegments));
  chart.setAttribute('chart-value', '50');
  chart.setAttribute('max-value', '100');
  chart.setAttribute('width', '252');
  chart.setAttribute('height', '173');
  chart.setAttribute('style', 'margin-top:20px;');

  const valueControl = createSliderField('Chart value', 'gauge-ca-value', 50, 0, 100, nextValue => {
    valueControl.setValue(nextValue);
    chart.setAttribute('chart-value', `${nextValue}`);
  });
  sliderControls.appendChild(valueControl.element);

  const maxValueControl = createSliderField('Max value', 'gauge-ca-max-value', 100, 1, 200, nextValue => {
    maxValueControl.setValue(nextValue);
    chart.setAttribute('max-value', `${nextValue}`);
  });
  sliderControls.appendChild(maxValueControl.element);

  const valueFormatControl = createDropdownField(
    'Chart value format',
    'gauge-ca-value-format',
    ['default', 'fraction'],
    'default',
    nextValue => {
      if (nextValue === 'default') {
        chart.removeAttribute('chart-value-format');
      } else {
        chart.setAttribute('chart-value-format', nextValue);
      }
    },
  );
  sliderControls.appendChild(valueFormatControl.element);

  const sublabelInput = createTextInputField('Sublabel', 'gauge-ca-sublabel', '', nextValue => {
    if (nextValue) {
      chart.setAttribute('sublabel', nextValue);
    } else {
      chart.removeAttribute('sublabel');
    }
  });
  sliderControls.appendChild(sublabelInput.element);

  const variantControl = createDropdownField(
    'Variant',
    'gauge-ca-variant',
    ['multiple-segments', 'single-segment'],
    'multiple-segments',
    nextValue => {
      chart.setAttribute('variant', nextValue);
    },
  );
  toggleControls.appendChild(variantControl.element);

  toggleControls.appendChild(
    createSwitchField('Hide Min/Max', 'gauge-ca-hide-min-max', false, checked => {
      chart.toggleAttribute('hide-min-max', checked);
    }).element,
  );

  container.appendChild(chart);
  return container;
};
ChartAttributes.storyName = 'Chart Attributes';
ChartAttributes.parameters = { docs: { story: { height: '520px' } } };

export const SingleSegment: Story<FluentGaugeChart> = renderComponent(html<StoryArgs<FluentGaugeChart>>`
  <fluent-gauge-chart
    chart-title="Storage capacity"
    segments="${JSON.stringify(singleSegment)}"
    chart-value="55"
    sublabel="used"
    chart-value-format="fraction"
    variant="single-segment"
  >
  </fluent-gauge-chart>
`);

SingleSegment.parameters = { docs: { story: { height: '320px' } } };
export const FractionFormat: Story<FluentGaugeChart> = renderComponent(html<StoryArgs<FluentGaugeChart>>`
  <fluent-gauge-chart
    chart-title="Gauge chart fraction format example"
    segments="${JSON.stringify(multiSegments)}"
    chart-value="50"
    max-value="100"
    chart-value-format="fraction"
  >
  </fluent-gauge-chart>
`);

FractionFormat.parameters = { docs: { story: { height: '320px' } } };
export const HideMinMax: Story<FluentGaugeChart> = renderComponent(html<StoryArgs<FluentGaugeChart>>`
  <fluent-gauge-chart
    chart-title="Gauge chart hidden min/max example"
    segments="${JSON.stringify(multiSegments)}"
    chart-value="50"
    max-value="100"
    hide-min-max
  >
  </fluent-gauge-chart>
`);

HideMinMax.parameters = { docs: { story: { height: '320px' } } };
export const WithSublabel: Story<FluentGaugeChart> = renderComponent(html<StoryArgs<FluentGaugeChart>>`
  <fluent-gauge-chart
    chart-title="Gauge chart sublabel example"
    segments="${JSON.stringify(multiSegments)}"
    height="116"
    chart-value="50"
    max-value="100"
    sublabel="out of 100"
  >
  </fluent-gauge-chart>
`);

WithSublabel.parameters = { docs: { story: { height: '320px' } } };
export const RoundedCorners: Story<FluentGaugeChart> = renderComponent(html<StoryArgs<FluentGaugeChart>>`
  <fluent-gauge-chart
    chart-title="Gauge chart rounded corners example"
    segments="${JSON.stringify(multiSegments)}"
    chart-value="50"
    max-value="100"
    round-corners
  >
  </fluent-gauge-chart>
`);

RoundedCorners.parameters = { docs: { story: { height: '320px' } } };
export const HideLegends: Story<FluentGaugeChart> = renderComponent(html<StoryArgs<FluentGaugeChart>>`
  <fluent-gauge-chart
    chart-title="Gauge chart hidden legend example"
    segments="${JSON.stringify(multiSegments)}"
    chart-value="50"
    max-value="100"
    hide-legends
  >
  </fluent-gauge-chart>
`);

HideLegends.parameters = { docs: { story: { height: '320px' } } };
export const Sizing: Story<FluentGaugeChart> = () => {
  const container = document.createElement('div');
  const controls = document.createElement('div');
  controls.setAttribute('style', controlsRowStyle);
  container.appendChild(controls);
  const chartHost = document.createElement('div');
  chartHost.setAttribute('style', 'margin-top:20px;');
  container.appendChild(chartHost);

  let width = 252;
  let height = 96;

  const renderChart = () => {
    const chart = document.createElement('fluent-gauge-chart') as FluentGaugeChart;
    chart.setAttribute('chart-title', 'Gauge chart sizing example');
    chart.setAttribute('segments', JSON.stringify(multiSegments));
    chart.setAttribute('chart-value', '50');
    chart.setAttribute('max-value', '100');
    chart.setAttribute('width', `${width}`);
    chart.setAttribute('height', `${height}`);
    chart.setAttribute('style', `width:${width}px;height:${height}px`);
    chartHost.replaceChildren(chart);
  };

  const widthControl = createSliderField('Width', 'gauge-width', width, 200, 640, nextWidth => {
    width = nextWidth;
    widthControl.setValue(nextWidth);
    renderChart();
  });
  controls.appendChild(widthControl.element);

  const heightControl = createSliderField('Height', 'gauge-height', height, 70, 400, nextHeight => {
    height = nextHeight;
    heightControl.setValue(nextHeight);
    renderChart();
  });
  controls.appendChild(heightControl.element);

  renderChart();
  return container;
};
Sizing.parameters = { docs: { story: { height: '460px' } } };

export const LiveValue: Story<FluentGaugeChart> = () => {
  const container = document.createElement('div');
  const controls = document.createElement('div');
  controls.setAttribute('style', controlsRowStyle);
  container.appendChild(controls);
  const chartHost = document.createElement('div');
  chartHost.setAttribute('style', 'margin-top:20px;');
  container.appendChild(chartHost);

  const chart = document.createElement('fluent-gauge-chart') as FluentGaugeChart;
  chart.setAttribute('chart-title', 'Gauge chart live value example');
  chart.setAttribute('segments', JSON.stringify(multiSegments));
  chart.setAttribute('chart-value', '50');
  chart.setAttribute('max-value', '100');
  chartHost.appendChild(chart);

  const valueControl = createSliderField('Chart value', 'gauge-value', 50, 0, 100, nextValue => {
    chart.setAttribute('chart-value', `${nextValue}`);
    valueControl.setValue(nextValue);
  });
  controls.appendChild(valueControl.element);

  return container;
};
LiveValue.parameters = { docs: { story: { height: '460px' } } };

export const CustomMinMax: Story<FluentGaugeChart> = () => {
  const container = document.createElement('div');
  const controls = document.createElement('div');
  controls.setAttribute('style', controlsRowStyle);
  container.appendChild(controls);
  const chartHost = document.createElement('div');
  chartHost.setAttribute('style', 'margin-top:20px;');
  container.appendChild(chartHost);

  const customSegments: GaugeChartSegment[] = [
    { legend: 'Cold', size: 20 },
    { legend: 'Warm', size: 20 },
    { legend: 'Hot', size: 20 },
  ];

  const chart = document.createElement('fluent-gauge-chart') as FluentGaugeChart;
  chart.setAttribute('chart-title', 'Gauge chart custom range example');
  chart.setAttribute('segments', JSON.stringify(customSegments));
  chart.setAttribute('chart-value', '40');
  chart.setAttribute('min-value', '10');
  chart.setAttribute('max-value', '70');
  chartHost.appendChild(chart);

  const valueControl = createSliderField('Chart value', 'gauge-custom-value', 40, 10, 70, nextValue => {
    chart.setAttribute('chart-value', `${nextValue}`);
    valueControl.setValue(nextValue);
  });
  controls.appendChild(valueControl.element);

  return container;
};
CustomMinMax.parameters = { docs: { story: { height: '460px' } } };

export const HideTooltip: Story<FluentGaugeChart> = () => {
  const container = document.createElement('div');
  const controls = document.createElement('div');
  controls.setAttribute('style', controlsRowStyle);
  container.appendChild(controls);

  let hideTooltip = true;

  const chart = document.createElement('fluent-gauge-chart') as FluentGaugeChart;
  chart.setAttribute('chart-title', 'Gauge chart hide tooltip example');
  chart.setAttribute('segments', JSON.stringify(multiSegments));
  chart.setAttribute('chart-value', '50');
  chart.setAttribute('max-value', '100');
  chart.setAttribute('style', 'margin-top:20px;');
  chart.toggleAttribute('hide-tooltip', hideTooltip);
  container.appendChild(chart);

  const hideTooltipControl = createSwitchField('Hide tooltip', 'gauge-hide-tooltip', hideTooltip, nextChecked => {
    hideTooltip = nextChecked;
    hideTooltipControl.setValue(nextChecked);
    chart.hideTooltip = nextChecked;
    chart.toggleAttribute('hide-tooltip', nextChecked);
  });
  controls.appendChild(hideTooltipControl.element);

  return container;
};
HideTooltip.parameters = { docs: { story: { height: '440px' } } };

export const WithSublabelAndTitle: Story<FluentGaugeChart> = renderComponent(html<StoryArgs<FluentGaugeChart>>`
  <fluent-gauge-chart
    chart-title="Gauge chart full example"
    segments="${JSON.stringify(multiSegments)}"
    chart-value="50"
    max-value="100"
    sublabel="out of 100"
    chart-value-format="fraction"
  >
  </fluent-gauge-chart>
`);

WithSublabelAndTitle.parameters = { docs: { story: { height: '360px' } } };
export const MultipleLegendSelection: Story<FluentGaugeChart> = () => {
  const container = document.createElement('div');
  const controls = document.createElement('div');
  controls.setAttribute('style', controlsRowStyle);
  container.appendChild(controls);

  let allowMultiple = true;

  const chart = document.createElement('fluent-gauge-chart') as FluentGaugeChart;
  chart.setAttribute('chart-title', 'Gauge chart multiple legend selection example');
  chart.setAttribute('segments', JSON.stringify(multiSegments));
  chart.setAttribute('chart-value', '50');
  chart.setAttribute('max-value', '100');
  chart.setAttribute('style', 'margin-top:20px;');
  chart.allowMultipleLegendSelection = allowMultiple;
  chart.toggleAttribute('allow-multiple-legend-selection', allowMultiple);
  container.appendChild(chart);

  const multipleControl = createSwitchField(
    'Allow multiple legend selection',
    'gauge-multiple-legend',
    allowMultiple,
    nextChecked => {
      allowMultiple = nextChecked;
      multipleControl.setValue(nextChecked);
      chart.allowMultipleLegendSelection = nextChecked;
      chart.toggleAttribute('allow-multiple-legend-selection', nextChecked);
    },
  );
  controls.appendChild(multipleControl.element);

  return container;
};
MultipleLegendSelection.parameters = { docs: { story: { height: '440px' } } };

export const Sublabel: Story<FluentGaugeChart> = () => {
  const container = document.createElement('div');
  const controls = document.createElement('div');
  controls.setAttribute('style', controlsRowStyle);
  container.appendChild(controls);
  const chartHost = document.createElement('div');
  chartHost.setAttribute('style', 'margin-top:20px;');
  container.appendChild(chartHost);

  const chart = document.createElement('fluent-gauge-chart') as FluentGaugeChart;
  chart.setAttribute('chart-title', 'Gauge chart sublabel example');
  chart.setAttribute('segments', JSON.stringify(multiSegments));
  chart.setAttribute('chart-value', '50');
  chart.setAttribute('max-value', '100');
  chart.setAttribute('sublabel', 'out of 100');
  chartHost.appendChild(chart);

  const sublabelInput = createTextInputField('Sublabel', 'gauge-sublabel', 'out of 100', nextValue => {
    chart.sublabel = nextValue;
    if (nextValue) {
      chart.setAttribute('sublabel', nextValue);
    } else {
      chart.removeAttribute('sublabel');
    }
  });
  controls.appendChild(sublabelInput.element);

  return container;
};
Sublabel.parameters = { docs: { story: { height: '460px' } } };

export const ResponsiveWidth: Story<FluentGaugeChart> = () => {
  const wrapper = document.createElement('div');
  wrapper.setAttribute(
    'style',
    'resize:horizontal;overflow:hidden;width:480px;min-width:280px;max-width:800px;border:1px solid #ccc;padding:8px;',
  );

  const chart = document.createElement('fluent-gauge-chart') as FluentGaugeChart;
  chart.setAttribute('chart-title', 'Gauge chart responsive example');
  chart.setAttribute('segments', JSON.stringify(multiSegments));
  chart.setAttribute('chart-value', '50');
  chart.setAttribute('max-value', '100');
  chart.setAttribute('width', '100%');
  chart.setAttribute('height', '104');
  wrapper.appendChild(chart);

  return wrapper;
};
ResponsiveWidth.parameters = { docs: { story: { height: '320px' } } };

const segmentsWithAriaLabels: GaugeChartSegment[] = [
  { legend: 'Low', size: 33, color: 'qualitative.1', ariaLabel: 'Custom label for Low' },
  { legend: 'Medium', size: 34, color: 'qualitative.3', ariaLabel: 'Custom label for Medium' },
  { legend: 'High', size: 33, color: 'qualitative.2', ariaLabel: 'Custom label for High' },
];

export const FormatTemplate: Story<FluentGaugeChart> = renderComponent(html<StoryArgs<FluentGaugeChart>>`
  <fluent-gauge-chart
    chart-title="Gauge chart custom value template example"
    segments="${JSON.stringify(multiSegments)}"
    chart-value="45"
    max-value="100"
    chart-value-format-template="{value} of {max} GB"
  >
  </fluent-gauge-chart>
`);

FormatTemplate.parameters = { docs: { story: { height: '320px' } } };
export const SegmentAriaLabels: Story<FluentGaugeChart> = renderComponent(html<StoryArgs<FluentGaugeChart>>`
  <fluent-gauge-chart
    chart-title="Gauge chart segment aria-label example"
    segments="${JSON.stringify(segmentsWithAriaLabels)}"
    chart-value="45"
    max-value="100"
  >
  </fluent-gauge-chart>
`);

SegmentAriaLabels.parameters = { docs: { story: { height: '320px' } } };

export const TooltipRendererStory: Story<FluentGaugeChart> = () => {
  const container = document.createElement('div');

  const info = document.createElement('p');
  info.textContent =
    'Hover over a gauge segment — the tooltip body is replaced by a custom renderer that wraps the default HTML in a styled box.';
  container.appendChild(info);

  const chart = document.createElement('fluent-gauge-chart') as FluentGaugeChart;
  chart.setAttribute('chart-title', 'Gauge chart custom tooltipRenderer');
  chart.setAttribute('segments', JSON.stringify(multiSegments));
  chart.setAttribute('chart-value', '50');
  chart.setAttribute('max-value', '100');
  chart.setAttribute('width', '252');
  chart.setAttribute('height', '173');
  chart.tooltipRenderer = (_point, defaultRender) => {
    const wrapper = document.createElement('div');
    wrapper.style.cssText = 'padding:8px;border-left:3px solid #637cef;background:#f3f6ff;';
    wrapper.innerHTML = defaultRender(_point);
    return wrapper;
  };

  container.appendChild(chart);
  return container;
};
TooltipRendererStory.storyName = 'Tooltip Renderer';
TooltipRendererStory.parameters = { docs: { story: { height: '320px' } } };

export const Culture: Story<FluentGaugeChart> = () => {
  const container = document.createElement('div');
  const controls = document.createElement('div');
  controls.setAttribute('style', controlsRowStyle);
  container.appendChild(controls);

  const cultures = ['en-US', 'de-DE', 'fr-FR', 'nl-NL', 'ja-JP', 'ar-SA'] as const;
  let currentCulture: string = 'en-US';

  const chart = document.createElement('fluent-gauge-chart') as FluentGaugeChart;
  chart.setAttribute('chart-title', `Gauge chart culture example (${currentCulture})`);
  chart.setAttribute('segments', JSON.stringify(multiSegments));
  chart.setAttribute('chart-value', '50');
  chart.setAttribute('max-value', '100');
  chart.setAttribute('culture', currentCulture);
  chart.setAttribute('style', 'margin-top:20px;');
  container.appendChild(chart);

  const cultureControl = createDropdownField('Culture', 'gauge-culture', [...cultures], currentCulture, nextCulture => {
    currentCulture = nextCulture;
    chart.setAttribute('culture', currentCulture);
    chart.setAttribute('chart-title', `Gauge chart culture example (${currentCulture})`);
  });
  controls.appendChild(cultureControl.element);

  return container;
};
Culture.parameters = { docs: { story: { height: '400px' } } };

export const TitleAlign: Story<FluentGaugeChart> = () => {
  const container = document.createElement('div');
  const controls = document.createElement('div');
  controls.setAttribute('style', controlsRowStyle);
  container.appendChild(controls);

  const alignments = ['start', 'center', 'end'] as const;
  let currentAlign: (typeof alignments)[number] = 'start';

  const chart = document.createElement('fluent-gauge-chart') as FluentGaugeChart;
  chart.setAttribute('chart-title', 'Gauge chart title alignment example');
  chart.setAttribute('segments', JSON.stringify(multiSegments));
  chart.setAttribute('chart-value', '50');
  chart.setAttribute('max-value', '100');
  chart.setAttribute('title-align', currentAlign);
  chart.setAttribute('style', 'margin-top:20px;');
  container.appendChild(chart);

  const alignControl = createDropdownField(
    'Title align',
    'gauge-title-align',
    [...alignments],
    currentAlign,
    nextAlign => {
      currentAlign = nextAlign as (typeof alignments)[number];
      chart.setAttribute('title-align', currentAlign);
    },
  );
  controls.appendChild(alignControl.element);

  return container;
};
TitleAlign.parameters = { docs: { story: { height: '320px' } } };

export const TitleAndLegendPositions: Story<FluentGaugeChart> = () => {
  const container = document.createElement('div');
  const controls = document.createElement('div');
  controls.setAttribute('style', controlsRowStyle);
  container.appendChild(controls);

  const positions = ['bottom', 'top', 'start', 'end'] as const;
  const titlePositions = ['top', 'bottom'] as const;
  let currentPosition: (typeof positions)[number] = 'bottom';
  let currentTitlePosition: (typeof titlePositions)[number] = 'top';

  const chart = document.createElement('fluent-gauge-chart') as FluentGaugeChart;
  chart.setAttribute('chart-title', 'Title and legend position example');
  chart.setAttribute('segments', JSON.stringify(multiSegments));
  chart.setAttribute('chart-value', '50');
  chart.setAttribute('max-value', '100');
  chart.setAttribute('style', 'margin-top:20px;');
  container.appendChild(chart);

  const posControl = createDropdownField(
    'Legend position',
    'gauge-legend-position',
    [...positions],
    currentPosition,
    nextPosition => {
      currentPosition = nextPosition as (typeof positions)[number];
      if (currentPosition === 'bottom') {
        chart.removeAttribute('legend-position');
      } else {
        chart.setAttribute('legend-position', currentPosition);
      }
    },
  );

  const titlePosControl = createDropdownField(
    'Title position',
    'gauge-title-position',
    [...titlePositions],
    currentTitlePosition,
    nextTitlePosition => {
      currentTitlePosition = nextTitlePosition as (typeof titlePositions)[number];
      if (currentTitlePosition === 'top') {
        chart.removeAttribute('title-position');
      } else {
        chart.setAttribute('title-position', currentTitlePosition);
      }
    },
  );
  controls.appendChild(titlePosControl.element);
  controls.appendChild(posControl.element);

  return container;
};
TitleAndLegendPositions.parameters = { docs: { story: { height: '320px' } } };

export const RTL: Story<FluentGaugeChart> = renderComponent(html<StoryArgs<FluentGaugeChart>>`
  <div dir="rtl">
    <fluent-gauge-chart
      chart-title="Gauge chart RTL example"
      segments="${JSON.stringify(multiSegments)}"
      chart-value="50"
      max-value="100"
    >
    </fluent-gauge-chart>
  </div>
`);
RTL.parameters = { docs: { story: { height: '320px' } } };
