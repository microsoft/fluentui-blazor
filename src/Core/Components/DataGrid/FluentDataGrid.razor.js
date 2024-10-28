let initialColumnsWidths = {};

export function init(gridElement) {
    if (gridElement === undefined || gridElement === null) {
        return;
    };

    if (gridElement.querySelectorAll('.column-header.resizable').length > 0) {
        initialColumnsWidths[gridElement.id] = gridElement.gridTemplateColumns ;
        enableColumnResizing(gridElement);
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
            delete initialColumnsWidths[gridElement.id];
        }
    };
}

export function checkColumnOptionsPosition(gridElement) {
    const colOptions = gridElement?._rowElements[0] && gridElement?.querySelector('.col-options'); // Only match within *our* thead, not nested tables
    if (colOptions) {
        // We want the options popup to be positioned over the grid, not overflowing on either side, because it's possible that
        // beyond either side is off-screen or outside the scroll range of an ancestor
        const gridRect = gridElement.getBoundingClientRect();
        const optionsRect = colOptions.getBoundingClientRect();
        const leftOverhang = Math.max(0, gridRect.left - optionsRect.left);
        const rightOverhang = Math.max(0, optionsRect.right - gridRect.right);
        if (leftOverhang || rightOverhang) {
            // In the unlikely event that it overhangs both sides, we'll center it
            const applyOffset = leftOverhang && rightOverhang ? (leftOverhang - rightOverhang) / 2 : (leftOverhang - rightOverhang);
            colOptions.style.transform = `translateX(${applyOffset}px)`;
        }

        colOptions.scrollIntoViewIfNeeded();

        const autoFocusElem = colOptions.querySelector('[autofocus]');
        if (autoFocusElem) {
            autoFocusElem.focus();
        }
    }
}

export function checkColumnResizePosition(gridElement) {
    const colOptions = gridElement?._rowElements[0] && gridElement?.querySelector('.col-resize'); // Only match within *our* thead, not nested tables
    if (colResize) {
        // We want the options popup to be positioned over the grid, not overflowing on either side, because it's possible that
        // beyond either side is off-screen or outside the scroll range of an ancestor
        const gridRect = gridElement.getBoundingClientRect();
        const resizeRect = colResize.getBoundingClientRect();
        const leftOverhang = Math.max(0, gridRect.left - resizeRect.left);
        const rightOverhang = Math.max(0, resizeRect.right - gridRect.right);
        if (leftOverhang || rightOverhang) {
            // In the unlikely event that it overhangs both sides, we'll center it
            const applyOffset = leftOverhang && rightOverhang ? (leftOverhang - rightOverhang) / 2 : (leftOverhang - rightOverhang);
            colResize.style.transform = `translateX(${applyOffset}px)`;
        }

        colResize.scrollIntoViewIfNeeded();

        const autoFocusElem = colResize.querySelector('[autofocus]');
        if (autoFocusElem) {
            autoFocusElem.focus();
        }
    }
}

