---
title: Drag and Drop
route: /Drag
icon: Drag
---

# Drag and Drop

A Blazor component implementation of a <a href="https://developer.mozilla.org/en-US/docs/Web/API/HTML_Drag_and_Drop_API" target="_blank">HTML Drag and Drop API</a>.

The user may select draggable elements with a mouse, drag those elements to a droppable element, and drop them by releasing the mouse button. A translucent representation of the draggable elements follows the pointer during the drag operation.

Create a `FluentDragContainer` surface that will contain drag-and-drop areas. Customize these areas with the `Draggable` and `Droppable` parameters. Each time an item is dragged or dropped onto an area, an event is triggered to notify the developer. You can retrieve a generic `Item` argument with your personal data.

## Basic example

{{ DragDropBasic }}

## Nested drag & drop
This example demonstrates how to nest multiple `FluentDragContainer` components to enable drag-and-drop interactions across hierarchical structures such as rows, columns, and elements.

The key to making this multi-level drag-and-drop system work is the use of the `StopPropagation` property. By enabling it where appropriate, each nested `FluentDragContainer` can handle drag events independently-preventing unintended interference from parent containers.

Each level supports independent drag behavior:

* **Rows** can be reordered vertically.
* **Columns** can be moved within the same row or between different rows.
* **Elements** can be rearranged inside a column or moved across columns.

The <span style="background-color: yellow; padding: 0 0.2em; color: #000;">yellow area</span> indicates an empty drop zone inside a row. It accepts columns when a row does not contain any yet. Similarly, empty columns behave as drop zones for elements.

This structure allows fully flexible layout editing with deep nesting and drag-and-drop handling at every level.

{{ DragDropNested }}

## Between containers

By default, a `FluentDragContainer` only handles the zones placed inside it. To move items from one container to another,
create a `FluentDragGroup<TItem>` instance and assign it to the `Group` parameter of every container that should take part.
All containers of a group must use the same `TItem` type.

```razor
<FluentDragContainer TItem="string" Group="_group" OnDropEnd="OnDropEnd"> ... </FluentDragContainer>
<FluentDragContainer TItem="string" Group="_group" OnDropEnd="OnDropEnd"> ... </FluentDragContainer>

@code {
    private readonly FluentDragGroup<string> _group = new();
}
```

* The `Source` of the event arguments can belong to another container than the `Target`. Use `OnDropEnd` to remove the item
  from its original list and add it to the target list. The `OnDropEnd` event of the container that holds the target zone is raised.
* The `Id` of each `FluentDropZone` must be unique across all containers of the group.
* Dropping on an item zone lets you insert the item before it. To also accept drops on an empty list or on the free space,
  make the list itself a droppable `FluentDropZone` and set `StopPropagation="true"` on the item zones.
* Touch devices are supported the same way as inside a single container: press and hold an item, then move it over a zone
  of any container in the group.
* A drag that is cancelled (Escape key, drop outside a target or touch cancel) clears the shared state of the group.
* Without a `Group`, containers behave as before and are independent of each other.

{{ DragDropBetweenContainers }}

## API FluentDragContainer

{{ API Type=FluentDragContainer<int> }}

## API FluentDropZone

{{ API Type=FluentDropZone<int> }}

## API FluentDragEventArgs

{{ API Type=FluentDragEventArgs<int> }}

## Migrating

{{ INCLUDE File=MigrationDrag }}
