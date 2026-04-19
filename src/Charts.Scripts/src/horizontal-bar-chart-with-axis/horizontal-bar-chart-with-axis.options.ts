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

export interface HorizontalBarChartWithAxisLegend {
  legend: string;
  color: string;
}
