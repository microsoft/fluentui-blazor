import { attr } from '@microsoft/fast-element';
import { ChartBase } from '../utils/chart-base.js';
import { create as d3Create, select as d3Select } from 'd3-selection';
import {
  getRTL,
  jsonConverter,
  lightenColor,
  SVG_NAMESPACE_URI,
  validateChartPropsArray,
} from '../utils/chart-helpers.js';
import type { ChartDataPoint, ChartProps } from './horizontal-bar-chart.options.js';
import type { Legend } from '../utils/chart.options.js';
import { Variant } from './horizontal-bar-chart.options.js';

/**
 * A Horizontal Bar Chart HTML Element.
 *
 * @public
 */
export class HorizontalBarChart extends ChartBase {
  @attr
  public width?: number | string;

  @attr
  public height?: number | string;

  @attr
  public variant?: Variant;

  @attr({ converter: jsonConverter })
  public data!: ChartProps[];

  @attr({ attribute: 'hide-ratio', mode: 'boolean' })
  public hideRatio: boolean = false;

  @attr({ attribute: 'chart-data-mode' })
  public chartDataMode: 'default' | 'fraction' | 'percentage' = 'default';

  @attr({ attribute: 'enable-gradient', mode: 'boolean' })
  public enableGradient: boolean = false;

  private _barHeight: number = 12;
  private _bars: SVGRectElement[] = [];

  connectedCallback() {
    // Class field initializers create own data properties that shadow the FAST @attr
    // and @observable reactive getter/setters on the prototype. Delete them so that
    // attribute changes go through the FAST reactive system and trigger the *Changed()
    // callbacks, and so that observable assignments notify template bindings.
    const self = this as Record<string, unknown>;
    const attrFields = ['width', 'height', 'variant', 'data', 'hideRatio', 'chartDataMode', 'enableGradient'] as const;
    const saved: Partial<Record<(typeof attrFields)[number], unknown>> = {};

    for (const field of attrFields) {
      saved[field] = self[field];
      delete self[field];
    }

    super.connectedCallback();

    for (const field of attrFields) {
      if (self[field] === undefined && saved[field] !== undefined) {
        self[field] = saved[field];
      }
    }

    this._requestRender();
  }

  protected dataChanged(_oldValue: ChartProps[], newValue: ChartProps[]) {
    if (newValue) {
      this._requestRender();
    }
  }

  protected _getHostAriaLabel(): string {
    return this.chartTitle || `Horizontal bar chart with ${this.data?.length ?? 0} categories.`;
  }

  protected widthChanged() {
    this._requestRender();
  }

  protected heightChanged() {
    this._requestRender();
  }

  protected variantChanged() {
    this._requestRender();
  }

  protected hideRatioChanged() {
    this._requestRender();
  }

  protected chartDataModeChanged() {
    this._requestRender();
  }

  protected enableGradientChanged() {
    this._requestRender();
  }

  protected _performRender(): void {
    if (!this.$fastController.isConnected || !this.data) {
      return;
    }
    this._clearChart();
    this._initializeAll();
    this._updateLegendInteractionState();
  }

  protected _applyActiveLegendState() {
    const highlighted = this._getHighlightedLegends();
    if (highlighted.length === 0) {
      this._bars?.forEach(bar => bar.classList.remove('inactive'));
    } else {
      this._bars?.forEach(bar => {
        const barLegend = bar.getAttribute('barinfo');
        const isActive = barLegend !== null && highlighted.includes(barLegend);
        bar.classList.toggle('inactive', !isActive);
      });
    }
  }

  private _clearChart() {
    if (this.chartContainer) {
      while (this.chartContainer.firstChild) {
        this.chartContainer.removeChild(this.chartContainer.firstChild);
      }
    }
    this._bars = [];
  }

  private _initializeAll() {
    validateChartPropsArray(this.data, 'data');

    this._isRTL = getRTL(this);
    this.elementInternals.ariaLabel = this._getHostAriaLabel();
    this._applyHostDimensions();

    this._initializeData();
    this._renderChart();
  }

  protected _applyHostDimensions() {
    super._applyHostDimensions(this.width, this.height);
  }

  private _initializeData() {
    if (this.variant === Variant.SingleBar) {
      this._hydrateData();
    }
    this._hydrateLegends();
  }

