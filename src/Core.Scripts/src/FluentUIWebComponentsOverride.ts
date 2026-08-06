
/// <reference path="./d-ts/Blazor.d.ts" />

export namespace Microsoft.FluentUI.Override {
    /**
     * Override the default behavior of the Fluent UI Web Components to use Blazor's features.
    */
    export function overrideComponents() {

        /**
         * fluent-anchor-button
         */
        waitForCustomElements().then(async registry => {
            const anchorButton = await registry.whenDefined('fluent-anchor-button');
            const proto = anchorButton.prototype;

            /* 
                Original code: https://github.com/microsoft/fluentui/blob/master/packages/web-components/src/anchor-button/anchor-button.base.ts

                private handleNavigation(newTab: boolean): void {
                    newTab ? window.open(this.href, '_blank') : this.internalProxyAnchor.click();
                }
            */
            proto.handleNavigation = function (newTab: boolean): void {
                if (newTab) {
                    window.open(this.href, '_blank');
                    return;
                }

                const url = new URL(this.href, document.baseURI);
                if (url.origin === location.origin) {
                    const forceLoad = this.getAttribute('force-load') === 'true';
                    console.log(`fluent-anchor-button handleNavigation: navigating to ${url.pathname + url.search + url.hash} using Blazor.navigateTo with forceLoad=${forceLoad}`);
                    Blazor.navigateTo(url.pathname + url.search + url.hash, forceLoad);
                }
                else {                    
                    console.log(`fluent-anchor-button handleNavigation: navigating to ${this.href} using internalProxyAnchor.click()`);
                    this.internalProxyAnchor.click();
                }
            };
        });
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