import { css } from '@microsoft/fast-element';
import { display } from '@fluentui/web-components';

export const styles = css`
  ${display('block')}

  :host {
    display: block;
  }

  .chart-container {
    display: flex;
    align-items: center;
    width: 100%;
    height: 100%;
  }

  .chart {
    display: block;
    flex: none;
  }

  .sparkline-line {
    fill: none;
    stroke-width: 2;
    stroke-linejoin: round;
    stroke-linecap: round;
  }

  .sparkline-area {
    opacity: 0.18;
  }

  .sparkline-legend {
    flex: none;
    overflow: visible;
  }

  .sparkline-legend-text {
    direction: ltr;
    fill: var(--colorNeutralForeground1);
    font-family: var(--fontFamilyBase);
    font-size: var(--fontSizeBase200);
    font-weight: var(--fontWeightRegular);
    forced-color-adjust: auto;
  }
`;
