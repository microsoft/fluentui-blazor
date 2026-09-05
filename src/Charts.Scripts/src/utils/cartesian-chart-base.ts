import { attr } from '@microsoft/fast-element';
import { renderChartAnnotations } from './chart-annotation-helpers.js';
import { resolveChartMargins, type CartesianChartMargins } from './cartesian-axis-helpers.js';
import type { AxisCategoryOrder, AxisScaleType, ChartAnnotation, ChartMargins } from './chart-options.js';
import { ChartBase } from './chart-base.js';
import { jsonConverter, SVG_NAMESPACE_URI } from './chart-helpers.js';

interface CartesianChartAnnotationRenderOptions {
  svg: SVGSVGElement;
  collisionLayer?: SVGGElement;
  margins: Pick<ChartMargins, 'left' | 'top'>;
  innerWidth: number;
  innerHeight: number;
  mapDataX: (value: number | string | Date) => number | undefined;
  mapDataY: (value: number | string | Date, axis: 'primary' | 'secondary') => number | undefined;
}

interface CartesianChartSvgOptions {
  role?: string;
}

interface CartesianChartRenderContextOptions extends CartesianChartSvgOptions {
  width: number;
  height: number;
  defaultMargins: CartesianChartMargins;
  hasSecondaryYAxis?: boolean;
}

interface CartesianChartRenderContext {
  svg: SVGSVGElement;
  plotGroup: SVGGElement;
  margins: CartesianChartMargins;
  innerWidth: number;
  innerHeight: number;
}

/**
 * Abstract base class for chart web components that use Cartesian axes (x/y).
 *
 * Extends {@link ChartBase} with axis-specific attributes for titles, tick
 * formatting, domain clamping, and label layout. Only Cartesian charts
 * (e.g. HorizontalBarChartWithAxis, GanttChart) should extend this class;
 * non-axis charts (DonutChart, FunnelChart, HorizontalBarChart) extend
 * {@link ChartBase} directly.
 *
 * @internal
 */
export abstract class CartesianChartBase extends ChartBase {
  // ── Axis-specific attrs ──────────────────────────────────────────

  /** Label rendered beneath the x-axis. */
  @attr({ attribute: 'x-axis-title' })
  public xAxisTitle?: string;

  /** Label rendered beside the y-axis. */
  @attr({ attribute: 'y-axis-title' })
  public yAxisTitle?: string;

  /** Label rendered beside the secondary y-axis when one is present. */
  @attr({ attribute: 'secondary-y-axis-title' })
  public secondaryYAxisTitle?: string;

  /** Plot margins in pixels. Missing sides use the chart defaults. */
  @attr({ converter: jsonConverter })
  public margins?: Partial<ChartMargins>;

  /** Text annotations rendered over the plot area. */
  @attr({ converter: jsonConverter })
  public annotations?: ChartAnnotation[];

  /** Scale type for a continuous numeric x-axis. */
  @attr({ attribute: 'x-scale-type' })
  public xScaleType: AxisScaleType = 'default';

  /** Scale type for the primary numeric y-axis. Vertical bars retain a linear zero baseline. */
  @attr({ attribute: 'y-scale-type' })
  public yScaleType: AxisScaleType = 'default';

  /** Scale type for a secondary numeric y-axis. */
  @attr({ attribute: 'secondary-y-scale-type' })
  public secondaryYScaleType: AxisScaleType = 'default';

  /**
   * A d3 format string (e.g. `'.2f'`, `'+,.0f'`) used to format x-axis
   * number tick labels. Has no effect on date-type axes.
   */
  @attr({ attribute: 'x-axis-tick-format' })
  public xAxisTickFormat?: string;

  /**
   * A d3 format string (e.g. `'.2f'`, `'+,.0f'`) used to format y-axis
   * number tick labels.
   */
  @attr({ attribute: 'y-axis-tick-format' })
  public yAxisTickFormat?: string;

  /** Gap in pixels between axis tick lines and their text labels. Defaults to 6. */
  @attr({ attribute: 'tick-padding' })
  public tickPadding?: number | string;

  /** Wraps long x-axis text labels onto multiple lines instead of truncating. */
  @attr({ attribute: 'wrap-x-axis-labels', mode: 'boolean' })
  public wrapXAxisLabels: boolean = false;

  /** Rotates x-axis text labels 45° to reduce overlap. */
  @attr({ attribute: 'rotate-x-axis-labels', mode: 'boolean' })
  public rotateXAxisLabels: boolean = false;

