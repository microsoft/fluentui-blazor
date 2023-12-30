import { findFocusableElements } from '/_content/Microsoft.FluentUI.AspNetCore.Components/Microsoft.FluentUI.AspNetCore.Components.lib.module.js';

const SELECT_FOCUSED_ATTR_NAME = 'data-fui-select-focused';

export function initSelectionHandlers(cardElement, checkboxElement) {
    if (!!checkboxElement) {
        initSelectionCheckbox(cardElement, checkboxElement);
    }

    const clickHandler = (event) => { onChangeHandler(event, cardElement); };
    const keyDownHandler = (event) => { onKeyDownHandler(event, cardElement); }

    cardElement.addEventListener('click', clickHandler);
    cardElement.addEventListener('keydown', keyDownHandler);

    return {
        removeHandlers: () => {
            cardElement.removeEventListener(clickHandler);
            cardElement.removeEventListener(keyDownHandler);
        }
    }
}

export function initSelectionCheckbox(cardElement, checkboxElement) {
    const a11yProperties = getA11yProperties(cardElement);

    if (a11yProperties.referenceId) {
        checkboxElement.setAttribute('aria-labelledby', a11yProperties.referenceId);
    } else if (a11yProperties.referenceLabel) {
        checkboxElement.setAttribute('aria-label', a11yProperties.referenceLabel);
    }

    checkboxElement.addEventListener('focus', (_) => cardElement.setAttribute(SELECT_FOCUSED_ATTR_NAME, ''));
    checkboxElement.addEventListener('blur', (_) => cardElement.removeAttribute(SELECT_FOCUSED_ATTR_NAME));
}

function getA11yProperties(cardElement) {
    const headerElement = cardElement.querySelector('.fluent-card-header > .header');

    let referenceId = headerElement?.getAttribute('id');
    let referenceLabel = null;

    if (!!headerElement) {
        const childId = getChildWithId(cardElement)?.getAttribute('id');

        if (!!childId) {
            referenceId = childId;
        }
    }

    if (!referenceId) {
        const previewImageElement = cardElement.querySelector('.fluent-card-preview > img');

        if (!!previewImageElement) {
            const ariaLabel = previewImageElement.getAttribute('aria-label');
            const ariaDescribedby = previewImageElement.getAttribute('aria-describedby');

            if (ariaDescribedby) {
                referenceId = ariaDescribedby;
            } else if (previewImageElement.alt) {
                referenceLabel = previewImageElement.alt;
            } else if (ariaLabel) {
                referenceLabel = ariaLabel;
            }
        }
    }

    return {
        referenceId,
        referenceLabel
    };
}

function getChildWithId(element) {
    if (element.children.length) {
        for (let i = 0; i < element.children.length; i++) {
            const child = element.children[i];
            if (!!child.getAttribute('id')) {
                return child;
            }
            const childWithId = getChildWithId(child);

            if (!!childWithId) {
                return childWithId;
            }
        }
    }
}

function isSelected(cardElement) {
    const selectedAttributeValue = cardElement.getAttribute('selected');

    return selectedAttributeValue === '' || !!selectedAttributeValue;
}

function shouldRestrictTriggerAction(event, cardElement) {
    const defaultCheckboxElement = cardElement.querySelector('.fluent-card-checkbox');
    const focusableElements = findFocusableElements(cardElement);
    const target = event.target;
    const isElementInFocusableGroup = focusableElements.some(element => element.contains(target));
    const isDefaultCheckbox = defaultCheckboxElement === target;

    return isElementInFocusableGroup && !isDefaultCheckbox;
}

function onChangeHandler(event, cardElement) {
    if (shouldRestrictTriggerAction(event, cardElement)) {
        return;
    }

    const selectedChangeEventDetails = {
        selected: !isSelected(cardElement)
    }

    const selectedChangeEvent = new CustomEvent(
        'card-selected',
        {
            detail: selectedChangeEventDetails,
            bubbles: true,
            cancelable: true
        })
    cardElement.dispatchEvent(selectedChangeEvent);
}

function onKeyDownHandler(event, cardElement) {
    if (['Enter'].includes(event.key)) {
        event.preventDefault();
        onChangeHandler(event, cardElement);
    }
}
