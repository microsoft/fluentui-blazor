let resizeObserver;
let observerAddRemove;
let lastHandledState = { id: null, isHorizontal: null };
export function fluentOverflowInitialize(dotNetHelper, id, isHorizontal, querySelector, threshold) {
    var localSelector = querySelector;
    if (!localSelector) {
        // cannot use :scope for node.matches() further down
        localSelector = ".fluent-overflow-item";
    }

    // Create a Add/Remove Observer, started later
    observerAddRemove = new MutationObserver(mutations => {
        mutations.forEach(mutation => {

            // Only new node (type=childList)
            if (mutation.type !== 'childList' && (mutation.addedNodes.length > 0 || mutation.removedNodes.length > 0)) {
                return
            }

            // Only for localSelector element
            const node = mutation.addedNodes.length > 0 ? mutation.addedNodes[0] : mutation.removedNodes[0];
            if (node.nodeType !== Node.ELEMENT_NODE || !node.matches(localSelector)) {
                return;
            }

            fluentOverflowRefresh(dotNetHelper, id, isHorizontal, querySelector, threshold);
        });
    });
    var el = document.getElementById(id);

    // Stop the resize observation if the element is already observed
    if (resizeObserver && el) {
        resizeObserver.unobserve(el);
    }

    let resizeTimeout;
    resizeObserver = new ResizeObserver((entries) => {
        clearTimeout(resizeTimeout);
        resizeTimeout = setTimeout(() => {
            fluentOverflowRefresh(dotNetHelper, id, isHorizontal, querySelector, threshold);
        }, 100); // Adjust the timeout as needed
    });

    // Create a ResizeObserver, started later
    resizeObserver = new ResizeObserver((entries) => {
        fluentOverflowRefresh(dotNetHelper, id, isHorizontal, querySelector, threshold);
    });

    // Start the resize observation
    if (el) {
        resizeObserver.observe(el);
        observerAddRemove.observe(el, { childList: true, subtree: false });
    }

    lastHandledState.id = id;
    lastHandledState.isHorizontal = isHorizontal;
}

// When the Element[id] is resized, set overflow attribute to all element outside of this element.
// Except for elements with fixed attribute.
export function fluentOverflowRefresh(dotNetHelper, id, isHorizontal, querySelector, threshold) {
    let container = document.getElementById(id);
    if (!container) return;

    if (!querySelector) {
        querySelector = ":scope .fluent-overflow-item";
    }
    else {
        querySelector = ":scope >" + querySelector;
    }
    let allItems = container.querySelectorAll(querySelector);
    let items = container.querySelectorAll(querySelector + ":not([fixed])");      // List of first level element of this container
    let fixedItems = container.querySelectorAll(querySelector + "[fixed]");       // List of element defined as fixed (not "overflowable")
    let itemsTotalSize = threshold > 0 ? 10 : 0;
    let containerMaxSize = isHorizontal ? container.offsetWidth : container.offsetHeight;
    let overflowChanged = false;
    let containerGap = parseFloat(window.getComputedStyle(container).gap);
    if (!containerGap) containerGap = 0;

    containerMaxSize -= threshold; // Account for the overflow bage width

    if (lastHandledState.id === id && lastHandledState.isHorizontal !== isHorizontal) {
        allItems.forEach(element => {
            element.removeAttribute("overflow");
            element.overflowSize = null;
        });
    }

    // Size of all fixed elements
    fixedItems.forEach(element => {
        element.overflowSize = isHorizontal ? getElementWidth(element) : getElementHeight(element);
        element.overflowSize += containerGap;
        itemsTotalSize += element.overflowSize;
    });

    // Add overflow attribute, if the element is out of total size.
    items.forEach(element => {
        let isOverflow = element.hasAttribute("overflow");

        // Compute element size (if not already set)
        // Save this element.size in the attribute 'overflowSize'
        if (!isOverflow) {
            element.overflowSize = isHorizontal ? getElementWidth(element) : getElementHeight(element);
            element.overflowSize += containerGap;
        }

        itemsTotalSize += element.overflowSize;

        // Only check for overflow if the container has a size
        if (containerMaxSize > 0) {
            if (itemsTotalSize > containerMaxSize) {
                // Add an attribute 'overflow'
                if (!isOverflow) {
                    element.setAttribute("overflow", "");
                    overflowChanged = true;
                }
            }
            else {
                // Remove the attribute 'overflow'
                if (isOverflow) {
                    element.removeAttribute("overflow");
                    overflowChanged = true;
                }
            }
        }

    });

    // If an attribute 'overflow' has been added or removed,
    // raise a C# method
    if (overflowChanged) {
        let listOfOverflow = [];
        items.forEach(element => {
            listOfOverflow.push({
                Id: element.id,
                Overflow: element.hasAttribute("overflow"),
                Text: element.innerText.trim()
            });
        });
        dotNetHelper.invokeMethodAsync("OverflowRaisedAsync", JSON.stringify(listOfOverflow));
    }
    lastHandledState.id = id;
    lastHandledState.isHorizontal = isHorizontal;
}

export function fluentOverflowDispose(id) {
    let el = document.getElementById(id);
    if (el) {
        resizeObserver.unobserve(el);
        observerAddRemove.disconnect();
    }
}

// Compute the element Width, including paddings, margins and borders.
function getElementWidth(element) {
    var style = element.currentStyle || window.getComputedStyle(element);
    var width = element.offsetWidth;    // Width including paddings and borders
    var margin = parseFloat(style.marginLeft) + parseFloat(style.marginRight);
    return width + margin;
}

// Compute the element Height, including paddings, margins and borders.
function getElementHeight(element) {
    var style = element.currentStyle || window.getComputedStyle(element);
    var height = element.offsetHeight;    // Height including paddings and borders
    var margin = parseFloat(style.marginTop) + parseFloat(style.marginBottom);
    return height + margin;
}
