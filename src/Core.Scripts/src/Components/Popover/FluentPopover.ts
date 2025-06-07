import { StartedMode } from "../../d-ts/StartedMode";

export namespace Microsoft.FluentUI.Blazor.Components.Popover {

  class FluentPopover extends HTMLElement {
    private dialog: HTMLDialogElement;
    private anchorEl: HTMLElement | null = null;
    private triggerEl: HTMLElement | null = null;
    private handleTriggerClick = this.showPopover.bind(this);
    private handleOutsideClick = this.onOutsideClick.bind(this);
    private lastAnchorRect: DOMRect | null = null;
    private positionObserverInterval: number | null = null;

    static get observedAttributes() {
      return ['anchor-id', 'trigger-id'];
    }

    constructor() {
      super();
      const shadow = this.attachShadow({ mode: 'open' });

      const style = document.createElement('style');
      style.textContent = `
            .fluent-popover {
                position: fixed;
                padding: 0;
                margin: 0;
                border-radius: 8px;
                border: 1px solid red;
                background: transparent;
                box-shadow: 0 4px 12px rgba(0, 0, 0, 0.3)
            }
        `;
      shadow.appendChild(style);

      this.dialog = document.createElement('dialog');
      this.dialog.classList.add('fluent-popover');

      // Slot for user content
      const slot = document.createElement('slot');
      this.dialog.appendChild(slot);
      shadow.appendChild(this.dialog);
    }

    connectedCallback() {
      this.updateReferences();
      this.addTriggerListener();
      window.addEventListener('scroll', this.handleWindowChange, true);
      window.addEventListener('resize', this.handleWindowChange, true);
    }

    disconnectedCallback() {
      this.removeTriggerListener();
      this.removeOutsideClickListener();
      window.removeEventListener('scroll', this.handleWindowChange, true);
      window.removeEventListener('resize', this.handleWindowChange, true);
      this.stopAnchorPositionObserver();
    }

    attributeChangedCallback(name: string, oldValue: string, newValue: string) {
      if (oldValue !== newValue) {
        this.updateReferences();
        this.removeTriggerListener();
        this.addTriggerListener();
      }
    }

    private updateReferences() {
      const anchorId = this.getAttribute('anchor-id');
      const triggerId = this.getAttribute('trigger-id');
      this.anchorEl = anchorId ? document.getElementById(anchorId) : null;
      this.triggerEl = triggerId ? document.getElementById(triggerId) : null;
    }

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
      }, 100);
    }

    private stopAnchorPositionObserver() {
      if (this.positionObserverInterval !== null) {
        clearInterval(this.positionObserverInterval);
        this.positionObserverInterval = null;
      }
    }

    public showPopover() {
      if (!this.dialog || !this.anchorEl) return;

      if (this.dialog.open) {
        this.closePopover();
      }

      this.startAnchorPositionObserver();
      this.adjustDialogPosition();
      this.dialog.show();
      setTimeout(() => this.addOutsideClickListener(), 0);
    }

    public closePopover() {
      if (this.dialog.open) {
        this.dialog.close();
        this.stopAnchorPositionObserver();
        this.removeOutsideClickListener();
      }
    }

    private addOutsideClickListener() {
      document.addEventListener('mousedown', this.handleOutsideClick);
      document.addEventListener('touchstart', this.handleOutsideClick);
    }

    private removeOutsideClickListener() {
      document.removeEventListener('mousedown', this.handleOutsideClick);
      document.removeEventListener('touchstart', this.handleOutsideClick);
    }

    private onOutsideClick(event: MouseEvent | TouchEvent) {
      if (this.dialog.open && !this.dialog.contains(event.target as Node) && event.target !== this.triggerEl) {
        this.closePopover();
      }
    }

    private adjustDialogPosition() {

      if (this.anchorEl === null || this.dialog === null) return;

      // Position dialog above the target
      const positionDialogAbove = () => {
        this.dialog.style.left = `${rect.left}px `;
        this.dialog.style.top = `${rect.top - dialogHeight}px`;
      }

      // Position dialog below the target
      const positionDialogBelow = () => {
        this.dialog.style.left = `${rect.left}px`;
        this.dialog.style.top = `${rect.bottom + 10}px`;
      }

      const rect = this.anchorEl.getBoundingClientRect();
      const dialogHeight = this.dialog.offsetHeight + 10;
      const spaceAbove = rect.top;
      const spaceBelow = window.innerHeight - rect.bottom;

      // Set the correct position
      if (spaceBelow >= dialogHeight) {
        positionDialogBelow();
      }
      else if (spaceAbove >= dialogHeight) {
        positionDialogAbove();
      }
      else {
        positionDialogBelow();  // Default
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
      customElements.define('fluent-popover', FluentPopover);
    }
  };
}

