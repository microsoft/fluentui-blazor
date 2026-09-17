export const fluentOverlayStyles: string = `
  :host {
    --overlayZIndex: 99999;
    --overlayBackground: '';
  }

  :host dialog {
    border: none;
    background: none;
    z-index: var(--overlayZIndex);
  }

  :focus-visible {
    outline: none;
  }

  :host([fullscreen]) dialog {
    position: fixed;
    top: 50%;
    left: 50%;
    margin: 0;
    transform: translate(-50%, -50%);
  }

  :host([fullscreen]) dialog::backdrop {
    background-color: var(--overlayBackground);
  }

  :host(:not([fullscreen])):has(dialog[open]) {
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background-color: var(--overlayBackground);
  }

  :host(:not([fullscreen])) dialog {
    position: fixed;
    margin: 0;
    transform: translate(-50%, -50%);
  }

  :host(:not([fullscreen])) dialog::backdrop {
    background-color: transparent;
    width: 0;
    height: 0;
  }

  :host([interactive][fullscreen]):has(dialog[open]) {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    z-index: calc(var(--overlayZIndex) - 1);
    background-color: var(--overlayBackground);
  }

  :host([interactive]):has(dialog[open]) {
    pointer-events: none;
  }
`;
