export function addAriaHidden(id) {
    const fieldElement = document.querySelector("#" + id)?.shadowRoot?.querySelector(".clear-button");

    if (!!fieldElement) {
        fieldElement?.setAttribute("aria-hidden", "true");
    }
}

export function setControlAttribute(id, attrName, value) {
    const fieldElement = document.querySelector("#" + id)?.shadowRoot?.querySelector("#control");

    if (!!fieldElement) {
        fieldElement?.setAttribute(attrName, value);
    }
}

export function setDataList(id, datalistid) {
    const fieldElement = document.getElementById(id);
    const dataList = document.getElementById(datalistid)?.cloneNode(true);

    const shadowRoot = fieldElement.shadowRoot;
    const shadowDataList = shadowRoot.getElementById(datalistid);
    if (shadowDataList) {
        shadowRoot.removeChild(shadowDataList);
    }
    shadowRoot.appendChild(dataList);
}
