import { StartedMode } from "../../d-ts/StartedMode";

export namespace Microsoft.FluentUI.Blazor.Components.Toast {

  class FluentToast extends HTMLElement {
    private dialog: ToastElement;
    private timeoutId: number | null = null;

    // Creates a new FluentToast element.
    constructor() {
      super();

      const shadow = this.attachShadow({ mode: 'open' });

      // Create the dialog element
      this.dialog = document.createElement('div') as ToastElement;
      this.dialog.setAttribute('fuib', '');
      this.dialog.setAttribute('popover', 'manual');
      this.dialog.setAttribute('part', 'dialog');    // To allow styling using `fluent-toast-b::part(dialog)`

      // Dispatch the toggle event when the toast is opened or closed
      this.dialog.addEventListener('toggle', (e: any) => {
        e.stopPropagation();
        this.dispatchOpenedEvent(e.newState === 'open');
      });

      // Set initial styles for the dialog
      const sheet = new CSSStyleSheet();
      sheet.replaceSync(`
            :host(:not([opened='true']):not(.animating)) {
                display: none;
            }

            :host {
                display: contents;
            }

            :host div[fuib][popover] {
                position: fixed;
                margin: 0;
                z-index: 2000;
                min-width: 292px;
                max-width: 292px;
                color: var(--colorNeutralForeground1);
                background-color: var(--colorNeutralBackground1);
                border: 1px solid var(--colorTransparentStroke);
                border-radius: var(--borderRadiusMedium);
                box-shadow: var(--shadow8);
                padding: 12px;
                flex-direction: column;
                gap: 8px;
                display: grid;
                grid-template-columns: auto 1fr auto;
                font-size: var(--fontSizeBase300);
                font-weight: var(--fontWeightSemibold);

                /* Fade out by default when hidden */
                opacity: 0;
            }

            /* Animations */
            :host div[fuib][popover]:popover-open {
                display: flex;
                opacity: 1;
                animation: toast-enter 0.25s cubic-bezier(0.33, 0, 0, 1) forwards;
            }

            :host div[fuib][popover].closing {
                animation: toast-exit 0.2s cubic-bezier(0.33, 0, 0, 1) forwards;
            }

            @keyframes toast-enter {
                from { opacity: 0; transform: var(--toast-enter-from, translateY(16px)); }
                to { opacity: 1; transform: var(--toast-enter-to, translateY(0)); }
            }

            @keyframes toast-exit {
                from { opacity: 1; transform: var(--toast-enter-to, translateY(0)); }
                to { opacity: 0; transform: scale(0.95); }
            }
        `);
      this.shadowRoot!.adoptedStyleSheets = [
        ...(this.shadowRoot!.adoptedStyleSheets || []),
        sheet
      ];

      // Slot for user content
      const slot = document.createElement('slot');
      this.dialog.appendChild(slot);
      shadow.appendChild(this.dialog);
    }

    connectedCallback() {
      window.addEventListener('resize', this.handleWindowChange, true);
    }

    // Disposes the toast by clearing the timeout.
    disconnectedCallback() {
      window.removeEventListener('resize', this.handleWindowChange, true);
      this.clearTimeout();
    }

    private handleWindowChange = () => {
      if (this.dialogIsOpen) {
        this.updatePosition();
      }
    };

    private get dialogIsOpen(): boolean {
      return this.dialog.matches(':popover-open');
    }

    // Getter and setter for the opened property
    public get opened(): boolean {
      return this.getAttribute('opened') === 'true';
    }

    public set opened(value: boolean) {
      this.setAttribute('opened', String(value));
      if (value && !this.dialogIsOpen) {
        this.showToast();
      } else if (!value && this.dialogIsOpen) {
        this.closeToast();
      }
    }

    static get observedAttributes() { return ['opened', 'timeout', 'position', 'vertical-offset', 'horizontal-offset']; }

    attributeChangedCallback(name: string, oldValue: string, newValue: string) {
      if (oldValue !== newValue) {
        if (name === 'opened') {
          this.opened = newValue === 'true';
        }
        if ((name === 'position' || name === 'vertical-offset' || name === 'horizontal-offset') && this.dialogIsOpen) {
          this.updatePosition();
        }
      }
    }

    public showToast() {
      if (!this.dialog) return;

      this.dialog.showPopover();
      this.updatePosition();
      this.startTimeout();
    }

