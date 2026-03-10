import { webLightTheme, webDarkTheme, teamsDarkTheme, teamsLightTheme, BrandVariants, Theme, createDarkTheme, createLightTheme } from '@fluentui/tokens';
import { setTheme } from '@fluentui/web-components';
import { getBrandTokensFromPalette } from './BrandRamp/ramp/getBrandTokensFromPalette';

export namespace Microsoft.FluentUI.Blazor.Utilities.Theme {

  const THEME_STORAGE_KEY = 'fluentui-blazor:theme-settings';
  let _rampCache: RampCache | null = null;

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

  type ThemePreferences = {
    mode: ThemeMode;
    base?: ThemeBase;
    dir?: Direction;
  };

  /**
   * Represents the settings used to generate a custom brand theme, including the base color, hue torsion, vibrancy, and theme mode.
   */
  export type ThemeSettings = { color: string; hueTorsion: number; vibrancy: number; mode?: ThemeMode | null; isExact?: boolean };

  /**
   * Sets the Fluent UI theme to light mode.
   * Takes brand color from data-theme-color attribute to calculates a light theme color ramp (palette).
   * If attribute not provided then checks local storage.
   * Fallback is default colorvalue
   */
  export function setLightTheme(): void {
    setThemeMode('light');
  }

  /**
   * Sets the Fluent UI theme to dark mode.
   * Takes brand color from data-theme-color attribute to calculates a dark theme color ramp (palette).
   * If attribute not provided then checks local storage.
   * Fallback is default color value
   */
  export function setDarkTheme(): void {
    setThemeMode('dark');
  }

  /**
   * Sets the Fluent UI theme to match the system theme (dark/light).
   * Takes brand color from data-theme-color attribute to calculates a theme color ramp (palette).
   * If attribute not provided then checks local storage.
   * Fallback is default color value
   */
  export function setSystemTheme(): void {
    setThemeMode('system');
  }

  /**
   * Sets the Fluent UI theme to the specified mode.
   * Takes brand color from data-theme-color attribute to calculate a theme color ramp (palette).
   * If attribute not provided then checks local storage.
   * Fallback is default color value
   * @param mode The theme mode to set ('light', 'dark', or 'system').
   */

  /**
   * Sets the Fluent UI theme to the built-in Web theme, using the current effective mode.
   */
  export function setWebTheme(): void {
    const next = updateThemeSettingsInStorage({ base: 'web' });
    applyCurrentTheme(next);
  }

  /**
   * Sets the Fluent UI theme to the built-in Teams light theme.
   * Also updates localStorage so this base theme can be restored on a later run.
   */
  export function setTeamsLightTheme(): void {
    updateThemeSettingsInStorage({ base: 'teams' });
    applyTheme(false, 'teams');
  }

  /**
   * Sets the Fluent UI theme to the built-in Teams dark theme.
   * Also updates localStorage so this base theme can be restored on a later run.
   */
  export function setTeamsDarkTheme(): void {
    updateThemeSettingsInStorage({ base: 'teams' });
    applyTheme(true, 'teams');
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

    const isDark = resolveIsDarkFromMode(mode);
    if (isDark) {
      setTeamsDarkTheme();
    } else {
      setTeamsLightTheme();
    }
  }

  /**
 * Sets the Fluent UI theme to the built-in Teams theme using the current effective mode.
 * Also updates localStorage so this base theme can be restored on a later run.
 */
  export function setTeamsTheme(): void {
    const next = updateThemeSettingsInStorage({ base: 'teams' });
    applyCurrentTheme(next);
  }

  export function setThemeMode(mode: ThemeMode): void {
    if (mode === 'dark') {
      const next = updateThemeSettingsInStorage({ theme: 'dark' });
      applyCurrentTheme(next);
      return;
    }

    if (mode === 'light') {
      const next = updateThemeSettingsInStorage({ theme: 'light' });
      applyCurrentTheme(next);
      return;
    }

    const next = updateThemeSettingsInStorage({ theme: undefined });
    applyCurrentTheme(next);
  }