export function checkColumnPopupPosition(gridElement, selector) {
    const colPopup = gridElement?._rowElements[0] && gridElement?.querySelector(selector); // Only match within *our* thead, not nested tables
    if (colPopup) {
        // We want the options popup to be positioned over the grid, not overflowing on either side, because it's possible that
        // beyond either side is off-screen or outside the scroll range of an ancestor
        const gridRect = gridElement.getBoundingClientRect();
        const popupRect = colPopup.getBoundingClientRect();
        const leftOverhang = Math.max(0, gridRect.left - popupRect.left);
        const rightOverhang = Math.max(0, popupRect.right - gridRect.right);
        if (leftOverhang || rightOverhang) {
            // In the unlikely event that it overhangs both sides, we'll center it
            const applyOffset = leftOverhang && rightOverhang ? (leftOverhang - rightOverhang) / 2 : (leftOverhang - rightOverhang);
            colPopup.style.transform = `translateX(${applyOffset}px)`;
        }

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

    gridElement.querySelectorAll('.column-header.resizable').forEach(header => {
        columns.push({ header });
        const onPointerMove = (e) => requestAnimationFrame(() => {
            if (!headerBeingResized) {
                return;
            }

            let gridEdge;
            let headerLocalEdge;
            let pointerLocalEdge;

            if (document.body.dir === '' || document.body.dir === 'ltr') {
                gridEdge = gridElement.getBoundingClientRect().left;
                headerLocalEdge = headerBeingResized.getBoundingClientRect().left - gridEdge;
                pointerLocalEdge = e.clientX - gridEdge;
            }
            else {
                gridEdge = gridElement.getBoundingClientRect().right;
                headerLocalEdge = gridEdge - headerBeingResized.getBoundingClientRect().right;
                pointerLocalEdge = gridEdge - e.clientX;
            }
            const width = pointerLocalEdge - headerLocalEdge;

            const column = columns.find(({ header }) => header === headerBeingResized);
            min = header.querySelector('.col-options-button') ? 100 : 75;

            column.size = Math.max(min, width) + 'px';

            // Set initial sizes
            columns.forEach((column) => {
                if (column.size === undefined) {
                    if (column.header.clientWidth === undefined || column.header.clientWidth === 0) {
                        column.size = '50px';
                    } else {
                        column.size = column.header.clientWidth + 'px';
                    }
                }
            });

            gridElement.gridTemplateColumns = columns
                .map(({ size }) => size)
                .join(' ');
        });

        const onPointerUp = (e) => {
            headerBeingResized = undefined;
            resizeHandle = undefined;

            if (e.target.hasPointerCapture(e.pointerId)) {
                e.target.releasePointerCapture(e.pointerId);
            }
        };

        const initResize = ({ target, pointerId }) => {
            resizeHandle = target;
            headerBeingResized = target.parentNode;

            if (resizeHandle) {
                resizeHandle.setPointerCapture(pointerId);
            }
        };

        const dragHandle = header.querySelector('.col-width-draghandle');
        if (dragHandle) {
            dragHandle.addEventListener('pointerdown', initResize);
            dragHandle.addEventListener('pointermove', onPointerMove);
            dragHandle.addEventListener('pointerup', onPointerUp);
            dragHandle.addEventListener('pointercancel', onPointerUp);
            dragHandle.addEventListener('pointerleave', onPointerUp);
        }
    });
}

export function resetColumnWidths(gridElement) {

    gridElement.gridTemplateColumns = initialColumnsWidths[gridElement.id];
}

export function resizeColumnDiscrete(gridElement, column, change) {

    let headers = gridElement.querySelectorAll('.column-header.resizable');
    if (headers.length <= 0) {
        return
    }

    let headerBeingResized;
    if (!column) {

        if (!(document.activeElement.classList.contains("column-header") && document.activeElement.classList.contains("resizable"))) {
            return;
        }
        headerBeingResized = document.activeElement;
    }
    else {
        headerBeingResized = gridElement.querySelector('.column-header[grid-column="' + column + '"]');
    }
    const columns = [];

    let min = 50;

    headers.forEach(header => {
        if (header === headerBeingResized) {
            min = headerBeingResized.querySelector('.col-options-button') ? 75 : 50;

            const width = headerBeingResized.getBoundingClientRect().width + change;

            if (change < 0) {
                header.size = Math.max(min, width) + 'px';
            }
            else {
                header.size = width + 'px';
            }
        }
        else {
            if (header.size === undefined) {
                if (header.clientWidth === undefined || header.clientWidth === 0) {
                    header.size = min + 'px';
                } else {
                    header.size = header.clientWidth + 'px';
                }
            }
        }

        columns.push({ header });
    });

    gridElement.gridTemplateColumns = columns
        .map(({ header }) => header.size)
        .join(' ');
}

export function autoFitGridColumns(gridElement, columnCount) {
    let gridTemplateColumns = '';

    for (var i = 0; i < columnCount; i++) {
        const columnWidths = Array
            .from(gridElement.querySelectorAll(`[grid-column="${i + 1}"]`))
            .flatMap((x) => x.offsetWidth);

        const maxColumnWidth = Math.max(...columnWidths);

        gridTemplateColumns += ` ${maxColumnWidth}fr`;
    }

    gridElement.setAttribute("grid-template-columns", gridTemplateColumns);
    gridElement.classList.remove("auto-fit");

    initialColumnsWidths[gridElement.id] = gridTemplateColumns;
}

export function resizeColumnExact(gridElement, column, width) {

    let headers = gridElement.querySelectorAll('.column-header.resizable');
    if (headers.length <= 0) {
        return
    }

    let headerBeingResized = gridElement.querySelector('.column-header[grid-column="' + column + '"]');
    if (!headerBeingResized) {
        return;
    }
    const columns = [];

    let min = 50;

    headers.forEach(header => {
        if (header === headerBeingResized) {
            min = headerBeingResized.querySelector('.col-options-button') ? 75 : 50;

            const newWidth = width;

            header.size = Math.max(min, newWidth) + 'px';
        }
        else {
            if (header.size === undefined) {
                if (header.clientWidth === undefined || header.clientWidth === 0) {
                    header.size = min + 'px';
                } else {
                    header.size = header.clientWidth + 'px';
                }
            }
        }

        columns.push({ header });
    });

    gridElement.gridTemplateColumns = columns
        .map(({ header }) => header.size)
        .join(' ');

    gridElement.dispatchEvent(new CustomEvent('closecolumnoptions', { bubbles: true }));
    gridElement.dispatchEvent(new CustomEvent('closecolumnresize', { bubbles: true }));
    gridElement.focus();
}
