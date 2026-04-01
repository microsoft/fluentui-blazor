import * as FluentUIComponents from '@fluentui/web-components'

export namespace Microsoft.FluentUI.Blazor.Components.NumberInput {

  /**
   * Sets the step, min, and max attributes on the inner <input> element of a <fluent-text-input>.
   * The web component exposes its internal input via the `.control` property.
   * @param {string} id - The element ID
   * @param {string | null} step - The step attribute value
   * @param {string | null} min - The min attribute value
   * @param {string | null} max - The max attribute value
   */
  export function setNumberAttributes(id: string, step: string | null, min: string | null, max: string | null): void {
    const element = document.getElementById(id) as FluentUIComponents.TextInput | null;
    if (!element) {
      return;
    }

    const control = element.control;
    if (!control) {
      return;
    }

    if (step !== null && step !== undefined) {
      control.setAttribute('step', step);
    } else {
      control.removeAttribute('step');
    }

    if (min !== null && min !== undefined) {
      control.setAttribute('min', min);
    } else {
      control.removeAttribute('min');
    }

    if (max !== null && max !== undefined) {
      control.setAttribute('max', max);
    } else {
      control.removeAttribute('max');
    }
  }
}
