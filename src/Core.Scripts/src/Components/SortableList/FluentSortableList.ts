import type Sortable from 'sortablejs';
import { ExternalLibraryLoader, Library } from '../../ExternalLibs';

const SortableLibrary: Library = {
  name: 'Sortable',
  url: 'https://unpkg.com/sortablejs@1.15.6/Sortable.min.js',
  debugUrl: 'https://unpkg.com/sortablejs@1.15.6/Sortable.js'
};

const sortableLoader = new ExternalLibraryLoader<typeof Sortable>(SortableLibrary);

export namespace Microsoft.FluentUI.Blazor.Components.SortableList {
  export async function Initialize(
    list: HTMLElement,
    group: string,
    pull: any,
    put: boolean,
    sort: boolean,
    handle: string | null,
    filter: string | null,
    fallback: boolean,
    component: any
  ): Promise<any> {

    const Sortable = await sortableLoader.load();

    const controller = new AbortController();
    const { signal } = controller;

    if (group) {
      list.setAttribute('data-sortable-group', group);
    }

    const sortable = new Sortable(list, {
      animation: 200,
      group: {
        name: group,
        pull: pull || true,
        put: put
      },
      filter: filter || undefined,
      sort: sort,
      forceFallback: fallback || false,
      handle: handle || undefined,
      onChoose: (event: any) => {
        event.item.setAttribute('aria-grabbed', 'true');
      },
      onUnchoose: (event: any) => {
        event.item.setAttribute('aria-grabbed', 'false');
      },
      onUpdate: (event: any) => {
        // Revert the DOM to match the .NET state
        event.item.remove();
        event.to.insertBefore(event.item, event.to.childNodes[event.oldIndex]);

        // Notify .NET to update its model and re-render
        component.invokeMethodAsync('OnUpdateJS', event.oldDraggableIndex, event.newDraggableIndex, event.from.id, event.to.id);
      },
      onRemove: (event: any) => {
        if (event.pullMode === 'clone') {
          // Remove the clone
          event.clone.remove();
        }

        event.item.remove();
        event.from.insertBefore(event.item, event.from.childNodes[event.oldIndex]);

        // Notify .NET to update its model and re-render
        component.invokeMethodAsync('OnRemoveJS', event.oldDraggableIndex, event.newDraggableIndex, event.from.id, event.to.id);
      }
    });

    list.addEventListener('keydown', (event: KeyboardEvent) => {
      const item = document.activeElement as HTMLElement;
      if (item == null || !item.classList.contains('sortable-item')) {
        return;
      }

      const isGrabbed = item.getAttribute('aria-grabbed') === 'true';

      switch (event.key) {
        case 'Enter':
        case ' ':
          if (typeof (sortable.options.filter) === 'string' && !item.classList.contains(sortable.options.filter.slice(1)) || !sortable.options.filter) {
            item.setAttribute('aria-grabbed', (!isGrabbed).toString());
          }
          event.preventDefault();
          break;

        case 'ArrowUp':
          if (item.previousElementSibling) {
            if (isGrabbed) {
              if (!sortable.options.sort) {
                item.setAttribute('aria-grabbed', 'false');
                event.preventDefault();
                break;
              }
              const oldIndex = Array.from(item.parentNode!.children).indexOf(item);
              const newIndex = oldIndex - 1;

              item.parentNode!.insertBefore(item, item.previousElementSibling);

              const updateEvent = new CustomEvent('update') as any;
              updateEvent.item = item;
              updateEvent.from = list;
              updateEvent.to = list;
              updateEvent.oldIndex = oldIndex;
              updateEvent.newIndex = newIndex;
              updateEvent.oldDraggableIndex = oldIndex;
              updateEvent.newDraggableIndex = newIndex;

              sortable.options.onUpdate?.(updateEvent);

              setTimeout(() => {
                const refreshedList = document.getElementById(list.id);
                const movedItem = refreshedList?.children[newIndex] as HTMLElement;
                if (movedItem) {
                  movedItem.focus();
                  movedItem.setAttribute('aria-grabbed', 'true');
                }
                const oldPositionItem = refreshedList?.children[oldIndex] as HTMLElement;
                if (oldPositionItem) {
                  oldPositionItem.setAttribute('aria-grabbed', 'false');
                }
              }, 50);
            } else {
              (item.previousElementSibling as HTMLElement).focus();
            }
          }
          event.preventDefault();
          break;

        case 'ArrowDown':
          if (item.nextElementSibling) {
            if (isGrabbed) {
              if (!sortable.options.sort) {
                item.setAttribute('aria-grabbed', 'false');
                event.preventDefault();
                break;
              }
              const oldIndex = Array.from(item.parentNode!.children).indexOf(item);
              const newIndex = oldIndex + 1;

              item.parentNode!.insertBefore(item.nextElementSibling, item);

              const updateEvent = new CustomEvent('update') as any;
              updateEvent.item = item;
              updateEvent.from = list;
              updateEvent.to = list;
              updateEvent.oldIndex = oldIndex;
              updateEvent.newIndex = newIndex;
              updateEvent.oldDraggableIndex = oldIndex;
              updateEvent.newDraggableIndex = newIndex;

              sortable.options.onUpdate?.(updateEvent);

              setTimeout(() => {
                const refreshedList = document.getElementById(list.id);
                const movedItem = refreshedList?.children[newIndex] as HTMLElement;
                if (movedItem) {
                  movedItem.focus();
                  movedItem.setAttribute('aria-grabbed', 'true');
                }
                const oldPositionItem = refreshedList?.children[oldIndex] as HTMLElement;
                if (oldPositionItem) {
                  oldPositionItem.setAttribute('aria-grabbed', 'false');
                }
              }, 50);
            } else {
              (item.nextElementSibling as HTMLElement).focus();
            }
          }
          event.preventDefault();
          break;

        case 'ArrowLeft':
        case 'ArrowRight':
          if (group) {

            const allLists = Array.from(document.querySelectorAll(`[data-sortable-group="${group}"]`));
            const currentIndex = allLists.indexOf(list);
            let nextIndex = currentIndex;

            if (event.key === 'ArrowRight') {
              nextIndex = (currentIndex + 1) % allLists.length;
            } else {
              nextIndex = (currentIndex - 1 + allLists.length) % allLists.length;
            }

            if (nextIndex !== currentIndex) {
              if (isGrabbed) {

                const targetList = allLists[nextIndex] as HTMLElement;
                const targetListId = targetList.id;
                const oldIndex = Array.from(item.parentNode!.children).indexOf(item);
                const newIndex = 0;

                const pullMode = pull || true

                if (pullMode === false) {
                  break;
                }

                // Move in DOM immediately for feedback
                targetList.insertBefore(item, targetList.firstChild);

                // Notify via sortable.onRemove
                const removeEvent = new CustomEvent('remove') as any;
                removeEvent.item = item;
                removeEvent.from = list;
                removeEvent.to = targetList;
                removeEvent.oldIndex = oldIndex;
                removeEvent.newIndex = newIndex;
                removeEvent.oldDraggableIndex = oldIndex;
                removeEvent.newDraggableIndex = newIndex;
                removeEvent.pullMode = pullMode;
                if (pullMode === 'clone') {
                  removeEvent.clone = item.cloneNode(true);
                }

                sortable.options.onRemove?.(removeEvent);

                setTimeout(() => {
                  const refreshedTargetList = document.getElementById(targetListId);
                  const movedItem = refreshedTargetList?.children[newIndex] as HTMLElement;
                  if (movedItem) {
                    movedItem.focus();
                    movedItem.setAttribute('aria-grabbed', 'false');
                  }
                  const refreshedSourceList = document.getElementById(list.id);
                  const oldPositionItem = refreshedSourceList?.children[oldIndex] as HTMLElement;
                  if (oldPositionItem) {
                    oldPositionItem.setAttribute('aria-grabbed', 'false');
                  }
                }, 50);
              }
              else {
                const targetList = allLists[nextIndex] as HTMLElement;
                const itemIndex = Array.from(item.parentNode!.children).indexOf(item);
                let targetItem = targetList.children[itemIndex] as HTMLElement;

                if (!targetItem && targetList.children.length > 0) {
                  const firstDist = Math.abs(itemIndex - 0);
                  const lastDist = Math.abs(itemIndex - (targetList.children.length - 1));
                  targetItem = (firstDist < lastDist ? targetList.firstElementChild : targetList.lastElementChild) as HTMLElement;
                }

                if (targetItem) {
                  targetItem.focus();
                }
              }
            }
          }
          event.preventDefault();
          break;

        case 'Tab':
          if (isGrabbed) {
            item.setAttribute('aria-grabbed', 'false');
          }
          break;
      }
    }, { signal });

    return {
      stop: () => {
        sortable.destroy();
        controller.abort();
      }
    };
  }
}