  /**
   * Allows the value axis to extend below zero when data contains negative
   * values. By default, the domain is clamped to a minimum of 0.
   */
  @attr({ attribute: 'support-negative-data', mode: 'boolean' })
  public supportNegativeData: boolean = false;

  /**
   * Rounds the auto-generated axis domain to "nice" values (calls d3's
   * `.nice()` equivalent on the tick scale).
   */
  @attr({ attribute: 'rounded-ticks', mode: 'boolean' })
  public roundedTicks: boolean = false;

  /** Minimum value for the value (x) axis domain. Overrides the data minimum. */
  @attr({ attribute: 'x-min-value' })
  public xMinValue?: number | string;

  /** Maximum value for the value (x) axis domain. Overrides the data maximum. */
  @attr({ attribute: 'x-max-value' })
  public xMaxValue?: number | string;

  /** Minimum value for the y axis domain (numeric y axis only). Overrides the data minimum. */
  @attr({ attribute: 'y-min-value' })
  public yMinValue?: number | string;

  /** Maximum value for the y axis domain (numeric y axis only). Overrides the data maximum. */
  @attr({ attribute: 'y-max-value' })
  public yMaxValue?: number | string;

  /**
   * Explicit tick positions for the x-axis. Overrides the auto-generated ticks.
   * Accepts an array of numbers, Date timestamps, or strings (parsed via JSON attribute).
   */
  @attr({ attribute: 'tick-values', converter: jsonConverter })
  public tickValues?: number[] | Date[] | string[];

  /**
   * A d3-time-format specifier string (e.g. `'%m/%d'`, `'%Y-%m'`) for date axis tick labels.
   * Only applicable when the x-axis uses a date/time scale.
   *
   * When set, overrides the `date-localize-options` / `culture`-based `Intl.DateTimeFormat` fallback.
   * Use `date-localize-options` together with `culture` to customise date formatting via `Intl.DateTimeFormat`
   * when locale-aware output is preferred over a fixed d3 specifier.
   */
  @attr({ attribute: 'tick-format' })
  public tickFormat?: string;

  /**
   * Explicit tick positions for the y-axis. Overrides auto-generated y-axis ticks.
   * Accepts an array of numbers (parsed via JSON attribute).
   */
  @attr({ attribute: 'y-axis-tick-values', converter: jsonConverter })
  public yAxisTickValues?: number[];

  /**
   * Preferred tick count for x-axis auto-generated ticks.
   * When not set, each chart keeps its existing default.
   */
  @attr({ attribute: 'x-axis-tick-count' })
  public xAxisTickCount?: number | string;

  /**
   * Preferred tick count for y-axis auto-generated ticks.
   * When not set, each chart keeps its existing default.
   */
  @attr({ attribute: 'y-axis-tick-count' })
  public yAxisTickCount?: number | string;

  /**
   * Optional order strategy for categorical x-axis domains.
   * Charts with non-categorical x-axes ignore this.
   */
  @attr({ attribute: 'x-axis-category-order' })
  public xAxisCategoryOrder: AxisCategoryOrder = 'default';

  /** Inner padding between categorical x-axis bands. Applies to categorical bar charts. */
  @attr({ attribute: 'x-axis-inner-padding' })
  public xAxisInnerPadding?: number | string;

  /** Outer padding around first and last categorical x-axis bands. Applies to categorical bar charts. */
  @attr({ attribute: 'x-axis-outer-padding' })
  public xAxisOuterPadding?: number | string;

  /** Width in pixels of the SVG stroke drawn on each bar. When set, an outline is applied. */
  @attr({ attribute: 'stroke-width' })
  public strokeWidth?: number | string;

  /** Width of the overlaid line stroke. */
  @attr({ attribute: 'line-stroke-width' })
  public lineStrokeWidth?: number | string;

  /** Dash pattern for the overlaid line stroke. */
  @attr({ attribute: 'line-stroke-dasharray' })
  public lineStrokeDasharray?: string | number;

  /** Dash offset for the overlaid line stroke. */
  @attr({ attribute: 'line-stroke-dashoffset' })
  public lineStrokeDashoffset?: string | number;

  /** Line cap style for the overlaid line stroke. */
  @attr({ attribute: 'line-stroke-linecap' })
  public lineStrokeLinecap?: 'butt' | 'round' | 'square' | 'inherit';

