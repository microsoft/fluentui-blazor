// Type-only imports - provides TypeScript types without bundling the library
import type IMaskType from 'imask';
import type { InputMask } from 'imask';
import * as FluentUIComponents from '@fluentui/web-components'
import { ExternalLibraryLoader, Library } from '../../ExternalLibs';

/**
 * See the imask docs at: https://github.com/uNmAnNeR/imaskjs
 *
 * The Default Lib Name and URL and debug URL to load the IMask library from CDN.
 * Debug URL can be used in development for better debugging experience with source maps, while the minified URL is recommended for production for better performance.
 * If no unminified version is available, the same minified URL can be used for both.
 *
 * The dev can override what script will be used in the end by adding a script before loading FluentUI.Blazor scripts:
 * <script src="https://unpkg.com/imask@7.6.1/dist/imask.min.js"></script>
 */


const iMaskLibrary: Library = {
  name: 'IMask',
  url: 'https://unpkg.com/imask@7.6.1/dist/imask.min.js',
  debugUrl: 'https://unpkg.com/imask@7.6.1/dist/imask.js'
};

// Create a loader instance for IMask
const imaskLoader = new ExternalLibraryLoader<typeof IMaskType>(iMaskLibrary);

export namespace Microsoft.FluentUI.Blazor.Components.TextMasked {

  /**
   * Applies a pattern mask to a text input element.
   * Doc: https://imask.js.org/guide.html#masked-pattern
   * @param id The ID of the input element.
   * @param mask The mask pattern to apply.
   */
  export async function applyPatternMask(id: string, mask: string, lazy: boolean, placeholderChar: string): Promise<void> {

    const fluentText = document.getElementById(id) as FluentUIComponents.TextInput;
    const inputElement = getInputElement(id);

    // Define mask options
    const maskOptions =
    {
      mask: mask,
      lazy: lazy,                       // false: make placeholder always visible
      placeholderChar: placeholderChar, // defaults to '_'
    };

    // Destroy existing mask if any
    if (inputElement?.mask) {
      inputElement.mask.destroy();
      if (inputElement.maskInputHandler) {
        inputElement.removeEventListener('input', inputElement.maskInputHandler);
        inputElement.maskInputHandler = undefined;
      }
      if (inputElement.maskChangeObserver) {
        inputElement.maskChangeObserver.disconnect();
        inputElement.maskChangeObserver = undefined;
      }
      if (inputElement.maskChangeHandler) {
        inputElement.removeEventListener('change', inputElement.maskChangeHandler);
        inputElement.maskChangeHandler = undefined;
      }
    }

    // Apply new mask
    if (inputElement && mask.length > 0) {
      // Ensure IMask library is loaded from CDN
      const IMask = await imaskLoader.load();

      inputElement.mask = IMask(inputElement, maskOptions);

      // Workaround to update FluentTextInput value
      inputElement.maskInputHandler = () => {
        (fluentText as any)._currentValue = inputElement.value;
      };
      inputElement.addEventListener('input', inputElement.maskInputHandler);

      // Detect when the input value is changed externally
      // and synchronize with mask using updateValue()
      inputElement.maskChangeHandler = () => {
        const currentInputValue = inputElement.value;
        const currentMaskValue = inputElement.mask?.value || '';

        if (currentInputValue !== currentMaskValue) {
          inputElement.mask?.updateValue();
        }
      }

      inputElement.maskChangeObserver = new MutationObserver(() => {
        if (inputElement.maskChangeHandler) {
          inputElement.maskChangeHandler(new Event('change'));
        }
      });
      inputElement.maskChangeObserver.observe(fluentText, { attributes: true, attributeFilter: ['value'] });

      inputElement.addEventListener('change', inputElement.maskChangeHandler);
    }
  }

