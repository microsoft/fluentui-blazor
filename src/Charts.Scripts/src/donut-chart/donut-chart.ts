import { attr, FASTElement, nullableNumberConverter, observable } from '@microsoft/fast-element';
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
import type { ChartDataPoint, ChartProps, Legend } from './donut-chart.options.js';

export class DonutChart extends FASTElement {
  @observable
  public tooltipProps = {
    isVisible: false,
    legend: '',
    yValue: '',
    color: '',
    xPos: 0,
    yPos: 0,
  };

  @observable
  public legends: Legend[] = [];

  @observable
  public activeLegend: string = '';
  protected activeLegendChanged(_oldValue: string, _newValue: string) {
    if (this._isSettingActiveLegend) {
      return;
    }

    this._updateLegendInteractionState();
  }

  @observable
  public isLegendSelected: boolean = false;

  @observable
  public selectedLegends: string[] = [];


  @attr({ attribute: 'chart-title' })
  public chartTitle?: string;

  @attr({ converter: nullableNumberConverter })
  public height: number = 200;

  @attr({ converter: nullableNumberConverter })
  public width: number = 200;

  @attr({ attribute: 'hide-legends', mode: 'boolean' })
  public hideLegends: boolean = false;

  @attr({ attribute: 'hide-tooltip', mode: 'boolean' })
  public hideTooltip: boolean = false;

  @attr({ attribute: 'hide-labels', mode: 'boolean' })
  public hideLabels: boolean = false;

  @attr({ attribute: 'show-labels-in-percent', mode: 'boolean' })
  public showLabelsInPercent: boolean = false;

  @attr({ attribute: 'round-corners', mode: 'boolean' })
  public roundCorners: boolean = false;

  @attr({ converter: jsonConverter })
  public data!: ChartProps;

  @attr({ attribute: 'inner-radius', converter: nullableNumberConverter })
  public innerRadius: number = 1;

  @attr({ attribute: 'value-inside-donut' })
  public valueInsideDonut?: string;

  @attr({ attribute: 'legend-list-label' })
  public legendListLabel?: string;

  @attr
  public order: 'default' | 'sorted' = 'default';

  @attr
  public culture?: string;

  @attr({ attribute: 'allow-multiple-legend-selection', mode: 'boolean' })
  public allowMultipleLegendSelection: boolean = false;

  public chartContainer!: HTMLDivElement;
  public group!: SVGGElement;
  public elementInternals: ElementInternals = this.attachInternals();

  private _arcs: SVGPathElement[] = [];
  private _arcLabels: SVGTextElement[] = [];
  private _isRTL: boolean = false;
  private _isSettingActiveLegend: boolean = false;
  private _isSettingTooltipProps: boolean = false;
  private _textInsideDonut?: SVGTextElement;

  private _renderPending = false;
  private _renderDirty = false;
  private _frameHandle: number | null = null;

  private readonly _handleMouseLeave = () => {
    this._setTooltipProps({ isVisible: false, legend: '', yValue: '', color: '', xPos: 0, yPos: 0 });
  };

  constructor() {
    super();

    this.elementInternals.role = 'region';
  }

  protected tooltipPropsChanged(_oldValue: any, _newValue: any) {
    if (this._isSettingTooltipProps) {
      return;
    }

    this._updateTooltipState();
  }

  public handleLegendMouseoverAndFocus(legendTitle: string) {
    if (this.allowMultipleLegendSelection) {
      if (this.selectedLegends.length > 0) {
        return;
      }
    } else if (this.isLegendSelected) {
      return;
    }

    this._setActiveLegend(legendTitle);
  }

  public handleLegendMouseoutAndBlur() {
    if (this.allowMultipleLegendSelection) {
      if (this.selectedLegends.length > 0) {
        return;
      }
    } else if (this.isLegendSelected) {
      return;
    }

    this._setActiveLegend('');
  }

