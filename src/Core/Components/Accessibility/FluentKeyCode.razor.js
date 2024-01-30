export function RegisterKeyCode(id, onlyCodes, excludeCodes, stopPropagation, dotNetHelper) {
    const element = document.getElementById(id);
    if (!!element) {
        element.addEventListener('keydown', function (e) {
            const keyCode = e.which || e.keyCode || e.charCode;

            if (stopPropagation) {
                e.stopPropagation();
            }

            if (!!dotNetHelper && !!dotNetHelper.invokeMethodAsync) {

                const targetId = e.currentTarget?.id ?? "";

                // Exclude
                if (excludeCodes.length > 0 && excludeCodes.includes(keyCode)) {
                    return;
                }

                // All or Include only
                if (onlyCodes.length == 0 || (onlyCodes.length > 0 && onlyCodes.includes(keyCode))) {
                    dotNetHelper.invokeMethodAsync("OnKeyDownRaisedAsync", keyCode, e.key, e.ctrlKey, e.shiftKey, e.altKey, e.metaKey, e.location, targetId);
                    return;
                }
            }
        })
    }
}
