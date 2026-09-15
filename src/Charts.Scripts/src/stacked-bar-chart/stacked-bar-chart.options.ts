/** @public */
export interface StackedBarChartDataPoint {
  /** @public */ legend: string;
  /** @public */ data: number;
  /** @public */ color?: string;
  /** @public */ gradient?: [string, string];
  /** @public */ onClick?: VoidFunction;
  /** @public */ placeHolder?: boolean;
}

/** @public */
export interface StackedBarChartData {
  /** @public */ chartTitle?: string;
  /** @public */ chartData: StackedBarChartDataPoint[];
}
