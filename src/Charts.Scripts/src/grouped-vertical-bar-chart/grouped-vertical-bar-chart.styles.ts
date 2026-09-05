import type { ElementStyles } from '@microsoft/fast-element';
import { css } from '@microsoft/fast-element';
import {
  colorNeutralForeground1,
  colorNeutralForeground2,
  colorNeutralStroke1,
  display,
  fontSizeBase500,
  shadow4,
  spacingHorizontalS,
  spacingVerticalXS,
  strokeWidthThick,
  strokeWidthThickest,
  typographyBody1StrongStyles,
  typographyCaption1Styles,
  typographySubtitle2StrongerStyles,
} from '@fluentui/web-components';
import { axisGridLineStyles } from '../utils/cartesian-grid.styles.js';
import { tooltipBaseStyles } from '../utils/tooltip.styles.js';

export const styles: ElementStyles = css`
  ${display('block')}

  :host {
    display: grid;
    grid-template-areas:
      'title'
      'chart'
      'legend';
    grid-template-columns: 1fr;
    grid-template-rows: auto 1fr auto;
    position: relative;
    width: 100%;
  }

  .chart-title {
    grid-area: title;
    margin-bottom: 8px;
    ${typographyBody1StrongStyles}
    text-align: start;
  }

  .chart-container {
    grid-area: chart;
    min-width: 0;
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

  :host([title-position='bottom']) .chart-title {
    margin-bottom: 0;
    margin-top: 8px;
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

  :host([title-align='center']) .chart-title {
    text-align: center;
  }

  :host([title-align='end']) .chart-title {
    text-align: end;
  }

  .chart-svg {
    display: block;
    overflow: visible;
  }

  .axis-domain,
  .origin-line {
    stroke: ${colorNeutralStroke1};
    stroke-width: 1;
    opacity: 0.2;
  }

  .axis-tick-line {
    stroke: ${colorNeutralForeground1};
    stroke-width: 1;
    opacity: 0.24;
  }

  ${axisGridLineStyles}

  .axis-text,
  .y-axis-text,
  .x-axis-title,
  .y-axis-title {
    ${typographyCaption1Styles}
    fill: ${colorNeutralForeground2};
    font-size: 10px;
    font-weight: 600;
  }

  .bar {
    opacity: 1;
  }

  .bar-label {
    fill: ${colorNeutralForeground2};
    font-size: 10px;
    font-weight: 600;
  }

  .bar.inactive {
    opacity: 0.1;
  }

  .bar:focus {
    outline: none;
    stroke-width: ${strokeWidthThick};
    stroke: black;
  }

  ${tooltipBaseStyles}

  .tooltip {
    ${typographyCaption1Styles}
    z-index: 999;
    box-shadow: ${shadow4};
    border: ${strokeWidthThick};
    white-space: nowrap;
  }

  .tooltip-header {
    ${typographyCaption1Styles}
    color: ${colorNeutralForeground2};
    opacity: 0.8;
  }

  .tooltip-info {
    margin-top: 11px;
    padding-inline-start: ${spacingHorizontalS};
    border-inline-start: ${strokeWidthThickest} solid;
  }

  .tooltip-legend-text {
    ${typographyCaption1Styles}
    color: ${colorNeutralForeground1};
    text-align: start;
    margin-bottom: ${spacingVerticalXS};
  }

  .tooltip-primary-value {
    ${typographySubtitle2StrongerStyles}
    font-size: ${fontSizeBase500};
    direction: ltr;
    unicode-bidi: isolate;
  }

  @media (forced-colors: active) {
    .tooltip-info {
      forced-color-adjust: none;
    }
  }
`;
