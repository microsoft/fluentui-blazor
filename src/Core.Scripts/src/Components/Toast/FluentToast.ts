import { StartedMode } from "../../d-ts/StartedMode";

export namespace Microsoft.FluentUI.Blazor.Components.Toast {

  class FluentToast extends HTMLElement {
    private static readonly stackGap = 12;
    private dialog: ToastElement;
    private mediaRegion: HTMLDivElement;
    private titleRegion: HTMLDivElement;
    private dismissRegion: HTMLDivElement;
    private bodyRegion: HTMLDivElement;
    private subtitleRegion: HTMLDivElement;
    private footerRegion: HTMLDivElement;
    private mediaSlot: HTMLSlotElement;
    private titleSlot: HTMLSlotElement;
    private dismissSlot: HTMLSlotElement;
    private bodySlot: HTMLSlotElement;
    private subtitleSlot: HTMLSlotElement;
    private footerSlot: HTMLSlotElement;
    private resizeObserver: ResizeObserver | null = null;
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

      this.mediaRegion = document.createElement('div');
      this.mediaRegion.classList.add('media');
      this.mediaSlot = document.createElement('slot');
      this.mediaSlot.name = 'media';
      this.mediaRegion.appendChild(this.mediaSlot);

      this.titleRegion = document.createElement('div');
      this.titleRegion.classList.add('title');
      this.titleSlot = document.createElement('slot');
      this.titleSlot.name = 'title';
      this.titleRegion.appendChild(this.titleSlot);

      this.dismissRegion = document.createElement('div');
      this.dismissRegion.classList.add('dismiss');
      this.dismissSlot = document.createElement('slot');
      this.dismissSlot.name = 'dismiss';
      this.dismissRegion.appendChild(this.dismissSlot);

      this.bodyRegion = document.createElement('div');
      this.bodyRegion.classList.add('body');
      this.bodySlot = document.createElement('slot');
      this.bodyRegion.appendChild(this.bodySlot);

      this.subtitleRegion = document.createElement('div');
      this.subtitleRegion.classList.add('subtitle');
      this.subtitleSlot = document.createElement('slot');
      this.subtitleSlot.name = 'subtitle';
      this.subtitleRegion.appendChild(this.subtitleSlot);

      this.footerRegion = document.createElement('div');
      this.footerRegion.classList.add('footer');
      this.footerSlot = document.createElement('slot');
      this.footerSlot.name = 'footer';
      this.footerRegion.appendChild(this.footerSlot);

      this.dialog.append(
        this.mediaRegion,
        this.titleRegion,
        this.dismissRegion,
        this.bodyRegion,
        this.subtitleRegion,
        this.footerRegion,
      );

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
                display: grid;
                grid-template-columns: auto 1fr auto;
                border: 0;
                background: var(--colorNeutralBackground1);
                padding: 0;
                font-size: var(--fontSizeBase300);
                line-height: var(--lineHeightBase300);
                font-weight: var(--fontWeightSemibold);
                color: var(--colorNeutralForeground1);
                border: 1px solid var(--colorTransparentStroke);
                border-radius: var(--borderRadiusMedium);
                box-shadow: var(--shadow8);
                box-sizing: border-box;
                min-width: 292px;
                max-width: 292px;
                padding: 12px;
            }

            .media,
            .title,
            .dismiss,
            .body,
            .subtitle,
            .footer {
                min-width: 0;
            }

            .media {
                display: flex;
                grid-column: 1;
                grid-row: 1;
                padding-top: 2px;
                padding-inline-end: 8px;
                font-size: var(--fontSizeBase400);
                color: var(--colorNeutralForeground1);
            }

            .title {
                display: flex;
                align-items: center;
                grid-column: 2;
                grid-row: 1;
                color: var(--colorNeutralForeground1);
                word-break: break-word;
            }

            :host(:not([has-dismiss])) .title {
                grid-column: 2 / -1;
            }

            :host(:not([has-media])) .title {
                grid-column: 1 / span 2;
            }

            :host(:not([has-media]):not([has-dismiss])) .title {
                grid-column: 1 / -1;
            }

            .dismiss {
                display: flex;
                align-items: start;
                justify-content: end;
                grid-column: 3;
                grid-row: 1;
                padding-inline-start: 12px;
                color: var(--colorBrandForeground1);
            }

            .body {
                grid-column: 2 / -1;
                padding-top: 6px;
                font-size: var(--fontSizeBase300);
                line-height: var(--lineHeightBase300);
                font-weight: var(--fontWeightRegular);
                color: var(--colorNeutralForeground1);
                word-break: break-word;
            }

            .subtitle {
                grid-column: 2 / -1;
                padding-top: 4px;
                font-size: var(--fontSizeBase200);
                line-height: var(--lineHeightBase200);
                font-weight: var(--fontWeightRegular);
                color: var(--colorNeutralForeground2);
            }

            .footer {
                display: flex;
                align-items: center;
                flex-wrap: wrap;
                gap: 14px;
                grid-column: 2 / -1;
                padding-top: 16px;
            }

            :host(:not([has-media])) .body,
            :host(:not([has-media])) .subtitle,
            :host(:not([has-media])) .footer {
                grid-column: 1 / -1;
            }

