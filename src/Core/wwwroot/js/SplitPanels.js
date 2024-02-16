function fireEvent(element, eventName, detail) {
    const event = new CustomEvent(eventName, { detail, bubbles: true, cancelable: true })
    return element.dispatchEvent(event);
}

const styleString = `
    :host{ display: grid; }
    :host([resizing]){ user-select: none; }
    :host([resizing][direction=row]){ cursor: col-resize; }
    :host([direction=row]) { grid-template-columns: var(--first-size, 1fr) max-content var(--second-size, 1fr); }
    :host([direction=row]) #median { grid-column: 2 / 3; }
    :host([direction=row]) #median:hover { cursor: col-resize; }
    :host([direction=row]) #median span[part="handle"] { height: 16px; margin: 2px 0;}
    :host([direction=row]) #slot1 { grid-column: 1 / 2; grid-row: 1 / 1; }
    :host([direction=row]) #slot2 { grid-column: 3 / 4; grid-row: 1 / 1; }

    :host([resizing][direction=col]){ cursor: row-resize; }
    :host([direction=column]) { grid-template-rows: var(--first-size, 1fr) max-content var(--second-size, 1fr); }
    :host([direction=column]) #median { grid-row: 2 / 3; }
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

    :host([collapsed]) { grid-template-columns: 1fr !important; grid-template-rows: none !important; }
    :host([collapsed]) #median { display: none; }
    :host([collapsed]) #slot2 { display: none; }
`;

const template = `
    <slot id="slot1" name="1"></slot>
    <div id="median" part="median">
        <span part="handle"></span>
    </div>
    <slot id="slot2" name="2"></slot>
`;

class SplitPanels extends HTMLElement {
    static observedAttributes = ["direction", "collapsed", "barsize", "slot1minsize", "slot2minsize"];
    #direction = "row";
    #isResizing = false;
    #collapsed = false;
    #barsize;
    #slot1size;
    #slot2size;
    #slot1minsize;
    #slot2minsize;
    #totalsize;

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
        if (document.adoptedStyleSheets) {
            const shadow = this.attachShadow({ mode: "open" });
            const styleSheet = new CSSStyleSheet();
            styleSheet.replaceSync(styleString);
            shadow.adoptedStyleSheets.push(styleSheet);
            shadow.innerHTML = template;
        }
        else {
            var style = document.createElement('style');
            style.type = 'text/css';
            style.innerHTML = styleString;
            document.getElementsByTagName('head')[0].appendChild(style);
        }

        this.updateBarSizeStyle();
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
        this.#totalsize = this.direction === "row" ? clientRect.width : clientRect.height;

        this.addEventListener("pointermove", this.resizeDrag);
        this.addEventListener("pointerup", this.pointerup);
    }
    pointerup() {
        this.isResizing = false;
        fireEvent(this, "splitterresized", {panel1size: this.#slot1size, panel2size: this.#slot2size});
        this.removeEventListener("pointermove", this.resizeDrag);
        this.removeEventListener("pointerup", this.pointerup);
    }
    resizeDrag(e) {
        if (this.direction === "row") {
            const newMedianLeft = e.clientX - this.left;
            const median = this.barsize;

            this.#slot1size = Math.floor(newMedianLeft - (median / 2));
            this.#slot2size = Math.floor(this.clientWidth - this.#slot1size - (median / 2));

            let min1size = this.ensurevalue(this.slot1minsize);
            if (this.#slot1size < min1size) {
                this.#slot1size = Math.floor(min1size);
                this.#slot2size = Math.floor(this.clientWidth - this.#slot1size - (median / 2));
            }
            let min2size = this.ensurevalue(this.slot2minsize);
            if (this.#slot2size < min2size) {
                this.#slot2size = Math.floor(min2size);
                this.#slot1size = Math.floor(this.clientWidth - this.#slot2size - (median / 2));
            }

            const totalSize = this.#slot1size + this.#slot2size - median;
            let slot1fraction = (this.#slot1size / totalSize).toFixed(2);
            let slot2fraction = (this.#slot2size / totalSize).toFixed(2);

            this.style.gridTemplateColumns = `${slot1fraction}fr ${median}px ${slot2fraction}fr`;
        }
        if (this.direction === "column") {
            const newMedianTop = e.clientY - this.top;
            const median = this.barsize;

            this.#slot1size = Math.floor(newMedianTop - (median / 2));
            this.#slot2size = Math.floor(this.clientHeight - this.#slot1size - (median / 2));

            let min1size = this.ensurevalue(this.slot1minsize);
            if (this.#slot1size < min1size) {
                this.#slot1size = Math.floor(min1size);
                this.#slot2size = Math.floor(this.clientHeight - this.#slot1size - (median / 2));
            }
            let min2size = this.ensurevalue(this.slot2minsize);
            if (this.#slot2size < min2size) {
                this.#slot2size = Math.floor(min2size);
                this.#slot1size = Math.floor(this.clientHeight - this.#slot2size - (median / 2));
            }

            const totalSize = this.#slot1size + this.#slot2size - median;
            let slot1fraction = (this.#slot1size / totalSize).toFixed(2);
            let slot2fraction = (this.#slot2size / totalSize).toFixed(2);

            this.style.gridTemplateRows = `${slot1fraction}fr ${median}px ${slot2fraction}fr`;
        }
    }
    updateBarSizeStyle() {
        let median = this.shadowRoot?.querySelector('#median');

        if (median) {
            if (this.direction === "row") {
                median.style.inlineSize = this.barsize + 'px';
                median.style.blockSize = null;
            }
            else {
                median.style.blockSize = this.barsize + 'px';
                median.style.inlineSize = null;
            }
        }
    }
    attributeChangedCallback(name, oldValue, newValue) {
        if (newValue != oldValue) {
            this[name] = newValue;
        }
    }
    ensurevalue(value) {
    if (!value)
        return null;

    value = value.trim().toLowerCase();

    if (value.endsWith("%"))
        return this.#totalsize * parseFloat(value) / 100;

    if (value.endsWith("px"))
        return parseFloat(value);

    if (value.endsWith("fr"))
        return this.#totalsize * parseFloat(value)

    return 0;
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
        this.updateBarSizeStyle();
    }
    get direction() {
        return this.#direction;
    }
    set collapsed(value) {
        const realValue = value !== null && value !== undefined && value !== false;
        if (this.#collapsed !== realValue) {
            this.#collapsed = realValue;
            if (this.#collapsed) {
                this.setAttribute("collapsed", "");
            } else {
                this.removeAttribute("collapsed");
            }
            fireEvent(this, "splittercollapsed", { collapsed: this.#collapsed });
        }
    }
    get collapsed() {
        return this.#collapsed;
    }

    set slot1minsize(value) {
        this.#slot1minsize = value ?? 0;
    }
    get slot1minsize() {
        return this.#slot1minsize;
    }

    set slot2minsize(value) {
        this.#slot2minsize = value ?? 0;
    }
    get slot2minsize() {
        return this.#slot2minsize;
    }
    set barsize(value) {
        this.#barsize = value;
        this.updateBarSizeStyle();
    }
    get barsize() {
        return this.#barsize;
    }
}

export { SplitPanels };
