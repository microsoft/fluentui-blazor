import { DotNet } from "../../d-ts/Microsoft.JSInterop";

export namespace Microsoft.FluentUI.Blazor.Components.MultiSplitter {

  /**
   * Initializes the splitter resize event.
   * @param el Element that contains the splitter.
   * @param splitter The DotNet object that contains the splitter methods.
   * @param paneId
   * @param paneNextId
   * @param orientation
   * @param clientPos
   * @param minValue
   * @param maxValue
   * @param minNextValue
   * @param maxNextValue
   */
  export function StartResize(
    el: HTMLElement,
    splitter: DotNet.DotNetObject,
    paneId: string | null,
    paneNextId: string | null,
    orientation: "horizontal" | "vertical",
    clientPos: number,
    minValue: string | null,
    maxValue: string | null,
    minNextValue: string | null,
    maxNextValue: string | null) {

    //var el = document.getElementById(id);
    var pane: HTMLElement | null = paneId ? document.getElementById(paneId) : null;
    var paneNext: HTMLElement | null = paneNextId ? document.getElementById(paneNextId) : null;
    var paneLength: number | null = null;
    var paneNextLength: number | null = null;
    var panePerc: string | null = null;
    var paneNextPerc: string | null = null;
    var isHOrientation: boolean = orientation == "horizontal";

    var totalLength: number = 0.0;
    Array.from(el.children).forEach(element => {
      totalLength += isHOrientation
        ? element.getBoundingClientRect().width
        : element.getBoundingClientRect().height;
    });

    if (pane) {
      paneLength = isHOrientation
        ? pane.getBoundingClientRect().width
        : pane.getBoundingClientRect().height;

      panePerc = (paneLength / totalLength * 100) + '%';
    }

    if (paneNext) {
      paneNextLength = isHOrientation
        ? paneNext.getBoundingClientRect().width
        : paneNext.getBoundingClientRect().height;

      paneNextPerc = (paneNextLength / totalLength * 100) + '%';
    }

    // Check if the string is a percentage or pixel value
    function ensurevalue(value: string | null): number | null {
      if (!value)
        return null;

      value = value.trim().toLowerCase();

      if (value.endsWith("%"))
        return totalLength * parseFloat(value) / 100;

      if (value.endsWith("px"))
        return parseFloat(value);

      throw "Invalid value";
    }

    const minValueNumber = ensurevalue(minValue) ?? 0;
    const maxValueNumber = ensurevalue(maxValue) ?? 0;
    const minNextValueNumber = ensurevalue(minNextValue) ?? 0;
    const maxNextValueNumber = ensurevalue(maxNextValue) ?? 0;

    // Create a splitterData object if it doesn't exist
    if (!(document as any).splitter || !(document as any).splitter.Data) {
      ((document as any).splitter as SplitterData) = {
        Data: new Map<HTMLElement, SplitterDataItem>()
      };;
    }

    const splitterData = ((document as any).splitter as SplitterData).Data;

    // Save the splitter data in `document.splitter.Data`
    splitterData.set(el, {
      clientPos: clientPos,
      panePercNumber: panePerc ? parseFloat(panePerc) : 0,
      paneNextPercNumber: paneNextPerc ? isFinite(parseFloat(paneNextPerc)) ? parseFloat(paneNextPerc) : 0 : 0,
      paneLength: paneLength ?? 0,
      paneNextLength: paneNextLength ? isFinite(paneNextLength) ? paneNextLength : 0 : 0,

      mouseUpHandler: (e: MouseEvent | TouchEvent) => {
        const splitteDataElement = ((document as any).splitter as SplitterData)?.Data.get(el);

        if (splitteDataElement &&
          pane?.style.flexBasis.includes('%') &&
          paneNext?.style.flexBasis.includes('%')) {

          if (splitteDataElement.moved === true) {
            splitter.invokeMethodAsync(
              'FluentMultiSplitter.OnPaneResizedAsync',
              parseInt(pane?.getAttribute('data-index') ?? ""),
              parseFloat(pane?.style.flexBasis),
              paneNext ? parseInt(paneNext?.getAttribute('data-index') ?? "") : null,
              paneNext ? parseFloat(paneNext?.style.flexBasis) : null
            );
          }

          document.removeEventListener('mousemove', splitteDataElement.mouseMoveHandler);
          document.removeEventListener('mouseup', splitteDataElement.mouseUpHandler);
          document.removeEventListener('touchmove', splitteDataElement.touchMoveHandler);
          document.removeEventListener('touchend', splitteDataElement.mouseUpHandler);
          splitterData.delete(el);
        }

        if (splitteDataElement) {
          splitteDataElement.moved = false;
        }
      },

      mouseMoveHandler: (e: MouseEvent) => {
        const splitteDataElement = ((document as any).splitter as SplitterData)?.Data.get(el);

        if (splitteDataElement && pane) {

          const rtl = window.getComputedStyle(pane as Element)?.getPropertyValue('direction') === 'rtl' ? -1 : 1;
          splitteDataElement.moved = true;

          var spacePerc = splitteDataElement.panePercNumber + splitteDataElement.paneNextPercNumber;
          var spaceLength = splitteDataElement.paneLength + splitteDataElement.paneNextLength;

          var length = isHOrientation
            ? splitteDataElement.paneLength - (splitteDataElement.clientPos - e.clientX) * rtl
            : splitteDataElement.paneLength - (splitteDataElement.clientPos - e.clientY);

          if (length > spaceLength)
            length = spaceLength;

          if (minValue && length < minValueNumber) length = minValueNumber;
          if (maxValue && length > maxValueNumber) length = maxValueNumber;

          if (paneNext) {
            var nextSpace = spaceLength - length;
            if (minNextValue && nextSpace < minNextValueNumber) length = spaceLength - minNextValueNumber;
            if (maxNextValue && nextSpace > maxNextValueNumber) length = spaceLength + maxNextValueNumber;
          }

          var perc = length / splitteDataElement.paneLength;
          if (!isFinite(perc)) {
            perc = 1;
            splitteDataElement.panePercNumber = 0.1;
            splitteDataElement.paneLength = isHOrientation
              ? pane.getBoundingClientRect().width ?? 0
              : pane.getBoundingClientRect().height ?? 0;
          }

          var newPerc = splitteDataElement.panePercNumber * perc;
          if (newPerc < 0) newPerc = 0;
          if (newPerc > 100) newPerc = 100;

          pane.style.flexBasis = newPerc + '%';
          if (paneNext)
            paneNext.style.flexBasis = (spacePerc - newPerc) + '%';
        }
      },

      touchMoveHandler: (e: TouchEvent) => {
        const splitteDataElement = ((document as any).splitter as SplitterData)?.Data.get(el);
        const touch = e.changedTouches[0]

        const eventArgs = new MouseEvent("mouseup", {
          bubbles: true,
          cancelable: true,
          view: window,
          clientX: touch.clientX,
          clientY: touch.clientY,
          screenX: touch.screenX,
          screenY: touch.screenY,
          button: 0 // Left click
        });

        if (splitteDataElement) {
          splitteDataElement.mouseMoveHandler(eventArgs);
        }
      }
    });

    // Add the event listeners
    const splitterDataElement = splitterData.get(el);
    if (splitterDataElement) {
      document.addEventListener('mousemove', splitterDataElement.mouseMoveHandler);
      document.addEventListener('mouseup', splitterDataElement.mouseUpHandler);
      document.addEventListener('touchmove', splitterDataElement.touchMoveHandler, { passive: true });
      document.addEventListener('touchend', splitterDataElement.mouseUpHandler, { passive: true });
    }
  }

  interface SplitterData {
    Data: Map<HTMLElement, SplitterDataItem>;
  }

  interface SplitterDataItem {
    clientPos: number;
    panePercNumber: number;
    paneNextPercNumber: number;
    paneLength: number;
    paneNextLength: number;
    mouseMoveHandler: (e: MouseEvent) => void;
    touchMoveHandler: (e: TouchEvent) => void;
    mouseUpHandler: (e: MouseEvent | TouchEvent) => void;
    moved?: boolean;
  }

}
