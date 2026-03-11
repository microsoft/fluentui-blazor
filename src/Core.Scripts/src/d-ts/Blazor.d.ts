// See https://github.com/dotnet/aspnetcore/blob/main/src/Components/Web.JS/src/GlobalExports.ts
type ThemeMode = 'light' | 'dark' | 'system';

interface ThemeSettings {
  color: string;
  hueTorsion: number;
  vibrancy: number;
  mode?: ThemeMode | null;
  isExact?: boolean;
}

interface Blazor {
  addEventListener?: (name: string, callback: (event: any) => void) => void;

  registerCustomEventType(eventName: string, options: EventTypeOptions): void;

  theme: {
    isSystemDark(): boolean,
    isDarkMode(): boolean,
    initializeThemeSettings(): void,
    addSystemThemeChangeListener(): void,
    getColorRamp(): BrandVariants | Record<string, string> | null,
    getColorRampFromSettings(settings: ThemeSettings): BrandVariants | Record<string, string> | null,
    clearThemeSettings(): void,

    setLightTheme(): void,
    setDarkTheme(): void,
    setSystemTheme(): void,
    setThemeMode(mode: 'light' | 'dark' | 'system'): void,
    setWebTheme(): void,

    setTeamsLightTheme(): void,
    setTeamsDarkTheme(): void,
    setTeamsSystemTheme(): void,
    setTeamsThemeMode(mode: 'light' | 'dark' | 'system'): void,
    setTeamsTheme(): void,

    createBrandTheme(settings: ThemeSettings): Theme | null,
    setBrandThemeFromTheme(theme: Theme): void,
    setBrandThemeFromSettings(settings: ThemeSettings): void,
    setBrandThemeFromColor(color: string): void,
    setBrandThemeFromColorExact(color: string): void,
    setBrandTheme(color: string, hueTorsion: number, vibrancy: number, isDark: boolean, isExact?: boolean): void,
    setBrandThemeToElementFromSettings(element: HTMLElement, settings: ThemeSettings): void,
    setBrandThemeToElementFromTheme(element: HTMLElement, theme: Theme): void,

    switchTheme(): boolean,
    switchDirection(): void,
  }
}
