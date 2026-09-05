import { attr, nullableNumberConverter, observable } from '@microsoft/fast-element';
import { arc as d3Arc } from 'd3-shape';
import { ChartBase } from '../utils/chart-base.js';
import {
  escapeHtml,
  getColorFromToken,
  getNextColor,
  jsonConverter,
  SVG_NAMESPACE_URI,
  wrapText,
} from '../utils/chart-helpers.js';
import type { Legend } from '../utils/chart-options.js';
import type { ExtendedSegment, GaugeChartSegment, GaugeChartVariant, GaugeValueFormat } from './gauge-chart.options.js';

// ── Layout constants (mirrors the React implementation) ───────────────────────

const GAUGE_MARGIN = 16;
const LABEL_WIDTH = 36;
const LABEL_OFFSET = 4;
const LABEL_HEIGHT = 16;
const EXTRA_NEEDLE_LENGTH = 4;
const ARC_PADDING = 2;

const BREAKPOINTS = [
  { minRadius: 52, arcWidth: 12, fontSize: 20 },
  { minRadius: 70, arcWidth: 16, fontSize: 24 },
  { minRadius: 88, arcWidth: 20, fontSize: 32 },
  { minRadius: 106, arcWidth: 24, fontSize: 32 },
  { minRadius: 124, arcWidth: 28, fontSize: 40 },
  { minRadius: 142, arcWidth: 32, fontSize: 40 },
];

const NEEDLE_TOOLTIP_DATA_POINT = 'needle';

// ── Tooltip shape specific to GaugeChart ─────────────────────────────────────

export interface GaugeTooltipRow {
  legend: string;
  value: string;
  color: string;
}

export interface GaugeTooltipProps {
  isVisible: boolean;
  /** Formatted current-value label shown as the tooltip header. */
  headerValue: string;
  /** One row per visible segment. */
  rows: GaugeTooltipRow[];
  xPos: number;
  yPos: number;
}

type TooltipAnchorMode = 'pointer' | 'element';

// ── Helper functions (mirrors React exports) ──────────────────────────────────

export function calcNeedleRotation(chartValue: number, minValue: number, maxValue: number): number {
  let needleRotation = ((chartValue - minValue) / (maxValue - minValue)) * 180;
  if (needleRotation < 0) {
    needleRotation = 0;
  } else if (needleRotation > 180) {
    needleRotation = 180;
  }
  return needleRotation;
}

export function getSegmentLabel(
  segment: ExtendedSegment,
  minValue: number,
  maxValue: number,
  variant?: GaugeChartVariant,
  isAriaLabel: boolean = false,
): string {
  if (isAriaLabel) {
    return minValue === 0 && variant === 'single-segment'
      ? `${segment.legend}, ${segment.size} out of ${maxValue} or ${((segment.size / maxValue) * 100).toFixed()}%`
      : `${segment.legend}, ${segment.start} to ${segment.end}`;
  }
  return minValue === 0 && variant === 'single-segment'
    ? `${segment.size} (${((segment.size / maxValue) * 100).toFixed()}%)`
    : `${segment.start} - ${segment.end}`;
}

export function getChartValueLabel(
  chartValue: number,
  minValue: number,
  maxValue: number,
  chartValueFormat?: GaugeValueFormat,
  forCallout: boolean = false,
  chartValueFormatFn?: (sweepFraction: [number, number]) => string,
  chartValueFormatTemplate?: string,
): string {
  // JS function takes highest precedence.
  if (chartValueFormatFn) {
    return chartValueFormatFn([chartValue, maxValue]);
  }
  // Template string (Blazor path) takes second precedence.
  if (chartValueFormatTemplate) {
    const percent = maxValue > minValue ? (((chartValue - minValue) / (maxValue - minValue)) * 100).toFixed() : '0';
    return chartValueFormatTemplate
      .replace('{value}', String(chartValue))
      .replace('{max}', String(maxValue))
      .replace('{min}', String(minValue))
      .replace('{percent}', percent);
  }
  if (forCallout) {
    return minValue !== 0
      ? chartValue.toString()
      : chartValueFormat === 'fraction'
      ? `${((chartValue / maxValue) * 100).toFixed()}%`
      : `${chartValue}/${maxValue}`;
  }
  return minValue !== 0
    ? chartValue.toString()
    : chartValueFormat === 'fraction'
    ? `${chartValue}/${maxValue}`
    : `${((chartValue / maxValue) * 100).toFixed()}%`;
}

