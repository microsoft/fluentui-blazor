import { StartedMode } from "../../d-ts/StartedMode";

export namespace Microsoft.FluentUI.Blazor.Components.Popover {

  class FluentPopover extends HTMLElement {
    private anchorPositionObserverInterval: number = 100; // ms
    private dialog: HTMLDialogElement;
    private handleOutsideClick = this.onOutsideClick.bind(this);
    private handleCloseKeydown = this.onCloseKeydown.bind(this);
    private lastAnchorRect: DOMRect | null = null;
    private positionObserverInterval: number | null = null;

    // Add backing field for opened property
    private _opened: boolean = false;

    // Creates a new FluentPopover element.
    constructor() {
      super();

      const shadow = this.attachShadow({ mode: 'open' });

      // Create the dialog element
      this.dialog = document.createElement('dialog');
      this.dialog.part = 'dialog';    // To allow styling using `fluent-popover-b::part(dialog)`

      // Set initial styles for the dialog
      const sheet = new CSSStyleSheet();
      sheet.replaceSync(`
            :host dialog {
                position: fixed;
                margin: 0;
                z-index: 2000;
                color: var(--colorNeutralForeground1);
                background-color: var(--colorNeutralBackground1);
                border: 1px solid var(--colorTransparentStroke);
                border-radius: var(--borderRadiusMedium);
                box-shadow: var(--shadow16);
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

    // Initializes the popover by setting up event listeners and updating references.
    connectedCallback() {
      window.addEventListener('scroll', this.handleWindowChange, true);
      window.addEventListener('resize', this.handleWindowChange, true);
    }

    // Disposes the popover by removing event listeners and stopping observers.
    disconnectedCallback() {
      this.removeEventsAfterClosing();
      this.stopAnchorPositionObserver();
      window.removeEventListener('scroll', this.handleWindowChange, true);
      window.removeEventListener('resize', this.handleWindowChange, true);
    }

    // Getter and setter for the opened property
    public get opened(): boolean {
      return this._opened;
    }

    public set opened(value: boolean) {
      if (this._opened !== value) {
        this._opened = value;
        this.setAttribute('opened', String(value));
      }

      if (value && !this.dialog.open) {
        this.showPopover();
        return;
      }

      if (!value && this.dialog.open) {
        this.closePopover();
        return;
      }
    }

    /* ****************/
    /* Attributes     */
    /* ****************/

    static get observedAttributes() { return ['style', 'class', 'opened']; }

    // Handles attribute changes to update references and listeners.
    attributeChangedCallback(name: string, oldValue: string, newValue: string) {
      if (oldValue !== newValue) {

        if (name === 'style') {
          if (this.dialog && this.hasAttribute('style')) {
            this.dialog.setAttribute('style', this.getAttribute('style')!);
          }
        }

        if (name === 'class') {
          if (this.dialog && this.hasAttribute('class')) {
            this.dialog.setAttribute('class', this.getAttribute('class')!);
          }
        }

        // Sync property if 'opened' attribute changes externally
        if (name === 'opened') {
          this.opened = newValue === 'true';
        }
      }
    }

    private get anchorEl(): HTMLElement | null {
      const anchorId = this.getAttribute('anchor-id');
      return anchorId ? document.getElementById(anchorId) : null;
    }

    private get offsetVertical(): number {
      const val = this.getAttribute('offset-vertical');
      return val !== null ? Number(val) : 0;
    }

    private get offsetHorizontal(): number {
      const val = this.getAttribute('offset-horizontal');
      return val !== null ? Number(val) : 0;
    }

    /* ****************/
    /* Show or Close  */
    /* ****************/

    public showPopover() {
      if (!this.dialog || !this.anchorEl) return;

      if (this.dialog.open) {
        this.closePopover();
      }

      this.startAnchorPositionObserver();
      this.dialog.show();
      this.adjustDialogPosition();
      setTimeout(() => this.addEventsAfterOpening(), 0);

      // Reflect opened property and attribute
      this.opened = true;

      // Dispatch event when shown
      this.dispatchOpenedEvent(true);
    }

    public closePopover() {
      if (this.dialog.open) {
        this.dialog.close();
        this.stopAnchorPositionObserver();
        this.removeEventsAfterClosing();

        // Reflect opened property and attribute
        this.opened = false;

        // Dispatch event when closed
        this.dispatchOpenedEvent(false);
      }
    }

    // Dispatch event when opened or closed
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

    // Handles clicks outside the dialog to close it
    private onOutsideClick(event: MouseEvent | TouchEvent) {
      if (this.dialog.open && !this.contains(event.target as Node)) {
        this.closePopover();
      }
    }

    // Handles the keydown to close it
    private onCloseKeydown(event: KeyboardEvent) {
      if (event.key === 'Escape' && this.dialog.open) {
        this.closePopover();
      }
    }

    /* ****************/
    /* Event Handlers */
    /* ****************/

    private addEventsAfterOpening() {
      document.addEventListener('mousedown', this.handleOutsideClick);
      document.addEventListener('touchstart', this.handleOutsideClick);
      document.addEventListener('keydown', this.handleCloseKeydown);
    }

    private removeEventsAfterClosing() {
      document.removeEventListener('mousedown', this.handleOutsideClick);
      document.removeEventListener('touchstart', this.handleOutsideClick);
      document.removeEventListener('keydown', this.handleCloseKeydown);
    }

    /* ****************************************************** */
    /* Detect Anchor movement and update the popover position */
    /* ****************************************************** */

    private handleWindowChange = () => {
      if (this.dialog.open) {
        this.adjustDialogPosition();
      }
    };

    private startAnchorPositionObserver() {
      this.stopAnchorPositionObserver();
      this.positionObserverInterval = window.setInterval(() => {
        if (this.dialog.open && this.anchorEl) {
          const rect = this.anchorEl.getBoundingClientRect();
          if (
            !this.lastAnchorRect ||
            rect.left !== this.lastAnchorRect.left ||
            rect.top !== this.lastAnchorRect.top ||
            rect.width !== this.lastAnchorRect.width ||
            rect.height !== this.lastAnchorRect.height
          ) {
            this.adjustDialogPosition();
            this.lastAnchorRect = rect;
          }
        }
      }, this.anchorPositionObserverInterval);
    }

    private stopAnchorPositionObserver() {
      if (this.positionObserverInterval !== null) {
        clearInterval(this.positionObserverInterval);
        this.positionObserverInterval = null;
      }
    }

    private adjustDialogPosition() {

      if (this.anchorEl === null || this.dialog === null) return;

      const rect = this.anchorEl.getBoundingClientRect();
      const dialogHeight = this.dialog.offsetHeight + this.offsetVertical;
      const dialogWidth = this.dialog.offsetWidth + this.offsetHorizontal;
      const spaceAbove = rect.top;
      const spaceBelow = window.innerHeight - rect.bottom;
      const spaceLeft = rect.left + dialogWidth;
      const spaceRight = window.innerWidth - rect.left;

      // Position dialog above the target
      const positionDialogAbove = () => {
        this.dialog.style.top = `${rect.top - dialogHeight}px`;
      }

      // Position dialog below the target
      const positionDialogBelow = () => {
        this.dialog.style.top = `${rect.bottom + this.offsetVertical}px`;
      }

      // Position dialog left of the target
      const positionDialogLeft = () => {
        this.dialog.style.left = `${rect.left + this.offsetHorizontal}px`;
      }

      // Position dialog right of the target
      const positionDialogRight = () => {
        this.dialog.style.left = `${rect.left + rect.width - dialogWidth - this.offsetHorizontal}px`;
      }

      // Set the position above or below the anchor element
      if (spaceBelow >= dialogHeight) {
        positionDialogBelow();
      }
      else if (spaceAbove >= dialogHeight) {
        positionDialogAbove();
      }
      else {
        positionDialogBelow();  // Default
      }

      // Set the position left or right of the anchor element
      if (spaceRight >= dialogWidth) {
        positionDialogLeft();
      }
      else if (spaceLeft >= dialogWidth) {
        positionDialogRight();
      }
      else {
        positionDialogLeft();  // Default
      }
    };
  }

  /**
    * Register the FluentPopover component
    * @param blazor
    * @param mode
    */
  export const registerComponent = (blazor: Blazor, mode: StartedMode): void => {
    if (typeof blazor.addEventListener === 'function' && mode === StartedMode.Web) {
      customElements.define('fluent-popover-b', FluentPopover);
    }
  };
}
