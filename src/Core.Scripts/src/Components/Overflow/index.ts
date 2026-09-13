/**
 * Exposes the public API of the `Overflow` folder.
 */

export {
  OverflowController,
  fluentOverflowChangeEventName,
  overflowChangeEventName,
} from './overflow-controller.js';

export {
  disposeOverflow,
  getOverflowItemIds,
  initializeOverflow,
  refreshOverflow,
} from './overflow-interop.js';

export {
  tagName as OverflowTagName,
  type OverflowChangeDetail,
  type OverflowControllerOptions,
  type OverflowDirection,
  type OverflowItem,
  type OverflowOrientation,
  type OverflowToggleDetail,
} from './overflow.types.js';

export { definition as OverflowDefinition } from './overflow.definition.js';
export { styles as OverflowStyles } from './overflow.styles.js';
export { template as OverflowTemplate } from './overflow.template.js';
export { Overflow } from './overflow.js';