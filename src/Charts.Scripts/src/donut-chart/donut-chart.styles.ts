import { css } from '@microsoft/fast-element';
import {
  borderRadiusMedium,
  colorNeutralForeground1,
  colorNeutralShadowAmbient,
  colorNeutralShadowKey,
  colorStrokeFocus1,
  colorStrokeFocus2,
  colorTransparentStroke,
  display,
  spacingHorizontalNone,
  spacingHorizontalS,
  spacingVerticalL,
  spacingVerticalNone,
  spacingVerticalS,
  strokeWidthThickest,
  strokeWidthThin,
  typographyBody1StrongStyles,
  typographyBody1Styles,
  typographyCaption1Styles,
  typographyCaption1StrongStyles,
  typographyTitle2Styles,
  typographyTitle3Styles,
} from '@fluentui/web-components';
import { tooltipBaseStyles } from '../utils/tooltip.styles.js';

/**
 * Styles for the DonutChart component.
 *
 * @public
 */
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
    width: 100%;
    height: 100%;
    position: relative;
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
  /* Use auto auto 1fr so the fixed-size SVG and legend sit flush together;
     the 1fr spacer column absorbs leftover host width. */
  :host([legend-position='start']) {
    grid-template-areas:
      'title  title  title'
      'legend chart  .    ';
    grid-template-columns: auto auto 1fr;
  }

  /* legend-position="end" */
  :host([legend-position='end']) {
    grid-template-areas:
      'title  title  title'
      'chart  legend .    ';
    grid-template-columns: auto auto 1fr;
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
      'legend chart  .    '
      'title  title  title';
    grid-template-columns: auto auto 1fr;
  }

  /* Combined: title-position="bottom" + legend-position="end" */
  :host([title-position='bottom'][legend-position='end']) {
    grid-template-areas:
      'chart  legend .    '
      'title  title  title';
    grid-template-columns: auto auto 1fr;
  }

  :host([title-align='center']) .chart-title {
    text-align: center;
  }

  :host([title-align='end']) .chart-title {
    text-align: end;
  }

  .chart {
    box-sizing: content-box;
    overflow: visible;
    display: block;
  }

  .arc.inactive {
    opacity: 0.1;
  }

  .arc:focus {
    outline: none;
    stroke-width: ${strokeWidthThin};
    stroke: ${colorStrokeFocus1};
  }

  .arc-outline {
    fill: none;
  }

  .arc-outline:has(+ .arc:focus) {
    stroke-width: ${strokeWidthThickest};
    stroke: ${colorStrokeFocus2};
  }

  .text-inside-donut {
    ${typographyTitle3Styles}
    fill: ${colorNeutralForeground1};
  }

  .arc-label {
    ${typographyCaption1StrongStyles}
    fill: ${colorNeutralForeground1};
    pointer-events: none;
    user-select: none;
  }

  .arc-label.inactive {
    opacity: 0.25;
  }

  ${tooltipBaseStyles}

  .tooltip {
    z-index: 1;
    background-blend-mode: normal, luminosity;
    border-radius: ${borderRadiusMedium};
    border: 1px solid ${colorTransparentStroke};
    filter: drop-shadow(0 0 2px ${colorNeutralShadowAmbient}) drop-shadow(0 8px 16px ${colorNeutralShadowKey});
  }

  .tooltip-inner {
    padding-inline-start: ${spacingHorizontalS};
    color: ${colorNeutralForeground1};
    border-inline-start: 4px solid;
  }

  .tooltip-legend-text {
    ${typographyCaption1Styles}
  }

  .tooltip-content-y {
    ${typographyTitle2Styles}
  }

  @media (forced-colors: active) {
    .text-inside-donut {
      fill: CanvasText;
    }

    .arc-label {
      fill: CanvasText;
    }

    .tooltip-body {
      forced-color-adjust: none;
    }

    .tooltip-legend-text,
    .tooltip-content-y {
      forced-color-adjust: auto;
      color: CanvasText;
    }
  }
`;
