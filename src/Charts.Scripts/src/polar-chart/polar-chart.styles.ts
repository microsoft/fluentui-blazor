import { css } from '@microsoft/fast-element';
import { chartTitleStyles } from '../utils/chart-title.styles.js';
import {
  borderRadiusMedium,
  colorNeutralBackground1,
  colorNeutralForeground1,
  colorNeutralShadowAmbient,
  colorNeutralShadowKey,
  colorTransparentStroke,
  display,
  spacingHorizontalL,
  spacingHorizontalS,
  spacingVerticalMNudge,
  spacingVerticalS,
  typographyBody1StrongStyles,
  typographyBody1Styles,
  typographyCaption1Styles,
} from '@fluentui/web-components';

export const styles = css`
  ${display('block')}

  :host {
    ${typographyBody1Styles}
    display: grid;
    grid-template-areas:
      'title'
      'chart'
      'legend';
    grid-template-columns: 1fr;
    grid-template-rows: auto 1fr auto;
    width: 100%;
    height: 100%;
    position: relative;
  }

  ${chartTitleStyles}

  .chart-container {
    grid-area: chart;
    min-inline-size: 0;
  }

  fluent-chart-legend {
    grid-area: legend;
  }

  :host([title-position='bottom']) {
    grid-template-areas:
      'chart'
      'legend'
      'title';
  }

  :host([legend-position='top']) {
    grid-template-areas:
      'title'
      'legend'
      'chart';
  }

  :host([legend-position='start']) {
    grid-template-areas:
      'title  title'
      'legend chart';
    grid-template-columns: auto 1fr;
  }

  :host([legend-position='end']) {
    grid-template-areas:
      'title  title'
      'chart  legend';
    grid-template-columns: 1fr auto;
  }

  :host([legend-position='start']) fluent-chart-legend,
  :host([legend-position='end']) fluent-chart-legend {
    align-self: start;
  }

  :host([title-position='bottom'][legend-position='top']) {
    grid-template-areas:
      'legend'
      'chart'
      'title';
  }

  :host([title-position='bottom'][legend-position='start']) {
    grid-template-areas:
      'legend chart'
      'title  title';
    grid-template-columns: auto 1fr;
  }

  :host([title-position='bottom'][legend-position='end']) {
    grid-template-areas:
      'chart  legend'
      'title  title';
    grid-template-columns: 1fr auto;
  }

  .chart {
    display: block;
    overflow: visible;
  }

  .polar-series {
    pointer-events: none;
  }

  .polar-area {
    fill-opacity: 0.7;
  }

  .polar-series.inactive {
    opacity: 0.1;
  }

  .polar-grid {
    fill: none;
    opacity: 0.2;
    stroke: ${colorNeutralForeground1};
    stroke-width: 1;
  }

  .polar-grid-outer,
  .polar-radial-axis,
  .polar-radial-tick {
    opacity: 1;
    stroke: ${colorNeutralForeground1};
    stroke-width: 1;
  }

  .polar-axis {
    opacity: 0.2;
    stroke: ${colorNeutralForeground1};
    stroke-width: 1;
  }

  .polar-axis-label {
    font-size: 11px;
    fill: ${colorNeutralForeground1};
    font-weight: 600;
    text-anchor: middle;
  }

  .polar-radial-tick-label {
    fill: ${colorNeutralForeground1};
    font-size: 10px;
    font-weight: 600;
    dominant-baseline: middle;
  }

  .polar-marker {
    opacity: 0;
    stroke-width: 0;
  }

  .polar-marker.inactive {
    opacity: 0;
    pointer-events: none;
  }

  .polar-marker.always-visible {
    opacity: 1;
  }

  .polar-marker.always-visible.inactive,
  .polar-point-text.inactive {
    opacity: 0.1;
  }

  .polar-marker.active {
    fill: ${colorNeutralBackground1};
    opacity: 1;
    stroke-width: 2;
  }

  .polar-marker.clickable {
    cursor: pointer;
  }

  .polar-callout-guide {
    stroke: ${colorNeutralForeground1};
    stroke-dasharray: 5 3;
    stroke-width: 1;
    pointer-events: none;
  }

  .polar-callout-surface {
    fill: ${colorNeutralBackground1};
    fill-opacity: 0;
    pointer-events: all;
  }

  .polar-point-text {
    fill: ${colorNeutralForeground1};
    font-size: 10px;
    pointer-events: none;
    text-anchor: middle;
  }

  .live-region {
    position: absolute;
    inline-size: 1px;
    block-size: 1px;
    padding: 0;
    overflow: hidden;
    clip-path: inset(50%);
    white-space: nowrap;
    pointer-events: none;
  }

  .tooltip {
    position: absolute;
    display: grid;
    overflow: hidden;
    padding: ${spacingVerticalMNudge} ${spacingHorizontalL};
    background: ${colorNeutralBackground1};
    pointer-events: none;
    z-index: 1;
    border-radius: ${borderRadiusMedium};
    border: 1px solid ${colorTransparentStroke};
    filter: drop-shadow(0 0 2px ${colorNeutralShadowAmbient}) drop-shadow(0 8px 16px ${colorNeutralShadowKey});
    min-width: 120px;
    transform: translateX(-50%);
  }

  .tooltip.measuring {
    visibility: hidden;
  }

  .tooltip-header {
    ${typographyCaption1Styles}
    color: ${colorNeutralForeground1};
    margin-bottom: ${spacingVerticalS};
    padding-bottom: ${spacingVerticalS};
    border-bottom: 1px solid ${colorTransparentStroke};
  }

  .tooltip-inner {
    padding-inline-start: ${spacingHorizontalS};
    border-inline-start: 4px solid;
    margin-block: ${spacingVerticalS};
  }

  .tooltip-legend-text {
    ${typographyCaption1Styles}
    color: ${colorNeutralForeground1};
  }

  .tooltip-content-y {
    ${typographyBody1StrongStyles}
  }
`;
