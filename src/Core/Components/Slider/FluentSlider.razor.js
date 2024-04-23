export function updateSlider(ref) {
    // incoming reference is to the fluent-slider web component

    if (ref != null && ref != undefined) {
        // fixes issue where you can't reset value programmatically
        ref.value = ref.attributes['value'].value;

        // fixes issue where the value indicator is not redrawn when either the min or max is changed
        ref.setThumbPositionForOrientation(ref.direction);
    }
}
