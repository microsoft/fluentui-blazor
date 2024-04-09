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

    // If LocalStorage is not available, do nothing.
    if (localStorage == null) {
      return;
    }

    // Wait the component to be initialized
    if (!this._designTheme._isInitialized) {
      return;
    }

    // Check if storageName attribute is defined
    if (this.storageName == null) {
      return;
    }

    // Save to the localstorage
    localStorage.setItem(this.storageName, JSON.stringify({
      mode: ThemeStorage.getValueOrNull(mode),
      primaryColor: ThemeStorage.getValueOrNull(primaryColor),
    }));
  }

  public readLocalStorage(): { mode: string | null, primaryColor: string | null } | null {

    // If LocalStorage is not available, do nothing.
    if (localStorage == null) {
      return null;
    }

    // Check if storageName attribute is defined
    if (this.storageName == null) {
      return null;
    }

    // Check if localstorage exists
    const storageJson = localStorage.getItem(this.storageName);

    if (storageJson == null) {
      return null;
    }

    // Read the localstorage
    const storageItems = JSON.parse(storageJson);

    return {
      mode: ThemeStorage.getValueOrNull(storageItems?.mode),
      primaryColor: ThemeStorage.getValueOrNull(storageItems?.primaryColor),
    }
  }

  public clearLocalStorage(): void {
    // If LocalStorage is not available, do nothing.
      if (localStorage == null) {
        return;
      }
  
      // Check if storageName attribute is defined
      if (this.storageName == null) {
        return;
      }
  
      // Clear the localstorage
      localStorage.removeItem(this.storageName);
  }

  /**
 * Return null or the specified value
 * @param value
 * @returns
 */
  public static getValueOrNull(value: any) {
    return value == null || value == undefined || value == "null" || value == "undefined" ? null : value;
  }
}

export { ThemeStorage };
