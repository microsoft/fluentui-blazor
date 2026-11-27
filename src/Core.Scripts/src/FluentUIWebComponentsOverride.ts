
/// <reference path="./d-ts/Blazor.d.ts" />
import type { Menu } from "@fluentui/web-components";
import { Microsoft as MenuDisabledTriggerFile } from "./Components/Menu/FluentMenuDisabledTrigger";

export namespace Microsoft.FluentUI.Override {
    import MenuDisabledTrigger = MenuDisabledTriggerFile.FluentUI.Blazor.Components.Menu;

    interface NavigationElement extends HTMLElement {
        href: string;
        internalProxyAnchor: { click(): void };
        handleNavigation(newTab: boolean): void;
    }

    interface TooltipElement extends HTMLElement {
        showPopover(): void;
    }

    interface DropdownElement extends HTMLElement {
        listbox?: HTMLElement;
        openChanged(prev: boolean, next: boolean): void;
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

            /**
             * fluent-menu
             * Keeps the menu closed while its trigger is disabled (see FluentMenuDisabledTrigger.ts).
             */
            const menu = await registry.whenDefined('fluent-menu');

            // The browser keeps the connectedCallback it found when the element was defined, so replacing it now
            // would not run. The menu's connectedCallback calls setComponent, a public method that Fluent kept
            // (as a no-op) for backward compatibility, so it is where each menu connected from now on is set up.
            const setMenuComponent = menu.prototype.setComponent;
            menu.prototype.setComponent = function (this: Menu): void {
                setMenuComponent.call(this);
                MenuDisabledTrigger.keepClosedWhileTriggerDisabled(this);
            };

            // Menus connected before this override (prerendered markup) did not go through it.
            document.querySelectorAll<Menu>('fluent-menu').forEach(MenuDisabledTrigger.keepClosedWhileTriggerDisabled);

            /**
             * fluent-dropdown
             */
            await fixDropdownListboxScrollOnWebKit(registry);
        });
    }

    /**
     * WebKit resets the scroll position of a CSS anchor-positioned popover on `pointerdown`,
     * so an option tapped or clicked after scrolling is not selected (https://github.com/microsoft/fluentui/issues/36823).
     * On WebKit, the opened listbox is positioned with explicit coordinates instead of CSS anchor positioning.
     */
    async function fixDropdownListboxScrollOnWebKit(registry: CustomElementRegistry): Promise<void> {
        // Only WebKit browsers (Safari and every iOS browser) using CSS anchor positioning are affected.
        // Without anchor positioning support, the web component already uses its own JavaScript fallback.
        const isWebKit = /^Apple/.test(navigator.vendor);
        if (!isWebKit || !CSS.supports('anchor-name: --anchor')) {
            return;
        }

        // Scroll/resize listeners of each open dropdown, removed when the dropdown closes.
        const repositionControllers = new WeakMap<DropdownElement, AbortController>();

        const dropdown = await registry.whenDefined('fluent-dropdown');
        const dropdownOpenChanged = dropdown.prototype.openChanged;

        dropdown.prototype.openChanged = function (this: DropdownElement, prev: boolean, next: boolean): void {
            dropdownOpenChanged.call(this, prev, next);

            // Stop following the page of a previous opening.
            repositionControllers.get(this)?.abort();
            repositionControllers.delete(this);

            const listbox = this.listbox;
            if (!next || !listbox) {
                return;
            }

            // Place the listbox below the field, or above it when there is not enough room below,
            // replacing the CSS anchor rules of the listbox (position-anchor, anchor(), anchor-size()).
            const reposition = () => {
                const field = this.getBoundingClientRect();
                const maxHeight = window.innerHeight / 2;
                const spaceBelow = window.innerHeight - field.bottom;
                const openAbove = spaceBelow < Math.min(listbox.scrollHeight, maxHeight) && field.top > spaceBelow;

                const styles: Record<string, string> = {
                    'position-anchor': 'none',
                    'position-try-fallbacks': 'none',
                    '--listbox-max-height': `${maxHeight}px`,
                    'min-width': `${field.width}px`,
                    'left': `${field.left}px`,
                    'top': openAbove ? 'auto' : `${field.bottom}px`,
                    'bottom': openAbove ? `${window.innerHeight - field.top}px` : 'auto',
                };

                Object.entries(styles).forEach(([name, value]) => listbox.style.setProperty(name, value));
            };

            // Without CSS anchor, the listbox must follow the field when the page scrolls or is resized.
            const controller = new AbortController();
            window.addEventListener('scroll', reposition, { passive: true, capture: true, signal: controller.signal });
            window.addEventListener('resize', reposition, { passive: true, signal: controller.signal });
            repositionControllers.set(this, controller);

            reposition();
        };
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

        const target = this.getAttribute('target');
        if (target && target.toLowerCase() !== '_self') {
            this.internalProxyAnchor.click();
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