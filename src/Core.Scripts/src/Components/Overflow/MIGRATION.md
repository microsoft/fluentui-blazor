# Migrating `fluent-overflow`

## Comparison scope

This report compares the previous `fluent-overflow` and
`AttachedOverflowController` APIs with the `fluent-overflow` and
`OverflowController` APIs in this project's `Overflow` folder.

The tables follow the **previous API -> new API** direction. Unchanged APIs and
features that are only new are not listed.

## `fluent-overflow` Web Component

### Attributes and properties

| Previous API | New API | Change and migration impact |
| --- | --- | --- |
| `max-rendered-items` | `max-overflow-items` / `maxOverflowItems` property | The attribute has been renamed. Its default value changes from `25` to unlimited. A non-numeric value previously used `25`; it now removes the limit. Values less than or equal to zero remain unlimited. |
| `selectors` | Removed | The plural alias is no longer read. Replace `selectors="..."` with `selector="..."`. |
| No `selector` | No `selector` | The previous default selection excluded `.fluent-overflow-more` children. All eligible direct children are now managed; elements assigned to customization slots are excluded. A `.fluent-overflow-more` class alone is therefore no longer enough to exclude a child. |
| `threshold`, default value `25` | `threshold`, default value `0` | Without the attribute, the space reserved before overflow changes from 25 px to 0 px. An invalid value previously used 25; it now becomes 0. A negative value, which was previously preserved, is now normalized to 0. |
| `store-overflow-in-memory` | Removed | The previous `OverflowState` can no longer be retained through an attribute. Remove this attribute from the markup. |

### Managed child attributes

| Previous API | New API | Change and migration impact |
| --- | --- | --- |
| `overflow` | `hidden` | The component no longer sets `overflow` on overflowed elements. CSS selectors, tests, and integrations that use `[overflow]` must use the `hidden` state. On disconnection, only visibility owned by the controller is restored. |
| `behavior` | `behavior="fixed"` or `behavior="ellipsis"` | Previously, any value other than `ellipsis` made the element fixed. Now only `fixed` does so. A custom or invalid value makes the element eligible for overflow. |

### Methods

| Previous API | New API | Change and migration impact |
| --- | --- | --- |
| `getOverflowState(): OverflowState` | Removed | The component no longer returns the complete `{ overflowItems, overflowCount, firstOverflowIndex, orderedItemIds }` object. Consumers must receive overflow state through events. There is no longer an equivalent imperative read for `firstOverflowIndex` or the previous complete `overflowItems` array. |

The `refresh()` method keeps its name and signature, so it is not an API
breaking change.

### `overflowchange` and `fluentoverflowchange` events

Both event names are preserved, as is `detail.overflowCount`, but the shape of
`detail` has changed:

```ts
// Previous detail
interface OverflowChangeDetail {
  items: Array<{
    Id: string;
    Overflow: boolean;
    Text: string;
    Behavior?: string | null;
    Index?: number;
  }>;
  overflowCount: number;
  firstOverflowIndex: number;
  orderedItemIds: string[];
}

// New detail
interface OverflowChangeDetail {
  items: Array<{
    id: string;
    text: string;
    index: number;
  }>;
  overflowCount: number;
}
```

Apply the following changes to event handlers:

| Previous field | New field | Status |
| --- | --- | --- |
| `items[].Id` | `items[].id` | Renamed and changed from PascalCase to camelCase. |
| `items[].Text` | `items[].text` | Renamed and changed from PascalCase to camelCase. |
| `items[].Index` | `items[].index` | Renamed and now required. |
| `items[].Overflow` | None | Removed. Every entry in `items` already represents an overflowed element. |
| `items[].Behavior` | None | Removed from the payload. |
| `firstOverflowIndex` | None | Removed from the detail. |
| `orderedItemIds` | None | Removed from the detail. |

The `items` limit is now controlled by `max-overflow-items` instead of
`max-rendered-items`. Because the default limit is now unlimited, a handler
that relied on the implicit maximum of 25 elements must explicitly set the new
value.

Event emission is also narrower: the previous version compared
`orderedItemIds`, among other fields. Reordering only visible elements could
therefore emit an event; it no longer does so if the new
`{ items, overflowCount }` detail remains unchanged.

## `OverflowController` class

The `AttachedOverflowController` class is renamed to `OverflowController`.

### Constructor and configuration

The positional constructor has been replaced with a single options object.

```ts
// Previous construction
new AttachedOverflowController(
  refreshContainer,
  host,
  querySelector,
  threshold,
  maxRenderedItems,
  pinnedItemIdAttribute,
  synchronizeHidden,
  notifyHostItemsChanged,
);

// Equivalent new construction for options that are still supported
new OverflowController({
  host,
  querySelector,
  threshold,
  maxRenderedItems,
  pinnedItemIdAttribute,
  onVisibilityChanged: notifyHostItemsChanged ? () => synchronizeHostItems() : undefined,
});
```

