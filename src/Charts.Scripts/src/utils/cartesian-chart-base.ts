import { attr } from '@microsoft/fast-element';
import {
  timeFormat as d3TimeFormat,
  timeFormatLocale as d3TimeFormatLocale,
  type TimeLocaleDefinition,
  utcFormat,
} from 'd3-time-format';
import { renderChartAnnotations } from './chart-annotation-helpers.js';
import { resolveChartMargins, type CartesianChartMargins } from './cartesian-axis-helpers.js';
import type {
  AxisCategoryOrder,
  AxisConfig,
  AxisScaleType,
  ChartAnnotation,
  ChartMargins,
  XAxisConfig,
} from './chart-options.js';
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
  ariaLabel?: string;
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

  /** Gap in pixels between x-axis tick lines and their text labels. Overrides `tick-padding` for the x-axis only. */
  @attr({ attribute: 'x-axis-tick-padding' })
  public xAxisTickPadding?: number | string;

  /** Length in pixels of x-axis tick lines. Falls back to the chart's existing default when unset. */
  @attr({ attribute: 'x-axis-tick-size' })
  public xAxisTickSize?: number | string;

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

  /** Minimum value for the secondary y axis domain (numeric secondary y axis only). Overrides the data minimum. */
  @attr({ attribute: 'secondary-y-min-value' })
  public secondaryYMinValue?: number | string;

  /** Maximum value for the secondary y axis domain (numeric secondary y axis only). Overrides the data maximum. */
  @attr({ attribute: 'secondary-y-max-value' })
  public secondaryYMaxValue?: number | string;

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

  /**
   * Optional order strategy for categorical y-axis domains (or string tick labels).
   * Charts with non-categorical y-axes ignore this.
   */
  @attr({ attribute: 'y-axis-category-order' })
  public yAxisCategoryOrder: AxisCategoryOrder = 'default';

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
   * When `true`, renders the full y-axis category label instead of truncating it,
   * and reserves enough y-axis width to fit the longest label.
   * Historically named `showYAxisLables` in the React implementation.
   */
  @attr({ attribute: 'show-y-axis-labels', mode: 'boolean' })
  public showYAxisLabels: boolean = false;

  /**
   * When `true`, truncates long y-axis category labels and shows the full text in a
   * `<title>` tooltip on hover.
   * Historically named `showYAxisLablesTooltip` in the React implementation.
   */
  @attr({ attribute: 'show-y-axis-labels-tooltip', mode: 'boolean' })
  public showYAxisLabelsTooltip: boolean = false;

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

  /** Locale definition used by d3-time-format when formatting date-axis ticks. */
  @attr({ attribute: 'time-format-locale', converter: jsonConverter })
  public timeFormatLocale?: TimeLocaleDefinition;

  /** When true, date axes display values in UTC instead of the user's local timezone. */
  @attr({ attribute: 'use-utc', mode: 'boolean' })
  public useUTC: boolean = false;

  /** Text annotation rendered above the plot area. */
  @attr({ attribute: 'x-axis-annotation' })
  public xAxisAnnotation?: string;

  /** Text annotation rendered along the right side when no secondary y-axis is present. */
  @attr({ attribute: 'y-axis-annotation' })
  public yAxisAnnotation?: string;

  /**
   * Shared x-axis configuration mirrored from React `xAxis` props.
   * Supports `tickStep`, `tick0`, and `tickText`.
   *
   * TODO: Support `tickLayout: 'auto'` behavior.
   */
  @attr({ attribute: 'x-axis-config', converter: jsonConverter })
  public xAxisConfig?: XAxisConfig;

  /** Shared y-axis configuration mirrored from React `yAxis` props. Supports `tickStep`, `tick0`, and `tickText`. */
  @attr({ attribute: 'y-axis-config', converter: jsonConverter })
  public yAxisConfig?: AxisConfig;

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
    if (options.ariaLabel) {
      svg.setAttribute('aria-label', options.ariaLabel);
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
    const annotationAwareDefaultMargins = {
      ...defaultMargins,
      top: defaultMargins.top + (this.xAxisAnnotation ? 20 : 0),
      right: defaultMargins.right + (!hasSecondaryYAxis && this.yAxisAnnotation ? 20 : 0),
    };
    const margins = resolveChartMargins(annotationAwareDefaultMargins, this.margins, this._isRTL, hasSecondaryYAxis);
    const innerWidth = Math.max(width - margins.left - margins.right, 1);
    const innerHeight = Math.max(height - margins.top - margins.bottom, 1);
    const svg = this._createChartSvg(width, height, { role, ariaLabel: role ? this._getHostAriaLabel() : undefined });
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
      'xAxisTickPadding',
      'xAxisTickSize',
      'wrapXAxisLabels',
      'rotateXAxisLabels',
      'supportNegativeData',
      'roundedTicks',
      'xMinValue',
      'xMaxValue',
      'yMinValue',
      'yMaxValue',
      'secondaryYMinValue',
      'secondaryYMaxValue',
      'tickValues',
      'tickFormat',
      'yAxisTickValues',
      'xAxisTickCount',
      'yAxisTickCount',
      'xAxisCategoryOrder',
      'yAxisCategoryOrder',
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
      'showYAxisLabels',
      'showYAxisLabelsTooltip',
      'hideTickOverlap',
      'dateLocalizeOptions',
      'timeFormatLocale',
      'useUTC',
      'xAxisAnnotation',
      'yAxisAnnotation',
      'xAxisConfig',
      'yAxisConfig',
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

  protected xAxisTickPaddingChanged() {
    this._requestRender();
  }

  protected xAxisTickSizeChanged() {
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

  protected secondaryYMinValueChanged() {
    this._requestRender();
  }

  protected secondaryYMaxValueChanged() {
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

  protected yAxisCategoryOrderChanged() {
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

  protected showYAxisLabelsChanged() {
    this._requestRender();
  }

  protected showYAxisLabelsTooltipChanged() {
    this._requestRender();
  }

  protected hideTickOverlapChanged() {
    this._requestRender();
  }

  protected dateLocalizeOptionsChanged() {
    this._requestRender();
  }

  protected timeFormatLocaleChanged() {
    this._requestRender();
  }

  protected useUTCChanged() {
    this._requestRender();
  }

  protected xAxisAnnotationChanged() {
    this._requestRender();
  }

  protected yAxisAnnotationChanged() {
    this._requestRender();
  }

  protected xAxisConfigChanged() {
    this._requestRender();
  }

  protected yAxisConfigChanged() {
    this._requestRender();
  }

  protected _getXAxisTickPadding(fallback: number): number {
    const value = this.xAxisTickPadding ?? this.tickPadding;
    if (value === undefined || value === null || value === '') {
      return fallback;
    }

    const parsed = Number(value);
    return Number.isFinite(parsed) ? parsed : fallback;
  }

  protected _getXAxisTickSize(fallback: number): number {
    if (this.xAxisTickSize === undefined || this.xAxisTickSize === null || this.xAxisTickSize === '') {
      return fallback;
    }

    const parsed = Number(this.xAxisTickSize);
    return Number.isFinite(parsed) ? parsed : fallback;
  }

  public _formatDateWithD3Specifier(date: Date, specifier: string): string {
    const locale = this.timeFormatLocale ? d3TimeFormatLocale(this.timeFormatLocale) : undefined;
    const formatter = locale
      ? this.useUTC
        ? locale.utcFormat(specifier)
        : locale.format(specifier)
      : this.useUTC
      ? utcFormat(specifier)
      : d3TimeFormat(specifier);
    return formatter(date);
  }
}
