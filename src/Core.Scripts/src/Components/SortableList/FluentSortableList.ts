import type Sortable from 'sortablejs';
import { SortableJSUrl } from '../../ExternalLibs';

declare global {
  interface Window {
    Sortable?: typeof Sortable;
  }
}

let loadPromise: Promise<typeof Sortable> | null = null;

/**
 * Dynamically loads SortableJS from CDN if not already loaded
 */
async function ensureSortableJSLoaded(): Promise<typeof Sortable> {
  // Check if SortableJS is already available
  if (window.Sortable) {
    return window.Sortable;
  }

  if (loadPromise) {
    return loadPromise;
  }

  // Load SortableJS from CDN
  loadPromise = new Promise((resolve, reject) => {
    const script = document.createElement('script');
    script.src = SortableJSUrl;
    script.onload = () => {
      if (window.Sortable) {
        resolve(window.Sortable);
      } else {
        reject(new Error('SortableJS library failed to load'));
      }
    };
    script.onerror = () => {
      loadPromise = null;
      reject(new Error('Failed to load SortableJS from CDN'));
    };
    document.head.appendChild(script);
  });

  return loadPromise;
}

export namespace Microsoft.FluentUI.Blazor.Components.SortableList {
  export async function init(
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

    await ensureSortableJSLoaded();

    const controller = new AbortController();
    const { signal } = controller;
    let grabMode: boolean = false;

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
      const item = document.activeElement;
      if (item == null) {
        return;
      }

      if (!item.classList.contains('sortable-item')) return;

      switch (event.key) {
        case 'Enter':
        case ' ':
          grabMode = !grabMode;
          item.setAttribute('aria-grabbed', grabMode.toString());
          event.preventDefault();
          break;

        case 'ArrowUp':
          if (grabMode && item.previousElementSibling) {
            item.parentNode!.insertBefore(item, item.previousElementSibling);
            (item as HTMLElement).focus();
          } else if (item.previousElementSibling) {
            (item.previousElementSibling as HTMLElement).focus();
          }
          event.preventDefault();
          break;

        case 'ArrowDown':
          if (grabMode && item.nextElementSibling) {
            item.parentNode!.insertBefore(item.nextElementSibling, item);
            (item as HTMLElement).focus();
          } else if (item.nextElementSibling) {
            (item.nextElementSibling as HTMLElement).focus();
          }
          event.preventDefault();
          break;

        case 'ArrowLeft':
        case 'ArrowRight':
          if (grabMode && group) {
            const allLists = Array.from(document.querySelectorAll(`[data-sortable-group="${group}"]`));
            const currentIndex = allLists.indexOf(list);
            let nextIndex = currentIndex;

            if (event.key === 'ArrowRight') {
              nextIndex = (currentIndex + 1) % allLists.length;
            } else {
              nextIndex = (currentIndex - 1 + allLists.length) % allLists.length;
            }

            if (nextIndex !== currentIndex) {
              const targetList = allLists[nextIndex] as HTMLElement;
              const targetListId = targetList.id;
              const oldIndex = Array.from(item.parentNode!.children).indexOf(item);
              const newIndex = 0;

              sortable.options.onRemove!({
                item: item,
                from: list,
                to: targetList,
                oldIndex: oldIndex,
                newIndex: newIndex,
                oldDraggableIndex: oldIndex,
                newDraggableIndex: newIndex
              } as any);

              item.setAttribute('aria-grabbed', 'false');
              grabMode = false;

              setTimeout(() => {
                const newList = document.getElementById(targetListId);
                const movedItem = newList?.children[newIndex] as HTMLElement;
                movedItem?.focus();
              }, 50);
            }
          }
          event.preventDefault();
          break;

        case 'Tab':
          if (item.getAttribute('aria-grabbed') === 'true') {
            item.setAttribute('aria-grabbed', 'false');
            grabMode = false;
          }
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
