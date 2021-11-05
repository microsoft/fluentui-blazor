export function afterStarted(Blazor) {
    Blazor.registerCustomEventType('fluentcheckedchange', {
        browserEventName: 'change',
        createEventArgs: event => {
            return {
                checked: event.target.currentChecked
            };
        }
    });
}