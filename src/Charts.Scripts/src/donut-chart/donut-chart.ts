import { attr, FASTElement, nullableNumberConverter, observable } from '@microsoft/fast-element';
import { format as d3Format } from 'd3-format';
import { arc as d3Arc, pie as d3Pie, PieArcDatum } from 'd3-shape';
import {
  getColorFromToken,
  getNextColor,
  getRTL,
  jsonConverter,
  booleanStringConverter,
  SVG_NAMESPACE_URI,
  validateChartProps,
  wrapText,
} from '../utils/chart-helpers.js';
import type { ChartDataPoint, ChartProps, Legend } from './donut-chart.options.js';

export class DonutChart extends FASTElement {
  @attr({ attribute: 'chart-title' })
  public chartTitle?: string;

  @attr({ converter: nullableNumberConverter })
  public height: number = 200;

  @attr({ converter: nullableNumberConverter })
  public width: number = 200;

  @attr({ attribute: 'hide-legends', converter: booleanStringConverter })
  public hideLegends: boolean = false;

  @attr({ attribute: 'hide-tooltip', converter: booleanStringConverter })
  public hideTooltip: boolean = false;

  @attr({ attribute: 'hide-labels', converter: booleanStringConverter })
  public hideLabels: boolean = true;

  @attr({ attribute: 'show-labels-in-percent', converter: booleanStringConverter })
  public showLabelsInPercent: boolean = false;

  @attr({ attribute: 'round-corners', converter: booleanStringConverter })
  public roundCorners: boolean = false;

  @attr
  public order: 'default' | 'sorted' = 'default';

  @attr({ converter: jsonConverter })
  public data!: ChartProps;

  @attr({ attribute: 'inner-radius', converter: nullableNumberConverter })
  public innerRadius: number = 1;

  @attr({ attribute: 'value-inside-donut' })
  public valueInsideDonut?: string;

  @attr({ attribute: 'legend-list-label' })
  public legendListLabel?: string;

  @attr
  public culture?: string;

  @observable
  public legends: Legend[] = [];

  @observable
  public activeLegend: string = '';
  protected activeLegendChanged(oldValue: string, newValue: string) {
    if (newValue === '') {
      this._arcs?.forEach(arc => arc.classList.remove('inactive'));
      this._arcLabels?.forEach(label => label.classList.remove('inactive'));
    } else {
      this._arcs?.forEach(arc => {
        if (arc.getAttribute('data-id') === newValue) {
          arc.classList.remove('inactive');
        } else {
          arc.classList.add('inactive');
        }
      });
      this._arcLabels?.forEach(label => {
        if (label.getAttribute('data-id') === newValue) {
          label.classList.remove('inactive');
        } else {
          label.classList.add('inactive');
        }
      });
    }

    this._updateTextInsideDonut();
  }

  @observable
  public isLegendSelected: boolean = false;

  @observable
  public tooltipProps = {
    isVisible: false,
    legend: '',
    yValue: '',
    color: '',
    xPos: 0,
    yPos: 0,
  };
  protected tooltipPropsChanged(oldValue: any, newValue: any) {
    this._updateTextInsideDonut();
  }

  public chartContainer!: HTMLDivElement;
  public group!: SVGGElement;
  public elementInternals: ElementInternals = this.attachInternals();

  private _arcs: SVGPathElement[] = [];
  private _arcLabels: SVGTextElement[] = [];
  private _isRTL: boolean = false;
  private _textInsideDonut?: SVGTextElement;
  private readonly _handleMouseLeave = () => {
    this.tooltipProps = { isVisible: false, legend: '', yValue: '', color: '', xPos: 0, yPos: 0 };
  };

  constructor() {
    super();

    this.elementInternals.role = 'region';
  }

  public handleLegendMouseoverAndFocus(legendTitle: string) {
    if (this.isLegendSelected) {
      return;
    }

    this.activeLegend = legendTitle;
  }

