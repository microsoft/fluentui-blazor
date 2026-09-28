import { css } from '@microsoft/fast-element';
import { colorNeutralForeground1 } from '@fluentui/web-components';

/** Shared plot-grid styling for Cartesian charts, independent of line orientation. */
export const axisGridLineStyles = css`
  .axis-grid-line {
    stroke: ${colorNeutralForeground1};
    stroke-width: 1;
    opacity: 0.2;
    pointer-events: none;
  }
`;
