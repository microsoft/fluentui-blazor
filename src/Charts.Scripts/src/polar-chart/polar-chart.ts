import { attr } from '@microsoft/fast-element';
import { extent } from 'd3-array';
import { areaRadial, lineRadial, pointRadial } from 'd3-shape';
import { ChartBase } from '../utils/chart-base.js';
import {
  escapeHtml,
  getColorFromToken,
  getNextColor,
  jsonConverter,
  SVG_NAMESPACE_URI,
} from '../utils/chart-helpers.js';
import type { ChartMargins, TooltipProps } from '../utils/chart-options.js';
import type { PolarAxisOptions, PolarChartSeries } from './polar-chart.options.js';
import {
  createAngularScale,
  createRadialScale,
  formatPolarAngle,
  formatPolarRadialValue,
  getCurveFactory,
  normalizeSeries,
  type NormalizedPolarPoint,
} from './polar-chart.utils.js';

const createSvgElement = <T extends SVGElement>(tag: string): T =>
  document.createElementNS(SVG_NAMESPACE_URI, tag) as T;

const toNumber = (value: number | string | undefined, fallback: number): number => {
  if (value === undefined || value === null || value === '') {
    return fallback;
  }

  const parsed = Number(value);
  return Number.isFinite(parsed) ? parsed : fallback;
};

interface RenderedSeries {
  legend: string;
  shapes: SVGPathElement[];
  markers: SVGCircleElement[];
  labels: SVGTextElement[];
}

/** A series value displayed in a grouped PolarChart callout. */
export type PolarTooltipEntry = { legend: string; color: string; value: string };

type PolarTooltipState = TooltipProps & { angularLabel?: string; entries?: PolarTooltipEntry[] };

/** @public */
export class PolarChart extends ChartBase {
  public declare tooltipProps: PolarTooltipState;

  @attr({ converter: jsonConverter })
  public data!: PolarChartSeries[];

  @attr({ attribute: 'show-markers', mode: 'boolean' })
  public showMarkers: boolean = false;

  @attr({ attribute: 'enable-multi-value-callout', mode: 'boolean' })
  public enableMultiValueCallout: boolean = false;

  @attr
  public shape: 'circle' | 'polygon' = 'circle';

  @attr
  public direction: 'clockwise' | 'counterclockwise' = 'counterclockwise';

  @attr
  public hole: number | string = 0;

  @attr({ attribute: 'radial-axis', converter: jsonConverter })
  public radialAxis?: PolarAxisOptions;

  @attr({ attribute: 'angular-axis', converter: jsonConverter })
  public angularAxis?: PolarAxisOptions;

  @attr({ converter: jsonConverter })
  public margins?: ChartMargins;

  @attr({ attribute: 'date-localize-options', converter: jsonConverter })
  public dateLocalizeOptions?: Intl.DateTimeFormatOptions;

  @attr({ attribute: 'use-utc', mode: 'boolean' })
  public useUTC: boolean = false;

  protected override _enableResizeObserver = true;

  private _seriesElements: RenderedSeries[] = [];
  private _activeGroupedTheta?: string | number;
  private _showGroupedCallout?: (theta: string | number) => void;

