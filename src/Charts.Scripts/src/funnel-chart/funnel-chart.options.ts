/**
 * A single sub-value within a stacked funnel stage.
 *
 * @public
 */
export interface FunnelChartSubValue {
  /** Category label for this sub-value. */
  category: string;
  /** Numeric value for this sub-value. */
  value: number;
  /** Fill color for this sub-value. */
  color: string;
}

/**
 * A single stage (segment) in a funnel chart.
 * Either `value` + `color` (simple funnel) or `subValues` (stacked funnel) must be provided.
 *
 * @public
 */
export interface FunnelChartDataPoint {
  /**
   * Stage name or identifier, shown in legends and tooltips.
   */
  stage: string;

  /**
   * Numeric value for a non-stacked stage.
   */
  value?: number;

  /**
   * Fill color for a non-stacked stage.
   */
  color?: string;

  /**
   * Sub-values for a stacked funnel stage.
   */
  subValues?: FunnelChartSubValue[];
}

/**
 * Orientation of the funnel chart.
 *
 * @public
 */
export type FunnelChartOrientation = 'vertical' | 'horizontal';
