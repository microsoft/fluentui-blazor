import { DesignTheme } from "../DesignTheme";

class ThemeStorage {

  private _designTheme: DesignTheme

  /**
   * Initializes a new instance of ThemeStorage
   * @param designTheme DesignTheme component
   */
  constructor(designTheme: DesignTheme) {
    this._designTheme = designTheme;
  }

  /**
   * Gets the value of the DesignTheme localStorage attribute.
   */
  get storageName(): string | null {
    return this._designTheme.localStorage;
  }


  public updateLocalStorage(mode: string | null, primaryColor: string | null): void {
    // Wait the component to be initialized
    if (!this._designTheme._isInitialized) {
        return;
    }

    // Check if LocalStorage attribute is defined
    if (this.storageName == null) {
      return;
    }

    // Save to the localstorage
    localStorage.setItem(this.storageName, JSON.stringify({
      mode: mode,
      primaryColor: primaryColor,
    }));
  }

  public readLocalStorage(): { mode: string | null, primaryColor: string | null } | null {

    // Check if LocalStorage attribute is defined
    if (this.storageName == null) {
      return null;
    }

    // Read the localstorage
    const storageItems = JSON.parse(localStorage.getItem(this.storageName) ?? "{ }");

    return {
      mode: storageItems?.mode,
      primaryColor: storageItems?.primaryColor,
    }
  }
}

export { ThemeStorage };