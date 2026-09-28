import type { ChartAnnotation } from './chart-options.js';
import { getColorFromToken, SVG_NAMESPACE_URI } from './chart-helpers.js';

export interface RenderChartAnnotationsOptions {
  layer: SVGGElement;
  collisionLayer?: SVGGElement;
  annotations: ChartAnnotation[] | undefined;
  innerWidth: number;
  innerHeight: number;
  mapDataX: (value: number | string | Date) => number | undefined;
  mapDataY: (value: number | string | Date, axis: 'primary' | 'secondary') => number | undefined;
}

const estimateTextWidth = (text: SVGTextElement, fontSize: number): number =>
  text.getComputedTextLength() || (text.textContent?.length ?? 0) * fontSize * 0.6;

export const renderChartAnnotations = ({
  layer,
  collisionLayer = layer,
  annotations,
  innerWidth,
  innerHeight,
  mapDataX,
  mapDataY,
}: RenderChartAnnotationsOptions): void => {
  annotations?.forEach((annotation, index) => {
    const { coordinates } = annotation;
    const xValue = Number(coordinates.x);
    const yValue = Number(coordinates.y);
    const anchorX =
      coordinates.type === 'data'
        ? mapDataX(coordinates.x)
        : coordinates.type === 'relative'
        ? xValue * innerWidth
        : xValue;
    const anchorY =
      coordinates.type === 'data'
        ? mapDataY(coordinates.y, coordinates.yAxis ?? 'primary')
        : coordinates.type === 'relative'
        ? yValue * innerHeight
        : yValue;
    if (anchorX === undefined || anchorY === undefined || !Number.isFinite(anchorX) || !Number.isFinite(anchorY)) {
      return;
    }

    const offsetX = annotation.layout?.offsetX ?? 0;
    const offsetY = annotation.layout?.offsetY ?? -8;
    const textX = anchorX + offsetX;
    let textY = anchorY + offsetY;
    const group = document.createElementNS(SVG_NAMESPACE_URI, 'g');
    group.classList.add('chart-annotation');
    group.dataset.annotationId = annotation.id ?? String(index);
    group.setAttribute('role', annotation.accessibility?.role ?? 'note');
    group.setAttribute('aria-label', annotation.accessibility?.ariaLabel ?? annotation.text);
    group.setAttribute('opacity', String(annotation.style?.opacity ?? 1));

    let connector: SVGLineElement | undefined;
    if (annotation.connector && (offsetX !== 0 || offsetY !== 0)) {
      connector = document.createElementNS(SVG_NAMESPACE_URI, 'line');
      connector.classList.add('chart-annotation-connector');
      connector.setAttribute('x1', String(anchorX));
      connector.setAttribute('y1', String(anchorY));
      connector.setAttribute('x2', String(textX));
      connector.setAttribute('y2', String(textY));
      connector.setAttribute('stroke', getColorFromToken(annotation.connector.strokeColor ?? 'colorNeutralStroke1'));
      connector.setAttribute('stroke-width', String(annotation.connector.strokeWidth ?? 1));
      if (annotation.connector.dashArray) {
        connector.setAttribute('stroke-dasharray', annotation.connector.dashArray);
      }
      if (annotation.connector.arrow) {
        const marker = document.createElementNS(SVG_NAMESPACE_URI, 'marker');
        const markerId = `chart-annotation-arrow-${annotation.id ?? index}`;
        marker.setAttribute('id', markerId);
        marker.setAttribute('markerWidth', '7');
        marker.setAttribute('markerHeight', '7');
        marker.setAttribute('refX', '1');
        marker.setAttribute('refY', '3.5');
        marker.setAttribute('orient', 'auto');
        const arrowhead = document.createElementNS(SVG_NAMESPACE_URI, 'path');
        arrowhead.setAttribute('d', 'M 7 0 L 0 3.5 L 7 7 z');
        arrowhead.setAttribute('fill', getColorFromToken(annotation.connector.strokeColor ?? 'colorNeutralStroke1'));
        marker.appendChild(arrowhead);
        group.appendChild(marker);
        connector.setAttribute('marker-start', `url(#${markerId})`);
      }
      group.appendChild(connector);
    }

    const text = document.createElementNS(SVG_NAMESPACE_URI, 'text');
    text.classList.add('chart-annotation-text');
    text.setAttribute('x', String(textX));
    text.setAttribute('y', String(textY));
    text.setAttribute(
      'text-anchor',
      annotation.layout?.align === 'start' ? 'start' : annotation.layout?.align === 'end' ? 'end' : 'middle',
    );
    text.setAttribute(
      'dominant-baseline',
      annotation.layout?.verticalAlign === 'top'
        ? 'hanging'
        : annotation.layout?.verticalAlign === 'bottom'
        ? 'auto'
        : 'middle',
    );
    text.setAttribute('fill', getColorFromToken(annotation.style?.textColor ?? 'colorNeutralForeground2'));
    text.setAttribute('font-size', annotation.style?.fontSize ?? '10px');
    text.setAttribute('font-weight', String(annotation.style?.fontWeight ?? 600));
    if (annotation.layout?.rotation) {
      text.setAttribute('transform', `rotate(${annotation.layout.rotation} ${textX} ${textY})`);
    }
    if (annotation.textLines) {
      annotation.textLines.forEach((line, lineIndex) => {
        const lineElement = document.createElementNS(SVG_NAMESPACE_URI, 'tspan');
        lineElement.classList.add('chart-annotation-line');
        lineElement.setAttribute('x', String(textX + (line.indent ?? 0)));
        if (lineIndex > 0) {
          lineElement.setAttribute('dy', '1.2em');
        }
        if (line.bullet) {
          lineElement.append('• ');
        }
        line.runs.forEach(run => {
          const runElement = document.createElementNS(SVG_NAMESPACE_URI, 'tspan');
          runElement.classList.add('chart-annotation-run');
          runElement.textContent = run.text;
          if (run.textColor) {
            runElement.setAttribute('fill', getColorFromToken(run.textColor));
          }
          if (run.fontWeight !== undefined) {
            runElement.setAttribute('font-weight', String(run.fontWeight));
          }
          if (run.fontStyle) {
            runElement.setAttribute('font-style', run.fontStyle);
          }
          lineElement.appendChild(runElement);
        });
        text.appendChild(lineElement);
      });
    } else {
      text.textContent = annotation.text;
    }
    group.appendChild(text);
    layer.appendChild(group);

    const annotationFontSize = Number.parseFloat(text.getAttribute('font-size') ?? '') || 10;
    const annotationWidth = estimateTextWidth(text, annotationFontSize);
    const annotationAnchor = text.getAttribute('text-anchor');
    const annotationLeft =
      annotationAnchor === 'start'
        ? textX
        : annotationAnchor === 'end'
        ? textX - annotationWidth
        : textX - annotationWidth / 2;
    const annotationRight = annotationLeft + annotationWidth;
    const collidingLabel = Array.from(collisionLayer.querySelectorAll<SVGTextElement>('.bar-label')).find(label => {
      const labelX = Number(label.getAttribute('x'));
      const labelY = Number(label.getAttribute('y'));
      const labelFontSize = Number.parseFloat(label.getAttribute('font-size') ?? '') || 10;
      const labelWidth = estimateTextWidth(label, labelFontSize);
      const labelLeft = labelX - labelWidth / 2;
      const labelRight = labelX + labelWidth / 2;
      const overlapsHorizontally = annotationLeft < labelRight + 4 && annotationRight + 4 > labelLeft;
      const overlapsVertically =
        textY - annotationFontSize / 2 < labelY + 2 && textY + annotationFontSize / 2 + 4 > labelY - labelFontSize;
      return overlapsHorizontally && overlapsVertically;
    });
    if (collidingLabel) {
      const labelY = Number(collidingLabel.getAttribute('y'));
      const labelFontSize = Number.parseFloat(collidingLabel.getAttribute('font-size') ?? '') || 10;
      const candidateTextY = labelY - labelFontSize - 3 - annotationFontSize / 2;
      const plotOffsetY = layer.transform.baseVal.consolidate()?.matrix.f ?? 0;
      if (candidateTextY - annotationFontSize / 2 < -plotOffsetY) {
        return;
      }

      textY = candidateTextY;
      text.setAttribute('y', String(textY));
      if (annotation.layout?.rotation) {
        text.setAttribute('transform', `rotate(${annotation.layout.rotation} ${textX} ${textY})`);
      }
      if (connector) {
        connector.setAttribute('x1', String(anchorX));
        connector.setAttribute('y1', String(labelY - labelFontSize));
        connector.setAttribute('x2', String(textX));
        connector.setAttribute('y2', String(textY));
      }
    }
  });
};
