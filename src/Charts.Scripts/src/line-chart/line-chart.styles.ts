import type { ElementStyles } from '@microsoft/fast-element';
import { css } from '@microsoft/fast-element';
import {
  borderRadiusMedium,
  colorNeutralBackground1,
  colorNeutralForeground1,
  colorNeutralForeground2,
  colorNeutralStroke1,
  colorNeutralStroke2,
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

  .axis-text,
  .y-axis-text,
  .x-axis-title,
  .y-axis-title {
    ${typographyCaption1Styles}
    fill: ${colorNeutralForeground2};
    font-size: 10px;
    font-weight: 600;
  }

  .event-annotation-label {
    ${typographyCaption1Styles}
    fill: ${colorNeutralForeground1};
    cursor: pointer;
  }

  .event-annotation-card {
    position: absolute;
    z-index: 1000;
    box-sizing: border-box;
    inline-size: max-content;
    max-inline-size: min(280px, calc(100% - 32px));
    padding: 12px 36px 12px 16px;
    border: 1px solid ${colorNeutralStroke2};
    border-radius: ${borderRadiusMedium};
    background: ${colorNeutralBackground1};
    box-shadow: ${shadow4};
    color: ${colorNeutralForeground1};
    transform: translateX(-50%);
    ${typographyCaption1Styles}
  }

  .event-annotation-card-close {
    position: absolute;
    inset-block-start: 4px;
    inset-inline-end: 4px;
    display: grid;
    place-items: center;
    inline-size: 28px;
    block-size: 28px;
    padding: 0;
    border: 0;
    background: transparent;
    color: ${colorNeutralForeground1};
    cursor: pointer;
  }

  .event-annotation-card-item + .event-annotation-card-item {
    margin-block-start: 8px;
  }

  ${axisGridLineStyles}

  .line-path {
    fill: none;
  }

  .line-marker {
    stroke: white;
    stroke-width: 1.5;
  }

  .hover-line {
    stroke: ${colorNeutralStroke1};
    stroke-width: 1;
    stroke-dasharray: 5 3;
    pointer-events: none;
  }

  .hover-dot {
    pointer-events: none;
  }

  .line-border.inactive,
  .line-path.inactive,
  .line-marker.inactive {
    opacity: 0.1;
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
