import type { ChartMarkerShape } from './chart-options.js';

export const markerShapeNames: readonly ChartMarkerShape[] = [
  'circle',
  'square',
  'triangle',
  'diamond',
  'pyramid',
  'hexagon',
  'pentagon',
  'octagon',
];

const markerShapeWidthRatios = [1, 1, 1, 1, 1, 2, 1.168, 2.414];

export const getMarkerPath = (x: number, y: number, referenceWidth: number, shapeIndex: number): string => {
  const width = referenceWidth / markerShapeWidthRatios[shapeIndex];
  const half = width / 2;
  const paths = [
    `M ${x - half} ${y} A ${half} ${half} 0 1 0 ${x + half} ${y} A ${half} ${half} 0 1 0 ${x - half} ${y}`,
    `M ${x - half} ${y - half} H ${x + half} V ${y + half} H ${x - half} Z`,
    `M ${x - half} ${y - 0.2886 * width} H ${x + half} L ${x} ${y + 0.5774 * width} Z`,
    `M ${x} ${y - half} L ${x + half} ${y} L ${x} ${y + half} L ${x - half} ${y} Z`,
    `M ${x} ${y - 0.5774 * width} L ${x + half} ${y + 0.2886 * width} L ${x - half} ${y + 0.2886 * width} Z`,
    `M ${x - 0.5 * width} ${y - 0.866 * width} L ${x + 0.5 * width} ${y - 0.866 * width} L ${x + width} ${y} L ${
      x + 0.5 * width
    } ${y + 0.866 * width} L ${x - 0.5 * width} ${y + 0.866 * width} L ${x - width} ${y} Z`,
    `M ${x} ${y - 0.851 * width} L ${x + 0.6884 * width} ${y - 0.2633 * width} L ${x + 0.5001 * width} ${
      y + 0.6884 * width
    } L ${x - 0.5001 * width} ${y + 0.6884 * width} L ${x - 0.6884 * width} ${y - 0.2633 * width} Z`,
    `M ${x - 0.5001 * width} ${y - 1.207 * width} L ${x + 0.5001 * width} ${y - 1.207 * width} L ${x + 1.207 * width} ${
      y - 0.5001 * width
    } L ${x + 1.207 * width} ${y + 0.5001 * width} L ${x + 0.5001 * width} ${y + 1.207 * width} L ${
      x - 0.5001 * width
    } ${y + 1.207 * width} L ${x - 1.207 * width} ${y + 0.5001 * width} L ${x - 1.207 * width} ${y - 0.5001 * width} Z`,
  ];
  return paths[shapeIndex];
};