  /**
 * Create a Fluent UI theme based on a specified color and customizations for hue, vibrancy and theme.
 * @param settings The settings to generate the theme, including the primary color in hex format, hue torsion, vibrancy, theme mode, and whether to use the exact color for the brand background.
 * @returns A Fluent UI Theme object, containing all token values, that can be applied globally or to specific elements, or null if the input parameters are invalid.
 */
  export function createBrandTheme(settings: ThemeSettings): Theme | null {
    if (typeof settings.color !== 'string' || !isValidHexColor(settings.color)) return null;
    if (typeof settings.hueTorsion !== 'number' || !Number.isFinite(settings.hueTorsion) || settings.hueTorsion < -0.5 || settings.hueTorsion > 0.5) return null;
    if (typeof settings.vibrancy !== 'number' || !Number.isFinite(settings.vibrancy) || settings.vibrancy < -0.5 || settings.vibrancy > 0.5) return null;

    const isExact = settings.isExact ?? false;

    const isDark = resolveIsDarkFromThemeSettings(settings);

    const normalized = normalizeHexColor(settings.color);
    const inputs: BrandRampInputs = { color: normalized, hueTorsion: settings.hueTorsion, vibrancy: settings.vibrancy, isExact, isDark };
    const ramp = getOrCreateRamp(inputs);
    return isDark ? createDarkTheme(ramp) : createLightTheme(ramp);
  }

  /**
   * Applies a Fluent UI theme object.
   */
  export function setBrandThemeFromTheme(theme: Theme): void {
    setTheme(theme);
  }

  /**
   * Sets a Fluent UI theme globally based on a specified color and customizations for hue, vibrancy and theme.
   * @param settings The settings to generate the theme, including the primary color in hex format, hue torsion, vibrancy, theme mode, and whether to use the exact color for the brand background.
   */
  export function setBrandThemeFromSettings(settings: ThemeSettings): void {
    const theme = createBrandTheme(settings);
    if (!theme) return;

    const isExact = settings.isExact ?? false;
    updateThemeSettingsInStorage({
      ...themeSettingsToStoredPatch({ ...settings, isExact }),
      base: 'brand',
    });
    setTheme(theme);
    updateBodyTag(resolveIsDarkFromThemeSettings(settings));
  }

  /**
  * Sets a Fluent UI theme globally based on the specified color using the current mode.
  * Generates a ramp (palette) that aligns with the supplied color.
  * @param color The primary color to generate the theme from, in hex format (e.g., #FF0000).
  */
  export function setBrandThemeFromColor(color: string): void {
    setBrandThemeFromSettings({ color, hueTorsion: 0, vibrancy: 0, mode: null, isExact: false });
  }

  /**
   * Sets a Fluent UI theme globally based on the exact specified color using the current mode only.
   * Generates a ramp (palette) that aligns with the supplied color.
   * Slots in the palette that are typically used for the brand color (e.g., 80 in light theme, 70 and 100 in dark theme) will be set to the
   * exact color provided.
   * THERE IS NO GUARANTEE ALL COLORS IN THE RAMP WILL PASS CONTRAST CHECKS FOR ACCESSIBILITY WHEN USING THIS EXACT MODE
   * @param color The primary color to generate the theme from, in hex format (e.g., #FF0000).
   */
  export function setBrandThemeFromColorExact(color: string): void {
    setBrandThemeFromSettings({ color, hueTorsion: 0, vibrancy: 0, mode: null, isExact: true });
  }

  /**
   * Sets the Fluent UI theme on a specified element based on a specified color and customizations for hue, vibrancy and theme.
   * @param settings The settings to generate the theme, including the primary color in hex format, hue torsion, vibrancy, theme mode, and whether to use the exact color for the brand background.
   */
  export function setBrandThemeToElement(element: HTMLElement, settings: ThemeSettings): void {
    const theme = createBrandTheme(settings);
    if (!theme) return;
    setTheme(theme, element);
  }

