/** @public */
export interface SparklineDataPoint {
  /** @public */ x: number | Date | string;
  /** @public */ y: number;
}

/** @public */
export interface SparklineChartSeries {
  /** @public */ legend?: string;
  /** @public */ color?: string;
  /** @public */ data: SparklineDataPoint[];
}

/** @public */
export interface SparklineChartData {
  /** @public */ chartTitle?: string;
  /** @public */ lineChartData: SparklineChartSeries[];
}

/** @public */
export type SparklineVariant = 'line' | 'area';