  /** Width of the border around the overlaid line. */
  @attr({ attribute: 'line-border-width' })
  public lineBorderWidth?: number | string;

  /** Color of the border around the overlaid line. */
  @attr({ attribute: 'line-border-color' })
  public lineBorderColor?: string;

  /**
   * When `true`, truncates long x-axis tick labels and shows the full text in a
   * `<title>` tooltip on hover.
   */
  @attr({ attribute: 'show-x-axis-labels-tooltip', mode: 'boolean' })
  public showXAxisLabelsTooltip: boolean = false;

  /**
   * Maximum number of characters shown in x-axis labels when
   * `show-x-axis-labels-tooltip` is enabled. Longer labels are truncated
   * and full text is available on hover.
   */
  @attr({ attribute: 'no-of-chars-to-truncate' })
  public noOfCharsToTruncate?: number | string;

  /**
   * When true (default), hides x-axis tick labels that would overlap with the previous label.
   * Set to false to always show all tick labels regardless of overlap.
   */
  @attr({ attribute: 'hide-tick-overlap', mode: 'boolean' })
  public hideTickOverlap: boolean = true;

  /**
   * Locale-aware date/time format options (`Intl.DateTimeFormatOptions`) passed to
   * `Intl.DateTimeFormat` when rendering date axis tick labels. Overrides the
   * auto-determined format options.
   */
  @attr({ attribute: 'date-localize-options', converter: jsonConverter })
  public dateLocalizeOptions?: Intl.DateTimeFormatOptions;

  /** When true, date axes display values in UTC instead of the user's local timezone. */
  @attr({ attribute: 'use-utc', mode: 'boolean' })
  public useUTC: boolean = false;

  /**
   * Optional custom formatter function for date-axis tick labels.
   * Receives a `Date` and returns a formatted string.
   * When set, takes precedence over `tickFormat` and `dateLocalizeOptions`.
   * Cannot be set via HTML attribute — assign directly on the element.
   */
  public customDateTimeFormatter?: (dateTime: Date) => string;

  /**
   * Optional custom formatter function for numeric y-axis tick labels.
   * Cannot be set via HTML attribute — assign directly on the element.
   */
  public customYAxisTickFormatter?: (value: number) => string;

  /** Creates the consistently configured SVG root used by Cartesian charts. */
  protected _createChartSvg(width: number, height: number, options: CartesianChartSvgOptions = {}): SVGSVGElement {
    const svg = document.createElementNS(SVG_NAMESPACE_URI, 'svg');
    svg.classList.add('chart-svg');
    svg.setAttribute('width', String(width));
    svg.setAttribute('height', String(height));
    svg.setAttribute('viewBox', `0 0 ${width} ${height}`);
    if (options.role) {
      svg.setAttribute('role', options.role);
    }
    return svg;
  }

  /** Creates the shared dimensions and plot layers used by standard Cartesian renderers. */
  protected _createCartesianRenderContext({
    width,
    height,
    defaultMargins,
    hasSecondaryYAxis = false,
    role,
  }: CartesianChartRenderContextOptions): CartesianChartRenderContext {
    const margins = resolveChartMargins(defaultMargins, this.margins, this._isRTL, hasSecondaryYAxis);
    const innerWidth = Math.max(width - margins.left - margins.right, 1);
    const innerHeight = Math.max(height - margins.top - margins.bottom, 1);
    const svg = this._createChartSvg(width, height, { role });
    const plotGroup = document.createElementNS(SVG_NAMESPACE_URI, 'g');
    plotGroup.setAttribute('transform', `translate(${margins.left}, ${margins.top})`);
    svg.appendChild(plotGroup);

    return { svg, plotGroup, margins, innerWidth, innerHeight };
  }

  /** Renders this chart's annotations in a final SVG layer above the plot and axes. */
  protected _renderAnnotations({
    svg,
    collisionLayer,
    margins,
    innerWidth,
    innerHeight,
    mapDataX,
    mapDataY,
  }: CartesianChartAnnotationRenderOptions): void {
    const annotationLayer = document.createElementNS(SVG_NAMESPACE_URI, 'g');
    annotationLayer.classList.add('annotation-layer');
    annotationLayer.setAttribute('transform', `translate(${margins.left}, ${margins.top})`);
    svg.appendChild(annotationLayer);
    renderChartAnnotations({
      layer: annotationLayer,
      collisionLayer,
      annotations: this.annotations,
      innerWidth,
      innerHeight,
      mapDataX,
      mapDataY,
    });
  }

