import { DotNet } from "../../../Core.Scripts/src/d-ts/Microsoft.JSInterop";

export namespace Microsoft.FluentUI.Blazor.Overflow {
  interface OverflowItem {
    Id: string;
    Overflow: boolean;
    Text: string;
  }

  interface OverflowElement extends HTMLElement {
    overflowSize?: number | null;
  }

  interface LastHandledState {
    id: string | null;
    isHorizontal: boolean | null;
  }

  let resizeObserver: ResizeObserver | undefined;
  let observerAddRemove: MutationObserver | undefined;
  let lastHandledState: LastHandledState = { id: null, isHorizontal: null };

  export function Initialize(dotNetHelper: DotNet.DotNetObject, id: string, isHorizontal: boolean, querySelector: string | null, threshold: number): void {
    let localSelector = querySelector;
    if (!localSelector) {
      localSelector = ".fluent-overflow-item";
    }

    observerAddRemove = new MutationObserver(mutations => {
      mutations.forEach(mutation => {
        if (mutation.type !== 'childList' && (mutation.addedNodes.length > 0 || mutation.removedNodes.length > 0)) {
          return;
        }
        const node = mutation.addedNodes.length > 0 ? mutation.addedNodes[0] : mutation.removedNodes[0];
        if (node.nodeType !== Node.ELEMENT_NODE || !(node as Element).matches(localSelector!)) {
          return;
        }
        Refresh(dotNetHelper, id, isHorizontal, querySelector, threshold);
      });
    });

    const el = document.getElementById(id);
    if (resizeObserver && el) {
      resizeObserver.unobserve(el);
    }

    let resizeTimeout: number | undefined;
    resizeObserver = new ResizeObserver(() => {
      clearTimeout(resizeTimeout);
      resizeTimeout = window.setTimeout(() => {
        Refresh(dotNetHelper, id, isHorizontal, querySelector, threshold);
      }, 100);
    });

    if (el) {
      resizeObserver.observe(el);
      observerAddRemove.observe(el, { childList: true, subtree: false });
    }

    lastHandledState.id = id;
    lastHandledState.isHorizontal = isHorizontal;
  }

  export function Refresh(dotNetHelper: DotNet.DotNetObject, id: string, isHorizontal: boolean, querySelector: string | null, threshold: number): void {
    const container = document.getElementById(id);
    if (!container) return;

    let localQuerySelector: string;
    if (!querySelector) {
      localQuerySelector = ":scope .fluent-overflow-item";
    } else {
      localQuerySelector = ":scope >" + querySelector;
    }

    const allItems = container.querySelectorAll<OverflowElement>(localQuerySelector);
    const items = container.querySelectorAll<OverflowElement>(localQuerySelector + ":not([fixed])");

    const fixedItemsFromSelector = container.querySelectorAll<OverflowElement>(localQuerySelector + "[fixed]");
    const otherFixedItems = container.querySelectorAll<OverflowElement>(":scope > [fixed]:not(.fluent-overflow-item)");
    const fixedItems = [
      ...Array.from(fixedItemsFromSelector),
      ...Array.from(otherFixedItems)
    ].filter(el => el.getAttribute("fixed") !== "ellipsis");

    const ellipsisItems = Array.from(container.querySelectorAll<OverflowElement>(localQuerySelector + "[fixed='ellipsis']"));

    let ellipsisTotal = 0;
    let containerGap = parseFloat(window.getComputedStyle(container).gap);
    if (!containerGap) containerGap = 0;

    ellipsisItems.forEach((el, idx) => {
      el.overflowSize = isHorizontal ? getElementWidth(el) : getElementHeight(el);
      ellipsisTotal += el.overflowSize || 0;
      if (idx > 0) ellipsisTotal += containerGap;
    });

    let itemsTotalSize = threshold > 0 ? 10 : 0;
    let containerMaxSize = isHorizontal ? container.offsetWidth : container.offsetHeight;
    let overflowChanged = false;

    containerMaxSize -= threshold;

    const availableSize = containerMaxSize - fixedItems.reduce((sum, el, idx) => sum + (el.overflowSize || 0) + (idx > 0 ? containerGap : 0), 0);

    if (ellipsisTotal > availableSize) {
      ellipsisItems.forEach(el => {
        el.style.flexShrink = "1";
      });
    } else {
      ellipsisItems.forEach(el => {
        el.style.flexShrink = "0";
      });
    }

    if (lastHandledState.id === id && lastHandledState.isHorizontal !== isHorizontal) {
      allItems.forEach(element => {
        element.removeAttribute("overflow");
        element.overflowSize = null;
      });
    }

    fixedItems.forEach(element => {
      element.overflowSize = isHorizontal ? getElementWidth(element) : getElementHeight(element);
      element.overflowSize = (element.overflowSize || 0) + containerGap;
      itemsTotalSize += element.overflowSize;
    });

    items.forEach(element => {
      const isOverflow = element.hasAttribute("overflow");
      if (!isOverflow) {
        element.overflowSize = isHorizontal ? getElementWidth(element) : getElementHeight(element);
        element.overflowSize = (element.overflowSize || 0) + containerGap;
      }
      itemsTotalSize += element.overflowSize || 0;
      if (containerMaxSize > 0) {
        if (itemsTotalSize > containerMaxSize) {
          if (!isOverflow) {
            element.setAttribute("overflow", "");
            overflowChanged = true;
          }
        } else {
          if (isOverflow) {
            element.removeAttribute("overflow");
            overflowChanged = true;
          }
        }
      }
    });

    if (overflowChanged) {
      const listOfOverflow: OverflowItem[] = [];
      items.forEach(element => {
        listOfOverflow.push({
          Id: element.id,
          Overflow: element.hasAttribute("overflow"),
          Text: element.innerText.trim()
        });
      });
      dotNetHelper.invokeMethodAsync("OverflowRaisedAsync", listOfOverflow);
    }

    lastHandledState.id = id;
    lastHandledState.isHorizontal = isHorizontal;
  }

  export function Dispose(id: string): void {
    const el = document.getElementById(id);
    if (el) {
      resizeObserver?.unobserve(el);
      observerAddRemove?.disconnect();
    }
  }

  function getElementWidth(element: HTMLElement): number {
    const style = window.getComputedStyle(element);
    const width = element.offsetWidth;
    const margin = parseFloat(style.marginLeft) + parseFloat(style.marginRight);
    return width + margin;
  }

  function getElementHeight(element: HTMLElement): number {
    const style = window.getComputedStyle(element);
    const height = element.offsetHeight;
    const margin = parseFloat(style.marginTop) + parseFloat(style.marginBottom);
    return height + margin;
  }
}