  public handleLegendMouseoutAndBlur() {
    if (this.isLegendSelected) {
      return;
    }

    this.activeLegend = '';
  }

  public handleLegendClick(legendTitle: string) {
    if (this.isLegendSelected && this.activeLegend === legendTitle) {
      this.activeLegend = '';
      this.isLegendSelected = false;
    } else {
      this.activeLegend = legendTitle;
      this.isLegendSelected = true;
    }
  }

  connectedCallback() {
    this._initializeFromAttributes();

    super.connectedCallback();
    this.addEventListener('mouseleave', this._handleMouseLeave);

    if (!this.data) {
      return;
    }

    this._initializeAndRender();
  }

  protected dataChanged(_oldValue: ChartProps, newValue: ChartProps) {
    if (this.$fastController.isConnected && newValue) {
      this._rerender();
    }
  }

  protected orderChanged() {
    this._rerender();
  }

  protected chartTitleChanged() {
    this._rerender();
  }

  protected heightChanged() {
    this._rerender();
  }

  protected widthChanged() {
    this._rerender();
  }

  protected hideLabelsChanged() {
    this._rerender();
  }

  protected showLabelsInPercentChanged() {
    this._rerender();
  }

  protected cultureChanged() {
    this._rerender();
  }

  protected roundCornersChanged() {
    this._rerender();
  }

  private _rerender() {
    if (!this.$fastController.isConnected || !this.data) {
      return;
    }
    this._clearChart();
    this._initializeAndRender();
  }

  private _initializeAndRender() {
    validateChartProps(this.data, 'data');

    const chartData =
      this.order === 'sorted' ? [...this.data.chartData].sort((a, b) => b.data - a.data) : this.data.chartData;

    chartData.forEach((dataPoint, index) => {
      if (dataPoint.color) {
        dataPoint.color = getColorFromToken(dataPoint.color);
      } else {
        dataPoint.color = getNextColor(index);
      }
    });

    this.legends = this._getLegends(chartData);
    this._isRTL = getRTL(this);
    this.elementInternals.ariaLabel = this.chartTitle || `Donut chart with ${chartData.length} segments.`;

    this._render(chartData);
  }

  private _initializeFromAttributes() {
    const setString = (name: string, assign: (value: string) => void) => {
      const value = this.getAttribute(name);
      if (value !== null) {
        assign(value);
      }
    };

    const setBoolean = (name: string, assign: (value: boolean) => void) => {
      const value = this.getAttribute(name);
      if (value !== null) {
        assign(booleanStringConverter.fromView(value));
      }
    };

    const setNumber = (name: string, assign: (value: number) => void) => {
      const value = this.getAttribute(name);
      if (value !== null) {
        const parsed = nullableNumberConverter.fromView(value);
        if (parsed !== null && parsed !== undefined) {
          assign(parsed);
        }
      }
    };

    setString('data', value => { this.data = jsonConverter.fromView(value) as ChartProps; });
    setString('chart-title', value => { this.chartTitle = value; });
    setString('value-inside-donut', value => { this.valueInsideDonut = value; });
    setString('legend-list-label', value => { this.legendListLabel = value; });
    setString('culture', value => { this.culture = value; });
    setString('order', value => { this.order = value as 'default' | 'sorted'; });

    setNumber('inner-radius', value => { this.innerRadius = value; });
    setNumber('height', value => { this.height = value; });
    setNumber('width', value => { this.width = value; });

    setBoolean('hide-legends', value => { this.hideLegends = value; });
    setBoolean('hide-tooltip', value => { this.hideTooltip = value; });
    setBoolean('hide-labels', value => { this.hideLabels = value; });
    setBoolean('show-labels-in-percent', value => { this.showLabelsInPercent = value; });
    setBoolean('round-corners', value => { this.roundCorners = value; });
  }