// ── Component class ───────────────────────────────────────────────────────────

export class GaugeChart extends ChartBase {
  // ── Attrs ─────────────────────────────────────────────────────────────────

  @attr({ attribute: 'chart-value', converter: nullableNumberConverter })
  public chartValue: number = 0;

  @attr({ converter: jsonConverter })
  public segments!: GaugeChartSegment[];

  @attr({ attribute: 'min-value', converter: nullableNumberConverter })
  public minValue: number = 0;

  @attr({ attribute: 'max-value', converter: nullableNumberConverter })
  public maxValue?: number;

  @attr
  public sublabel?: string;

  @attr({ attribute: 'hide-min-max', mode: 'boolean' })
  public hideMinMax: boolean = false;

  @attr({ attribute: 'chart-value-format' })
  public chartValueFormat?: GaugeValueFormat;

  /**
   * Optional JS function for custom center-value label formatting.
   * Receives `[currentValue, maxValue]` and returns a string.
   * When set, takes precedence over `chartValueFormat` and `chartValueFormatTemplate`.
   * Cannot be set via HTML attribute — assign directly on the element.
   */
  public chartValueFormatFn?: (sweepFraction: [number, number]) => string;

  /**
   * Template string for the center-value label. Tokens replaced at render time:
   * `{value}` → current value, `{max}` → max value, `{min}` → min value, `{percent}` → percentage.
   * Used by Blazor (where JS functions cannot be passed as attributes).
   */
  @attr({ attribute: 'chart-value-format-template' })
  public chartValueFormatTemplate?: string;

  @attr
  public variant?: GaugeChartVariant;

  @attr({ attribute: 'enable-gradient', mode: 'boolean' })
  public enableGradient: boolean = false;

  // ── Gauge-specific observable ─────────────────────────────────────────────

  @observable
  public gaugeTooltipProps: GaugeTooltipProps = {
    isVisible: false,
    headerValue: '',
    rows: [],
    xPos: 0,
    yPos: 0,
  };

  // ── Template refs ─────────────────────────────────────────────────────────

  public group!: SVGGElement;
  public svgDefsEl!: SVGDefsElement;

  // ── Private render state ──────────────────────────────────────────────────
  protected override _enableResizeObserver = true;
  private _segmentEls: SVGPathElement[] = [];
  private _needle?: SVGPathElement;
  private _computedMinValue: number = 0;
  private _computedMaxValue: number = 0;
  private _processedSegments: ExtendedSegment[] = [];

  private readonly _handleMouseLeave = () => {
    this._clearGaugeTooltip();
  };

  // ── Lifecycle ─────────────────────────────────────────────────────────────

  connectedCallback() {
    const self = this as Record<string, unknown>;
    const attrFields = [
      'chartValue',
      'segments',
      'minValue',
      'maxValue',
      'sublabel',
      'hideMinMax',
      'chartValueFormat',
      'chartValueFormatTemplate',
      'variant',
      'enableGradient',
    ] as const;

    const saved: Partial<Record<(typeof attrFields)[number], unknown>> = {};
    for (const field of attrFields) {
      saved[field] = self[field];
      delete self[field];
    }

    const gaugeObsFields = ['gaugeTooltipProps'] as const;
    for (const field of gaugeObsFields) {
      const v = self[field];
      delete self[field];
      if (v !== undefined) {
        self[field] = v;
      }
    }

    super.connectedCallback();

    for (const field of attrFields) {
      if (self[field] === undefined && saved[field] !== undefined) {
        self[field] = saved[field];
      }
    }

    this.addEventListener('mouseleave', this._handleMouseLeave);
    this._requestRender();
  }

  disconnectedCallback() {
    this.removeEventListener('mouseleave', this._handleMouseLeave);
    super.disconnectedCallback();
  }

  // ── Attr change handlers ──────────────────────────────────────────────────

  protected chartValueChanged() {
    this._requestRender();
  }

  protected segmentsChanged() {
    this._requestRender();
  }

  protected minValueChanged() {
    this._requestRender();
  }

  protected maxValueChanged() {
    this._requestRender();
  }

  protected sublabelChanged() {
    this._requestRender();
  }

  protected hideMinMaxChanged() {
    this._requestRender();
  }

  protected chartValueFormatChanged() {
    this._requestRender();
  }

