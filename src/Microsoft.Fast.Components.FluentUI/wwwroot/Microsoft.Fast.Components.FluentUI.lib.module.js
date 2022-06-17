export function afterStarted(Blazor) {
    Blazor.registerCustomEventType('fluentcheckedchange', {
        browserEventName: 'change',
        createEventArgs: event => {
            return {
                checked: event.target.currentChecked
            };
        }
    });
    Blazor.registerCustomEventType('tabchange', {
        browserEventName: 'change',
        createEventArgs: event => {
            return {
                activeId: event.detail.id
            };
        }
    });
}