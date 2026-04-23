export interface ChartDataPoint {
  /**
   * Legend text for the datapoint in the chart
   */
  legend: string;

  /**
   * data the datapoint in the chart
   */
  data: number;

  /**
   * Color for the legend in the chart. If not provided, it will fallback on the default color palette.
   */
  color?: string;

  /**
   * Callout data shown in the tooltip and the center label when the segment is highlighted.
   * If not provided, the numeric data value is used.
   */
  calloutData?: string;
}

export interface ChartProps {
  /**
   * data for the points in the chart
   */
  chartData: ChartDataPoint[];
}

export type Legend = {
  legend: string;
  color: string;
};