  protected chartValueFormatTemplateChanged() {
    this._requestRender();
  }

  protected variantChanged() {
    this._requestRender();
  }

  protected enableGradientChanged() {
    this._requestRender();
  }

  // ── Abstract implementation ───────────────────────────────────────────────

  protected _getHostAriaLabel(): string {
    const segCount = this._processedSegments.length;
    return this.chartTitle
      ? `${this.chartTitle}, gauge chart with ${segCount} segment${segCount !== 1 ? 's' : ''}.`
      : `Gauge chart with ${segCount} segment${segCount !== 1 ? 's' : ''}.`;
  }

  protected _performRender() {
    if (!this.$fastController.isConnected || !this.segments || !this.group) {
      return;
    }

    this._applyHostDimensions();
    this._clearChart();
    this._initializeAndRender();
  }

  protected _applyHostDimensions() {
    super._applyHostDimensions(this.width, this.height);
  }

  protected override _buildDefaultTooltipHTML(): string {
    const header = `<div class="tooltip-header">${escapeHtml(this.gaugeTooltipProps.headerValue ?? '')}</div>`;
    const rows = (this.gaugeTooltipProps.rows ?? [])
      .map(row =>
        [
          `<div class="tooltip-inner" style="border-color: ${escapeHtml(row.color)};">`,
          `<div class="tooltip-legend-text">${escapeHtml(row.legend)}</div>`,
          `<div class="tooltip-content-y" style="color: ${escapeHtml(row.color)};">${escapeHtml(row.value)}</div>`,
          `</div>`,
        ].join(''),
      )
      .join('');

    return header + rows;
  }

  protected gaugeTooltipPropsChanged(_oldValue: GaugeTooltipProps, newValue: GaugeTooltipProps): void {
    if (!newValue) {
      this.tooltipProps = { isVisible: false, legend: '', yValue: '', color: '', xPos: 0, yPos: 0 };
      return;
    }

    const rows = Array.isArray(newValue.rows) ? newValue.rows : [];
    const firstRow = rows[0];

    this.tooltipProps = {
      isVisible: Boolean(newValue.isVisible),
      legend: newValue.headerValue,
      yValue: firstRow?.value ?? '',
      color: firstRow?.color ?? '',
      xPos: newValue.xPos ?? 0,
      yPos: newValue.yPos ?? 0,
    };
  }

  protected _applyActiveLegendState() {
    if (!this._segmentEls) {
      return;
    }

    const highlighted = this._getHighlightedLegends();

    if (highlighted.length === 0) {
      this._segmentEls.forEach(el => {
        el.classList.remove('inactive');
        el.style.removeProperty('opacity');
      });
      if (this._segmentEls.length > 0 && !this._segmentEls.some(el => el.tabIndex === 0)) {
        this._segmentEls[0].tabIndex = 0;
      }
    } else {
      this._segmentEls.forEach(el => {
        const legendId = el.getAttribute('data-id');
        const isActive = legendId !== null && highlighted.includes(legendId);
        el.classList.toggle('inactive', !isActive);
        el.style.opacity = isActive ? '1' : '0.1';
        if (!isActive) {
          el.tabIndex = -1;
        }
      });

      const activeEls = this._segmentEls.filter(el => !el.classList.contains('inactive'));
      if (activeEls.length > 0 && !activeEls.some(el => el.tabIndex === 0)) {
        activeEls[0].tabIndex = 0;
      }

      this._relocateFocusIfNeeded(this._segmentEls);
    }
  }

  // ── Private render helpers ────────────────────────────────────────────────

  private _clearChart() {
    if (this.group) {
      while (this.group.firstChild) {
        this.group.removeChild(this.group.firstChild);
      }
    }
    if (this.svgDefsEl) {
      while (this.svgDefsEl.firstChild) {
        this.svgDefsEl.removeChild(this.svgDefsEl.firstChild);
      }
    }
    this._segmentEls = [];
    this._needle = undefined;
  }

