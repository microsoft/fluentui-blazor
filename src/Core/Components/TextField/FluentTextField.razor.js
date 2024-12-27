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

export function ensureCurrentValueMatch(ref) {
    if (ref !== undefined && ref != null) {
        const observer = new MutationObserver((mutations) => {
            mutations.forEach((mutation) => {
                if (mutation.type === "attributes") {
                    ref.value = mutation.target.getAttribute("value");
                }
            });
        });
        observer.observe(ref, {
            attributes: true,
            attributeFilter: ["value"],
        });
    }
}

