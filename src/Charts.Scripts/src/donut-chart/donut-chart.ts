import { attr, nullableNumberConverter } from '@microsoft/fast-element';
import { ChartBase } from '../utils/chart-base.js';
import { format as d3Format } from 'd3-format';
import { arc as d3Arc, pie as d3Pie, PieArcDatum } from 'd3-shape';
import {
  getColorFromToken,
  getNextColor,
  getRTL,
  jsonConverter,
  SVG_NAMESPACE_URI,
  validateChartProps,
  wrapText,
} from '../utils/chart-helpers.js';
import type { ChartDataPoint, ChartProps } from './donut-chart.options.js';
import type { Legend } from '../utils/chart.options.js';

export class DonutChart extends ChartBase {
  @attr({ converter: nullableNumberConverter })
  public height: number = 200;

  @attr({ converter: nullableNumberConverter })
  public width: number = 200;

  @attr({ attribute: 'show-labels-in-percent', mode: 'boolean' })
  public showLabelsInPercent: boolean = false;

  @attr({ converter: jsonConverter })
  public data!: ChartProps;

  @attr({ attribute: 'inner-radius', converter: nullableNumberConverter })
  public innerRadius: number = 1;

  @attr({ attribute: 'value-inside-donut' })
  public valueInsideDonut?: string;

  @attr
  public order: 'default' | 'sorted' = 'default';

  public group!: SVGGElement;

  private _arcs: SVGPathElement[] = [];
  private _arcLabels: SVGTextElement[] = [];
  private _textInsideDonut?: SVGTextElement;

  private readonly _handleMouseLeave = () => {
    this._clearTooltip();
  };

  protected tooltipPropsChanged(_oldValue: any, _newValue: any) {
    this._updateTextInsideDonut();
  }

  connectedCallback() {
    // Class field initializers create own data properties that shadow the FAST @attr
    // and @observable reactive getter/setters on the prototype. Delete them so that
    // attribute changes go through the FAST reactive system and trigger the *Changed()
    // callbacks, and so that observable assignments notify template bindings.
    // We save the default values first so we can restore them for fields that have
    // no corresponding HTML attribute (FAST won't call the setter in that case).
    const self = this as Record<string, unknown>;
    const attrFields = [
      'height',
      'width',
      'showLabelsInPercent',
      'data',
      'innerRadius',
      'valueInsideDonut',
      'order',
    ] as const;

    const saved: Partial<Record<(typeof attrFields)[number], unknown>> = {};

    for (const field of attrFields) {
      saved[field] = self[field];
      delete self[field];
    }

    super.connectedCallback();

    // Restore defaults for any attr-backed field that was not set from an HTML attribute.
    for (const field of attrFields) {
      if (self[field] === undefined && saved[field] !== undefined) {
        self[field] = saved[field];
      }
    }

    this.addEventListener('mouseleave', this._handleMouseLeave);
    this._requestRender();
  }

  public disconnectedCallback() {
    this.removeEventListener('mouseleave', this._handleMouseLeave);
    super.disconnectedCallback();
  }

  protected dataChanged(_oldValue: ChartProps, newValue: ChartProps) {
    if (newValue) {
      this._requestRender();
    }
  }

  protected widthChanged() {
    this._requestRender();
  }

  protected heightChanged() {
    this._requestRender();
  }

  protected innerRadiusChanged() {
    this._requestRender();
  }

  protected valueInsideDonutChanged() {
    this._requestRender();
  }

  protected showLabelsInPercentChanged() {
    this._requestRender();
  }

  protected _getHostAriaLabel(): string {
    return this.chartTitle || `Donut chart with ${this.data?.chartData?.length ?? 0} segments.`;
  }

  protected orderChanged() {
    this._requestRender();
  }

  protected _performRender() {
    if (!this.$fastController.isConnected || !this.data || !this.group) {
      return;
    }

    this._clearChart();
    this._initializeAndRender();
  }

  private _clearChart() {
    if (this.group) {
      while (this.group.firstChild) {
        this.group.removeChild(this.group.firstChild);
      }
    }

    this._arcs = [];
    this._arcLabels = [];
    this._textInsideDonut = undefined;
  }

  private _initializeAndRender() {
    const chartData = this._prepareChartData();

    this._isRTL = getRTL(this);

    this._render(chartData);
  }

  private _prepareChartData(): ChartDataPoint[] {
    validateChartProps(this.data, 'data');

    const chartData = this._resolveChartData();

    this.legends = this._getLegends(chartData);
    this.elementInternals.ariaLabel = this._getHostAriaLabel();

    return chartData;
  }