  private _renderChart() {
    const chartContainerDiv = d3Select(this.chartContainer);
    chartContainerDiv
      .selectAll('div')
      .data(this.data!)
      .enter()
      .append('div')
      .each((d, i, nodes) => {
        this._createSingleChartBars(d, i, nodes);
      });
  }

  private _createSingleChartBars(singleChartData: ChartProps, index: number, nodes: any) {
    const singleChartBars = this._createBarsAndLegends(singleChartData!, index);

    d3Select(nodes[index])
      .attr('key', index)
      .attr('id', `_MSBC_bar-${index}`)
      .node()!
      .appendChild(singleChartBars.node());
  }

  private _hydrateLegends() {
    const uniqueLegendsMap = new Map();

    for (const dataSeries of this.data) {
      for (const point of dataSeries.chartData!) {
        if ((point as any).placeholder === true) {
          continue;
        }
        if (!uniqueLegendsMap.has(point.legend)) {
          uniqueLegendsMap.set(point.legend, {
            legend: point.legend,
            color: point.gradient ? point.gradient[0] : point.color,
          });
        }
      }
    }

    this.legends = Array.from(uniqueLegendsMap.values());
  }

  private _hydrateData() {
    this.data!.forEach(({ chartData }) => {
      if (chartData!.length === 1) {
        const pointData = chartData![0];
        const newEntry = {
          legend: '',
          data: Math.max(pointData.total! - pointData.data!, 0),
          y: pointData.total!,
          color: '#edebe9',
          placeholder: true,
        };
        chartData!.push(newEntry);
      }
    });
  }

  private _calculateBarSpacing(): number {
    const svgWidth = this.getBoundingClientRect().width;
    let barSpacing = 0;
    const MARGIN_WIDTH_IN_PX = 3;
    if (svgWidth) {
      const currentBarSpacing = (MARGIN_WIDTH_IN_PX / svgWidth) * 100;
      barSpacing = currentBarSpacing;
    }
    return barSpacing;
  }

