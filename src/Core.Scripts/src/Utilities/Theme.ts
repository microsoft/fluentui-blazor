import { webLightTheme, webDarkTheme, teamsDarkTheme, teamsLightTheme,  BrandVariants, Theme, createDarkTheme, createLightTheme } from '@fluentui/tokens';
import { setTheme } from '@fluentui/web-components';
import { getBrandTokensFromPalette } from './BrandRamp/ramp/getBrandTokensFromPalette';

export namespace Microsoft.FluentUI.Blazor.Utilities.Theme {

  const THEME_STORAGE_KEY = 'fluentui-blazor:theme-settings';
  const THEME_EFFECTIVE_ATTRIBUTE = 'data-theme-effective';

  type ThemeMode = 'light' | 'dark' | 'system';
  type Direction = 'ltr' | 'rtl';
  type ThemeBase = 'web' | 'teams' | 'brand';
  type BrandRampInputs = { color: string; hueTorsion: number; vibrancy: number; isExact: boolean; isDark: boolean };
  type RampCache = {
    inputs: BrandRampInputs;
    ramp: BrandVariants;
  };

  type StoredThemeSettings = {
    color?: string;
    hue?: number;
    vibrancy?: number;
    exact?: boolean;
    theme?: ThemeMode;
    base?: ThemeBase;
    dir?: Direction;
  };

  /**
  * Sets the Fluent UI theme to light mode.
  * Takes brand color from data-theme-color attribute and calculates a light theme palette
  * If attribute not provided then checks local storage value.
  * Fallback is default value
  */
  export function setLightTheme(): void {
    applyTheme(false);
  }

  /**
   * Sets the Fluent UI theme to dark mode.
   * Takes brand color from data-theme-color attribute and calculates a dark theme palette
   * If attribute not provided then looks for a value in local storage.
   * Fallback is default value
   */
  export function setDarkTheme(): void {
    applyTheme(true);
  }

  /**
   * Sets the Fluent UI theme to match the system theme (dark/light).
   * Takes brand color from data-theme-color attribute and calculates a theme palette
   * If attribute not provided then looks for a value in local storage.
   * Fallback is default value
   */
  export function setSystemTheme(): void {
    setThemeMode('system');
  }

  /**
   * Sets the Fluent UI theme to the specified mode.
   * Takes brand color from data-theme-color attribute and calculates a theme palette
   * If attribute not provided then looks for a value in local storage.
   * fallback is default value
   * @param mode The theme mode to set ('light', 'dark', or 'system').
   */

  export function setThemeMode(mode: ThemeMode): void {
    if (mode === 'dark') {
      updateStoredThemeSettings({ base: undefined, theme: 'dark' });
      setDarkTheme();
      return;
    }

    if (mode === 'light') {
      updateStoredThemeSettings({ base: undefined, theme: 'light' });
      setLightTheme();
      return;
    }

    updateStoredThemeSettings({ base: undefined, theme: undefined });
    applyTheme(isSystemDark());
  }

  /**
   * Sets the Fluent UI theme to the built-in Web theme, based on the current effective mode.
   */
  export function setWebTheme(): void {
    applyTheme(isDarkMode());
  }

  /**
   * Sets the Fluent UI theme to the built-in Teams light theme.
   * Also updates localStorage so this base theme can be restored on a later run.
   */
  export function setTeamsLightTheme(): void {
    updateStoredThemeSettings({ base: 'teams' });
    setTheme(teamsLightTheme);
    updateBodyTag(false);
  }

  /**
   * Sets the Fluent UI theme to the built-in Teams dark theme.
   * Also updates localStorage so this base theme can be restored on a later run.
   */
  export function setTeamsDarkTheme(): void {
    updateStoredThemeSettings({ base: 'teams' });
    setTheme(teamsDarkTheme);
    updateBodyTag(true);
  }

  /**
  * Sets the Fluent UI theme to the built-in Teams theme based on the system theme (dark/light).
  */
  export function setTeamsSystemTheme(): void {
    setTeamsThemeMode('system');
  }

  /**
  * Sets the Fluent UI theme to the built-in Teams theme in the specified mode.
  * @param mode The theme mode to set ('light', 'dark', or 'system').
  */
  export function setTeamsThemeMode(mode: ThemeMode): void {
    if (mode === 'dark') {
      setTeamsDarkTheme();
      return;
    }

    if (mode === 'light') {
      setTeamsLightTheme();
      return;
    }

    const isDark = isSystemDark();
    if (isDark) {
      setTeamsDarkTheme();
    } else {
      setTeamsLightTheme();
    }
  }


