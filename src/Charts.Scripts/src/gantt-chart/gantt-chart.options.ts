export interface AccessibilityData {
  ariaLabel?: string;
}

export interface GanttChartDataPoint {
  /**
   * Dependent value of the data point, rendered along the x-axis.
   * Represents the time or numeric range of the bar.
   */
  x: {
    start: Date | number;
    end: Date | number;
  };

  /**
   * Independent value of the data point, rendered along the y-axis.
   * If y is a number, then each y-coordinate is plotted at its y-coordinate.
   * If y is a string, then the data is evenly spaced along the y-axis.
   */
  y: number | string;

  /**
   * Legend text for the datapoint in the chart.
   */
  legend?: string;

  /**
   * Color for the bar in the chart.
   */
  color?: string;

  /**
   * Gradient for the bar in the chart. If not provided, falls back on the default color palette.
   * Overrides the color prop when `enableGradient` is set to true for the chart.
   */
  gradient?: [string, string];

  /**
   * Callout data for x axis (shown in the tooltip xValue).
   */
  xAxisCalloutData?: string;

  /**
   * Callout data for y axis (shown in the tooltip yValue).
   */
  yAxisCalloutData?: string;

  /**
   * onClick action for the data point.
   */
  onClick?: VoidFunction;

  /**
   * Accessibility data for screen readers.
   */
  callOutAccessibilityData?: AccessibilityData;
}

export type AxisCategoryOrder =
  | 'default'
  | 'data'
  | 'category ascending'
  | 'category descending'
  | 'total ascending'
  | 'total descending'
  | 'min ascending'
  | 'min descending'
  | 'max ascending'
  | 'max descending'
  | 'sum ascending'
  | 'sum descending'
  | 'mean ascending'
  | 'mean descending'
  | 'median ascending'
  | 'median descending';
