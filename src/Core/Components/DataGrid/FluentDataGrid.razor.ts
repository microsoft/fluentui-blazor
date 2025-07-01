export namespace Microsoft.FluentUI.Blazor.DataGrid {

  interface Grid {
    id: string;
    columns: any[]; // or a more specific type if you have one
    initialWidths: string;
  }

  interface Column {
    header: Element;
    size: string;
  }

  // Use a dictionary for grids for id-based access
  let grids: { [id: string]: Grid } = {};
  const minWidth = 100;

  export function Initialize(gridElement: HTMLElement, autoFocus: boolean) {
    if (gridElement === undefined || gridElement === null) {
      return;
    }

    EnableColumnResizing(gridElement);

    let start = gridElement.querySelector('td:first-child') as HTMLElement | null;

    if (autoFocus && start) {
      start.focus();
    }

    const bodyClickHandler = (event: MouseEvent) => {
      const columnOptionsElement = gridElement?.querySelector('.col-options');
      if (columnOptionsElement && event.composedPath().indexOf(columnOptionsElement) < 0) {
        gridElement.dispatchEvent(new CustomEvent('closecolumnoptions', { bubbles: true }));
      }
      const columnResizeElement = gridElement?.querySelector('.col-resize');
      if (columnResizeElement && event.composedPath().indexOf(columnResizeElement) < 0) {
        gridElement.dispatchEvent(new CustomEvent('closecolumnresize', { bubbles: true }));
      }
    };
    const keyboardNavigation = (sibling: HTMLElement | null) => {
      if (sibling !== null) {
        if (start) start.focus();
        sibling.focus();
        start = sibling;
      }
    }
    const keyDownHandler = (event: KeyboardEvent) => {
      if (document.activeElement?.tagName.toLowerCase() != 'table' && document.activeElement?.tagName.toLowerCase() != 'td' && document.activeElement?.tagName.toLowerCase() != 'th') {
        return;
      }
      const columnOptionsElement = gridElement?.querySelector('.col-options');
      if (columnOptionsElement) {
        if (event.key === "Escape") {
          gridElement.dispatchEvent(new CustomEvent('closecolumnoptions', { bubbles: true }));
          gridElement.focus();
        }
        columnOptionsElement.addEventListener(
          "keydown",
          function (ev) {
            const kEvent = ev as KeyboardEvent;
            if (kEvent.key === "ArrowRight" || kEvent.key === "ArrowLeft" || kEvent.key === "ArrowDown" || kEvent.key === "ArrowUp") {
              kEvent.stopPropagation();
            }
          }
        );
      }
      const columnResizeElement = gridElement?.querySelector('.col-resize');
      if (columnResizeElement) {
        if (event.key === "Escape") {
          gridElement.dispatchEvent(new CustomEvent('closecolumnresize', { bubbles: true }));
          gridElement.focus();
        }
        columnResizeElement.addEventListener(
          "keydown",
          function (ev) {
            const kEvent = ev as KeyboardEvent;
            if (kEvent.key === "ArrowRight" || kEvent.key === "ArrowLeft" || kEvent.key === "ArrowDown" || kEvent.key === "ArrowUp") {
              kEvent.stopPropagation();
            }
          }
        );
      }

      // check if start is a child of gridElement
      if (start !== null && (gridElement.contains(start) || gridElement === start) && document.activeElement === start && document.activeElement.tagName.toLowerCase() !== 'fluent-text-field' && document.activeElement.tagName.toLowerCase() !== 'fluent-menu-item') {
        const idx = (start as HTMLTableCellElement).cellIndex;

        if (event.key === "ArrowUp") {
          // up arrow
          const previousRow = start.parentElement?.previousElementSibling as HTMLTableRowElement | null;
          if (previousRow !== null) {
            event.preventDefault();
            const previousSibling = previousRow.cells[idx];
            keyboardNavigation(previousSibling);
          }
        } else if (event.key === "ArrowDown") {
          // down arrow
          const nextRow = start.parentElement?.nextElementSibling as HTMLTableRowElement | null;
          if (nextRow !== null) {
            event.preventDefault();
            const nextSibling = nextRow.cells[idx];
            keyboardNavigation(nextSibling);
          }
        } else if (event.key === "ArrowLeft") {
          // left arrow
          event.preventDefault();
          const previousSibling = (document.body.dir === '' || document.body.dir === 'ltr') ? start.previousElementSibling as HTMLElement : start.nextElementSibling as HTMLElement;
          keyboardNavigation(previousSibling);
          event.stopPropagation();
        } else if (event.key === "ArrowRight") {
          // right arrow
          event.preventDefault();
          const nextsibling = (document.body.dir === '' || document.body.dir === 'ltr') ? start.nextElementSibling as HTMLElement : start.previousElementSibling as HTMLElement;
          keyboardNavigation(nextsibling);
          event.stopPropagation();
        }
      }
      else {
        start = document.activeElement as HTMLElement;
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
          if ((event.target as HTMLElement).getAttribute('role') !== "gridcell" && (event.key === "ArrowRight" || event.key === "ArrowLeft")) {
            event.stopPropagation();
          }
        }
      );
    });

    document.body.addEventListener('click', bodyClickHandler);
    document.body.addEventListener('mousedown', bodyClickHandler); // Otherwise it seems strange that it doesn't go away until you release the mouse button
    gridElement.addEventListener('keydown', keyDownHandler);

    return {
      stop: () => {
        document.body.removeEventListener('click', bodyClickHandler);
        document.body.removeEventListener('mousedown', bodyClickHandler);
        gridElement.removeEventListener('keydown', keyDownHandler);
        delete grids[gridElement.id];
      }
    };
  }

  export function CheckColumnPopupPosition(gridElement: HTMLElement, selector: string) {
    const colPopup = gridElement.querySelector(selector) as HTMLElement | null;
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

      const autoFocusElem = colPopup.querySelector('[autofocus]') as HTMLElement | null;
      if (autoFocusElem) {
        autoFocusElem.focus();
      }
    }
  }

  export function EnableColumnResizing(gridElement: HTMLElement, resizeColumnOnAllRows: boolean = true) {
    const columns: Column[] = [];
    const headers = gridElement.querySelectorAll('.column-header.resizable');

    if (headers.length === 0) {
      return;
    }

    const isRTL = getComputedStyle(gridElement).direction === 'rtl';
    const isGrid = gridElement.classList.contains('grid');

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
        size: `${(header as HTMLElement).clientWidth}px`,
      });

      // remove any previously created divs
      const resizedivs = header.querySelectorAll('.actual-resize-handle');
      resizedivs.forEach(div => div.remove());

      // add a new resize div
      const div = createDiv(resizeHandleHeight, isRTL);
      header.appendChild(div);
      setListeners(div, isRTL);
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

    const id = gridElement.id;
    grids[id] = {
      id,
      columns,
      initialWidths,
    };

    function setListeners(div: HTMLElement, isRTL: boolean) {
      let pageX: number | undefined, curCol: HTMLElement | undefined, curColWidth: number | undefined;

      div.addEventListener('pointerdown', function (e: PointerEvent) {
        curCol = (e.target as HTMLElement).parentElement as HTMLElement;
        pageX = e.pageX;

        const padding = paddingDiff(curCol);

        curColWidth = curCol.offsetWidth - padding;
      });

      div.addEventListener('pointerover', function (e: MouseEvent) {
        (e.target as HTMLElement).style.borderInlineEnd = 'var(--fluent-data-grid-resize-handle-width) solid var(--fluent-data-grid-resize-handle-color)';
        if ((e.target as HTMLElement).previousElementSibling) {
          ((e.target as HTMLElement).previousElementSibling as HTMLElement).style.visibility = 'hidden';
        }
      });

      div.addEventListener('pointerup', removeBorder);
      div.addEventListener('pointercancel', removeBorder);
      div.addEventListener('pointerleave', removeBorder);

      document.addEventListener('pointermove', (e: PointerEvent) =>
        requestAnimationFrame(() => {
          gridElement.style.tableLayout = 'fixed';

          if (curCol) {
            const diffX = isRTL ? (pageX! - e.pageX) : (e.pageX - pageX!);
            const column: Column = columns.find(({ header }) => header === curCol)!;

            column.size = parseInt(Math.max(minWidth, curColWidth! + diffX) as any, 10) + 'px';

            columns.forEach((col) => {
              if (col.size.startsWith('minmax')) {
                col.size = col.header.clientWidth + 'px';
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
          }
        })
      );

      document.addEventListener('pointerup', function () {
        curCol = undefined;
        curColWidth = undefined;
        pageX = undefined;
      });
    }

    function createDiv(height: number, isRTL: boolean) {
      const div = document.createElement('div');
      div.className = 'actual-resize-handle';
      div.style.top = '5px';
      div.style.position = 'absolute';
      div.style.cursor = 'col-resize';
      div.style.userSelect = 'none';
      div.style.height = height + 'px';
      div.style.width = '6px';
      div.style.opacity = 'var(--fluent-data-grid-header-opacity)';

      if (isRTL) {
        div.style.left = '0px';
        div.style.right = 'unset';
      } else {
        div.style.left = 'unset';
        div.style.right = '0px';
      }
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
    const isGrid = gridElement.classList.contains('grid');
    const grid = grids[gridElement.id];
    if (!grid) {
      return;
    }

    const columnsWidths = grid.initialWidths.split(' ');

    grid.columns.forEach((column: any, index: number) => {
      if (isGrid) {
        column.size = columnsWidths[index];
      } else {
        column.header.style.width = columnsWidths[index];
      }
    });

    if (isGrid) {
      gridElement.style.gridTemplateColumns = grid.initialWidths;
    }
    gridElement.dispatchEvent(
      new CustomEvent('closecolumnresize', { bubbles: true })
    );
    gridElement.focus();
  }

  export function ResizeColumnDiscrete(gridElement: HTMLElement, column: string | undefined, change: number) {
    const columns: any[] = [];
    let headerBeingResized: HTMLElement | null | undefined;

    if (!column) {
      const targetElement = (document.activeElement as HTMLElement)?.parentElement?.parentElement?.parentElement?.parentElement;
      if (!(targetElement && targetElement.classList.contains('column-header') && targetElement.classList.contains('resizable'))) {
        return;
      }
      headerBeingResized = targetElement;
    }
    else {
      headerBeingResized = gridElement.querySelector('.column-header[col-index="' + column + '"]') as HTMLElement | null;
    }

    grids[gridElement.id].columns.forEach((column: any) => {
      if (column.header === headerBeingResized) {
        const width = headerBeingResized!.getBoundingClientRect().width + change;

        if (change < 0) {
          column.size = Math.max(minWidth, width) + 'px';
        }
        else {
          column.size = width + 'px';
        }
      }
      else {
        if (column.size.startsWith('minmax')) {
          column.size = parseInt(column.header.clientWidth, 10) + 'px';
        }
      }
      columns.push(column.size);
    });

    gridElement.style.gridTemplateColumns = columns.join(' ');
  }

  export function ResizeColumnExact(gridElement: HTMLElement, column: string, width: number) {
    const columns: any[] = [];
    let headerBeingResized = gridElement.querySelector('.column-header[col-index="' + column + '"]') as HTMLElement | null;

    if (!headerBeingResized) {
      return;
    }

    grids[gridElement.id].columns.forEach((column: any) => {
      if (column.header === headerBeingResized) {
        column.size = Math.max(minWidth, width) + 'px';
      }
      else {
        if (column.size.startsWith('minmax')) {
          column.size = parseInt(column.header.clientWidth, 10) + 'px';
        }
      }
      columns.push(column.size);
    });

    gridElement.style.gridTemplateColumns = columns.join(' ');

    gridElement.dispatchEvent(new CustomEvent('closecolumnresize', { bubbles: true }));
    gridElement.focus();
  }

  export function AutoFitGridColumns(gridElement: HTMLElement, columnCount: number) {
    let gridTemplateColumns = '';

    for (let i = 0; i < columnCount; i++) {
      const columnWidths = Array
        .from(gridElement.querySelectorAll(`[col-index="${i + 1}"]`))
        .flatMap((x: any) => x.offsetWidth);

      const maxColumnWidth = Math.max(...columnWidths);

      gridTemplateColumns += ` ${maxColumnWidth}px`;
    }

    gridElement.style.gridTemplateColumns = gridTemplateColumns;
    gridElement.classList.remove('auto-fit');

    grids[gridElement.id].initialWidths = gridTemplateColumns;
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
}
