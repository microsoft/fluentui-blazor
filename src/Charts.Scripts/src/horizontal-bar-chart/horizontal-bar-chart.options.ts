/**
 * Display variant of the horizontal bar chart.
 *
 * @public
 */
export type HorizontalBarChartVariant = 'part-to-whole' | 'absolute-scale' | 'single-bar';

/**
 * A single data point in the horizontal bar chart.
 *
 * @public
 */
export interface HorizontalBarChartDataPoint {
  /**
   * Legend text for the datapoint in the chart
   */
  legend: string;

  /**
   * data the datapoint in the chart
   */
  data: number;

  /**
   * total length of bar
   */
  total?: number;

  /**
   * onClick action for each datapoint in the chart
   */
  onClick?: VoidFunction;

  /**
   * Color for the legend in the chart. If not provided, it will fallback on the default color palette.
   */
  color?: string;

  /**
   * Gradient fill for the bar. A tuple of two CSS color strings: [startColor, endColor].
   * When provided, overrides the `color` property.
   */
  gradient?: [string, string];
}

/**
 * Props for a single series in the horizontal bar chart.
 *
 * @public
 */
export interface HorizontalBarChartProps {
  /**
   * title for the data series
   */
  chartSeriesTitle?: string;

  /**
   * data for the points in the chart
   */
  chartData: HorizontalBarChartDataPoint[];

  /**
   * Benchmark value rendered as a reference line on the bar.
   */
  benchmarkData?: number;

  /**
   * Text label displayed alongside the bar, overriding the computed data label.
   */
  chartDataText?: string;
}
