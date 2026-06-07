// chart-tooltip-bridge.ts
// ES module that wires a Blazor-rendered tooltip portal into the chart web component's
// shadow DOM by supplying a `tooltipRenderer` function on the custom element.
//
// Flow:
//  1. Blazor calls `initTooltipBridge(chartId, portalId, dotNetRef)` in OnAfterRenderAsync.
//  2. This module waits for the custom element to be defined, then:
//       a. Sets up a MutationObserver on the hidden portal div so that whenever Blazor
//          re-renders the template the updated content is pushed into shadow DOM.
//       b. Sets `tooltipRenderer` on the element instance.
//  3. When the chart fires the renderer callback (on hover):
//       a. We notify Blazor fire-and-forget via `UpdateTooltipContextAsync` so it updates
//          the portal div with the hovered data point.
//       b. We return the default tooltip HTML immediately so the tooltip appears at once.
//       c. Once Blazor's render completes the MutationObserver fires and swaps in the
//          custom Blazor-rendered content automatically.
//  4. `destroyTooltipBridge` removes the renderer, disconnects the observer, and cleans up.

interface Bridge {
  element: HTMLElement & { tooltipRenderer?: TooltipRenderer };
  dotNetRef: any;
  observer: MutationObserver;
  shadowObserver: MutationObserver;
}

type TooltipRenderer = (dataPoint: unknown, defaultRender: (dataPoint: unknown) => string) => string;

const _bridges = new Map<string, Bridge>();

/**
 * Initialises the tooltip bridge for a single chart instance.
 * @param chartId   The `id` attribute of the fluent-*-chart element.
 * @param portalId  The `id` of the hidden Blazor portal div.
 * @param dotNetRef A DotNetObjectReference to the Blazor component.
 */
