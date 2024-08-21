let eventName = 'mousedown';
let closeCalendarFromExternalClickFunctionName = 'CloseCalendarFromExternalClickAsync';
let mouseDownEventHandler = null;

export function addEventListenerToCheckExternalClick(dotNetHelper, datePickerTextFieldId, calenderId) {
    if (mouseDownEventHandler) {
        return;
    }

    let isOnCalendar = function (element, id) {
        while (element) {
            if (element.id === id) {
                return true;
            }

            element = element.parentElement;
        }

        return false;
    }

    mouseDownEventHandler = function (event) {
        if (event.target.id === datePickerTextFieldId || isOnCalendar(event.target, calenderId)) {
            return;
        };

        try {
            // setTimeout: https://github.com/dotnet/aspnetcore/issues/26809
            setTimeout(() => {
                dotNetHelper.invokeMethodAsync(closeCalendarFromExternalClickFunctionName);
            }, 0);
        } catch (error) {
            console.error(`FluentDatePicker: failing to call ${closeCalendarFromExternalClickFunctionName}.`, error);
        }
    }

    document.addEventListener(eventName, mouseDownEventHandler);
 }

export function removeEventListenerToCheckExternalClick() {
    if (mouseDownEventHandler) {
        document.removeEventListener(eventName, mouseDownEventHandler);
        mouseDownEventHandler = null;
    }
}
