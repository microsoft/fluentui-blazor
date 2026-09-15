/**
 * Defines the component styles.
 */

export const styles = `
    :host {
      box-sizing: border-box;
      display: block;
      min-inline-size: 0;
    }

    :host([orientation='vertical']) {
      block-size: 100%;
      min-block-size: 0;
    }

    [part='container'] {
      align-items: center;
      display: flex;
      gap: var(--fluent-overflow-gap, 4px);
      min-inline-size: 0;
    }

    :host([orientation='vertical']) [part='container'] {
      align-items: stretch;
      block-size: 100%;
      flex-direction: column;
      min-block-size: 0;
    }

    [part='items'] {
      align-items: center;
      display: flex;
      flex: 1 1 auto;
      gap: var(--fluent-overflow-gap, 4px);
      min-inline-size: 0;
      overflow: hidden;
    }

    :host([orientation='vertical']) [part='items'] {
      align-items: stretch;
      flex-direction: column;
      min-block-size: 0;
    }

    slot:not([name]),
    slot[name='menu'] {
      display: contents;
    }

    slot:not([name])::slotted(*) {
      flex: 0 0 auto;
    }

    slot:not([name])::slotted([behavior='ellipsis']) {
      flex: 0 1 auto;
      min-inline-size: 0;
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
    }

    [part='trigger'] {
      display: block;
      flex: 0 0 auto;
    }

    [part='trigger'][hidden] {
      display: none;
    }

    :host([overflow-direction='start']) [part='trigger'] {
      order: -1;
    }
`;