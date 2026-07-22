import { expect, test } from "@playwright/test";
import { readFile } from "node:fs/promises";
import path from "node:path";
import { fileURLToPath } from "node:url";

const testDirectory = path.dirname(fileURLToPath(import.meta.url));
const anchoredRegionPath = path.resolve(testDirectory, "../../Core/Components/AnchoredRegion/FluentAnchoredRegion.razor.js");
const fluentWebComponentsPath = path.resolve(testDirectory, "../node_modules/@fluentui/web-components/dist/web-components.min.js");

async function loadAnchoredRegionModule(page) {
    const moduleSource = await readFile(anchoredRegionPath, "utf8");

    await page.goto("about:blank");
    await page.addScriptTag({ path: fluentWebComponentsPath, type: "module" });
    await page.evaluate(async source => {
        const moduleUrl = URL.createObjectURL(new Blob([source], { type: "text/javascript" }));
        globalThis.anchoredRegionModule = await import(moduleUrl);
        URL.revokeObjectURL(moduleUrl);
    }, moduleSource);
}

test.beforeEach(async ({ page }) => {
    await loadAnchoredRegionModule(page);
});

test("Tab from a menu anchored to a Fluent element moves focus after the anchor", async ({ page }) => {
    const result = await page.evaluate(async () => {
        await customElements.whenDefined("fluent-button");
        document.body.innerHTML = `
            <button id="first">First</button>
            <fluent-button id="anchor" tabindex="0">Anchor</fluent-button>
            <button id="next">Next</button>
            <div id="popup"><button id="menu-item">Menu item</button></div>`;

        await document.getElementById("anchor").updateComplete;

        let closeCalls = 0;
        anchoredRegionModule.initializeKeyboardNavigation("anchor", "popup", {
            invokeMethodAsync() {
                closeCalls++;
            }
        }, undefined, true);

        const menuItem = document.getElementById("menu-item");
        menuItem.focus();
        menuItem.dispatchEvent(new KeyboardEvent("keydown", {
            bubbles: true,
            composed: true,
            key: "Tab",
            keyCode: 9
        }));

        return { activeElementId: document.activeElement.id, closeCalls };
    });

    expect(result).toEqual({ activeElementId: "next", closeCalls: 1 });
});

test("Tab recognizes the last popup control focused through a Fluent shadow root", async ({ page }) => {
    const result = await page.evaluate(() => {
        class FluentControlElement extends HTMLElement {
            constructor() {
                super();
                const shadowRoot = this.attachShadow({ mode: "open" });
                shadowRoot.innerHTML = "<button id=\"shadow-control\">Popup control</button>";
            }
        }

        customElements.define("fluent-control-test", FluentControlElement);
        document.body.innerHTML = `
            <button id="first">First</button>
            <button id="anchor">Anchor</button>
            <button id="next">Next</button>
            <div id="popup"><fluent-control-test id="control"></fluent-control-test></div>`;

        let closeCalls = 0;
        anchoredRegionModule.initializeKeyboardNavigation("anchor", "popup", {
            invokeMethodAsync() {
                closeCalls++;
            }
        });

        const control = document.getElementById("control");
        const shadowControl = control.shadowRoot.getElementById("shadow-control");
        shadowControl.focus();
        shadowControl.dispatchEvent(new KeyboardEvent("keydown", {
            bubbles: true,
            composed: true,
            key: "Tab",
            keyCode: 9
        }));

        return {
            activeElementId: document.activeElement.id,
            closeCalls
        };
    });

    expect(result).toEqual({ activeElementId: "next", closeCalls: 1 });
});

test("Tab recognizes a focused shadow control represented by its focusable Fluent host", async ({ page }) => {
    const result = await page.evaluate(() => {
        class FluentFocusableControlElement extends HTMLElement {
            constructor() {
                super();
                const shadowRoot = this.attachShadow({ mode: "open" });
                shadowRoot.innerHTML = "<button id=\"shadow-control\">Popup control</button>";
            }
        }

        customElements.define("fluent-focusable-control-test", FluentFocusableControlElement);
        document.body.innerHTML = `
            <button id="first">First</button>
            <button id="anchor">Anchor</button>
            <button id="next">Next</button>
            <div id="popup"><fluent-focusable-control-test id="control" tabindex="0"></fluent-focusable-control-test></div>`;

        let closeCalls = 0;
        anchoredRegionModule.initializeKeyboardNavigation("anchor", "popup", {
            invokeMethodAsync() {
                closeCalls++;
            }
        });

        const control = document.getElementById("control");
        const shadowControl = control.shadowRoot.getElementById("shadow-control");
        shadowControl.focus();
        shadowControl.dispatchEvent(new KeyboardEvent("keydown", {
            bubbles: true,
            composed: true,
            key: "Tab",
            keyCode: 9
        }));

        return { activeElementId: document.activeElement.id, closeCalls };
    });

    expect(result).toEqual({ activeElementId: "next", closeCalls: 1 });
});

