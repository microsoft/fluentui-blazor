/**
 * Registers the `<fluent-overflow>` custom element.
 */

import { definition } from './overflow.definition.js';

if (!customElements.get(definition.name)) {
  customElements.define(definition.name, definition.constructor);
}