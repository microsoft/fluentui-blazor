import Sortable from 'sortablejs';

export namespace Microsoft.FluentUI.Blazor.Components.SortableList {
  export function init(
    list: HTMLElement,
    group: string,
    pull: any,
    put: boolean,
    sort: boolean,
    handle: string | null,
    filter: string | null,
    fallback: boolean,
    component: any
  ): any {
    const controller = new AbortController();
    const { signal } = controller;
    let grabMode: boolean = false;

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
          //announce(
          //  grabMode
          //    ? `Grabbed ${item.textContent!.trim()}. Use arrow keys to move.`
          //    : `Dropped ${item.textContent!.trim()}.`
          //);
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
