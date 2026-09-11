import { css } from '@microsoft/fast-element';
import {
  borderRadiusMedium,
  colorNeutralBackground1,
  colorNeutralForeground1,
  colorNeutralForegroundStaticInverted,
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

  .chart-title {
    grid-area: title;
    margin-bottom: ${spacingVerticalS};
    ${typographyBody1StrongStyles}
    color: ${colorNeutralForeground1};
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
    margin-top: ${spacingVerticalS};
  }

  :host([legend-position='top']) {
    grid-template-areas:
      'title'
      'legend'
      'chart';
  }

  :host([legend-position='start']) {
    grid-template-areas:
      'title title title'
      'legend chart .';
    grid-template-columns: auto auto 1fr;
  }

  :host([legend-position='end']) {
    grid-template-areas:
      'title title title'
      'chart legend .';
    grid-template-columns: auto auto 1fr;
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
      'legend chart .'
      'title title title';
    grid-template-columns: auto auto 1fr;
  }

  :host([title-position='bottom'][legend-position='end']) {
    grid-template-areas:
      'chart legend .'
      'title title title';
    grid-template-columns: auto auto 1fr;
  }

  :host([title-align='center']) .chart-title {
    text-align: center;
  }

  :host([title-align='end']) .chart-title {
    text-align: end;
  }

  .chart {
    display: block;
    overflow: visible;
  }

  .sankey-link {
    fill: none;
    stroke-opacity: 0.3;
  }

  .sankey-link:hover {
    stroke-opacity: 0.7;
  }

  .sankey-node:focus-visible {
    outline: 2px solid ${colorNeutralForeground1};
    outline-offset: 2px;
  }

  .sankey-link:focus-visible {
    outline: none;
    stroke-opacity: 1;
  }

  .sankey-link-focus-outline {
    fill: none;
    opacity: 0;
    pointer-events: none;
  }

  .sankey-link-focus-outline.outer {
    stroke: ${colorStrokeFocus2};
    stroke-width: calc(var(--sankey-link-width) + ${strokeWidthThickest});
  }

  .sankey-link-focus-outline.inner {
    stroke: ${colorStrokeFocus1};
    stroke-width: calc(var(--sankey-link-width) + ${strokeWidthThick});
  }

  .sankey-link-focus-outline.outer:has(+ .sankey-link-focus-outline.inner + .sankey-link:focus-visible),
  .sankey-link-focus-outline.inner:has(+ .sankey-link:focus-visible) {
    opacity: 1;
  }

  .sankey-node-label {
    fill: ${colorNeutralForegroundStaticInverted};
    font-size: 10px;
    pointer-events: none;
  }

  .sankey-node-value {
    font-size: 14px;
    font-weight: 700;
  }

  .sankey-node.inactive,
  .sankey-link.inactive,
  .sankey-node-label.inactive {
    opacity: 0.1;
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
