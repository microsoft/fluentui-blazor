/**
 * Provides the declarative component definition.
 */

import { Microsoft as OverflowFile } from './overflow';
import { tagName } from './overflow.types';

/** Definition for the `<fluent-overflow>` element. */
export const definition = {
  name: tagName,
  constructor: OverflowFile.FluentUI.Blazor.Components.Overflow.Overflow,
} as const;