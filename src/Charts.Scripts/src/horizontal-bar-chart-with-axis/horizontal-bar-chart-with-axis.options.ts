export interface AccessibilityData {
  ariaLabel?: string;
}

export interface HorizontalBarChartWithAxisDataPoint {
  x: number;
  y: number | string;
  legend?: string;
  color?: string;
  gradient?: [string, string];
  xAxisCalloutData?: string;
  yAxisCalloutData?: string;
  onClick?: VoidFunction;
  callOutAccessibilityData?: AccessibilityData;
}
