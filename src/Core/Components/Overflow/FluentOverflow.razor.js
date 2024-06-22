let resizeObserver;
let observerAddRemove;

export function fluentOverflowInitialize(dotNetHelper, id, isHorizontal, querySelector) {
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

            fluentOverflowRefresh(dotNetHelper, id, isHorizontal, querySelector);
        });
    });

    // Create a ResizeObserver, started later
    resizeObserver = new ResizeObserver((entries) => {
        fluentOverflowRefresh(dotNetHelper, id, isHorizontal, querySelector);
    });

    // Start the resize observation
    var el = document.getElementById(id);
    if (el) {
        resizeObserver.observe(el);
        observerAddRemove.observe(el, { childList: true, subtree: false });
    }
}

// When the Element[id] is resized, set overflow attribute to all element outside of this element.
// Except for elements with fixed attribute.
export function fluentOverflowRefresh(dotNetHelper, id, isHorizontal, querySelector) {
    let container = document.getElementById(id);                                  // Container
    if (!container) return;

    if (!querySelector) {
        querySelector = ":scope .fluent-overflow-item";
    }
    else {
        querySelector = ":scope " + querySelector;
    }

    let items = container.querySelectorAll(querySelector + ":not([fixed])");      // List of first level element of this container
    let fixedItems = container.querySelectorAll(querySelector + "[fixed]");       // List of element defined as fixed (not "overflowdable")
    let itemsTotalSize = 10;
    let containerMaxSize = isHorizontal ? container.offsetWidth : container.offsetHeight;
    let overflowChanged = false;
    let containerGap = parseFloat(window.getComputedStyle(container).gap);
    if (!containerGap) containerGap = 0;

    containerMaxSize -= 25; // Account for the overflow bage width

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
}

export function FluentOverflowDispose(id) {
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
