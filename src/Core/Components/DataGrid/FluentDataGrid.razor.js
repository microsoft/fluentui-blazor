export function init(gridElement) {
    if (gridElement === undefined || gridElement === null) {
        return;
    };

    enableColumnResizing(gridElement);

    const bodyClickHandler = event => {
        const columnOptionsElement = gridElement?.querySelector('.col-options');
        if (columnOptionsElement && event.composedPath().indexOf(columnOptionsElement) < 0) {
            gridElement.dispatchEvent(new CustomEvent('closecolumnoptions', { bubbles: true }));
        }
    };
    const keyDownHandler = event => {
        const columnOptionsElement = gridElement?.querySelector('.col-options');
        if (columnOptionsElement) {
            if (event.key === "Escape") {
                gridElement.dispatchEvent(new CustomEvent('closecolumnoptions', { bubbles: true }));
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
        }
    };
}



export function checkColumnOptionsPosition(gridElement) {
    const colOptions = gridElement?._rowItems[0] && gridElement?.querySelector('.col-options'); // Only match within *our* thead, not nested tables
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

function enableColumnResizing(gridElement) {
    const columns = [];
    let min = 50;
    let headerBeingResized;
    let resizeHandle;

    gridElement.querySelectorAll('.column-header').forEach(header => {
        columns.push({ header });
        const onPointerMove = (e) => requestAnimationFrame(() => {
            //console.log(`onPointerMove${headerBeingResized ? '' : ' [not resizing]'}`);

            if (!headerBeingResized) {
                return;
            }

            const gridLeft = gridElement.getBoundingClientRect().left;
            const headerLocalLeft = headerBeingResized.getBoundingClientRect().left - gridLeft;
            const pointerLocalLeft = e.clientX - gridLeft;

            const width = pointerLocalLeft - headerLocalLeft;

            const column = columns.find(({ header }) => header === headerBeingResized);
            min = header.querySelector('.col-options-button') ? 75 : 50;

            column.size = Math.max(min, width) + 'px';

            // Set initial sizes
            columns.forEach((column) => {
                if (column.size === undefined) {
                    column.size = column.header.clientWidth + 'px';
                }
            });

            gridElement.gridTemplateColumns = columns
                .map(({ size }) => size)
                .join(' ');
        });

        const onPointerUp = () => {
            //console.log('onPointerUp');

            headerBeingResized = undefined;
            resizeHandle = undefined;
        };

        const initResize = ({ target, pointerId }) => {
            //console.log('initResize');

            resizeHandle = target;
            headerBeingResized = target.parentNode;

            resizeHandle.setPointerCapture(pointerId);
        };

        const dragHandle = header.querySelector('.col-width-draghandle');
        if (dragHandle) {
            dragHandle.addEventListener('pointerdown', initResize);
            dragHandle.addEventListener('pointermove', onPointerMove);
            dragHandle.addEventListener('pointerup', onPointerUp);
        }
    });
}
