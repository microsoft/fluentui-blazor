const pageScriptInfoBySrc = new Map();
class FluentPageScript extends HTMLElement {
  static observedAttributes = ['src'];
  src: string | null = null;

  attributeChangedCallback(name: string | null, oldValue: string | null, newValue: string | null) {
    if (name !== 'src') {
      return;
    }

    this.src = newValue;
    this.unregisterPageScriptElement(oldValue);
    this.registerPageScriptElement(newValue);
  }

  disconnectedCallback() {
    this.unregisterPageScriptElement(this.src);
  }

  registerPageScriptElement(src: string | null) {
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

  unregisterPageScriptElement(src: string | null) {
    if (!src) {
      return;
    }

    const pageScriptInfo = pageScriptInfoBySrc.get(src);
    if (!pageScriptInfo) {
      return;
    }

    pageScriptInfo.referenceCount--;
  }

  async initializePageScriptModule(src: string, pageScriptInfo: any) {
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

export function onEnhancedLoad() {
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

export { FluentPageScript };