  /**
   * Applies a number mask to a text input element.
   * Doc: https://imask.js.org/guide.html#masked-number
   * @param id The ID of the input element.
   * @param scale Digits after the decimal point, 0 for integers.
   * @param radixChar Fractional delimiter character.
   * @param mapToRadix Symbols to process as radix.
   * @param min Minimum value.
   * @param max Maximum value.
   * @param thousandsSeparator Thousands separator character.
   */
  export async function applyNumberMask(id: string, scale: number, radixChar: string, mapToRadix: string[], min: number | null, max: number | null, thousandsSeparator: string, step: number | null): Promise<void> {

    const fluentText = document.getElementById(id) as FluentUIComponents.TextInput;
    const inputElement = getInputElement(id);

    // Define mask options
    const maskOptions =
    {
      mask: Number,
      scale: scale,
      radix: radixChar,
      mapToRadix: mapToRadix,
      min: min !== null ? min : Number.NEGATIVE_INFINITY,
      max: max !== null ? max : Number.POSITIVE_INFINITY,
      thousandsSeparator: thousandsSeparator,
      normalizeZeros: true,
      padFractionalZeros: false,
      autofix: true,
    };

    // Destroy existing mask if any
    if (inputElement?.mask) {
      inputElement.mask.destroy();
      if (inputElement.maskInputHandler) {
        inputElement.removeEventListener('input', inputElement.maskInputHandler);
        inputElement.maskInputHandler = undefined;
      }
      if (inputElement.maskChangeObserver) {
        inputElement.maskChangeObserver.disconnect();
        inputElement.maskChangeObserver = undefined;
      }
      if (inputElement.maskChangeHandler) {
        inputElement.removeEventListener('change', inputElement.maskChangeHandler);
        inputElement.maskChangeHandler = undefined;
      }
      if (inputElement.maskKeydownHandler) {
        inputElement.removeEventListener('keydown', inputElement.maskKeydownHandler);
        inputElement.maskKeydownHandler = undefined;
      }
    }

    // Apply new mask
    if (inputElement) {
      // Ensure IMask library is loaded from CDN
      const IMask = await imaskLoader.load();

      inputElement.mask = IMask(inputElement, maskOptions);

      // Workaround to update FluentTextInput value
      inputElement.maskInputHandler = () => {
        (fluentText as any)._currentValue = inputElement.value;
      };
      inputElement.addEventListener('input', inputElement.maskInputHandler);

      // Detect when the input value is changed externally
      // and synchronize with mask using updateValue()
      inputElement.maskChangeHandler = () => {
        const currentInputValue = inputElement.value;
        const currentMaskValue = inputElement.mask?.value || '';

        if (currentInputValue !== currentMaskValue) {
          inputElement.mask?.updateValue();
        }
      }

      inputElement.maskChangeObserver = new MutationObserver(() => {
        if (inputElement.maskChangeHandler) {
          inputElement.maskChangeHandler(new Event('change'));
        }
      });
      inputElement.maskChangeObserver.observe(fluentText, { attributes: true, attributeFilter: ['value'] });

      inputElement.addEventListener('change', inputElement.maskChangeHandler);

      // ArrowUp/ArrowDown to increment/decrement by step
      if (step !== 0 && step !== null) {
        inputElement.maskKeydownHandler = (event: KeyboardEvent) => {
          if (event.key !== 'ArrowUp' && event.key !== 'ArrowDown') {
            return;
          }

          event.preventDefault();

          const currentValue = parseFloat(inputElement.mask?.unmaskedValue || '0') || 0;
          const delta = event.key === 'ArrowUp' ? step : -step;
          let newValue = currentValue + delta;

          // Round to avoid floating-point precision errors (e.g. 0.3 - 0.1 = 0.19999999999999998)
          if (scale > 0) {
            newValue = parseFloat(newValue.toFixed(scale));
          } else {
            newValue = Math.round(newValue);
          }

          // Clamp to min/max if defined
          if (min !== null && newValue < min) {
            newValue = min;
          }
          if (max !== null && newValue > max) {
            newValue = max;
          }
          
          inputElement.mask!.unmaskedValue = String(newValue);
          (fluentText as any)._currentValue = inputElement.value;
          inputElement.dispatchEvent(new Event('change', { bubbles: true }));
        };
        inputElement.addEventListener('keydown', inputElement.maskKeydownHandler);
      }
    }
  }

  /**
   * Returns the input element inside a FluentTextInput component by its ID.
   * @param id
   * @returns
   */
  function getInputElement(id: string): HTMLMaskedInputElement | null {
    const inputElement = document.getElementById(id)?.shadowRoot?.querySelector("input")

    if (inputElement && inputElement.type === "text") {
      return inputElement as HTMLMaskedInputElement;
    }

    return null;
  }

  /**
   * Extending HTMLInputElement to include mask and other properties
   */
  interface HTMLMaskedInputElement extends HTMLInputElement {
    mask?: InputMask;
    maskInputHandler?: (event: Event) => void;
    maskChangeHandler?: (event: Event) => void;
    maskChangeObserver?: MutationObserver;
    maskKeydownHandler?: (event: KeyboardEvent) => void;
  }
}
