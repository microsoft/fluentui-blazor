export namespace Microsoft.FluentUI.Blazor.DragContainer {

  interface DotNetReference {
    invokeMethodAsync(methodName: string, ...args: any[]): Promise<any>;
  }

  const ZONE_SELECTOR = '[data-fluent-dropzone]';
  const DEFAULT_LONG_PRESS_DELAY = 200;
  const MOVE_TOLERANCE = 10;
  const OVER_THROTTLE = 100;
  const SCROLL_EDGE = 48;
  const SCROLL_SPEED = 12;

  const instances = new Map<string, TouchDragHandler>();

  /**
   * Enables touch dragging (long press, then move) for the drop zones inside the container.
   * HTML5 drag and drop events are not raised by touch devices such as iPhone or iPad.
   */
  export function Initialize(containerId: string, dotNetRef: DotNetReference, longPressDelay: number): void {
    const container = document.getElementById(containerId);
    if (!container) {
      return;
    }

    Dispose(containerId);
    instances.set(containerId, new TouchDragHandler(container, dotNetRef, longPressDelay > 0 ? longPressDelay : DEFAULT_LONG_PRESS_DELAY));
  }

  export function Dispose(containerId: string): void {
    const handler = instances.get(containerId);
    if (handler) {
      handler.dispose();
      instances.delete(containerId);
    }
  }

  class TouchDragHandler {
    private queue: Promise<any> = Promise.resolve();
    private timer: number | undefined;
    private touchId: number | null = null;
    private startX = 0;
    private startY = 0;
    private lastX = 0;
    private lastY = 0;
    private offsetX = 0;
    private offsetY = 0;
    private candidate: HTMLElement | null = null;
    private source: HTMLElement | null = null;
    private ghost: HTMLElement | null = null;
    private target: HTMLElement | null = null;
    private lastOver = 0;
    private scrollFrame: number | undefined;

    constructor(
      private readonly container: HTMLElement,
      private readonly dotNetRef: DotNetReference,
      private readonly longPressDelay: number) {
      container.addEventListener('touchstart', this.onTouchStart, { passive: true });
      container.addEventListener('touchmove', this.onTouchMove, { passive: false });
      container.addEventListener('touchend', this.onTouchEnd);
      container.addEventListener('touchcancel', this.onTouchCancel);
      container.addEventListener('dragstart', this.onNativeDragStart, true);
      container.addEventListener('contextmenu', this.onContextMenu);
    }

    dispose(): void {
      this.container.removeEventListener('touchstart', this.onTouchStart);
      this.container.removeEventListener('touchmove', this.onTouchMove);
      this.container.removeEventListener('touchend', this.onTouchEnd);
      this.container.removeEventListener('touchcancel', this.onTouchCancel);
      this.container.removeEventListener('dragstart', this.onNativeDragStart, true);
      this.container.removeEventListener('contextmenu', this.onContextMenu);
      this.clearTimer();
      this.cleanup();
    }

    private readonly onTouchStart = (e: TouchEvent): void => {
      if (this.touchId !== null || e.touches.length !== 1) {
        return;
      }

      const zone = this.findZone(e.target as Element | null);
      if (!zone || zone.getAttribute('data-draggable') !== 'true' || this.isInteractive(e.target as Element, zone)) {
        return;
      }

      // With nested containers, only the innermost one (first to see the event) handles it.
      if ((e as any).__fluentDragHandled) {
        return;
      }
      (e as any).__fluentDragHandled = true;

      const touch = e.changedTouches[0];
      this.touchId = touch.identifier;
      this.candidate = zone;
      this.startX = this.lastX = touch.clientX;
      this.startY = this.lastY = touch.clientY;

      this.clearTimer();
      this.timer = window.setTimeout(() => this.beginDrag(), this.longPressDelay);
    };

    private readonly onTouchMove = (e: TouchEvent): void => {
      const touch = this.findTouch(e.changedTouches);
      if (!touch) {
        return;
      }

      this.lastX = touch.clientX;
      this.lastY = touch.clientY;

      if (!this.source) {
        // Still waiting for the long press: moving means the user wants to scroll.
        if (Math.hypot(touch.clientX - this.startX, touch.clientY - this.startY) > MOVE_TOLERANCE) {
          this.clearTimer();
          this.touchId = null;
          this.candidate = null;
        }
        return;
      }

      if (e.cancelable) {
        e.preventDefault();
      }

      this.updateDrag();
    };

    private readonly onTouchEnd = (e: TouchEvent): void => {
      const touch = this.findTouch(e.changedTouches);
      if (!touch) {
        return;
      }

      this.lastX = touch.clientX;
      this.lastY = touch.clientY;
      this.clearTimer();

      if (!this.source) {
        this.touchId = null;
        this.candidate = null;
        return;
      }

      if (e.cancelable) {
        e.preventDefault();
      }

      this.updateTarget();
      this.finish(true);
    };

    private readonly onTouchCancel = (e: TouchEvent): void => {
      if (!this.findTouch(e.changedTouches)) {
        return;
      }

      this.clearTimer();
      if (this.source) {
        this.finish(false);
      } else {
        this.touchId = null;
        this.candidate = null;
      }
    };

    // Prevents the browser native drag (iOS can start one after a long press) from interfering.
    private readonly onNativeDragStart = (e: Event): void => {
      if (this.candidate || this.source) {
        e.preventDefault();
        e.stopPropagation();
      }
    };

    private readonly onContextMenu = (e: Event): void => {
      if (this.candidate || this.source) {
        e.preventDefault();
      }
    };

    private beginDrag(): void {
      this.timer = undefined;
      const zone = this.candidate;
      if (!zone || !zone.isConnected) {
        this.touchId = null;
        this.candidate = null;
        return;
      }

      this.source = zone;
      const rect = zone.getBoundingClientRect();
      this.offsetX = this.startX - rect.left;
      this.offsetY = this.startY - rect.top;

      this.ghost = this.createGhost(zone, rect);
      document.body.appendChild(this.ghost);
      zone.setAttribute('data-touch-dragging', 'true');
      this.positionGhost();

      if (navigator.vibrate) {
        navigator.vibrate(10);
      }

      this.send('TouchDragStartAsync', zone.id);
      this.updateTarget();
      this.scrollFrame = window.requestAnimationFrame(this.autoScroll);
    }

    private updateDrag(): void {
      this.positionGhost();
      const changed = this.updateTarget();

      const now = Date.now();
      if (!changed && this.target && now - this.lastOver >= OVER_THROTTLE) {
        this.lastOver = now;
        this.send('TouchDragOverAsync', this.source!.id, this.target.id);
      }
    }

    // Returns true when the zone under the finger changed.
    private updateTarget(): boolean {
      const zone = this.zoneFromPoint(this.lastX, this.lastY);
      if (zone === this.target) {
        return false;
      }

      const previous = this.target;
      this.target = zone;

      if (previous) {
        this.send('TouchDragLeaveAsync', this.source!.id, previous.id);
      }

      if (zone) {
        this.send('TouchDragEnterAsync', this.source!.id, zone.id);
      }

      return true;
    }

    private finish(drop: boolean): void {
      const source = this.source;
      const target = this.target;

      if (source) {
        if (drop && target && target.getAttribute('data-droppable') === 'true') {
          this.send('TouchDropAsync', source.id, target.id);
        } else if (target) {
          this.send('TouchDragLeaveAsync', source.id, target.id);
        }

        this.send('TouchDragEndAsync', source.id);
      }

      this.cleanup();
    }

    private cleanup(): void {
      if (this.scrollFrame !== undefined) {
        window.cancelAnimationFrame(this.scrollFrame);
        this.scrollFrame = undefined;
      }

      this.ghost?.remove();
      this.source?.removeAttribute('data-touch-dragging');

      this.ghost = null;
      this.source = null;
      this.target = null;
      this.candidate = null;
      this.touchId = null;
    }

    private createGhost(zone: HTMLElement, rect: DOMRect): HTMLElement {
      const ghost = zone.cloneNode(true) as HTMLElement;
      ghost.removeAttribute('id');
      ghost.removeAttribute('data-fluent-dropzone');
      ghost.removeAttribute('dragged-over');
      ghost.querySelectorAll('[id]').forEach(el => el.removeAttribute('id'));

      const style = ghost.style;
      style.position = 'fixed';
      style.left = '0';
      style.top = '0';
      style.width = `${rect.width}px`;
      style.height = `${rect.height}px`;
      style.margin = '0';
      style.boxSizing = 'border-box';
      style.pointerEvents = 'none';
      style.opacity = '0.85';
      style.zIndex = '2147483647';
      style.boxShadow = '0 8px 24px rgba(0, 0, 0, 0.3)';
      style.transition = 'none';
      style.userSelect = 'none';
      style.setProperty('-webkit-user-select', 'none');
      style.setProperty('-webkit-touch-callout', 'none');
      return ghost;
    }

    private positionGhost(): void {
      if (this.ghost) {
        const x = this.lastX - this.offsetX;
        const y = this.lastY - this.offsetY;
        this.ghost.style.transform = `translate3d(${x}px, ${y}px, 0) scale(1.03)`;
      }
    }

    private zoneFromPoint(x: number, y: number): HTMLElement | null {
      // The ghost has pointer-events: none, so it never hides the element below.
      const element = document.elementFromPoint(x, y);
      let zone = this.findZone(element);

      while (zone && zone !== this.source && zone.getAttribute('data-droppable') !== 'true') {
        zone = this.findZone(zone.parentElement);
      }

      if (zone === this.source) {
        return null;
      }

      return zone;
    }

    private findZone(element: Element | null): HTMLElement | null {
      // Nested containers share the same DOM: each one only handles the zones registered with it.
      const zone = element?.closest(`${ZONE_SELECTOR}[data-container-id="${this.container.id}"]`) as HTMLElement | null;
      return zone && this.container.contains(zone) ? zone : null;
    }

    private isInteractive(element: Element, zone: HTMLElement): boolean {
      const interactive = element.closest('button, a[href], input, select, textarea, [contenteditable="true"], [data-no-drag]');
      return !!interactive && zone.contains(interactive);
    }

    private readonly autoScroll = (): void => {
      if (!this.source) {
        return;
      }

      const scrollable = this.findScrollable(document.elementFromPoint(this.lastX, this.lastY));
      let moved = false;

      if (scrollable) {
        const rect = scrollable === document.scrollingElement
          ? { top: 0, bottom: window.innerHeight, left: 0, right: window.innerWidth }
          : scrollable.getBoundingClientRect();

        let dx = 0;
        let dy = 0;
        if (this.lastY < rect.top + SCROLL_EDGE) { dy = -SCROLL_SPEED; }
        else if (this.lastY > rect.bottom - SCROLL_EDGE) { dy = SCROLL_SPEED; }
        if (this.lastX < rect.left + SCROLL_EDGE) { dx = -SCROLL_SPEED; }
        else if (this.lastX > rect.right - SCROLL_EDGE) { dx = SCROLL_SPEED; }

        if (dx !== 0 || dy !== 0) {
          const beforeLeft = scrollable.scrollLeft;
          const beforeTop = scrollable.scrollTop;
          scrollable.scrollBy(dx, dy);
          moved = scrollable.scrollLeft !== beforeLeft || scrollable.scrollTop !== beforeTop;
        }
      }

      if (moved) {
        this.updateTarget();
      }

      this.scrollFrame = window.requestAnimationFrame(this.autoScroll);
    };

    private findScrollable(element: Element | null): Element | null {
      let current: Element | null = element;
      while (current && current !== document.body && current !== document.documentElement) {
        const style = getComputedStyle(current);
        const canY = /(auto|scroll)/.test(style.overflowY) && current.scrollHeight > current.clientHeight;
        const canX = /(auto|scroll)/.test(style.overflowX) && current.scrollWidth > current.clientWidth;
        if (canY || canX) {
          return current;
        }
        current = current.parentElement;
      }

      return document.scrollingElement;
    }

    private findTouch(list: TouchList): Touch | null {
      for (let i = 0; i < list.length; i++) {
        if (list[i].identifier === this.touchId) {
          return list[i];
        }
      }

      return null;
    }

    private clearTimer(): void {
      if (this.timer !== undefined) {
        window.clearTimeout(this.timer);
        this.timer = undefined;
      }
    }

    // Calls are chained so the .NET side receives them in the order they happened.
    private send(method: string, ...args: any[]): void {
      this.queue = this.queue
        .then(() => this.dotNetRef.invokeMethodAsync(method, ...args))
        .catch(() => { /* component may be disposed */ });
    }
  }
}
