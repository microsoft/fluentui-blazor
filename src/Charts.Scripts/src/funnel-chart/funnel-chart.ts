import { attr, nullableNumberConverter } from '@microsoft/fast-element';
import { ChartBase } from '../utils/chart-base.js';
import { getColorFromToken, getNextColor, jsonConverter, SVG_NAMESPACE_URI } from '../utils/chart-helpers.js';
import type { Legend } from '../utils/chart.options.js';
import type { FunnelDataPoint, FunnelSubValue } from './funnel-chart.options.js';
import {
  buildStackedGeometryParams,
  getContrastTextColor,
  getHorizontalFunnelSegmentGeometry,
  getSegmentTextProps,
  getStackedHorizontalFunnelSegmentGeometry,
  getStackedVerticalFunnelSegmentGeometry,
  getVerticalFunnelSegmentGeometry,
  isSimpleFunnelDataPoint,
  isStackedFunnelData,
} from './funnel-geometry.js';

/**
 * Normalizes orientation attribute values and defaults invalid values to `horizontal`.
 */
const orientationConverter = {
  fromView(value: string | null): 'vertical' | 'horizontal' {
    return value === 'vertical' ? 'vertical' : 'horizontal';
  },
  toView(value: 'vertical' | 'horizontal'): string {
    return value;
  },
};

/**
 * A Funnel Chart HTML Element.
 * Supports vertical and horizontal orientation, and stacked sub-values.
 *
 * @public
 */
export class FunnelChart extends ChartBase {
  @attr({ converter: nullableNumberConverter })
  public width: number = 400;

  @attr({ converter: nullableNumberConverter })
  public height: number = 400;

  @attr({ converter: jsonConverter })
  public data!: FunnelDataPoint[];

  @attr({ converter: orientationConverter })
  public orientation: 'vertical' | 'horizontal' = 'horizontal';

  public svgElement!: SVGSVGElement;
  public group!: SVGGElement;

  private _segments: SVGPathElement[] = [];
  private _segmentTexts: SVGTextElement[] = [];

  private readonly _handleMouseLeave = () => {
    this._clearTooltip();
  };

