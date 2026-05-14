import type { ElementStyles } from '@microsoft/fast-element';
import { css } from '@microsoft/fast-element';
import {
  colorNeutralForeground1,
  colorNeutralForeground2,
  colorNeutralStroke1,
  colorNeutralStrokeAccessible,
  display,
  fontSizeHero700,
  shadow4,
  spacingHorizontalNone,
  spacingHorizontalS,
  spacingVerticalL,
  spacingVerticalNone,
  spacingVerticalS,
  spacingVerticalXS,
  strokeWidthThick,
  strokeWidthThickest,
  strokeWidthThin,
  typographyBody1StrongStyles,
  typographyBody1Styles,
  typographyCaption1Styles,
  typographySubtitle2StrongerStyles,
} from '@fluentui/web-components';
import { tooltipBaseStyles } from '../utils/tooltip.styles.js';

export const styles: ElementStyles = css`
  ${display('block')}

  :host {
    position: relative;
    width: 100%;
  }

  .chart-title {
    ${typographyBody1StrongStyles}
    margin-bottom: ${spacingVerticalS};
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
  .y-axis-text {
    ${typographyCaption1Styles}
    fill: ${colorNeutralForeground2};
    font-size: 10px;
    font-weight: 600;
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

  .bar-label {
    ${typographyBody1StrongStyles}
    fill: ${colorNeutralForeground1};
    direction: ltr;
    unicode-bidi: isolate;
  }

  ${tooltipBaseStyles}

  .tooltip {
    ${typographyCaption1Styles}
    z-index: 999;
    box-shadow: ${shadow4};
    border: ${strokeWidthThick};
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
    font-size: ${fontSizeHero700};
    direction: ltr;
    unicode-bidi: isolate;
  }

  @media (forced-colors: active) {
    .tooltip-info {
      forced-color-adjust: none;
    }

    .bar-label {
      fill: CanvasText !important;
    }
  }
`;
