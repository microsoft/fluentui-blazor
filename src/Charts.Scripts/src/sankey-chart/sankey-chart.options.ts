/** @public */
export interface SankeyChartNode {
  /** @public */ name: string;
  /** @public */ color?: string;
}

/** @public */
export interface SankeyChartLink {
  /** @public */ source: number;
  /** @public */ target: number;
  /** @public */ value: number;
}

/** @public */
export interface SankeyChartData {
  /** @public */ nodes: SankeyChartNode[];
  /** @public */ links: SankeyChartLink[];
}