| Previous parameter | New configuration | Status and impact |
| --- | --- | --- |
| `refreshContainer` | None | Removed. Consumers can no longer inject the calculation algorithm. |
| `host` | `options.host` | Moved into the options object. The specialized public `OverflowHostElement` type is no longer required. |
| `querySelector` | `options.querySelector` | Moved into the options object. |
| `threshold` | `options.threshold` | Moved into the options object. Non-finite or negative values are now normalized to 0. |
| `maxRenderedItems` | `options.maxRenderedItems` | Moved into the options object. A non-finite value is now unlimited instead of producing the previous numeric comparison behavior. |
| `pinnedItemIdAttribute` | `options.pinnedItemIdAttribute` | Moved into the options object and is now optional. |
| `synchronizeHidden` | None | Removed. The new controller directly synchronizes overflow with `HTMLElement.hidden`; it no longer exposes a mode that only preserves the `overflow` attribute. |
| `notifyHostItemsChanged` | `options.onVisibilityChanged` | The Boolean and implicit call to `host.tabsChanged()` are replaced by an explicit callback. Consumers must provide this callback when they need to synchronize their host component. |

### Methods

| Previous method | New API | Status and impact |
| --- | --- | --- |
| `update(querySelector, threshold, maxRenderedItems, pinnedItemIdAttribute, synchronizeHidden, notifyHostItemsChanged)` | Removed | Positional option updates are no longer available. For dynamic values, options can be provided through getters, after which a recalculation must be requested. Otherwise, disconnect and recreate the controller. |
| `getOverflowState(): OverflowState` | Removed | The class no longer provides an `OverflowState` snapshot. `firstOverflowIndex` and `orderedItemIds` are no longer exposed by the controller. |

`connect()`, `refresh()`, and `disconnect()` keep their names and signatures, so
they are not listed as changes.

### Events emitted by the controller

The controller continues to emit `overflowchange` and `fluentoverflowchange`
on the host. Their detail has exactly the same changes as the Web Component's
detail: item fields use camelCase, and `Overflow`, `Behavior`,
`firstOverflowIndex`, and `orderedItemIds` are removed.

### Removed or modified public types

| Previous type | Status |
| --- | --- |
| `OverflowItem` | Modified: `Id`, `Text`, and `Index` become `id`, `text`, and `index`; `Overflow` and `Behavior` are removed. |
| `OverflowState` | Removed. |
| `RefreshResult` | Removed from the controller's public API. |
| `RefreshContainer` | Removed along with calculation algorithm injection through the constructor. |
| `OverflowElement` | Removed from the public API. |
| `OverflowHostElement` | Removed; the host is now an `HTMLElement`, and the specific `tabsChanged()` notification is provided through a callback. |

## Related imperative API

The exported functions around the controller have been renamed and their
contracts have changed:

| Previous function | New function | Change and migration impact |
| --- | --- | --- |
| `Initialize(id, querySelector, threshold, maxRenderedItems, pinnedItemIdAttribute, synchronizeHidden, notifyHostItemsChanged)` | `initializeOverflow(id, querySelector, threshold, maxRenderedItems, pinnedItemIdAttribute)` | Changed to camelCase and the last two parameters were removed. Synchronization through `hidden` is now systematic; the specific `tabsChanged()` notification can no longer be configured through this function. |
| `Dispose(id)` | `disposeOverflow(id)` | Renamed to camelCase. |
| `Refresh(id)` | `refreshOverflow(id)` | Renamed to camelCase. |
| `GetOverflowState(id): OverflowState` | Removed | The complete snapshot can no longer be read imperatively. The available imperative read API only returns managed identifiers as a `string[]`; it therefore does not replace the removed `OverflowState` fields. |

## Migration checklist

1. Replace `max-rendered-items` with `max-overflow-items`, and explicitly set
   it to `25` if the previous implicit limit must be preserved.
2. Replace `selectors` with `selector`, then check children that were previously
   excluded only through `.fluent-overflow-more`.
3. Remove `store-overflow-in-memory` and migrate its application-level uses.
4. Replace selectors and tests based on `[overflow]` with the `hidden` state.
5. Verify that `behavior` values are only `fixed` or `ellipsis`.
6. Update event payloads to camelCase and remove dependencies on `Overflow`,
   `Behavior`, `firstOverflowIndex`, and `orderedItemIds`.
7. Replace the controller's positional constructor with the options object, and
   migrate `notifyHostItemsChanged` to an explicit callback.
8. Remove calls to `update()` and `getOverflowState()`.
9. Rename imperative API calls to camelCase and stop expecting an
   `OverflowState` from the imperative API.