import { DotNet } from "../../d-ts/Microsoft.JSInterop";

type DragTarget = 'square' | 'hue';

interface HsvState {
  hue: number;
  saturation: number;
  value: number;
  dotNetHelper: DotNet.DotNetObject;
  square: HTMLElement;
  hueBar: HTMLElement;
  squareIndicator: HTMLElement;
  hueIndicator: HTMLElement;

  // Currently active drag target (null when not dragging).
  dragging: DragTarget | null;

  // Listeners bound to the element itself (kept for the lifetime of the
  // component). Stored so they can be removed on Dispose.
  squareMouseDown: (e: MouseEvent) => void;
  hueMouseDown: (e: MouseEvent) => void;
  squareTouchStart: (e: TouchEvent) => void;
  hueTouchStart: (e: TouchEvent) => void;

  // Listeners bound to `document` only during an active drag.
  documentMouseMove: (e: MouseEvent) => void;
  documentMouseUp: () => void;
  documentTouchMove: (e: TouchEvent) => void;
  documentTouchEnd: () => void;
}

const states = new Map<string, HsvState>();

export namespace Microsoft.FluentUI.Blazor.Components.ColorPicker {

  /**
   * Initializes the HSV color picker component.
   * @param dotNetHelper The .NET object reference for callbacks.
   * @param id The element ID of the color picker container.
   * @param initialHue Initial hue value (0-360).
   * @param initialSaturation Initial saturation value (0-1).
   * @param initialValue Initial brightness value (0-1).
   */
  export function Initialize(
    dotNetHelper: DotNet.DotNetObject,
    id: string,
    initialHue: number,
    initialSaturation: number,
    initialValue: number
  ): void {

    const container = document.getElementById(id);
    if (!container) {
      return;
    }

    const square = container.querySelector('[part="canvas"]') as HTMLElement;
    const hueBar = container.querySelector('[part="hue-bar"]') as HTMLElement;
    const squareIndicator = container.querySelector('[part="indicator"]') as HTMLElement;
    const hueIndicator = container.querySelector('[part="hue-indicator"]') as HTMLElement;

    if (!square || !hueBar || !squareIndicator || !hueIndicator) {
      return;
    }

    const onSquareMove = (clientX: number, clientY: number): void => {
      const state = states.get(id);
      if (!state) {
        return;
      }

      const rect = state.square.getBoundingClientRect();
      const s = Math.max(0, Math.min(1, (clientX - rect.left) / rect.width));
      const v = Math.max(0, Math.min(1, 1 - (clientY - rect.top) / rect.height));

      state.saturation = s;
      state.value = v;

      state.squareIndicator.style.left = `${s * 100}%`;
      state.squareIndicator.style.top = `${(1 - v) * 100}%`;

      const hex = hsvToHex(state.hue, s, v);
      state.dotNetHelper.invokeMethodAsync('FluentColorPicker.ColorChangedAsync', hex);
    };

    const onHueMove = (clientY: number): void => {
      const state = states.get(id);
      if (!state) {
        return;
      }

      const rect = state.hueBar.getBoundingClientRect();
      const t = Math.max(0, Math.min(1, (clientY - rect.top) / rect.height));
      const hue = t * 360;

      state.hue = hue;
      state.square.style.backgroundColor = `hsl(${hue}, 100%, 50%)`;
      state.hueIndicator.style.top = `${t * 100}%`;

      const hex = hsvToHex(hue, state.saturation, state.value);
      state.dotNetHelper.invokeMethodAsync('FluentColorPicker.ColorChangedAsync', hex);
    };

    // ---- Document-level handlers (attached only during an active drag) ----

    const documentMouseMove = (e: MouseEvent): void => {
      const state = states.get(id);
      if (!state || state.dragging === null) {
        return;
      }
      if (state.dragging === 'square') {
        onSquareMove(e.clientX, e.clientY);
      } else {
        onHueMove(e.clientY);
      }
    };

    const documentTouchMove = (e: TouchEvent): void => {
      const state = states.get(id);
      if (!state || state.dragging === null || e.touches.length < 1) {
        return;
      }
      if (e.cancelable) {
        e.preventDefault();
      }
      const touch = e.touches[0];
      if (state.dragging === 'square') {
        onSquareMove(touch.clientX, touch.clientY);
      } else {
        onHueMove(touch.clientY);
      }
    };

    const stopDragging = (): void => {
      const state = states.get(id);
      if (!state || state.dragging === null) {
        return;
      }
      state.dragging = null;
      document.removeEventListener('mousemove', state.documentMouseMove);
      document.removeEventListener('mouseup', state.documentMouseUp);
      document.removeEventListener('touchmove', state.documentTouchMove);
      document.removeEventListener('touchend', state.documentTouchEnd);
      document.removeEventListener('touchcancel', state.documentTouchEnd);
    };

    const documentMouseUp = (): void => stopDragging();
    const documentTouchEnd = (): void => stopDragging();

    const startDragging = (target: DragTarget): void => {
      const state = states.get(id);
      if (!state || state.dragging !== null) {
        return;
      }
      state.dragging = target;
      document.addEventListener('mousemove', state.documentMouseMove);
      document.addEventListener('mouseup', state.documentMouseUp);
      document.addEventListener('touchmove', state.documentTouchMove, { passive: false });
      document.addEventListener('touchend', state.documentTouchEnd);
      document.addEventListener('touchcancel', state.documentTouchEnd);
    };

    // ---- Element-level handlers (attached for the lifetime of the picker) ----

    const squareMouseDown = (e: MouseEvent): void => {
      startDragging('square');
      onSquareMove(e.clientX, e.clientY);
    };

    const hueMouseDown = (e: MouseEvent): void => {
      startDragging('hue');
      onHueMove(e.clientY);
    };

    const squareTouchStart = (e: TouchEvent): void => {
      if (e.touches.length === 0) {
        return;
      }
      startDragging('square');
      onSquareMove(e.touches[0].clientX, e.touches[0].clientY);
    };

    const hueTouchStart = (e: TouchEvent): void => {
      if (e.touches.length === 0) {
        return;
      }
      startDragging('hue');
      onHueMove(e.touches[0].clientY);
    };

    const state: HsvState = {
      hue: initialHue,
      saturation: initialSaturation,
      value: initialValue,
      dotNetHelper,
      square,
      hueBar,
      squareIndicator,
      hueIndicator,
      dragging: null,
      squareMouseDown,
      hueMouseDown,
      squareTouchStart,
      hueTouchStart,
      documentMouseMove,
      documentMouseUp,
      documentTouchMove,
      documentTouchEnd,
    };

    states.set(id, state);

    // Attach element-level listeners only. Document-level listeners are
    // attached on demand in startDragging() and removed in stopDragging(),
    // so the page remains fully scrollable on mobile when no drag is active.
    square.addEventListener('mousedown', squareMouseDown);
    hueBar.addEventListener('mousedown', hueMouseDown);
    square.addEventListener('touchstart', squareTouchStart, { passive: true });
    hueBar.addEventListener('touchstart', hueTouchStart, { passive: true });
  }

