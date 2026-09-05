export type CartesianChartMargins = {
  top: number;
  right: number;
  bottom: number;
  left: number;
};

export const getDirectionalMargins = (margins: CartesianChartMargins, isRTL: boolean): CartesianChartMargins => {
  return isRTL ? { top: margins.top, right: margins.left, bottom: margins.bottom, left: margins.right } : margins;
};

export const resolveChartMargins = (
  defaults: CartesianChartMargins,
  custom: Partial<CartesianChartMargins> | undefined,
  isRTL: boolean,
  hasSecondaryYAxis: boolean = false,
): CartesianChartMargins => {
  const resolved = { ...defaults, ...custom };
  if (hasSecondaryYAxis) {
    const secondarySide = isRTL ? 'left' : 'right';
    if (custom?.[secondarySide] === undefined) {
      resolved[secondarySide] = 70;
    }
  }
  return getDirectionalMargins(resolved, isRTL);
};

export type PrimaryYAxisLayout = {
  axisX: number;
  tickLineX2: number;
  tickLabelX: number;
  titleX: number;
  titleRotation: 'rotate(-90)' | 'rotate(90)';
};

export const getPrimaryYAxisLayout = (
  isRTL: boolean,
  margins: CartesianChartMargins,
  innerWidth: number,
  innerHeight: number,
  tickPadding: number,
): PrimaryYAxisLayout => {
  return {
    axisX: isRTL ? margins.left + innerWidth : margins.left,
    tickLineX2: isRTL ? 6 : -6,
    tickLabelX: isRTL ? 6 + tickPadding : -(6 + tickPadding),
    titleX: isRTL ? innerHeight / 2 : -innerHeight / 2,
    titleRotation: isRTL ? 'rotate(90)' : 'rotate(-90)',
  };
};
