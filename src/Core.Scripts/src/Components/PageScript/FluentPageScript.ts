import { StartedMode } from "../../d-ts/StartedMode";

export namespace Microsoft.FluentUI.Blazor.Components.PageScript {

  const pageScriptInfoBySrc = new Map();

  export class FluentPageScript extends HTMLElement {
    static observedAttributes = ['src'];
    src: string | null = null;

    attributeChangedCallback(name: string | null, oldValue: string | null, newValue: string | null): void {
      if (name !== 'src') {
        return;
      }

      this.src = newValue;
      this.unregisterPageScriptElement(oldValue);
      this.registerPageScriptElement(newValue);
    }

    disconnectedCallback(): void {
      this.unregisterPageScriptElement(this.src);
    }

    registerPageScriptElement(src: string | null): void {
      if (!src) {
        throw new Error('Must provide a non-empty value for the "src" attribute.');
      }

      let pageScriptInfo = pageScriptInfoBySrc.get(src);

      if (pageScriptInfo) {
        pageScriptInfo.referenceCount++;
      } else {
        pageScriptInfo = { referenceCount: 1, module: null };
        pageScriptInfoBySrc.set(src, pageScriptInfo);
        this.initializePageScriptModule(src, pageScriptInfo);
      }
    }

    unregisterPageScriptElement(src: string | null): void {
      if (!src) {
        return;
      }

      const pageScriptInfo = pageScriptInfoBySrc.get(src);
      if (!pageScriptInfo) {
        return;
      }

      pageScriptInfo.referenceCount--;
    }

    async initializePageScriptModule(src: string, pageScriptInfo: any): void {
      if (src.startsWith("./")) {
        src = new URL(src.substring(2), document.baseURI).toString();
      }

      const module = await import(src);

      if (pageScriptInfo.referenceCount <= 0) {
        return;
      }

      pageScriptInfo.module = module;
      module.onLoad?.();
      module.onUpdate?.();
    }
  }

  const onEnhancedLoad = (): void => {
    for (const [src, { module, referenceCount }] of pageScriptInfoBySrc) {
      if (referenceCount <= 0) {
        module?.onDispose?.();
        pageScriptInfoBySrc.delete(src);
      }
    }

    for (const { module } of pageScriptInfoBySrc.values()) {
      module?.onUpdate?.();
    }
  }

  /**
   * Register the FluentPageScript component
   * @param blazor
   * @param mode
   */
  export const registerComponent = (blazor: Blazor, mode: StartedMode): void => {
    if (typeof blazor.addEventListener === 'function' && mode === StartedMode.Web) {
      customElements.define('fluent-page-script', FluentPageScript);
      blazor.addEventListener('enhancedload', onEnhancedLoad);
    }
  };

}
