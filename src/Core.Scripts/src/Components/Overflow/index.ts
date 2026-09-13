/**
 * Exposes the public API of the `Overflow` folder.
 */

import { Microsoft as OverflowControllerFile } from './overflow-controller';
import { Microsoft as OverflowInteropFile } from './overflow-interop';
import { Microsoft as OverflowFile } from './overflow';

export import OverflowController = OverflowControllerFile.FluentUI.Blazor.Components.Overflow.OverflowController;
export import fluentOverflowChangeEventName = OverflowControllerFile.FluentUI.Blazor.Components.Overflow.fluentOverflowChangeEventName;
export import overflowChangeEventName = OverflowControllerFile.FluentUI.Blazor.Components.Overflow.overflowChangeEventName;

export import disposeOverflow = OverflowInteropFile.FluentUI.Blazor.Components.Overflow.disposeOverflow;
export import getOverflowItemIds = OverflowInteropFile.FluentUI.Blazor.Components.Overflow.getOverflowItemIds;
export import initializeOverflow = OverflowInteropFile.FluentUI.Blazor.Components.Overflow.initializeOverflow;
export import refreshOverflow = OverflowInteropFile.FluentUI.Blazor.Components.Overflow.refreshOverflow;

export {
  tagName as OverflowTagName,
  type OverflowChangeDetail,
  type OverflowControllerOptions,
  type OverflowDirection,
  type OverflowItem,
  type OverflowOrientation,
  type OverflowToggleDetail,
} from './overflow.types';

export { definition as OverflowDefinition } from './overflow.definition';
export { styles as OverflowStyles } from './overflow.styles';
export { template as OverflowTemplate } from './overflow.template';
export import Overflow = OverflowFile.FluentUI.Blazor.Components.Overflow.Overflow;