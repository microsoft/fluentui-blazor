import { SVG_NAMESPACE_URI } from './chart-helpers.js';

export interface BorderedLinePathOptions {
  layer: SVGGElement;
  pathData: string;
  legend: string;
  color: string;
  strokeWidth: number;
  borderWidth?: number;
  borderColor?: string;
  strokeLinecap?: 'butt' | 'round' | 'square' | 'inherit';
  strokeDasharray?: string | number;
  strokeDashoffset?: string | number;
}

export interface BorderedLinePaths {
  linePath: SVGPathElement;
  borderPath?: SVGPathElement;
}

const createLinePath = (
  className: string,
  options: BorderedLinePathOptions,
  stroke: string,
  strokeWidth: number,
): SVGPathElement => {
  const path = document.createElementNS(SVG_NAMESPACE_URI, 'path');
  path.classList.add(className);
  path.dataset.legend = options.legend;
  path.setAttribute('fill', 'none');
  path.setAttribute('stroke', stroke);
  path.setAttribute('stroke-width', String(strokeWidth));
  path.setAttribute('stroke-linecap', options.strokeLinecap ?? 'round');
  if (options.strokeDasharray !== undefined) {
    path.setAttribute('stroke-dasharray', String(options.strokeDasharray));
  }
  if (options.strokeDashoffset !== undefined) {
    path.setAttribute('stroke-dashoffset', String(options.strokeDashoffset));
  }
  path.setAttribute('d', options.pathData);
  return path;
};

export const renderBorderedLinePath = (options: BorderedLinePathOptions): BorderedLinePaths => {
  let borderPath: SVGPathElement | undefined;
  if (options.borderWidth && options.borderWidth > 0) {
    borderPath = createLinePath(
      'line-border',
      options,
      options.borderColor ?? 'var(--colorNeutralBackground1, #fff)',
      options.strokeWidth + options.borderWidth * 2,
    );
    options.layer.appendChild(borderPath);
  }

  const linePath = createLinePath('line-path', options, options.color, options.strokeWidth);
  options.layer.appendChild(linePath);
  return { borderPath, linePath };
};
