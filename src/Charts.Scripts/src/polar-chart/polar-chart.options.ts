import type { AxisCategoryOrder, AxisScaleType } from '../utils/chart-options.js';

/** @public */
export type PolarChartValue = string | number | Date;

/** @public */
export interface PolarChartDataPoint {
  /** Angular value. React-compatible alias for `x`. */
  /** @public */ theta?: string | number;
  /** Radial value. React-compatible alias for `y`. */
  /** @public */ r?: PolarChartValue;
  /** Legacy angular category. */
  /** @public */ x?: string;
  /** Legacy radial value. */
  /** @public */ y?: number;
  /** @public */ radialAxisCalloutData?: string;
  /** @public */ angularAxisCalloutData?: string;
  /** @public */ markerSize?: number;
  /** @public */ color?: string;
  /** @public */ text?: string;
  /** @public */ onClick?: () => void;
  /** @public */ callOutAccessibilityData?: { ariaLabel?: string };
}

/** @public */
export interface PolarAxisOptions {
  /** @public */ tickCount?: number;
  /** @public */ tickValues?: PolarChartValue[];
  /** @public */ tickText?: string[];
  /** @public */ tickFormat?: string;
  /** @public */ tickStep?: number | string;
  /** Numeric value, Date, or an ISO 8601 date string from a serialized data source. */
  /** @public */ tick0?: number | Date | string;
  /** @public */ categoryOrder?: AxisCategoryOrder;
  /** @public */ scaleType?: AxisScaleType;
  /** @public */ rangeStart?: number | Date | string;
  /** @public */ rangeEnd?: number | Date | string;
  /** @public */ unit?: 'radians' | 'degrees';
}

/** @public */
export interface PolarLineOptions {
  /** @public */ strokeWidth?: number | string;
  /** @public */ strokeDasharray?: number | string;
  /** @public */ strokeDashoffset?: number | string;
  /** @public */ strokeLinecap?: 'butt' | 'round' | 'square' | 'inherit';
  /** @public */ curve?: 'linear' | 'natural' | 'step' | 'stepAfter' | 'stepBefore';
}

/** @public */
export interface PolarChartSeries {
  /** @public */ legend: string;
  /** @public */ data: PolarChartDataPoint[];
  /** @public */ color?: string;
  /** Defaults to `areapolar` for backward compatibility. */
  /** @public */ type?: 'areapolar' | 'linepolar' | 'scatterpolar';
  /** @public */ lineOptions?: PolarLineOptions;
}