  /**
   * Generates a new brand ramp (palette) from the provided settings.
   * This always recalculates the ramp and does not use the internal cache.
   */
  export function getRampFromSettings(settings: ThemeSettings): BrandVariants | null {
    if (typeof settings.color !== 'string' || !isValidHexColor(settings.color)) return null;
    if (typeof settings.hueTorsion !== 'number' || !Number.isFinite(settings.hueTorsion) || settings.hueTorsion < -0.5 || settings.hueTorsion > 0.5) return null;
    if (typeof settings.vibrancy !== 'number' || !Number.isFinite(settings.vibrancy) || settings.vibrancy < -0.5 || settings.vibrancy > 0.5) return null;

    const isDark = resolveIsDarkFromThemeSettings(settings);
    const isExact = settings.isExact ?? false;

    const rampBase = createBrandRamp(normalizeHexColor(settings.color), settings.hueTorsion, settings.vibrancy);
    return isExact
      ? isDark
        ? ({ ...rampBase, 70: normalizeHexColor(settings.color), 100: normalizeHexColor(settings.color) } as BrandVariants)
        : ({ ...rampBase, 80: normalizeHexColor(settings.color) } as BrandVariants)
      : rampBase;
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
    if ((window as any).__FluentUIBlazorSystemThemeChangeListener) {
      return;
    }
    (window as any).__FluentUIBlazorSystemThemeChangeListener = true;

    const mql = window.matchMedia?.('(prefers-color-scheme: dark)');
    if (!mql) return;

    const handler = (e?: MediaQueryListEvent) => {
      const storedTheme = tryGetThemePreferencesFromStorage(tryGetThemeSettingsFromStorage())?.mode;
      if (storedTheme === 'dark' || storedTheme === 'light') return;

      const bodyTheme = document.body.getAttribute('data-theme');
      if (bodyTheme === 'dark' || bodyTheme === 'light') return;

      applyCurrentTheme();
    };

    mql.addEventListener('change', handler);
  }

  /**
   * Returns the currently cached custom brand ramp, or null if no custom ramp has been generated yet.
   */
  export function getCurrentRamp(): BrandVariants | null {
    return _rampCache?.ramp ?? null;
  }


  /**
   * Returns true if the current Fluent UI theme is dark mode
   */
  export function isDarkMode(): boolean {
    return resolveEffectiveIsDark();
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
   * Switches the Fluent UI theme between light and dark mode.
   * Returns true if the new theme is dark mode, false otherwise.
   */
  export function switchTheme(): boolean {
    const storedTheme = tryGetThemePreferencesFromStorage(tryGetThemeSettingsFromStorage())?.mode;
    const isCurrentlyDark = storedTheme === 'dark' ? true : storedTheme === 'light' ? false : isDarkMode();

    if (isCurrentlyDark) {
      setThemeMode('light');
      return false;
    }

    setThemeMode('dark');
    return true;
  }

  /**
   * Sets the initial Fluent UI theme to the default mode and color based on:
   * - body `data-theme` attribute (dark/light), else
   * - stored user preference in localStorage, else
   * - system preference
   *
   * Uses default webLightTheme/webDarkTheme unless custom BrandVariants exist.
   */
  export function initializeThemeSettings(): void {
    const stored = tryGetThemeSettingsFromStorage();
    const prefs = tryGetThemePreferencesFromStorage(stored);

    applyCurrentTheme(stored);

    const htmlElement = document.documentElement;
    const htmlDir = htmlElement?.getAttribute('dir');
    if (htmlDir === 'ltr' || htmlDir === 'rtl') {
      updateThemeSettingsInStorage({ dir: htmlDir });
    }
    else if (prefs?.dir !== undefined) {
      updateThemeSettingsInStorage({ dir: undefined });
    }

    const dir: Direction | undefined = prefs?.dir;
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
      updateThemeSettingsInStorage({ dir: newDir });
    }
  }

