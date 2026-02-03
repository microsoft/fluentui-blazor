/**
 * The Default URL to load the IMask library from CDN.
 *
 * The dev can override this value by adding a script before loading FluentUI.Blazor scripts:
 * <script src="https://unpkg.com/imask@7.6.1/dist/imask.min.js"></script>
 */
export const IMaskUrl = 'https://unpkg.com/imask@7.6.1/dist/imask.min.js';

/*
    How It Works:

    - package.json:       TypeScript types are available in devDependencies (e.g. `imask`).

    - esbuild.config.mjs: The external library is NOT bundled into your output (marked as external).
    
    - Source code:        The library follows the standard pattern and exposes itself as window.<LiBName> object (e.g. window.IMask).
                          When the `TextMasked.ts > applyPatternMask()` function is called,
                          it automatically loads the lib into this window property,
                          from the URL defined in `ExternalLibs.ts` if not already present     

     Alternative: Pre-load IMask

       If the developer want more control, he can add the script tag to the HTML:

       ```
         <!-- In your _Host.cshtml, _Layout.cshtml, App.razor or index.html -->
         <script src="https://unpkg.com/imask@7.6.1/dist/imask.min.js"></script>
       ```
*/