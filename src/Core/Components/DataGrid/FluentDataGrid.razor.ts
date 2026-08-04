export namespace Microsoft.FluentUI.Blazor.DataGrid {

  const headerUiSelector = '[col-header-ui]';

  const closeHeaderUi = (gridElement: HTMLElement) => {
    gridElement.dispatchEvent(new CustomEvent('closecolumnheaderui', { bubbles: true }));
  };

  const hideHeaderUi = (gridElement: HTMLElement) => {
    const element = gridElement.querySelector(headerUiSelector) as HTMLElement | null;
    if (element) {
      element.style.visibility = 'hidden';
    }
  };

  const getDeepActiveElement = (): HTMLElement | null => {
    let activeElement: Element | null = document.activeElement;
    while (activeElement instanceof HTMLElement && activeElement.shadowRoot?.activeElement) {
      activeElement = activeElement.shadowRoot.activeElement;
    }

    return activeElement instanceof HTMLElement ? activeElement : null;
  };

  const getFocusedGridElement = (gridElement: HTMLElement, event: KeyboardEvent): HTMLElement | null => {
    const composedPath = event.composedPath();

    for (const entry of composedPath) {
      if (!(entry instanceof HTMLElement)) {
        continue;
      }

      const tableCell = entry.closest('td,th');
      if (tableCell instanceof HTMLElement && gridElement.contains(tableCell)) {
        return tableCell;
      }

      if (entry === gridElement) {
        return gridElement;
      }
    }

    const activeElement = getDeepActiveElement();
    if (activeElement) {
      const tableCell = activeElement.closest('td,th');
      if (tableCell instanceof HTMLElement && gridElement.contains(tableCell)) {
        return tableCell;
      }

      const table = activeElement.closest('table');
      if (table instanceof HTMLElement && table === gridElement) {
        return table;
      }
    }

    return null;
  };

  const handledArrowNavigationEventFlag = '__fluentDataGridArrowNavigationHandled';

  interface Grid {
    id: string;
    columns: Column[]; // or a more specific type if you have one
    initialWidths: string;
    isResizing?: boolean;
    resizeController?: AbortController;
    reorderController?: AbortController;
  }

  interface Column {
    header: Element;
    size: string;
  }

  // Use a dictionary for grids for id-based access
  let grids: Grid[] = []; // { [id: string]: Grid } = {};

  export function Initialize(gridElement: HTMLElement, autoFocus: boolean) {
    if (gridElement === undefined || gridElement === null) {
      return;
    }

    const controller = new AbortController();
    const { signal } = controller;

    EnableColumnResizing(gridElement, true, signal);

    // Recalculate sticky offsets now that the DOM is fully rendered, so any
    // browser-computed widths (e.g. after grid layout) are reflected.
    UpdatePinnedColumnOffsets(gridElement);

    let start = gridElement.querySelector('td:first-child') as HTMLElement | null;

    if (autoFocus && start) {
      start.focus();
    }

    const tryCloseHeaderUi = (event?: Event) => {
      const element = gridElement?.querySelector(headerUiSelector);
      if (!element) {
        return false;
      }

      if (event && event.composedPath().indexOf(element) >= 0) {
        return false;
      }

      closeHeaderUi(gridElement);
      return true;
    };

    const bodyClickHandler = (event: MouseEvent) => {
      tryCloseHeaderUi(event);
    };

    const bodyKeyDownHandler = (event: KeyboardEvent) => {
      if (event.key === "Escape") {
        const closedHeaderUi = tryCloseHeaderUi();
        if (closedHeaderUi) {
          gridElement.focus();
        }
      }
    };
    const keyboardNavigation = (sibling: HTMLElement | null) => {
      if (sibling !== null) {
        // th elements are not focusable; transfer focus to the inner header button instead.
        if (sibling.matches('th')) {
          const headerButton = sibling.querySelector<HTMLElement>('[col-sort-button], [col-options-button]');
          if (headerButton) {
            sibling = headerButton;
          }
        }

        const focusSibling = () => {
          sibling?.focus({ preventScroll: true });
          start = sibling;
        };

        // Defer focusing until after the current key event completes so focus traps
        // or inner component key handlers cannot immediately override DataGrid focus.
        setTimeout(() => {
          focusSibling();

          // Some host components can move focus again on the next frame.
          // Re-apply focus once when focus escaped the current grid.
          requestAnimationFrame(() => {
            const activeElement = getDeepActiveElement();
            if (!activeElement || !gridElement.contains(activeElement)) {
              focusSibling();
            }
          });
        }, 0);
      }
    }
    const getHeaderButtons = () => {
      return Array.from(
        gridElement.querySelectorAll<HTMLElement>("th[cell-type='columnheader'] [col-sort-button], th[cell-type='columnheader'] [col-options-button]")
      ).filter(button => button.tabIndex >= 0 && !button.hasAttribute('disabled'));
    };
    const moveHeaderFocus = (current: HTMLElement, shiftKey: boolean) => {
      const headerButtons = getHeaderButtons();
      const currentIndex = headerButtons.indexOf(current);
      if (currentIndex < 0) {
        return false;
      }

      const nextIndex = shiftKey ? currentIndex - 1 : currentIndex + 1;
      if (nextIndex < 0 || nextIndex >= headerButtons.length) {
        return false;
      }

      headerButtons[nextIndex].focus({ preventScroll: true });
      return true;
    };
    const moveFocusIntoHeader = () => {
      const headerButtons = getHeaderButtons();
      const firstHeaderButton = headerButtons[0];
      if (!firstHeaderButton) {
        return false;
      }

      firstHeaderButton.focus({ preventScroll: true });
      return true;
    };
    const getAdjacentRowCell = (cell: HTMLTableCellElement, direction: 'up' | 'down') => {
      const row = cell.parentElement as HTMLTableRowElement | null;
      if (!row) {
        return null;
      }

      const rowGroupName = row.parentElement?.tagName.toLowerCase();
      let targetRow = direction === 'up'
        ? row.previousElementSibling as HTMLTableRowElement | null
        : row.nextElementSibling as HTMLTableRowElement | null;

      if (!targetRow) {
        const table = row.closest('table');
        if (direction === 'down' && rowGroupName === 'thead') {
          targetRow = table?.querySelector('tbody tr') as HTMLTableRowElement | null;
        } else if (direction === 'up' && rowGroupName === 'tbody') {
          targetRow = table?.querySelector('thead tr:last-child') as HTMLTableRowElement | null;
        }
      }

      if (!targetRow) {
        return null;
      }

      return targetRow.cells[cell.cellIndex] as HTMLTableCellElement | null;
    };
    const keyDownHandler = (event: KeyboardEvent) => {
      if ((event as any)[handledArrowNavigationEventFlag]) {
        return;
      }

      const isArrowKey = event.key === "ArrowRight" || event.key === "ArrowLeft" || event.key === "ArrowDown" || event.key === "ArrowUp";
      const targetElement = event.target instanceof HTMLElement ? event.target : null;
      const isMenuInteraction = event.composedPath().some((entry) =>
        entry instanceof HTMLElement &&
        (entry.matches('fluent-menu, fluent-menu-list, fluent-menu-item, [role="menu"], [role="menuitem"]') ||
          !!entry.closest('fluent-menu, fluent-menu-list, fluent-menu-item, [role="menu"], [role="menuitem"]'))
      );

      if (isArrowKey && isMenuInteraction) {
        return;
      }

      if (event.key === "Tab" && !event.shiftKey) {
        const activeElement = getDeepActiveElement();
        if (activeElement && !activeElement.matches('[col-sort-button], [col-options-button]')) {
          const focusableElements = Array.from(
            document.querySelectorAll<HTMLElement>('button, [href], input, select, textarea, [tabindex]:not([tabindex=\"-1\"])')
          ).filter(element => element.tabIndex >= 0 && !element.hasAttribute('disabled') && element.getClientRects().length > 0);
          const currentIndex = focusableElements.indexOf(activeElement);
          if (currentIndex >= 0) {
            const nextFocusable = focusableElements[currentIndex + 1] ?? null;
            if (nextFocusable && getHeaderButtons().includes(nextFocusable) && moveFocusIntoHeader()) {
              event.preventDefault();
              event.stopPropagation();
              return;
            }
          }
        }
      }

      if (event.key === "Tab" && targetElement && (targetElement.matches('[col-sort-button]') || targetElement.matches('[col-options-button]'))) {
        if (moveHeaderFocus(targetElement, event.shiftKey)) {
          event.preventDefault();
          event.stopPropagation();
          return;
        }
      }

      const headerUiElement = gridElement?.querySelector(headerUiSelector);
      if (headerUiElement && headerUiElement.contains(event.target as HTMLElement)) {
        if (isArrowKey) {
          event.stopPropagation();
          return;
        }
      }

      if (!isArrowKey) {
        return;
      }

      const focusedGridElement = getFocusedGridElement(gridElement, event);
      if (!(focusedGridElement instanceof HTMLTableCellElement)) {
        return;
      }

      if (targetElement && targetElement !== focusedGridElement && targetElement.closest('[role="gridcell"]') === focusedGridElement) {
        return;
      }

      if (start !== focusedGridElement) {
        start = focusedGridElement;
      }

      if (start !== null && (gridElement.contains(start) || gridElement === start)) {
        (event as any)[handledArrowNavigationEventFlag] = true;
        const isRTL = getComputedStyle(gridElement).direction === 'rtl';

        if (event.key === "ArrowUp") {
          event.preventDefault();
          const previousSibling = getAdjacentRowCell(start as HTMLTableCellElement, 'up');
          keyboardNavigation(previousSibling);
          event.stopPropagation();
        } else if (event.key === "ArrowDown") {
          event.preventDefault();
          const nextSibling = getAdjacentRowCell(start as HTMLTableCellElement, 'down');
          keyboardNavigation(nextSibling);
          event.stopPropagation();
        } else if (event.key === "ArrowLeft") {
          // left arrow
          event.preventDefault();
          const previousSibling = isRTL ? start.nextElementSibling as HTMLElement : start.previousElementSibling as HTMLElement;
          keyboardNavigation(previousSibling);
          event.stopPropagation();
        } else if (event.key === "ArrowRight") {
          // right arrow
          event.preventDefault();
          const nextsibling = isRTL ? start.previousElementSibling as HTMLElement : start.nextElementSibling as HTMLElement;
          keyboardNavigation(nextsibling);
          event.stopPropagation();
        }
      }
    };

    const cells = gridElement.querySelectorAll('[role="gridcell"]');
    cells.forEach((cell: any) => {
      cell.columnDefinition = {
        columnDataKey: "",
        cellInternalFocusQueue: true,
        cellFocusTargetCallback: (cell: HTMLElement) => {
          return cell.children[0];
        }
      }
      cell.addEventListener(
        "keydown",
        (event: KeyboardEvent) => {
          if ((event.target as HTMLElement).role !== "gridcell" && (event.key === "ArrowRight" || event.key === "ArrowLeft")) {
            event.stopPropagation();
          }
        },
        { signal }
      );
    });

    document.body.addEventListener('click', bodyClickHandler, { signal });
    document.body.addEventListener('mousedown', bodyClickHandler, { signal });
    document.body.addEventListener('keydown', bodyKeyDownHandler, { signal });

    gridElement.addEventListener('keydown', keyDownHandler, { signal, capture: true });

    return {
      stop: () => {
        controller.abort();
        const grid = grids.find(g => g.id === gridElement.id);
        if (grid?.resizeController) {
          grid.resizeController.abort();
        }
        if (grid?.reorderController) {
          grid.reorderController.abort();
        }
        grids = grids.filter(grid => grid.id !== gridElement.id);
      }
    };
  }

  export function CheckColumnPopupPosition(gridElement: HTMLElement) {
    const colPopup = gridElement.querySelector(headerUiSelector) as HTMLElement | null;
    if (colPopup) {
      const gridRect = gridElement.getBoundingClientRect();
      const popupRect = colPopup.getBoundingClientRect();
      const leftOverhang = Math.max(0, gridRect.left - popupRect.left);
      const rightOverhang = Math.max(0, popupRect.right - gridRect.right);
      if (leftOverhang || rightOverhang) {
        const applyOffset = leftOverhang && rightOverhang ? (leftOverhang - rightOverhang) / 2 : (leftOverhang - rightOverhang);
        colPopup.style.transform = `translateX(${applyOffset}px)`;
      }

      colPopup.style.visibility = 'visible';
      (colPopup as any).scrollIntoViewIfNeeded?.();

      requestAnimationFrame(() => {
        const autoFocusElem = colPopup.querySelector<HTMLElement>('[autofocus]');
        if (autoFocusElem) {
          autoFocusElem.focus({ preventScroll: true });
          return;
        }

        const firstFocusable = colPopup.querySelector<HTMLElement>(
          'button:not([disabled]), [href], input:not([disabled]), select:not([disabled]), textarea:not([disabled]), [tabindex]:not([tabindex="-1"])'
        );
        if (firstFocusable && firstFocusable.getClientRects().length > 0) {
          firstFocusable.focus({ preventScroll: true });
        }
      });
    }
  }

  export function EnableColumnResizing(gridElement: HTMLElement, resizeColumnOnAllRows: boolean = true, signal?: AbortSignal) {
    if (gridElement === undefined || gridElement === null) {
      return;
    }

    const columns: Column[] = [];
    const headers = gridElement.querySelectorAll("th[cell-type='columnheader'][resizable='true']");

    if (headers.length === 0) {
      return;
    }

    const id = gridElement.id;
    let grid = grids.find((g: Grid) => g.id === id);

    if (grid?.resizeController) {
      grid.resizeController.abort();
    }

    const localController = new AbortController();
    const effectiveSignal = signal ?? localController.signal;

    const isGrid = gridElement.getAttribute('display-mode') === 'grid';

    let tableHeight = gridElement.offsetHeight;
    // rows have not been loaded yet, so we need to calculate the height
    if (tableHeight < 70) {
      // by getting the aria rowcount attribute
      const rowCount = gridElement.getAttribute('aria-rowcount');
      if (rowCount) {
        const rowHeight = (gridElement.querySelector('thead tr th') as HTMLElement)?.offsetHeight;
        // and multiply by the itemsize (== height of the header cells)
        tableHeight = Number(rowCount) * rowHeight;
      }
    }

    // Determine the height based on the resizeColumnOnAllRows parameter
    let resizeHandleHeight = tableHeight;
    if (!resizeColumnOnAllRows) {
      // Only use the header height when resizeColumnOnAllRows is false
      // Use the first header's height if available
      resizeHandleHeight = headers.length > 0 ? ((headers[0] as HTMLElement).offsetHeight - 14) : 30; // fallback to 30px if no headers
    }

    headers.forEach((header) => {
      columns.push({
        header,
        size: `${isGrid ? (header as HTMLElement).offsetWidth : (header as HTMLElement).clientWidth}px`,
      });

      // remove any previously created divs
      const resizedivs = header.querySelectorAll('[actual-resize-handle]');
      resizedivs.forEach(div => div.remove());

      // get the top of the first resize handle
      const resizeTop = (header.querySelector('[resize-handle]') as HTMLElement)?.offsetTop ?? 2;

      // add a new resize div
      const div = createDiv(resizeHandleHeight, resizeTop);
      header.appendChild(div);
      if (header.nextElementSibling && (header.nextElementSibling as HTMLElement).getAttribute('col-pinned') === 'end') {
        div.style.insetInlineEnd = '-1px';
      }
      setListeners(div, effectiveSignal);
    });

    let initialWidths: string;
    if ((gridElement.style as any).gridTemplateColumns) {
      initialWidths = gridElement.style.gridTemplateColumns;
    } else {
      initialWidths = columns.map(({ size }) => size).join(' ');

      if (isGrid) {
        gridElement.style.gridTemplateColumns = initialWidths;
      }
    }

    if (!grid) {
      grids.push({
        id,
        columns,
        initialWidths,
        resizeController: signal ? undefined : localController,
      });
    } else {
      const columnsChanged = grid.columns.length !== columns.length;
      grid.columns = columns;
      if (columnsChanged) {
        grid.initialWidths = initialWidths;
      }
      grid.resizeController = signal ? undefined : localController;
    }

    function setListeners(div: HTMLElement, signal?: AbortSignal) {
      let pageX: number | undefined, curCol: HTMLElement | undefined, curColWidth: number | undefined;
      let previousDraggable: string | null = null;

      const moveHandler = (e: PointerEvent) => {
        requestAnimationFrame(() => {
          gridElement.style.tableLayout = 'fixed';

          if (curCol) {
            const isRTL = getComputedStyle(gridElement).direction === 'rtl';
            const diffX = isRTL ? (pageX! - e.pageX) : (e.pageX - pageX!);
            const column: Column = columns.find(({ header }) => header === curCol)!;

            const minWidth = getMinWidthPx(column.header as HTMLElement);
            column.size = Math.max(minWidth, curColWidth! + diffX) + 'px';

            columns.forEach((col) => {
              if (col.size.startsWith('minmax')) {
                col.size = (isGrid ? (column.header as HTMLElement).offsetWidth : col.header.clientWidth) + 'px';
              }
            });

            if (isGrid) {
              gridElement.style.gridTemplateColumns = columns
                .map(({ size }) => size)
                .join(' ');

            }
            else {
              curCol.style.width = column.size;
            }

            // Keep sticky offsets in sync after every resize step.
            UpdatePinnedColumnOffsets(gridElement);
          }
        });
      };

      const upHandler = function () {
        gridElement.removeEventListener('pointermove', moveHandler);
        gridElement.removeEventListener('pointerup', upHandler);
        gridElement.removeEventListener('pointerleave', upHandler);
        gridElement.removeEventListener('pointercancel', upHandler);

        if (grid) {
          grid.isResizing = false;
        }

        if (curCol) {
          if (previousDraggable === null) {
            curCol.removeAttribute('draggable');
          } else {
            curCol.setAttribute('draggable', previousDraggable);
          }
        }

        curCol = undefined;
        curColWidth = undefined;
        pageX = undefined;
        previousDraggable = null;
      };

      div.addEventListener('pointerdown', function (e: PointerEvent) {
        curCol = (e.target as HTMLElement).parentElement as HTMLElement;
        pageX = e.pageX;
        previousDraggable = curCol.getAttribute('draggable');
        curCol.setAttribute('draggable', 'false');

        if (grid) {
          grid.isResizing = true;
        }

        const isGrid = gridElement.getAttribute('display-mode') === 'grid';
        const padding = isGrid ? 0 : paddingDiff(curCol);

        curColWidth = curCol.offsetWidth - padding;

        gridElement.addEventListener('pointermove', moveHandler, { signal });
        gridElement.addEventListener('pointerup', upHandler, { signal });
        gridElement.addEventListener('pointerleave', upHandler, { signal });
        gridElement.addEventListener('pointercancel', upHandler, { signal });
      }, { signal });

      div.addEventListener('pointerover', function (e: MouseEvent) {
        (e.target as HTMLElement).style.borderInlineEnd = 'var(--fluent-data-grid-resize-handle-width) solid var(--fluent-data-grid-resize-handle-color)';
        if ((e.target as HTMLElement).previousElementSibling) {
          ((e.target as HTMLElement).previousElementSibling as HTMLElement).style.visibility = 'hidden';
        }
      }, { signal });

      div.addEventListener('pointerup', removeBorder, { signal });
      div.addEventListener('pointercancel', removeBorder, { signal });
      div.addEventListener('pointerleave', removeBorder, { signal });
      div.addEventListener('dragstart', function (e: DragEvent) {
        e.preventDefault();
        e.stopPropagation();
      }, { signal });
    }

    function createDiv(height: number, top: number) {
      const div = document.createElement('div');
      div.setAttribute('actual-resize-handle', '');
      div.style.top = top + 'px';
      div.style.position = 'absolute';
      div.style.cursor = 'col-resize';
      div.style.userSelect = 'none';
      div.style.height = (height - 4) + 'px'; // adjust for the top offset
      div.style.width = '6px';
      div.style.opacity = 'var(--fluent-data-grid-header-opacity)';
      div.style.insetInlineEnd = '0'
      return div;
    }

    function paddingDiff(col: HTMLElement) {
      if (getStyleVal(col, 'box-sizing') === 'border-box') {
        return 0;
      }

      const padLeft = getStyleVal(col, 'padding-left');
      const padRight = getStyleVal(col, 'padding-right');
      return parseInt(padLeft) + parseInt(padRight);
    }

    function getStyleVal(elm: HTMLElement, css: string) {
      return window.getComputedStyle(elm, null).getPropertyValue(css);
    }

    function removeBorder(e: Event) {
      (e.target as HTMLElement).style.borderInlineEnd = '';
      if ((e.target as HTMLElement).previousElementSibling) {
        ((e.target as HTMLElement).previousElementSibling as HTMLElement).style.visibility = 'visible';
      }
    }
  }

  export function ResetColumnWidths(gridElement: HTMLElement) {
    const isGrid = gridElement.getAttribute('display-mode') === 'grid';
    const grid = grids.find(grid => grid.id === gridElement.id);
    if (!grid) {
      return;
    }

    if (isGrid) {
      gridElement.style.gridTemplateColumns = grid.initialWidths;

      // Force browser to recalculate so we can get accurate computed widths
      const resolvedWidths = window.getComputedStyle(gridElement).gridTemplateColumns.split(' ');

      grid.columns.forEach((column, index) => {
        column.size = resolvedWidths[index];
        (column.header as HTMLElement).style.width = "";
      });
    } else {
      const columnsWidths = grid.initialWidths.split(' ');
      grid.columns.forEach((column: any, index: number) => {
        column.size = columnsWidths[index];
        column.header.style.width = column.size;
      });
    }

    gridElement.dispatchEvent(
      new CustomEvent('closecolumnheaderui', { bubbles: true })
    );

    gridElement.focus();
  }

  export function EnableColumnReordering(gridElement: HTMLElement, dotNetHelper: any) {
    if (gridElement === undefined || gridElement === null) {
      return;
    }

    const id = gridElement.id;
    let grid = grids.find((g: Grid) => g.id === id);

    if (grid?.reorderController) {
      grid.reorderController.abort();
    }

    const controller = new AbortController();
    const { signal } = controller;

    if (!grid) {
      grid = {
        id,
        columns: [],
        initialWidths: ''
      };
      grids.push(grid);
    }

    grid.reorderController = controller;

    const headers = Array.from(
      gridElement.querySelectorAll("th[cell-type='columnheader'][reorderable='true']")
    ) as HTMLElement[];

    headers.forEach(header => header.setAttribute('draggable', 'true'));

    let sourceColumnKey: string | null = null;
    let sourceColumnIndex: string | null = null;

    const setColumnAttribute = (columnIndex: string | null, attributeName: string, enabled: boolean = true) => {
      if (!columnIndex) {
        return;
      }

      const columnCells = gridElement.querySelectorAll(`[col-index="${columnIndex}"]`);
      columnCells.forEach(cell => {
        if (enabled) {
          (cell as HTMLElement).setAttribute(attributeName, 'true');
        } else {
          (cell as HTMLElement).removeAttribute(attributeName);
        }
      });
    };

    const clearColumnAttribute = (attributeName: string) => {
      const cells = gridElement.querySelectorAll(`[${attributeName}]`);
      cells.forEach(cell => {
        (cell as HTMLElement).removeAttribute(attributeName);
      });
    };

    const clearDragState = () => {
      clearColumnAttribute('col-reorder-dragging');
      clearColumnAttribute('col-reorder-drop-target');
      gridElement.removeAttribute('col-reorder-active');
      sourceColumnKey = null;
      sourceColumnIndex = null;
    };

    headers.forEach(header => {
      header.addEventListener('dragstart', event => {
        if (grid?.isResizing) {
          event.preventDefault();
          return;
        }

        hideHeaderUi(gridElement);
        sourceColumnKey = header.dataset.columnKey ?? null;
        sourceColumnIndex = header.getAttribute('col-index');

        if (!sourceColumnKey || !sourceColumnIndex) {
          return;
        }

        gridElement.setAttribute('col-reorder-active', 'true');
        setColumnAttribute(sourceColumnIndex, 'col-reorder-dragging');
        event.dataTransfer?.setData('text/plain', sourceColumnKey);
        if (event.dataTransfer) {
          event.dataTransfer.effectAllowed = 'move';
        }
      }, { signal });

      header.addEventListener('dragend', () => {
        clearDragState();
        closeHeaderUi(gridElement);
      }, { signal });

      header.addEventListener('dragover', event => {
        if (!sourceColumnKey || !sourceColumnIndex) {
          return;
        }

        const targetColumnIndex = header.getAttribute('col-index');
        if (!targetColumnIndex || targetColumnIndex === sourceColumnIndex) {
          return;
        }

        event.preventDefault();
        clearColumnAttribute('col-reorder-drop-target');
        setColumnAttribute(targetColumnIndex, 'col-reorder-drop-target');
      }, { signal });

      header.addEventListener('drop', event => {
        if (!sourceColumnKey || !sourceColumnIndex) {
          clearColumnAttribute('col-reorder-drop-target');
          return;
        }

        event.preventDefault();
        const targetColumnKey = header.dataset.columnKey ?? null;
        const targetColumnIndex = header.getAttribute('col-index');

        clearColumnAttribute('col-reorder-drop-target');

        if (!targetColumnKey || !targetColumnIndex || targetColumnIndex === sourceColumnIndex) {
          return;
        }

        const insertAfter = Number(sourceColumnIndex) < Number(targetColumnIndex);

        dotNetHelper.invokeMethodAsync('ReorderColumnFromDragAsync', sourceColumnKey, targetColumnKey, insertAfter)
          .catch((err: any) => console.error('Error invoking Blazor method:', err));
      }, { signal });
    });
  }

  export function ResizeColumnDiscrete(gridElement: HTMLElement, column: string | undefined, change: number) {
    const isGrid = gridElement.getAttribute('display-mode') === 'grid';
    const columns: any[] = [];
    let headerBeingResized: HTMLElement | null | undefined;

    if (!column) {
      const targetElement = (document.activeElement as HTMLElement)?.parentElement;
      if (!(targetElement && targetElement.getAttribute('cell-type') === 'columnheader' && targetElement.getAttribute('resizable') === 'true')) {
        return;
      }
      headerBeingResized = targetElement;
    }
    else {
      headerBeingResized = gridElement.querySelector("th[cell-type='columnheader'][col-index='" + column + "']") as HTMLElement | null;
    }
    grids.find(grid => grid.id === gridElement.id)!.columns.forEach((column: any) => {
      if (column.header === headerBeingResized) {
        const width = headerBeingResized!.offsetWidth + change;
        //const width = headerBeingResized!.getBoundingClientRect().width + change;

        if (change < 0) {
          column.size = Math.max(getMinWidthPx(column.header), width) + 'px';
        }
        else {
          column.size = width + 'px';
        }
        column.header.style.width = column.size;
      }

      if (isGrid) {
        // for grid we need to recalculate all columns that are minmax
        if (column.size.startsWith('minmax')) {
          column.size = column.header.offsetWidth + 'px';
        }
        columns.push(column.size);
      }
    });

    if (isGrid) {
      gridElement.style.gridTemplateColumns = columns.join(' ');
    }
  }

  export function ResizeColumnExact(gridElement: HTMLElement, column: string, width: number) {
    const isGrid = gridElement.getAttribute('display-mode') === 'grid';
    const columns: any[] = [];
    let headerBeingResized = gridElement.querySelector("th[cell-type='columnheader'][col-index='" + column + "']") as HTMLElement | null;

    if (!headerBeingResized) {
      return;
    }

    grids.find(grid => grid.id === gridElement.id)!.columns.forEach((column: any) => {
      if (column.header === headerBeingResized) {
        column.size = Math.max(getMinWidthPx(column.header), width) + 'px';
        column.header.style.width = column.size;
      }

      if (isGrid) {
        // for grid we need to recalculate all columns that are minmax
        if (column.size.startsWith('minmax')) {
          column.size = column.header.offsetWidth + 'px';
        }
        columns.push(column.size);
      }
    });

    if (isGrid) {
      gridElement.style.gridTemplateColumns = columns.join(' ');
    }

    gridElement.dispatchEvent(new CustomEvent('closecolumnheaderui', { bubbles: true }));
    gridElement.focus();
  }

  export function AutoFitGridColumns(gridElement: HTMLElement, columnCount: number) {
    let gridTemplateColumns = '';

    for (let i = 0; i < columnCount; i++) {
      const columnWidths = Array
        .from(gridElement.querySelectorAll(`[col-index="${i + 1}"]`))
        .map((x: any) => x.offsetWidth);

      const maxColumnWidth = Math.max(...columnWidths);

      gridTemplateColumns += ` ${maxColumnWidth}px`;
    }

    gridElement.style.gridTemplateColumns = gridTemplateColumns;
    gridElement.removeAttribute('auto-fit');

    const grid = grids.find(grid => grid.id === gridElement.id);
    if (grid) {
      grid.initialWidths = gridTemplateColumns;
    }
  }

  export function DynamicItemsPerPage(gridElement: HTMLElement, dotNetObject: any, rowSize: number) {
    const observer = new ResizeObserver(() => {
      const visibleRows = calculateVisibleRows(gridElement, rowSize)
      dotNetObject.invokeMethodAsync('UpdateItemsPerPageAsync', visibleRows)
        .catch((err: any) => console.error('Error invoking Blazor method:', err));
    });

    const targetElement = gridElement.parentElement;
    if (targetElement) {
      observer.observe(targetElement);
    }
  }

  function getMinWidthPx(header: HTMLElement): number {
    const configuredMinWidth = header.style.minWidth;
    if (configuredMinWidth) {
      const parsedInlineMinWidth = parseInt(configuredMinWidth, 10);
      return Number.isNaN(parsedInlineMinWidth) ? 100 : parsedInlineMinWidth;
    }

    const parsedComputedMinWidth = parseInt(getComputedStyle(header).minWidth, 10);
    return Number.isNaN(parsedComputedMinWidth) ? 100 : parsedComputedMinWidth;
  }
  function calculateVisibleRows(gridElement: HTMLElement, rowHeight: number) {
    if (rowHeight <= 0) {
      return 0;
    }

    const gridContainer = gridElement.parentElement;

    if (!gridContainer) {
      return 0;
    }

    const availableHeight = gridContainer?.clientHeight || window.visualViewport?.height || window.innerHeight;

    const visibleRows = Math.max(Math.floor(availableHeight / rowHeight), 1);
    return visibleRows;
  }

  /**
   * Recalculates and applies the `left` / `right` inline offsets for every cell in each
   * pinned column so that columns stack correctly against the grid edge after the initial
   * render or after a column is resized.
   *
   * Start-pinned columns are processed in DOM order; each column's offset is the sum of
   * the widths of all start-pinned columns before it.
   * End-pinned columns are processed in reverse DOM order; each column's offset is the sum of
   * the widths of all end-pinned columns after it.
   *
   * The function reads the actual rendered header-cell width so it handles pinned columns whose
   * configured widths use non-pixel CSS units as well as both Grid mode
   * (CSS grid layout) and Table mode (standard table layout). Grid mode uses `offsetWidth`
   * (includes borders, matches the grid-track width) while Table mode uses `clientWidth`
   * (excludes borders, matches the CSS column width), consistent with how existing resize
   * logic measures columns throughout this file.
   */
  export function UpdatePinnedColumnOffsets(gridElement: HTMLElement) {
    const isGrid = gridElement.getAttribute('display-mode') === 'grid';

    /**
     * Returns the rendered pixel width of a header cell in a way that is consistent with
     * how the resize logic elsewhere in this file measures column widths.
     * Grid mode: offsetWidth (includes borders — matches the CSS grid-track size).
     * Table mode: clientWidth (excludes borders — matches the CSS column width).
     */
    function headerWidth(header: HTMLElement): number {
      return isGrid ? header.offsetWidth : header.clientWidth;
    }

    /**
     * Applies a cumulative sticky offset to all cells in a column and returns the new
     * running total to be used by the next column in the sequence.
     */
    function applyOffset(header: HTMLElement, offset: number, side: 'insetInlineStart' | 'insetInlineEnd'): number {
      const colIndex = header.getAttribute('col-index');
      if (!colIndex) { return offset; }

      (gridElement.querySelectorAll(`[col-index="${colIndex}"]`) as NodeListOf<HTMLElement>)
        .forEach(cell => { cell.style[side] = offset + 'px'; });

      return offset + headerWidth(header);
    }

    // Start-pinned columns: process in DOM order.
    const startPinnedHeaders = Array.from(
      gridElement.querySelectorAll("th[col-pinned='start']")
    ) as HTMLElement[];

    let startOffset = 0;
    for (const header of startPinnedHeaders) {
      startOffset = applyOffset(header, startOffset, 'insetInlineStart');
    }

    // End-pinned columns: process in reverse DOM order.
    const endPinnedHeaders = Array.from(
      gridElement.querySelectorAll("th[col-pinned='end']")
    ) as HTMLElement[];

    let endOffset = 0;
    for (let i = endPinnedHeaders.length - 1; i >= 0; i--) {
      endOffset = applyOffset(endPinnedHeaders[i], endOffset, 'insetInlineEnd');
    }
  }
}