  private _initializeAndRender() {
    const geo = this._calculateGeometry();

    // Position the group at the center-bottom of the gauge half-circle.
    this.group.setAttribute('transform', `translate(${geo.cx}, ${geo.cy})`);

    const processedData = this._processSegments(geo.outerRadius, geo.innerRadius);
    this._computedMinValue = processedData.minValue;
    this._computedMaxValue = processedData.maxValue;
    this._processedSegments = processedData.segments;

    this._renderArcs(processedData);
    this._renderNeedle(geo.outerRadius, geo.innerRadius);
    this._renderValueText(geo.innerRadius, geo.chartValueSize);

    if (!this.hideMinMax) {
      this._renderMinMaxLabels(geo.outerRadius);
    }
    if (this.sublabel) {
      this._renderSublabel(geo.innerRadius);
    }

    // Update legends (omit the trailing "Unknown" fill segment from legend list)
    const legendSegments = processedData.segments.filter(s => s.legend !== 'Unknown');
    this.legends = this._buildLegends(legendSegments);
    this.elementInternals.ariaLabel = this._getHostAriaLabel();

    this._applyActiveLegendState();
    this._applyLegendButtonState();
  }

  private _calculateGeometry() {
    const svgEl = this.group.ownerSVGElement!;
    const svgRect = svgEl.getBoundingClientRect();
    const w = svgRect.width || parseFloat(String(this.width)) || 252;
    const defaultHeight = this.sublabel ? 116 : 96;
    const h = svgRect.height || parseFloat(String(this.height)) || defaultHeight;

    const marginLeft = (!this.hideMinMax ? LABEL_OFFSET + LABEL_WIDTH : 0) + GAUGE_MARGIN;
    const marginRight = (!this.hideMinMax ? LABEL_OFFSET + LABEL_WIDTH : 0) + GAUGE_MARGIN;
    // The chart title is rendered as an HTML element outside the SVG (not as SVG text inside),
    // so no extra top margin is needed for it. Only reserve space for the needle overshoot.
    const marginTop = EXTRA_NEEDLE_LENGTH / 2 + GAUGE_MARGIN;
    const marginBottom = (this.sublabel ? LABEL_OFFSET + LABEL_HEIGHT : 0) + GAUGE_MARGIN;

    const cx = w / 2;
    const cy = h - marginBottom;

    const outerRadius = Math.max(0, Math.min((w - marginLeft - marginRight) / 2, h - marginTop - marginBottom));

    let arcWidth = BREAKPOINTS[0].arcWidth;
    let chartValueSize = BREAKPOINTS[0].fontSize;
    for (let i = BREAKPOINTS.length - 1; i >= 0; i--) {
      if (outerRadius >= BREAKPOINTS[i].minRadius) {
        arcWidth = BREAKPOINTS[i].arcWidth;
        chartValueSize = BREAKPOINTS[i].fontSize;
        break;
      }
    }

    const innerRadius = outerRadius - arcWidth;

    return { cx, cy, outerRadius, innerRadius, chartValueSize };
  }

  private _processSegments(outerRadius: number, innerRadius: number) {
    const minValue = this.minValue ?? 0;
    let total = minValue;

    const segments: ExtendedSegment[] = (this.segments || []).map((seg, index) => {
      const size = Math.max(seg.size, 0);
      total += size;
      return {
        ...seg,
        color: seg.color ? getColorFromToken(seg.color) : getNextColor(index),
        start: total - size,
        end: total,
      };
    });

    let maxValue = total;
    if (this.maxValue !== undefined && this.maxValue !== null && total < this.maxValue) {
      segments.push({
        legend: 'Unknown',
        size: this.maxValue - total,
        color: getColorFromToken('semantic.disabled'),
        start: total,
        end: this.maxValue,
      });
      maxValue = this.maxValue;
    }

    // Build SVG arcs
    const cornerRadius = this.roundCorners ? 3 : 0;
    const arcGenerator = d3Arc()
      .cornerRadius(cornerRadius)
      .padAngle(ARC_PADDING / (outerRadius || 1))
      .padRadius(outerRadius);

    const rtlSegments = this._isRTL ? [...segments].reverse() : segments;
    let prevAngle = -Math.PI / 2;

    const arcs = rtlSegments.map((seg, i) => {
      const endAngle = prevAngle + (seg.size / (maxValue - minValue)) * Math.PI;
      const d = arcGenerator({
        innerRadius,
        outerRadius,
        startAngle: prevAngle,
        endAngle,
      })!;
      prevAngle = endAngle;
      return {
        d,
        segmentIndex: this._isRTL ? segments.length - 1 - i : i,
      };
    });

    return { arcs, segments, minValue, maxValue };
  }

