/**
 * Defines the HTML structure rendered in the Shadow DOM.
 */

import { styles } from './overflow.styles.js';

/** Template for the overflow component. */
export const template = document.createElement('template');

template.innerHTML = `
  <style>${styles}</style>
  <div part="container">
    <div part="items">
      <slot></slot>
    </div>
    <slot name="trigger" part="trigger" hidden></slot>
  </div>
  <slot name="menu"></slot>
`;