  /**
 * Create a Fluent UI theme based on a specified color and customizations for hue, vibrancy and theme.
 * @param color The primary color to generate the theme from, in hex format (e.g., #FF0000).
 * @param hueTorsion A number between -0.5 and 0.5 that adjusts the hue of the generated theme. Positive values shift the hue clockwise, negative values shift it counterclockwise.
 * @param vibrancy A number between -0.5 and 0.5 that adjusts the vibrancy of the generated theme. Positive values increase vibrancy, negative values decrease it.
 * @param isDark A boolean indicating whether to generate a dark theme (true) or light theme (false) based on the provided key color and customizations.
 * @param isExact A boolean indicating whether to use the exact color for the brand background.
 * @returns A Fluent UI Theme object, containing all token values, that can be applied globally or to specific elements, or null if the input parameters are invalid.
 */
  export function createBrandTheme(
    color: string,
    hueTorsion: number,
    vibrancy: number,
    isDark: boolean,
    isExact: boolean = false
  ): Theme | null {
    if (typeof color !== 'string' || !isValidHexColor(color)) return null;
    if (typeof hueTorsion !== 'number' || !Number.isFinite(hueTorsion) || hueTorsion < -0.5 || hueTorsion > 0.5) return null;
    if (typeof vibrancy !== 'number' || !Number.isFinite(vibrancy) || vibrancy < -0.5 || vibrancy > 0.5) return null;

    const normalized = normalizeHexColor(color);
    const inputs: BrandRampInputs = { color: normalized, hueTorsion, vibrancy, isExact, isDark };
    const ramp = getOrCreateCustomRamp(inputs);
    return isDark ? createDarkTheme(ramp) : createLightTheme(ramp);
  }

  /**
   * Applies a Fluent UI theme object.
   */
  export function setBrandThemeFromTheme(theme: Theme): void {
    setTheme(theme);
  }

  /**
   * Sets a FluentUI theme globally based on a specified color and customizations for hue, vibrancy and theme.
   * This allows dynamic generation of a theme that aligns with a brand color. The generated theme is stored in a cache to optimize performance for repeated inputs.
   * @param color The primary color to generate the theme from, in hex format (e.g., #FF0000).
   * @param hueTorsion A number between -0.5 and 0.5 that adjusts the hue of the generated theme. Positive values shift the hue clockwise, negative values shift it counterclockwise.
   * @param vibrancy A number between -0.5 and 0.5 that adjusts the vibrancy of the generated theme. Positive values increase vibrancy, negative values decrease it.
   * @param isDark A boolean indicating whether to generate a dark theme (true) or light theme (false) based on the provided key color and customizations.
   * @param isExact A boolean indicating whether to use the exact color for the brand background.
   */
  export function setBrandTheme(color: string, hueTorsion: number, vibrancy: number, isDark: boolean, isExact: boolean = false): void {
    const theme = createBrandTheme(color, hueTorsion, vibrancy, isDark, isExact);
    if (!theme) return;
    updateStoredThemeSettings({ color: normalizeHexColor(color), hue: hueTorsion, vibrancy, exact: isExact });
    setTheme(theme);
    updateBodyTag(isDark);
  }

  /**
  * Sets a FluentUI theme globally based on the specified color and the current mode only.
  * This allows dynamic generation of a theme that aligns with a brand color. The generated theme is stored in a cache to optimize performance for repeated inputs.
  * @param color The primary color to generate the theme from, in hex format (e.g., #FF0000).
  */
  export function setBrandThemeFromColor(color: string): void {
    const isDark = isDarkMode();
    const theme = createBrandTheme(color, 0, 0, isDark, false);
    if (!theme) return;
    updateStoredThemeSettings({ color: normalizeHexColor(color), hue: 0, vibrancy: 0, exact: false });
    setTheme(theme);
    updateBodyTag(isDark);
  }

  /**
   * Sets a FluentUI theme globally based on the exact specified color and the current mode only.
   * This allows dynamic generation of a theme that aligns with a brand color. The generated theme is stored in a cache to optimize performance for repeated inputs.
   * @param color The primary color to generate the theme from, in hex format (e.g., #FF0000).
   */
  export function setBrandThemeFromColorExact(color: string): void {
    const isDark = isDarkMode();
    const theme = createBrandTheme(color, 0, 0, isDark, true);
    if (!theme) return;
    updateStoredThemeSettings({ color: normalizeHexColor(color), hue: 0, vibrancy: 0, exact: true });
    setTheme(theme);
    updateBodyTag(isDark);
  }



