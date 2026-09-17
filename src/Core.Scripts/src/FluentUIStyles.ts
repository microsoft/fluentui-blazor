export namespace Microsoft.FluentUI.Blazor.FluentUIStyles {

  const styles: string = `
body:has(.prevent-scroll) {
    overflow: hidden;
}

:root {
    --font-monospace: var(--fontFamilyMonospace);
    --success: var(--colorStatusSuccessForeground1);
    --warning: var(--colorStatusWarningForeground1);
    --error: var(--colorPaletteRedForeground1);
    --info: var(--colorNeutralForeground3);
    --success-inverted: var(--colorStatusSuccessForegroundInverted);
    --warning-inverted: var(--colorStatusWarningForegroundInverted);
    --error-inverted: var(--colorPaletteRedForegroundInverted);
    --info-inverted: var(--colorNeutralForegroundInverted2);
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
  const REBOOT_CSS_URL: string = './_content/Microsoft.FluentUI.AspNetCore.Components/css/reboot.css';
  const DEFAULT_FUIB_CSS_URL: string = './_content/Microsoft.FluentUI.AspNetCore.Components/css/default-fuib.css';

  let fluentUIStyleSheet: CSSStyleSheet | null = null;
  let defaultFuibStyleSheet: CSSStyleSheet | null = null;
  let rebootStyleSheet: CSSStyleSheet | null = null;
  let styleObserver: MutationObserver | null = null;

  /**
   * Update the default FluentUI Blazor styles to the document
   */
  export const applyStyles = (): void => {
    // Only add the adopted stylesheet once
    if (!fluentUIStyleSheet) {
      fluentUIStyleSheet = new CSSStyleSheet();
      fluentUIStyleSheet.replaceSync(styles);
      document.adoptedStyleSheets = [...document.adoptedStyleSheets, fluentUIStyleSheet];
    }

    AddOrRemoveDefaultStyleSheet();
    AddOrRemoveRebootStyleSheet();

    if (!styleObserver) {
      styleObserver = observeDefaultStyle();
    }
  }

  export const reapplyStyles = (): void => {
    if (fluentUIStyleSheet) {
      const alreadyIncluded = Array.from(document.adoptedStyleSheets).indexOf(fluentUIStyleSheet) !== -1;
      if (!alreadyIncluded) {
        document.adoptedStyleSheets = [...document.adoptedStyleSheets, fluentUIStyleSheet];
      }
    }

    EnsureDefaultStyleSheet();
    EnsureRebootStyleSheet();
  }

    async function AddOrRemoveDefaultStyleSheet() {
    const noDefaultStyle =
      document.body?.hasAttribute(BODY_ATTRIBUTE_NOFUIBSTYLE) === true ||
      document.documentElement?.hasAttribute(BODY_ATTRIBUTE_NOFUIBSTYLE) === true;

    if (noDefaultStyle) {
        if (defaultFuibStyleSheet && document.adoptedStyleSheets.indexOf(defaultFuibStyleSheet) !== -1) {
        const sheets = document.adoptedStyleSheets.filter(sheet => sheet !== defaultFuibStyleSheet);
        document.adoptedStyleSheets = sheets;
      }
      return;
    }

      if (!defaultFuibStyleSheet) {
      defaultFuibStyleSheet = await LoadCSSAsAdoptedStyleSheet(DEFAULT_FUIB_CSS_URL);
      if (defaultFuibStyleSheet) {
        document.adoptedStyleSheets = [...document.adoptedStyleSheets, defaultFuibStyleSheet];
      }
    }
  }

  async function AddOrRemoveRebootStyleSheet() {
    const useRebootStyle =
      document.body?.hasAttribute(BODY_ATTRIBUTE_USEREBOOT) === true ||
      document.documentElement?.hasAttribute(BODY_ATTRIBUTE_USEREBOOT) === true;

    if (useRebootStyle) {
        if (!rebootStyleSheet) {
        rebootStyleSheet = await LoadCSSAsAdoptedStyleSheet(REBOOT_CSS_URL);
        if (rebootStyleSheet) {
          document.adoptedStyleSheets = [...document.adoptedStyleSheets, rebootStyleSheet];
        }
      }
    } else {
      // Remove if present
      if (rebootStyleSheet && document.adoptedStyleSheets.indexOf(rebootStyleSheet) !== -1) {
        const sheets = document.adoptedStyleSheets.filter(sheet => sheet !== rebootStyleSheet);
        document.adoptedStyleSheets = sheets;
        rebootStyleSheet = null;
      }
    }
  }
  function EnsureDefaultStyleSheet() {
    const noDefaultStyle =
      document.body?.hasAttribute(BODY_ATTRIBUTE_NOFUIBSTYLE) === true ||
      document.documentElement?.hasAttribute(BODY_ATTRIBUTE_NOFUIBSTYLE) === true;

    if (noDefaultStyle) {
      return;
    }

    if (defaultFuibStyleSheet && document.adoptedStyleSheets.indexOf(defaultFuibStyleSheet) === -1) {
      document.adoptedStyleSheets = [...document.adoptedStyleSheets, defaultFuibStyleSheet];
    }
  }

  function EnsureRebootStyleSheet() {
    const useRebootStyle =
      document.body?.hasAttribute(BODY_ATTRIBUTE_USEREBOOT) === true ||
      document.documentElement?.hasAttribute(BODY_ATTRIBUTE_USEREBOOT) === true;

    if (useRebootStyle && rebootStyleSheet && document.adoptedStyleSheets.indexOf(rebootStyleSheet) === -1) {
      document.adoptedStyleSheets = [...document.adoptedStyleSheets, rebootStyleSheet];
    }
  }

    async function LoadCSSAsAdoptedStyleSheet(url: string): Promise<CSSStyleSheet | null> {
    try {
      const response = await fetch(url);
      if (!response.ok) {
        console.warn(`Failed to load CSS from ${url}: ${response.statusText}`);
        return null;
      }
      const cssText = await response.text();
      const styleSheet = new CSSStyleSheet();
      styleSheet.replaceSync(cssText);
      return styleSheet;
    } catch (error) {
      console.error(`Error loading CSS from ${url}:`, error);
      return null;
    }
  }

  function observeDefaultStyle() {
    const observer = new MutationObserver((mutationsList) => {
      for (const mutation of mutationsList) {
        if (mutation.type === 'attributes' && mutation.attributeName === BODY_ATTRIBUTE_NOFUIBSTYLE) {
          AddOrRemoveDefaultStyleSheet();
        }
        if (mutation.type === 'attributes' && mutation.attributeName === BODY_ATTRIBUTE_USEREBOOT) {
          AddOrRemoveRebootStyleSheet();
        }
      }
    });
    observer.observe(document.body, { attributes: true });
    return observer;
  }
}
