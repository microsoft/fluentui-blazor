// See https://github.com/dotnet/aspnetcore/blob/main/src/Components/Web.JS/src/GlobalExports.ts
interface Blazor {
  addEventListener?: (name: string, callback: (event: any) => void) => void;

  registerCustomEventType(eventName: string, options: EventTypeOptions): void;

  // Custom properties
  theme: {
    isSystemDark(): boolean,
    isDarkMode(): boolean,
    initializeThemeSettings(): void,
    addSystemThemeChangeListener(): void,
    getCachedRamp(): BrandVariants | Record<string, string> | null,
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

    createBrandTheme(color: string, hueTorsion: number, vibrancy: number, isDark: boolean, isExact?: boolean): Theme | null,
    setBrandThemeFromTheme(theme: Theme): void,
    setBrandThemeFromColor(color: string): void,
    setBrandThemeFromColorExact(color: string): void,
    setBrandTheme(color: string, hueTorsion: number, vibrancy: number, isDark: boolean, isExact?: boolean): void,
    setBrandThemeToElement(element: HTMLElement, color: string, hueTorsion: number, vibrancy: number, isDark: boolean, isExact?: boolean): void,

    switchTheme(): boolean,
    switchDirection(): void,
  }
}
