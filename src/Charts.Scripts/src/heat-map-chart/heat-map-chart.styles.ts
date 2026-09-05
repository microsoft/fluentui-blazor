import type { ElementStyles } from '@microsoft/fast-element';
import { css } from '@microsoft/fast-element';
import {
  borderRadiusMedium,
  colorNeutralForeground1,
  colorNeutralForeground2,
  colorNeutralShadowAmbient,
  colorNeutralShadowKey,
  colorNeutralStroke1,
  colorNeutralStroke2,
  colorStrokeFocus1,
  colorStrokeFocus2,
  colorTransparentStroke,
  display,
  spacingHorizontalS,
  spacingHorizontalSNudge,
  spacingVerticalMNudge,
  strokeWidthThick,
  strokeWidthThickest,
  typographyBody1StrongStyles,
  typographyCaption1Styles,
  typographyCaption2StrongStyles,
  typographyCaption2Styles,
  typographyTitle2Styles,
} from '@fluentui/web-components';
import { tooltipBaseStyles } from '../utils/tooltip.styles.js';

/**
 * Styles for the HeatMapChart component.
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
    grid-template-rows: auto 1fr auto;
    position: relative;
    width: 100%;
  }

  /* ── Title and legend layout (CSS Grid named areas) ─────────── */

  .chart-title {
    grid-area: title;
    margin-bottom: 8px;
    ${typographyBody1StrongStyles}
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
    margin-top: 8px;
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

  .chart-svg {
    display: block;
    overflow: visible;
  }

  /* ── Axes ──────────────────────────────────────────────────── */

  .axis-domain {
    stroke: ${colorNeutralStroke1};
    stroke-width: 1;
    opacity: 0.2;
  }

  .axis-text,
  .y-axis-text {
    ${typographyCaption1Styles}
    fill: ${colorNeutralForeground2};
    font-size: 10px;
    font-weight: 600;
  }

  .axis-title {
    ${typographyCaption1Styles}
    fill: ${colorNeutralForeground2};
    font-size: 11px;
  }

  /* ── Grid cells ────────────────────────────────────────────── */

  .heat-cell {
    cursor: default;
  }

  .heat-cell.inactive {
    opacity: 0.1;
  }

  .heat-rect {
    rx: 2px;
  }

  .cell-text {
    pointer-events: none;
    user-select: none;
  }

  .heat-cell:focus {
    outline: none;
  }

  .heat-cell:focus .heat-rect {
    stroke: ${colorStrokeFocus1};
    stroke-width: ${strokeWidthThickest};
    filter: drop-shadow(0 0 ${strokeWidthThick} ${colorStrokeFocus2});
  }

  /* ── Live region (screen-reader announcements) ─────────────── */

  .live-region {
    position: absolute;
    width: 1px;
    height: 1px;
    overflow: hidden;
    clip: rect(0, 0, 0, 0);
    white-space: nowrap;
  }

  /* ── Tooltip ────────────────────────────────────────────────── */

  ${tooltipBaseStyles}

  .tooltip {
    z-index: 999;
    max-width: 238px;
    background-blend-mode: normal, luminosity;
    border-radius: ${borderRadiusMedium};
    border: 1px solid ${colorTransparentStroke};
    filter: drop-shadow(0 0 2px ${colorNeutralShadowAmbient}) drop-shadow(0 8px 16px ${colorNeutralShadowKey});
  }

  .tooltip-inner {
    display: flex;
    align-items: flex-end;
    padding-inline-start: ${spacingHorizontalS};
    color: ${colorNeutralForeground1};
    border-inline-start: 4px solid;
  }

  .tooltip-legend-text {
    ${typographyCaption1Styles}
    color: ${colorNeutralForeground2};
  }

  .tooltip-content-y {
    ${typographyTitle2Styles}
  }

  .tooltip-ratio {
    ${typographyCaption2Styles}
    margin-inline-start: ${spacingHorizontalSNudge};
    color: ${colorNeutralForeground1};
    align-self: flex-end;
    white-space: nowrap;
  }

  .tooltip-numerator,
  .tooltip-denominator {
    ${typographyCaption2StrongStyles}
  }

  .tooltip-description {
    ${typographyCaption1Styles}
    color: ${colorNeutralForeground2};
    margin-top: ${spacingVerticalMNudge};
    padding-top: ${spacingVerticalMNudge};
    border-top: 1px solid ${colorNeutralStroke2};
  }

  /* ── Forced-colors (Windows High Contrast) ─────────────────── */

  @media (forced-colors: active) {
    .heat-rect {
      forced-color-adjust: none;
    }

    .heat-cell:focus .heat-rect {
      stroke: Highlight;
    }

    .axis-text,
    .y-axis-text,
    .axis-title,
    .cell-text {
      fill: CanvasText;
    }

    .axis-domain {
      stroke: CanvasText;
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
