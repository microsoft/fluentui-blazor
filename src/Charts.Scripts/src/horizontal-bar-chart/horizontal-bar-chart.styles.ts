import type { ElementStyles } from '@microsoft/fast-element';
import { css } from '@microsoft/fast-element';
import {
  colorNeutralForeground1,
  colorNeutralStrokeAccessible,
  display,
  shadow4,
  spacingHorizontalNone,
  spacingHorizontalS,
  spacingHorizontalSNudge,
  spacingVerticalL,
  spacingVerticalM,
  spacingVerticalNone,
  spacingVerticalS,
  spacingVerticalXS,
  strokeWidthThick,
  strokeWidthThickest,
  strokeWidthThin,
  typographyBody1StrongStyles,
  typographyBody1Styles,
  typographyCaption1Styles,
  typographyTitle2Styles,
} from '@fluentui/web-components';
import { tooltipBaseStyles } from '../utils/tooltip.styles.js';

/**
 * Styles for the HorizontalBarChart component.
 *
 * @public
 */
export const styles: ElementStyles = css`
  ${display('block')}

  :host {
    display: grid;
    grid-template-areas:
      'title'
      'chart'
      'legend';
    grid-template-columns: 1fr;
    position: relative;
    width: 100%;
  }

  /* ── Title and legend layout (CSS Grid named areas) ─────────── */

  .chart-title {
    grid-area: title;
    margin-bottom: ${spacingVerticalS};
    ${typographyBody1StrongStyles}
    color: ${colorNeutralForeground1};
    text-align: start;
  }

  .chart-container {
    grid-area: chart;
    min-width: 0; /* allow grid cell to shrink below SVG intrinsic width */
  }

  fluent-chart-legend {
    grid-area: legend;
  }

  /* title-position="bottom" */
  :host([title-position='bottom']) {
    grid-template-areas:
      'chart'
      'legend'
      'title';
  }

  :host([title-position='bottom']) .chart-title {
    margin-bottom: 0;
    margin-top: ${spacingVerticalS};
  }

  /* legend-position="top" */
  :host([legend-position='top']) {
    grid-template-areas:
      'title'
      'legend'
      'chart';
  }

  /* legend-position="start" */
  :host([legend-position='start']) {
    grid-template-areas:
      'title  title'
      'legend chart';
    grid-template-columns: auto 1fr;
  }

  /* legend-position="end" */
  :host([legend-position='end']) {
    grid-template-areas:
      'title  title'
      'chart  legend';
    grid-template-columns: 1fr auto;
  }

  /* Legend on side: anchor legend to the top of its cell */
  :host([legend-position='start']) fluent-chart-legend,
  :host([legend-position='end']) fluent-chart-legend {
    align-self: start;
  }

  /* Combined: title-position="bottom" + legend-position="top" */
  :host([title-position='bottom'][legend-position='top']) {
    grid-template-areas:
      'legend'
      'chart'
      'title';
    grid-template-columns: 1fr;
  }

  /* Combined: title-position="bottom" + legend-position="start" */
  :host([title-position='bottom'][legend-position='start']) {
    grid-template-areas:
      'legend chart'
      'title  title';
    grid-template-columns: auto 1fr;
  }

  /* Combined: title-position="bottom" + legend-position="end" */
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
  ${tooltipBaseStyles}

  .tooltip {
    ${typographyCaption1Styles}
    z-index: 999;
    background-blend-mode: normal, luminosity;
    text-align: center;
    box-shadow: ${shadow4};
    border: ${strokeWidthThick};
  }
  .tooltip-inner {
    padding-inline-start: ${spacingHorizontalS};
    height: 50px;
    border-inline-start: ${strokeWidthThickest} solid;
  }
  .tooltip-legend-text {
    ${typographyCaption1Styles}
    color: ${colorNeutralForeground1};
    text-align: start;
  }
  .tooltip-content-y {
    ${typographyTitle2Styles}
    text-align: start;
  }
  .bar {
    opacity: 1;
  }
  .bar.inactive {
    opacity: 0.1;
  }
  .bar:focus {
    outline: none;
    stroke-width: ${strokeWidthThick};
    stroke: black;
  }
  .svg-chart {
    display: block;
    overflow: visible;
  }
  .bar-title {
    ${typographyBody1Styles}
    color: ${colorNeutralForeground1};
    text-overflow: ellipsis;
    overflow: hidden;
    white-space: nowrap;
    display: block;
  }
  .bar-label {
    ${typographyBody1StrongStyles}
    fill: ${colorNeutralForeground1};
  }
  .bar-title-div {
    width: 100%;
    display: flex;
    justify-content: space-between;
  }
  .ratio-numerator {
    ${typographyBody1StrongStyles}
    color: ${colorNeutralForeground1};
  }
  .ratio-denominator {
    ${typographyBody1StrongStyles}
    color: ${colorNeutralForeground1};
    font-weight: bold;
  }
  .benchmark-container {
    position: relative;
    height: 7px;
    margin-top: -3px;
  }
  .triangle {
    width: 0;
    height: 0;
    border-left: ${strokeWidthThickest} solid transparent;
    border-right: ${strokeWidthThickest} solid transparent;
    border-bottom: 7px solid;
    border-bottom-color: ${colorNeutralStrokeAccessible};
    margin-bottom: ${spacingVerticalXS};
    position: absolute;
  }
  .chart-data-text {
    ${typographyBody1StrongStyles}
    color: ${colorNeutralForeground1};
  }

  @media (forced-colors: active) {
    .tooltip-line,
    .triangle {
      forced-color-adjust: none;
    }
    .tooltip-legend-text,
    .tooltip-content-y {
      forced-color-adjust: auto;
      color: CanvasText;
    }
    .bar-label {
      fill: CanvasText !important;
    }
  }
`;
