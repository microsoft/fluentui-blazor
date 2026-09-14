# fluent-overflow

`fluent-overflow` keeps as many child elements visible as its container allows. A required slotted `fluent-button` displays the number of hidden items, and a required slotted `fluent-tooltip` lists their text.

## Source structure

The component follows the module organization used by Fluent UI Web Components. Each file has a focused responsibility:

- `FluentOverflow.ts`: Defines the main `Overflow` class and the Web Component behavior.
- `FluentOverflowTemplate.ts`: Defines the HTML structure rendered in the Shadow DOM.
- `FluentOverflowStyles.ts`: Defines the component styles.
- `FluentOverflowTypes.ts`: Defines the public types and custom element tag name.
- `FluentOverflowController.ts`: Calculates and manages overflow independently of the custom element.
- `FluentOverflowInterop.ts`: Exposes the imperative API for attaching overflow behavior to existing elements.

## Usage

Add direct children. By default, every direct child participates in overflow calculations.

```html
<fluent-overflow aria-label="Document actions">
	<button id="new" behavior="fixed">New</button>
	<button id="open">Open</button>
	<button id="save">Save</button>
	<span id="file-name" behavior="ellipsis">A very long document name.docx</span>

	<fluent-button slot="trigger" id="document-overflow-trigger" appearance="subtle">
		<span target="count">0</span>
	</fluent-button>
	<fluent-tooltip slot="menu" anchor="document-overflow-trigger" positioning="below">
		<div target="content"></div>
	</fluent-tooltip>
</fluent-overflow>
```

## API

### Properties and attributes

| Property | Attribute | Type | Default | Description |
| --- | --- | --- | --- | --- |
| `overflowDirection` | `overflow-direction` | `'start' \| 'end'` | `'end'` | Side of the managed DOM order from which eligible items are hidden. The component positions its trigger at the corresponding logical CSS side in LTR and RTL layouts. |
| `orientation` | `orientation` | `'horizontal' \| 'vertical'` | `'horizontal'` | Main axis used for layout and measurement. |
| `maxOverflowItems` | `max-overflow-items` | `number` | Unlimited | Maximum number of entries rendered in menu content and returned in each event list. Positive fractional values are floored; non-positive, invalid, and non-finite values remove the limit. |
| `threshold` | `threshold` | `number` | `0` | Space in pixels reserved before overflow begins. Zero, negative, and non-finite values resolve to `0`. |
| `selector` | `selector` | `string` | `''` | CSS selector that limits managed direct children. An empty selector manages every direct child. |
| `visibleOnLoad` | `visible-on-load` | `boolean` | `true` | When set to `false`, hides the component with `visibility: hidden` until its first layout completes, preventing unmeasured items from flashing. |

Only direct children matching `selector` are hidden when they overflow. Other direct children stay visible and still count toward the available space. Children assigned to the `trigger` and `menu` customization slots are excluded from layout items. An invalid selector matches no children.

Give managed items stable, unique IDs when consuming event items or calling `getOverflowItemIds`. IDs are returned as provided; the component does not generate or validate them.

### Methods

| Method | Description |
| --- | --- |
| `refresh()` | Recalculates overflow synchronously when the component is connected. It also cancels a pending scheduled refresh. |
| `getItemIds()` | Returns the IDs of the items from the most recently completed layout, in managed DOM order. It does not trigger a refresh. |
| `dispose()` | Stops observation, restores visibility owned by the controller, and hides the trigger. Reconnecting the element starts management again. |

### Child attributes

| Attribute | Value | Description |
| --- | --- | --- |
| `hidden` | Boolean | Set while an item is hidden by overflow. A hidden state not owned by the controller is preserved and is not reported as an overflowed item. |
| `behavior` | `fixed` | Keeps the item visible. Other items are overflowed first. |
| `behavior` | `ellipsis` | Keeps the item visible and allows it to shrink with text truncation on horizontal layouts. |

Fixed content can still exceed a container that is physically too small to contain it. In that case the component preserves the fixed item as requested.

### Slots

| Slot | Description |
| --- | --- |
| Default | Direct child items managed by the overflow algorithm. |
| `trigger` | Required `fluent-button`. A descendant marked with `target="count"` receives the current hidden count. |
| `menu` | Required `fluent-tooltip`. A descendant marked with `target="content"` receives one `<div>` containing text per hidden item, up to `maxOverflowItems`. |

Give the trigger an `id` and set the tooltip's `anchor` to that ID. Both slotted elements remain in the light DOM, allowing `fluent-tooltip` to resolve its anchor.

