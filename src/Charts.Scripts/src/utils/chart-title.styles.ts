import { css } from '@microsoft/fast-element';
import { colorNeutralForeground1, spacingVerticalS, typographyBody1StrongStyles } from '@fluentui/web-components';

/** Shared title styles for chart web components. */
export const chartTitleStyles = css`
  .chart-title {
    grid-area: title;
    ${typographyBody1StrongStyles}
    color: ${colorNeutralForeground1};
    text-align: start;
  }

  :host([title-position='bottom']) .chart-title {
    margin-bottom: 0;
    margin-top: ${spacingVerticalS};
  }

  :host([title-align='center']) .chart-title {
    text-align: center;
  }

  :host([title-align='end']) .chart-title {
    text-align: end;
  }
`;
