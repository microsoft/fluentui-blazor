import { afterEach, beforeAll, beforeEach, describe, expect, it, vi } from "vitest";
import { Microsoft } from "./FluentOverflow";

interface OverflowState {
  overflowItems: Array<{ Id: string; Text: string; Index?: number }>;
  overflowCount: number;
  firstOverflowIndex: number;
  orderedItemIds: string[];
}

interface TestOverflowElement extends HTMLElement {
  refresh(): void;
  getOverflowState(): OverflowState;
}

class TestResizeObserver {
  static instances: TestResizeObserver[] = [];

  disconnected = false;

  constructor(private readonly callback: ResizeObserverCallback) {
    TestResizeObserver.instances.push(this);
  }

  observe(): void { }

  unobserve(): void { }

  disconnect(): void {
    this.disconnected = true;
  }

  trigger(): void {
    this.callback([], this as unknown as ResizeObserver);
  }
}

class TestMutationObserver {
  static instances: TestMutationObserver[] = [];

  disconnected = false;

  constructor(private readonly callback: MutationCallback) {
    TestMutationObserver.instances.push(this);
  }

  observe(): void { }

  takeRecords(): MutationRecord[] {
    return [];
  }

  disconnect(): void {
    this.disconnected = true;
  }

  trigger(records: MutationRecord[]): void {
    this.callback(records, this as unknown as MutationObserver);
  }
}

