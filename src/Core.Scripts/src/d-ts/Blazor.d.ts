// See https://github.com/dotnet/aspnetcore/blob/main/src/Components/Web.JS/src/GlobalExports.ts
interface Blazor {
  addEventListener?: (name: string, callback: (event: any) => void) => void;

  registerCustomEventType(eventName: string, options: EventTypeOptions): void;

  // Custom properties
  theme: {
    isSystemDark(): boolean,
    isDarkMode(): boolean,
    setLightTheme(): void,
    setDarkTheme(): void,
  }
 
}
