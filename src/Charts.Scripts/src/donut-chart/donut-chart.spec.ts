import { test } from '@playwright/test';
import { expect, fixtureURL } from '../helpers.tests.js';
import type { DonutChart as FluentDonutChart } from './donut-chart.js';
import type { ChartDataPoint, ChartProps } from './donut-chart.options.js';

const basicTitle = 'Donut chart basic example';

const points: ChartDataPoint[] = [
  {
    legend: 'first',
    data: 20000,
  },
  {
    legend: 'second',
    data: 39000,
  },
];

const data: ChartProps = {
  chartData: points,
};

test.describe('Donut-chart - Basic', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto(fixtureURL('components-donutchart--basic'));
    await page.setContent(/* html */ `
      <div>
        <fluent-donut-chart chart-title="${basicTitle}" value-inside-donut="39,000" inner-radius="55" data='${JSON.stringify(data)}'>
        </fluent-donut-chart>
      </div>
    `);
    await page.waitForFunction(() => customElements.whenDefined('fluent-donut-chart'));
  });

  test('Should render chart properly', async ({ page }) => {
    const element = page.locator('fluent-donut-chart');
    const legends = element.locator('.legend-text');
    await expect(legends.nth(0).getByText('first')).toBeVisible();
    await expect(legends.nth(1).getByText('second')).toBeVisible();
    await expect(element.getByText('39,000')).toBeVisible();
    await expect(element.locator('.arc-label')).toHaveCount(0);
  });

  test('Should render path with proper attributes and css', async ({ page }) => {
    const element = page.locator('fluent-donut-chart');
    const arcList = element.locator('.arc');
    await expect(arcList).toHaveCount(2);
    await expect(arcList.nth(0)).toHaveAttribute('fill', '#637cef');
    await expect(arcList.nth(0)).toHaveAttribute('aria-label', 'first, 20000.');
    await expect(arcList.nth(0)).toHaveAttribute(
      'd',
      'M-76.547,47.334A90,90,0,0,1,-1.055,-89.994L-1.055,-54.99A55,55,0,0,0,-46.993,28.577Z',
    );
    await expect(arcList.nth(0)).toHaveCSS('fill', 'rgb(99, 124, 239)');
    await expect(arcList.nth(0)).toHaveCSS('--borderRadiusMedium', '4px');

    await expect(arcList.nth(1)).toHaveAttribute('fill', '#e3008c');
    await expect(arcList.nth(1)).toHaveAttribute('aria-label', 'second, 39000.');
    await expect(arcList.nth(1)).toHaveAttribute(
      'd',
      'M1.055,-89.994A90,90,0,1,1,-75.417,49.115L-45.863,30.358A55,55,0,1,0,1.055,-54.99Z',
    );
    await expect(arcList.nth(1)).toHaveCSS('fill', 'rgb(227, 0, 140)');
    await expect(arcList.nth(1)).toHaveCSS('--borderRadiusMedium', '4px');
  });

  test('Should render legends data properly', async ({ page }) => {
    const element = page.locator('fluent-donut-chart');
    const legends = element.getByRole('option');
    await expect(legends).toHaveCount(2);
    const firstLegend = element.getByRole('option', { name: 'First' });
    const secondLegend = element.getByRole('option', { name: 'Second' });
    await expect(firstLegend).toBeVisible();
    await expect(firstLegend).toHaveText('first');
    await expect(firstLegend).toHaveCSS('--borderRadiusMedium', '4px');
    await expect(secondLegend).toBeVisible();
    await expect(secondLegend).toHaveText('second');
    await expect(secondLegend).toHaveCSS('--borderRadiusMedium', '4px');
  });

  test('Should update path css values with mouse click event on legend', async ({ page }) => {
    const element = page.locator('fluent-donut-chart');
    const firstPath = element.getByLabel('first,');
    const secondPath = element.getByLabel('second,');
    const firstLegend = element.getByRole('option', { name: 'First' });
    //mouse events
    await firstLegend.click();
    await expect(firstPath).toHaveCSS('opacity', '1');
    await expect(secondPath).toHaveCSS('opacity', '0.1');
    await firstLegend.dispatchEvent('mouseout');
    await expect(firstPath).toHaveCSS('opacity', '1');
    await expect(secondPath).toHaveCSS('opacity', '0.1');
    await firstLegend.click();
    await expect(firstPath).toHaveCSS('opacity', '1');
    await expect(secondPath).toHaveCSS('opacity', '1');
  });

  test('Should remove inactive arcs from the tab order when a legend is active', async ({ page }) => {
    const element = page.locator('fluent-donut-chart');
    const firstPath = element.getByLabel('first,');
    const secondPath = element.getByLabel('second,');
    const firstLegend = element.getByRole('option', { name: 'First' });

    await expect(firstPath).toHaveAttribute('tabindex', '0');
    await expect(secondPath).toHaveAttribute('tabindex', '0');

    await firstLegend.dispatchEvent('mouseover');

    await expect(firstPath).toHaveAttribute('tabindex', '0');
    await expect(secondPath).toHaveAttribute('tabindex', '-1');

    await firstLegend.dispatchEvent('mouseout');

    await expect(firstPath).toHaveAttribute('tabindex', '0');
    await expect(secondPath).toHaveAttribute('tabindex', '0');
  });

  test('Should update path css values with mouse hover event on legend', async ({ page }) => {
    const element = page.locator('fluent-donut-chart');
    const firstPath = element.getByLabel('first,');
    const secondPath = element.getByLabel('second,');
    const firstLegend = element.getByRole('option', { name: 'First' });
    //mouse events
    await firstLegend.dispatchEvent('mouseover');
    await expect(firstPath).toHaveCSS('opacity', '1');
    await expect(secondPath).toHaveCSS('opacity', '0.1');
    await firstLegend.dispatchEvent('mouseout');
    await expect(firstPath).toHaveCSS('opacity', '1');
    await expect(secondPath).toHaveCSS('opacity', '1');
  });

  test('Should show callout with mouse hover event on path', async ({ page }) => {
    const element = page.locator('fluent-donut-chart');
    const firstPath = element.getByLabel('first,');
    const calloutRoot = element.locator('.tooltip');
    await expect(calloutRoot).toHaveCount(0);
    await firstPath.dispatchEvent('mouseover');
    await expect(calloutRoot).toHaveCount(1);
    await expect(calloutRoot).toHaveCSS('opacity', '1');
    const calloutLegendText = element.locator('.tooltip-legend-text');
    await expect(calloutLegendText).toHaveText('first');
    const calloutContentY = element.locator('.tooltip-content-y');
    await expect(calloutContentY).toHaveText('20000');
    await firstPath.dispatchEvent('mouseout');
    await expect(calloutRoot).not.toHaveCSS('opacity', '0');
  });

  test('Should update callout data when mouse moved from one path to another path', async ({ page }) => {
    const element = page.locator('fluent-donut-chart');
    const firstPath = element.getByLabel('first,');
    const calloutRoot = element.locator('.tooltip');
    await expect(calloutRoot).toHaveCount(0);
    await firstPath.dispatchEvent('mouseover');
    await expect(calloutRoot).toHaveCSS('opacity', '1');
    const calloutLegendText = element.locator('.tooltip-legend-text');
    await expect(calloutLegendText).toHaveText('first');
    const calloutContentY = element.locator('.tooltip-content-y');
    await expect(calloutContentY).toHaveText('20000');
    const secondPath = element.getByLabel('second,');
    await secondPath.dispatchEvent('mouseover');
    await expect(calloutRoot).toHaveCSS('opacity', '1');
    await expect(calloutLegendText).toHaveText('second');
    await expect(calloutContentY).toHaveText('39000');
  });
});

