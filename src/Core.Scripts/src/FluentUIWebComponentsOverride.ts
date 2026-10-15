
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
             * the trigger is disabled. A `disabled-focusable` trigger stays focusable and still receives these events,
             * so the trigger handlers of each menu ignore them while the trigger is disabled.
             */
            const menu = await registry.whenDefined('fluent-menu');
            const addTriggerListeners = menu.prototype.addTriggerListeners;
            menu.prototype.addTriggerListeners = function (this: Menu): void {
                ignoreDisabledTriggerEvents(this);
                addTriggerListeners.call(this);
            };

            // Menus upgraded before this override (prerendered markup) already listen to their trigger: set them up again.
            document.querySelectorAll<Menu>('fluent-menu').forEach(element => {
                if (element.slottedTriggers?.length) {
                    element.slottedTriggersChanged(undefined, element.slottedTriggers);
                }
            });
        });
    }

    /**
     * The menus whose trigger handlers already ignore the events of a disabled trigger.
     */
    const menusIgnoringDisabledTrigger = new WeakSet<Menu>();

    /**
     * Wraps the handlers that the menu attaches to its trigger, so they do nothing while the trigger is disabled.
     * These handlers are instance fields (arrow functions), so they are wrapped on each menu, before being attached.
     * @param menu The fluent-menu element.
     */
    function ignoreDisabledTriggerEvents(menu: Menu): void {
        if (menusIgnoringDisabledTrigger.has(menu)) {
            return;
        }
        menusIgnoringDisabledTrigger.add(menu);

        const { toggleMenu, openMenu, triggerKeydownHandler } = menu;

        menu.toggleMenu = (e?: Event) => {
            if (!isFromDisabledTrigger(menu, e)) {
                toggleMenu();
            }
        };

        menu.openMenu = (e?: Event) => {
            if (!isFromDisabledTrigger(menu, e)) {
                openMenu(e);
            }
        };

        menu.triggerKeydownHandler = (e: KeyboardEvent) => {
            if (!isFromDisabledTrigger(menu, e)) {
                return triggerKeydownHandler(e);
            }
        };
    }

    /**
     * Gets whether a trigger handler is called by an event of the trigger while the trigger is disabled.
     * Calls from code (`menu.openMenu()`) pass no event, so they still open the menu,
     * for instance a loading button that renders both `disabled` and `disabled-focusable`.
     * @param menu The fluent-menu element.
     * @param e The event received by the handler, if any.
     */
    function isFromDisabledTrigger(menu: Menu, e?: Event): boolean {
        return e !== undefined && !!menu['_trigger']?.matches('[disabled], [disabled-focusable]');
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