  connectedCallback() {
    const self = this as Record<string, unknown>;
    const attrFields = ['width', 'height', 'data', 'orientation'] as const;
    const saved: Partial<Record<(typeof attrFields)[number], unknown>> = {};

    for (const field of attrFields) {
      saved[field] = self[field];
      delete self[field];
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

  public disconnectedCallback() {
    this.removeEventListener('mouseleave', this._handleMouseLeave);
    super.disconnectedCallback();
  }

  protected dataChanged(_oldValue: FunnelDataPoint[], newValue: FunnelDataPoint[]) {
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

  protected orientationChanged() {
    this._requestRender();
  }

  protected _getHostAriaLabel(): string {
    return this.chartTitle || `Funnel chart with ${this.data?.length ?? 0} stages.`;
  }

  protected _performRender(): void {
    if (!this.$fastController.isConnected || !this.data || !this.group) {
      return;
    }
    this._clearChart();
    this._initializeAndRender();
  }

  private _clearChart() {
    while (this.group.firstChild) {
      this.group.removeChild(this.group.firstChild);
    }
    this._segments = [];
    this._segmentTexts = [];
  }

  private _initializeAndRender() {
    const data = this._resolveColors();
    this.legends = this._buildLegends(data);
    this.elementInternals.ariaLabel = this._getHostAriaLabel();

    if (this.svgElement) {
      this.svgElement.setAttribute('width', String(this.width));
      this.svgElement.setAttribute('height', String(this.height));
    }

    const verticalPadding = 16;
    const funnelWidth = this.width * 0.8;
    const funnelHeight = this.height - verticalPadding * 2;
    const funnelOffsetX = (this.width - funnelWidth) / 2;
    const funnelOffsetY = verticalPadding;

    this.group.setAttribute('transform', `translate(${funnelOffsetX}, ${funnelOffsetY})`);

    if (isStackedFunnelData(data)) {
      this._renderStackedFunnel(data, funnelWidth, funnelHeight);
    } else {
      this._renderSimpleFunnel(data, funnelWidth, funnelHeight);
    }

    this._applyActiveLegendState();
    this._applyLegendButtonState();
  }

  private _resolveColors(): FunnelDataPoint[] {
    return this.data.map((d, i) => {
      if (d.subValues && d.subValues.length > 0) {
        const resolvedSubValues = d.subValues.map((sv, k) => ({
          ...sv,
          color: sv.color ? getColorFromToken(sv.color) : getNextColor(k),
        }));
        return { ...d, subValues: resolvedSubValues };
      }
      const color = d.color ? getColorFromToken(d.color) : getNextColor(i);
      return { ...d, color };
    });
  }

  private _buildLegends(data: FunnelDataPoint[]): Legend[] {
    if (isStackedFunnelData(data)) {
      const seen = new Map<string, string>();
      data.forEach(stage => {
        (stage.subValues ?? []).forEach(sv => {
          if (!seen.has(sv.category)) {
            seen.set(sv.category, sv.color);
          }
        });
      });
      return Array.from(seen.entries()).map(([category, color]) => ({ legend: category, color }));
    }
    return data.filter(isSimpleFunnelDataPoint).map(d => ({ legend: d.stage, color: d.color! }));
  }

  private _renderSimpleFunnel(data: FunnelDataPoint[], funnelWidth: number, funnelHeight: number) {
    const simpleData = data.filter(isSimpleFunnelDataPoint);
    if (simpleData.length === 0) {
      return;
    }
    const maxValue = Math.max(...simpleData.map(point => point.value));
    if (maxValue <= 0) {
      return;
    }
    simpleData.forEach((d, i) => {
      const geom =
        this.orientation === 'vertical'
          ? getVerticalFunnelSegmentGeometry({
              d,
              i,
              data: simpleData,
              maxValue,
              funnelWidth,
              funnelHeight,
              isRTL: this._isRTL,
              roundCorners: this.roundCorners,
            })
          : getHorizontalFunnelSegmentGeometry({
              d,
              i,
              data: simpleData,
              maxValue,
              funnelWidth,
              funnelHeight,
              isRTL: this._isRTL,
              roundCorners: this.roundCorners,
            });

      const textProps = getSegmentTextProps({
        availableWidth: geom.availableWidth,
        textX: geom.textX,
        textY: geom.textY,
        value: d.value,
      });

      this._createSegment({
        key: `${i}`,
        pathD: geom.pathD,
        fill: d.color!,
        ariaLabel: `${d.stage}, ${d.value}.`,
        legendKey: d.stage,
        textProps,
        onHover: (event: MouseEvent) => this._showTooltip(d.stage, String(d.value), d.color!, event),
        onFocus: (path: SVGPathElement) => this._showTooltipForElement(d.stage, String(d.value), d.color!, path),
      });
    });
  }

  private _renderStackedFunnel(data: FunnelDataPoint[], funnelWidth: number, funnelHeight: number) {
    const { stages, totals, maxTotal } = buildStackedGeometryParams(data);

    data.forEach((stage, i) => {
      (stage.subValues ?? []).forEach((sv: FunnelSubValue, k) => {
        const geom =
          this.orientation === 'vertical'
            ? getStackedVerticalFunnelSegmentGeometry({
                i,
                k,
                stages,
                totals,
                maxTotal,
                funnelWidth,
                funnelHeight,
                roundCorners: this.roundCorners,
              })
            : getStackedHorizontalFunnelSegmentGeometry({
                i,
                k,
                stages,
                totals,
                maxTotal,
                funnelWidth,
                funnelHeight,
                roundCorners: this.roundCorners,
              });

        const textProps = getSegmentTextProps({
          availableWidth: geom.availableWidth,
          textX: geom.textX,
          textY: geom.textY,
          value: sv.value,
        });

        this._createSegment({
          key: `${i}-${k}`,
          pathD: geom.pathD,
          fill: sv.color,
          ariaLabel: `${stage.stage}, ${sv.category}, ${sv.value}.`,
          legendKey: sv.category,
          textProps,
          onHover: (event: MouseEvent) => this._showTooltip(sv.category, String(sv.value), sv.color, event),
          onFocus: (path: SVGPathElement) => this._showTooltipForElement(sv.category, String(sv.value), sv.color, path),
        });
      });
    });
  }

  private _createSegment({
    key,
    pathD,
    fill,
    ariaLabel,
    legendKey,
    textProps,
    onHover,
    onFocus,
  }: {
    key: string;
    pathD: string;
    fill: string;
    ariaLabel: string;
    legendKey: string;
    textProps: { show: boolean; x: number; y: number; value: number };
    onHover: (event: MouseEvent) => void;
    onFocus: (path: SVGPathElement) => void;
  }) {
    const segGroup = document.createElementNS(SVG_NAMESPACE_URI, 'g');
    this.group.appendChild(segGroup);

    const path = document.createElementNS(SVG_NAMESPACE_URI, 'path');
    segGroup.appendChild(path);
    path.classList.add('funnel-segment');
    path.setAttribute('d', pathD);
    path.setAttribute('fill', fill);
    path.setAttribute('data-id', legendKey);
    path.setAttribute('tabindex', this._segments.length === 0 ? '0' : '-1');
    path.setAttribute('role', 'img');
    path.setAttribute('aria-label', ariaLabel);
    this._segments.push(path);

    path.addEventListener('mouseover', event => {
      if (!this._shouldShowTooltip(legendKey)) {
        return;
      }
      onHover(event as MouseEvent);
    });

    path.addEventListener('mousemove', event => {
      if (!this._shouldShowTooltip(legendKey)) {
        return;
      }
      onHover(event as MouseEvent);
    });

    path.addEventListener('focus', () => {
      if (!this._shouldShowTooltip(legendKey)) {
        return;
      }
      onFocus(path);
    });

    path.addEventListener('blur', () => {
      this._clearTooltip();
    });

    path.addEventListener('keydown', (e: KeyboardEvent) => {
      if (e.key === 'Enter' || e.key === ' ') {
        e.preventDefault();
        path.dispatchEvent(new MouseEvent('click', { bubbles: true }));
      } else {
        this._rovingKeydown(this._segments, e);
      }
    });

    path.addEventListener('mouseout', () => {
      this._clearTooltip();
    });

    if (textProps.show) {
      const textEl = document.createElementNS(SVG_NAMESPACE_URI, 'text');
      segGroup.appendChild(textEl);
      textEl.classList.add('funnel-segment-text');
      textEl.setAttribute('x', String(textProps.x));
      textEl.setAttribute('y', String(textProps.y));
      textEl.setAttribute('text-anchor', 'middle');
      textEl.setAttribute('dominant-baseline', 'middle');
      textEl.setAttribute('fill', getContrastTextColor(fill));
      textEl.setAttribute('pointer-events', 'none');
      textEl.setAttribute('data-id', legendKey);
      textEl.textContent = textProps.value.toLocaleString(this.culture ?? undefined);
      this._segmentTexts.push(textEl);
    }
  }

  private _showTooltip(legend: string, yValue: string, color: string, event: MouseEvent) {
    const bounds = this.getBoundingClientRect();
    this.tooltipProps = {
      isVisible: true,
      legend,
      yValue,
      color,
      xPos: this._isRTL ? bounds.right - event.clientX : event.clientX - bounds.left,
      yPos: event.clientY - bounds.top - 85,
    };
  }

  private _showTooltipForElement(legend: string, yValue: string, color: string, el: SVGPathElement) {
    const rootBounds = this.getBoundingClientRect();
    const elBounds = el.getBoundingClientRect();
    this.tooltipProps = {
      isVisible: true,
      legend,
      yValue,
      color,
      xPos: this._isRTL
        ? rootBounds.right - elBounds.left - elBounds.width / 2
        : elBounds.left + elBounds.width / 2 - rootBounds.left,
      yPos: elBounds.top - rootBounds.top - 85,
    };
  }

  protected _applyActiveLegendState() {
    if (!this._segments || !this._segmentTexts) {
      return;
    }

    const highlighted = this._getHighlightedLegends();

    if (highlighted.length === 0) {
      this._segments.forEach(seg => {
        seg.classList.remove('inactive');
      });
      this._segmentTexts.forEach(t => t.classList.remove('inactive'));
      if (this._segments.length > 0 && !this._segments.some(el => el.tabIndex === 0)) {
        this._segments[0].tabIndex = 0;
      }
    } else {
      this._segments.forEach(seg => {
        const id = seg.getAttribute('data-id');
        const isActive = id !== null && highlighted.includes(id);
        seg.classList.toggle('inactive', !isActive);
        if (!isActive) {
          seg.tabIndex = -1;
        }
      });
      const activeSegs = this._segments.filter(el => !el.classList.contains('inactive'));
      if (activeSegs.length > 0 && !activeSegs.some(el => el.tabIndex === 0)) {
        activeSegs[0].tabIndex = 0;
      }
      this._segmentTexts.forEach(t => {
        const id = t.getAttribute('data-id');
        t.classList.toggle('inactive', id === null || !highlighted.includes(id));
      });
    }
  }
}
