import { css } from '@microsoft/fast-element';
import { chartTitleStyles } from '../utils/chart-title.styles.js';
import {
  colorNeutralForeground1,
  display,
  spacingVerticalS,
  typographyBody1StrongStyles,
  typographyBody1Styles,
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
    grid-template-columns: 1fr;
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

  .tree-link {
    fill: none;
    stroke: #999;
    stroke-width: 1.5;
  }

  .tree-node {
    rx: 4;
    cursor: default;
  }

  .tree-node-label {
    font-size: 12px;
    pointer-events: none;
    fill: #fff;
    text-anchor: middle;
    dominant-baseline: middle;
  }

  .tree-node-subname {
    font-size: 10px;
    pointer-events: none;
    fill: rgba(255, 255, 255, 0.8);
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
`;