  /**
   * Sets the FluentUI theme on a specified element based on a specified color and customizations for hue, vibrancy and theme.
   * This allows dynamic generation of a theme that aligns with a brand color. The generated theme is stored in a cache to optimize performance for repeated inputs.
   * @param color The primary color to generate the theme from, in hex format (e.g., #FF0000).
   * @param hueTorsion A number between -0.5 and 0.5 that adjusts the hue of the generated theme. Positive values shift the hue clockwise, negative values shift it counterclockwise.
   * @param vibrancy A number between -0.5 and 0.5 that adjusts the vibrancy of the generated theme. Positive values increase vibrancy, negative values decrease it.
   * @param isDark A boolean indicating whether to generate a dark theme (true) or light theme (false) based on the provided key color and customizations.
   * @param isExact A boolean indicating whether to use the exact color for the brand background.
   */
  export function setBrandThemeToElement(element: HTMLElement, color: string, hueTorsion: number, vibrancy: number, isDark: boolean, isExact: boolean = false): void {
    const theme = createBrandTheme(color, hueTorsion, vibrancy, isDark, isExact);
    if (!theme) return;
    setTheme(theme, element);
  }

  /**
   * Returns true if the browser is in dark mode
   */
  export function isSystemDark(): boolean {
    return window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
  }

  /**
   * Adds a listener for changes to the system theme (dark/light) and applies the new theme if the
   * user has not explicitly set a preference.
   * The listener is added only once, even if this function is called multiple times. This should be
   * called on startup to ensure the theme stays in sync with system changes. The library already takes
   * care of calling this in the beforeStart lifecycle method.
   */
  export function addSystemThemeChangeListener(): void {
    if ((window as any).__fluentuiBlazorSystemThemeChangeListener) {
      return;
    }
    (window as any).__fluentuiBlazorSystemThemeChangeListener = true;

    const mql = window.matchMedia?.('(prefers-color-scheme: dark)');
    if (!mql) return;

    const handler = (e?: MediaQueryListEvent) => {
      const storedTheme = tryGetStoredThemeSettings()?.theme;
      if (storedTheme === 'dark' || storedTheme === 'light') return;

      const bodyTheme = document.body.getAttribute('data-theme');
      if (bodyTheme === 'dark' || bodyTheme === 'light') return;

      const isDark = e ? e.matches : mql.matches;
      applyTheme(isDark);
    };

    mql.addEventListener('change', handler);
  }

  let _rampCache: RampCache | null = null;

  /**
   * Returns the currently cached custom brand ramp, or null if no custom ramp has been generated yet.
   */
  export function getCachedRamp(): BrandVariants | null {
    return _rampCache?.ramp ?? null;
  }


  /**
   * Returns true if the current FluentUI theme is dark mode
   */
  export function isDarkMode(): boolean {
    const effectiveTheme = document.body.getAttribute(THEME_EFFECTIVE_ATTRIBUTE);
    return effectiveTheme === 'dark';
  }

  /**
   * Removes the stored theme settings from localStorage.
   */
  export function clearThemeSettings(): void {
    try {
      localStorage.removeItem(THEME_STORAGE_KEY);
    } catch {
      // ignore
    }
  }



  /**
   * Switches the FluentUI theme between light and dark mode.
   * Returns true if the new theme is dark mode, false otherwise.
   */
  export function switchTheme(): boolean {
    const storedTheme = tryGetStoredThemeSettings()?.theme;
    const isCurrentlyDark = storedTheme === 'dark' ? true : storedTheme === 'light' ? false : isDarkMode();

    if (isCurrentlyDark) {
      setThemeMode('light');
      return false;
    }

    setThemeMode('dark');
    return true;
  }

  /**
   * Sets the initial Fluent UI theme to the default mode, color and customizations based on:
   * - body `data-theme` attribute (dark/light), else
   * - stored user preference in localStorage, else
   * - system preference
   *
   * Uses default webLightTheme/webDarkTheme unless custom BrandVariants exist.
   */
  export function initializeThemeSettings(): void {
    const bodyTheme = document.body.getAttribute('data-theme');
    const bodyThemeColor = document.body.getAttribute('data-theme-color');
    const stored = tryGetStoredThemeSettings();

    const modeToApply: ThemeMode =
      bodyTheme === 'dark' || bodyTheme === 'light' || bodyTheme === 'system'
        ? bodyTheme
        : (stored?.theme ?? 'system');

    const isDark =
      modeToApply === 'dark'
        ? true
        : modeToApply === 'light'
          ? false
          : isSystemDark();

    const hasColorAttribute = bodyThemeColor !== null;
    if (!hasColorAttribute && stored?.base === 'teams') {
      if (isDark) {
        setTheme(teamsDarkTheme);
      } else {
        setTheme(teamsLightTheme);
      }
      updateBodyTag(isDark);
    }
    else {
      applyTheme(isDark);
    }

    const htmlElement = document.documentElement;
    const htmlDir = htmlElement?.getAttribute('dir');
    if (htmlDir === 'ltr' || htmlDir === 'rtl') {
      updateStoredThemeSettings({ dir: htmlDir });
    }
    else if (stored?.dir !== undefined) {
      updateStoredThemeSettings({ dir: undefined });
    }

    const dir: Direction | undefined = stored?.dir;
    if (dir) {
      htmlElement?.setAttribute('dir', dir);
    }
  }

