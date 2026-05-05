import { attr, FASTElement, observable } from '@microsoft/fast-element';
import { create as d3Create, select as d3Select } from 'd3-selection';
import { getRTL, jsonConverter, lightenColor, SVG_NAMESPACE_URI, validateChartPropsArray } from '../utils/chart-helpers.js';
import type { ChartDataPoint, ChartProps } from './horizontal-bar-chart.options.js';
import { Variant } from './horizontal-bar-chart.options.js';

/**
 * A Horizontal Bar Chart HTML Element.
 *
 * @public
 */
export class HorizontalBarChart extends FASTElement {
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

  @attr({ attribute: 'hide-labels', mode: 'boolean' })
  public hideLabels: boolean = false;

  @attr({ attribute: 'round-corners', mode: 'boolean' })
  public roundCorners: boolean = false;

  @attr({ attribute: 'chart-data-mode' })
  public chartDataMode: 'default' | 'fraction' | 'percentage' = 'default';

  @attr({ attribute: 'hide-legends', mode: 'boolean' })
  public hideLegends: boolean = false;

  @attr({ attribute: 'hide-tooltip', mode: 'boolean' })
  public hideTooltip: boolean = false;

  @attr({ attribute: 'legend-list-label' })
  public legendListLabel?: string;

  @attr({ attribute: 'chart-title' })
  public chartTitle?: string;

  @attr
  public culture?: string;

  @attr({ attribute: 'allow-multiple-legend-selection', mode: 'boolean' })
  public allowMultipleLegendSelection: boolean = false;

  @attr({ attribute: 'enable-gradient', mode: 'boolean' })
  public enableGradient: boolean = false;

  @observable
  public legends: ChartDataPoint[] = [];

  @observable
  public activeLegend: string = '';
  protected activeLegendChanged(_oldValue: string, _newValue: string) {
    if (this._isSettingActiveLegend) {
      return;
    }

    this._updateLegendInteractionState();
  }

  @observable
  public isLegendSelected: boolean = false;

  @observable
  public selectedLegends: string[] = [];

  @observable
  public tooltipProps = {
    isVisible: false,
    legend: '',
    yValue: '',
    color: '',
    xPos: 0,
    yPos: 0,
  };

  public chartContainer!: HTMLDivElement;
  public elementInternals: ElementInternals = this.attachInternals();

  private _isRTL: boolean = false;
  private _barHeight: number = 12;
  private _bars: SVGRectElement[] = [];
  private _isSettingActiveLegend: boolean = false;

  constructor() {
    super();

    this.elementInternals.role = 'region';
  }

  public handleLegendMouseoverAndFocus(legendTitle: string) {
    if (this.allowMultipleLegendSelection) {
      if (this.selectedLegends.length > 0) {
        return;
      }
    } else {
      if (this.isLegendSelected) {
        return;
      }
    }

    this._setActiveLegend(legendTitle);
  }

  public handleLegendMouseoutAndBlur() {
    if (this.allowMultipleLegendSelection) {
      if (this.selectedLegends.length > 0) {
        return;
      }
    } else {
      if (this.isLegendSelected) {
        return;
      }
    }

    this._setActiveLegend('');
  }

  public handleLegendClick(legendTitle: string) {
    if (this.allowMultipleLegendSelection) {
      const nextSelection = this.selectedLegends.includes(legendTitle)
        ? this.selectedLegends.filter(legend => legend !== legendTitle)
        : [...this.selectedLegends, legendTitle];
      this.selectedLegends = nextSelection;
      if (nextSelection.length === 0) {
        this._setActiveLegend('');
      } else if (!nextSelection.includes(this.activeLegend)) {
        this._setActiveLegend(nextSelection[nextSelection.length - 1]);
      } else {
        this._updateLegendInteractionState();
      }
      return;
    }

    if (this.isLegendSelected && this.activeLegend === legendTitle) {
      this._setActiveLegend('');
      this.isLegendSelected = false;
    } else {
      this._setActiveLegend(legendTitle);
      this.isLegendSelected = true;
    }
  }

  public isLegendItemSelected(legendTitle: string) {
    return Array.isArray(this.selectedLegends) && this.selectedLegends.includes(legendTitle);
  }

  public isLegendItemDimmed(legendTitle: string) {
    const highlighted = this._getHighlightedLegends();
    return highlighted.length > 0 && !highlighted.includes(legendTitle);
  }

