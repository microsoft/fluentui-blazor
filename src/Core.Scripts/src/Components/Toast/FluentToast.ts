import { StartedMode } from "../../d-ts/StartedMode";

export namespace Microsoft.FluentUI.Blazor.Components.Toast {

  class FluentToast extends HTMLElement {
    private static readonly stackGap = 12;
    private dialog: ToastElement;
    private timeoutId: number | null = null;
    private remainingTimeout: number | null = null;
    private timeoutStartedAt: number | null = null;
    private pauseReasons: Set<string> = new Set();

    // Creates a new FluentToast element.
    constructor() {
      super();

      const shadow = this.attachShadow({ mode: 'open' });

      // Create the dialog element
      this.dialog = document.createElement('div') as ToastElement;
      this.dialog.setAttribute('fuib', '');
      this.dialog.setAttribute('popover', 'manual');
      this.dialog.setAttribute('part', 'dialog');    // To allow styling using `fluent-toast-b::part(dialog)`

      // Dispatch the toggle events when the toast is opened or closed
      this.dialog.addEventListener('beforetoggle', (e: any) => {
        e.stopPropagation();
        const oldState = e?.oldState ?? (this.dialogIsOpen ? 'open' : 'closed');
        const newState = e?.newState ?? (oldState === 'open' ? 'closed' : 'open');
        this.dispatchDialogToggleEvent('beforetoggle', oldState, newState);
      });

      this.dialog.addEventListener('toggle', (e: any) => {
        e.stopPropagation();
        const oldState = e?.oldState ?? (this.dialogIsOpen ? 'open' : 'closed');
        const newState = e?.newState ?? (oldState === 'open' ? 'closed' : 'open');
        this.dispatchDialogToggleEvent('toggle', oldState, newState);
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
                border: 0;
                background: transparent;
            }

            /* Animations */
            :host div[fuib][popover]:popover-open {
                display: block;
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
      this.addEventListener('pointerover', this.handlePointerOver, true);
      this.addEventListener('pointerout', this.handlePointerOut, true);
      this.addEventListener('focusin', this.handleFocusIn, true);
      this.addEventListener('focusout', this.handleFocusOut, true);
      window.addEventListener('blur', this.handleWindowBlur, true);
      window.addEventListener('focus', this.handleWindowFocus, true);
      this.updateAccessibility();
    }

    // Disposes the toast by clearing the timeout.
    disconnectedCallback() {
      window.removeEventListener('resize', this.handleWindowChange, true);
      this.removeEventListener('pointerover', this.handlePointerOver, true);
      this.removeEventListener('pointerout', this.handlePointerOut, true);
      this.removeEventListener('focusin', this.handleFocusIn, true);
      this.removeEventListener('focusout', this.handleFocusOut, true);
      window.removeEventListener('blur', this.handleWindowBlur, true);
      window.removeEventListener('focus', this.handleWindowFocus, true);
      this.clearTimeout();
    }

    private handlePointerOver = () => {
      if (this.attributeIsTrue('pause-on-hover')) {
        this.pause('hover');
      }
    };

    private handlePointerOut = (e: PointerEvent) => {
      const related = e.relatedTarget as Node | null;
      if (related && this.contains(related)) {
        return;
      }

      if (this.attributeIsTrue('pause-on-hover')) {
        this.resume('hover');
      }
    };

    private handleFocusIn = () => {
      this.pause('focus');
    };

    private handleFocusOut = (e: FocusEvent) => {
      const related = e.relatedTarget as Node | null;
      if (related && this.contains(related)) {
        return;
      }

      this.resume('focus');
    };

    private handleWindowBlur = () => {
      if (this.attributeIsTrue('pause-on-window-blur')) {
        this.pause('window');
      }
    };

    private handleWindowFocus = () => {
      if (this.attributeIsTrue('pause-on-window-blur')) {
        this.resume('window');
      }
    };

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

    static get observedAttributes() { return ['opened', 'timeout', 'position', 'vertical-offset', 'horizontal-offset', 'intent', 'politeness', 'pause-on-hover', 'pause-on-window-blur']; }

    attributeChangedCallback(name: string, oldValue: string, newValue: string) {
      if (oldValue !== newValue) {
        if (name === 'opened') {
          this.opened = newValue === 'true';
        }
        if (name === 'timeout' && this.dialogIsOpen) {
          this.startTimeout();
        }
        if ((name === 'position' || name === 'vertical-offset' || name === 'horizontal-offset') && this.dialogIsOpen) {
          this.updatePosition();
        }
        if (name === 'intent' || name === 'politeness') {
          this.updateAccessibility();
        }
        if (name === 'pause-on-hover' && !this.attributeIsTrue('pause-on-hover')) {
          this.resume('hover');
        }
        if (name === 'pause-on-window-blur' && !this.attributeIsTrue('pause-on-window-blur')) {
          this.resume('window');
        }
      }
    }

    public showToast() {
      if (!this.dialog) return;

      this.dialog.showPopover();
      this.updatePosition();
      this.updateToastStack();
      this.updateAccessibility();
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
        this.pauseReasons.clear();
        this.clearTimeout();
        this.updateToastStack();
      }
    }

    private startTimeout() {
      this.clearTimeout();
      const timeoutAttr = this.getAttribute('timeout');
      const timeout = timeoutAttr ? parseInt(timeoutAttr) : 0;
      if (timeout > 0) {
        this.remainingTimeout = timeout;
        this.timeoutStartedAt = Date.now();
        this.timeoutId = window.setTimeout(() => {
          this.opened = false;
        }, timeout);
      } else {
        this.remainingTimeout = null;
        this.timeoutStartedAt = null;
      }
    }

    private clearTimeout() {
      if (this.timeoutId !== null) {
        window.clearTimeout(this.timeoutId);
        this.timeoutId = null;
      }
    }

    private pause(reason: string) {
      if (this.pauseReasons.has(reason)) {
        return;
      }

      this.pauseReasons.add(reason);
      this.pauseTimeout();
    }

    private resume(reason: string) {
      if (!this.pauseReasons.has(reason)) {
        return;
      }

      this.pauseReasons.delete(reason);
      if (this.pauseReasons.size === 0) {
        this.resumeTimeout();
      }
    }

    private pauseTimeout() {
      if (this.timeoutId === null || this.remainingTimeout === null || this.timeoutStartedAt === null) {
        return;
      }

      const elapsed = Date.now() - this.timeoutStartedAt;
      this.remainingTimeout = Math.max(0, this.remainingTimeout - elapsed);
      this.timeoutStartedAt = null;

      window.clearTimeout(this.timeoutId);
      this.timeoutId = null;
    }

    private resumeTimeout() {
      if (!this.dialogIsOpen || this.remainingTimeout === null) {
        return;
      }

      if (this.pauseReasons.size > 0) {
        return;
      }

      if (this.remainingTimeout <= 0) {
        this.opened = false;
        return;
      }

      if (this.timeoutId !== null) {
        return;
      }

      this.timeoutStartedAt = Date.now();
      this.timeoutId = window.setTimeout(() => {
        this.opened = false;
      }, this.remainingTimeout);
    }

    private updatePosition() {
      const isRtl = getComputedStyle(this).direction === 'rtl';
      const position = this.getAttribute('position') || (isRtl ? 'bottom-left' : 'bottom-right');
      const horizontalOffset = parseInt(this.getAttribute('horizontal-offset') || '20');
      const verticalOffset = parseInt(this.getAttribute('vertical-offset') || '16') + this.getStackOffset(position);

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

    private getStackOffset(position: string): number {
      const toastElements = Array.from(document.querySelectorAll('fluent-toast-b')) as FluentToast[];
      const toastsBeforeCurrent = toastElements
        .slice(0, toastElements.indexOf(this))
        .filter(toast => toast.getToastPosition() === position && toast.dialogIsOpen);

      return toastsBeforeCurrent.reduce((offset, toast) => {
        const height = toast.dialog.getBoundingClientRect().height;
        return offset + height + FluentToast.stackGap;
      }, 0);
    }

    private getToastPosition(): string {
      const isRtl = getComputedStyle(this).direction === 'rtl';
      return this.getAttribute('position') || (isRtl ? 'bottom-left' : 'bottom-right');
    }

    private updateToastStack() {
      const toastElements = Array.from(document.querySelectorAll('fluent-toast-b')) as FluentToast[];
      toastElements
        .filter(toast => toast.dialogIsOpen)
        .forEach(toast => toast.updatePosition());
    }

    private dispatchDialogToggleEvent(type: string, oldState: string, newState: string) {
      this.dispatchEvent(new CustomEvent(type, {
        detail: {
          oldState,
          newState,
        },
        bubbles: true,
        composed: true
      }));
    }

    private updateAccessibility() {
      const intent = (this.getAttribute('intent') || '').toLowerCase();
      const politeness = (this.getAttribute('politeness') || '').toLowerCase();

      const live = politeness === 'polite' || politeness === 'assertive'
        ? politeness
        : ((intent !== '' && intent !== 'info') ? 'assertive' : 'polite');

      this.dialog.setAttribute('aria-live', live);
      this.dialog.setAttribute('role', live === 'assertive' ? 'alert' : 'status');
    }

    private attributeIsTrue(attributeName: string): boolean {
      const value = this.getAttribute(attributeName);
      if (value === null) {
        return false;
      }

      if (value === '') {
        return true;
      }

      return value.toLowerCase() === 'true';
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