  private _renderArcs(data: ReturnType<typeof this._processSegments>) {
    const { arcs, segments, minValue, maxValue } = data;

    // If gradients are requested, set up SVG defs
    if (this.enableGradient) {
      this._setupGradients(segments);
    }

    arcs.forEach((arc, _i) => {
      const segment = segments[arc.segmentIndex];
      const arcId = `gauge-arc-${arc.segmentIndex}`;

      // Focus outline element (rendered behind the fill)
      const outline = document.createElementNS(SVG_NAMESPACE_URI, 'path');
      outline.setAttribute('d', arc.d);
      outline.classList.add('segment-outline');
      this.group.appendChild(outline);

      const path = document.createElementNS(SVG_NAMESPACE_URI, 'path');
      this.group.appendChild(path);
      this._segmentEls.push(path);

      const gradientId = `gauge-gradient-${arc.segmentIndex}`;
      let fill: string;
      if (this.enableGradient && segment.gradient) {
        fill = `url(#${gradientId})`;
      } else if (this.enableGradient) {
        fill = this._buildLinearGradient(gradientId, segment.color!, segment.color!);
      } else {
        fill = segment.color!;
      }

      path.setAttribute('d', arc.d);
      path.setAttribute('fill', fill);
      path.setAttribute('id', arcId);
      path.setAttribute('data-id', segment.legend);
      path.setAttribute('role', 'img');
      path.setAttribute(
        'aria-label',
        segment.ariaLabel ?? getSegmentLabel(segment, minValue, maxValue, this.variant, true),
      );
      path.setAttribute('tabindex', this._segmentEls.length === 1 ? '0' : '-1');
      path.classList.add('segment');

      path.addEventListener('mouseover', e => {
        this._showTooltip(e, segment);
      });
      path.addEventListener('mousemove', e => {
        this._showTooltip(e, segment);
      });
      path.addEventListener('mouseleave', () => {
        this._clearGaugeTooltip();
      });
      path.addEventListener('focus', () => {
        this._showTooltipForElement(path, segment);
      });
      path.addEventListener('blur', () => {
        this._clearGaugeTooltip();
      });
      path.addEventListener('click', () => this._focusRovingElement(this._getRovingElements(), path));
      path.addEventListener('keydown', (e: KeyboardEvent) => {
        if (e.key === 'Enter' || e.key === ' ') {
          e.preventDefault();
          path.dispatchEvent(new MouseEvent('click', { bubbles: true }));
        } else {
          this._rovingKeydown(this._getRovingElements(), e);
        }
      });
    });
  }

  private _setupGradients(segments: ExtendedSegment[]) {
    if (!this.svgDefsEl) return;
    segments.forEach((seg, index) => {
      if (!seg.gradient) return;
      const gradId = `gauge-gradient-${index}`;
      const existing = this.svgDefsEl.querySelector(`#${gradId}`);
      if (existing) return;
      const lg = document.createElementNS(SVG_NAMESPACE_URI, 'linearGradient');
      lg.setAttribute('id', gradId);
      lg.setAttribute('x1', '0');
      lg.setAttribute('y1', '0');
      lg.setAttribute('x2', '1');
      lg.setAttribute('y2', '0');
      const stop1 = document.createElementNS(SVG_NAMESPACE_URI, 'stop');
      stop1.setAttribute('offset', '0%');
      stop1.setAttribute('stop-color', getColorFromToken(seg.gradient[0]));
      const stop2 = document.createElementNS(SVG_NAMESPACE_URI, 'stop');
      stop2.setAttribute('offset', '100%');
      stop2.setAttribute('stop-color', getColorFromToken(seg.gradient[1]));
      lg.appendChild(stop1);
      lg.appendChild(stop2);
      this.svgDefsEl.appendChild(lg);
    });
  }

  private _buildLinearGradient(id: string, color1: string, color2: string): string {
    if (!this.svgDefsEl) return color1;
    const lg = document.createElementNS(SVG_NAMESPACE_URI, 'linearGradient');
    lg.setAttribute('id', id);
    const stop1 = document.createElementNS(SVG_NAMESPACE_URI, 'stop');
    stop1.setAttribute('offset', '0%');
    stop1.setAttribute('stop-color', color1);
    const stop2 = document.createElementNS(SVG_NAMESPACE_URI, 'stop');
    stop2.setAttribute('offset', '100%');
    stop2.setAttribute('stop-color', color2);
    lg.appendChild(stop1);
    lg.appendChild(stop2);
    this.svgDefsEl.appendChild(lg);
    return `url(#${id})`;
  }

