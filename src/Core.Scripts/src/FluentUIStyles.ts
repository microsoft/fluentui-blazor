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

    --spacingVerticalNone: 0;
    --spacingVerticalXS: 4px;
    --spacingVerticalS: 8px;
    --spacingVerticalM: 12px;
    --spacingVerticalL: 16px;
    --spacingVerticalXL: 20px;
    --spacingVerticalXXL: 24px;
    --spacingVerticalXXXL: 28px;
    --spacingVerticalXXXXL: 32px;
    --spacingHorizontalNone: 0;
    --spacingHorizontalXS: 4px;
    --spacingHorizontalS: 8px;
    --spacingHorizontalM: 12px;
    --spacingHorizontalL: 16px;
    --spacingHorizontalXL: 20px;
    --spacingHorizontalXXL: 24px;
    --spacingHorizontalXXXL: 28px;
    --spacingHorizontalXXXXL: 32px;
}
`;

  /**
   * Update the default FluentUI Blazor styles to the document
   */
  export const applyStyles = (): void => {

    var styleSheet = new CSSStyleSheet();
    styleSheet.replaceSync(styles);
    document.adoptedStyleSheets = [...document.adoptedStyleSheets, styleSheet];

    // Read the default style `fluentui-blazor.css`.
    const useDefaultStyle = document.body?.hasAttribute('no-fuib-style') === true || document.documentElement?.hasAttribute('no-fuib-style') === true;

    if (!useDefaultStyle) {
      const link = document.createElement('link');
      link.rel = 'stylesheet';
      link.type = 'text/css';
      link.href = '_content/Microsoft.FluentUI.AspNetCore.Components/css/default-fuib.css';
      document.head.appendChild(link);
    }
  }
}
