export interface AccessibilityData {
  ariaLabel?: string;
}

export interface GanttChartDataPoint {
  /**
   * Dependent value of the data point, rendered along the x-axis.
   * Represents the time or numeric range of the bar.
   *
   * When data arrives via JSON (e.g. from a Blazor wrapper), C# `DateTime` and
   * `DateTimeOffset` values are serialized as ISO 8601 strings such as
   * `"2024-01-15T10:30:00Z"` or `"2024-01-15T10:30:00+05:30"`. The chart
   * accepts those strings directly and converts them to `Date` internally.
   */
  x: {
    start: Date | number | string;
    end: Date | number | string;
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
