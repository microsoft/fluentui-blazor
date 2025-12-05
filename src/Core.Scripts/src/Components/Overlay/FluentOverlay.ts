import { StartedMode } from "../../d-ts/StartedMode";
import { fluentOverlayStyles } from "./FluentOverlay-Styles";

export namespace Microsoft.FluentUI.Blazor.Components.Overlay {

  export type CloseMode = 'all' | 'inside' | 'outside' | null;

  class FluentOverlay extends HTMLElement {

    private container: HTMLElement | null = null;
    private dialog: HTMLDialogElement;
    private resizeObserver: ResizeObserver | null = null;
    private clickHandler: ((ev: MouseEvent) => any) | null = null;

    private _opened: boolean = false;

    /************************
      Initialization
     ************************/
    constructor() {
      super();
    }

    // Delay initialization to ensure child content is parsed
    connectedCallback() {
      setTimeout(() => {
        this.initialize();
      }, 0);
    }

    private initialize() {
      this.container = this.parentElement;
      const shadow = this.attachShadow({ mode: 'open' });

      // Create the dialog element
      this.dialog = document.createElement('dialog') as HTMLDialogElement;
      this.dialog.setAttribute('fuib', '');
      this.dialog.setAttribute('part', 'dialog');    // To allow styling using `fluent-overlay::part(dialog)`

      // Prevent to use ESC key to close the dialog
      // https://developer.mozilla.org/en-US/docs/Web/API/HTMLDialogElement/closedBy#browser_compatibility
      this.dialog.setAttribute('closedBy', 'none');

      // Set initial styles for the dialog
      const sheet = new CSSStyleSheet();
      sheet.replaceSync(fluentOverlayStyles);
      this.shadowRoot!.adoptedStyleSheets = [
        ...(this.shadowRoot!.adoptedStyleSheets || []),
        sheet
      ];

      // Slot for user content
      const contentSlot = document.createElement('slot');
      while (this.firstChild) {
        contentSlot.appendChild(this.firstChild);
      }
      this.dialog.appendChild(contentSlot);
      shadow.appendChild(this.dialog);
    }

    /************************
       Public Properties
    ************************/

    // Property getter/setter for fullscreen
    public get fullscreen(): boolean {
      return this.hasAttribute('fullscreen');
    }

    public set fullscreen(value: boolean) {
      if (value) {
        this.setAttribute('fullscreen', '');
      } else {
        this.removeAttribute('fullscreen');
      }
    }

    // Property getter/setter for interactive
    public get interactive(): boolean {
      return this.hasAttribute('interactive');
    }

    public set interactive(value: boolean) {
      if (value) {
        this.setAttribute('interactive', '');
      } else {
        this.removeAttribute('interactive');
      }
    }

    // Property getter/setter for interactive-outside
    public get closeMode(): CloseMode {
      return this.getAttribute('close-mode') as CloseMode;
    }

    public set closeMode(value: CloseMode) {
      if (value) {
        this.setAttribute('close-mode', value);
      } else {
        this.removeAttribute('close-mode');
      }
    }

    // Property getter/setter for background-color
    public get background(): string {
      return this.getAttribute('background') ?? 'color-mix(in srgb, var(--colorBackgroundOverlay) 40%, transparent)';
    }

    public set background(value: string) {
      if (value) {
        this.setAttribute('background', '');
      } else {
        this.removeAttribute('background');
      }
    }

    /************************
      Public methods
    ************************/


    // Public method to open the dialog
    public show() {
      if (this.dialog) {

        if (this.fullscreen === false) {
          this.ensureParentPositioning();
          this.createResizeObserver();
          this.positionDialogInContainer();
        }
        else {
          this.positionDialogClear();
        }

        if (this.background) {
          this.style.setProperty('--overlayBackground', this.background);
        }

        if (this.interactive || !this.fullscreen) {
          this.dialog.show();
        } else {
          this.dialog.showModal();
        }

        if (!this.clickHandler) {
          this.clickHandler = (e) => this.onClick(e);

          // Use capture phase and delay listener registration to avoid capturing the current click
          setTimeout(() => {
            document.addEventListener('click', this.clickHandler!);
          }, 0);
        }
      }
    }

    // Public method to close the dialog
    public close() {
      if (this.dialog) {
        this.dialog.close();

        // Remove the click event listener if it exists
        if (this.clickHandler) {
          document.removeEventListener('click', this.clickHandler);
          this.clickHandler = null;
        }
      }
    }

    /************************
      Private methods
    ************************/

    // Private method to handle click events
    private onClick(event: MouseEvent): void {
      if (this.dialog.open && event.target instanceof HTMLElement) {
        const insideDialog = this.isClickInsideDialog(event);
        event.stopPropagation();

        if (this.closeMode === `all` || this.closeMode === null) {
          this.close();
          return;
        }
        if (this.closeMode === `inside` && insideDialog) {
          this.close();
          return;
        }
        if (this.closeMode === `outside` && !insideDialog) {
          this.close();
          return;
        }
      }
    }

    // Private method to check if a click event is inside the dialog
    private isClickInsideDialog(event: MouseEvent): boolean {
      const dialogRect = this.dialog.getBoundingClientRect();
      const clickX = event.clientX;
      const clickY = event.clientY;

      return clickX >= dialogRect.left &&
        clickX <= dialogRect.right &&
        clickY >= dialogRect.top &&
        clickY <= dialogRect.bottom;
    }

    // Private method to ensure parent has proper positioning
    private ensureParentPositioning(): void {
      const parent = this.parentElement;
      if (parent) {
        const computedStyle = window.getComputedStyle(parent);
        const position = computedStyle.position;

        // Check if position is one of the required values
        if (position !== 'relative' && position !== 'absolute' && position !== 'fixed' && position !== 'sticky') {
          parent.style.position = 'relative';
        }
      }
    }

    // Private method to position the dialog in the container
    private positionDialogInContainer(): void {
      if (this.container && this.dialog && this.fullscreen === false) {
        const containerRect = this.container.getBoundingClientRect();
        this.dialog.style.top = `${containerRect.top + containerRect.height / 2}px`;
        this.dialog.style.left = `${containerRect.left + containerRect.width / 2}px`;
      }      
    }

    // Private method to clear the dialog position
    private positionDialogClear(): void {
      if (this.dialog) {
        this.dialog.style.top = '';
        this.dialog.style.left = '';
      }
    }

    // Subscribe to container size changes        
    private createResizeObserver(): void {
      if (!this.resizeObserver && this.container) {
        this.resizeObserver = new ResizeObserver(() => {
          this.positionDialogInContainer();
        });
        this.resizeObserver.observe(this.container);
      }
    }

    // Private method to clean up the resize observer
    private cleanResizeObserver(): void {
      this.dialog.style.top = '';
      this.dialog.style.left = '';
      this.resizeObserver?.disconnect();
      this.resizeObserver = null;
    }

    // Cleanup when element is removed from DOM
    disconnectedCallback() {
      this.cleanResizeObserver();

      // Remove the click event listener if it exists
      if (this.clickHandler) {
        document.removeEventListener('click', this.clickHandler);
        this.clickHandler = null;
      }
    }
  }

  /**
  * Register the FluentOverlay component
  * @param blazor
  * @param mode
  */
  export const registerComponent = (blazor: Blazor, mode: StartedMode): void => {
    if (typeof blazor.addEventListener === 'function' && mode === StartedMode.Web) {
      customElements.define('fluent-overlay', FluentOverlay);
    }
  };
}