  public handleLegendClick(legendTitle: string) {
    if (this.allowMultipleLegendSelection) {
      const nextSelection = this.selectedLegends.includes(legendTitle)
        ? this.selectedLegends.filter(legend => legend !== legendTitle)
        : [...this.selectedLegends, legendTitle];
      this.selectedLegends = nextSelection;
      if (nextSelection.length === 0) {
        this._setActiveLegend('');
      } else if (!nextSelection.includes(this.activeLegend)) {
        this._setActiveLegend(nextSelection[nextSelection.length - 1]);
      } else {
        this._updateLegendInteractionState();
      }
      return;
    }

    if (this.isLegendSelected && this.activeLegend === legendTitle) {
      this._setActiveLegend('');
      this.isLegendSelected = false;
    } else {
      this._setActiveLegend(legendTitle);
      this.isLegendSelected = true;
    }
  }

  public isLegendItemSelected(legendTitle: string) {
    return Array.isArray(this.selectedLegends) && this.selectedLegends.includes(legendTitle);
  }

  public isLegendItemDimmed(legendTitle: string) {
    const highlighted = this._getHighlightedLegends();
    return highlighted.length > 0 && !highlighted.includes(legendTitle);
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
      'chartTitle',
      'height',
      'width',
      'hideLegends',
      'hideTooltip',
      'hideLabels',
      'showLabelsInPercent',
      'roundCorners',
      'data',
      'innerRadius',
      'valueInsideDonut',
      'legendListLabel',
      'order',
      'culture',
      'allowMultipleLegendSelection',
    ] as const;
    const observableFields = ['tooltipProps', 'legends', 'activeLegend', 'isLegendSelected', 'selectedLegends'] as const;

    const saved: Partial<Record<(typeof attrFields)[number], unknown>> = {};
    const savedObservables: Partial<Record<(typeof observableFields)[number], unknown>> = {};

    for (const field of attrFields) {
      saved[field] = self[field];
      delete self[field];
    }
    for (const field of observableFields) {
      savedObservables[field] = self[field];
      delete self[field];
      // Restore observable defaults through the prototype's FAST reactive setter
      // BEFORE super.connectedCallback() renders the template.
      if (savedObservables[field] !== undefined) {
        self[field] = savedObservables[field];
      }
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
    this._cancelScheduledRender();
    super.disconnectedCallback();
  }

  protected roundCornersChanged() {
    this._requestRender();
  }

  protected dataChanged(_oldValue: ChartProps, newValue: ChartProps) {
    if (newValue) {
      this._requestRender();
    }
  }

  protected chartTitleChanged() {
    this._requestRender();
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

  protected hideLabelsChanged() {
    this._requestRender();
  }

  protected hideLegendsChanged(_oldValue: boolean, newValue: boolean) {
    this.shadowRoot?.querySelector('.legend-container')?.toggleAttribute('hidden', newValue);
  }

  protected hideTooltipChanged() {
    this._updateTooltipState();
  }

  protected showLabelsInPercentChanged() {
    this._requestRender();
  }

  protected cultureChanged() {
    this._requestRender();
  }

  protected orderChanged() {
    this._requestRender();
  }

  protected allowMultipleLegendSelectionChanged() {
    if (!this.allowMultipleLegendSelection) {
      this.selectedLegends = [];
      this._setActiveLegend('');
      this.isLegendSelected = false;
      return;
    }

    this._updateLegendInteractionState();
  }

  protected selectedLegendsChanged() {
    this._updateLegendInteractionState();
  }

  /**
   * Schedules a single re-render on the next animation frame, batching
   * multiple attribute changes from one Blazor render cycle into a single
   * visual update. Interactive-state changes (activeLegend, tooltipProps)
   * bypass this and update immediately.
   */
  private _requestRender(): void {
    this._renderDirty = true;

    if (this._renderPending) {
      return;
    }

    this._renderPending = true;
    this._frameHandle = requestAnimationFrame(() => {
      this._renderPending = false;
      this._frameHandle = null;

      if (!this._renderDirty) {
        return;
      }

      this._renderDirty = false;
      this._rerenderIfReady();
    });
  }

  private _cancelScheduledRender(): void {
    if (this._frameHandle !== null) {
      cancelAnimationFrame(this._frameHandle);
      this._frameHandle = null;
    }

    this._renderPending = false;
    this._renderDirty = false;
  }

  private _rerenderIfReady() {
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
    this.elementInternals.ariaLabel =
      this.chartTitle || this.data.chartTitle || `Donut chart with ${chartData.length} segments.`;

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

    const pie = d3Pie<ChartDataPoint>().value(d => d.data).padAngle(0.02);

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
        const highlighted = this._getHighlightedLegends();
        if (highlighted.length > 0 && !highlighted.includes(arcDatum.data.legend)) {
          return;
        }

        const bounds = this.getBoundingClientRect();

        this._setTooltipProps({
          isVisible: true,
          legend: arcDatum.data.legend,
          yValue: this._formatDataPointValue(arcDatum.data),
          color: arcDatum.data.color!,
          xPos: this._isRTL ? bounds.right - event.clientX : event.clientX - bounds.left,
          yPos: event.clientY - bounds.top - 85,
        });
      });