  public connectedCallback() {
    const self = this as Record<string, unknown>;
    const attrFields = [
      'data',
      'showMarkers',
      'enableMultiValueCallout',
      'shape',
      'direction',
      'hole',
      'radialAxis',
      'angularAxis',
      'margins',
      'dateLocalizeOptions',
      'useUTC',
    ] as const;
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

  protected showMarkersChanged() {
    this._requestRender();
  }

  protected enableMultiValueCalloutChanged() {
    this._requestRender();
  }

  protected shapeChanged() {
    this._requestRender();
  }

  protected directionChanged() {
    this._requestRender();
  }

  protected holeChanged() {
    this._requestRender();
  }

  protected radialAxisChanged() {
    this._requestRender();
  }

  protected angularAxisChanged() {
    this._requestRender();
  }

  protected marginsChanged() {
    this._requestRender();
  }

  protected dateLocalizeOptionsChanged() {
    this._requestRender();
  }

  protected useUTCChanged() {
    this._requestRender();
  }

  protected override tooltipPropsChanged(oldValue: TooltipProps, newValue: PolarTooltipState): void {
    super.tooltipPropsChanged(oldValue, newValue);
    if (newValue.isVisible && this.enableMultiValueCallout && newValue.entries) {
      this.liveRegionText = [newValue.angularLabel, ...newValue.entries.map(entry => `${entry.legend}: ${entry.value}`)]
        .filter(Boolean)
        .join(', ');
    }
  }

  protected override _performRender(): void {
    if (!this.$fastController.isConnected || !this.chartContainer) {
      return;
    }

    this._applyHostDimensions(this.width, this.height);
    this._clearChart();
    this._tooltipTransform = this._isRTL ? 'translateX(50%)' : 'translateX(-50%)';

    const series = normalizeSeries(this.data ?? []);
    const allPoints = series.flatMap(entry => entry.data);

    this.legends = series.map((entry, index) => ({
      legend: entry.legend,
      color: entry.color ? getColorFromToken(entry.color) : getNextColor(index, 0),
    }));
    this._updateLegendInteractionState();

    if (series.length === 0 || allPoints.length === 0) {
      this.elementInternals.ariaLabel = this._getHostAriaLabel();
      return;
    }

    const width = this.chartContainer.getBoundingClientRect().width || toNumber(this.width, 400);
    const height = this.chartContainer.getBoundingClientRect().height || toNumber(this.height, 400);
    const margins = { top: 32, right: 56, bottom: 48, left: 56, ...this.margins };
    const innerWidth = Math.max(width - margins.left - margins.right, 1);
    const innerHeight = Math.max(height - margins.top - margins.bottom, 1);
    const centerX = margins.left + innerWidth / 2;
    const centerY = margins.top + innerHeight / 2;
    const radius = Math.max(Math.min(innerWidth, innerHeight) / 2, 1);
    const holeRatio = Math.min(Math.abs(toNumber(this.hole, 0)), 1);
    const innerRadius = radius * holeRatio;
    const radialScale = createRadialScale(
      allPoints,
      this.radialAxis,
      [innerRadius, radius],
      this.culture,
      this.useUTC,
      this.dateLocalizeOptions,
    );
    const angularScale = createAngularScale(allPoints, this.angularAxis, this.direction);

    const svg = createSvgElement<SVGSVGElement>('svg');
    svg.classList.add('chart');
    svg.setAttribute('width', String(width));
    svg.setAttribute('height', String(height));
    svg.setAttribute('viewBox', `0 0 ${width} ${height}`);

    const seriesLayer = createSvgElement<SVGGElement>('g');
    seriesLayer.classList.add('polar-series-layer');
    const axisLayer = createSvgElement<SVGGElement>('g');
    axisLayer.classList.add('polar-axis-layer');
    const markerLayer = createSvgElement<SVGGElement>('g');
    markerLayer.classList.add('polar-marker-layer');
    svg.append(seriesLayer, axisLayer, markerLayer);

    const guideLine = createSvgElement<SVGLineElement>('line');
    guideLine.classList.add('polar-callout-guide');
    guideLine.style.display = 'none';
    markerLayer.appendChild(guideLine);

    const interactionSurface = createSvgElement<SVGCircleElement>('circle');
    interactionSurface.classList.add('polar-callout-surface');
    interactionSurface.setAttribute('cx', String(centerX));
    interactionSurface.setAttribute('cy', String(centerY));
    interactionSurface.setAttribute('r', String(radius));
    interactionSurface.style.display = this.enableMultiValueCallout ? '' : 'none';
    markerLayer.appendChild(interactionSurface);

    const angularTicks = angularScale.tickValues.flatMap((value, index) => {
      const angle = angularScale.map(value);
      return angle === undefined ? [] : [{ value, label: angularScale.tickLabels[index], angle }];
    });

    const gridRadii = [...radialScale.tickValues.map(radialScale.map), radius]
      .filter((value): value is number => value !== undefined)
      .filter(
        (currentRadius, index, values) => values.findIndex(value => Math.abs(value - currentRadius) < 0.01) === index,
      );
    gridRadii.forEach((currentRadius, index) => {
      const isOuter = index === gridRadii.length - 1;
      if (this.shape === 'polygon') {
        const polygon = createSvgElement<SVGPolygonElement>('polygon');
        polygon.classList.add('polar-grid', ...(isOuter ? ['polar-grid-outer'] : []));
        polygon.setAttribute(
          'points',
          angularTicks
            .map(point => {
              const [x, y] = pointRadial(point.angle, currentRadius);
              return `${centerX + x},${centerY + y}`;
            })
            .join(' '),
        );
        axisLayer.appendChild(polygon);
      } else {
        const circle = createSvgElement<SVGCircleElement>('circle');
        circle.classList.add('polar-grid', ...(isOuter ? ['polar-grid-outer'] : []));
        circle.setAttribute('cx', String(centerX));
        circle.setAttribute('cy', String(centerY));
        circle.setAttribute('r', String(currentRadius));
        axisLayer.appendChild(circle);
      }
    });

    angularTicks.forEach(point => {
      const [axisX, axisY] = pointRadial(point.angle, radius);
      const axis = createSvgElement<SVGLineElement>('line');
      axis.classList.add('polar-axis');
      axis.setAttribute('x1', String(centerX));
      axis.setAttribute('y1', String(centerY));
      axis.setAttribute('x2', String(centerX + axisX));
      axis.setAttribute('y2', String(centerY + axisY));
      axisLayer.appendChild(axis);

      const label = createSvgElement<SVGTextElement>('text');
      label.classList.add('polar-axis-label');
      const [labelX, labelY] = pointRadial(point.angle, radius + 20);
      label.setAttribute('x', String(centerX + labelX));
      label.setAttribute('y', String(centerY + labelY));
      label.textContent = point.label;
      axisLayer.appendChild(label);
    });

    const radialAxisAngle = this.direction === 'clockwise' ? 0 : Math.PI / 2;
    const [radialStartX, radialStartY] = pointRadial(radialAxisAngle, innerRadius);
    const [radialEndX, radialEndY] = pointRadial(radialAxisAngle, radius);
    const radialAxis = createSvgElement<SVGLineElement>('line');
    radialAxis.classList.add('polar-radial-axis');
    radialAxis.setAttribute('x1', String(centerX + radialStartX));
    radialAxis.setAttribute('y1', String(centerY + radialStartY));
    radialAxis.setAttribute('x2', String(centerX + radialEndX));
    radialAxis.setAttribute('y2', String(centerY + radialEndY));
    axisLayer.appendChild(radialAxis);

    radialScale.tickValues.forEach((tick, index) => {
      const tickRadius = radialScale.map(tick);
      if (tickRadius === undefined) {
        return;
      }
      const [relativeTickX, relativeTickY] = pointRadial(radialAxisAngle, tickRadius);
      const tickX = centerX + relativeTickX;
      const tickY = centerY + relativeTickY;
      const tickOffsetX = Math.cos(radialAxisAngle) * 3;
      const tickOffsetY = Math.sin(radialAxisAngle) * 3;
      const tickMark = createSvgElement<SVGLineElement>('line');
      tickMark.classList.add('polar-radial-tick');
      tickMark.setAttribute('x1', String(tickX - tickOffsetX));
      tickMark.setAttribute('x2', String(tickX + tickOffsetX));
      tickMark.setAttribute('y1', String(tickY - tickOffsetY));
      tickMark.setAttribute('y2', String(tickY + tickOffsetY));
      axisLayer.appendChild(tickMark);

      const tickLabel = createSvgElement<SVGTextElement>('text');
      tickLabel.classList.add('polar-radial-tick-label');
      tickLabel.setAttribute('x', String(this.direction === 'clockwise' ? tickX - 10 : tickX));
      tickLabel.setAttribute('y', String(this.direction === 'clockwise' ? tickY : tickY + 12));
      tickLabel.setAttribute('text-anchor', this.direction === 'clockwise' ? 'end' : 'middle');
      tickLabel.textContent = radialScale.tickLabels[index];
      axisLayer.appendChild(tickLabel);
    });

    const markerSizes = allPoints
      .map(point => point.source.markerSize)
      .filter((value): value is number => value !== undefined);
    const [minMarkerSize = 0, maxMarkerSize = 0] = extent(markerSizes);
    const markersOnlyMode = series.every(entry => entry.type === 'scatterpolar');
    const renderedPoints: Array<{
      point: NormalizedPolarPoint;
      legend: string;
      color: string;
      marker: SVGCircleElement;
    }> = [];
    const angularKey = (value: string | number): string => `${typeof value}:${String(value)}`;
    const clearGroupedCallout = (): void => {
      this._activeGroupedTheta = undefined;
      guideLine.style.display = 'none';
      renderedPoints.forEach(renderedPoint => renderedPoint.marker.classList.remove('active'));
      this._clearTooltip();
    };
    const showGroupedCallout = (theta: string | number): void => {
      const angle = angularScale.map(theta);
      if (angle === undefined || this.hideTooltip) {
        return;
      }
      const points = renderedPoints.filter(
        renderedPoint =>
          angularKey(renderedPoint.point.theta) === angularKey(theta) && this._shouldShowTooltip(renderedPoint.legend),
      );
      if (points.length === 0) {
        clearGroupedCallout();
        return;
      }

      this._activeGroupedTheta = theta;

      renderedPoints.forEach(renderedPoint =>
        renderedPoint.marker.classList.toggle('active', points.includes(renderedPoint)),
      );
      const [guideStartX, guideStartY] = pointRadial(angle, innerRadius);
      const [guideEndX, guideEndY] = pointRadial(angle, radius);
      guideLine.setAttribute('x1', String(centerX + guideStartX));
      guideLine.setAttribute('y1', String(centerY + guideStartY));
      guideLine.setAttribute('x2', String(centerX + guideEndX));
      guideLine.setAttribute('y2', String(centerY + guideEndY));
      guideLine.style.display = '';

      const angularLabel = points[0].point.source.angularAxisCalloutData ?? formatPolarAngle(theta, this.angularAxis);
      const entries = points.map<PolarTooltipEntry>(renderedPoint => ({
        legend: renderedPoint.legend,
        color: renderedPoint.color,
        value:
          renderedPoint.point.source.radialAxisCalloutData ??
          formatPolarRadialValue(renderedPoint.point.r, this.culture, this.useUTC, this.dateLocalizeOptions),
      }));
      const rootBounds = this.getBoundingClientRect();
      const svgBounds = svg.getBoundingClientRect();
      const svgLeft = svgBounds.left - rootBounds.left;
      const svgTop = svgBounds.top - rootBounds.top;
      const scaleX = svgBounds.width / width;
      const scaleY = svgBounds.height / height;
      const originX = svgLeft + centerX * scaleX;
      const preferredSide =
        guideEndX > 0 ? 'left' : guideEndX < 0 ? 'right' : this.offsetWidth - originX >= originX ? 'right' : 'left';
      const anchorX = originX;
      const markerBounds = points.map(renderedPoint => renderedPoint.marker.getBoundingClientRect());
      const topY = Math.min(...markerBounds.map(bounds => bounds.top - rootBounds.top));
      const bottomY = Math.max(...markerBounds.map(bounds => bounds.bottom - rootBounds.top));
      const anchorY = svgTop + (centerY + guideEndY) * scaleY;
      const isFreshShow = !this.tooltipProps.isVisible || this.tooltipProps.angularLabel !== angularLabel;
      this._currentTooltipDataPoint = {
        theta,
        points: points.map(renderedPoint => renderedPoint.point.source),
      };
      this.tooltipProps = {
        isVisible: true,
        legend: angularLabel,
        yValue: entries[0].value,
        color: entries[0].color,
        angularLabel,
        entries,
        xPos: anchorX,
        yPos: Math.max(anchorY, 0),
      };
      this._positionTooltipAvoidingOverlap(anchorX, topY, bottomY, isFreshShow, {
        horizontalPlacement: 'side',
        preferredHorizontalSide: preferredSide,
        gap: 12,
      });
    };
    this._showGroupedCallout = showGroupedCallout;

    this._seriesElements = series.map((entry, seriesIndex) => {
      const color = entry.color ? getColorFromToken(entry.color) : getNextColor(seriesIndex, 0);
      const isPlottable = (point: NormalizedPolarPoint): boolean =>
        angularScale.map(point.theta) !== undefined && radialScale.map(point.r) !== undefined;
      const shapes: SVGPathElement[] = [];
      const labels: SVGTextElement[] = [];

      if (entry.type === 'areapolar') {
        const area = areaRadial<NormalizedPolarPoint>()
          .angle(point => angularScale.map(point.theta)!)
          .innerRadius(innerRadius)
          .outerRadius(point => radialScale.map(point.r)!)
          .curve(getCurveFactory(entry.lineOptions?.curve, true))
          .defined(isPlottable);
        const areaPath = createSvgElement<SVGPathElement>('path');
        areaPath.classList.add('polar-series', 'polar-area');
        areaPath.dataset.legend = entry.legend;
        areaPath.setAttribute('d', area(entry.data) ?? '');
        areaPath.setAttribute('fill', color);
        areaPath.setAttribute('transform', `translate(${centerX}, ${centerY})`);
        seriesLayer.appendChild(areaPath);
        shapes.push(areaPath);
      }

      if (entry.type !== 'scatterpolar') {
        const line = lineRadial<NormalizedPolarPoint>()
          .angle(point => angularScale.map(point.theta)!)
          .radius(point => radialScale.map(point.r)!)
          .curve(getCurveFactory(entry.lineOptions?.curve))
          .defined(isPlottable);
        const linePath = createSvgElement<SVGPathElement>('path');
        linePath.classList.add('polar-series', 'polar-line');
        linePath.dataset.legend = entry.legend;
        linePath.setAttribute('d', line(entry.data) ?? '');
        linePath.setAttribute('fill', 'none');
        linePath.setAttribute('stroke', color);
        linePath.setAttribute('stroke-width', String(entry.lineOptions?.strokeWidth ?? 3));
        if (entry.lineOptions?.strokeDasharray !== undefined) {
          linePath.setAttribute('stroke-dasharray', String(entry.lineOptions.strokeDasharray));
        }
        if (entry.lineOptions?.strokeDashoffset !== undefined) {
          linePath.setAttribute('stroke-dashoffset', String(entry.lineOptions.strokeDashoffset));
        }
        if (entry.lineOptions?.strokeLinecap) {
          linePath.setAttribute('stroke-linecap', entry.lineOptions.strokeLinecap);
        }
        linePath.setAttribute('transform', `translate(${centerX}, ${centerY})`);
        seriesLayer.appendChild(linePath);
        shapes.push(linePath);
      }

      const markers: SVGCircleElement[] = [];
      entry.data.forEach((point, pointIndex) => {
        const angle = angularScale.map(point.theta);
        const pointRadius = radialScale.map(point.r);
        if (angle === undefined || pointRadius === undefined) {
          return;
        }
        const [relativeX, relativeY] = pointRadial(angle, pointRadius);
        const pointColor = point.source.color ? getColorFromToken(point.source.color) : color;
        const minimumRadius = markersOnlyMode ? 4 : 6;
        const markerRadius =
          point.source.markerSize !== undefined && maxMarkerSize !== minMarkerSize
            ? minimumRadius +
              ((point.source.markerSize - minMarkerSize) / (maxMarkerSize - minMarkerSize)) * (16 - minimumRadius)
            : minimumRadius;
        const marker = createSvgElement<SVGCircleElement>('circle');
        marker.classList.add('polar-marker');
        marker.classList.toggle('always-visible', this.showMarkers || entry.type === 'scatterpolar');
        marker.dataset.legend = entry.legend;
        marker.setAttribute('cx', String(centerX + relativeX));
        marker.setAttribute('cy', String(centerY + relativeY));
        marker.setAttribute('r', String(markerRadius));
        marker.setAttribute('fill', pointColor);
        marker.setAttribute('stroke', pointColor);
        marker.setAttribute('role', 'img');
        marker.setAttribute('tabindex', seriesIndex === 0 && pointIndex === 0 ? '0' : '-1');
        marker.setAttribute(
          'aria-label',
          point.source.callOutAccessibilityData?.ariaLabel ??
            `${String(point.r)}. ${entry.legend}, ${String(point.theta)}.`,
        );
        if (point.source.onClick) {
          marker.classList.add('clickable');
        }
        marker.addEventListener('click', () => {
          this._focusRovingElement(this._getRovingMarkers(), marker);
          point.source.onClick?.();
        });

        renderedPoints.push({ point, legend: entry.legend, color: pointColor, marker });

        const showTooltip = (): void => {
          if (!this._shouldShowTooltip(entry.legend) || this.hideTooltip) {
            return;
          }
          if (this.enableMultiValueCallout) {
            showGroupedCallout(point.theta);
            return;
          }
          marker.classList.add('active');
          const rootBounds = this.getBoundingClientRect();
          const markerBounds = marker.getBoundingClientRect();
          const anchorX = markerBounds.left - rootBounds.left + markerBounds.width / 2;
          const anchorY = markerBounds.top - rootBounds.top;
          this._currentTooltipDataPoint = point.source;
          this.tooltipProps = {
            isVisible: true,
            legend: point.source.angularAxisCalloutData ?? formatPolarAngle(point.theta, this.angularAxis),
            yValue:
              point.source.radialAxisCalloutData ??
              formatPolarRadialValue(point.r, this.culture, this.useUTC, this.dateLocalizeOptions),
            color: pointColor,
            xPos: anchorX,
            yPos: Math.max(anchorY, 0),
          };
          this._positionTooltipFromAnchor(anchorX, anchorY, { preferredVertical: 'above', horizontalAlign: 'center' });
        };
        const hideTooltip = (): void => {
          if (this.enableMultiValueCallout) {
            clearGroupedCallout();
          } else {
            marker.classList.remove('active');
            this._clearTooltip();
          }
        };
        marker.addEventListener('mouseenter', showTooltip);
        marker.addEventListener('focus', showTooltip);
        marker.addEventListener('mouseleave', hideTooltip);
        marker.addEventListener('blur', hideTooltip);
        marker.addEventListener('keydown', event => {
          this._rovingKeydown(this._getRovingMarkers(), event);
        });
        markerLayer.appendChild(marker);
        markers.push(marker);

        if (point.source.text) {
          const text = createSvgElement<SVGTextElement>('text');
          text.classList.add('polar-point-text');
          text.dataset.legend = entry.legend;
          text.setAttribute('x', String(centerX + relativeX));
          text.setAttribute('y', String(centerY + relativeY - markerRadius - 4));
          text.textContent = point.source.text;
          markerLayer.appendChild(text);
          labels.push(text);
        }
      });

      return { legend: entry.legend, shapes, markers, labels };
    });

    const angularGroups = [
      ...new Map(renderedPoints.map(item => [angularKey(item.point.theta), item.point.theta])).values(),
    ];
    interactionSurface.addEventListener('mousemove', event => {
      if (!this.enableMultiValueCallout || angularGroups.length === 0) {
        return;
      }
      const bounds = svg.getBoundingClientRect();
      const pointerX = ((event.clientX - bounds.left) / bounds.width) * width - centerX;
      const pointerY = ((event.clientY - bounds.top) / bounds.height) * height - centerY;
      const pointerAngle = (Math.atan2(pointerX, -pointerY) + Math.PI * 2) % (Math.PI * 2);
      const nearestTheta = angularGroups.reduce((nearest, candidate) => {
        const candidateAngle = angularScale.map(candidate) ?? 0;
        const nearestAngle = angularScale.map(nearest) ?? 0;
        const distance = Math.abs(
          Math.atan2(Math.sin(candidateAngle - pointerAngle), Math.cos(candidateAngle - pointerAngle)),
        );
        const nearestDistance = Math.abs(
          Math.atan2(Math.sin(nearestAngle - pointerAngle), Math.cos(nearestAngle - pointerAngle)),
        );
        return distance < nearestDistance ? candidate : nearest;
      });
      showGroupedCallout(nearestTheta);
    });
    interactionSurface.addEventListener('mouseleave', clearGroupedCallout);

    this.chartContainer.appendChild(svg);
    this._applyActiveLegendState();
    this.elementInternals.ariaLabel = this._getHostAriaLabel();
  }

  protected override _applyActiveLegendState(): void {
    if (!this._seriesElements) {
      return;
    }

    const highlighted = this._getHighlightedLegends();
    const hasSelection = highlighted.length > 0;

    for (const series of this._seriesElements) {
      const isActive = !hasSelection || highlighted.includes(series.legend);
      series.shapes.forEach(shape => shape.classList.toggle('inactive', !isActive));
      series.markers.forEach(marker => {
        marker.classList.toggle('inactive', !isActive);
      });
      series.labels.forEach(label => label.classList.toggle('inactive', !isActive));
    }

    const rovingMarkers = this._getRovingMarkers();
    const currentMarker = rovingMarkers.find(marker => marker.tabIndex === 0) ?? rovingMarkers[0];
    this._seriesElements
      .flatMap(series => series.markers)
      .forEach(marker => marker.setAttribute('tabindex', marker === currentMarker ? '0' : '-1'));

    if (this.enableMultiValueCallout && this._activeGroupedTheta !== undefined) {
      this._showGroupedCallout?.(this._activeGroupedTheta);
    }
  }

  protected override _getHostAriaLabel(): string {
    const count = this.data?.length ?? 0;
    return `Polar chart with ${count} series.`;
  }

  protected override _buildDefaultTooltipHTML(dataPoint: unknown): string {
    const entries = this.tooltipProps.entries;
    if (!this.enableMultiValueCallout || !entries) {
      return super._buildDefaultTooltipHTML(dataPoint);
    }

    return [
      `<div class="tooltip-header">${escapeHtml(this.tooltipProps.angularLabel ?? '')}</div>`,
      ...entries.map(entry =>
        [
          `<div class="tooltip-inner" style="border-color: ${escapeHtml(entry.color)};">`,
          `<div class="tooltip-legend-text">${escapeHtml(entry.legend)}</div>`,
          `<div class="tooltip-content-y" style="color: ${escapeHtml(entry.color)};">${escapeHtml(entry.value)}</div>`,
          `</div>`,
        ].join(''),
      ),
    ].join('');
  }

  private _clearChart(): void {
    this._seriesElements = [];
    this._activeGroupedTheta = undefined;
    this._showGroupedCallout = undefined;
    this._clearTooltip();

    if (!this.chartContainer) {
      return;
    }

    while (this.chartContainer.firstChild) {
      this.chartContainer.firstChild.remove();
    }
  }

  private _getRovingMarkers(): SVGCircleElement[] {
    return this._seriesElements
      .flatMap(series => series.markers)
      .filter(marker => !marker.classList.contains('inactive'));
  }
}