  export function switchDirection(): void {
    const htmlElement = document.documentElement;
    if (htmlElement) {
      const currentDir = htmlElement.getAttribute('dir');
      const newDir: Direction = currentDir === 'rtl' ? 'ltr' : 'rtl';
      htmlElement.setAttribute('dir', newDir);
      updateStoredThemeSettings({ dir: newDir });
    }
  }

  /**
  * Adds a listener for media queries to the window object
  * to update the body `data-media` attribute with the current media query
  */
  export function addMediaQueriesListener(): void {
    if ((window as any).__fluentuiBlazorMediaQueriesListener) {
      return;
    }
    (window as any).__fluentuiBlazorMediaQueriesListener = true;

    const getMediaQueries = (): { id: string, query: string }[] => {
      return [
        { id: 'xs', query: '(max-width: 599.98px)' },
        { id: 'sm', query: '(min-width: 600px) and (max-width: 959.98px)' },
        { id: 'md', query: '(min-width: 960px) and (max-width: 1279.98px)' },
        { id: 'lg', query: '(min-width: 1280px) and (max-width: 1919.98px)' },
        { id: 'xl', query: '(min-width: 1920px) and (max-width: 2559.98px)' },
        { id: 'xxl', query: '(min-width: 2560px)' },
      ];
    }

    const dispatchMediaChanged = (bodyTag: HTMLElement) => {
      if (bodyTag) {
        const event = new CustomEvent('mediaChanged', {
          detail: { media: bodyTag.getAttribute('data-media') }
        });
        bodyTag.dispatchEvent(event);
      }
    }

    const bodyTag: HTMLElement = document?.body;
    if (bodyTag) {
      const matched = getMediaQueries().find(mq => window.matchMedia(mq.query).matches);
      if (matched) {
        bodyTag.setAttribute('data-media', matched.id);
        dispatchMediaChanged(bodyTag);
      }
    }

    getMediaQueries().forEach((mediaQuery) => {
      window.matchMedia(mediaQuery.query).addEventListener('change', media => {
        if (media.matches) {
          const bodyTag: HTMLElement = document?.body;
          if (bodyTag && bodyTag.getAttribute('data-media') !== mediaQuery.id) {
            bodyTag.setAttribute('data-media', mediaQuery.id);
            dispatchMediaChanged(bodyTag);
          }
        }
      });
    });
  }

  function applyTheme(isDark: boolean): void {
    const bodyThemeColor = document.body.getAttribute('data-theme-color');
    const overrideColor = bodyThemeColor && isValidHexColor(bodyThemeColor) ? normalizeHexColor(bodyThemeColor) : null;

    const hasColorAttribute = bodyThemeColor !== null;
    const hasValidOverrideColor = overrideColor !== null;

    const bodyThemeHue = hasValidOverrideColor ? document.body.getAttribute('data-theme-hue') : null;
    const overrideHue = tryParseThemeFloat(bodyThemeHue);

    const bodyThemeVibrancy = hasValidOverrideColor ? document.body.getAttribute('data-theme-vibrancy') : null;
    const overrideVibrancy = tryParseThemeFloat(bodyThemeVibrancy);

    const stored = tryGetStoredThemeSettings();

    const bodyThemeExact = hasValidOverrideColor ? document.body.getAttribute('data-theme-exact') : null;
    const overrideExact = bodyThemeExact !== null ? (bodyThemeExact !== 'false' && bodyThemeExact !== '0') : null;

    const isExact = hasValidOverrideColor
      ? (overrideExact ?? false)
      : hasColorAttribute
        ? false
        : (stored?.exact ?? false);

    const customInputs = hasColorAttribute && !hasValidOverrideColor
      ? null
      : tryGetCustomRampInputs(overrideColor, overrideHue, overrideVibrancy);

    if (customInputs) {
      const ramp = getOrCreateCustomRamp({ ...customInputs, isExact, isDark });
      const theme: Theme = isDark ? createDarkTheme(ramp) : createLightTheme(ramp);
      setTheme(theme);
    } else {
      setTheme(isDark ? webDarkTheme : webLightTheme);
    }

    updateBodyTag(isDark);
  }