  connectedCallback() {
    // Class field initializers create own data properties that shadow the FAST @attr
    // and @observable reactive getter/setters on the prototype. Delete them so that
    // attribute changes go through the FAST reactive system and trigger the *Changed()
    // callbacks, and so that observable assignments notify template bindings.
    const self = this as Record<string, unknown>;
    const attrFields = [
      'width',
      'height',
      'variant',
      'data',
      'hideRatio',
      'hideLabels',
      'roundCorners',
      'chartDataMode',
      'hideLegends',
      'hideTooltip',
      'legendListLabel',
      'chartTitle',
      'culture',
      'allowMultipleLegendSelection',
      'enableGradient',
    ] as const;
    const observableFields = [
      'legends',
      'activeLegend',
      'isLegendSelected',
      'tooltipProps',
      'selectedLegends',
    ] as const;
    const saved: Partial<Record<(typeof attrFields)[number], unknown>> = {};
    const savedObservables: Partial<Record<(typeof observableFields)[number], unknown>> = {};
    for (const field of attrFields) {
      saved[field] = self[field];
      delete self[field];
    }
    for (const field of observableFields) {
      savedObservables[field] = self[field];
      delete self[field];
      if (savedObservables[field] !== undefined) {
        self[field] = savedObservables[field];
      }
    }

    super.connectedCallback();

    for (const field of attrFields) {
      if (self[field] === undefined && saved[field] !== undefined) {
        self[field] = saved[field];
      }
    }

    if (!this.data) {
      return;
    }

    this._initializeAll();
  }

  public disconnectedCallback() {
    super.disconnectedCallback();
  }

  attributeChangedCallback(name: string, oldValue: string | null, newValue: string | null) {
    super.attributeChangedCallback(name, oldValue, newValue);

    if (oldValue === newValue) {
      return;
    }

    const booleanValue = newValue !== null && newValue !== 'false';

    if (name === 'round-corners') {
      this.roundCorners = booleanValue;
    }
    if (name === 'hide-ratio') {
      this.hideRatio = booleanValue;
    }
    if (name === 'hide-labels') {
      this.hideLabels = booleanValue;
    }
    if (name === 'hide-legends') {
      this.hideLegends = booleanValue;
    }
    if (name === 'hide-tooltip') {
      this.hideTooltip = booleanValue;
    }
    if (name === 'allow-multiple-legend-selection') {
      this.allowMultipleLegendSelection = booleanValue;
    }
  }

  protected dataChanged(_oldValue: ChartProps[], newValue: ChartProps[]) {
    if (newValue) {
      this._scheduleRender();
    }
  }

  protected chartTitleChanged() {
    if (this.$fastController.isConnected) {
      this.elementInternals.ariaLabel =
        this.chartTitle || `Horizontal bar chart with ${this.data?.length ?? 0} categories.`;
    }
  }

  protected widthChanged() {
    this._scheduleRender();
  }

  protected heightChanged() {
    this._scheduleRender();
  }

  protected variantChanged() {
    this._scheduleRender();
  }

  protected hideRatioChanged() {
    this._scheduleRender();
  }

  protected hideLabelsChanged() {
    this._scheduleRender();
  }

  protected roundCornersChanged() {
    this._scheduleRender();
  }

  protected enableGradientChanged() {
    this._scheduleRender();
  }

  protected chartDataModeChanged() {
    this._scheduleRender();
  }

  protected legendListLabelChanged() {
    this._scheduleRender();
  }

  protected cultureChanged() {
    this._scheduleRender();
  }

  protected allowMultipleLegendSelectionChanged() {
    if (!this.allowMultipleLegendSelection) {
      this.selectedLegends = [];
      this._setActiveLegend('');
      this.isLegendSelected = false;
      return;
    }

    this._updateLegendInteractionState();
  }

  protected selectedLegendsChanged() {
    this._updateLegendInteractionState();
  }

  private _renderPending = false;

  /**
   * Schedules a single re-render deferred to the next event-loop task,
   * batching multiple simultaneous attribute changes into one render pass.
   */
  private _scheduleRender(): void {
    if (this._renderPending) {
      return;
    }
    this._renderPending = true;
    setTimeout(() => {
      this._renderPending = false;
      this._rerender();
    }, 0);
  }

  private _rerender() {
    if (!this.$fastController.isConnected || !this.data) {
      return;
    }
    this._clearChart();
    this._initializeAll();
    this._updateLegendInteractionState();
  }

  private _getHighlightedLegends(): string[] {
    if (this.allowMultipleLegendSelection) {
      if (Array.isArray(this.selectedLegends) && this.selectedLegends.length > 0) {
        return this.selectedLegends;
      }
      return this.activeLegend ? [this.activeLegend] : [];
    }
    return this.activeLegend ? [this.activeLegend] : [];
  }

