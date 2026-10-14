
/// <reference path="./d-ts/Blazor.d.ts" />

export namespace Microsoft.FluentUI.Override {
    interface NavigationElement extends HTMLElement {
        href: string;
        internalProxyAnchor: { click(): void };
        handleNavigation(newTab: boolean): void;
    }

    interface TooltipElement extends HTMLElement {
        showPopover(): void;
    }

    interface MenuElement extends HTMLElement {
        _trigger?: HTMLElement;
        slottedMenuList?: HTMLElement[];
    }

    /**
     * Override the default behavior of the Fluent UI Web Components to use Blazor's features.
    */
    export function overrideComponents() {
        waitForCustomElements().then(async registry => {

            /**
             * fluent-anchor-button
            */
            const anchorButton = await registry.whenDefined('fluent-anchor-button');
            anchorButton.prototype.handleNavigation = handleNavigation;

            /**
             * fluent-link
             */
            const link = await registry.whenDefined('fluent-link');
            link.prototype.handleNavigation = handleNavigation;

            /**
             * fluent-tooltip
             */
            const tooltip = await registry.whenDefined('fluent-tooltip');
            const showTooltipPopover = tooltip.prototype.showPopover;
            tooltip.prototype.showPopover = function (this: TooltipElement): void {
                // A tooltip's delayed show callback can run after its dialog removes it from the DOM.
                // Chromium throws when showPopover is called in that state, so discard the stale callback.
                if (!this.isConnected) {
                    return;
                }

                showTooltipPopover.call(this);
            };
        });

        /**
         * fluent-menu
         * The menu opens from its trigger (Enter/Space keydown, hover, context menu) without checking whether
         * the trigger is disabled. A `disabled-focusable` trigger stays focusable and still receives these events,
         * so cancel the opening of any menu whose trigger is disabled. Opening from code (see `openMenuFromCode`)
         * is not affected.
         * Registered on the document in the capture phase (`beforetoggle` does not bubble), so it also covers
         * menus upgraded before this override runs.
         */
        document.addEventListener('beforetoggle', preventDisabledMenuOpening, { capture: true });
    }

    /**
     * The menu being opened by `openMenuFromCode`, if any.
     * `beforetoggle` is dispatched synchronously while the menu opens, so this identifies the menu to let through.
     */
    let menuOpenedFromCode: HTMLElement | null = null;

    /**
     * Opens a menu on behalf of the application, even when its trigger is disabled
     * (for instance, a loading button renders both `disabled` and `disabled-focusable`).
     * @param menu The fluent-menu element to open.
     * @param open The function that opens the menu.
     */
    export function openMenuFromCode(menu: HTMLElement, open: () => void): void {
        menuOpenedFromCode = menu;
        try {
            open();
        }
        finally {
            menuOpenedFromCode = null;
        }
    }

    /**
     * Gets the element that opens the menu.
     * Falls back to the DOM when the private fields of the web component are not available.
     * @param menu The fluent-menu element.
     */
    export function getMenuTrigger(menu: HTMLElement): HTMLElement | null {
        const triggerId = menu.getAttribute('trigger');
        return (menu as MenuElement)._trigger
            ?? menu.querySelector<HTMLElement>(':scope > [slot="trigger"]')
            ?? (triggerId ? document.getElementById(triggerId) : null);
    }

    /**
     * Gets the menu list of the menu itself (not of a submenu).
     * Falls back to the DOM when the private fields of the web component are not available.
     * @param menu The fluent-menu element.
     */
    export function getMenuList(menu: HTMLElement): HTMLElement | null {
        return (menu as MenuElement).slottedMenuList?.[0]
            ?? menu.querySelector<HTMLElement>(':scope > fluent-menu-list');
    }

    function preventDisabledMenuOpening(e: Event): void {
        const menuList = e.target;
        if ((e as ToggleEvent).newState !== 'open' ||
            !(menuList instanceof HTMLElement) ||
            menuList.localName !== 'fluent-menu-list') {
            return;
        }

        // Only the menu's own list: submenus open from their (enabled) parent menu item.
        const menu = menuList.closest<HTMLElement>('fluent-menu');
        if (!menu || menu === menuOpenedFromCode || getMenuList(menu) !== menuList) {
            return;
        }

        if (getMenuTrigger(menu)?.matches('[disabled], [disabled-focusable]')) {
            e.preventDefault();
        }
    }

    /* 
        Original code: https://github.com/microsoft/fluentui/blob/master/packages/web-components/src/anchor-button/anchor-button.base.ts

        private handleNavigation(newTab: boolean): void {
            newTab ? window.open(this.href, '_blank') : this.internalProxyAnchor.click();
        }
    */
    function handleNavigation(this: NavigationElement, newTab: boolean): void {
        if (newTab) {
            window.open(this.href, '_blank');
            return;
        }

        const url = new URL(this.href, document.baseURI);
        if (url.origin === location.origin) {
            const forceLoad = this.getAttribute('force-load') === 'true';
            Blazor.navigateTo(url.pathname + url.search + url.hash, forceLoad);
        }
        else {
            this.internalProxyAnchor.click();
        }
    }

    /**
     * Wait for the customElements registry to be available before overriding the components.
     * This is necessary because the Fluent UI Web Components are defined asynchronously.
     * @returns A promise that resolves with the customElements registry.
     */
    function waitForCustomElements(): Promise<CustomElementRegistry> {
        return new Promise(resolve => {
            const checkCustomElements = () => {
                if (typeof customElements !== 'undefined') {
                    resolve(customElements);
                    return;
                }

                setTimeout(checkCustomElements, 10);
            };

            checkCustomElements();
        });
    }
}