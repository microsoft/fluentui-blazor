
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

        // Listeners and observers of each open dropdown, removed when the dropdown closes.
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

            // The inline max-height (e.g. FluentSelect.Height) is the upper limit of the listbox height; 50vh by default.
            const configuredMaxHeight = listbox.style.getPropertyValue('max-height');
            const maxHeightLimit = configuredMaxHeight || '50vh';

            const controller = new AbortController();

            // Height the listbox would have with its configured max-height only (not limited by the available space),
            // measured without changing its styles, so the scroll position of the listbox is preserved.
            const measureListboxHeight = (): number => {
                const style = getComputedStyle(listbox);
                const paddingTop = parseFloat(style.paddingTop) || 0;
                const paddingBottom = parseFloat(style.paddingBottom) || 0;
                const borderTop = parseFloat(style.borderTopWidth) || 0;
                const borderBottom = parseFloat(style.borderBottomWidth) || 0;

                // `scrollHeight` is the full height of the options, padding included.
                const height = style.boxSizing === 'border-box'
                    ? listbox.scrollHeight + borderTop + borderBottom
                    : listbox.scrollHeight - paddingTop - paddingBottom;

                // The limit can be any CSS max-height value (px, vh, %, calc(), none...): a hidden probe with the
                // same box model resolves it, without touching the listbox.
                const probe = document.createElement('div');
                probe.style.cssText = 'position: fixed; visibility: hidden; pointer-events: none; top: 0; left: 0; width: 0;';
                probe.style.setProperty('box-sizing', style.boxSizing);
                probe.style.setProperty('padding', `${paddingTop}px 0 ${paddingBottom}px`);
                probe.style.setProperty('border-style', 'solid');
                probe.style.setProperty('border-width', `${borderTop}px 0 ${borderBottom}px`);
                probe.style.setProperty('height', `${Math.max(height, 0)}px`);
                probe.style.setProperty('max-height', maxHeightLimit);
                document.body.appendChild(probe);
                const measuredHeight = probe.getBoundingClientRect().height;
                probe.remove();

                return measuredHeight;
            };

            // Place the listbox below the field, or above it when there is not enough room below,
            // replacing the CSS anchor rules of the listbox (position-anchor, anchor(), anchor-size()).
            const reposition = () => {
                // A dropdown removed while open is closed without `openChanged`: stop following the page.
                if (!this.isConnected) {
                    controller.abort();
                    repositionControllers.delete(this);
                    return;
                }

                const field = this.getBoundingClientRect();
                const viewportWidth = document.documentElement.clientWidth;

                const listboxHeight = measureListboxHeight();
                const listboxWidth = Math.max(listbox.getBoundingClientRect().width, field.width);

                const spaceBelow = window.innerHeight - field.bottom;
                const spaceAbove = field.top;
                const openAbove = spaceBelow < listboxHeight && spaceAbove > spaceBelow;
                const availableSpace = Math.max(openAbove ? spaceAbove : spaceBelow, 0);

                // Align the listbox with the inline-start edge of the field (right edge in RTL),
                // or with the opposite edge when it would overflow the viewport.
                const isRtl = getComputedStyle(this).direction === 'rtl';
                const fitsFromLeft = field.left + listboxWidth <= viewportWidth;
                const fitsFromRight = field.right - listboxWidth >= 0;
                const alignRight = isRtl ? fitsFromRight || !fitsFromLeft : !fitsFromLeft && fitsFromRight;

                const styles: Record<string, string> = {
                    'position-anchor': 'none',
                    'position-try-fallbacks': 'none',
                    'max-height': `min(${maxHeightLimit}, ${availableSpace}px)`,
                    'min-width': `${field.width}px`,
                    'left': alignRight ? 'auto' : `${field.left}px`,
                    'right': alignRight ? `${viewportWidth - field.right}px` : 'auto',
                    'top': openAbove ? 'auto' : `${field.bottom}px`,
                    'bottom': openAbove ? `${window.innerHeight - field.top}px` : 'auto',
                };

                Object.entries(styles).forEach(([name, value]) => {
                    if (listbox.style.getPropertyValue(name) !== value) {
                        listbox.style.setProperty(name, value);
                    }
                });
            };

            // Observer callbacks are deferred to the next frame, so the size changes made by `reposition`
            // are not reported in the same frame (no "ResizeObserver loop" error) and bursts are merged.
            let frame = 0;
            const scheduleReposition = () => {
                cancelAnimationFrame(frame);
                frame = requestAnimationFrame(reposition);
            };

            // Without CSS anchor, the listbox must follow the field when the page scrolls or is resized,
            // when the field or the listbox is resized, and when the options change.
            // Scrolling inside the listbox does not move it: it is ignored, so its scroll position is never disturbed.
            const onScroll = (e: Event) => {
                if (!(e.target instanceof Node && listbox.contains(e.target))) {
                    scheduleReposition();
                }
            };
            window.addEventListener('scroll', onScroll, { passive: true, capture: true, signal: controller.signal });
            window.addEventListener('resize', scheduleReposition, { passive: true, signal: controller.signal });

            const resizeObserver = new ResizeObserver(scheduleReposition);
            resizeObserver.observe(this);
            resizeObserver.observe(listbox);

            const optionsObserver = new MutationObserver(scheduleReposition);
            optionsObserver.observe(listbox, { childList: true, subtree: true, characterData: true });

            controller.signal.addEventListener('abort', () => {
                cancelAnimationFrame(frame);
                resizeObserver.disconnect();
                optionsObserver.disconnect();

                // Restore the configured max-height, so it is measured again at the next opening.
                if (configuredMaxHeight) {
                    listbox.style.setProperty('max-height', configuredMaxHeight);
                } else {
                    listbox.style.removeProperty('max-height');
                }
            });

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