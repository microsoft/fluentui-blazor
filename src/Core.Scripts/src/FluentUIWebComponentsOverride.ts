
/// <reference path="./d-ts/Blazor.d.ts" />

export namespace Microsoft.FluentUI.Override {

    /**
     * Override the default behavior of the Fluent UI Web Components to use Blazor's features.
    */
    export function overrideComponents() {

        /* Original code: https://github.com/microsoft/fluentui/blob/master/packages/web-components/src/anchor-button/anchor-button.base.ts

           private handleNavigation(newTab: boolean): void {
             newTab ? window.open(this.href, '_blank') : this.internalProxyAnchor.click();
           }
        */

        customElements.whenDefined('fluent-anchor-button').then(() => {
            const proto = customElements.get('fluent-anchor-button').prototype;
            proto.handleNavigation = function (newTab: boolean): void {
                if (newTab) {
                    window.open(this.href, '_blank');
                    return;
                }

                const url = new URL(this.href, document.baseURI);
                if (url.origin === location.origin) {
                    Blazor.navigateTo(url.pathname + url.search + url.hash, false);
                }
                else {
                    this.internalProxyAnchor.click()
                    //location.href = this.href;
                }
            };
        });
    }
}