  // ── Lifecycle ────────────────────────────────────────────────────

  connectedCallback() {
    // Delete own field shadows for all axis-specific attrs before calling
    // super.connectedCallback(), which handles the base attrs and FASTElement setup.
    const self = this as Record<string, unknown>;
    const attrFields = [
      'xAxisTitle',
      'yAxisTitle',
      'secondaryYAxisTitle',
      'margins',
      'annotations',
      'xScaleType',
      'yScaleType',
      'secondaryYScaleType',
      'xAxisTickFormat',
      'yAxisTickFormat',
      'tickPadding',
      'wrapXAxisLabels',
      'rotateXAxisLabels',
      'supportNegativeData',
      'roundedTicks',
      'xMinValue',
      'xMaxValue',
      'yMinValue',
      'yMaxValue',
      'tickValues',
      'tickFormat',
      'yAxisTickValues',
      'xAxisTickCount',
      'yAxisTickCount',
      'xAxisCategoryOrder',
      'xAxisInnerPadding',
      'xAxisOuterPadding',
      'strokeWidth',
      'lineStrokeWidth',
      'lineStrokeDasharray',
      'lineStrokeDashoffset',
      'lineStrokeLinecap',
      'lineBorderWidth',
      'lineBorderColor',
      'showXAxisLabelsTooltip',
      'noOfCharsToTruncate',
      'hideTickOverlap',
      'dateLocalizeOptions',
      'useUTC',
    ] as const;

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
  }

  // ── Attr change handlers ─────────────────────────────────────────

  protected xAxisTitleChanged() {
    this._requestRender();
  }

  protected yAxisTitleChanged() {
    this._requestRender();
  }

  protected secondaryYAxisTitleChanged() {
    this._requestRender();
  }

  protected marginsChanged() {
    this._requestRender();
  }

  protected annotationsChanged() {
    this._requestRender();
  }

  protected xScaleTypeChanged() {
    this._requestRender();
  }

  protected yScaleTypeChanged() {
    this._requestRender();
  }

  protected secondaryYScaleTypeChanged() {
    this._requestRender();
  }

  protected xAxisTickFormatChanged() {
    this._requestRender();
  }

  protected yAxisTickFormatChanged() {
    this._requestRender();
  }

  protected tickPaddingChanged() {
    this._requestRender();
  }

  protected wrapXAxisLabelsChanged() {
    this._requestRender();
  }

  protected rotateXAxisLabelsChanged() {
    this._requestRender();
  }

  protected supportNegativeDataChanged() {
    this._requestRender();
  }

  protected roundedTicksChanged() {
    this._requestRender();
  }

  protected xMinValueChanged() {
    this._requestRender();
  }

  protected xMaxValueChanged() {
    this._requestRender();
  }

  protected yMinValueChanged() {
    this._requestRender();
  }

  protected yMaxValueChanged() {
    this._requestRender();
  }

  protected tickValuesChanged() {
    this._requestRender();
  }

  protected tickFormatChanged() {
    this._requestRender();
  }

  protected yAxisTickValuesChanged() {
    this._requestRender();
  }

  protected xAxisTickCountChanged() {
    this._requestRender();
  }

  protected yAxisTickCountChanged() {
    this._requestRender();
  }

  protected xAxisCategoryOrderChanged() {
    this._requestRender();
  }

  protected xAxisInnerPaddingChanged() {
    this._requestRender();
  }

  protected xAxisOuterPaddingChanged() {
    this._requestRender();
  }

  protected strokeWidthChanged() {
    this._requestRender();
  }

  protected lineStrokeWidthChanged() {
    this._requestRender();
  }

  protected lineStrokeDasharrayChanged() {
    this._requestRender();
  }

  protected lineStrokeDashoffsetChanged() {
    this._requestRender();
  }

  protected lineStrokeLinecapChanged() {
    this._requestRender();
  }

  protected lineBorderWidthChanged() {
    this._requestRender();
  }

  protected lineBorderColorChanged() {
    this._requestRender();
  }

  protected showXAxisLabelsTooltipChanged() {
    this._requestRender();
  }

  protected noOfCharsToTruncateChanged() {
    this._requestRender();
  }

  protected hideTickOverlapChanged() {
    this._requestRender();
  }

  protected dateLocalizeOptionsChanged() {
    this._requestRender();
  }

  protected useUTCChanged() {
    this._requestRender();
  }
}