      path.addEventListener('focus', () => {
        const highlighted = this._getHighlightedLegends();
        if (highlighted.length > 0 && !highlighted.includes(arcDatum.data.legend)) {
          return;
        }

        const rootBounds = this.getBoundingClientRect();
        const arcBounds = path.getBoundingClientRect();

        this._setTooltipProps({
          isVisible: true,
          legend: arcDatum.data.legend,
          yValue: this._formatDataPointValue(arcDatum.data),
          color: arcDatum.data.color!,
          xPos: this._isRTL
            ? rootBounds.right - arcBounds.left - arcBounds.width / 2
            : arcBounds.left + arcBounds.width / 2 - rootBounds.left,
          yPos: arcBounds.top - rootBounds.top - 85,
        });
      });

      path.addEventListener('blur', () => {
        this._setTooltipProps({ isVisible: false, legend: '', yValue: '', color: '', xPos: 0, yPos: 0 });
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

    this._updateTooltipState();
  }

  private _getLegends(chartData: ChartDataPoint[]): Legend[] {
    return chartData.map(d => ({
      title: d.legend,
      color: d.color!,
    }));
  }

  private _getHighlightedLegends(): string[] {
    if (this.allowMultipleLegendSelection) {
      if (Array.isArray(this.selectedLegends) && this.selectedLegends.length > 0) {
        return this.selectedLegends;
      }
      return this.activeLegend ? [this.activeLegend] : [];
    }
    return this.activeLegend ? [this.activeLegend] : [];
  }

  private _applyActiveLegendState() {
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
      return;
    }

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

  private _updateLegendInteractionState() {
    this._applyActiveLegendState();
    this._applyLegendButtonState();
    this._updateTextInsideDonut();
  }

  private _setActiveLegend(value: string) {
    this._isSettingActiveLegend = true;
    this.activeLegend = value;
    this._isSettingActiveLegend = false;
    this._updateLegendInteractionState();
  }

  private _applyLegendButtonState() {
    const legends = this.shadowRoot?.querySelectorAll<HTMLButtonElement>('.legend');
    if (!legends) {
      return;
    }

    const highlighted = this._getHighlightedLegends();
    legends.forEach(button => {
      const title = button.querySelector('.legend-text')?.textContent ?? '';
      const isActive = highlighted.length === 0 || highlighted.includes(title);
      button.classList.toggle('inactive', !isActive);
      button.setAttribute('aria-selected', `${highlighted.includes(title)}`);
    });
  }

  private _updateTooltipState() {
    this._updateTextInsideDonut();
  }

  private _setTooltipProps(value: typeof this.tooltipProps) {
    this._isSettingTooltipProps = true;
    this.tooltipProps = value;
    this._isSettingTooltipProps = false;
    this._updateTooltipState();
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
    return dataPoint.yAxisCalloutData ?? dataPoint.calloutData ?? dataPoint.data.toLocaleString(this.culture || undefined);
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
