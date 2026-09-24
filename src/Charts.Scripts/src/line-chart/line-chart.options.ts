/** @public */
export interface LineChartDataPoint {
  /** Numeric value, Date, or an ISO 8601 date string from a serialized data source. */
  /** @public */ x: number | Date | string;
  /** @public */ y: number;
  /** @public */ xAxisCalloutData?: string | Date;
  /** @public */ yAxisCalloutData?: string;
  /** @public */ hideCallout?: boolean;
  /** @public */ onDataPointClick?: VoidFunction;
  /** @deprecated Use `onDataPointClick` instead. @public */ onClick?: VoidFunction;
}

/** @public */
export interface LineChartGap {
  /** @public */ startIndex: number;
  /** @public */ endIndex: number;
}

/** @public */
export interface LineChartLineOptions {
  /** @public */ strokeWidth?: number | string;
  /** @public */ strokeDasharray?: number | string;
  /** @public */ strokeDashoffset?: number | string;
  /** @public */ strokeLinecap?: 'butt' | 'round' | 'square' | 'inherit';
  /** @public */ lineBorderWidth?: number | string;
  /** @public */ lineBorderColor?: string;
}

/** @public */
export interface LineChartColorFillBarData {
  /** @public */ startX: number | Date | string;
  /** @public */ endX: number | Date | string;
}

/** @public */
export interface LineChartColorFillBar {
  /** @public */ legend: string;
  /** @public */ color: string;
  /** @public */ data: LineChartColorFillBarData[];
  /** @public */ applyPattern?: boolean;
}

/** @public */
export interface LineChartEventAnnotation {
  /** Date value or an ISO 8601 date string from a serialized data source. */
  /** @public */ date: Date | string;
  /** @public */ event: string;
  /** Plain-text content rendered in the event detail card. */
  /** @public */ cardContent?: string;
  /** @public */ onRenderCard?: () => HTMLElement | string;
}

/** @public */
export interface LineChartEventAnnotationProps {
  /** @public */ events: LineChartEventAnnotation[];
  /** @public */ strokeColor?: string;
  /** @public */ labelColor?: string;
  /** @public */ labelHeight?: number;
  /** @public */ labelWidth?: number;
  /** Label for grouped events. Strings may include a `{count}` placeholder. */
  /** @public */ mergedLabel?: string | ((count: number) => string);
}

/** @public */
export interface LineChartSeries {
  /** @public */ legend: string;
  /** @public */ data: LineChartDataPoint[];
  /** @public */ color?: string;
  /** @public */ gaps?: LineChartGap[];
  /** @public */ useSecondaryYScale?: boolean;
  /** @public */ lineOptions?: LineChartLineOptions;
  /** @public */ onLineClick?: VoidFunction;
}