  private _renderNeedle(outerRadius: number, innerRadius: number) {
    const strokeWidth = 2;
    const half = strokeWidth / 2;
    const needleLength = outerRadius - innerRadius + EXTRA_NEEDLE_LENGTH;

    const needleRotation = calcNeedleRotation(this.chartValue, this._computedMinValue, this._computedMaxValue);
    const rtlRotation = this._isRTL ? 180 - needleRotation : needleRotation;

    const gEl = document.createElementNS(SVG_NAMESPACE_URI, 'g');
    gEl.setAttribute('transform', `rotate(${rtlRotation}, 0, 0)`);
    this.group.appendChild(gEl);

    const path = document.createElementNS(SVG_NAMESPACE_URI, 'path');
    const tx = -innerRadius + EXTRA_NEEDLE_LENGTH / 2;
    path.setAttribute(
      'd',
      `M 0,${-half - 3}` +
        ` L ${-needleLength},${-half - 1}` +
        ` A ${half + 1},${half + 1},0,0,0,${-needleLength},${half + 1}` +
        ` L 0,${half + 3}` +
        ` A ${half + 3},${half + 3},0,0,0,0,${-half - 3}`,
    );
    path.setAttribute('transform', `translate(${tx})`);
    path.setAttribute('role', 'img');
    path.setAttribute(
      'aria-label',
      `Current value: ${getChartValueLabel(
        this.chartValue,
        this._computedMinValue,
        this._computedMaxValue,
        this.chartValueFormat,
        false,
        this.chartValueFormatFn,
        this.chartValueFormatTemplate,
      )}`,
    );
    path.setAttribute('tabindex', this._segmentEls.length === 0 ? '0' : '-1');
    path.classList.add('needle');
    gEl.appendChild(path);
    this._needle = path;

    path.addEventListener('mouseover', e => {
      this._showNeedleTooltip(e);
    });
    path.addEventListener('mousemove', e => {
      this._showNeedleTooltip(e);
    });
    path.addEventListener('mouseleave', () => {
      this._clearGaugeTooltip();
    });
    path.addEventListener('focus', () => {
      this._showNeedleTooltipForElement(path);
    });
    path.addEventListener('blur', () => {
      this._clearGaugeTooltip();
    });
    path.addEventListener('click', () => this._focusRovingElement(this._getRovingElements(), path));
    path.addEventListener('keydown', (e: KeyboardEvent) => {
      if (e.key !== 'Enter' && e.key !== ' ') {
        this._rovingKeydown(this._getRovingElements(), e);
      }
    });
  }

  private _getRovingElements(): SVGPathElement[] {
    return [...this._segmentEls, ...(this._needle ? [this._needle] : [])];
  }

  private _renderValueText(innerRadius: number, fontSize: number) {
    const text = document.createElementNS(SVG_NAMESPACE_URI, 'text');
    text.classList.add('chart-value');
    text.setAttribute('x', '0');
    text.setAttribute('y', '0');
    text.setAttribute('text-anchor', 'middle');
    text.setAttribute('dominant-baseline', 'auto');
    text.setAttribute('aria-hidden', 'true');
    text.setAttribute('font-size', `${fontSize}`);
    text.textContent = getChartValueLabel(
      this.chartValue,
      this._computedMinValue,
      this._computedMaxValue,
      this.chartValueFormat,
      false,
      this.chartValueFormatFn,
      this.chartValueFormatTemplate,
    );
    this.group.appendChild(text);

    // Wrap/truncate to fit inside the inner radius
    const maxWidth = Math.max(0, innerRadius * 2 - 24);
    wrapText(text, maxWidth);
  }

  private _renderSublabel(innerRadius: number) {
    if (!this.sublabel) return;
    const text = document.createElementNS(SVG_NAMESPACE_URI, 'text');
    text.classList.add('sublabel');
    text.setAttribute('x', '0');
    text.setAttribute('y', '4');
    text.setAttribute('text-anchor', 'middle');
    text.setAttribute('dominant-baseline', 'hanging');
    text.setAttribute('aria-hidden', 'true');
    text.textContent = this.sublabel;
    this.group.appendChild(text);

    const maxWidth = Math.max(0, innerRadius * 2);
    wrapText(text, maxWidth);
  }