    public async closeToast() {
      if (this.dialogIsOpen) {
        this.classList.add('animating');
        this.dialog.classList.add('closing');

        // Wait for the exit animation to complete
        await new Promise(resolve => {
          const onAnimationEnd = (e: AnimationEvent) => {
            if (e.animationName === 'toast-exit') {
              this.dialog.removeEventListener('animationend', onAnimationEnd);
              resolve(true);
            }
          };
          this.dialog.addEventListener('animationend', onAnimationEnd);
          // Fallback in case animation doesn't fire
          setTimeout(() => resolve(false), 300);
        });

        this.dialog.hidePopover();
        this.dialog.classList.remove('closing');
        this.classList.remove('animating');
        this.clearTimeout();
      }
    }

    private startTimeout() {
      this.clearTimeout();
      const timeoutAttr = this.getAttribute('timeout');
      const timeout = timeoutAttr ? parseInt(timeoutAttr) : 0;
      if (timeout > 0) {
        this.timeoutId = window.setTimeout(() => {
          this.opened = false;
        }, timeout);
      }
    }

    private clearTimeout() {
      if (this.timeoutId !== null) {
        window.clearTimeout(this.timeoutId);
        this.timeoutId = null;
      }
    }

    private updatePosition() {
      const isRtl = getComputedStyle(this).direction === 'rtl';
      const position = this.getAttribute('position') || (isRtl ? 'bottom-left' : 'bottom-right');
      const horizontalOffset = parseInt(this.getAttribute('horizontal-offset') || '16');
      const verticalOffset = parseInt(this.getAttribute('vertical-offset') || '16');

      this.dialog.style.top = 'auto';
      this.dialog.style.left = 'auto';
      this.dialog.style.right = 'auto';
      this.dialog.style.bottom = 'auto';

      let enterFrom = 'translateY(16px)';
      let enterTo = 'translateY(0)';

      switch (position) {
        case 'top-right':
          this.dialog.style.top = `${verticalOffset}px`;
          this.dialog.style.right = `${horizontalOffset}px`;
          enterFrom = 'translateX(16px)';
          enterTo = 'translateX(0)';
          break;
        case 'top-left':
          this.dialog.style.top = `${verticalOffset}px`;
          this.dialog.style.left = `${horizontalOffset}px`;
          enterFrom = 'translateX(-16px)';
          enterTo = 'translateX(0)';
          break;
        case 'bottom-right':
          this.dialog.style.bottom = `${verticalOffset}px`;
          this.dialog.style.right = `${horizontalOffset}px`;
          enterFrom = 'translateX(16px)';
          enterTo = 'translateX(0)';
          break;
        case 'bottom-left':
          this.dialog.style.bottom = `${verticalOffset}px`;
          this.dialog.style.left = `${horizontalOffset}px`;
          enterFrom = 'translateX(-16px)';
          enterTo = 'translateX(0)';
          break;
        case 'top-center':
          this.dialog.style.top = `${verticalOffset}px`;
          this.dialog.style.left = '50%';
          enterFrom = 'translate(-50%, -16px)';
          enterTo = 'translate(-50%, 0)';
          break;
        case 'bottom-center':
          this.dialog.style.bottom = `${verticalOffset}px`;
          this.dialog.style.left = '50%';
          enterFrom = 'translate(-50%, 16px)';
          enterTo = 'translate(-50%, 0)';
          break;
      }

      this.dialog.style.setProperty('--toast-enter-from', enterFrom);
      this.dialog.style.setProperty('--toast-enter-to', enterTo);

      // Centers need translate(-50%, 0) applied permanently if they are open
      if (position.includes('center')) {
          this.dialog.style.transform = enterTo;
      }
    }

    private dispatchOpenedEvent(opened: boolean) {
      this.dispatchEvent(new CustomEvent('toggle', {
        detail: {
          oldState: opened ? 'closed' : 'open',
          newState: opened ? 'open' : 'closed',
        },
        bubbles: true,
        composed: true
      }));
    }
  }

  interface ToastElement extends HTMLDivElement {
    showPopover: () => void;
    hidePopover: () => void;
  }

  export const registerComponent = (blazor: Blazor, mode: StartedMode): void => {
    if (typeof blazor.addEventListener === 'function' && mode === StartedMode.Web) {
      customElements.define('fluent-toast-b', FluentToast);
    }
  };
}