  private _clearChart() {
    while (this.group.firstChild) {
      this.group.removeChild(this.group.firstChild);
    }
    this._arcs = [];
    this._arcLabels = [];
    this._textInsideDonut = undefined;
  }

  private _render(chartData: ChartDataPoint[]) {
    const totalValue = chartData.reduce((total, dataPoint) => total + dataPoint.data, 0);
    const outerRadius = (Math.min(this.height, this.width) - 20) / 2;
    const cornerRadius = this.roundCorners ? 3 : 0;
    const pie = d3Pie<ChartDataPoint>()
      .value(d => d.data)
      .padAngle(0.02);
    const arc = d3Arc<PieArcDatum<ChartDataPoint>>()
      .innerRadius(this.innerRadius)
      .outerRadius(outerRadius)
      .cornerRadius(cornerRadius);

    pie(chartData).forEach(arcDatum => {
      const arcGroup = document.createElementNS(SVG_NAMESPACE_URI, 'g');
      this.group.appendChild(arcGroup);

      const pathOutline = document.createElementNS(SVG_NAMESPACE_URI, 'path');
      arcGroup.appendChild(pathOutline);
      pathOutline.classList.add('arc-outline');
      pathOutline.setAttribute('d', arc(arcDatum)!);

      const path = document.createElementNS(SVG_NAMESPACE_URI, 'path');
      arcGroup.appendChild(path);
      this._arcs.push(path);
      path.classList.add('arc');
      path.setAttribute('d', arc(arcDatum)!);
      path.setAttribute('fill', arcDatum.data.color!);
      path.setAttribute('data-id', arcDatum.data.legend);
      path.setAttribute('tabindex', '0');
      path.setAttribute('aria-label', `${arcDatum.data.legend}, ${arcDatum.data.data}.`);
      path.setAttribute('role', 'img');

      path.addEventListener('mouseover', event => {
        if (this.activeLegend !== '' && this.activeLegend !== arcDatum.data.legend) {
          return;
        }

        const bounds = this.getBoundingClientRect();

        this.tooltipProps = {
          isVisible: true,
          legend: arcDatum.data.legend,
          yValue: `${arcDatum.data.data}`,
          color: arcDatum.data.color!,
          xPos: this._isRTL ? bounds.right - event.clientX : event.clientX - bounds.left,
          yPos: event.clientY - bounds.top - 85,
        };
      });
      path.addEventListener('focus', event => {
        if (this.activeLegend !== '' && this.activeLegend !== arcDatum.data.legend) {
          return;
        }

        const rootBounds = this.getBoundingClientRect();
        const arcBounds = path.getBoundingClientRect();

        this.tooltipProps = {
          isVisible: true,
          legend: arcDatum.data.legend,
          yValue: `${arcDatum.data.data}`,
          color: arcDatum.data.color!,
          xPos: this._isRTL
            ? rootBounds.right - arcBounds.left - arcBounds.width / 2
            : arcBounds.left + arcBounds.width / 2 - rootBounds.left,
          yPos: arcBounds.top - rootBounds.top - 85,
        };
      });
      path.addEventListener('blur', event => {
        this.tooltipProps = { isVisible: false, legend: '', yValue: '', color: '', xPos: 0, yPos: 0 };
      });

      const label = this._createArcLabel(arc, arcDatum, totalValue, outerRadius);
      if (label) {
        arcGroup.appendChild(label);
        this._arcLabels.push(label);
      }
    });

    if (this.valueInsideDonut) {
      this._textInsideDonut = document.createElementNS(SVG_NAMESPACE_URI, 'text');
      this.group.appendChild(this._textInsideDonut);
      this._textInsideDonut.classList.add('text-inside-donut');
      this._textInsideDonut.setAttribute('x', '0');
      this._textInsideDonut.setAttribute('y', '0');
      this._textInsideDonut.setAttribute('text-anchor', 'middle');
      this._textInsideDonut.setAttribute('dominant-baseline', 'middle');
      this._updateTextInsideDonut();
    }
  }

