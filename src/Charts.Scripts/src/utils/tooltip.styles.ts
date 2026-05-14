import { css } from '@microsoft/fast-element';
import { colorNeutralBackground1, spacingHorizontalL, spacingVerticalMNudge } from '@fluentui/web-components';

/**
 * Shared base styles for the chart tooltip container.
 * Each chart extends these with its own z-index, shadow/border, and typography rules.
 *
 * @internal
 */
export const tooltipBaseStyles = css`
  .tooltip {
    position: absolute;
    display: grid;
    overflow: hidden;
    padding: ${spacingVerticalMNudge} ${spacingHorizontalL};
    background: ${colorNeutralBackground1};
    pointer-events: none;
  }
`;
