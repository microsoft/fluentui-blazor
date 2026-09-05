import { attr } from '@microsoft/fast-element';
import { format } from 'd3-format';
import { sankey, sankeyLinkHorizontal, type SankeyLink, type SankeyNode } from 'd3-sankey';
import { ChartBase } from '../utils/chart-base.js';
import { getColorFromToken, getNextColor, jsonConverter, SVG_NAMESPACE_URI } from '../utils/chart-helpers.js';
import type { SankeyChartData, SankeyChartLink, SankeyChartNode } from './sankey-chart.options.js';

const createSvgElement = <T extends SVGElement>(tag: string): T =>
  document.createElementNS(SVG_NAMESPACE_URI, tag) as T;

const defaultNumberFormatter = format(',.2~f');
const NODE_WIDTH = 124;
const MIN_HEIGHT_FOR_LABEL = 24;
const MIN_HEIGHT_FOR_TWO_LINE_LABEL = 36;

const toNumber = (value: number | string | undefined, fallback: number): number => {
  if (value === undefined || value === null || value === '') {
    return fallback;
  }

  const parsed = Number(value);
  return Number.isFinite(parsed) ? parsed : fallback;
};

interface SankeyNodeDatum extends SankeyChartNode {
  actualValue?: number;
}
interface SankeyLinkDatum extends SankeyChartLink {
  unnormalizedValue?: number;
}
interface SankeyLayoutData {
  nodes: SankeyNodeDatum[];
  links: SankeyLinkDatum[];
}

const normalizeSmallNodes = (
  nodes: Array<SankeyNode<SankeyNodeDatum, SankeyLinkDatum>>,
  links: Array<SankeyLink<SankeyNodeDatum, SankeyLinkDatum>>,
): void => {
  const nodesByColumn = new Map<number, Array<SankeyNode<SankeyNodeDatum, SankeyLinkDatum>>>();

  nodes.forEach(node => {
    node.actualValue = node.value ?? 0;
    const column = node.depth ?? 0;
    const columnNodes = nodesByColumn.get(column) ?? [];
    columnNodes.push(node);
    nodesByColumn.set(column, columnNodes);
  });
  links.forEach(link => {
    link.unnormalizedValue = link.value;
  });

  nodesByColumn.forEach(columnNodes => {
    const columnValue = columnNodes.reduce((total, node) => total + (node.actualValue ?? 0), 0);
    if (columnValue === 0) {
      return;
    }

    const onePercent = columnValue * 0.01;
    let totalPercentage = 0;
    columnNodes.forEach(node => {
      const nodePercentage = ((node.actualValue ?? 0) / columnValue) * 100;
      node.value = nodePercentage < 1 ? onePercent : node.actualValue;
      totalPercentage += Math.max(nodePercentage, 1);
    });

    const scalingRatio = totalPercentage / 100;
    if (scalingRatio <= 1) {
      return;
    }

    columnNodes.forEach(node => {
      const actualValue = node.actualValue ?? 0;
      const normalizedValue = (node.value ?? 0) / scalingRatio;
      node.value = normalizedValue;
      if (actualValue === 0) {
        return;
      }

      [...(node.sourceLinks ?? []), ...(node.targetLinks ?? [])].forEach(link => {
        const originalLinkValue = link.unnormalizedValue ?? link.value;
        link.value = Math.max(normalizedValue * (originalLinkValue / actualValue), link.value);
      });
    });
  });
};

interface RenderedNode {
  legend: string;
  node: SVGRectElement;
  label: SVGTextElement;
}

interface RenderedLink {
  sourceLegend: string;
  targetLegend: string;
  path: SVGPathElement;
}

/** @public */
export class SankeyChart extends ChartBase {
  @attr({ converter: jsonConverter })
  public data!: SankeyChartData;

  @attr({ attribute: 'path-color' })
  public pathColor?: string;

  protected override _enableResizeObserver = true;

  private _nodes: RenderedNode[] = [];
  private _links: RenderedLink[] = [];