  private _getLegends(chartData: ChartDataPoint[]): Legend[] {
    return chartData.map((d, index) => ({
      legend: d.legend,
      color: d.color!,
    }));
  }

  private _createArcLabel(
    arc: ReturnType<typeof d3Arc<PieArcDatum<ChartDataPoint>>>,
    arcDatum: PieArcDatum<ChartDataPoint>,
    totalValue: number,
    outerRadius: number,
  ) {
    if (this.hideLabels || Math.abs(arcDatum.endAngle - arcDatum.startAngle) < Math.PI / 12) {
      return undefined;
    }

    const [base, perp] = arc.centroid(arcDatum);
    const hypotenuse = Math.sqrt(base * base + perp * perp);
    const labelRadius = Math.max(this.innerRadius, outerRadius) + 2;
    const angle = (arcDatum.startAngle + arcDatum.endAngle) / 2;
    const label = document.createElementNS(SVG_NAMESPACE_URI, 'text');

    label.classList.add('arc-label');
    label.setAttribute('data-id', arcDatum.data.legend);
    label.setAttribute('x', `${(hypotenuse === 0 ? 0 : base / hypotenuse) * labelRadius}`);
    label.setAttribute('y', `${(hypotenuse === 0 ? 0 : perp / hypotenuse) * labelRadius}`);
    label.setAttribute('text-anchor', angle > Math.PI !== this._isRTL ? 'end' : 'start');
    label.setAttribute('dominant-baseline', angle > Math.PI / 2 && angle < (3 * Math.PI) / 2 ? 'hanging' : 'auto');
    label.setAttribute('aria-hidden', 'true');
    label.textContent = this.showLabelsInPercent
      ? d3Format('.0%')(totalValue === 0 ? 0 : arcDatum.value / totalValue)
      : this._formatArcLabelValue(arcDatum.value);

    return label;
  }

  private _formatArcLabelValue(value: number) {
    const formatted = new Intl.NumberFormat(this.culture || undefined, {
      maximumFractionDigits: value >= 1000 ? 1 : 2,
      notation: value >= 1000 ? 'compact' : 'standard',
    }).format(value);

    return formatted.endsWith('K') ? `${formatted.slice(0, -1)}k` : formatted;
  }

  private _getTextInsideDonut(valueInsideDonut: string) {
    let textInsideDonut = valueInsideDonut;

    if (valueInsideDonut && (this.activeLegend !== '' || this.tooltipProps.isVisible)) {
      const highlightedDataPoint = this.data.chartData.find(
        dataPoint =>
          dataPoint.legend === this.activeLegend ||
          (this.tooltipProps.isVisible && dataPoint.legend === this.tooltipProps.legend),
      );
      if (this.showLabelsInPercent) {
        const total = this.data.chartData.reduce((acc, point) => acc + point.data, 0);
        const percentage = total > 0 ? Math.round(((highlightedDataPoint?.data ?? 0) / total) * 100) : 0;
        textInsideDonut = `${percentage}%`;
      } else {
        textInsideDonut =
          highlightedDataPoint!.calloutData ?? highlightedDataPoint!.data.toLocaleString(this.culture || undefined);
      }
    }

    return textInsideDonut;
  }

  private _updateTextInsideDonut() {
    if (!this._textInsideDonut || !this.valueInsideDonut) {
      return;
    }

    this._textInsideDonut.textContent = this._getTextInsideDonut(this.valueInsideDonut);
    const lineHeight = this._textInsideDonut.getBoundingClientRect().height;
    wrapText(this._textInsideDonut, 2 * this.innerRadius);
    const lines = this._textInsideDonut.getElementsByTagName('tspan');
    const start = -1 * Math.trunc((lines.length - 1) / 2);
    for (let i = 0; i < lines.length; i++) {
      lines[i].setAttribute('dy', `${(start + i) * lineHeight}`);
    }
  }
}
