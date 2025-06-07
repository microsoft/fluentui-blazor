import { StartedMode } from "../../d-ts/StartedMode";

export namespace Microsoft.FluentUI.Blazor.Components.Popover {

  class FluentPopover extends HTMLElement {

    static get observedAttributes() {
      return ['anchor-id'];
    }

    //private _onResize: () => void;
    private _anchorElement: HTMLElement | null = null;
    private _triggerElement: HTMLElement | null = null;

    /**
     * Gets the dialog element within the shadow DOM.
     */
    private get dialog(): HTMLDialogElement | null {
      if (!this.shadowRoot) {
        console.error('FluentPopover: Shadow root is not initialized.');
        return null;
      }

      return this.shadowRoot.querySelector('dialog');
    }

    constructor() {
      super();
      //this._onResize = this.handleResize.bind(this);
      this.attachShadow({ mode: 'open' });
    }

    /**
     * Initialize the popover component
     */
    connectedCallback() {

      const triggerButton = this._triggerElement ?? this._anchorElement;
      console.log(`FluentPopover: connectedCallback called. Trigger element: ${triggerButton?.id ?? 'none'}`);
      triggerButton?.addEventListener('click', this.handleAnchorClick);

      //window.addEventListener('resize', this._onResize);
      this.render();
    }

    /**
     * Dispose of the popover component
     */
    disconnectedCallback() {
      const triggerButton = this._triggerElement ?? this._anchorElement;
      triggerButton?.removeEventListener('click', this.handleAnchorClick);

      //window.removeEventListener('resize', this._onResize);
    }

    /**
     * Detect when an attribute changes
     * @param attrName
     * @param oldVal
     * @param newVal
     */
    attributeChangedCallback(attrName: string, oldVal: string | null, newVal: string | null) {

      console.log(`FluentPopover: attributeChangedCallback called for ${attrName} from ${oldVal} to ${newVal}`);

      if (attrName === 'anchor-id' && newVal !== oldVal) {
        this._anchorElement = document.getElementById(newVal ?? '');
        console.log(document.getElementById(newVal ?? ''));
        this.render();
      }

      if (attrName === 'trigger-id' && newVal !== oldVal) {
        this._triggerElement = document.getElementById(newVal ?? '');
        this.render();
      }

      console.log(`attributeChangedCallback ${this._anchorElement} - ${this._triggerElement}`);

    }

    private handleAnchorClick(event: MouseEvent): void {
      if (this.dialog) {
        console.log('FluentPopover: Anchor clicked. Showing dialog...');
        this.dialog.show();
      }
    }

    private handleResize(): void {
      console.log('Window resized. Updating anchor position...');
      //this.updateAnchorPosition();
    }

    /**
     * Render the popover component
     * @returns
     */
    private render() {
      if (!this.shadowRoot) return;

      this.shadowRoot.innerHTML = `
      <dialog >
        ${this.innerHTML}
      </dialog>
    `;
    }
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