  private _createBarsAndLegends(data: ChartProps, barNo?: number) {
    const _isRTL = this._isRTL;
    const _computeLongestBarTotalValue = () => {
      let longestBarTotalValue = 0;
      this.data!.forEach(({ chartData }) => {
        const barTotalValue = chartData!.reduce((acc: number, point: ChartDataPoint) => acc + (point.data ?? 0), 0);
        longestBarTotalValue = Math.max(longestBarTotalValue, barTotalValue);
      });
      return longestBarTotalValue;
    };
    const longestBarTotalValue = _computeLongestBarTotalValue();
    const noOfBars =
      data.chartData?.reduce((count: number, point: ChartDataPoint) => (count += (point.data || 0) > 0 ? 1 : 0), 0) ||
      1;
    const barSpacingInPercent = this._calculateBarSpacing();
    const totalMarginPercent = barSpacingInPercent * (noOfBars - 1);
    const startingPoint: number[] = [];
    const barTotalValue = data.chartData!.reduce((acc: number, point: ChartDataPoint) => acc + (point.data ?? 0), 0);
    const total = this.variant === Variant.AbsoluteScale ? longestBarTotalValue : barTotalValue;

    let sumOfPercent = 0;
    data.chartData!.map((point: ChartDataPoint) => {
      const pointData = point.data ?? 0;
      const currValue = (pointData / total) * 100;
      let value = currValue ?? 0;

      if (value < 1 && value !== 0) {
        value = 1;
      } else if (value > 99 && value !== 100) {
        value = 99;
      }
      sumOfPercent += value;

      return sumOfPercent;
    });

    if (this.variant === Variant.AbsoluteScale) {
      let value = total === 0 ? 0 : ((total - barTotalValue) / total) * 100;
      if (value < 1 && value !== 0) {
        value = 1;
      } else if (value > 99 && value !== 100) {
        value = 99;
      }
      sumOfPercent += value;
    }

    const scalingRatio = sumOfPercent !== 0 ? sumOfPercent / (100 - totalMarginPercent) : 1;

    let prevPosition = 0;
    let value = 0;

    const createBars = (g: SVGGElement, point: ChartDataPoint, index: number) => {
      const barHeight = 12;
      const pointData = point.data ?? 0;
      if (index > 0) {
        prevPosition += value;
      }
      value = (pointData / total) * 100 ? (pointData / total) * 100 : 0;
      if (value < 1 && value !== 0) {
        value = 1 / scalingRatio;
      } else if (value > 99 && value !== 100) {
        value = 99 / scalingRatio;
      } else {
        value = value / scalingRatio;
      }

      startingPoint.push(prevPosition);

      const gEle = d3Select(g).attr('key', index).attr('role', 'img').attr('aria-label', pointData);

      let gradientId = '';
      if (this.enableGradient || point.gradient) {
        const defs = document.createElementNS(SVG_NAMESPACE_URI, 'defs');
        gEle.node()!.appendChild(defs);

        const linearGradient = document.createElementNS(SVG_NAMESPACE_URI, 'linearGradient');
        defs.appendChild(linearGradient);
        gradientId = `gradient-${barNo}-${index}`;
        linearGradient.setAttribute('id', gradientId);
        linearGradient.setAttribute('x1', _isRTL ? '100%' : '0%');
        linearGradient.setAttribute('x2', _isRTL ? '0%' : '100%');
        linearGradient.setAttribute('y1', '0%');
        linearGradient.setAttribute('y2', '0%');

        const stop1 = document.createElementNS(SVG_NAMESPACE_URI, 'stop');
        linearGradient.appendChild(stop1);
        stop1.setAttribute('offset', '0%');
        stop1.setAttribute('stop-color', (point.gradient ?? [lightenColor(point.color!, 0.35), point.color!])[0]);

        const stop2 = document.createElementNS(SVG_NAMESPACE_URI, 'stop');
        linearGradient.appendChild(stop2);
        stop2.setAttribute('offset', '100%');
        stop2.setAttribute('stop-color', (point.gradient ?? [lightenColor(point.color!, 0.35), point.color!])[1]);
      }

      const rect = gEle
        .append('rect')
        .attr('key', index)
        .attr('id', `${barNo}-${index}`)
        .attr('barinfo', `${point.legend}`)
        .attr('class', 'bar')
        .attr('style', this.enableGradient || point.gradient ? `fill:url(#${gradientId})` : `fill:${point.color!}`)
        .attr('rx', `${this.roundCorners ? 3 : 0}`)
        .attr(
          'x',
          `${
            _isRTL
              ? 100 - startingPoint[index] - value - barSpacingInPercent * index
              : startingPoint[index] + barSpacingInPercent * index
          }%`,
        )
        .attr('y', 0)
        .attr('width', value + '%')
        .attr('height', barHeight)
        .attr('tabindex', 0);
      this._bars.push(rect.node()!);
    };

    const containerDiv = d3Create('div').attr(
      'style',
      'position: relative; margin-bottom: var(--spacingVerticalMNudge);',
    );

    const barTitleDiv = containerDiv.append('div').attr('class', 'bar-title-div');
    barTitleDiv
      .append('div')
      .append('span')
      .attr('class', 'bar-title')
      .text(data?.chartSeriesTitle ? data?.chartSeriesTitle : '');

    const showChartDataText = this.variant !== Variant.AbsoluteScale;

    if (!this.hideLabels && showChartDataText) {
      const numData = data!.chartData![0].data ?? 0;
      const explicitTotal = data!.chartData![0].total;
      const sumTotal = data!.chartData!.reduce((acc: number, p: ChartDataPoint) => acc + (p.data ?? 0), 0);
      const barTotal = explicitTotal !== undefined ? explicitTotal : sumTotal;

      if (data.chartDataText) {
        const chartTitleRight = document.createElement('div');
        barTitleDiv.node()!.appendChild(chartTitleRight);
        chartTitleRight.classList.add('chart-data-text');
        chartTitleRight.textContent = data.chartDataText;
      } else if (this.chartDataMode === 'fraction') {
        const ratioDiv = barTitleDiv.append('div').attr('role', 'text');
        ratioDiv.append('span').attr('class', 'ratio-numerator').text(numData);
        ratioDiv.append('span').attr('class', 'ratio-denominator').text(`/${barTotal}`);
      } else if (this.chartDataMode === 'percentage') {
        const percentage = barTotal > 0 ? Math.round((numData / barTotal) * 100) : 0;
        barTitleDiv
          .append('div')
          .attr('role', 'text')
          .append('span')
          .attr('class', 'ratio-numerator')
          .text(`${percentage}%`);
      } else {
        const showRatio = !this.hideRatio && data!.chartData!.length === 2;
        if (showRatio) {
          const ratioDiv = barTitleDiv.append('div').attr('role', 'text');
          ratioDiv.append('span').attr('class', 'ratio-numerator').text(numData);
          ratioDiv.append('span').attr('class', 'ratio-denominator').text(`/${barTotal}`);
        }
      }
    }

    const svgDiv = containerDiv.append('div').attr('style', 'display: flex;');
    const svgEle = svgDiv
      .append('svg')
      .attr('height', 12)
      .attr('width', 100 + '%')
      .attr('class', 'svg-chart')
      .attr(
        'aria-label',
        data?.chartSeriesTitle ??
          `Series with ${data.chartData.length}${data.chartData.length > 1 ? ' stacked' : ''} bars.`,
      )
      .selectAll('g')
      .data(data.chartData!)
      .enter()
      .append('g')
      .each(function (this, d, i) {
        createBars(this, d, i);
      })
      .on('mouseover', (event, d) => {
        if (d && d.hasOwnProperty('placeholder') && (d as any).placeholder === true) {
          return;
        }

        if (!this._shouldShowTooltip(d.legend)) {
          return;
        }

        const bounds = this.getBoundingClientRect();
        const centerX = window.innerWidth / 2;
        const xPos = Math.max(0, Math.min(centerX, window.innerWidth));

        this.tooltipProps = {
          isVisible: true,
          legend: d.legend,
          yValue: d.data.toLocaleString(this.culture || undefined),
          color: d.gradient ? d.gradient[0] : d.color!,
          xPos: this._isRTL ? bounds.right - event.clientX : Math.min(event.clientX - bounds.left, xPos),
          yPos: event.clientY - bounds.top - 40,
        };
      })
      .on('mouseout', () => {
        this._clearTooltip();
      });

    if (this.variant === Variant.AbsoluteScale) {
      const showLabel = !this.hideLabels;
      const barLabel = barTotalValue;
      if (showLabel) {
        if (Math.round((startingPoint[startingPoint.length - 1] || 0) + value + totalMarginPercent) === 100) {
          svgDiv
            .append('text')
            .attr('key', 'text')
            .attr('style', 'margin-top: -4.5px; margin-left: 2px;')
            .attr('class', 'bar-label')
            .attr(
              'x',
              `${
                this._isRTL
                  ? 100 - (startingPoint[startingPoint.length - 1] || 0) - value - totalMarginPercent
                  : (startingPoint[startingPoint.length - 1] || 0) + value + totalMarginPercent
              }%`,
            )
            .attr('textAnchor', 'start')
            .attr('y', this._barHeight / 2 + 6)
            .attr('dominantBaseline', 'central')
            .attr('transform', `translate(${this._isRTL ? -4 : 4})`)
            .attr('aria-label', `Total: ${barLabel}`)
            .attr('role', 'img')
            .text(barLabel);
        } else {
          svgEle
            .append('text')
            .attr('key', 'text')
            .attr('class', 'bar-label')
            .attr(
              'x',
              `${
                this._isRTL
                  ? 100 - (startingPoint[startingPoint.length - 1] || 0) - value - totalMarginPercent
                  : (startingPoint[startingPoint.length - 1] || 0) + value + totalMarginPercent
              }%`,
            )
            .attr('textAnchor', 'start')
            .attr('y', this._barHeight / 2 + 6)
            .attr('dominantBaseline', 'central')
            .attr('transform', `translate(${this._isRTL ? -4 : 4})`)
            .attr('aria-label', `Total: ${barLabel}`)
            .attr('role', 'img')
            .text(barLabel);
        }
      }
    }

    if (data.benchmarkData) {
      const benchmarkContainer = document.createElement('div');
      containerDiv.node()!.appendChild(benchmarkContainer);
      benchmarkContainer.classList.add('benchmark-container');

      const triangle = document.createElement('div');
      benchmarkContainer.appendChild(triangle);
      triangle.classList.add('triangle');

      const benchmarkRatio = (data.benchmarkData / total) * 100;
      triangle.style['insetInlineStart'] = `calc(${benchmarkRatio}% - 4px)`;
    }

    return containerDiv;
  }
}