describe("FluentOverflow", () => {
  let frameCallbacks: Map<number, FrameRequestCallback>;
  let requestedFrameCount: number;
  let canceledFrameCount: number;
  let nextFrameId: number;

  beforeAll(() => {
    vi.stubGlobal("ResizeObserver", TestResizeObserver);
    vi.stubGlobal("MutationObserver", TestMutationObserver);
    Microsoft.FluentUI.Blazor.Components.Overflow.registerComponent(undefined as never, undefined as never);
  });

  beforeEach(() => {
    vi.useFakeTimers();
    document.body.replaceChildren();
    TestResizeObserver.instances = [];
    TestMutationObserver.instances = [];
    frameCallbacks = new Map();
    requestedFrameCount = 0;
    canceledFrameCount = 0;
    nextFrameId = 1;

    vi.stubGlobal("requestAnimationFrame", (callback: FrameRequestCallback) => {
      const frameId = nextFrameId++;
      requestedFrameCount++;
      frameCallbacks.set(frameId, callback);
      return frameId;
    });
    vi.stubGlobal("cancelAnimationFrame", (frameId: number) => {
      if (frameCallbacks.delete(frameId)) {
        canceledFrameCount++;
      }
    });
  });

  afterEach(() => {
    document.body.replaceChildren();
    vi.useRealTimers();
  });

  it("coalesces connection and synchronous attribute changes into one frame", () => {
    const fixture = createOverflowFixture([20, 20]);

    fixture.element.setAttribute("threshold", "10");
    fixture.element.setAttribute("max-rendered-items", "1");
    fixture.element.setAttribute("store-overflow-in-memory", "true");

    expect(requestedFrameCount).toBe(1);
    runAnimationFrame(frameCallbacks);
    expect(fixture.measurementCount()).toBe(1);
  });

  it("coalesces connection, mutation, and resize notifications into one frame", () => {
    createOverflowFixture([20]);

    getMutationObserver().trigger([childListMutation(document.querySelector("fluent-overflow")!)]);
    getResizeObserver().trigger();
    vi.advanceTimersByTime(16);

    expect(requestedFrameCount).toBe(1);
    expect(frameCallbacks).toHaveLength(1);
  });

  it("schedules a later resize in a new frame", () => {
    const fixture = createOverflowFixture([20]);
    runAnimationFrame(frameCallbacks);

    fixture.width.value = 80;
    getResizeObserver().trigger();
    vi.advanceTimersByTime(16);

    expect(requestedFrameCount).toBe(2);
    runAnimationFrame(frameCallbacks);
    expect(fixture.measurementCount()).toBe(3);
  });

  it("ignores an initial resize notification after the connected frame", () => {
    const fixture = createOverflowFixture([20]);
    runAnimationFrame(frameCallbacks);

    getResizeObserver().trigger();
    vi.advanceTimersByTime(16);

    expect(requestedFrameCount).toBe(1);
    expect(frameCallbacks).toHaveLength(0);
    expect(fixture.measurementCount()).toBe(2);
  });

  it("cancels pending work when disconnected and schedules fresh work when reconnected", () => {
    const fixture = createOverflowFixture([20]);
    const firstResizeObserver = getResizeObserver();
    const firstMutationObserver = getMutationObserver();

    fixture.element.remove();

    expect(frameCallbacks).toHaveLength(0);
    expect(canceledFrameCount).toBe(1);
    expect(firstResizeObserver.disconnected).toBe(true);
    expect(firstMutationObserver.disconnected).toBe(true);
    runAnimationFrame(frameCallbacks);
    expect(fixture.measurementCount()).toBe(0);

    document.body.append(fixture.element);

    expect(requestedFrameCount).toBe(2);
    expect(TestResizeObserver.instances).toHaveLength(2);
    expect(TestMutationObserver.instances).toHaveLength(2);
    runAnimationFrame(frameCallbacks);
    expect(fixture.measurementCount()).toBe(1);
  });

  it("flushes pending work for imperative refresh and state reads", () => {
    const refreshFixture = createOverflowFixture([20], 100, "imperative-refresh");

    Microsoft.FluentUI.Blazor.Components.Overflow.Refresh(refreshFixture.element.id);

    expect(frameCallbacks).toHaveLength(0);
    expect(canceledFrameCount).toBe(1);
    expect(refreshFixture.measurementCount()).toBe(1);
    Microsoft.FluentUI.Blazor.Components.Overflow.GetOverflowState(refreshFixture.element.id);
    expect(refreshFixture.measurementCount()).toBe(1);

    const stateFixture = createOverflowFixture([20, 20, 20], 70, "state-read");
    const state = Microsoft.FluentUI.Blazor.Components.Overflow.GetOverflowState(stateFixture.element.id);

    expect(frameCallbacks).toHaveLength(0);
    expect(canceledFrameCount).toBe(2);
    expect(state).toMatchObject({
      overflowCount: 2,
      firstOverflowIndex: 1,
      orderedItemIds: ["item-0", "item-1", "item-2"]
    });
    expect(state.overflowItems.map(item => item.Text)).toEqual(["Item 1", "Item 2"]);
  });

  it("cancels pending observer timers when state is requested", () => {
    const fixture = createOverflowFixture([20], 100);
    runAnimationFrame(frameCallbacks);
    getMutationObserver().trigger([attributeMutation(fixture.items[0], "style")]);

    fixture.element.getOverflowState();

    expect(fixture.measurementCount()).toBe(2);
    vi.advanceTimersByTime(16);
    expect(frameCallbacks).toHaveLength(0);
    expect(fixture.measurementCount()).toBe(2);
  });

  it("keeps all items in the DOM while limiting only the returned payload", () => {
    const fixture = createOverflowFixture([20, 20, 20, 20], 1);
    fixture.element.setAttribute("max-rendered-items", "2");
    const limitedState = fixture.element.getOverflowState();

    expect(fixture.element.children).toHaveLength(4);
    expect(fixture.element.querySelectorAll("[overflow]")).toHaveLength(4);
    expect(limitedState.overflowCount).toBe(4);
    expect(limitedState.overflowItems).toHaveLength(2);

    fixture.element.setAttribute("max-rendered-items", "0");
    const unlimitedState = fixture.element.getOverflowState();
    expect(unlimitedState.overflowCount).toBe(4);
    expect(unlimitedState.overflowItems).toHaveLength(4);
  });

  it("preserves fixed and ellipsis items outside the managed state", () => {
    const fixture = createOverflowFixture([30, 30, 30], 50);
    fixture.items[0].setAttribute("behavior", "fixed");
    fixture.items[1].setAttribute("behavior", "ellipsis");

    const state = fixture.element.getOverflowState();

    expect(state.orderedItemIds).toEqual(["item-2"]);
    expect(state.overflowCount).toBe(1);
    expect(fixture.items[0].hasAttribute("overflow")).toBe(false);
    expect(fixture.items[1].hasAttribute("overflow")).toBe(false);
    expect(fixture.items[1].style.flexShrink).toBe("1");
  });

  it("remeasures in the new dimension after an orientation change", () => {
    const fixture = createOverflowFixture([20], 100);
    runAnimationFrame(frameCallbacks);
    expect(fixture.itemWidthMeasurements[0]()).toBe(1);
    expect(fixture.itemHeightMeasurements[0]()).toBe(0);

    fixture.element.setAttribute("orientation", "vertical");
    runAnimationFrame(frameCallbacks);

    expect(fixture.itemHeightMeasurements[0]()).toBe(1);
  });

  it("invalidates only the mutated child's cached measurement", () => {
    const fixture = createOverflowFixture([20, 20], 100);
    runAnimationFrame(frameCallbacks);

    getMutationObserver().trigger([attributeMutation(fixture.items[0], "style")]);
    vi.advanceTimersByTime(16);
    runAnimationFrame(frameCallbacks);

    expect(fixture.itemWidthMeasurements[0]()).toBe(2);
    expect(fixture.itemWidthMeasurements[1]()).toBe(1);
  });

  it("ignores mutations below direct children", () => {
    const fixture = createOverflowFixture([20], 100);
    const nestedChild = document.createElement("span");
    fixture.items[0].append(nestedChild);
    runAnimationFrame(frameCallbacks);

    getMutationObserver().trigger([
      childListMutation(fixture.items[0]),
      attributeMutation(nestedChild, "style")
    ]);
    vi.advanceTimersByTime(16);

    expect(frameCallbacks).toHaveLength(0);
    expect(fixture.measurementCount()).toBe(1);
  });

  it("does not emit an unchanged payload and reveals only after measurement", () => {
    const fixture = createOverflowFixture([20], 100, undefined, false);
    let eventCount = 0;
    fixture.element.addEventListener("overflowchange", () => eventCount++);

    expect(fixture.element.style.visibility).toBe("hidden");
    runAnimationFrame(frameCallbacks);
    expect(fixture.element.style.visibility).toBe("");
    expect(eventCount).toBe(1);

    fixture.element.setAttribute("threshold", "24");
    runAnimationFrame(frameCallbacks);
    expect(eventCount).toBe(1);
  });

  function runAnimationFrame(callbacks: Map<number, FrameRequestCallback>): void {
    const scheduledCallbacks = [...callbacks.values()];
    callbacks.clear();
    for (const callback of scheduledCallbacks) {
      callback(0);
    }
  }

  function getResizeObserver(): TestResizeObserver {
    return TestResizeObserver.instances.at(-1)!;
  }

  function getMutationObserver(): TestMutationObserver {
    return TestMutationObserver.instances.at(-1)!;
  }
});

