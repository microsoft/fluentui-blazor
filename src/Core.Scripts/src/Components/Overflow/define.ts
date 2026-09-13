/**
 * Registers the `<fluent-overflow>` custom element.
 */

import { definition } from './overflow.definition';

if (!customElements.get(definition.name)) {
  customElements.define(definition.name, definition.constructor);
}