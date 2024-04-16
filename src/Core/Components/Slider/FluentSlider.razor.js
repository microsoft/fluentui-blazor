export function updateSlider(ref) {
    // incoming reference is to the fluent-slider web component

    // fixes issue where the value indicator is not redrawn when either the min or max is changed
    ref.setThumbPositionForOrientation(ref.direction);
}