            .media[hidden],
            .title[hidden],
            .dismiss[hidden],
            .body[hidden],
            .subtitle[hidden],
            .footer[hidden] {
                display: none !important;
            }

            /* Animations */
            :host div[fuib][popover]:popover-open {
                opacity: 1;
                animation: toast-enter 0.25s cubic-bezier(0.33, 0, 0, 1) forwards;
            }

            :host div[fuib][popover].closing {
               pointer-events: none;
               overflow: hidden;
               will-change: opacity, height, margin, padding;
               animation:
                  toast-exit 600ms cubic-bezier(0.33, 0, 0.67, 1) forwards,
                  toast-dismiss-collapse-height 200ms cubic-bezier(0.33, 0, 0.67, 1) 400ms forwards,
                  toast-dismiss-collapse-spacing 200ms cubic-bezier(0.33, 0, 0.67, 1) 400ms forwards;
            }

            @keyframes toast-enter {
                from { opacity: 0; transform: var(--toast-enter-from, translateY(16px)); }
                to { opacity: 1; transform: var(--toast-enter-to, translateY(0)); }
            }

           @keyframes toast-exit {
              0% {
                opacity: 1;
                height: var(--toast-height);
                margin-top: var(--toast-margin-top, 0px);
                margin-bottom: var(--toast-margin-bottom, 0px);
                padding-top: var(--toast-padding-top, 0px);
                padding-bottom: var(--toast-padding-bottom, 0px);
              }

              66.666% {
                opacity: 0;
                height: var(--toast-height);
                margin-top: var(--toast-margin-top, 0px);
                margin-bottom: var(--toast-margin-bottom, 0px);
                padding-top: var(--toast-padding-top, 0px);
                padding-bottom: var(--toast-padding-bottom, 0px);
              }

              100% {
                opacity: 0;
                height: 0;
                margin-top: 0;
                margin-bottom: 0;
                padding-top: 0;
                padding-bottom: 0;
              }
            }
        `);
      this.shadowRoot!.adoptedStyleSheets = [
        ...(this.shadowRoot!.adoptedStyleSheets || []),
        sheet
      ];

      shadow.appendChild(this.dialog);

      this.mediaSlot.addEventListener('slotchange', () => this.updateSlotState(this.mediaRegion, this.mediaSlot, 'has-media'));
      this.titleSlot.addEventListener('slotchange', () => this.updateSlotState(this.titleRegion, this.titleSlot, 'has-title'));
      this.dismissSlot.addEventListener('slotchange', () => this.updateSlotState(this.dismissRegion, this.dismissSlot, 'has-dismiss'));
      this.bodySlot.addEventListener('slotchange', () => this.updateSlotState(this.bodyRegion, this.bodySlot, 'has-body'));
      this.subtitleSlot.addEventListener('slotchange', () => this.updateSlotState(this.subtitleRegion, this.subtitleSlot, 'has-subtitle'));
      this.footerSlot.addEventListener('slotchange', () => this.updateSlotState(this.footerRegion, this.footerSlot, 'has-footer'));
    }

    connectedCallback() {
      window.addEventListener('resize', this.handleWindowChange, true);
      this.addEventListener('pointerover', this.handlePointerOver, true);
      this.addEventListener('pointerout', this.handlePointerOut, true);
      this.addEventListener('focusin', this.handleFocusIn, true);
      this.addEventListener('focusout', this.handleFocusOut, true);
      window.addEventListener('blur', this.handleWindowBlur, true);
      window.addEventListener('focus', this.handleWindowFocus, true);
      this.updateSlotStates();
      if (typeof ResizeObserver !== 'undefined') {
        this.resizeObserver = new ResizeObserver(() => {
          if (this.dialogIsOpen) {
            this.updateToastStack();
          }
        });
        this.resizeObserver.observe(this.dialog);
      }
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
      this.resizeObserver?.disconnect();
      this.resizeObserver = null;
      this.clearTimeout();
      this.updateToastStack();
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

      this.updateSlotStates();
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
          setTimeout(() => resolve(false), 650);
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

    private updateSlotStates() {
      this.updateSlotState(this.mediaRegion, this.mediaSlot, 'has-media');
      this.updateSlotState(this.titleRegion, this.titleSlot, 'has-title');
      this.updateSlotState(this.dismissRegion, this.dismissSlot, 'has-dismiss');
      this.updateSlotState(this.bodyRegion, this.bodySlot, 'has-body');
      this.updateSlotState(this.subtitleRegion, this.subtitleSlot, 'has-subtitle');
      this.updateSlotState(this.footerRegion, this.footerSlot, 'has-footer');
    }

    private updateSlotState(region: HTMLDivElement, slot: HTMLSlotElement, hostAttribute: string) {
      const hasContent = slot.assignedNodes({ flatten: true }).some(node =>
        node.nodeType !== Node.TEXT_NODE || Boolean(node.textContent?.trim())
      );

      region.hidden = !hasContent;
      if (hasContent) {
        this.setAttribute(hostAttribute, '');
      } else {
        this.removeAttribute(hostAttribute);
      }

      if (this.dialogIsOpen) {
        this.updateToastStack();
      }
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
