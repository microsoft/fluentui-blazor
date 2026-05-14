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
    position: relative;
    width: 100%;
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
  .tooltip-line {
    padding-inline-start: ${spacingHorizontalS};
    height: 50px;
    border-inline-start: ${strokeWidthThickest} solid;
  }
  .tooltip-legend-text {
    ${typographyCaption1Styles}
    color: ${colorNeutralForeground1};
    text-align: start;
  }
  .tooltip-data-y {
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
  .chart-title {
    ${typographyBody1StrongStyles}
    color: ${colorNeutralForeground1};
    margin-bottom: ${spacingVerticalS};
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