test.describe('Donut-chart - Reactive rerender', () => {
  test('Should rerender when data attribute changes after initial render', async ({ page }) => {
    await page.goto(fixtureURL('components-donutchart--basic'));
    await page.setContent(/* html */ `
      <div>
        <fluent-donut-chart chart-title="${basicTitle}" value-inside-donut="39,000" inner-radius="55" data='${JSON.stringify(
      data,
    )}'>
        </fluent-donut-chart>
      </div>
    `);
    await page.waitForFunction(() => customElements.whenDefined('fluent-donut-chart'));

    const element = page.locator('fluent-donut-chart');
    await expect(element.locator('.arc')).toHaveCount(2);

    const newData: ChartProps = {
      chartData: [
        { legend: 'alpha', data: 10000 },
        { legend: 'beta', data: 20000 },
        { legend: 'gamma', data: 30000 },
      ],
    };

    await element.evaluate((el, d) => {
      el.setAttribute('chart-title', 'Updated chart');
      el.setAttribute('data', JSON.stringify(d));
    }, newData);

    await expect(element.locator('.arc')).toHaveCount(3);
    await expect(element.locator('.legend-text').nth(0).getByText('alpha')).toBeVisible();
    await expect(element.locator('.legend-text').nth(1).getByText('beta')).toBeVisible();
    await expect(element.locator('.legend-text').nth(2).getByText('gamma')).toBeVisible();
  });
});