```html
<fluent-overflow selector="[data-overflow-item]" style="inline-size: 300px">
	<button data-overflow-item behavior="fixed">Cut</button>
	<button data-overflow-item>Copy</button>
	<button data-overflow-item>Paste</button>
	<button data-overflow-item>Rename</button>
	<button data-overflow-item>Download</button>

	<fluent-button
		slot="trigger"
		id="custom-overflow-trigger"
		appearance="primary"
	>
		<span target="count">0</span> more
	</fluent-button>

	<fluent-tooltip
		slot="menu"
		anchor="custom-overflow-trigger"
		positioning="below-end"
	>
		<div target="content"></div>
	</fluent-tooltip>
</fluent-overflow>
```

The component updates only the targeted descendants, preserving surrounding custom text and markup. It also sets an accessible count label on the slotted trigger. Style the `[target="content"]` container in the light DOM when a specific list layout is required.

### Change events

`overflowchange` and `fluentoverflowchange` bubble through the composed tree
after the first completed layout and whenever their serialized detail changes.
Both events carry the same `OverflowChangeDetail` object. A recalculation whose
detail is unchanged does not emit either event.

```ts
interface OverflowItem {
	id: string;
	text: string;
	index: number;
}

interface OverflowChangeDetail {
	items: OverflowItem[];
	overflowCount: number;
}

document.querySelector('fluent-overflow')?.addEventListener('overflowchange', (event) => {
	const { items, overflowCount } = (event as CustomEvent<OverflowChangeDetail>).detail;
	console.log({ items, overflowCount });
});
```

The `items` list is limited to `maxOverflowItems`, while `overflowCount` always
reports the complete count before that limit is applied. Every entry in `items`
represents a hidden item: `text` is its trimmed `textContent`, and `index` is its
position among all managed items in DOM order. If the limit hides a change from
the serialized detail, no change event is emitted for that change.

### Toggle event

The custom element emits `fluentoverflowtoggle` when its slotted
`fluent-tooltip` opens or closes. The event bubbles through the composed tree.
It is not emitted by `OverflowController` when used on a standalone
host.

```ts
interface OverflowToggleDetail {
	totalItemsCount: number;
	totalHiddenCount: number;
	opened: boolean;
	items: OverflowItem[];
}

document.querySelector('fluent-overflow')?.addEventListener('fluentoverflowtoggle', (event) => {
	const detail = (event as CustomEvent<OverflowToggleDetail>).detail;
	console.log(detail);
});
```

`totalItemsCount` includes all managed items and `totalHiddenCount` includes all
items hidden by overflow. The `items` list contains hidden items in managed DOM
order and is limited by `maxOverflowItems`.

## Imperative host integration

Overflow behavior can also be attached to an existing element such as a
`fluent-tablist`, without wrapping or moving its children:

```ts
import { Microsoft as OverflowInteropFile } from './FluentOverflowInterop.js';

const {
	disposeOverflow,
	getOverflowItemIds,
	initializeOverflow,
	refreshOverflow,
} = OverflowInteropFile.FluentUI.Blazor.Components.Overflow;

initializeOverflow('tabs', 'fluent-tab', 0, 0, 'activeid');
refreshOverflow('tabs');
const managedItemIds = getOverflowItemIds('tabs');
disposeOverflow('tabs');
```

The imperative functions have the following behavior:

| Function | Description |
| --- | --- |
| `initializeOverflow(id, querySelector, threshold, maxRenderedItems, pinnedItemIdAttribute)` | Attaches overflow to the element with `id`. Reinitializing the same ID replaces its attached controller. A missing host is ignored. |
| `refreshOverflow(id)` | Recalculates a connected custom element or attached host synchronously. |
| `getOverflowItemIds(id)` | Recalculates a connected target synchronously, then returns its managed item IDs in DOM order. Returns an empty array when no active target is found. |
| `disposeOverflow(id)` | Disconnects the controller and restores visibility that it owns. A missing target is ignored. |

`maxRenderedItems` values less than or equal to zero are unlimited, and positive
fractional values are floored when the payload is built. The pinned item is read
from the host attribute named by `pinnedItemIdAttribute` and is never hidden by
the controller. When the target is already a `<fluent-overflow>`, initialization
sets its selector, threshold, and payload limit; `pinnedItemIdAttribute` applies
only to attached hosts.

For components that own their lifecycle, use the controller directly. Options
are exposed as properties and can use JavaScript getters when attribute or state
changes must be read at the time of each layout:

