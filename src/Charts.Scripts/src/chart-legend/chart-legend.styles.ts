import type { ElementStyles } from '@microsoft/fast-element';
import { css } from '@microsoft/fast-element';
import {
  colorNeutralForeground1,
  spacingHorizontalL,
  spacingHorizontalNone,
  spacingHorizontalS,
  spacingVerticalL,
  spacingVerticalNone,
  spacingVerticalS,
  strokeWidthThin,
  typographyCaption1Styles,
} from '@fluentui/web-components';

/**
 * Styles for the ChartLegend component.
 *
 * @public
 */
export const styles: ElementStyles = css`
  :host {
    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    padding-top: ${spacingVerticalL};
    width: 100%;
    align-items: center;
    margin: -${spacingVerticalS} ${spacingHorizontalNone} ${spacingVerticalNone} -${spacingHorizontalS};
  }

  :host([hidden]) {
    display: none;
  }

  /* ── Position overrides ──────────────────────────────────────────── */

  /* top: legend sits above chart — padding moves from top to bottom */
  :host([position='top']) {
    padding-top: 0;
    padding-bottom: ${spacingVerticalL};
  }

  /* start: legend sits inline-start of chart */
  :host([position='start']) {
    flex-direction: column;
    flex-wrap: nowrap;
    width: auto;
    padding-top: 0;
    padding-inline-end: ${spacingHorizontalL};
    align-items: flex-start;
    margin: 0;
  }

  /* end: legend sits inline-end of chart */
  :host([position='end']) {
    flex-direction: column;
    flex-wrap: nowrap;
    width: auto;
    padding-top: 0;
    padding-inline-start: ${spacingHorizontalL};
    align-items: flex-start;
    margin: 0;
  }

  .legend {
    display: flex;
    align-items: center;
    cursor: pointer;
    border: none;
    padding: ${spacingHorizontalS};
    background: none;
    text-transform: capitalize;
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
    opacity: 0.1;
  }

  .legend.inactive .legend-text {
    opacity: 0.67;
  }

  @media (forced-colors: active) {
    .legend-rect {
      forced-color-adjust: none;
    }
  }
`;
