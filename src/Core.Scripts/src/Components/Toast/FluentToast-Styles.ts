export const fluentToastStyles: string = `
:host(:not([opened='true']):not(.animating)) {
    display: none;
}

:host {
    display: contents;
}

:host div[fuib][popover] {
    display: grid;
    grid-template-columns: auto 1fr auto;
    background: var(--colorNeutralBackground1);
    font-size: var(--fontSizeBase300);
    line-height: var(--lineHeightBase300);
    font-weight: var(--fontWeightSemibold);
    color: var(--colorNeutralForeground1);
    border: 1px solid var(--colorTransparentStroke);
    border-radius: var(--borderRadiusMedium);
    box-shadow: var(--shadow8);
    box-sizing: border-box;
    min-width: var(--toast-width, 292px);
    max-width: var(--toast-width, 292px);
    height: auto;
    padding: 12px;
    transition:
        top 240ms cubic-bezier(0.22, 1, 0.36, 1),
        bottom 240ms cubic-bezier(0.22, 1, 0.36, 1),
        left 240ms cubic-bezier(0.22, 1, 0.36, 1),
        right 240ms cubic-bezier(0.22, 1, 0.36, 1),
        transform 240ms cubic-bezier(0.22, 1, 0.36, 1);
}

:host([inverted]) div[fuib][popover]{
    color: var(--colorNeutralForegroundInverted2);
    background-color: var(--colorNeutralBackgroundInverted);
}

.media {
    display: flex;
    grid-column-end: 2;
    padding-top: 2px;
    padding-inline-end: 8px;
    font-size: var(--fontSizeBase400);
    color: var(--colorNeutralForeground1);
}

:host([inverted]) .media {
    color: var(--colorNeutralForegroundInverted);
}

:host([inverted]) .media  {
    color: var(--colorNeutralForegroundInverted);
}

.media[data-intent="success"] {
    color: var(--colorStatusSuccessForeground1);
}

.media[data-intent="error"] {
    color: var(--colorStatusDangerForeground1);
}

.media[data-intent="warning"] {
    color: var(--colorStatusWarningForeground1);
}

.media[data-intent="info"] {
    color: var(--colorNeutralForeground2);
}

:host([inverted]) .media[data-intent="success"] {
    color: var(--colorStatusSuccessForegroundInverted);
}

:host([inverted]) .media[data-intent="error"] {
    color: var(--colorStatusDangerForegroundInverted);
}

:host([inverted]) .media[data-intent="warning"] {
    color: var(--colorStatusWarningForegroundInverted);
}

:host([inverted]) .media[data-intent="info"] {
    color: var(--colorNeutralForegroundInverted2);
}

.title {
    display: flex;
    grid-column-end: 3;
    color: var(--colorNeutralForeground1);
    word-break: break-word;
}

:host([inverted]) .title {
    color: var(--colorNeutralForegroundInverted2);
}

.action {
    display: flex;
    align-items: start;
    justify-content: end;
    grid-column-end: -1;
    padding-inline-start: 12px;
    color: var(--colorBrandForeground1);
}

:host([inverted]) .action {
    color: var(--colorBrandForegroundInverted);
}

.body {
    grid-column: 2 / 3;
    padding-top: 6px;
    font-size: var(--fontSizeBase300);
    line-height: var(--lineHeightBase300);
    font-weight: var(--fontWeightRegular);
    color: var(--colorNeutralForeground1);
    word-break: break-word;
}

    :host([inverted]) .body {
    color: var(--colorNeutralForegroundInverted2);
}

.subtitle {
    grid-column: 2 / 3;
    padding-top: 4px;
    font-size: var(--fontSizeBase200);
    line-height: var(--lineHeightBase200);
    font-weight: var(--fontWeightRegular);
    color: var(--colorNeutralForeground2);
}

:host([inverted]) .subtitle {
    color: var(--colorNeutralForegroundInverted2);
}

.footer {
    display: flex;
    align-items: center;
    flex-wrap: wrap;
    gap: 14px;
    grid-column: 2 / 3;
    padding-top: 16px;
}

:host([inverted]) slot[name="footer"]::slotted(fluent-link[clickable]) {
    color: var(--colorBrandForegroundInverted);
}

.footer ::slotted(*) {
    display: contents;
}

:host(:not([has-media])) .body,
:host(:not([has-media])) .subtitle,
:host(:not([has-media])) .footer {
    grid-column: 1 / -1;
}

:host(:not([has-action])) .title {
    grid-column: 2 / -1;
}

.media[hidden],
.title[hidden],
.action[hidden],
.body[hidden],
.subtitle[hidden],
.footer[hidden] {
    display: none !important;
}

/* Animations */
:host div[fuib][popover]:popover-open {
    opacity: 1;
    animation: toast-enter 0.25s cubic-bezier(0.33, 0, 0, 1) forwards;
}

:host div[fuib][popover].closing {
    pointer-events: none;
    overflow: hidden;
    will-change: opacity, height, margin, padding;
    animation:
        toast-exit 400ms cubic-bezier(0.33, 0, 0.67, 1) forwards,
        toast-collapse-height 200ms cubic-bezier(0.33, 0, 0.67, 1) 400ms forwards,
        toast-collapse-spacing 200ms cubic-bezier(0.33, 0, 0.67, 1) 400ms forwards;
}

@keyframes toast-enter {
    from { opacity: 0; transform: var(--toast-enter-from, translateY(16px)); }
    to { opacity: 1; transform: var(--toast-enter-to, translateY(0)); }
}

@keyframes toast-exit {
    from {
    opacity: 1;
    }
    to {
    opacity: 0;
    }
}

@keyframes toast-collapse-height {
    from {
    height: var(--toast-height);
    }
    to {
    height: 0;
    }
}

@keyframes toast-collapse-spacing {
    from {
    margin-top: var(--toast-margin-top, 0px);
    margin-bottom: var(--toast-margin-bottom, 0px);
    padding-top: var(--toast-padding-top, 0px);
    padding-bottom: var(--toast-padding-bottom, 0px);
    }
    to {
    margin-top: 0;
    margin-bottom: 0;
    padding-top: 0;
    padding-bottom: 0;
    }
}
`