  private _resolveChartData(): ChartDataPoint[] {
    const sourceData =
      this.order === 'sorted' ? [...this.data.chartData].sort((a, b) => b.data - a.data) : this.data.chartData;

    const totalValue = sourceData.reduce((sum, point) => sum + (point.data ?? 0), 0);
    const minimumValue = totalValue * 0.01;

    return sourceData.map((dataPoint, index) => {
      const color = dataPoint.color ? getColorFromToken(dataPoint.color) : getNextColor(index);
      const resolvedData = minimumValue > dataPoint.data && dataPoint.data > 0 ? minimumValue : dataPoint.data;

      return {
        ...dataPoint,
        color,
        data: resolvedData,
        yAxisCalloutData:
          resolvedData !== dataPoint.data
            ? dataPoint.yAxisCalloutData ?? dataPoint.data.toLocaleString(this.culture || undefined)
            : dataPoint.yAxisCalloutData,
      };
    });
  }

  private _render(chartData: ChartDataPoint[]) {
    const totalValue = chartData.reduce((sum, point) => sum + (point.data ?? 0), 0);
    const outerRadius = Math.max(0, (Math.min(this.height, this.width) - 20) / 2);
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
        if (!this._shouldShowTooltip(arcDatum.data.legend)) {
          return;
        }

        const bounds = this.getBoundingClientRect();

        this.tooltipProps = {
          isVisible: true,
          legend: arcDatum.data.legend,
          yValue: this._formatDataPointValue(arcDatum.data),
          color: arcDatum.data.color!,
          xPos: this._isRTL ? bounds.right - event.clientX : event.clientX - bounds.left,
          yPos: event.clientY - bounds.top - 85,
        };
      });

      path.addEventListener('focus', () => {
        if (!this._shouldShowTooltip(arcDatum.data.legend)) {
          return;
        }

        const rootBounds = this.getBoundingClientRect();
        const arcBounds = path.getBoundingClientRect();

        this.tooltipProps = {
          isVisible: true,
          legend: arcDatum.data.legend,
          yValue: this._formatDataPointValue(arcDatum.data),
          color: arcDatum.data.color!,
          xPos: this._isRTL
            ? rootBounds.right - arcBounds.left - arcBounds.width / 2
            : arcBounds.left + arcBounds.width / 2 - rootBounds.left,
          yPos: arcBounds.top - rootBounds.top - 85,
        };
      });

      path.addEventListener('blur', () => {
        this._clearTooltip();
      });

      const label = this._createArcLabel(arc, arcDatum, totalValue, outerRadius);
      if (label) {
        arcGroup.appendChild(label);
        this._arcLabels.push(label);
      }
    });

    this._applyActiveLegendState();
    this._applyLegendButtonState();

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
    return chartData.map(d => ({
      legend: d.legend,
      color: d.color!,
    }));
  }

  protected _applyActiveLegendState() {
    if (!this._arcs || !this._arcLabels) {
      return;
    }

    const highlighted = this._getHighlightedLegends();

    if (highlighted.length === 0) {
      this._arcs.forEach(arcEl => {
        arcEl.classList.remove('inactive');
        arcEl.setAttribute('tabindex', '0');
      });
      this._arcLabels.forEach(label => label.classList.remove('inactive'));
    } else {
      this._arcs.forEach(arcEl => {
        const legendId = arcEl.getAttribute('data-id');
        const isActive = legendId !== null && highlighted.includes(legendId);
        arcEl.classList.toggle('inactive', !isActive);
        arcEl.setAttribute('tabindex', isActive ? '0' : '-1');
      });

      this._arcLabels.forEach(label => {
        const legendId = label.getAttribute('data-id');
        label.classList.toggle('inactive', legendId === null || !highlighted.includes(legendId));
      });
    }

    this._updateTextInsideDonut();
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

  private _formatDataPointValue(dataPoint: ChartDataPoint): string {
    return (
      dataPoint.yAxisCalloutData ?? dataPoint.calloutData ?? dataPoint.data.toLocaleString(this.culture || undefined)
    );
  }

  private _getTextInsideDonut(valueInsideDonut: string) {
    let textInsideDonut = valueInsideDonut;

    const highlighted = this._getHighlightedLegends();
    const singleHighlight =
      highlighted.length === 1 ? highlighted[0] : this.tooltipProps.isVisible ? this.tooltipProps.legend : null;

    if (valueInsideDonut && singleHighlight) {
      const highlightedDataPoint = this.data.chartData.find(dataPoint => dataPoint.legend === singleHighlight);
      if (highlightedDataPoint) {
        textInsideDonut = this._formatDataPointValue(highlightedDataPoint);
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
