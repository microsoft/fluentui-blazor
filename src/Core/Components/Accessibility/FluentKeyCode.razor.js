export function RegisterKeyCode(id, onlyCodes, excludeCodes, dotNetHelper) {
    const element = document.getElementById(id);
    if (!!element) {
        element.addEventListener('keydown', function (e) {
            const keyCode = event.which || event.keyCode || event.charCode;

            console.log(keyCode);

            // Exclude
            if (excludeCodes.length > 0 && excludeCodes.includes(keyCode)) {
                return;
            }

            // Include
            if (onlyCodes.length > 0 && onlyCodes.includes(keyCode)) {
                dotNetHelper.invokeMethodAsync("OnKeyDownRaised", keyCode);
                return;
            }

            if (onlyCodes.length == 0) {
                dotNetHelper.invokeMethodAsync("OnKeyDownRaised", keyCode);
                return;
            }
        })
    }
}