test("Tab enters a popup control nested in open shadow roots", async ({ page }) => {
    const activeElementId = await page.evaluate(() => {
        class FluentNestedControlElement extends HTMLElement {
            constructor() {
                super();
                const shadowRoot = this.attachShadow({ mode: "open" });
                shadowRoot.innerHTML = "<button id=\"nested-control\">Popup control</button>";
            }
        }

        class FluentOuterControlElement extends HTMLElement {
            constructor() {
                super();
                const shadowRoot = this.attachShadow({ mode: "open" });
                shadowRoot.innerHTML = "<fluent-nested-control-test></fluent-nested-control-test>";
            }
        }

        customElements.define("fluent-nested-control-test", FluentNestedControlElement);
        customElements.define("fluent-outer-control-test", FluentOuterControlElement);
        document.body.innerHTML = `
            <button id="anchor">Anchor</button>
            <div id="popup"><fluent-outer-control-test></fluent-outer-control-test></div>`;

        anchoredRegionModule.initializeKeyboardNavigation("anchor", "popup", {
            invokeMethodAsync() { }
        });

        const anchor = document.getElementById("anchor");
        anchor.focus();
        anchor.dispatchEvent(new KeyboardEvent("keydown", {
            bubbles: true,
            key: "Tab",
            keyCode: 9
        }));

        let activeElement = document.activeElement;
        while (activeElement?.shadowRoot?.activeElement) {
            activeElement = activeElement.shadowRoot.activeElement;
        }

        return activeElement?.id;
    });

    expect(activeElementId).toBe("nested-control");
});

test("Tab exits after the focusable descendant of a composite anchor", async ({ page }) => {
    const result = await page.evaluate(async () => {
        await customElements.whenDefined("fluent-button");
        document.body.innerHTML = `
            <button id="first">First</button>
            <div id="anchor"><fluent-button id="trigger">Anchor</fluent-button></div>
            <button id="next">Next</button>
            <div id="popup"><button id="menu-item">Menu item</button></div>`;

        await document.getElementById("trigger").updateComplete;

        let closeCalls = 0;
        anchoredRegionModule.initializeKeyboardNavigation("anchor", "popup", {
            invokeMethodAsync() {
                closeCalls++;
            }
        }, undefined, true);

        const menuItem = document.getElementById("menu-item");
        menuItem.focus();
        menuItem.dispatchEvent(new KeyboardEvent("keydown", {
            bubbles: true,
            composed: true,
            key: "Tab",
            keyCode: 9
        }));

        return { activeElementId: document.activeElement.id, closeCalls };
    });

    expect(result).toEqual({ activeElementId: "next", closeCalls: 1 });
});

test("Shift+Tab returns focus to the control inside a focusable composite anchor", async ({ page }) => {
    const result = await page.evaluate(() => {
        class FocusableAnchorElement extends HTMLElement {
            constructor() {
                super();
                const shadowRoot = this.attachShadow({ mode: "open" });
                shadowRoot.innerHTML = "<button id=\"anchor-control\">Anchor</button>";
            }
        }

        customElements.define("focusable-anchor-test", FocusableAnchorElement);
        document.body.innerHTML = `
            <focusable-anchor-test id="anchor" tabindex="0"></focusable-anchor-test>
            <div id="popup"><button id="menu-item">Menu item</button></div>`;

        let closeCalls = 0;
        anchoredRegionModule.initializeKeyboardNavigation("anchor", "popup", {
            invokeMethodAsync() {
                closeCalls++;
            }
        }, undefined, true);

        const menuItem = document.getElementById("menu-item");
        menuItem.focus();
        menuItem.dispatchEvent(new KeyboardEvent("keydown", {
            bubbles: true,
            composed: true,
            key: "Tab",
            keyCode: 9,
            shiftKey: true
        }));

        let activeElement = document.activeElement;
        while (activeElement?.shadowRoot?.activeElement) {
            activeElement = activeElement.shadowRoot.activeElement;
        }

        return { activeElementId: activeElement?.id, closeCalls };
    });

    expect(result).toEqual({ activeElementId: "anchor-control", closeCalls: 1 });
});

test("Shift+Tab falls back to the anchor control when host focus does not move focus", async ({ page }) => {
    const result = await page.evaluate(() => {
        class UnfocusableAnchorElement extends HTMLElement {
            constructor() {
                super();
                const shadowRoot = this.attachShadow({ mode: "open" });
                shadowRoot.innerHTML = "<button id=\"anchor-control\">Anchor</button>";
            }

            focus() {
            }
        }

        customElements.define("unfocusable-anchor-test", UnfocusableAnchorElement);
        document.body.innerHTML = `
            <unfocusable-anchor-test id="anchor" tabindex="0"></unfocusable-anchor-test>
            <div id="popup"><button id="menu-item">Menu item</button></div>`;

        let closeCalls = 0;
        anchoredRegionModule.initializeKeyboardNavigation("anchor", "popup", {
            invokeMethodAsync() {
                closeCalls++;
            }
        }, undefined, true);

        const menuItem = document.getElementById("menu-item");
        menuItem.focus();
        menuItem.dispatchEvent(new KeyboardEvent("keydown", {
            bubbles: true,
            composed: true,
            key: "Tab",
            keyCode: 9,
            shiftKey: true
        }));

        let activeElement = document.activeElement;
        while (activeElement?.shadowRoot?.activeElement) {
            activeElement = activeElement.shadowRoot.activeElement;
        }

        return { activeElementId: activeElement?.id, closeCalls };
    });

    expect(result).toEqual({ activeElementId: "anchor-control", closeCalls: 1 });
});

test("FocusableElement does not wrap when the current element is outside its focus list", async ({ page }) => {
    const nextElementId = await page.evaluate(() => {
        document.body.innerHTML = `
            <button id="first">First</button>
            <button id="second">Second</button>`;

        const elementOutsideDocument = document.createElement("button");
        return new anchoredRegionModule.FocusableElement(document)
            .findNextFocusableElement(elementOutsideDocument)?.id ?? null;
    });

    expect(nextElementId).toBeNull();
});