import { css } from '@microsoft/fast-element';
import {
  borderRadiusMedium,
  colorNeutralBackground1,
  colorNeutralForeground1,
  colorNeutralShadowAmbient,
  colorNeutralShadowKey,
  colorStrokeFocus1,
  colorStrokeFocus2,
  colorTransparentStroke,
  display,
  spacingHorizontalL,
  spacingHorizontalS,
  spacingVerticalMNudge,
  spacingVerticalS,
  strokeWidthThickest,
  strokeWidthThin,
  typographyBody1StrongStyles,
  typographyCaption1Styles,
  typographyTitle2Styles,
} from '@fluentui/web-components';
import { tooltipBaseStyles } from '../utils/tooltip.styles.js';

/**
 * Styles for the FunnelChart component.
 *
 * @public
 */
export const styles = css`
  ${display('block')}

  :host {
    ${typographyBody1StrongStyles}
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

  .funnel-segment {
    transition: opacity 0.1s ease;
  }

  .funnel-segment.inactive {
    opacity: 0.1;
  }

  .funnel-segment:focus {
    outline: none;
    stroke-width: ${strokeWidthThin};
    stroke: ${colorStrokeFocus1};
  }

  .funnel-segment:focus-visible {
    stroke-width: ${strokeWidthThickest};
    stroke: ${colorStrokeFocus2};
  }

  .funnel-segment-text {
    font-size: 12px;
    pointer-events: none;
    user-select: none;
  }

  .funnel-segment-text.inactive {
    opacity: 0.25;
  }

  ${tooltipBaseStyles}

  .tooltip {
    ${typographyCaption1Styles}
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
    .funnel-segment-text {
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
