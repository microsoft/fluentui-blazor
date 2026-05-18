import { DotNet } from "../../d-ts/Microsoft.JSInterop";

interface HsvState {
  hue: number;
  saturation: number;
  value: number;
  draggingSquare: boolean;
  draggingHue: boolean;
  dotNetHelper: DotNet.DotNetObject;
  square: HTMLElement;
  hueBar: HTMLElement;
  squareIndicator: HTMLElement;
  hueIndicator: HTMLElement;
  mouseMoveHandler: (e: MouseEvent) => void;
  mouseUpHandler: () => void;
  touchMoveHandler: (e: TouchEvent) => void;
  touchEndHandler: () => void;
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

    const mouseMoveHandler = (e: MouseEvent): void => {
      const state = states.get(id);
      if (!state) {
        return;
      }

      if (state.draggingSquare) {
        onSquareMove(e.clientX, e.clientY);
      }
      if (state.draggingHue) {
        onHueMove(e.clientY);
      }
    };

    const mouseUpHandler = (): void => {
      const state = states.get(id);
      if (state) {
        state.draggingSquare = false;
        state.draggingHue = false;
      }
    };

    const touchMoveHandler = (e: TouchEvent): void => {
      const state = states.get(id);
      if (!state) {
        return;
      }

      if (e.touches.length < 1) {
        return;
      }

      if (e.cancelable) {
        e.preventDefault();
      }

      const touch = e.touches[0];

      if (state.draggingSquare) {
        onSquareMove(touch.clientX, touch.clientY);
      }
      if (state.draggingHue) {
        onHueMove(touch.clientY);
      }
    };

    const touchEndHandler = (): void => {
      const state = states.get(id);
      if (state) {
        state.draggingSquare = false;
        state.draggingHue = false;
      }
    };

    const state: HsvState = {
      hue: initialHue,
      saturation: initialSaturation,
      value: initialValue,
      draggingSquare: false,
      draggingHue: false,
      dotNetHelper,
      square,
      hueBar,
      squareIndicator,
      hueIndicator,
      mouseMoveHandler,
      mouseUpHandler,
      touchMoveHandler,
      touchEndHandler,
    };

    states.set(id, state);

    // Square mouse events
    square.addEventListener('mousedown', (e: MouseEvent) => {
      const s = states.get(id);
      if (s) {
        s.draggingSquare = true;
        onSquareMove(e.clientX, e.clientY);
      }
    });

    // Hue bar mouse events
    hueBar.addEventListener('mousedown', (e: MouseEvent) => {
      const s = states.get(id);
      if (s) {
        s.draggingHue = true;
        onHueMove(e.clientY);
      }
    });

    // Square touch events
    square.addEventListener('touchstart', (e: TouchEvent) => {
      const s = states.get(id);
      if (s && e.touches.length > 0) {
        s.draggingSquare = true;
        onSquareMove(e.touches[0].clientX, e.touches[0].clientY);
      }
    }, { passive: true });

    // Hue bar touch events
    hueBar.addEventListener('touchstart', (e: TouchEvent) => {
      const s = states.get(id);
      if (s && e.touches.length > 0) {
        s.draggingHue = true;
        onHueMove(e.touches[0].clientY);
      }
    }, { passive: true });

    // Global document events for dragging
    document.addEventListener('mousemove', mouseMoveHandler);
    document.addEventListener('mouseup', mouseUpHandler);
    document.addEventListener('touchmove', touchMoveHandler, { passive: false });
    document.addEventListener('touchend', touchEndHandler);
  }

  /**
   * Disposes the HSV color picker state and removes event listeners.
   * @param id The element ID of the color picker container.
   */
  export function Dispose(id: string): void {
    const state = states.get(id);
    if (state) {
      document.removeEventListener('mousemove', state.mouseMoveHandler);
      document.removeEventListener('mouseup', state.mouseUpHandler);
      document.removeEventListener('touchmove', state.touchMoveHandler);
      document.removeEventListener('touchend', state.touchEndHandler);
      states.delete(id);
    }
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
