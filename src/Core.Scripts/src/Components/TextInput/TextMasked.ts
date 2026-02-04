// Type-only imports - provides TypeScript types without bundling the library
import type IMaskType from 'imask';
import type { InputMask } from 'imask';
import * as FluentUIComponents from '@fluentui/web-components'
import { ExternalLibraryLoader, IMaskName, IMaskUrl } from '../../ExternalLibs';

// Doc: https://github.com/uNmAnNeR/imaskjs

// Create a loader instance for IMask
const imaskLoader = new ExternalLibraryLoader<typeof IMaskType>(IMaskName, IMaskUrl);

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
  }
}
