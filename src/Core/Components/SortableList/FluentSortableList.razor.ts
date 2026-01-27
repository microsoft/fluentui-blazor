import { DotNet } from "../../../Core.Scripts/src/d-ts/Microsoft.JSInterop";

declare const Sortable: any;

export namespace Microsoft.FluentUI.Blazor.SortableList {
    export function init(
        list: HTMLElement,
        group: string,
        pull: string | null,
        put: boolean,
        sort: boolean,
        handle: string | null,
        filter: string | null,
        fallback: boolean,
        component: DotNet.DotNetObject
    ): void {
        new Sortable(list, {
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
    }
}