  public connectedCallback() {
    const self = this as Record<string, unknown>;
    const attrFields = ['data', 'pathColor'] as const;
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

  protected pathColorChanged() {
    this._requestRender();
  }

  protected override _performRender(): void {
    if (!this.$fastController.isConnected || !this.chartContainer) {
      return;
    }

    this._applyHostDimensions(this.width, this.height);
    this._clearChart();

    const chartData = this.data;
    const nodes = chartData?.nodes ?? [];
    const links = chartData?.links ?? [];

    if (nodes.length === 0) {
      this.legends = [];
      this._updateLegendInteractionState();
      this.elementInternals.ariaLabel = this._getHostAriaLabel();
      return;
    }

    const width = this.chartContainer.getBoundingClientRect().width || toNumber(this.width, 700);
    const height = this.chartContainer.getBoundingClientRect().height || toNumber(this.height, 300);
    const margins = { top: 16, right: 48, bottom: 32, left: 48 };
    const innerWidth = Math.max(width - margins.left - margins.right, 1);
    const innerHeight = Math.max(height - margins.top - margins.bottom, 1);

    const svg = createSvgElement<SVGSVGElement>('svg');
    svg.classList.add('chart');
    svg.setAttribute('width', String(width));
    svg.setAttribute('height', String(height));
    svg.setAttribute('viewBox', `0 0 ${width} ${height}`);

    const group = createSvgElement<SVGGElement>('g');
    group.setAttribute('transform', `translate(${margins.left}, ${margins.top})`);
    svg.appendChild(group);

    const graphData: SankeyLayoutData = {
      nodes: nodes.map(node => ({ ...node })),
      links: links.map(link => ({ ...link })),
    };

    const layout = sankey<SankeyLayoutData, SankeyNodeDatum, SankeyLinkDatum>()
      .nodeWidth(NODE_WIDTH)
      .nodePadding(8)
      .extent([
        [0, 0],
        [innerWidth, innerHeight],
      ]);
    let graph = layout(graphData);
    normalizeSmallNodes(graph.nodes, graph.links);
    graph = layout(graphData);

    this.legends = graph.nodes.map((node, index) => ({
      legend: node.name,
      color: node.color ? getColorFromToken(node.color) : getNextColor(index, 0),
    }));
    this._updateLegendInteractionState();

    const linkPath = sankeyLinkHorizontal<SankeyNodeDatum, SankeyLinkDatum>();

    this._links = graph.links.map((link, index) => {
      const source = link.source as SankeyNode<SankeyNodeDatum, SankeyLinkDatum>;
      const target = link.target as SankeyNode<SankeyNodeDatum, SankeyLinkDatum>;
      const sourceLegend = source.name;
      const targetLegend = target.name;
      const strokeColor = this.pathColor
        ? getColorFromToken(this.pathColor)
        : source.color
        ? getColorFromToken(source.color)
        : getNextColor(source.index ?? index, 0);

      const path = createSvgElement<SVGPathElement>('path');
      const pathData = linkPath(link) ?? '';
      const linkWidth = Math.max(link.width ?? 1, 1);
      const outerFocusPath = createSvgElement<SVGPathElement>('path');
      outerFocusPath.classList.add('sankey-link-focus-outline', 'outer');
      outerFocusPath.setAttribute('d', pathData);
      outerFocusPath.setAttribute('aria-hidden', 'true');
      outerFocusPath.style.setProperty('--sankey-link-width', `${linkWidth}px`);
      const innerFocusPath = createSvgElement<SVGPathElement>('path');
      innerFocusPath.classList.add('sankey-link-focus-outline', 'inner');
      innerFocusPath.setAttribute('d', pathData);
      innerFocusPath.setAttribute('aria-hidden', 'true');
      innerFocusPath.style.setProperty('--sankey-link-width', `${linkWidth}px`);
      path.classList.add('sankey-link');
      path.dataset.sourceLegend = sourceLegend;
      path.dataset.targetLegend = targetLegend;
      path.setAttribute('d', pathData);
      path.setAttribute('stroke', strokeColor);
      path.setAttribute('stroke-width', String(linkWidth));
      const linkValue = link.unnormalizedValue ?? link.value;
      path.setAttribute('role', 'img');
      path.setAttribute('aria-label', `${sourceLegend} to ${targetLegend}, ${defaultNumberFormatter(linkValue)}`);
      path.setAttribute('tabindex', index === 0 ? '0' : '-1');

      const showLinkCallout = (): void => {
        if (!this._shouldShowLinkTooltip(sourceLegend, targetLegend) || this.hideTooltip) {
          return;
        }

        const rootBounds = this.getBoundingClientRect();
        const linkBounds = path.getBoundingClientRect();
        const anchorX = linkBounds.left - rootBounds.left + linkBounds.width / 2;
        const anchorY = linkBounds.top - rootBounds.top;
        this._currentTooltipDataPoint = link;
        this.tooltipProps = {
          isVisible: true,
          legend: `${sourceLegend} → ${targetLegend}`,
          yValue: defaultNumberFormatter(linkValue),
          color: strokeColor,
          xPos: this._isRTL ? rootBounds.width - anchorX : anchorX,
          yPos: Math.max(anchorY, 0),
        };
        this._positionTooltipFromAnchor(anchorX, anchorY, { preferredVertical: 'above', horizontalAlign: 'center' });
      };

      path.addEventListener('mouseenter', showLinkCallout);
      path.addEventListener('focus', showLinkCallout);
      path.addEventListener('mouseleave', () => this._clearTooltip());
      path.addEventListener('blur', () => this._clearTooltip());
      path.addEventListener('click', () => this._focusRovingElement(this._getRovingElements(), path));
      path.addEventListener('keydown', (event: KeyboardEvent) => {
        this._rovingKeydown(this._getRovingElements(), event);
      });

      group.append(outerFocusPath, innerFocusPath, path);
      return { sourceLegend, targetLegend, path };
    });

    this._nodes = graph.nodes.map((node, index) => {
      const fill = node.color ? getColorFromToken(node.color) : getNextColor(index, 0);
      const rect = createSvgElement<SVGRectElement>('rect');
      rect.classList.add('sankey-node');
      rect.dataset.legend = node.name;
      rect.setAttribute('x', String(node.x0 ?? 0));
      rect.setAttribute('y', String(node.y0 ?? 0));
      rect.setAttribute('width', String(Math.max((node.x1 ?? 0) - (node.x0 ?? 0), 0)));
      rect.setAttribute('height', String(Math.max((node.y1 ?? 0) - (node.y0 ?? 0), 0)));
      rect.setAttribute('fill', fill);
      rect.setAttribute('role', 'img');
      rect.setAttribute('tabindex', graph.links.length === 0 && index === 0 ? '0' : '-1');
      const nodeValue = node.actualValue ?? node.value ?? 0;
      rect.setAttribute('aria-label', `${node.name}, ${defaultNumberFormatter(nodeValue)}`);
      group.appendChild(rect);

      const label = createSvgElement<SVGTextElement>('text');
      label.classList.add('sankey-node-label');
      label.dataset.legend = node.name;
      const nodeHeight = Math.max((node.y1 ?? 0) - (node.y0 ?? 0), 0);
      label.setAttribute('x', String((node.x0 ?? 0) + 5));
      label.setAttribute('y', String((node.y0 ?? 0) + 5));
      label.setAttribute('text-anchor', 'start');

      if (nodeHeight > MIN_HEIGHT_FOR_LABEL) {
        const name = createSvgElement<SVGTSpanElement>('tspan');
        name.classList.add('sankey-node-name');
        name.setAttribute('x', String((node.x0 ?? 0) + 5));
        name.setAttribute('dy', '0.8em');
        name.textContent = node.name;
        label.appendChild(name);

        const value = createSvgElement<SVGTSpanElement>('tspan');
        value.classList.add('sankey-node-value');
        const hasTwoLineLabel = nodeHeight > MIN_HEIGHT_FOR_TWO_LINE_LABEL;
        value.setAttribute('x', String(hasTwoLineLabel ? (node.x0 ?? 0) + 5 : (node.x1 ?? 0) - 8));
        value.setAttribute('dy', hasTwoLineLabel ? '1.2em' : '0');
        value.setAttribute('text-anchor', hasTwoLineLabel ? 'start' : 'end');
        value.textContent = defaultNumberFormatter(nodeValue);
        label.appendChild(value);
      }
      group.appendChild(label);

      const showNodeCallout = (): void => {
        if (nodeHeight > MIN_HEIGHT_FOR_LABEL || this.hideTooltip) {
          return;
        }

        const rootBounds = this.getBoundingClientRect();
        const nodeBounds = rect.getBoundingClientRect();
        const anchorX = nodeBounds.left - rootBounds.left + nodeBounds.width / 2;
        const anchorY = nodeBounds.top - rootBounds.top;
        const position = this._resolveTooltipPositionFromAnchor(anchorX, anchorY, {
          preferredVertical: 'above',
          horizontalAlign: 'center',
        });
        this._currentTooltipDataPoint = node;
        this.tooltipProps = {
          isVisible: true,
          legend: node.name,
          yValue: defaultNumberFormatter(nodeValue),
          color: fill,
          ...position,
        };
      };
      rect.addEventListener('mouseenter', showNodeCallout);
      rect.addEventListener('focus', showNodeCallout);
      rect.addEventListener('mouseleave', () => this._clearTooltip());
      rect.addEventListener('blur', () => this._clearTooltip());
      rect.addEventListener('click', () => this._focusRovingElement(this._getRovingElements(), rect));
      rect.addEventListener('keydown', (event: KeyboardEvent) => {
        this._rovingKeydown(this._getRovingElements(), event);
      });

      return { legend: node.name, node: rect, label };
    });

    this.chartContainer.appendChild(svg);
    this._applyActiveLegendState();
    this.elementInternals.ariaLabel = this._getHostAriaLabel();
  }

  protected override _applyActiveLegendState(): void {
    if (!this._nodes || !this._links) {
      return;
    }

    const highlighted = this._getHighlightedLegends();
    const hasSelection = highlighted.length > 0;

    for (const renderedNode of this._nodes) {
      const isActive = !hasSelection || highlighted.includes(renderedNode.legend);
      renderedNode.node.classList.toggle('inactive', !isActive);
      renderedNode.node.setAttribute('opacity', isActive ? '1' : '0.1');
      renderedNode.label.classList.toggle('inactive', !isActive);
      renderedNode.label.setAttribute('opacity', isActive ? '1' : '0.1');
    }

    for (const renderedLink of this._links) {
      const isActive =
        !hasSelection ||
        highlighted.includes(renderedLink.sourceLegend) ||
        highlighted.includes(renderedLink.targetLegend);
      renderedLink.path.classList.toggle('inactive', !isActive);
      renderedLink.path.setAttribute('opacity', isActive ? '1' : '0.1');
    }
  }

  protected override _getHostAriaLabel(): string {
    const nodeCount = this.data?.nodes?.length ?? 0;
    const linkCount = this.data?.links?.length ?? 0;
    return `Sankey chart with ${nodeCount} nodes and ${linkCount} links.`;
  }

  private _shouldShowLinkTooltip(sourceLegend: string, targetLegend: string): boolean {
    const highlighted = this._getHighlightedLegends();
    return highlighted.length === 0 || highlighted.includes(sourceLegend) || highlighted.includes(targetLegend);
  }

  private _getRovingElements(): SVGElement[] {
    return [...this._links.map(link => link.path), ...this._nodes.map(node => node.node)];
  }

  private _clearChart(): void {
    this._nodes = [];
    this._links = [];
    this._clearTooltip();

    if (!this.chartContainer) {
      return;
    }

    while (this.chartContainer.firstChild) {
      this.chartContainer.firstChild.remove();
    }
  }
}
