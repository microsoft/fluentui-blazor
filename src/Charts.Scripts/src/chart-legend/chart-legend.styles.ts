import type { ElementStyles } from '@microsoft/fast-element';
import { css } from '@microsoft/fast-element';
import {
  borderRadiusSmall,
  borderRadiusMedium,
  colorNeutralForeground1,
  colorStrokeFocus2,
  colorSubtleBackgroundHover,
  spacingHorizontalL,
  spacingHorizontalNone,
  spacingHorizontalS,
  spacingVerticalL,
  spacingVerticalNone,
  spacingVerticalS,
  strokeWidthThin,
  strokeWidthThick,
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
    flex-wrap: nowrap;
    overflow-x: clip;
    overflow-y: visible;
    position: relative;
    box-sizing: border-box;
    padding-top: ${spacingVerticalL};
    padding-inline-start: ${spacingHorizontalS};
    width: 100%;
    align-items: center;
  }

  :host([hidden]) {
    display: none;
  }

  :host([center]) {
    justify-content: center;
  }

  /* ── Position overrides ──────────────────────────────────────────── */

  /* top: legend sits above chart — padding moves from top to bottom */
  :host([position='top']) {
    padding-top: 0;
    padding-bottom: ${spacingVerticalL};
  }

  /* start / end: vertical column layout — restore wrapping, no overflow detection */
  :host([position='start']) {
    flex-direction: column;
    flex-wrap: wrap;
    overflow: visible;
    width: auto;
    padding-top: 0;
    padding-inline-end: ${spacingHorizontalL};
    align-items: flex-start;
    margin: 0;
  }

  :host([position='end']) {
    flex-direction: column;
    flex-wrap: wrap;
    overflow: visible;
    width: auto;
    padding-top: 0;
    padding-inline-start: ${spacingHorizontalL};
    align-items: flex-start;
    margin: 0;
  }

  .legend {
    display: flex;
    flex-shrink: 0;
    align-items: center;
    cursor: pointer;
    border: none;
    padding: ${spacingHorizontalS};
    background: none;
    text-transform: capitalize;
    border-radius: ${borderRadiusMedium};
  }

  .legend:hover {
    background-color: ${colorSubtleBackgroundHover};
  }

  .legend:focus-visible {
    outline: ${strokeWidthThick} solid ${colorStrokeFocus2};
    outline-offset: 1px;
  }

  .legend-rect {
    width: 12px;
    height: 12px;
    margin-inline-end: ${spacingHorizontalS};
    border: ${strokeWidthThin} solid;
  }

  .legend-rect.rounded {
    border-radius: ${borderRadiusSmall};
  }

/* Same for overflow menu items */
fluent-menu-item .legend-rect.rounded {
  border-radius: ${borderRadiusSmall};
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

  @media (forced-colors: active) {
    .legend-rect {
      forced-color-adjust: none;
    }
  }
`;