  function createBrandRamp(color: string, hueTorsion: number, vibrancy: number): BrandVariants {
    return getBrandTokensFromPalette(color, {
      hueTorsion,
      darkCp: vibrancy,
      lightCp: vibrancy,
    });
  }

  function getOrCreateCustomRamp(inputs: BrandRampInputs): BrandVariants {
    if (_rampCache && areRampInputsEqual(_rampCache.inputs, inputs)) {
      return _rampCache.ramp;
    }

    const rampBase = createBrandRamp(inputs.color, inputs.hueTorsion, inputs.vibrancy);
    const ramp = inputs.isExact
      ? inputs.isDark
        ? ({ ...rampBase, 70: inputs.color, 100: inputs.color } as BrandVariants)
        : ({ ...rampBase, 80: inputs.color } as BrandVariants)
      : rampBase;
    _rampCache = { inputs, ramp };
    return ramp;
  }

  function areRampInputsEqual(a: BrandRampInputs, b: BrandRampInputs): boolean {
    return a.color === b.color && a.hueTorsion === b.hueTorsion && a.vibrancy === b.vibrancy && a.isExact === b.isExact && a.isDark === b.isDark;
  }



  function tryGetStoredThemeSettings(): StoredThemeSettings | null {
    try {
      const stored = localStorage.getItem(THEME_STORAGE_KEY);
      if (!stored) return null;
      const obj = JSON.parse(stored);
      return (obj && typeof obj === 'object') ? (obj as StoredThemeSettings) : null;
    } catch {
      return null;
    }
  }

  function updateStoredThemeSettings(patch: Partial<StoredThemeSettings>): void {
    try {
      const current = tryGetStoredThemeSettings() ?? {};
      const next: StoredThemeSettings = { ...current, ...patch };

      (Object.keys(next) as (keyof StoredThemeSettings)[]).forEach((k) => {
        if ((next as any)[k] === undefined) {
          delete (next as any)[k];
        }
      });

      localStorage.setItem(THEME_STORAGE_KEY, JSON.stringify(next));
    } catch {
      // ignore
    }
  }

  function tryGetCustomRampInputs(overrideColor: string | null = null, overrideHue: number | null = null, overrideVibrancy: number | null = null): BrandRampInputs | null {
    const settings = tryGetStoredThemeSettings();
    const color = overrideColor ?? settings?.color;
    if (typeof color !== 'string' || !isValidHexColor(color)) return null;

    const hueTorsion = overrideHue ?? (settings?.hue === undefined ? 0 : settings.hue);
    const vibrancy = overrideVibrancy ?? (settings?.vibrancy === undefined ? 0 : settings.vibrancy);

    if (typeof hueTorsion !== 'number' || !Number.isFinite(hueTorsion)) return null;
    if (typeof vibrancy !== 'number' || !Number.isFinite(vibrancy)) return null;

    return { color: normalizeHexColor(color), hueTorsion, vibrancy, isExact: false, isDark: false };
  }

  function isValidHexColor(value: string): boolean {
    // #RGB or #RRGGBB
    return /^#([0-9a-fA-F]{3}|[0-9a-fA-F]{6})$/.test(value.trim());
  }

  function normalizeHexColor(value: string): string {
    const v = value.trim();
    if (v.length === 4) {
      // Expand #RGB -> #RRGGBB
      const r = v[1], g = v[2], b = v[3];
      return `#${r}${r}${g}${g}${b}${b}`.toUpperCase();
    }
    return v.toUpperCase();
  }

  function tryParseThemeFloat(value: string | null): number | null {
    if (value === null) return null;
    const parsed = Number.parseFloat(value);
    if (!Number.isFinite(parsed)) return null;
    if (parsed < -0.5 || parsed > 0.5) return null;
    return parsed;
  }

  // Update the body tag to set the data-theme attribute + raise event
  function updateBodyTag(isDark: boolean): void {
    const bodyTag: HTMLElement = document?.body;

    if (bodyTag) {
      if (isDark) {
        bodyTag.setAttribute(THEME_EFFECTIVE_ATTRIBUTE, 'dark');
      } else {
        bodyTag.setAttribute(THEME_EFFECTIVE_ATTRIBUTE, 'light');
      }

      const event = new CustomEvent('themeChanged', {
        detail: { isDark }
      });
      bodyTag.dispatchEvent(event);
    }
  }
}