```ts
import { Microsoft as OverflowControllerFile } from './FluentOverflowController.js';

const OverflowController =
	OverflowControllerFile.FluentUI.Blazor.Components.Overflow.OverflowController;

const host = document.querySelector<HTMLElement>('#tabs')!;
const controller = new OverflowController({
	host,
	querySelector: 'fluent-tab',
	threshold: 0,
	maxRenderedItems: 25,
	visibleOnLoad: false,
	pinnedItemIdAttribute: 'activeid',
	onVisibilityChanged: () => {
		// Synchronize any host-specific state after item visibility changes.
	},
});

controller.connect();
controller.refresh();
controller.disconnect();
```

The Web Component property is named `maxOverflowItems`; the controller option is
named `maxRenderedItems`. Both limit `items` arrays, while total-count fields
remain unlimited.

`OverflowControllerOptions` supports the following adapters:

| Option | Required | Default or behavior |
| --- | --- | --- |
| `host` | Yes | Owns the managed direct children and receives change events. |
| `querySelector` | Yes | Filters candidate layout children with `Element.matches`; an empty selector manages all candidates. |
| `threshold` | Yes | Reserves non-negative finite space before overflow. |
| `maxRenderedItems` | Yes | Limits public `items` arrays; non-positive and non-finite values are unlimited. |
| `visibleOnLoad` | No | Defaults to `true`. When `false`, the controller hides the host with `visibility: hidden` until the first layout completes, then restores its previous inline visibility. |
| `pinnedItemIdAttribute` | No | Names a host attribute whose current value is the ID of an item that must remain visible. |
| `orientation` | No | Uses the host's `orientation="vertical"` attribute when omitted; otherwise horizontal. |
| `overflowDirection` | No | Defaults to `end`. |
| `layoutContainer` | No | Element whose client width or height constrains layout; defaults to `host`. |
| `gapContainer` | No | Element whose computed column or row gap is measured; defaults to `host`. |
| `layoutChildren` | No | Candidate elements and total-size participants; defaults to the host's direct children. |
| `reservedElement` | No | Element, such as a trigger, whose outer size and one gap are reserved when overflow is active. |
| `setOverflowActive` | No | Called while calculating and after layout so presentation can be enabled only when items overflow. |
| `onLayoutChanged` | No | Called after every completed calculation with the event detail and hidden elements, even when event detail is unchanged. |
| `onVisibilityChanged` | No | Called only when controller-owned visibility changes, and on disconnect when such visibility is restored. |

The controller exposes `connect()`, `requestRefresh()`, `refresh()`,
`disconnect()`, `getManagedItems()`, `getHiddenItems()`, and
`getHiddenOverflowItems(maximum)`. `connect()` schedules the initial calculation
for the next animation frame. `requestRefresh()` coalesces work into one frame;
`refresh()` calculates synchronously and cancels pending work. Both are no-ops
until the controller is connected to a host that is in the document.

`disconnect()` stops observation and restores only visibility owned by the
controller. Getter results reflect the most recently completed calculation.
Changing values supplied through option getters does not itself schedule work;
call `requestRefresh()` or `refresh()` when no observed DOM or size change does
so.

## Styling

The component exposes spacing custom properties and three Shadow Parts. Style the required slotted Fluent elements directly in the light DOM:

```css
fluent-overflow {
	--fluent-overflow-gap: 8px;
}

fluent-overflow::part(container) { /* main flex container */ }
fluent-overflow::part(items) { /* visible item container */ }
fluent-overflow::part(trigger) { /* trigger slot */ }
fluent-overflow > [slot='trigger'] { /* fluent-button */ }
fluent-overflow > [slot='menu'] { /* fluent-tooltip */ }
```

Style child elements and slotted replacements normally in the light DOM. Hidden elements keep their original classes, event listeners, and state.

Overflow requires a constrained size on the active axis. In vertical mode, the
host also needs a constrained block size from its containing layout.

## Accessibility

- The slotted button is keyboard-focusable and its `aria-label` is updated to `<count> hidden items` after each completed layout.
- The tooltip opens on hover or focus and contains text only, with no hidden interactive controls.
- The trigger should remain focusable so its tooltip is keyboard-accessible.
- The host defaults to `role="group"`; provide an `aria-label` or `aria-labelledby` that describes the group.

## Dynamic content

Direct children can be added, removed, resized, or changed at runtime. `MutationObserver` refreshes the managed set and `ResizeObserver` recalculates the allocation. Property and observed attribute changes schedule the same calculation automatically.