test.describe('Donut-chart - hide-labels', () => {
  test('Should keep center text visible and hide outside labels when hide-labels is set', async ({ page }) => {
    await page.goto(fixtureURL('components-donutchart--basic'));
    await page.setContent(/* html */ `
      <div>
        <fluent-donut-chart
          chart-title="${basicTitle}"
          value-inside-donut="39,000"
          inner-radius="55"
          hide-labels
          data='${JSON.stringify(data)}'>
        </fluent-donut-chart>
      </div>
    `);
    await page.waitForFunction(() => customElements.whenDefined('fluent-donut-chart'));

    const element = page.locator('fluent-donut-chart');
    await expect(element.locator('.text-inside-donut')).toHaveCount(1);
    await expect(element.locator('.arc-label')).toHaveCount(0);
    await expect(element.locator('.arc')).toHaveCount(2);
  });

  test('Should show outside labels when hide-labels is false', async ({ page }) => {
    await page.goto(fixtureURL('components-donutchart--basic'));
    await page.setContent(/* html */ `
      <div>
        <fluent-donut-chart
          chart-title="${basicTitle}"
          value-inside-donut="39,000"
          inner-radius="55"
          data='${JSON.stringify(data)}'>
        </fluent-donut-chart>
      </div>
    `);
    await page.waitForFunction(() => customElements.whenDefined('fluent-donut-chart'));

    const element = page.locator('fluent-donut-chart');
    const firstArc = element.locator('.arc').first();
    const defaultPath = await firstArc.getAttribute('d');

    await element.evaluate(el => {
      (el as FluentDonutChart).hideLabels = false;
    });

    await expect(element.locator('.text-inside-donut')).toHaveCount(1);
    await expect(element.locator('.arc-label')).toHaveCount(2);
    await expect(firstArc).toHaveAttribute('d', defaultPath ?? '');
  });

  test('Should react to hide-labels string attribute updates', async ({ page }) => {
    await page.goto(fixtureURL('components-donutchart--basic'));
    await page.setContent(/* html */ `
      <div>
        <fluent-donut-chart
          chart-title="${basicTitle}"
          value-inside-donut="39,000"
          inner-radius="55"
          hide-labels="false"
          data='${JSON.stringify(data)}'>
        </fluent-donut-chart>
      </div>
    `);
    await page.waitForFunction(() => customElements.whenDefined('fluent-donut-chart'));

    const element = page.locator('fluent-donut-chart');
    await expect(element.locator('.arc-label')).toHaveCount(2);

    await element.evaluate(el => {
      el.setAttribute('hide-labels', 'true');
    });

    await expect(element.locator('.arc-label')).toHaveCount(0);
  });
});