export async function initTooltipBridge(chartId: string, portalId: string, dotNetRef: any): Promise<void> {
  const chartEl = document.getElementById(chartId) as (HTMLElement & { tooltipRenderer?: TooltipRenderer }) | null;
  if (!chartEl) {
    return;
  }

  // Wait until the custom element is upgraded (defined + connected).
  if (customElements.get(chartEl.tagName.toLowerCase()) === undefined) {
    await customElements.whenDefined(chartEl.tagName.toLowerCase());
  }

  // Portal div where Blazor renders the custom tooltip template.
  const portal = document.getElementById(portalId);

  // Stores the latest portal content clone waiting to be pushed into .tooltip-body.
  // Set whenever Blazor re-renders the portal; cleared after a successful push.
  // This decouples the two async events (Blazor render + FAST .tooltip-body insertion)
  // so whichever arrives second can complete the push — critical for charts like
  // FunnelChart where mouseout/mouseover cycles hide/show .tooltip-body rapidly.
  let _pendingContent: Element | null = null;

  /**
   * Push _pendingContent into .tooltip-body if both are available.
   * Safe to call at any time; no-ops when either is absent or already pushed.
   */
  const tryPushToTooltipBody = (): void => {
    if (!_pendingContent) return;
    const tooltipBody = chartEl.shadowRoot?.querySelector(".tooltip-body");
    if (!tooltipBody) return;
    tooltipBody.innerHTML = "";
    tooltipBody.appendChild(_pendingContent.cloneNode(true));
    _pendingContent = null;
  };

  /**
   * Called by the portal MutationObserver whenever Blazor re-renders the portal.
   * Stores the updated content and immediately tries to push it to .tooltip-body.
   * If .tooltip-body is not yet in the shadow DOM (FAST hasn't run its RAF yet),
   * the content is kept in _pendingContent for the shadow observer to push later.
   */
  const pushPortalContent = (): void => {
    if (!portal?.firstElementChild) return;
    _pendingContent = portal.firstElementChild.cloneNode(true) as Element;
    tryPushToTooltipBody();
  };

  // Watch the portal for any DOM change caused by Blazor re-rendering the template.
  const observer = new MutationObserver(pushPortalContent);
  if (portal) {
    observer.observe(portal, {
      childList: true,
      subtree: true,
      characterData: true,
    });
  }

  // Watch the chart's shadow root for .tooltip-body being inserted by FAST's RAF.
  // When the tooltip becomes visible, FAST inserts .tooltip-body via requestAnimationFrame.
  // For charts with rapid show/hide cycles (e.g. FunnelChart), Blazor's async render may
  // finish only after .tooltip-body has already been removed. The shadow observer ensures
  // _pendingContent is pushed the next time .tooltip-body appears, regardless of order.
  const shadowObserver = new MutationObserver(tryPushToTooltipBody);
  if (chartEl.shadowRoot) {
    shadowObserver.observe(chartEl.shadowRoot, {
      childList: true,
      subtree: true,
    });
  }

  /**
   * The tooltipRenderer assigned to the WC property.
   *
   * Returns the default tooltip HTML synchronously so the tooltip is visible
   * immediately. At the same time, notifies Blazor fire-and-forget so it can
   * re-render the portal with custom content. Once Blazor's OnAfterRenderAsync
   * completes it calls updateTooltipContent() to swap in the custom HTML.
   *
   * @param dataPoint    The raw JS data point object from the chart.
   * @param defaultRender The chart's built-in default renderer.
   * @returns Default tooltip HTML (shown immediately).
   */
  const renderer: TooltipRenderer = (dataPoint: any, defaultRender): string => {
    // Extract common fields; fall back to empty strings for axis charts.
    const legend: string = dataPoint?.legend ?? dataPoint?.stage ?? "";
    const yValue: string =
      dataPoint?.yValue ??
      dataPoint?.value ??
      dataPoint?.yAxisCalloutData ??
      (dataPoint?.data != null ? String(dataPoint.data) : null) ??
      String(dataPoint?.y ?? "");

    // For GanttChart dataPoint.x is { start, end } — guard against [object Object].
    const rawX: unknown = dataPoint?.x;
    const xIsRange = rawX !== null && typeof rawX === "object";
    const xValue: string =
      dataPoint?.xValue ??
      dataPoint?.xAxisCalloutData ??
      (xIsRange ? "" : String(rawX ?? ""));

    // XStart / XEnd: ISO date strings for GanttChart ranges; empty for all other charts.
    const xStart: string = xIsRange ? _toISODateString((rawX as { start: unknown }).start) : "";
    const xEnd: string = xIsRange ? _toISODateString((rawX as { end: unknown }).end) : "";

    const color: string = dataPoint?.color ?? "";
    const rawJson: string = _safeStringify(dataPoint);

    // Fire-and-forget: tell Blazor about the new data point.
    // The MutationObserver will push the updated portal content once Blazor re-renders.
    dotNetRef
      .invokeMethodAsync(
        "UpdateTooltipContextAsync",
        legend,
        yValue,
        xValue,
        color,
        rawJson,
        xStart,
        xEnd,
      )
      .catch(() => {
        /* component disposed — ignore */
      });

    // Return default content immediately so the tooltip is shown at once
    // while Blazor re-renders the portal in the background.
    return defaultRender(dataPoint);
  };

  // Assign the renderer to the element property.
  chartEl.tooltipRenderer = renderer;

  _bridges.set(chartId, {
    element: chartEl,
    dotNetRef,
    observer,
    shadowObserver,
  });
}

/**
 * Removes the tooltip bridge for a chart instance and disposes the DotNet reference.
 * Called from `FluentChartBase.DisposeAsync`.
 * @param chartId
 */
export function destroyTooltipBridge(chartId: string): void {
  const bridge = _bridges.get(chartId);
  if (bridge) {
    bridge.element.tooltipRenderer = undefined;
    bridge.observer?.disconnect();
    bridge.shadowObserver?.disconnect();
    _bridges.delete(chartId);
  }
}

// ---------------------------------------------------------------------------
// Helpers
// ---------------------------------------------------------------------------

/**
 * Converts a date-like value to a string suitable for Blazor to parse.
 * Strings are returned unchanged (already ISO-formatted from C# JSON serialization).
 * Date objects are converted to ISO strings. Numbers are stringified.
 * Returns an empty string for null/undefined.
 */
function _toISODateString(value: unknown): string {
  if (value == null) return "";
  if (typeof value === "string") return value;
  if (typeof value === "number") return String(value);
  if (value instanceof Date)
    return isNaN(value.getTime()) ? "" : value.toISOString();
  return String(value);
}

/**
 * Safely serialises a value to JSON, handling circular references.
 */
function _safeStringify(value: unknown): string {
  try {
    return JSON.stringify(value);
  } catch {
    return "{}";
  }
}
