import { attr } from '@microsoft/fast-element';
import type { AxisCategoryOrder } from './chart-options.js';
import { ChartBase } from './chart-base.js';
import { jsonConverter } from './chart-helpers.js';

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

  /** Width in pixels of the SVG stroke drawn on each bar. When set, an outline is applied. */
  @attr({ attribute: 'stroke-width' })
  public strokeWidth?: number | string;

  /**
   * When `true`, truncates long x-axis tick labels and shows the full text in a
   * `<title>` tooltip on hover.
   */
  @attr({ attribute: 'show-x-axis-labels-tooltip', mode: 'boolean' })
  public showXAxisLabelsTooltip: boolean = false;

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

  // ── Lifecycle ────────────────────────────────────────────────────

  connectedCallback() {
    // Delete own field shadows for all axis-specific attrs before calling
    // super.connectedCallback(), which handles the base attrs and FASTElement setup.
    const self = this as Record<string, unknown>;
    const attrFields = [
      'xAxisTitle',
      'yAxisTitle',
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
      'strokeWidth',
      'showXAxisLabelsTooltip',
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

  protected strokeWidthChanged() {
    this._requestRender();
  }

  protected showXAxisLabelsTooltipChanged() {
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
