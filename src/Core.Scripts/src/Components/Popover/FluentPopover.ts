import { StartedMode } from "../../d-ts/StartedMode";

export namespace Microsoft.FluentUI.Blazor.Components.Popover {

  class FluentPopover extends HTMLElement {
    private anchorPositionObserverInterval: number = 100; // ms
    private dialog: HTMLDialogElement;
    private anchorEl: HTMLElement | null = null;
    private triggerEl: HTMLElement | null = null;
    private handleTriggerClick = this.showPopover.bind(this);
    private handleOutsideClick = this.onOutsideClick.bind(this);
    private handleCloseKeydown = this.onCloseKeydown.bind(this);
    private lastAnchorRect: DOMRect | null = null;
    private positionObserverInterval: number | null = null;

    static get observedAttributes() {
      return [
        'anchor-id',
        'trigger-id',
        'offset-vertical',
        'offset-horizontal',
        'style',
        'class'
      ];
    }

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
      this.updateAttributeReferences();
      this.addTriggerListener();
      window.addEventListener('scroll', this.handleWindowChange, true);
      window.addEventListener('resize', this.handleWindowChange, true);
    }

    // Disposes the popover by removing event listeners and stopping observers.
    disconnectedCallback() {
      this.removeTriggerListener();
      this.removeEventsAfterClosing();
      this.stopAnchorPositionObserver();
      window.removeEventListener('scroll', this.handleWindowChange, true);
      window.removeEventListener('resize', this.handleWindowChange, true);
    }

    /* ****************/
    /* Attributes     */
    /* ****************/

    // Handles attribute changes to update references and listeners.
    attributeChangedCallback(name: string, oldValue: string, newValue: string) {
      if (oldValue !== newValue) {
        this.updateAttributeReferences();
        this.removeTriggerListener();
        this.addTriggerListener();

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
      }
    }

    // Updates the references to the anchor and trigger elements based on their IDs.
    private updateAttributeReferences() {
      const anchorId = this.getAttribute('anchor-id');
      const triggerId = this.getAttribute('trigger-id');
      this.anchorEl = anchorId ? document.getElementById(anchorId) : null;
      this.triggerEl = triggerId ? document.getElementById(triggerId) : null;
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

      // Reflect opened attribute
      if (!this.hasAttribute('opened')) {
        this.setAttribute('opened', '');
      }

      // Dispatch event when shown
      this.dispatchOpenedEvent(true);
    }

    public closePopover() {
      if (this.dialog.open) {
        this.dialog.close();
        this.stopAnchorPositionObserver();
        this.removeEventsAfterClosing();

        // Remove opened attribute
        if (this.hasAttribute('opened')) {
          this.removeAttribute('opened');
        }

        // Dispatch event when closed
        this.dispatchOpenedEvent(false);
      }
    }

    // Dispatch event when opened or closed
    private dispatchOpenedEvent(opened: boolean) {
      this.dispatchEvent(new CustomEvent('ontoggle', {
        detail: { open: opened },
        bubbles: true,
        composed: true
      }));
    }

    // Handles clicks outside the dialog to close it
    private onOutsideClick(event: MouseEvent | TouchEvent) {
      if (this.dialog.open && !this.dialog.contains(event.target as Node) && event.target !== this.triggerEl) {
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

    private addTriggerListener() {
      if (this.triggerEl) {
        this.triggerEl.addEventListener('click', this.handleTriggerClick);
      }
    }

    private removeTriggerListener() {
      if (this.triggerEl) {
        this.triggerEl.removeEventListener('click', this.handleTriggerClick);
      }
    }

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
