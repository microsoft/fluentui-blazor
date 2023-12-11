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
   * Gets the value of the DesignTheme storageName attribute.
   */
  get storageName(): string | null {
    return this._designTheme.storageName;
  }


  public updateLocalStorage(mode: string | null, primaryColor: string | null): void {
    // Wait the component to be initialized
    if (!this._designTheme._isInitialized) {
        return;
    }

    // Check if storageName attribute is defined
    if (this.storageName == null) {
      return;
    }

    if (mode == "null") mode = null;
    if (primaryColor == "null") primaryColor = null;

    // Save to the localstorage
    localStorage.setItem(this.storageName, JSON.stringify({
      mode: mode,
      primaryColor: primaryColor,
    }));
  }

  public readLocalStorage(): { mode: string | null, primaryColor: string | null } | null {

    // Check if storageName attribute is defined
    if (this.storageName == null) {
      return null;
    }

    // Read the localstorage
    const storageItems = JSON.parse(localStorage.getItem(this.storageName) ?? "{ }");

    return {
      mode: storageItems?.mode == "null" ? null : storageItems?.mode,
      primaryColor: storageItems?.primaryColor == "null" ? null : storageItems?.primaryColor,
    }
  }
}

export { ThemeStorage };