  private _updateLegendInteractionState() {
    this._applyActiveLegendState();
    this._applyLegendButtonState();
  }

  private _applyActiveLegendState() {
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

  private _applyLegendButtonState() {
    const legendButtons = this.shadowRoot?.querySelectorAll<HTMLButtonElement>('.legend');
    if (!legendButtons) {
      return;
    }

    const highlighted = this._getHighlightedLegends();
    legendButtons.forEach(button => {
      const title = button.querySelector('.legend-text')?.textContent ?? '';
      const isActive = highlighted.length === 0 || highlighted.includes(title);
      button.classList.toggle('inactive', !isActive);
      button.setAttribute('aria-selected', `${highlighted.includes(title)}`);
    });
  }

  private _setActiveLegend(value: string) {
    this._isSettingActiveLegend = true;
    this.activeLegend = value;
    this._isSettingActiveLegend = false;
    this._updateLegendInteractionState();
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
    this.elementInternals.ariaLabel = this.chartTitle || `Horizontal bar chart with ${this.data.length} categories.`;
    this._applyHostDimensions();

    this._initializeData();
    this._renderChart();
  }

  private _applyHostDimensions() {
    if (this.width === undefined || this.width === null || this.width === '') {
      this.style.removeProperty('width');
    } else {
      this.style.width = this._toCssLength(this.width);
    }

    if (this.height === undefined || this.height === null || this.height === '') {
      this.style.removeProperty('height');
    } else {
      this.style.height = this._toCssLength(this.height);
    }
  }

  private _toCssLength(value: number | string) {
    return typeof value === 'number' || /^\d+(\.\d+)?$/.test(value) ? `${value}px` : value;
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

    // create a div element. Loop through chart bars and add to the div as its children
    d3Select(nodes[index])
      .attr('key', index)
      .attr('id', `_MSBC_bar-${index}`)
      .node()!
      .appendChild(singleChartBars.node());
  }

  private _hydrateLegends() {
    // Create a map to store unique legends
    const uniqueLegendsMap = new Map();

    // Iterate through all chart points and populate the map
    for (const dataSeries of this.data) {
      for (const point of dataSeries.chartData!) {
        if ((point as any).placeholder === true) {
          continue;
        }
        // Check if the legend is already in the map
        if (!uniqueLegendsMap.has(point.legend)) {
          uniqueLegendsMap.set(point.legend, {
            legend: point.legend,
            data: point.data,
            color: point.gradient ? point.gradient[0] : point.color,
          });
        }
      }
    }

    // Convert the map values back to an array
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
    // calculating starting point of each bar and it's range
    const startingPoint: number[] = [];
    const barTotalValue = data.chartData!.reduce((acc: number, point: ChartDataPoint) => acc + (point.data ?? 0), 0);
    const total = this.variant === Variant.AbsoluteScale ? longestBarTotalValue : barTotalValue;

    let sumOfPercent = 0;
    data.chartData!.map((point: ChartDataPoint, index: number) => {
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

    // Include an imaginary placeholder bar with value equal to
    // the difference between longestBarTotalValue and barTotalValue
    // while calculating sumOfPercent to get correct scalingRatio for absolute-scale variant
    if (this.variant === Variant.AbsoluteScale) {
      let value = total === 0 ? 0 : ((total - barTotalValue) / total) * 100;
      if (value < 1 && value !== 0) {
        value = 1;
      } else if (value > 99 && value !== 100) {
        value = 99;
      }
      sumOfPercent += value;
    }

    /**
     * The %age of the space occupied by the margin needs to subtracted
     * while computing the scaling ratio, since the margins are not being
     * scaled down, only the data is being scaled down from a higher percentage to lower percentage
     * Eg: 95% of the space is taken by the bars, 5% by the margins
     * Now if the sumOfPercent is 120% -> This needs to be scaled down to 95%, not 100%
     * since that's only space available to the bars
     */

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

      const gEle = d3Select(g) // 'this' refers to the current 'g' element
        .attr('key', index)
        .attr('role', 'img')
        .attr('aria-label', pointData);

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
        .attr('style', (this.enableGradient || point.gradient) ? `fill:url(#${gradientId})` : `fill:${point.color!}`)
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
      // Compute total: prefer explicit total field, fall back to sum of all bar data
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
        // 'default' mode: show ratio when there are exactly 2 data points and hideRatio is false
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

        const highlighted = this._getHighlightedLegends();
        if (highlighted.length > 0 && d.legend && !highlighted.includes(d.legend)) {
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
        this.tooltipProps = { isVisible: false, legend: '', yValue: '', color: '', xPos: 0, yPos: 0 };
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
