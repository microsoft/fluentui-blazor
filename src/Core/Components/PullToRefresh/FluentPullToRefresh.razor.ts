export namespace Microsoft.FluentUI.Blazor.PullToRefresh {

  type MutableTouchList = Touch[] & {
    item: (index: number) => Touch | null;
    identifiedTouch: (id: number) => Touch | null;
    length: number;
  };

  interface TouchCoordinates {
    pageX: number;
    pageY: number;
    screenX: number;
    screenY: number;
    clientX: number;
    clientY: number;
  }

  class PolyfilledTouch implements Touch {
    constructor(
      public readonly target: EventTarget,
      public readonly identifier: number,
      pos: TouchCoordinates,
      deltaX = 0,
      deltaY = 0) {
      this.clientX = pos.clientX + deltaX;
      this.clientY = pos.clientY + deltaY;
      this.screenX = pos.screenX + deltaX;
      this.screenY = pos.screenY + deltaY;
      this.pageX = pos.pageX + deltaX;
      this.pageY = pos.pageY + deltaY;
    }

    readonly clientX: number;
    readonly clientY: number;
    readonly screenX: number;
    readonly screenY: number;
    readonly pageX: number;
    readonly pageY: number;
    readonly radiusX = 0;
    readonly radiusY = 0;
    readonly rotationAngle = 0;
    readonly force = 0;
    readonly altitudeAngle = 0;
    readonly azimuthAngle = 0;
    readonly touchType: TouchType = "direct";
  }

  let emulatorInitialized = false;

  export function GetScrollDistToTop(): number {
    const scrollTop = document.documentElement.scrollTop || document.body.scrollTop || 0;
    return Math.round(scrollTop);
  }

  export function GetScrollDistToBottom(): number {
    const dist = document.documentElement.scrollHeight -
      document.documentElement.scrollTop -
      document.documentElement.clientHeight;
    return Math.round(dist);
  }

  export function InitTouchEmulator(): void {
    loadTouchEmulator();
  }

  function loadTouchEmulator(): void {
    if (typeof window === "undefined" || emulatorInitialized) {
      return;
    }

    let eventTarget: Element | null = null;
    const supportTouch = "ontouchstart" in window;

    const docAny = document as Document & {
      createTouch?: (
        view: Window,
        target: EventTarget,
        identifier: number,
        pageX: number,
        pageY: number,
        screenX: number,
        screenY: number) => Touch;
      createTouchList?: (...touches: Touch[]) => TouchList;
    };

    if (!docAny.createTouch) {
      docAny.createTouch = (
        view,
        target,
        identifier,
        pageX,
        pageY,
        screenX,
        screenY) => new PolyfilledTouch(target, identifier, {
        pageX,
        pageY,
        screenX,
        screenY,
        clientX: pageX - window.scrollX,
        clientY: pageY - window.scrollY,
      });
    }

    if (!docAny.createTouchList) {
      docAny.createTouchList = (...touches: Touch[]) => createTouchList(touches);
    }

    if (!Element.prototype.matches) {
      Element.prototype.matches = (Element.prototype as any).msMatchesSelector || Element.prototype.matches;
    }

    if (!Element.prototype.closest) {
      Element.prototype.closest = function (selector: string): Element | null {
        let el: Element | null = this;
        while (el && el.nodeType === 1) {
          if (el.matches(selector)) {
            return el;
          }
          el = el.parentElement;
        }
        return null;
      };
    }

    let initiated = false;

    const onMouse = (touchType: string) => (ev: MouseEvent) => {
      if (ev.type === "mousedown") {
        initiated = true;
      }

      if (ev.type === "mouseup") {
        initiated = false;
      }

      if (ev.type === "mousemove" && !initiated) {
        return;
      }

      const candidate = ev.target as Element | null;
      if (ev.type === "mousedown" || !eventTarget || (eventTarget && !eventTarget.dispatchEvent)) {
        eventTarget = candidate;
      }

      if (eventTarget && eventTarget.closest && eventTarget.closest("[data-no-touch-simulate]") == null) {
        triggerTouch(touchType, ev, eventTarget);
      }

      if (ev.type === "mouseup") {
        eventTarget = null;
      }
    };

    const triggerTouch = (eventName: string, mouseEv: MouseEvent, target: Element) => {
      const touchEvent = new Event(eventName, {bubbles: true, cancelable: true});
      (touchEvent as any).altKey = mouseEv.altKey;
      (touchEvent as any).ctrlKey = mouseEv.ctrlKey;
      (touchEvent as any).metaKey = mouseEv.metaKey;
      (touchEvent as any).shiftKey = mouseEv.shiftKey;

      const touches = getActiveTouches(mouseEv, target);
      (touchEvent as any).touches = touches;
      (touchEvent as any).targetTouches = touches;
      (touchEvent as any).changedTouches = createTouchListFromMouse(target, mouseEv);

      target.dispatchEvent(touchEvent);
    };

    const createTouchFromMouse = (target: Element, mouseEv: MouseEvent): Touch =>
      new PolyfilledTouch(
        target,
        1,
        {
          pageX: mouseEv.pageX,
          pageY: mouseEv.pageY,
          screenX: mouseEv.screenX,
          screenY: mouseEv.screenY,
          clientX: mouseEv.clientX,
          clientY: mouseEv.clientY,
        });

    const createTouchList = (touches: Touch[]): MutableTouchList => {
      const touchList = [...touches] as MutableTouchList;
      touchList.item = (index: number) => touchList[index] ?? null;
      touchList.identifiedTouch = (id: number) => touchList[id + 1] ?? null;
      return touchList;
    };

    const createTouchListFromMouse = (target: Element, mouseEv: MouseEvent): MutableTouchList =>
      createTouchList([createTouchFromMouse(target, mouseEv)]);

    const getActiveTouches = (mouseEv: MouseEvent, target: Element): MutableTouchList => {
      if (mouseEv.type === "mouseup") {
        return createTouchList([]);
      }
      return createTouchListFromMouse(target, mouseEv);
    };

    const touchEmulator = () => {
      window.addEventListener("mousedown", onMouse("touchstart"), true);
      window.addEventListener("mousemove", onMouse("touchmove"), true);
      window.addEventListener("mouseup", onMouse("touchend"), true);
    };

    (touchEmulator as any).multiTouchOffset = 75;

    if (!supportTouch) {
      touchEmulator();
    }

    emulatorInitialized = true;
  }


}