test.describe('Donut-chart - outside labels', () => {
  test('Should render outside labels for visible segments', async ({ page }) => {
    await page.goto(fixtureURL('components-donutchart--basic'));
    await page.setContent(/* html */ `
      <div>
        <fluent-donut-chart
          chart-title="Outside labels"
          width="320"
          height="320"
          style="width:320px;height:320px"
          value-inside-donut="39,000"
          inner-radius="55"
          data='${JSON.stringify(data)}'>
        </fluent-donut-chart>
      </div>
    `);
    await page.waitForFunction(() => customElements.whenDefined('fluent-donut-chart'));

    const element = page.locator('fluent-donut-chart');
    await element.evaluate(el => {
      (el as FluentDonutChart).hideLabels = false;
    });
    const labels = element.locator('.arc-label');

    await expect(labels).toHaveCount(2);
    await expect(labels.nth(0)).toContainText('20');
    await expect(labels.nth(1)).toContainText('39');
  });

  test('Should render percent outside labels when show-labels-in-percent is set', async ({ page }) => {
    await page.goto(fixtureURL('components-donutchart--basic'));
    await page.setContent(/* html */ `
      <div>
        <fluent-donut-chart
          chart-title="Percent labels"
          width="320"
          height="320"
          style="width:320px;height:320px"
          value-inside-donut="39,000"
          inner-radius="55"
          show-labels-in-percent
          data='${JSON.stringify(data)}'>
        </fluent-donut-chart>
      </div>
    `);
    await page.waitForFunction(() => customElements.whenDefined('fluent-donut-chart'));

    const element = page.locator('fluent-donut-chart');
    await element.evaluate(el => {
      (el as FluentDonutChart).hideLabels = false;
    });
    const labels = element.locator('.arc-label');
    await expect(labels).toHaveCount(2);
    await expect(labels.nth(0)).toContainText('%');
    await expect(labels.nth(1)).toContainText('%');
  });

  test('Should react to show-labels-in-percent string attribute updates', async ({ page }) => {
    await page.goto(fixtureURL('components-donutchart--basic'));
    await page.setContent(/* html */ `
      <div>
        <fluent-donut-chart
          chart-title="Percent labels"
          width="320"
          height="320"
          style="width:320px;height:320px"
          value-inside-donut="39,000"
          inner-radius="55"
          hide-labels="false"
          show-labels-in-percent="false"
          data='${JSON.stringify(data)}'>
        </fluent-donut-chart>
      </div>
    `);
    await page.waitForFunction(() => customElements.whenDefined('fluent-donut-chart'));

    const element = page.locator('fluent-donut-chart');
    const labels = element.locator('.arc-label');
    await expect(labels).toHaveCount(2);
    await expect(labels.nth(0)).not.toContainText('%');

    await element.evaluate(el => {
      el.setAttribute('show-labels-in-percent', 'true');
    });

    await expect(labels.nth(0)).toContainText('%');
    await expect(labels.nth(1)).toContainText('%');
  });
});

test.describe('Donut-chart - hide-tooltip', () => {
  test('Should react to hide-tooltip string attribute updates', async ({ page }) => {
    await page.goto(fixtureURL('components-donutchart--basic'));
    await page.setContent(/* html */ `
      <div>
        <fluent-donut-chart
          chart-title="${basicTitle}"
          value-inside-donut="39,000"
          inner-radius="55"
          hide-tooltip="false"
          data='${JSON.stringify(data)}'>
        </fluent-donut-chart>
      </div>
    `);
    await page.waitForFunction(() => customElements.whenDefined('fluent-donut-chart'));

    const element = page.locator('fluent-donut-chart');
    const firstPath = element.getByLabel('first,');

    await firstPath.dispatchEvent('mouseover');
    await expect(element.locator('.tooltip')).toHaveCount(1);

    await element.evaluate(el => {
      el.setAttribute('hide-tooltip', 'true');
    });

    await expect(element.locator('.tooltip')).toHaveCount(0);
  });
});

test.describe('Donut-chart - hide-legends', () => {
  test('Should react to hide-legends string attribute updates', async ({ page }) => {
    await page.goto(fixtureURL('components-donutchart--basic'));
    await page.setContent(/* html */ `
      <div>
        <fluent-donut-chart
          chart-title="${basicTitle}"
          value-inside-donut="39,000"
          inner-radius="55"
          hide-legends="false"
          data='${JSON.stringify(data)}'>
        </fluent-donut-chart>
      </div>
    `);
    await page.waitForFunction(() => customElements.whenDefined('fluent-donut-chart'));

    const element = page.locator('fluent-donut-chart');
    const legendContainer = element.locator('.legend-container');
    await expect(legendContainer).toHaveCount(1);
    await expect(legendContainer).not.toBeHidden();

    await element.evaluate(el => {
      el.setAttribute('hide-legends', 'true');
    });

    await expect(legendContainer).toBeHidden();
  });
});

