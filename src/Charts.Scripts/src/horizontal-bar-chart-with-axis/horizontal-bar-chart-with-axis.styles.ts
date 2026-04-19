import type { ElementStyles } from '@microsoft/fast-element';
import { css } from '@microsoft/fast-element';
import {
  colorNeutralBackground1,
  colorNeutralForeground1,
  colorNeutralForeground2,
  colorNeutralStroke1,
  colorNeutralStrokeAccessible,
  display,
  fontSizeHero700,
  forcedColorsStylesheetBehavior,
  shadow4,
  spacingHorizontalL,
  spacingHorizontalNone,
  spacingHorizontalS,
  spacingVerticalL,
  spacingVerticalMNudge,
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

export const styles: ElementStyles = css`
  ${display('inline-block')}

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

  .legend-container {
    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    padding-top: ${spacingVerticalL};
    width: 100%;
    align-items: center;
    margin: -${spacingVerticalS} ${spacingHorizontalNone} ${spacingVerticalNone} -${spacingHorizontalS};
  }

  .legend {
    display: flex;
    align-items: center;
    cursor: pointer;
    border: none;
    padding: ${spacingHorizontalS};
    background: none;
  }

  .legend-rect {
    width: 12px;
    height: 12px;
    margin-inline-end: ${spacingHorizontalS};
    border: ${strokeWidthThin} solid;
  }

  .legend-text {
    ${typographyCaption1Styles}
    color: ${colorNeutralForeground1};
  }

  .legend.inactive .legend-rect {
    background-color: transparent !important;
  }

  .legend.inactive .legend-text {
    opacity: 0.67;
  }

  .tooltip {
    ${typographyCaption1Styles}
    position: absolute;
    z-index: 999;
    display: grid;
    overflow: hidden;
    padding: ${spacingVerticalMNudge} ${spacingHorizontalL};
    background: ${colorNeutralBackground1};
    box-shadow: ${shadow4};
    border: ${strokeWidthThick};
    pointer-events: none;
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
`.withBehaviors(
  forcedColorsStylesheetBehavior(css`
    .legend-rect,
    .tooltip-info {
      forced-color-adjust: none;
    }

    .bar-label {
      fill: CanvasText !important;
    }
  `),
);
