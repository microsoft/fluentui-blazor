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

  const BODY_ATTRIBUTE_NOFUIBSTYLE: string = 'no-fuib-style';
  const BODY_ATTRIBUTE_USEREBOOT: string = 'use-reboot';
  const ID_DEFAULT_FUIB_CSS: string = 'default-fuib-css';
  const ID_REBOOT_CSS: string = 'reboot-css';

  /**
   * Update the default FluentUI Blazor styles to the document
   */
  export const applyStyles = (): void => {

    var styleSheet = new CSSStyleSheet();
    styleSheet.replaceSync(styles);
    document.adoptedStyleSheets = [...document.adoptedStyleSheets, styleSheet];

    // Read (or not) the default styles: `fluentui-blazor.css` and 'reboot.css'
    AddOrRemoveDefaultStyle();
    AddOrRemoveReboot();
    observeDefaultStyle();
  }

  // Add or remove the `default-fuib.css` styles to the document
  function AddOrRemoveDefaultStyle() {
    const noDefaultStyle =
      document.body?.hasAttribute(BODY_ATTRIBUTE_NOFUIBSTYLE) === true ||
      document.documentElement?.hasAttribute(BODY_ATTRIBUTE_NOFUIBSTYLE) === true;

    // Remove the style
    if (noDefaultStyle) {
      const link = document.getElementById(ID_DEFAULT_FUIB_CSS);
      if (link) {
        link.remove();
      }
    }

    // Add the style
    else {
      const link = document.createElement('link');
      link.rel = 'stylesheet';
      link.type = 'text/css';
      link.href = './_content/Microsoft.FluentUI.AspNetCore.Components/css/default-fuib.css';
      link.id = ID_DEFAULT_FUIB_CSS;
      document.head.appendChild(link);
    }
  }

  // Add or remove the `Reboot.css` styles to the document
  function AddOrRemoveReboot() {
    const useRebootStyle =
      document.body?.hasAttribute(BODY_ATTRIBUTE_USEREBOOT) === true ||
      document.documentElement?.hasAttribute(BODY_ATTRIBUTE_USEREBOOT) === true;

    // Add the style
    if (useRebootStyle) {
      const link = document.createElement('link');
      link.rel = 'stylesheet';
      link.type = 'text/css';
      link.href = './_content/Microsoft.FluentUI.AspNetCore.Components/css/reboot.css';
      link.id = ID_REBOOT_CSS;
      document.head.appendChild(link);
    }

    // Remove the style
    else {
      const link = document.getElementById(ID_REBOOT_CSS);
      if (link) {
        link.remove();
      }
    }
  }

  function observeDefaultStyle() {
    const observer = new MutationObserver((mutationsList) => {
      for (const mutation of mutationsList) {
        if (mutation.type === 'attributes' && mutation.attributeName === BODY_ATTRIBUTE_NOFUIBSTYLE) {
          AddOrRemoveDefaultStyle();
        }
        if (mutation.type === 'attributes' && mutation.attributeName === BODY_ATTRIBUTE_USEREBOOT) {
          AddOrRemoveReboot();
        }
      }
    });
    observer.observe(document.body, { attributes: true });
  }
}
