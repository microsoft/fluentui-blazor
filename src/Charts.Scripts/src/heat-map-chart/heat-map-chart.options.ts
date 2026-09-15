/**
 * Accessibility data attached to an individual heat map data point.
 * @public
 */
export interface HeatMapAccessibilityData {
  /** Custom aria-label that overrides the auto-generated one. */
  ariaLabel?: string;
}

/**
 * A single cell in the heat map grid.
 * @public
 */
export interface HeatMapChartDataPoint {
  /** X-axis category value (string label, Date, or number). */
  x: string | Date | number;
  /** Y-axis category value (string label, Date, or number). */
  y: string | Date | number;
  /** Numeric value used to pick a color from the color scale. */
  value: number;
  /** Text rendered inside the rectangle. Defaults to `value`. */
  rectText?: string | number;
  /** Ratio shown in the tooltip as `numerator/denominator`. */
  ratio?: [number, number];
  /** Extra descriptive message shown in the tooltip. */
  descriptionMessage?: string;
  /** Click handler called when the cell is activated. */
  onClick?: () => void;
  /** Overrides the auto-generated aria-label. */
  callOutAccessibilityData?: HeatMapAccessibilityData;
}

/**
 * One data series (legend entry) in the heat map.
 * @public
 */
export interface HeatMapChartData {
  /** Name of this series, shown in the legend and tooltip. */
  legend: string;
  /**
   * Representative scalar for this series used to look up its legend color
   * via the color scale.
   */
  value: number;
  /** The data points that belong to this series. */
  data: HeatMapChartDataPoint[];
}

/**
 * Sort order for string axis labels.
 * - `'alphabetical'` (default) – ascending case-insensitive sort.
 * - `'none'` – preserve insertion order.
 * @public
 */
export type HeatMapSortOrder = 'none' | 'alphabetical';