test.describe('Donut-chart - allow-multiple-legend-selection', () => {
  const multiData: ChartProps = {
    chartData: [
      { legend: 'first', data: 20000 },
      { legend: 'second', data: 39000 },
      { legend: 'third', data: 15000 },
    ],
  };

  test.beforeEach(async ({ page }) => {
    await page.goto(fixtureURL('components-donutchart--basic'));
    await page.setContent(/* html */ `
      <div>
        <fluent-donut-chart
          allow-multiple-legend-selection
          inner-radius="55"
          data='${JSON.stringify(multiData)}'>
        </fluent-donut-chart>
      </div>
    `);
    await page.waitForFunction(() => customElements.whenDefined('fluent-donut-chart'));
  });

  test('Should highlight multiple arcs when multiple legends are selected', async ({ page }) => {
    const element = page.locator('fluent-donut-chart');
    const firstPath = element.getByLabel('first,');
    const secondPath = element.getByLabel('second,');
    const thirdPath = element.getByLabel('third,');
    const firstLegend = element.getByRole('option', { name: 'first' });
    const secondLegend = element.getByRole('option', { name: 'second' });

    await firstLegend.click();
    await secondLegend.click();

    await expect(firstPath).toHaveCSS('opacity', '1');
    await expect(secondPath).toHaveCSS('opacity', '1');
    await expect(thirdPath).toHaveCSS('opacity', '0.1');
  });

  test('Should deselect a legend on second click in multi-select mode', async ({ page }) => {
    const element = page.locator('fluent-donut-chart');
    const firstPath = element.getByLabel('first,');
    const secondPath = element.getByLabel('second,');
    const thirdPath = element.getByLabel('third,');
    const firstLegend = element.getByRole('option', { name: 'first' });
    const secondLegend = element.getByRole('option', { name: 'second' });

    await firstLegend.click();
    await secondLegend.click();
    await firstLegend.click(); // deselect first

    await expect(firstPath).toHaveCSS('opacity', '0.1');
    await expect(secondPath).toHaveCSS('opacity', '1');
    await expect(thirdPath).toHaveCSS('opacity', '0.1');
  });

  test('Should restore all arcs when all selections are cleared', async ({ page }) => {
    const element = page.locator('fluent-donut-chart');
    const firstPath = element.getByLabel('first,');
    const secondPath = element.getByLabel('second,');
    const thirdPath = element.getByLabel('third,');
    const firstLegend = element.getByRole('option', { name: 'first' });

    await firstLegend.click();
    await firstLegend.click(); // deselect — all clear

    await expect(firstPath).toHaveCSS('opacity', '1');
    await expect(secondPath).toHaveCSS('opacity', '1');
    await expect(thirdPath).toHaveCSS('opacity', '1');
  });

  test('Should set aria-selected on selected legends', async ({ page }) => {
    const element = page.locator('fluent-donut-chart');
    const firstLegend = element.getByRole('option', { name: 'first' });
    const secondLegend = element.getByRole('option', { name: 'second' });
    const thirdLegend = element.getByRole('option', { name: 'third' });

    await firstLegend.click();
    await secondLegend.click();

    await expect(firstLegend).toHaveAttribute('aria-selected', 'true');
    await expect(secondLegend).toHaveAttribute('aria-selected', 'true');
    await expect(thirdLegend).toHaveAttribute('aria-selected', 'false');
  });

  test('Should fall back to single-select when allow-multiple-legend-selection is removed', async ({ page }) => {
    const element = page.locator('fluent-donut-chart');
    const firstPath = element.getByLabel('first,');
    const secondPath = element.getByLabel('second,');
    const firstLegend = element.getByRole('option', { name: 'first' });
    const secondLegend = element.getByRole('option', { name: 'second' });

    await firstLegend.click();
    await secondLegend.click();

    // disable multi-select → selectedLegends should be cleared
    await element.evaluate(el => {
      el.removeAttribute('allow-multiple-legend-selection');
    });

    await expect(firstPath).toHaveCSS('opacity', '1');
    await expect(secondPath).toHaveCSS('opacity', '1');

    // now single-select should work
    await firstLegend.click();
    await expect(firstPath).toHaveCSS('opacity', '1');
    await expect(secondPath).toHaveCSS('opacity', '0.1');
  });
});