//import { PopoverDataItem, PopoverData } from "./PopoverData";
//import { PopoverOptions } from "./PopoverOptions";

//export namespace Microsoft.FluentUI.Blazor.Components.Popover {

//  /**
//   * Initialize the popover with the given options
//   */
//  export function Initialize(options: PopoverOptions): void {
//    const target = document.getElementById(options.anchorId);
//    const openBtn = document.getElementById(options.triggerId ?? options.anchorId);
//    const dialog = document.getElementById(options.dialogId) as HTMLDialogElement;

//    // Validate elements
//    if (!target || !openBtn || !dialog) {
//      console.error("Popover: Target, Trigger or Dialog element not found.");
//      return;
//    }

//    const adjustDialogPosition = () => {

//      // Position dialog above the target
//      const positionDialogAbove = () => {
//        dialog.style.left = `${rect.left + window.scrollX}px `;
//        dialog.style.top = `${rect.top + window.scrollY - dialogHeight}px`;
//      }

//      // Position dialog below the target
//      const positionDialogBelow = () => {
//        dialog.style.left = `${rect.left + window.scrollX}px`;
//        dialog.style.top = `${rect.bottom + window.scrollY + options.offsetVertical}px`;
//      }

//      const rect = target.getBoundingClientRect();
//      const dialogHeight = dialog.offsetHeight + options.offsetVertical;
//      const spaceAbove = rect.top;
//      const spaceBelow = window.innerHeight - rect.bottom;

//      console.log(`rect ${Math.trunc(rect.left)} ${Math.trunc(rect.top)}`);

//      // Set the correct position
//      if (spaceBelow >= dialogHeight) {
//        positionDialogBelow();
//      }
//      else if (spaceAbove >= dialogHeight) {
//        positionDialogAbove();
//      }
//      else {
//        positionDialogBelow();  // Default
//      }

//      console.log(dialog.style.left, dialog.style.top);

//    };

//    const popup_windowResize = (e: UIEvent): void => {
//      if (dialog.open) {
//        adjustDialogPosition();
//      }
//    }

//    const popup_windowScroll = (e: Event): void => {
//      if (dialog.open) {
//        adjustDialogPosition();
//      }
//    }

//    const popup_buttonClick = (e: MouseEvent | TouchEvent): void => {
//      if (!dialog.open) {
//        dialog.show(); // or dialog.showModal() if you want modal behavior
//        adjustDialogPosition();
//      }
//    }

//    const popup_outsideClick = (e: MouseEvent | TouchEvent): void => {
//      if (dialog.open && !dialog.contains(e.target as Node) && e.target !== openBtn) {
//        dialog.close();
//      }
//    }

//    const popup_dialogKeydown = (e: KeyboardEvent): void => {
//      if (e.key === 'Escape' && dialog.open) {
//        dialog.close();
//      }
//    }

//    let lastRect = target.getBoundingClientRect();
//    const checkPositionChange = () => {
//      const rect = target.getBoundingClientRect();
//      if (rect.x !== lastRect.x || rect.y !== lastRect.y) {
//        console.log('Target moved!', rect);
//        lastRect = rect;
//      }

//      // This uses requestAnimationFrame for efficiency (runs at ~60fps and pauses when tab is inactive).
//      requestAnimationFrame(checkPositionChange);
//    }

//    /**
//     * Gets or creates a popover data item for the given ID and options.
//     * @param id
//     * @param options
//     * @returns
//     */
//    const getPopoverData = (id: string, options: PopoverOptions): PopoverDataItem => {
//      if (!(document as any).__fluentPopover || !(document as any).__fluentPopover.Data) {
//        ((document as any).__fluentPopover as PopoverData) = {
//          Data: new Map<string, PopoverDataItem>()
//        };;
//      }

//      const data = ((document as any).__fluentPopover as PopoverData).Data;
//      data.set(id, {
//        options: options,
//        resizeHandler: popup_windowResize,
//        clickHandler: popup_buttonClick,
//        outsideHandler: popup_outsideClick,
//        scrollHandler: popup_windowScroll,
//        keydownHandler: popup_dialogKeydown,
//      });

//      return data.get(id) as PopoverDataItem;
//    }

//    // Get or create a popover object if it doesn't exist
//    const popover = getPopoverData(options.id, options);

//    // Attach event listeners
//    openBtn.addEventListener('click', popover.clickHandler);
//    //window.addEventListener('resize', popover.resizeHandler);
//    //window.addEventListener('scroll', popover.scrollHandler);
//    window.addEventListener('click', popover.outsideHandler);
//    window.addEventListener('keydown', popover.keydownHandler);

//    checkPositionChange();
//  }
//}
