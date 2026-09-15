/** @public */
export interface TreeChartDataPoint {
  /** @public */ name: string;
  /** @public */ subname?: string;
  /** @public */ fill?: string;
  /** @public */ children?: TreeChartDataPoint[];
}