  private _renderMinMaxLabels(outerRadius: number) {
    const minLabel = document.createElementNS(SVG_NAMESPACE_URI, 'text');
    minLabel.classList.add('limit-label');
    const minX = (this._isRTL ? 1 : -1) * (outerRadius + LABEL_OFFSET);
    minLabel.setAttribute('x', `${minX}`);
    minLabel.setAttribute('y', '0');
    minLabel.setAttribute('text-anchor', 'end');
    minLabel.setAttribute('role', 'img');
    minLabel.setAttribute('aria-label', `Min value: ${this._computedMinValue}`);
    minLabel.textContent = this._formatScientific(this._computedMinValue);
    this.group.appendChild(minLabel);

    const maxLabel = document.createElementNS(SVG_NAMESPACE_URI, 'text');
    maxLabel.classList.add('limit-label');
    const maxX = (this._isRTL ? -1 : 1) * (outerRadius + LABEL_OFFSET);
    maxLabel.setAttribute('x', `${maxX}`);
    maxLabel.setAttribute('y', '0');
    maxLabel.setAttribute('text-anchor', 'start');
    maxLabel.setAttribute('role', 'img');
    maxLabel.setAttribute('aria-label', `Max value: ${this._computedMaxValue}`);
    maxLabel.textContent = this._formatScientific(this._computedMaxValue);
    this.group.appendChild(maxLabel);
  }

  private _formatScientific(value: number): string {
    const abs = Math.abs(value);
    if (abs === 0) return '0';
    if (abs >= 1e3 && abs < 1e6) return `${(value / 1e3).toFixed(1)}k`;
    if (abs >= 1e6 && abs < 1e9) return `${(value / 1e6).toFixed(1)}M`;
    if (abs >= 1e9) return `${(value / 1e9).toFixed(1)}B`;
    return value.toString();
  }

  private _buildLegends(segments: ExtendedSegment[]): Legend[] {
    return segments.map(seg => ({ legend: seg.legend, color: seg.color! }));
  }

  // ── Tooltip helpers ───────────────────────────────────────────────────────

  private _buildTooltipRows(singleSegment?: ExtendedSegment): GaugeTooltipRow[] {
    const highlighted = this._getHighlightedLegends();
    return this._processedSegments
      .filter(
        seg =>
          seg.legend !== 'Unknown' &&
          (highlighted.length === 0 || highlighted.includes(seg.legend)) &&
          (!singleSegment || seg.legend === singleSegment.legend),
      )
      .map(seg => ({
        legend: seg.legend,
        value: getSegmentLabel(seg, this._computedMinValue, this._computedMaxValue, this.variant),
        color: seg.color!,
      }));
  }

  private _buildHeaderValue(): string {
    return (
      'Current value: ' +
      getChartValueLabel(
        this.chartValue,
        this._computedMinValue,
        this._computedMaxValue,
        this.chartValueFormat,
        true,
        this.chartValueFormatFn,
        this.chartValueFormatTemplate,
      )
    );
  }

  private _estimateGaugeTooltipSize(rowCount: number): { estimatedWidth: number; estimatedHeight: number } {
    const clampedRows = Math.max(1, rowCount);
    // Gauge tooltips are compact in width but tall due header + stacked rows.
    const estimatedWidth = 168;
    const estimatedHeight = 84 + clampedRows * 36;
    return { estimatedWidth, estimatedHeight };
  }

  private _resolveGaugeTooltipPosition(
    anchorX: number,
    anchorY: number,
    rowCount: number,
    mode: TooltipAnchorMode,
  ): { xPos: number; yPos: number } {
    const hostWidth = Math.max(this.offsetWidth || 0, 1);
    const hostHeight = Math.max(this.offsetHeight || 0, 1);
    const gap = 12;
    const { estimatedWidth, estimatedHeight } = this._estimateGaugeTooltipSize(rowCount);

    if (mode === 'pointer') {
      const left = Math.max(10, Math.min(anchorX + gap, hostWidth - estimatedWidth - 10));
      const top = Math.max(10, Math.min(anchorY + gap, hostHeight - estimatedHeight - 10));
      return { xPos: left, yPos: top };
    }

    return this._resolveTooltipPositionFromAnchor(anchorX, anchorY, {
      preferredVertical: 'above',
      horizontalAlign: 'center',
      estimatedWidth,
      estimatedHeight,
      gap,
      padding: 10,
    });
  }

