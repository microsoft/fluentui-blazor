export function RegisterKeyCode(globalDocument, id, elementRef, onlyCodes, excludeCodes, stopPropagation, preventDefault, preventDefaultOnly, dotNetHelper) {
    const element = globalDocument
                  ? document
                  : elementRef == null ? document.getElementById(id) : elementRef;

    if (document.fluentKeyCodeEvents == null) {
        document.fluentKeyCodeEvents = {};
    }

    if (!!element) {

        const eventId = Math.random().toString(36).slice(2);
        const handler = function (e) {
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
                    dotNetHelper.invokeMethodAsync("OnKeyDownRaisedAsync", keyCode, e.key, e.ctrlKey, e.shiftKey, e.altKey, e.metaKey, e.location, targetId);
                    return;
                }
            }
        };

        element.addEventListener('keydown', handler)
        document.fluentKeyCodeEvents[eventId] = { source: element, handler };

        return eventId;
    }

    return "";
}

export function UnregisterKeyCode(eventId) {

    if (document.fluentKeyCodeEvents != null) {
        const keyEvent = document.fluentKeyCodeEvents[eventId];
        const element = keyEvent.source;
        const handler = keyEvent.handler;

        element.removeEventListener("keydown", handler);

        delete document.fluentKeyCodeEvents[eventId];
    }
}
