function fireEvent(element, eventName) {
    const event = new Event(eventName, { bubbles: true, cancelable: true })
    return element.dispatchEvent(event);
}

class SplitPanels extends HTMLElement {
    static observedAttributes = ["direction"];
    #direction = "row";
    #isResizing = false;
    constructor() {
        super();
        this.bind(this);
    }
    bind(element) {
        element.attachEvents = element.attachEvents.bind(element);
        element.render = element.render.bind(element);
        element.cacheDom = element.cacheDom.bind(element);
        element.pointerdown = element.pointerdown.bind(element);
        element.resizeDrag = element.resizeDrag.bind(element);
    }
    render() {
        this.attachShadow({ mode: "open" });
        this.shadowRoot.innerHTML = `
            <style>
                :host{ display: grid; }
                :host([resizing]){ user-select: none; }
                :host([resizing][direction=row]){ cursor: col-resize; }
                :host([direction=row]) { grid-template-columns: var(--first-size, 1fr) max-content var(--second-size, 1fr); }
                :host([direction=row]) #median { inline-size: 0.5rem; grid-column: 2 / 3; }
                :host([direction=row]) #median:hover { cursor: col-resize; }
                :host([direction=row]) #median span[part="handle"] { height: 16px; margin: 2px 0;}
                :host([direction=row]) #slot1 { grid-column: 1 / 2; grid-row: 1 / 1; }
                :host([direction=row]) #slot2 { grid-column: 3 / 4; grid-row: 1 / 1; }

                :host([resizing][direction=col]){ cursor: row-resize; }
                :host([direction=column]) { grid-template-rows: var(--first-size, 1fr) max-content var(--second-size, 1fr); }
                :host([direction=column]) #median { block-size: 0.5rem; grid-row: 2 / 3; }
                :host([direction=column]) #median:hover { cursor: row-resize; }
                :host([direction=column]) #median span[part="handle"] { width: 16px; margin: 0 2px;}
                :host([direction=column]) #slot1 { grid-row: 1 / 2; grid-column: 1 / 1; }
                :host([direction=column]) #slot2 { grid-row: 3 / 4; grid-column: 1 / 1; }

                #median { background: var(--neutral-stroke-rest); display: inline-flex; align-items:center; justify-content: center; }
                #median:hover { background: var(--neutral-stroke-hover); }
                #median:active { background: var(--neutral-stroke-active); }
                #median:focus { background: var(--neutral-stroke-focus); }
                
                #median span[part="handle"] {  border: 1px solid var(--neutral-stroke-strong-rest); border-radius: 1px; }

                ::slotted(*) { overflow: auto; }
            </style>
            <slot id="slot1" name="1"></slot>
            <div id="median" part="median">
                <span part="handle"></span>
            </div>
            <slot id="slot2" name="2"></slot>
        `;
    }
    connectedCallback() {
        this.render();
        this.cacheDom();
        this.attachEvents();
    }
    cacheDom() {
        this.dom = {
            median: this.shadowRoot.querySelector("#median")
        };
    }
    attachEvents() {
        this.dom.median.addEventListener("pointerdown", this.pointerdown);
    }
    pointerdown(e) {
        this.isResizing = true;
        const clientRect = this.getBoundingClientRect();
        this.left = clientRect.x;
        this.top = clientRect.y;
        this.addEventListener("pointermove", this.resizeDrag);
        this.addEventListener("pointerup", this.pointerup);
    }
    pointerup() {
        this.isResizing = false;
        fireEvent(this, "sizechanged");
        this.removeEventListener("pointermove", this.resizeDrag);
        this.removeEventListener("pointerup", this.pointerup);
    }
    resizeDrag(e) {
        if (this.direction === "row") {
            const newMedianLeft = e.clientX - this.left;
            const median = this.dom.median.getBoundingClientRect().width;
            this.style.gridTemplateColumns = `calc(${newMedianLeft}px - ${median / 2}px) ${median}px 1fr`;
        }
        if (this.direction === "column") {
            const newMedianTop = e.clientY - this.top;
            const median = this.dom.median.getBoundingClientRect().height;
            this.style.gridTemplateRows = `calc(${newMedianTop}px - ${median / 2}px) ${median}px 1fr`;
        }
    }
    attributeChangedCallback(name, oldValue, newValue) {
        if (newValue != oldValue) {
            this[name] = newValue;
        }
    }
    set isResizing(value) {
        this.#isResizing = value;
        if (value) {
            this.setAttribute("resizing", "");
        } else {
            this.style.userSelect = "";
            this.style.cursor = "";
            this.removeAttribute("resizing");
        }
    }
    get isResizing() {
        return this.#isResizing;
    }
    set direction(value) {
        this.#direction = value;
        this.setAttribute("direction", value);
        this.style.gridTemplateRows = "";
        this.style.gridTemplateColumns = "";
    }
    get direction() {
        return this.#direction;
    }
}

export { SplitPanels };
