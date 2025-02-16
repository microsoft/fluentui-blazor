export function updateSlider(ref: HTMLElement | null | undefined): void {
  // Incoming reference is to the fluent-slider web component
  if (ref !== undefined && ref !== null) {
    // Check that ref is an element with a 'value' property
    if ('value' in ref && ref.attributes) {
      // Fixes the issue where the value cannot be reset
      ref.value = ref.attributes.getNamedItem('value')?.value ?? '';
    }

    // Check if the method setThumbPositionForOrientation exists before calling it
    if (typeof (ref as any).setThumbPositionForOrientation === 'function') {
      (ref as any).setThumbPositionForOrientation((ref as any).direction);
    }
  }
}
