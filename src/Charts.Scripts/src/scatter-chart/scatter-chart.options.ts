/** @public */
export interface ScatterChartDataPoint {
  /** @public */ x: number | Date | string;
  /** @public */ y: number | string;
  /** @public */ markerSize?: number;
}

/** @public */
export interface ScatterChartSeries {
  /** @public */ legend: string;
  /** @public */ data: ScatterChartDataPoint[];
  /** @public */ color?: string;
}
