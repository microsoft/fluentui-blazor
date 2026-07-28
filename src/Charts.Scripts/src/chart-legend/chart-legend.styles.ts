import type { ElementStyles } from '@microsoft/fast-element';
import { css } from '@microsoft/fast-element';
import {
<<<<<<< HEAD
  colorNeutralForeground1,
=======
  borderRadiusSmall,
  borderRadiusMedium,
  colorNeutralForeground1,
  colorStrokeFocus2,
  colorSubtleBackgroundHover,
>>>>>>> users/vnbaaij/dev-v5/add-areachart
  spacingHorizontalL,
  spacingHorizontalNone,
  spacingHorizontalS,
  spacingVerticalL,
  spacingVerticalNone,
  spacingVerticalS,
  strokeWidthThin,
<<<<<<< HEAD
=======
  strokeWidthThick,
>>>>>>> users/vnbaaij/dev-v5/add-areachart
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
<<<<<<< HEAD
    flex-wrap: wrap;
    padding-top: ${spacingVerticalL};
    width: 100%;
    align-items: center;
    margin: -${spacingVerticalS} ${spacingHorizontalNone} ${spacingVerticalNone} -${spacingHorizontalS};
=======
    flex-wrap: nowrap;
    overflow-x: clip;
    overflow-y: visible;
    position: relative;
    box-sizing: border-box;
    padding-top: ${spacingVerticalL};
    padding-inline-start: ${spacingHorizontalS};
    width: 100%;
    align-items: center;
>>>>>>> users/vnbaaij/dev-v5/add-areachart
  }

  :host([hidden]) {
    display: none;
  }

<<<<<<< HEAD
=======
  :host([center]) {
    justify-content: center;
  }

>>>>>>> users/vnbaaij/dev-v5/add-areachart
  /* ── Position overrides ──────────────────────────────────────────── */

  /* top: legend sits above chart — padding moves from top to bottom */
  :host([position='top']) {
    padding-top: 0;
    padding-bottom: ${spacingVerticalL};
  }

<<<<<<< HEAD
  /* start: legend sits inline-start of chart */
  :host([position='start']) {
    flex-direction: column;
    flex-wrap: nowrap;
=======
  /* start / end: vertical column layout — restore wrapping, no overflow detection */
  :host([position='start']) {
    flex-direction: column;
    flex-wrap: wrap;
    overflow: visible;
>>>>>>> users/vnbaaij/dev-v5/add-areachart
    width: auto;
    padding-top: 0;
    padding-inline-end: ${spacingHorizontalL};
    align-items: flex-start;
    margin: 0;
  }

<<<<<<< HEAD
  /* end: legend sits inline-end of chart */
  :host([position='end']) {
    flex-direction: column;
    flex-wrap: nowrap;
=======
  :host([position='end']) {
    flex-direction: column;
    flex-wrap: wrap;
    overflow: visible;
>>>>>>> users/vnbaaij/dev-v5/add-areachart
    width: auto;
    padding-top: 0;
    padding-inline-start: ${spacingHorizontalL};
    align-items: flex-start;
    margin: 0;
  }

  .legend {
    display: flex;
<<<<<<< HEAD
=======
    flex-shrink: 0;
>>>>>>> users/vnbaaij/dev-v5/add-areachart
    align-items: center;
    cursor: pointer;
    border: none;
    padding: ${spacingHorizontalS};
    background: none;
    text-transform: capitalize;
<<<<<<< HEAD
=======
    border-radius: ${borderRadiusMedium};
  }

  .legend:hover {
    background-color: ${colorSubtleBackgroundHover};
  }

  .legend:focus-visible {
    outline: ${strokeWidthThick} solid ${colorStrokeFocus2};
    outline-offset: 1px;
>>>>>>> users/vnbaaij/dev-v5/add-areachart
  }

  .legend-rect {
    width: 12px;
    height: 12px;
    margin-inline-end: ${spacingHorizontalS};
    border: ${strokeWidthThin} solid;
  }

<<<<<<< HEAD
=======
  .legend-rect.rounded {
    border-radius: ${borderRadiusSmall};
  }

/* Same for overflow menu items */
fluent-menu-item .legend-rect.rounded {
  border-radius: ${borderRadiusSmall};
}
>>>>>>> users/vnbaaij/dev-v5/add-areachart
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

<<<<<<< HEAD
=======
  /* ── Overflow menu (fluent-menu/fluent-menu-item) ───────────────── */

  fluent-menu {
    flex-shrink: 0;
  }

  /*
   * fluent-menu-list sets data-indent="2" on all items when any item has
   * role="menuitemcheckbox", which reserves a 20px column for the checkmark.
   * We suppress the checkmark visually (empty <span slot="indicator">) and
   * collapse that first column to 0px so no space is wasted.
   * Outer-context author styles beat :host() shadow rules in the CSS cascade.
   */
  fluent-menu-item {
    grid-template-columns: 0 20px auto auto;
  }

  fluent-menu-item .legend-rect {
    width: 12px;
    height: 12px;
    border: ${strokeWidthThin} solid;
  }

  fluent-menu-item.inactive .legend-rect {
    opacity: 0.1;
  }

  fluent-menu-item.inactive .legend-text {
    opacity: 0.67;
  }

  fluent-menu-item .legend-text {
    ${typographyCaption1Styles}
    color: ${colorNeutralForeground1};
    text-transform: capitalize;
  }

>>>>>>> users/vnbaaij/dev-v5/add-areachart
  @media (forced-colors: active) {
    .legend-rect {
      forced-color-adjust: none;
    }
  }
`;