  /**
   * Disposes the HSV color picker state and removes all event listeners.
   * @param id The element ID of the color picker container.
   */
  export function Dispose(id: string): void {
    const state = states.get(id);
    if (!state) {
      return;
    }

    // Remove element-level listeners.
    state.square.removeEventListener('mousedown', state.squareMouseDown);
    state.hueBar.removeEventListener('mousedown', state.hueMouseDown);
    state.square.removeEventListener('touchstart', state.squareTouchStart);
    state.hueBar.removeEventListener('touchstart', state.hueTouchStart);

    // Remove any leftover document-level listeners (in case Dispose is
    // called mid-drag).
    document.removeEventListener('mousemove', state.documentMouseMove);
    document.removeEventListener('mouseup', state.documentMouseUp);
    document.removeEventListener('touchmove', state.documentTouchMove);
    document.removeEventListener('touchend', state.documentTouchEnd);
    document.removeEventListener('touchcancel', state.documentTouchEnd);

    states.delete(id);
  }

  function hsvToHex(h: number, s: number, v: number): string {
    const c = v * s;
    const x = c * (1 - Math.abs((h / 60) % 2 - 1));
    const m = v - c;
    let r: number, g: number, b: number;

    if (h < 60) { r = c; g = x; b = 0; }
    else if (h < 120) { r = x; g = c; b = 0; }
    else if (h < 180) { r = 0; g = c; b = x; }
    else if (h < 240) { r = 0; g = x; b = c; }
    else if (h < 300) { r = x; g = 0; b = c; }
    else { r = c; g = 0; b = x; }

    const toHex = (n: number): string => Math.round((n + m) * 255).toString(16).padStart(2, '0');
    return `#${toHex(r)}${toHex(g)}${toHex(b)}`.toUpperCase();
  }
}
