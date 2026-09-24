import { attr, nullableNumberConverter, observable } from '@microsoft/fast-element';
import { ChartBase } from '../utils/chart-base.js';
import { format as d3Format } from 'd3-format';
import { arc as d3Arc, pie as d3Pie, PieArcDatum } from 'd3-shape';
import {
  createNumberFormat,
  formatLocaleNumber,
  getColorFromToken,
  getNextColor,
  jsonConverter,
  SVG_NAMESPACE_URI,
  validateDonutDataArray,
  wrapText,
} from '../utils/chart-helpers.js';
import type { DonutChartDataPoint } from './donut-chart.options.js';
import type { Legend, TooltipRenderer } from '../utils/chart-options.js';

export class DonutChart extends ChartBase {
  @attr({ attribute: 'show-labels-in-percent', mode: 'boolean' })
  public showLabelsInPercent: boolean = false;

  @attr({ converter: jsonConverter })
  public data!: DonutChartDataPoint[];

  @attr({ attribute: 'inner-radius', converter: nullableNumberConverter })
  public innerRadius: number = 55;

  @attr({ attribute: 'value-inside-donut' })
  public valueInsideDonut?: string;

  @attr
  public order: 'default' | 'sorted' = 'default';

  /**
   * Optional formatter function for the text inside the donut.
   * Receives the sum of all data values and returns the formatted string.
   * When provided, this takes precedence over format string processing.
   */
  @observable
  public valueInsideFormatter: ((value: number) => string) | null = null;

  public group!: SVGGElement;

  protected override _enableResizeObserver = true;

  /** Narrows the inherited base tooltipRenderer type to the DonutChart data point. */
  public declare tooltipRenderer: TooltipRenderer<DonutChartDataPoint> | undefined;

  private _arcs: SVGPathElement[] = [];
  private _arcLabels: SVGTextElement[] = [];
  private _textInsideDonut?: SVGTextElement;

  private readonly _handleMouseLeave = () => {
    this._clearTooltip();
  };

