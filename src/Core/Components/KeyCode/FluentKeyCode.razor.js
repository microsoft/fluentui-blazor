export function RegisterKeyCode(globalDocument, eventNames, id, elementRef, onlyCodes, excludeCodes, stopPropagation, preventDefault, preventDefaultOnly, dotNetHelper, preventMultipleKeydown) {
    const element = globalDocument
                  ? document
                  : elementRef == null ? document.getElementById(id) : elementRef;

    if (document.fluentKeyCodeEvents == null) {
        document.fluentKeyCodeEvents = {};
    }

    if (!!element) {

        const eventId = Math.random().toString(36).slice(2);
        let fired = false;

        const handlerKeydown = function (e) {
            if (!fired || !preventMultipleKeydown) {
                fired = true;
                return handler(e, "OnKeyDownRaisedAsync");
            }
        }

        const handlerKeyup = function (e) {
            fired = false;
            return handler(e, "OnKeyUpRaisedAsync");
        }

        const handler = function (e, netMethod) {
            const keyCode = e.which || e.keyCode || e.charCode;

            if (!!dotNetHelper && !!dotNetHelper.invokeMethodAsync) {

                const targetId = e.currentTarget?.id ?? "";
                const isPreventDefault = preventDefault || (preventDefaultOnly.length > 0 && preventDefaultOnly.includes(keyCode));
                const isStopPropagation = stopPropagation;

                // Exclude
                if (excludeCodes.length > 0 && excludeCodes.includes(keyCode)) {
                    if (isPreventDefault) {
                        e.preventDefault();
                    }
                    if (isStopPropagation) {
                        e.stopPropagation();
                    }
                    return;
                }

                // All or Include only
                if (onlyCodes.length == 0 || (onlyCodes.length > 0 && onlyCodes.includes(keyCode))) {
                    if (isPreventDefault) {
                        e.preventDefault();
                    }
                    if (isStopPropagation) {
                        e.stopPropagation();
                    }
                    dotNetHelper.invokeMethodAsync(netMethod, keyCode, e.key, e.ctrlKey, e.shiftKey, e.altKey, e.metaKey, e.location, targetId);
                    return;
                }
            }
        };

        if (preventMultipleKeydown || (!!eventNames && eventNames.includes("KeyDown"))) {
            element.addEventListener('keydown', handlerKeydown)
        }
        if (preventMultipleKeydown || (!!eventNames && eventNames.includes("KeyUp"))) {
            element.addEventListener('keyup', handlerKeyup)
        }
        document.fluentKeyCodeEvents[eventId] = { source: element, handlerKeydown, handlerKeyup };

        return eventId;
    }

    return "";
}

export function UnregisterKeyCode(eventId) {

    if (document.fluentKeyCodeEvents != null) {
        const keyEvent = document.fluentKeyCodeEvents[eventId];
        const element = keyEvent.source;

        if (!!keyEvent.handlerKeydown) {
            element.removeEventListener("keydown", keyEvent.handlerKeydown);
        }

        if (!!keyEvent.handlerKeyup) {
            element.removeEventListener("keyup", keyEvent.handlerKeyup);
        }

        delete document.fluentKeyCodeEvents[eventId];
    }
}
