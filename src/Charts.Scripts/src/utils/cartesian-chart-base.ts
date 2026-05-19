import { attr } from '@microsoft/fast-element';
import { ChartBase } from './chart-base.js';

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
}
