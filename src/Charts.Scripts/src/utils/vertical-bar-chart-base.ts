import { attr } from '@microsoft/fast-element';
import { CartesianChartBase } from './cartesian-chart-base.js';
import { jsonConverter } from './chart-helpers.js';

/**
 * Shared foundation for Cartesian charts that render vertical bars.
 *
 * @internal
 */
export abstract class VerticalBarChartBase extends CartesianChartBase {
  /** Width of each bar. Use `auto` to fill the available band. */
  @attr({ attribute: 'bar-width' })
  public barWidth?: number | string;

  /** Maximum width of each bar when its width is calculated automatically. */
  @attr({ attribute: 'max-bar-width' })
  public maxBarWidth?: number | string;

  /** Uses the first resolved series color for every bar. */
  @attr({ attribute: 'use-single-color', mode: 'boolean' })
  public useSingleColor: boolean = false;

  /** Renders vertical color gradients on bars. */
  @attr({ attribute: 'enable-gradient', mode: 'boolean' })
  public enableGradient: boolean = false;

  /** Ordered color palette used when a data point does not provide a color. */
  @attr({ converter: jsonConverter })
  public colors?: string[];

  public connectedCallback(): void {
    const self = this as Record<string, unknown>;
    const attrFields = ['barWidth', 'maxBarWidth', 'useSingleColor', 'enableGradient', 'colors'] as const;
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

  protected barWidthChanged(): void {
    this._requestRender();
  }

  protected maxBarWidthChanged(): void {
    this._requestRender();
  }

  protected useSingleColorChanged(): void {
    this._requestRender();
  }

  protected enableGradientChanged(): void {
    this._requestRender();
  }

  protected colorsChanged(): void {
    this._requestRender();
  }
}