  connectedCallback() {
    // Class field initializers create own data properties that shadow the FAST @attr
    // and @observable reactive getter/setters on the prototype. Delete them so that
    // attribute changes go through the FAST reactive system and trigger the *Changed()
    // callbacks, and so that observable assignments notify template bindings.
    // We save the default values first so we can restore them for fields that have
    // no corresponding HTML attribute (FAST won't call the setter in that case).
    const self = this as Record<string, unknown>;
    const reactiveFields = [
      'showLabelsInPercent',
      'data',
      'innerRadius',
      'valueInsideDonut',
      'order',
      'valueInsideFormatter',
    ] as const;

    const saved: Partial<Record<(typeof reactiveFields)[number], unknown>> = {};

    for (const field of reactiveFields) {
      saved[field] = self[field];
      delete self[field];
    }

    super.connectedCallback();

    // Restore defaults for any attr-backed field that was not set from an HTML attribute.
    for (const field of reactiveFields) {
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

  protected tooltipPropsChanged(_oldValue: any, _newValue: any) {
    this._updateTextInsideDonut();
    super.tooltipPropsChanged(_oldValue, _newValue);
  }

  protected dataChanged() {
    this._requestRender();
  }

  protected innerRadiusChanged() {
    this._requestRender();
  }

  protected valueInsideDonutChanged() {
    this._requestRender();
  }

  protected valueInsideFormatterChanged() {
    this._requestRender();
  }

  protected showLabelsInPercentChanged() {
    this._requestRender();
  }

  protected orderChanged() {
    this._requestRender();
  }

  protected _getHostAriaLabel(): string {
    return this.chartTitle ? `${this.chartTitle}, donut chart` : `Donut chart with ${this.data?.length ?? 0} segments.`;
  }

  protected _performRender() {
    if (!this.$fastController.isConnected || !this.data || !this.group) {
      return;
    }

    this._applyHostDimensions();
    this._clearChart();
    this._initializeAndRender();
  }

  protected _applyHostDimensions() {
    super._applyHostDimensions(this.width, this.height);
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

    this._render(chartData);
  }

  private _prepareChartData(): DonutChartDataPoint[] {
    validateDonutDataArray(this.data, 'data');

    const chartData = this._resolveChartData();

    this.legends = this._getLegends(chartData);
    this.elementInternals.ariaLabel = this._getHostAriaLabel();

    return chartData;
  }

  private _resolveChartData(): DonutChartDataPoint[] {
    const sourceData = this.order === 'sorted' ? [...this.data].sort((a, b) => b.data - a.data) : this.data;

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
            ? dataPoint.yAxisCalloutData ?? formatLocaleNumber(dataPoint.data, this.culture || undefined)
            : dataPoint.yAxisCalloutData,
      };
    });
  }

  private _render(chartData: DonutChartDataPoint[]) {
    const totalValue = chartData.reduce((sum, point) => sum + (point.data ?? 0), 0);
    const svgEl = this.group.ownerSVGElement!;
    const svgRect = svgEl.getBoundingClientRect();
    const pixelWidth = svgRect.width || parseFloat(String(this.width)) || 200;
    const pixelHeight = svgRect.height || parseFloat(String(this.height)) || 200;
    this.group.setAttribute('transform', `translate(${pixelWidth / 2}, ${pixelHeight / 2})`);
    const outerRadius = Math.max(0, (Math.min(pixelHeight, pixelWidth) - 20) / 2);
    const cornerRadius = this.roundCorners ? 3 : 0;

    const pie = d3Pie<DonutChartDataPoint>()
      .value(d => d.data)
      .padAngle(0.02);

    const arc = d3Arc<PieArcDatum<DonutChartDataPoint>>()
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
      path.setAttribute('tabindex', this._arcs.length === 1 ? '0' : '-1');
      path.setAttribute('aria-label', `${arcDatum.data.legend}, ${arcDatum.data.data}.`);
      path.setAttribute('role', 'img');

      path.addEventListener('mouseover', event => this._showArcTooltip(arcDatum.data, path, event));

      path.addEventListener('focus', () => this._showArcTooltip(arcDatum.data, path));

      path.addEventListener('blur', () => {
        this._clearTooltip();
      });

      path.addEventListener('keydown', (e: KeyboardEvent) => {
        if (e.key === 'Enter' || e.key === ' ') {
          e.preventDefault();
          path.dispatchEvent(new MouseEvent('click', { bubbles: true }));
        } else {
          this._rovingKeydown(this._arcs, e);
        }
      });

      const label = this._createArcLabel(arc, arcDatum, totalValue, outerRadius);
      if (label) {
        arcGroup.appendChild(label);
        this._arcLabels.push(label);
      }
    });

    this._applyActiveLegendState();
    this._applyLegendButtonState();

    // Create text element if valueInsideDonut is set or auto-sum is enabled
    const displayValue = this._computeDisplayValue(totalValue);
    if (displayValue !== null) {
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

  private _showArcTooltip(dataPoint: DonutChartDataPoint, arcPath: SVGPathElement, event?: MouseEvent): void {
    if (!this._shouldShowTooltip(dataPoint.legend)) {
      return;
    }

    const hostBounds = this.getBoundingClientRect();
    const arcBounds = arcPath.getBoundingClientRect();
    const segmentCenterX = arcBounds.left + arcBounds.width / 2 - hostBounds.left;
    const segmentCenterY = arcBounds.top + arcBounds.height / 2 - hostBounds.top;
    const anchorX = event?.clientX !== undefined ? event.clientX - hostBounds.left : segmentCenterX;
    const anchorY = event?.clientY !== undefined ? event.clientY - hostBounds.top : segmentCenterY;
    const horizontalAlign: 'start' | 'center' | 'end' = event ? (this._isRTL ? 'end' : 'start') : 'center';

    this._currentTooltipDataPoint = dataPoint;
    this.tooltipProps = {
      isVisible: true,
      legend: dataPoint.legend,
      yValue: this._formatDataPointValue(dataPoint),
      color: dataPoint.color!,
      xPos: this._isRTL ? hostBounds.width - anchorX : anchorX,
      yPos: Math.max(0, anchorY - 8),
    };

    this._positionTooltipFromAnchor(anchorX, anchorY, {
      preferredVertical: 'above',
      horizontalAlign,
      gap: 8,
      padding: 8,
      estimatedWidth: 176,
      estimatedHeight: 64,
    });
  }

  private _getLegends(chartData: DonutChartDataPoint[]): Legend[] {
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
      });
      this._arcLabels.forEach(label => label.classList.remove('inactive'));
      if (this._arcs.length > 0 && !this._arcs.some(el => el.tabIndex === 0)) {
        this._arcs[0].tabIndex = 0;
      }
    } else {
      this._arcs.forEach(arcEl => {
        const legendId = arcEl.getAttribute('data-id');
        const isActive = legendId !== null && highlighted.includes(legendId);
        arcEl.classList.toggle('inactive', !isActive);
        if (!isActive) {
          arcEl.tabIndex = -1;
        }
      });
      const activeArcs = this._arcs.filter(el => !el.classList.contains('inactive'));
      if (activeArcs.length > 0 && !activeArcs.some(el => el.tabIndex === 0)) {
        activeArcs[0].tabIndex = 0;
      }

      this._arcLabels.forEach(label => {
        const legendId = label.getAttribute('data-id');
        label.classList.toggle('inactive', legendId === null || !highlighted.includes(legendId));
      });
      this._relocateFocusIfNeeded(this._arcs);
    }

    this._updateTextInsideDonut();
  }

  private _createArcLabel(
    arc: ReturnType<typeof d3Arc<PieArcDatum<DonutChartDataPoint>>>,
    arcDatum: PieArcDatum<DonutChartDataPoint>,
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
    const formatted = createNumberFormat(this.culture || undefined, {
      maximumFractionDigits: value >= 1000 ? 1 : 2,
      notation: value >= 1000 ? 'compact' : 'standard',
    }).format(value);

    return formatted.endsWith('K') ? `${formatted.slice(0, -1)}k` : formatted;
  }

  private _formatDataPointValue(dataPoint: DonutChartDataPoint): string {
    return (
      dataPoint.yAxisCalloutData ??
      dataPoint.calloutData ??
      formatLocaleNumber(dataPoint.data, this.culture || undefined)
    );
  }

  /**
   * Computes the display value for inside the donut.
   * Returns null if no value should be displayed.
   * Logic:
   * - If valueInsideDonut is a space, return null (force empty)
   * - If valueInsideDonut contains \{0\} placeholder, it's a format string
   * - If valueInsideDonut is empty/undefined, compute auto-sum
   * - Otherwise, return valueInsideDonut as-is
   */
  private _computeDisplayValue(totalValue: number): string | null {
    // Space forces empty (no text)
    if (this.valueInsideDonut === ' ') {
      return null;
    }

    const value = this.valueInsideDonut?.trim() ?? '';

    // Format string with {0} placeholder
    if (value.includes('{0}')) {
      // If formatter function is set, use it directly
      if (this.valueInsideFormatter) {
        return this.valueInsideFormatter(totalValue);
      }
      // Otherwise use locale formatting for the {0} replacement
      const formattedValue = formatLocaleNumber(totalValue, this.culture || undefined);
      return value.replace('{0}', formattedValue);
    }

    // Not set or explicitly empty → auto-sum
    if (this.valueInsideDonut == null || this.valueInsideDonut === '') {
      // If formatter function is set, use it
      if (this.valueInsideFormatter) {
        return this.valueInsideFormatter(totalValue);
      }
      // Always use locale formatting for auto-sum
      return formatLocaleNumber(totalValue, this.culture || undefined);
    }

    // Explicit value
    return value;
  }

  private _getTextInsideDonut(computedValue: string) {
    let textInsideDonut = computedValue;

    const highlighted = this._getHighlightedLegends();
    const singleHighlight =
      highlighted.length === 1 ? highlighted[0] : this.tooltipProps.isVisible ? this.tooltipProps.legend : null;

    if (computedValue && singleHighlight) {
      const highlightedDataPoint = this.data.find(dataPoint => dataPoint.legend === singleHighlight);
      if (highlightedDataPoint) {
        textInsideDonut = this._formatDataPointValue(highlightedDataPoint);
      }
    }

    return textInsideDonut;
  }

  private _updateTextInsideDonut() {
    if (!this._textInsideDonut) {
      return;
    }

    const totalValue = this.data?.reduce((sum, point) => sum + (point.data ?? 0), 0) ?? 0;
    const displayValue = this._computeDisplayValue(totalValue);

    if (displayValue === null) {
      this._textInsideDonut.textContent = '';
      return;
    }

    this._textInsideDonut.textContent = this._getTextInsideDonut(displayValue);
    const lineHeight = this._textInsideDonut.getBoundingClientRect().height;
    wrapText(this._textInsideDonut, 2 * this.innerRadius);
    const lines = this._textInsideDonut.getElementsByTagName('tspan');
    const start = -1 * Math.trunc((lines.length - 1) / 2);

    for (let i = 0; i < lines.length; i++) {
      lines[i].setAttribute('dy', `${(start + i) * lineHeight}`);
    }
  }
}
