/**
 * Provides the declarative component definition.
 */

import { Overflow } from './overflow.js';
import { tagName } from './overflow.types.js';

/** Definition for the `<fluent-overflow>` element. */
export const definition = {
  name: tagName,
  constructor: Overflow,
} as const;