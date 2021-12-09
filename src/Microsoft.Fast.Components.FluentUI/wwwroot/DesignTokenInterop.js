import {
    StandardLuminance,
    bodyFont,
    baseHeightMultiplier,
    baseHorizontalSpacingMultiplier,
    baseLayerLuminance,
    controlCornerRadius,
    density,
    designUnit,
    direction,
    disabledOpacity,
    strokeWidth,
    focusStrokeWidth,
    typeRampBaseFontSize,
    typeRampBaseLineHeight,
    typeRampMinus1FontSize,
    typeRampMinus1LineHeight,
    typeRampMinus2FontSize,
    typeRampMinus2LineHeight,
    typeRampPlus1FontSize,
    typeRampPlus1LineHeight,
    typeRampPlus2FontSize,
    typeRampPlus2LineHeight,
    typeRampPlus3FontSize,
    typeRampPlus3LineHeight,
    typeRampPlus4FontSize,
    typeRampPlus4LineHeight,
    typeRampPlus5FontSize,
    typeRampPlus5LineHeight,
    typeRampPlus6FontSize,
    typeRampPlus6LineHeight,
    accentFillRestDelta,
    accentFillHoverDelta,
    accentFillActiveDelta,
    accentFillFocusDelta,
    accentForegroundRestDelta,
    accentForegroundHoverDelta,
    accentForegroundActiveDelta,
    accentForegroundFocusDelta,
    neutralFillRestDelta,
    neutralFillHoverDelta,
    neutralFillActiveDelta,
    neutralFillFocusDelta,
    neutralFillInputRestDelta,
    neutralFillInputHoverDelta,
    neutralFillInputActiveDelta,
    neutralFillInputFocusDelta,
    neutralFillStealthRestDelta,
    neutralFillStealthHoverDelta,
    neutralFillStealthActiveDelta,
    neutralFillStealthFocusDelta,
    neutralFillStrongRestDelta,
    neutralFillStrongHoverDelta,
    neutralFillStrongActiveDelta,
    neutralFillStrongFocusDelta,
    neutralFillLayerRestDelta,
    neutralStrokeRestDelta,
    neutralStrokeHoverDelta,
    neutralStrokeActiveDelta,
    neutralStrokeFocusDelta,
    neutralStrokeDividerRestDelta
} from 'https://cdn.jsdelivr.net/npm/@fluentui/web-components/dist/web-components.min.js'


export function setValueForSelector(designtoken, selector, value) {

    var x = document.querySelectorAll(selector);
    var i;
    for (i = 0; i < x.length; i++) {
        eval(designtoken).setValueFor(x[i], value);
    }
}

export function setValueFor(designtoken, element, value) {
    if (element instanceof HTMLElement) {
        eval(designtoken).setValueFor(element, value);
    }
    else {
        throw new Error('Unable to set value for an invalid element.');
    }
}