  private _showTooltip(e: MouseEvent, segment: ExtendedSegment) {
    if (this.hideTooltip) return;
    this._currentTooltipDataPoint = segment;
    const bounds = this.getBoundingClientRect();
    const anchorX = e.clientX - bounds.left;
    const anchorY = e.clientY - bounds.top;
    const rows = this._buildTooltipRows(undefined);
    const activeRows = rows.length > 0 ? rows : this._buildTooltipRows(segment);
    const { xPos, yPos } = this._resolveGaugeTooltipPosition(anchorX, anchorY, activeRows.length, 'pointer');

    this.gaugeTooltipProps = {
      isVisible: true,
      headerValue: this._buildHeaderValue(),
      rows: activeRows,
      xPos,
      yPos,
    };

    this.liveRegionText = [this._buildHeaderValue(), ...rows.map(r => `${r.legend}: ${r.value}`)].join(', ');
  }

  private _showTooltipForElement(el: SVGPathElement, segment: ExtendedSegment) {
    if (this.hideTooltip) return;
    this._currentTooltipDataPoint = segment;
    const rootBounds = this.getBoundingClientRect();
    const elBounds = el.getBoundingClientRect();
    const gap = 12;
    const anchorX = this._isRTL ? elBounds.left - rootBounds.left - gap : elBounds.right - rootBounds.left + gap;
    const anchorY = elBounds.top + elBounds.height / 2 - rootBounds.top;
    const rows = this._buildTooltipRows(undefined);
    const activeRows = rows.length > 0 ? rows : this._buildTooltipRows(segment);
    const { estimatedWidth, estimatedHeight } = this._estimateGaugeTooltipSize(activeRows.length);
    const { xPos, yPos } = this._resolveTooltipPositionFromAnchor(anchorX, anchorY, {
      preferredVertical: 'above',
      horizontalAlign: this._isRTL ? 'end' : 'start',
      estimatedWidth,
      estimatedHeight,
      gap,
      padding: 10,
    });

    this.gaugeTooltipProps = {
      isVisible: true,
      headerValue: this._buildHeaderValue(),
      rows: activeRows,
      xPos,
      yPos,
    };

    this.liveRegionText = [this._buildHeaderValue(), ...rows.map(r => `${r.legend}: ${r.value}`)].join(', ');
  }

  private _showNeedleTooltip(e: MouseEvent) {
    if (this.hideTooltip) return;
    this._currentTooltipDataPoint = NEEDLE_TOOLTIP_DATA_POINT;
    const bounds = this.getBoundingClientRect();
    const anchorX = e.clientX - bounds.left;
    const anchorY = e.clientY - bounds.top;
    const rows = this._buildTooltipRows(undefined);
    const { xPos, yPos } = this._resolveGaugeTooltipPosition(anchorX, anchorY, rows.length, 'pointer');

    this.gaugeTooltipProps = {
      isVisible: true,
      headerValue: this._buildHeaderValue(),
      rows,
      xPos,
      yPos,
    };

    this.liveRegionText = [this._buildHeaderValue(), ...rows.map(r => `${r.legend}: ${r.value}`)].join(', ');
  }

  private _showNeedleTooltipForElement(el: SVGPathElement) {
    if (this.hideTooltip) return;
    this._currentTooltipDataPoint = NEEDLE_TOOLTIP_DATA_POINT;
    const rootBounds = this.getBoundingClientRect();
    const elBounds = el.getBoundingClientRect();
    const anchorX = elBounds.left + elBounds.width / 2 - rootBounds.left;
    const anchorY = elBounds.top - rootBounds.top;
    const rows = this._buildTooltipRows(undefined);
    const { xPos, yPos } = this._resolveGaugeTooltipPosition(anchorX, anchorY, rows.length, 'element');

    this.gaugeTooltipProps = {
      isVisible: true,
      headerValue: this._buildHeaderValue(),
      rows,
      xPos,
      yPos,
    };

    this.liveRegionText = [this._buildHeaderValue(), ...rows.map(r => `${r.legend}: ${r.value}`)].join(', ');
  }

  private _clearGaugeTooltip() {
    this._currentTooltipDataPoint = null;
    this.gaugeTooltipProps = { isVisible: false, headerValue: '', rows: [], xPos: 0, yPos: 0 };
    this.liveRegionText = '';
  }
}
