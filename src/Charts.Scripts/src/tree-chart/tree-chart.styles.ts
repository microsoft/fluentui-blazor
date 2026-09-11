import { css } from '@microsoft/fast-element';
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
      'chart';
    grid-template-columns: 1fr;
    grid-template-rows: auto 1fr;
    width: 100%;
    height: 100%;
    position: relative;
  }

  .chart-title {
    grid-area: title;
    margin-bottom: ${spacingVerticalS};
    ${typographyBody1StrongStyles}
    color: ${colorNeutralForeground1};
    text-align: start;
  }

  .chart-container {
    grid-area: chart;
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
