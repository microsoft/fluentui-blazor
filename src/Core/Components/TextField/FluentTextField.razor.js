export function setAutocomplete(id, value) {
    const fieldElement = document.querySelector("#" + id)?.shadowRoot?.querySelector("#control");

    if (!!fieldElement) {
        fieldElement?.setAttribute("autocomplete", value);
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