function createOverflowFixture(itemWidths: number[], containerWidth = 100, id?: string, visibleOnLoad = true) {
  const element = document.createElement("fluent-overflow") as TestOverflowElement;
  const width = { value: containerWidth };
  let measurementCount = 0;
  Object.defineProperty(element, "offsetWidth", {
    configurable: true,
    get: () => {
      measurementCount++;
      return width.value;
    }
  });
  Object.defineProperty(element, "offsetHeight", { configurable: true, get: () => width.value });

  if (id) {
    element.id = id;
  }
  if (!visibleOnLoad) {
    element.setAttribute("visible-on-load", "false");
  }

  const itemWidthMeasurements: Array<() => number> = [];
  const itemHeightMeasurements: Array<() => number> = [];
  const items = itemWidths.map((itemWidth, index) => {
    const item = document.createElement("span") as HTMLElement & { overflowSize?: number | null };
    let widthMeasurements = 0;
    let heightMeasurements = 0;
    item.id = `item-${index}`;
    item.textContent = `Item ${index}`;
    item.style.margin = "0px";
    Object.defineProperty(item, "offsetWidth", {
      configurable: true,
      get: () => {
        widthMeasurements++;
        return itemWidth;
      }
    });
    Object.defineProperty(item, "offsetHeight", {
      configurable: true,
      get: () => {
        heightMeasurements++;
        return itemWidth;
      }
    });
    itemWidthMeasurements.push(() => widthMeasurements);
    itemHeightMeasurements.push(() => heightMeasurements);
    element.append(item);
    return item;
  });

  document.body.append(element);
  return {
    element,
    items,
    width,
    measurementCount: () => measurementCount,
    itemWidthMeasurements,
    itemHeightMeasurements
  };
}

function childListMutation(target: Node): MutationRecord {
  return { type: "childList", target } as MutationRecord;
}

function attributeMutation(target: Node, attributeName: string): MutationRecord {
  return { type: "attributes", target, attributeName } as MutationRecord;
}