  /**
  * Adds a listener for media queries to the window object
  * to update the body `data-media` attribute with the current media query
  */
  export function addMediaQueriesListener(): void {
    if ((window as any).__FluentUIBlazorMediaQueriesListener) {
      return;
    }
    (window as any).__FluentUIBlazorMediaQueriesListener = true;

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

  function applyCurrentTheme(stored: StoredThemeSettings | null = null): void {
    const storedSettings = stored ?? tryGetThemeSettingsFromStorage();
    const prefs = tryGetThemePreferencesFromStorage(storedSettings);
    const mode = resolveEffectiveThemeMode(null, prefs);
    const isDark = resolveIsDarkFromMode(mode);

    const bodyThemeColor = document.body.getAttribute('data-theme-color');
    const effectiveBase = resolveEffectiveThemeBase(prefs, bodyThemeColor);
    applyTheme(isDark, effectiveBase);
  }

  function applyTheme(isDark: boolean, base: ThemeBase): void {
    const resolved = resolveTheme(isDark, base);
    if (!resolved) return;
    setTheme(resolved);
    updateBodyTag(isDark);
  }

  function resolveTheme(isDark: boolean, base: ThemeBase): Theme | null {
    if (base === 'teams') {
      return isDark ? teamsDarkTheme : teamsLightTheme;
    }

    if (base === 'web') {
      return isDark ? webDarkTheme : webLightTheme;
    }

    const bodyThemeColor = document.body.getAttribute('data-theme-color');
    const overrideColor = bodyThemeColor && isValidHexColor(bodyThemeColor) ? normalizeHexColor(bodyThemeColor) : null;
    const hasColorAttribute = bodyThemeColor !== null;
    const hasValidOverrideColor = overrideColor !== null;

    const stored = tryGetThemeSettingsFromStorage();
    const storedRampSettings = tryGetBrandSettingsFromStorage(stored);
    const isExact = storedRampSettings?.isExact ?? false;

    const customInputs = hasColorAttribute && !hasValidOverrideColor
      ? null
      : tryGetRampInputs(overrideColor, storedRampSettings);

    if (!customInputs) return isDark ? webDarkTheme : webLightTheme;

    const ramp = getOrCreateRamp({ ...customInputs, isExact, isDark });
    return isDark ? createDarkTheme(ramp) : createLightTheme(ramp);
  }

  function createBrandRamp(color: string, hueTorsion: number, vibrancy: number): BrandVariants {
    return getBrandTokensFromPalette(color, {
      hueTorsion,
      darkCp: vibrancy,
      lightCp: vibrancy,
    });
  }

  function getOrCreateRamp(inputs: BrandRampInputs): BrandVariants {
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

  function tryGetThemeSettingsFromStorage(): StoredThemeSettings | null {
    try {
      const stored = localStorage.getItem(THEME_STORAGE_KEY);
      if (!stored) return null;
      const obj = JSON.parse(stored);
      return (obj && typeof obj === 'object') ? (obj as StoredThemeSettings) : null;
    } catch {
      return null;
    }
  }

  function updateThemeSettingsInStorage(patch: Partial<StoredThemeSettings>): StoredThemeSettings {
    try {
      const current = tryGetThemeSettingsFromStorage() ?? {};
      const next: StoredThemeSettings = { ...current, ...patch };

      (Object.keys(next) as (keyof StoredThemeSettings)[]).forEach((k) => {
        if ((next as any)[k] === undefined) {
          delete (next as any)[k];
        }
      });

      localStorage.setItem(THEME_STORAGE_KEY, JSON.stringify(next));
      return next;
    } catch {
      // ignore
      return tryGetThemeSettingsFromStorage() ?? {};
    }
  }

  function resolveIsDarkFromMode(mode: ThemeMode): boolean {
    return mode === 'dark' ? true : mode === 'light' ? false : isSystemDark();
  }

  function tryGetRampInputs(
    overrideColor: string | null = null,
    storedSettings: Pick<ThemeSettings, 'color' | 'hueTorsion' | 'vibrancy' | 'isExact'> | null = null
  ): BrandRampInputs | null {
    const colorRaw = overrideColor ?? storedSettings?.color;
    if (typeof colorRaw !== 'string' || !isValidHexColor(colorRaw)) return null;

    const hueTorsion = storedSettings?.hueTorsion ?? 0;
    const vibrancy = storedSettings?.vibrancy ?? 0;

    if (typeof hueTorsion !== 'number' || !Number.isFinite(hueTorsion)) return null;
    if (typeof vibrancy !== 'number' || !Number.isFinite(vibrancy)) return null;

    const isExact = storedSettings?.isExact ?? false;
    return { color: normalizeHexColor(colorRaw), hueTorsion, vibrancy, isExact, isDark: false };
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

  function updateBodyTag(isDark: boolean): void {
    const bodyTag: HTMLElement = document?.body;

    if (bodyTag) {
      const event = new CustomEvent('themeChanged', {
        detail: { isDark }
      });
      bodyTag.dispatchEvent(event);
    }
  }

  function themeSettingsToStoredPatch(settings: ThemeSettings): Partial<StoredThemeSettings> {
    return {
      color: normalizeHexColor(settings.color),
      hue: settings.hueTorsion,
      vibrancy: settings.vibrancy,
      exact: settings.isExact ?? false,
    };
  }

  function tryGetBrandSettingsFromStorage(stored: StoredThemeSettings | null): Pick<ThemeSettings, 'color' | 'hueTorsion' | 'vibrancy' | 'isExact'> | null {
    if (!stored) return null;
    if (typeof stored.color !== 'string' || !isValidHexColor(stored.color)) return null;
    const hueTorsion = stored.hue ?? 0;
    const vibrancy = stored.vibrancy ?? 0;
    if (typeof hueTorsion !== 'number' || !Number.isFinite(hueTorsion)) return null;
    if (typeof vibrancy !== 'number' || !Number.isFinite(vibrancy)) return null;
    return {
      color: normalizeHexColor(stored.color),
      hueTorsion,
      vibrancy,
      isExact: stored.exact ?? false,
    };
  }

  function tryGetThemePreferencesFromStorage(stored: StoredThemeSettings | null): ThemePreferences | null {
    if (!stored) return null;
    const mode: ThemeMode = stored.theme ?? 'system';
    const base = stored.base;
    const dir = stored.dir;
    return { mode, base, dir };
  }

  function resolveIsDarkFromThemeSettings(settings: ThemeSettings): boolean {
    return settings.mode === 'dark'
      ? true
      : settings.mode === 'light'
        ? false
        : settings.mode === 'system' || settings.mode === undefined || settings.mode === null
          ? isSystemDark()
          : resolveEffectiveIsDark();
  }

  function resolveEffectiveThemeMode(bodyTheme: string | null = null, prefs: ThemePreferences | null = null): ThemeMode {
    const bt = bodyTheme ?? document.body.getAttribute('data-theme');
    if (bt === 'dark' || bt === 'light' || bt === 'system') return bt;

    const p = prefs ?? tryGetThemePreferencesFromStorage(tryGetThemeSettingsFromStorage());
    return p?.mode ?? 'system';
  }

  function resolveEffectiveIsDark(): boolean {
    const mode = resolveEffectiveThemeMode();
    return mode === 'dark' ? true : mode === 'light' ? false : isSystemDark();
  }

  function resolveEffectiveThemeBase(prefs: ThemePreferences | null, bodyThemeColor: string | null): ThemeBase {
    const hasColorAttribute = bodyThemeColor !== null;
    if (hasColorAttribute) return 'brand';

    if (prefs?.base === 'web') return 'web';
    if (prefs?.base === 'teams') return 'teams';
    if (prefs?.base === 'brand') return 'brand';
    return 'web';
  }
}
