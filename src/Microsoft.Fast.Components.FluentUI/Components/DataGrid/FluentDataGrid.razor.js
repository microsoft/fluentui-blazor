export function init(gridElement) {
    enableColumnResizing(gridElement);

    const bodyClickHandler = event => {
        const columnOptionsElement = gridElement?.querySelector('.col-options');
        if (columnOptionsElement && event.composedPath().indexOf(columnOptionsElement) < 0) {
            gridElement.dispatchEvent(new CustomEvent('closecolumnoptions', { bubbles: true }));
        }
    };
    const keyDownHandler = event => {
        const columnOptionsElement = gridElement?.querySelector('.col-options');
        if (columnOptionsElement && event.key === "Escape") {
            gridElement.dispatchEvent(new CustomEvent('closecolumnoptions', { bubbles: true }));
        }
    };

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
    const min = 150;
    const columns = [];
    let headerBeingResized;

    if (gridElement === null) {
        return;
    };
    gridElement.querySelectorAll('.column-header').forEach(header => {
        const max = '1fr';

        columns.push({
            header,
            // The initial size value for grid-template-columns:
            size: `minmax(${min}px, ${header.clientWidth}px)`
        });

        const onMouseMove = (e) => requestAnimationFrame(() => {
            console.log('onMouseMove');

            let horizontalScrollOffset = gridElement.offsetParent.offsetLeft;
            const width = (e.clientX) - (headerBeingResized.offsetLeft + horizontalScrollOffset);

            const column = columns.find(({ header }) => header === headerBeingResized);
            column.size = Math.max(min, width) + 'px';

            columns.forEach((column) => {
                if (column.size.startsWith('minmax')) {
                    column.size = parseInt(column.header.clientWidth, 10) + 'px';
                }
            });

            gridElement.gridTemplateColumns = columns
                .map(({ header, size }) => size)
                .join(' ');
        });

        const onMouseUp = () => {
            console.log('onMouseUp');

            window.removeEventListener('mousemove', onMouseMove);
            window.removeEventListener('mouseup', onMouseUp);

            if ('ontouchstart' in window) {
                window.removeEventListener('touchmove', onMouseMove);
                window.removeEventListener('touchend', onMouseUp);
            }

            headerBeingResized = null;
        };

        const initResize = ({ target }) => {
            console.log('initResize');

            headerBeingResized = target.parentNode;
            window.addEventListener('mousemove', onMouseMove);
            window.addEventListener('mouseup', onMouseUp);

            if ('ontouchstart' in window) {
                window.addEventListener('touchmove', onMouseMove);
                window.addEventListener('touchend', onMouseUp);
            }
        };

        if (header.querySelector('.col-width-draghandle')) {

            header.querySelector('.col-width-draghandle').addEventListener('mousedown', initResize);
            if ('ontouchstart' in window) {
                header.querySelector('.col-width-draghandle').addEventListener('touchstart', initResize);
            }
        }
    });
}