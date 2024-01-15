export function startSplitterResize(
    id,
    splitter,
    paneId,
    paneNextId,
    orientation,
    clientPos,
    minValue,
    maxValue,
    minNextValue,
    maxNextValue) {

    var el = document.getElementById(id);
    var pane = document.getElementById(paneId);
    var paneNext = document.getElementById(paneNextId);
    var paneLength;
    var paneNextLength;
    var panePerc;
    var paneNextPerc;
    var isHOrientation = orientation == 'Horizontal';

    var totalLength = 0.0;
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

    function ensurevalue(value) {
        if (!value)
            return null;

        value = value.trim().toLowerCase();

        if (value.endsWith("%"))
            return totalLength * parseFloat(value) / 100;

        if (value.endsWith("px"))
            return parseFloat(value);

        throw 'Invalid value';
    }

    minValue = ensurevalue(minValue);
    maxValue = ensurevalue(maxValue);
    minNextValue = ensurevalue(minNextValue);
    maxNextValue = ensurevalue(maxNextValue);

    if (!document.splitterData) {
        document.splitterData = {};
    }

    document.splitterData[el] = {
        clientPos: clientPos,
        panePerc: parseFloat(panePerc),
        paneNextPerc: isFinite(parseFloat(paneNextPerc)) ? parseFloat(paneNextPerc) : 0,
        paneLength: paneLength,
        paneNextLength: isFinite(paneNextLength) ? paneNextLength : 0,
        mouseUpHandler: function (e) {
            if (document.splitterData[el]) {
                splitter.invokeMethodAsync(
                    'FluentMultiSplitter.OnPaneResizedAsync',
                    parseInt(pane.getAttribute('data-index')),
                    parseFloat(pane.style.flexBasis),
                    paneNext ? parseInt(paneNext.getAttribute('data-index')) : null,
                    paneNext ? parseFloat(paneNext.style.flexBasis) : null
                );
                document.removeEventListener('mousemove', document.splitterData[el].mouseMoveHandler);
                document.removeEventListener('mouseup', document.splitterData[el].mouseUpHandler);
                document.removeEventListener('touchmove', document.splitterData[el].touchMoveHandler);
                document.removeEventListener('touchend', document.splitterData[el].mouseUpHandler);
                document.splitterData[el] = null;
            }
        },
        mouseMoveHandler: function (e) {
            if (document.splitterData[el]) {

                var spacePerc = document.splitterData[el].panePerc + document.splitterData[el].paneNextPerc;
                var spaceLength = document.splitterData[el].paneLength + document.splitterData[el].paneNextLength;

                var length = (document.splitterData[el].paneLength -
                    (document.splitterData[el].clientPos - (isHOrientation ? e.clientX : e.clientY)));

                if (length > spaceLength)
                    length = spaceLength;

                if (minValue && length < minValue) length = minValue;
                if (maxValue && length > maxValue) length = maxValue;

                if (paneNext) {
                    var nextSpace = spaceLength - length;
                    if (minNextValue && nextSpace < minNextValue) length = spaceLength - minNextValue;
                    if (maxNextValue && nextSpace > maxNextValue) length = spaceLength + maxNextValue;
                }

                var perc = length / document.splitterData[el].paneLength;
                if (!isFinite(perc)) {
                    perc = 1;
                    document.splitterData[el].panePerc = 0.1;
                    document.splitterData[el].paneLength = isHOrientation
                        ? pane.getBoundingClientRect().width
                        : pane.getBoundingClientRect().height;
                }

                var newPerc = document.splitterData[el].panePerc * perc;
                if (newPerc < 0) newPerc = 0;
                if (newPerc > 100) newPerc = 100;

                pane.style.flexBasis = newPerc + '%';
                if (paneNext)
                    paneNext.style.flexBasis = (spacePerc - newPerc) + '%';
            }
        },
        touchMoveHandler: function (e) {
            if (e.targetTouches[0]) {
                document.splitterData[el].mouseMoveHandler(e.targetTouches[0]);
            }
        }
    };
    document.addEventListener('mousemove', document.splitterData[el].mouseMoveHandler);
    document.addEventListener('mouseup', document.splitterData[el].mouseUpHandler);
    document.addEventListener('touchmove', document.splitterData[el].touchMoveHandler, { passive: true });
    document.addEventListener('touchend', document.splitterData[el].mouseUpHandler, { passive: true });
}