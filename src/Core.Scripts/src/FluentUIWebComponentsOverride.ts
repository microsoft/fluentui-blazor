
/// <reference path="./d-ts/Blazor.d.ts" />
import type { Menu } from "@fluentui/web-components";

export namespace Microsoft.FluentUI.Override {
    interface NavigationElement extends HTMLElement {
        href: string;
        internalProxyAnchor: { click(): void };
        handleNavigation(newTab: boolean): void;
    }

    interface TooltipElement extends HTMLElement {
        showPopover(): void;
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
             * The menu opens from its trigger (click, Enter/Space keydown, hover, context menu) without checking whether
             * the trigger is disabled. A `disabled-focusable` trigger stays focusable and still receives these events.
             * Every way of opening shows the menu list as a popover, so its cancelable `beforetoggle` event is where
             * the menu is kept closed while its trigger is disabled.
             */
            const menu = await registry.whenDefined('fluent-menu');

            // The browser keeps the connectedCallback it found when the element was defined, so replacing it now
            // would not run. The menu's connectedCallback calls setComponent, a public method that Fluent kept
            // (as a no-op) for backward compatibility, so it is where each menu connected from now on is set up.
            const setComponent = menu.prototype.setComponent;
            menu.prototype.setComponent = function (this: Menu): void {
                setComponent.call(this);
                keepClosedWhileTriggerDisabled(this);
            };

            // Menus connected before this override (prerendered markup) did not go through it.
            document.querySelectorAll<Menu>('fluent-menu').forEach(keepClosedWhileTriggerDisabled);
        });
    }

    /**
     * Keeps the menu closed while its trigger is disabled.
     * The listener is on the menu itself, so it goes away with the menu, and adding it again does nothing.
     * `beforetoggle` does not bubble, but the capture phase still reaches the menu from its menu list.
     * @param menu The fluent-menu element.
     */
    function keepClosedWhileTriggerDisabled(menu: Menu): void {
        menu.addEventListener('beforetoggle', cancelOpeningFromDisabledTrigger, { capture: true });
    }

    /**
     * Cancels the opening of the menu's own menu list (not of a submenu) while the menu's trigger is disabled.
     * Closing is never cancelled, so a menu that was open when its trigger got disabled can still be closed.
     * @param e The `beforetoggle` event.
     */
    function cancelOpeningFromDisabledTrigger(e: Event): void {
        const menu = e.currentTarget as Menu;
        const target = e.target as Element;
        if ((e as ToggleEvent).newState !== 'open' || target.parentElement !== menu) {
            return;
        }

        if (getTrigger(menu)?.matches('[disabled], [disabled-focusable]')) {
            e.preventDefault();
        }
    }

    /**
     * Gets the menu's trigger: the element in its `trigger` slot or,
     * for a menu opened by an element elsewhere (`FluentMenu.Trigger`), the element whose id is in its `trigger` attribute.
     * @param menu The fluent-menu element.
     */
    function getTrigger(menu: Menu): Element | null {
        const triggerId = menu.getAttribute('trigger');
        return triggerId
            ? document.getElementById(triggerId)
            : menu.querySelector(':scope > [slot="trigger"]');
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