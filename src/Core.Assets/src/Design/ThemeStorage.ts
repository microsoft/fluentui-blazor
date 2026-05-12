import { DesignTheme } from "../DesignTheme";

type ThemeStorageValue = { mode: string | null, primaryColor: string | null, neutralColor: string | null };

type StorageLike = Pick<Storage, "getItem" | "setItem" | "removeItem">;

const memoryStorage = (() => {
  const values: Record<string, string> = {};

  return {
    getItem(key: string): string | null {
      return Object.prototype.hasOwnProperty.call(values, key) ? values[key] : null;
    },
    setItem(key: string, value: string): void {
      values[key] = value;
    },
    removeItem(key: string): void {
      delete values[key];
    },
  } satisfies StorageLike;
})();

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

  private static getStorage(): StorageLike {
    try {
      const storage = window.localStorage;
      if (storage == null) {
        return memoryStorage;
      }

      return storage;
    } catch {
      return memoryStorage;
    }
  }

  public updateLocalStorage(mode: string | null, primaryColor: string | null, neutralColor: string | null): void {
    const storage = ThemeStorage.getStorage();

    // Wait the component to be initialized
    if (!this._designTheme._isInitialized) {
      return;
    }

    // Check if storageName attribute is defined
    if (this.storageName == null) {
      return;
    }

    try {
      storage.setItem(this.storageName, JSON.stringify({
        mode: ThemeStorage.getValueOrNull(mode),
        primaryColor: ThemeStorage.getValueOrNull(primaryColor),
        neutralColor: ThemeStorage.getValueOrNull(neutralColor),
      }));
    } catch {
      // Ignore storage write failures and continue with in-memory theme state.
    }
  }

  public readLocalStorage(): ThemeStorageValue | null {
    const storage = ThemeStorage.getStorage();

    // Check if storageName attribute is defined
    if (this.storageName == null) {
      return null;
    }

    try {
      const storageJson = storage.getItem(this.storageName);
      if (storageJson == null) {
        return null;
      }

      // Read the localstorage
      const storageItems = JSON.parse(storageJson);

      return {
        mode: ThemeStorage.getValueOrNull(storageItems?.mode),
        primaryColor: ThemeStorage.getValueOrNull(storageItems?.primaryColor),
        neutralColor: ThemeStorage.getValueOrNull(storageItems?.neutralColor),
      }
    } catch {
      this.clearLocalStorage();
      return null;
    }
  }

  public clearLocalStorage(): void {
    const storage = ThemeStorage.getStorage();

    // Check if storageName attribute is defined
    if (this.storageName == null) {
      return;
    }

    try {
      storage.removeItem(this.storageName);
    } catch {
      // Ignore storage clear failures and continue with in-memory theme state.
    }
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
