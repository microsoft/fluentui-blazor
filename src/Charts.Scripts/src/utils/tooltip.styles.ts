import { css } from '@microsoft/fast-element';
import { colorNeutralBackground1, spacingHorizontalL, spacingVerticalMNudge } from '@fluentui/web-components';

/**
 * Shared base styles for the chart tooltip container.
 * Each chart extends these with its own z-index, shadow/border, and typography rules.
 *
 * @internal
 */
export const tooltipBaseStyles = css`
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
  }
`;
