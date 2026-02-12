import { BUILD_MODE } from "./BuildConstants";

export interface Library {
  name: string;
  url: string;
  debugUrl: string;
}

/*
    ---------------------------------------------------------------------------------------

    How It Works:

    - package.json:       TypeScript types are available in devDependencies (e.g. `imask`).

    - esbuild.config.mjs: The external library is NOT bundled into your output (marked as external).

    - Source code:        The library follows the standard pattern and exposes itself as `window.<LiBName>` object (e.g. window.IMask).
                          When the `TextMasked.ts > await imaskLoader.load()` function is called,
                          it automatically loads the lib into this window property,
                          from the URLs defined in the Library object in `TextMasked.ts` if not already present

     Alternative: Pre-load IMask

       If you as a developer want more control, you can add a script tag to the HTML:

       ```
         <!-- In your _Host.cshtml, _Layout.cshtml, App.razor or index.html -->
         <script src="https://unpkg.com/imask@7.6.1/dist/imask.min.js"></script>
       ```

    ---------------------------------------------------------------------------------------
*/


/**
 * Generic class for dynamically loading external libraries from CDN.
 *
 * This class handles:
 * - Checking if the library is already loaded
 * - Preventing duplicate script loading
 * - Providing a promise-based API for library availability
 *
 * @example
 * ```typescript
 * const iMaskLibrary: Library = {
 *  name: 'IMask',
 *  url: 'https://unpkg.com/imask@7.6.1/dist/imask.min.js',
 *  debugUrl: 'https://unpkg.com/imask@7.6.1/dist/imask.js'
 * };
 *
 * // Create a loader instance for IMask
 * const imaskLoader = new ExternalLibraryLoader<typeof IMaskType>(iMaskLibrary);
 * :
 * const IMask = await imaskLoader.load();
 * ```
 */
export class ExternalLibraryLoader<T = any> {
  private loadPromise: Promise<T> | null = null;

  /**
   * Creates a new library loader instance.
   * @param library - The library object containing name and URLs
   */
  constructor(
    private readonly library: Library
  ) {}

  /**
   * Loads the external library if not already loaded.
   * @returns Promise that resolves with the library object
   */
  async load(): Promise<T> {
    // Check if library is already available on window
    const existingLib = (window as any)[this.library.name];
    if (existingLib) {
      return existingLib as T;
    }

    // If a load is already in progress, return the existing promise
    if (this.loadPromise) {
      return this.loadPromise;
    }

    // Load library from CDN
    this.loadPromise = new Promise((resolve, reject) => {
      const script = document.createElement('script');
      var url: string;
      if (BUILD_MODE === 'Debug') {
        url = this.library.debugUrl;
      } else {
        url = this.library.url;
      }
      script.src = url;
      script.onload = () => {
        const loadedLib = (window as any)[this.library.name];
        if (loadedLib) {
          resolve(loadedLib as T);
        } else {
          reject(new Error(`${this.library.name} library failed to load`));
        }
      };
      script.onerror = () => reject(new Error(`Failed to load ${this.library.name} from CDN: ${url}`));
      document.head.appendChild(script);
    });

    return this.loadPromise;
  }

  /**
   * Checks if the library is already loaded without triggering a load.
   * @returns true if the library is available on the window object
   */
  isLoaded(): boolean {
    return !!(window as any)[this.library.name];
  }
}
