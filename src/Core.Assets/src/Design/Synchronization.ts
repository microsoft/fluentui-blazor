import { DesignTheme } from "../DesignTheme";

class Synchronization {

  private _designTheme: DesignTheme

  /**
   * Initializes a new instance of Synchronization
   * @param designTheme DesignTheme component
   */
  constructor(designTheme: DesignTheme) {
    this._designTheme = designTheme;
  }

  /**
   * Synchronize the attribute value with an external value (from another component).
   * @param id
   * @param name
   * @param value
   */
  public synchronizeAttribute = (id: string, name: string, value: string | null): void => {

    if (this._designTheme.id === id) {
      return;
    }

    this._designTheme.dispatchAttributeChanged(name, this._designTheme.getAttribute(name), value);
    this._designTheme.updateAttribute(name, value);
  }

  /**
   * Start the attribute synchronization with other components.
   * @param name
   * @param value
   */
  public synchronizeOtherComponents(name: string, value: string | null) {

    if (!this._designTheme._isInitialized) {
      return;
    }

    const components = document.querySelectorAll(`fluent-design-theme:not([id="${this._designTheme.id}"])`);
    for (let i = 0; i < components.length; i++) {
      const component = components[i] as DesignTheme;
      if (component.synchronization.synchronizeAttribute instanceof Function) {
        setTimeout(component.synchronization.synchronizeAttribute, 0, this._designTheme.id, name, value);
        //component.synchronization.synchronizeAttribute(this._designTheme.id, name, value);
      }
    }
  }

}

export { Synchronization };
