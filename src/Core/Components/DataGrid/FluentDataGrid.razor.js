let grids = [];
const minWidth = 100;

export function init(gridElement, autoFocus) {
    if (gridElement === undefined || gridElement === null) {
        return;
    };

    enableColumnResizing(gridElement);

    let start = gridElement.querySelector('td:first-child');

    if (autoFocus) {
        start.focus();
    }

    const bodyClickHandler = event => {
        const columnOptionsElement = gridElement?.querySelector('.col-options');
        if (columnOptionsElement && event.composedPath().indexOf(columnOptionsElement) < 0) {
            gridElement.dispatchEvent(new CustomEvent('closecolumnoptions', { bubbles: true }));
        }
        const columnResizeElement = gridElement?.querySelector('.col-resize');
        if (columnResizeElement && event.composedPath().indexOf(columnResizeElement) < 0) {
            gridElement.dispatchEvent(new CustomEvent('closecolumnresize', { bubbles: true }));
        }
    };
    const keyboardNavigation = (sibling) => {
        if (sibling !== null) {
            start.focus();
            sibling.focus();
            start = sibling;
        }
    }
    const keyDownHandler = event => {
        const columnOptionsElement = gridElement?.querySelector('.col-options');
        if (columnOptionsElement) {
            if (event.key === "Escape") {
                gridElement.dispatchEvent(new CustomEvent('closecolumnoptions', { bubbles: true }));
                gridElement.focus();
            }
            columnOptionsElement.addEventListener(
                "keydown",
                (event) => {
                    if (event.key === "ArrowRight" || event.key === "ArrowLeft" || event.key === "ArrowDown" || event.key === "ArrowUp") {
                        event.stopPropagation();
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
                (event) => {
                    if (event.key === "ArrowRight" || event.key === "ArrowLeft" || event.key === "ArrowDown" || event.key === "ArrowUp") {
                        event.stopPropagation();
                    }
                }
            );
        }

        // check if start is a child of gridElement
        if (start !== null && (gridElement.contains(start) || gridElement === start) && document.activeElement === start) {
            const idx = start.cellIndex;

            if (event.key === "ArrowUp") {
                // up arrow
                const previousRow = start.parentElement.previousElementSibling;
                if (previousRow !== null) {
                    event.preventDefault();
                    const previousSibling = previousRow.cells[idx];
                    keyboardNavigation(previousSibling);
                }
            } else if (event.key === "ArrowDown") {
                // down arrow
                const nextRow = start.parentElement.nextElementSibling;
                if (nextRow !== null) {
                    event.preventDefault();
                    const nextSibling = nextRow.cells[idx];
                    keyboardNavigation(nextSibling);
                }
            } else if (event.key === "ArrowLeft") {
                // left arrow
                event.preventDefault();
                const previousSibling = (document.body.dir === '' || document.body.dir === 'ltr') ? start.previousElementSibling : start.nextElementSibling;
                keyboardNavigation(previousSibling);
                event.stopPropagation();
            } else if (event.key === "ArrowRight") {
                // right arrow
                event.preventDefault();
                const nextsibling = (document.body.dir === '' || document.body.dir === 'ltr') ? start.nextElementSibling : start.previousElementSibling;
                keyboardNavigation(nextsibling);
                event.stopPropagation();
            }
        }
        else {
            start = document.activeElement;
        }

    };

    const cells = gridElement.querySelectorAll('[role="gridcell"]');
    cells.forEach((cell) => {
        cell.columnDefinition = {
            columnDataKey: "",
            cellInternalFocusQueue: true,
            cellFocusTargetCallback: (cell) => {
                return cell.children[0];
            }
        }
        cell.addEventListener(
            "keydown",
            (event) => {
                if (event.target.role !== "gridcell" && (event.key === "ArrowRight" || event.key === "ArrowLeft")) {
                    event.stopPropagation();
                }
            }
        );
    });

    document.body.addEventListener('click', bodyClickHandler);
    document.body.addEventListener('mousedown', bodyClickHandler); // Otherwise it seems strange that it doesn't go away until you release the mouse button
    document.body.addEventListener('keydown', keyDownHandler);

    return {
        stop: () => {
            document.body.removeEventListener('click', bodyClickHandler);
            document.body.removeEventListener('mousedown', bodyClickHandler);
            document.body.removeEventListener('keydown', keyDownHandler);
            delete grids[gridElement];
        }
    };
}

export function checkColumnPopupPosition(gridElement, selector) {
    const colPopup = gridElement.querySelector(selector);
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
        colPopup.scrollIntoViewIfNeeded();

        const autoFocusElem = colPopup.querySelector('[autofocus]');
        if (autoFocusElem) {
            autoFocusElem.focus();
        }
    }
}

export function enableColumnResizing(gridElement) {
    const columns = [];
    let min = 75;
    let headerBeingResized;
    let resizeHandle;

    const headers = gridElement.querySelectorAll('.column-header.resizable');

    if (headers.length === 0) {
        return;
    }

    headers.forEach(header => {
        columns.push({
            header,
            size: `minmax(${minWidth}px,auto)`,
        });

        const onPointerMove = (e) => requestAnimationFrame(() => {
            if (!headerBeingResized) {
                return;
            }
            gridElement.style.tableLayout = "fixed";

            const horizontalScrollOffset = document.documentElement.scrollLeft;
            let width;

            if (document.body.dir === '' || document.body.dir === 'ltr') {
                width = (horizontalScrollOffset + e.clientX) - headerBeingResized.getClientRects()[0].x;
            }
            else {
                width = headerBeingResized.getClientRects()[0].x + headerBeingResized.clientWidth - (horizontalScrollOffset + e.clientX);
            }

            const column = columns.find(({ header }) => header === headerBeingResized);
            column.size = Math.max(minWidth, width) + 'px';

            columns.forEach((column) => {
                if (column.size.startsWith('minmax')) {
                    column.size = parseInt(column.header.clientWidth, 10) + 'px';
                }
            });

            gridElement.style.gridTemplateColumns = columns
                .map(({ size }) => size)
                .join(' ');
        });

        const onPointerUp = (e) => {

            window.removeEventListener('pointermove', onPointerMove);
            window.removeEventListener('pointerup', onPointerUp);
            window.removeEventListener('pointercancel', onPointerUp);
            window.removeEventListener('pointerleave', onPointerUp);

            headerBeingResized.classList.remove('header-being-resized');
            headerBeingResized = null;

            if (e.target.hasPointerCapture(e.pointerId)) {
                e.target.releasePointerCapture(e.pointerId);
            }
        };

        const initResize = ({ target, pointerId }) => {
            headerBeingResized = target.parentNode;
            headerBeingResized.classList.add('header-being-resized');


            window.addEventListener('pointermove', onPointerMove);
            window.addEventListener('pointerup', onPointerUp);
            window.addEventListener('pointercancel', onPointerUp);
            window.addEventListener('pointerleave', onPointerUp);

            if (resizeHandle) {
                resizeHandle.setPointerCapture(pointerId);
            }
        };

        header.querySelector('.resize-handle').addEventListener('pointerdown', initResize);

    });

    let initialWidths;
    if (gridElement.style.gridTemplateColumns) {
        initialWidths = gridElement.style.gridTemplateColumns;
    }
    else {
        initialWidths = columns
            .map(({ header, size }) => size)
            .join(' ');

        gridElement.style.gridTemplateColumns = initialWidths;
    }

    let id = gridElement.id;
    grids.push({
        id,
        columns,
        initialWidths
    });
}

export function resetColumnWidths(gridElement) {

    const grid = grids.find(({ id }) => id === gridElement.id);
    if (!grid) {
        return;
    }

    const columnsWidths = grid.initialWidths.split(' ');

    grid.columns.forEach((column, index) => {
        column.size = columnsWidths[index];
    });

    gridElement.style.gridTemplateColumns = grid.initialWidths;
    gridElement.dispatchEvent(new CustomEvent('closecolumnresize', { bubbles: true }));
    gridElement.focus();
}

export function resizeColumnDiscrete(gridElement, column, change) {

    const columns = [];
    let headerBeingResized;

    if (!column) {
        const targetElement = document.activeElement.parentElement.parentElement.parentElement.parentElement;
        if (!(targetElement.classList.contains("column-header") && targetElement.classList.contains("resizable"))) {
            return;
        }
        headerBeingResized = targetElement;
    }
    else {
        headerBeingResized = gridElement.querySelector('.column-header[col-index="' + column + '"]');
    }


    grids.find(({ id }) => id === gridElement.id).columns.forEach(column => {
        if (column.header === headerBeingResized) {
            const width = headerBeingResized.getBoundingClientRect().width + change;

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

export function resizeColumnExact(gridElement, column, width) {
    const columns = [];
    let headerBeingResized = gridElement.querySelector('.column-header[col-index="' + column + '"]');

    if (!headerBeingResized) {
        return;
    }

    grids.find(({ id }) => id === gridElement.id).columns.forEach(column => {
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

export function autoFitGridColumns(gridElement, columnCount) {
    let gridTemplateColumns = '';

    for (var i = 0; i < columnCount; i++) {
        const columnWidths = Array
            .from(gridElement.querySelectorAll(`[col-index="${i + 1}"]`))
            .flatMap((x) => x.offsetWidth);

        const maxColumnWidth = Math.max(...columnWidths);

        gridTemplateColumns += ` ${maxColumnWidth}px`;
    }

    gridElement.style.gridTemplateColumns = gridTemplateColumns;
    gridElement.classList.remove("auto-fit");

    grids[gridElement.id] = gridTemplateColumns;
}

function calculateVisibleRows(gridElement, rowHeight) {
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

export function dynamicItemsPerPage(gridElement, dotNetObject, rowSize) {
    const observer = new ResizeObserver(() => {
        const visibleRows = calculateVisibleRows(gridElement, rowSize)
        dotNetObject.invokeMethodAsync('UpdateItemsPerPageAsync', visibleRows)
            .catch(err => console.error("Error invoking Blazor method:", err));
    });

    const targetElement = gridElement.parentElement;
    if (targetElement) {
        observer.observe(targetElement);
    }
}