test.describe('Donut-chart - round-corners', () => {
  test('Should change arc geometry when round-corners is enabled', async ({ page }) => {
    await page.goto(fixtureURL('components-donutchart--basic'));
    await page.setContent(/* html */ `
      <div>
        <fluent-donut-chart
          chart-title="Rounded corners"
          value-inside-donut="39,000"
          inner-radius="55"
          data='${JSON.stringify(data)}'>
        </fluent-donut-chart>
      </div>
    `);
    await page.waitForFunction(() => customElements.whenDefined('fluent-donut-chart'));

    const element = page.locator('fluent-donut-chart');
    const firstArc = element.locator('.arc').first();
    const defaultPath = await firstArc.getAttribute('d');

    await element.evaluate(el => {
      el.setAttribute('round-corners', 'true');
    });

    await expect(firstArc).not.toHaveAttribute('d', defaultPath ?? '');
  });
});

test.describe('Donut-chart - order', () => {
  const unorderedData: ChartProps = {
    chartData: [
      { legend: 'small', data: 5000 },
      { legend: 'large', data: 39000 },
      { legend: 'medium', data: 15000 },
    ],
  };

  test('Should render legends in default order when order is not set', async ({ page }) => {
    await page.goto(fixtureURL('components-donutchart--basic'));
    await page.setContent(/* html */ `
      <div>
        <fluent-donut-chart chart-title="Sorted test" inner-radius="55" data='${JSON.stringify(unorderedData)}'>
        </fluent-donut-chart>
      </div>
    `);
    await page.waitForFunction(() => customElements.whenDefined('fluent-donut-chart'));

    const element = page.locator('fluent-donut-chart');
    const legends = element.locator('.legend-text');
    await expect(legends.nth(0).getByText('small')).toBeVisible();
    await expect(legends.nth(1).getByText('large')).toBeVisible();
    await expect(legends.nth(2).getByText('medium')).toBeVisible();
  });

  test('Should render legends in sorted (descending) order when order="sorted"', async ({ page }) => {
    await page.goto(fixtureURL('components-donutchart--basic'));
    await page.setContent(/* html */ `
      <div>
        <fluent-donut-chart chart-title="Sorted test" inner-radius="55" order="sorted" data='${JSON.stringify(
          unorderedData,
        )}'>
        </fluent-donut-chart>
      </div>
    `);
    await page.waitForFunction(() => customElements.whenDefined('fluent-donut-chart'));

    const element = page.locator('fluent-donut-chart');
    const legends = element.locator('.legend-text');
    // Sorted descending: large (39000), medium (15000), small (5000)
    await expect(legends.nth(0).getByText('large')).toBeVisible();
    await expect(legends.nth(1).getByText('medium')).toBeVisible();
    await expect(legends.nth(2).getByText('small')).toBeVisible();
  });

  test('uses chart-title attr and calloutData for highlighted center text', async ({ page }) => {
    const calloutData: ChartProps = {
      chartData: [
        { legend: 'first', data: 20000, calloutData: '20K highlighted' },
        { legend: 'second', data: 39000 },
      ],
    };

    await page.goto(fixtureURL('components-donutchart--basic'));
    await page.setContent(/* html */ `
      <div>
        <fluent-donut-chart chart-title="Callout contract" value-inside-donut="39,000" inner-radius="55" data='${JSON.stringify(
          calloutData,
        )}'>
        </fluent-donut-chart>
      </div>
    `);
    await page.waitForFunction(() => customElements.whenDefined('fluent-donut-chart'));

    const element = page.locator('fluent-donut-chart');
    await expect(element.getByText('Callout contract')).toBeVisible();

    const firstPath = element.getByLabel('first,');
    await firstPath.dispatchEvent('mouseover');
    await expect(element.locator('.text-inside-donut')).toContainText('20K highlighted');
  });
});
