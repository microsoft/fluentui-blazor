import { attr } from '@microsoft/fast-element';
import { hierarchy, tree } from 'd3-hierarchy';
import { ChartBase } from '../utils/chart-base.js';
import {
  getColorFromToken,
  getNextColor,
  jsonConverter,
  parseNumber as toNumber,
  resolvePixelDimension,
  SVG_NAMESPACE_URI,
} from '../utils/chart-helpers.js';
import type { TreeChartDataPoint } from './tree-chart.options.js';

const createSvgElement = <T extends SVGElement>(tag: string): T =>
  document.createElementNS(SVG_NAMESPACE_URI, tag) as T;

/** @public */
export class TreeChart extends ChartBase {
  @attr({ converter: jsonConverter })
  public data!: TreeChartDataPoint;

  @attr({ attribute: 'node-width' })
  public nodeWidth?: number | string;

  @attr({ attribute: 'node-height' })
  public nodeHeight?: number | string;

  protected override _enableResizeObserver = true;

  public connectedCallback() {
    const self = this as Record<string, unknown>;
    const attrFields = ['data', 'nodeWidth', 'nodeHeight'] as const;
    const saved: Partial<Record<(typeof attrFields)[number], unknown>> = {};

    for (const field of attrFields) {
      saved[field] = self[field];
      delete self[field];
    }

    super.connectedCallback();

    for (const field of attrFields) {
      if (self[field] === undefined && saved[field] !== undefined) {
        self[field] = saved[field];
      }
    }

    this._requestRender();
  }

  protected dataChanged() {
    this._requestRender();
  }

  protected nodeWidthChanged() {
    this._requestRender();
  }

  protected nodeHeightChanged() {
    this._requestRender();
  }

  protected override _performRender(): void {
    if (!this.$fastController.isConnected || !this.chartContainer) {
      return;
    }

    this._applyHostDimensions(this.width, this.height);
    this._clearChart();
    this.legends = [];
    this._updateLegendInteractionState();

    if (!this.data) {
      this.elementInternals.ariaLabel = this._getHostAriaLabel();
      return;
    }

    const width = resolvePixelDimension(this.width, this.chartContainer.getBoundingClientRect().width, 600);
    const height = resolvePixelDimension(this.height, this.chartContainer.getBoundingClientRect().height, 300);
    const nodeWidth = toNumber(this.nodeWidth, 96);
    const nodeHeight = toNumber(this.nodeHeight, 42);
    const margins = { top: nodeHeight, right: nodeWidth / 2, bottom: nodeHeight, left: nodeWidth / 2 };
    const innerWidth = Math.max(width - margins.left - margins.right, 1);
    const innerHeight = Math.max(height - margins.top - margins.bottom, 1);

    const root = hierarchy(this.data);
    const layout = tree<TreeChartDataPoint>().size([innerWidth, innerHeight]);
    const treeRoot = layout(root);

    const svg = createSvgElement<SVGSVGElement>('svg');
    svg.classList.add('chart');
    svg.setAttribute('width', String(width));
    svg.setAttribute('height', String(height));
    svg.setAttribute('viewBox', `0 0 ${width} ${height}`);

    const group = createSvgElement<SVGGElement>('g');
    group.setAttribute('transform', `translate(${margins.left}, ${margins.top})`);
    svg.appendChild(group);

    for (const link of treeRoot.links()) {
      const line = createSvgElement<SVGLineElement>('line');
      line.classList.add('tree-link');
      line.setAttribute('x1', String(link.source.x));
      line.setAttribute('y1', String(link.source.y));
      line.setAttribute('x2', String(link.target.x));
      line.setAttribute('y2', String(link.target.y));
      group.appendChild(line);
    }

    treeRoot.descendants().forEach((node, index) => {
      const fill = node.data.fill ? getColorFromToken(node.data.fill) : getNextColor(index, 0);

      const rect = createSvgElement<SVGRectElement>('rect');
      rect.classList.add('tree-node');
      rect.setAttribute('x', String(node.x - nodeWidth / 2));
      rect.setAttribute('y', String(node.y - nodeHeight / 2));
      rect.setAttribute('width', String(nodeWidth));
      rect.setAttribute('height', String(nodeHeight));
      rect.setAttribute('fill', fill);
      group.appendChild(rect);

      const label = createSvgElement<SVGTextElement>('text');
      label.classList.add('tree-node-label');
      label.setAttribute('x', String(node.x));
      label.setAttribute('y', String(node.data.subname ? node.y - 5 : node.y));
      label.textContent = node.data.name;
      group.appendChild(label);

      if (node.data.subname) {
        const subname = createSvgElement<SVGTextElement>('text');
        subname.classList.add('tree-node-subname');
        subname.setAttribute('x', String(node.x));
        subname.setAttribute('y', String(node.y + 12));
        subname.textContent = node.data.subname;
        group.appendChild(subname);
      }
    });

    this.chartContainer.appendChild(svg);
    this.elementInternals.ariaLabel = this._getHostAriaLabel();
  }

  protected override _applyActiveLegendState(): void {}

  protected override _getHostAriaLabel(): string {
    const nodeCount = this.data ? hierarchy(this.data).descendants().length : 0;
    return `Tree chart with ${nodeCount} nodes.`;
  }

  private _clearChart(): void {
    this._clearTooltip();

    if (!this.chartContainer) {
      return;
    }

    while (this.chartContainer.firstChild) {
      this.chartContainer.firstChild.remove();
    }
  }
}
