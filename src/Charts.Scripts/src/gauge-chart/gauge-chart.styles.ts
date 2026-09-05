import { css } from '@microsoft/fast-element';
import {
  borderRadiusMedium,
  colorNeutralBackground1,
  colorNeutralForeground1,
  colorNeutralForeground3,
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
  strokeWidthThick,
  strokeWidthThickest,
  strokeWidthThin,
  typographyBody1StrongStyles,
  typographyBody1Styles,
  typographyCaption1StrongStyles,
  typographyCaption1Styles,
  typographyTitle3Styles,
} from '@fluentui/web-components';

/**
 * Styles for the GaugeChart component.
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
    grid-template-rows: auto 1fr auto;
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

  /* ── SVG chart ───────────────────────────────────────────────── */

  .chart {
    box-sizing: content-box;
    overflow: visible;
    display: block;
  }

  /* ── Gauge segments ──────────────────────────────────────────── */

  .segment {
    outline: none;
    cursor: default;
  }

  .segment.inactive {
    opacity: 0.1;
  }

  .segment:focus {
    outline: none;
    stroke-width: ${strokeWidthThin};
    stroke: ${colorStrokeFocus1};
  }

  .segment-outline {
    fill: none;
  }

  .segment-outline:has(+ .segment:focus) {
    stroke-width: ${strokeWidthThickest};
    stroke: ${colorStrokeFocus2};
  }

  /* ── Needle ──────────────────────────────────────────────────── */

  .needle {
    fill: ${colorNeutralForeground1};
    outline: none;
    cursor: default;
  }

  .needle:focus {
    outline: none;
    stroke-width: ${strokeWidthThick};
    stroke: ${colorStrokeFocus2};
  }

  /* ── Value / sublabel text ───────────────────────────────────── */

  .chart-value {
    ${typographyTitle3Styles}
    fill: ${colorNeutralForeground1};
    pointer-events: none;
  }

  .sublabel {
    ${typographyCaption1Styles}
    fill: ${colorNeutralForeground3};
    pointer-events: none;
  }

  /* ── Min / max limit labels ──────────────────────────────────── */

  .limit-label {
    ${typographyCaption1StrongStyles}
    fill: ${colorNeutralForeground1};
    pointer-events: none;
  }

  /* ── Tooltip ─────────────────────────────────────────────────── */

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
    background-blend-mode: normal, luminosity;
    border-radius: ${borderRadiusMedium};
    border: 1px solid ${colorTransparentStroke};
    filter: drop-shadow(0 0 2px ${colorNeutralShadowAmbient}) drop-shadow(0 8px 16px ${colorNeutralShadowKey});
    min-width: 120px;
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
    color: ${colorNeutralForeground1};
    border-inline-start: 4px solid;
    margin-block: ${spacingVerticalS};
  }

  .tooltip-legend-text {
    ${typographyCaption1Styles}
  }

  .tooltip-content-y {
    ${typographyBody1StrongStyles}
  }

  @media (forced-colors: active) {
    .needle {
      fill: CanvasText;
    }

    .chart-value,
    .sublabel,
    .limit-label {
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
