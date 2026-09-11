import { css } from '@microsoft/fast-element';
import {
  borderRadiusMedium,
  colorNeutralBackground1,
  colorNeutralForeground1,
  colorNeutralForeground3,
  colorNeutralShadowAmbient,
  colorNeutralShadowKey,
  colorStrokeFocus2,
  colorTransparentStroke,
  display,
  spacingHorizontalL,
  spacingHorizontalS,
  spacingVerticalMNudge,
  spacingVerticalS,
  strokeWidthThin,
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

  .bar {
    outline: none;
    cursor: default;
  }

  .bar.interactive {
    cursor: pointer;
  }

  .bar:focus {
    outline: none;
    stroke: ${colorStrokeFocus2};
    stroke-width: ${strokeWidthThin};
  }

  .bar-value {
    ${typographyCaption1Styles}
    fill: ${colorNeutralForeground3};
    pointer-events: none;
  }

  .bar.inactive,
  .bar-value.inactive {
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
    transform: translateX(-50%);
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
