export namespace Microsoft.FluentUI.Blazor.FluentUIStyles {

  const styles: string = `
body:has(.prevent-scroll) {
    overflow: hidden;
}

:root {
    --font-monospace: var(--fontFamilyMonospace);
    --success: var(--colorStatusSuccessForeground1);
    --warning: var(--colorStatusWarningForeground1);
    --error: var(--colorStatusDangerForeground1);
    --info: var(--colorPalettePlatinumForeground2);
    --presence-available: var(--colorPaletteLightGreenForeground3);
    --presence-away: var(--colorPaletteMarigoldBackground3);
    --presence-busy: var(--colorPaletteRedBackground3);
    --presence-dnd: var(--colorPaletteRedBackground3);
    --presence-offline: var(--colorNeutralForeground3);
    --presence-oof: var(--colorPaletteBerryForeground3);
    --presence-blocked: var(--colorPaletteRedBackground3);
    --presence-unknown: var(--colorNeutralForeground3);
    --highlight-bg: #fff3cd;
    --design-unit: 4px;
}
`;

  /**
   * Update the default FluentUI Blazor styles to the document
   */
  export const applyStyles = (): void => {
    var styleSheet = new CSSStyleSheet();
    styleSheet.replaceSync(styles);
    document.adoptedStyleSheets = [...document.adoptedStyleSheets, styleSheet];
  }
}
