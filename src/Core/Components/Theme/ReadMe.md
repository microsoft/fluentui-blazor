# Typescript Theme API (`Theme.ts`)

This document describes only the TypeScript/JavaScript theme helpers exposed by `src\Core.Scripts\src\Utilities\Theme.ts` (namespace `Microsoft.FluentUI.Blazor.Utilities.Theme`).
For the C# API, see the Theme pages in the demo and documentation site.

## Goals

- Apply Fluent UI Web Components themes using `@fluentui/web-components` `setTheme(...)`.
- Support built-in themes (Web/Teams; light/dark/system).
- Support a generated **brand theme** from a key color and optional adjustments.
- Persist user preferences and brand ramp inputs in `localStorage`.
- Provide safe defaults and validation when values are missing/invalid.

Terminology used in this document:

- "Default web theme" means `webLightTheme` / `webDarkTheme` from `@fluentui/tokens`.

## DOM attributes

Only these DOM attributes are honored:

- `body[data-theme]`: `'light' | 'dark' | 'system'`
- `body[data-theme-color]`: optional brand key color (`#RGB` or `#RRGGBB`)

All other brand parameters (hue torsion, vibrancy, exact) are managed via the code API and persisted storage only.

Declarative `body[data-theme]` influences the effective mode (`'light' | 'dark' | 'system'`) regardless of base.

Declarative `body[data-theme-color]` is the only declarative input that influences the **brand color**; all other brand parameters can only be managed via the code API and persisted storage.

## Storage

Settings are stored in `localStorage` under the key:

- `fluentui-blazor:theme-settings`

The stored object contains two conceptual groups:

### A) Theme preferences

- `theme`: `'light' | 'dark' | 'system'` (if missing, defaults to `'system'` when interpreted)
- `base`: `'web' | 'teams' | 'brand'` (controls which built-in/base theme family is applied; default is `'web'`)
- `dir`: `'ltr' | 'rtl'`

These are read via `tryGetThemePreferencesFromStorage(...)`.

### B) Brand ramp inputs

- `color`: hex string `#RGB` or `#RRGGBB` (stored normalized to uppercase `#RRGGBB`)
- `hue`: `number` (defaults to `0`)
- `vibrancy`: `number` (defaults to `0`)
- `exact`: `boolean` (defaults to `false`)

These are read via `tryGetBrandSettingsFromStorage(...)` and written via `themeSettingsToStoredPatch(...)`.

## Theme mode resolution

`ThemeMode` is a union type:

- `'light' | 'dark' | 'system'`

Some APIs accept `mode?: ThemeMode | null` (notably `ThemeSettings`).

Mode resolution for brand theme generation is centralized in:

- `resolveIsDarkFromThemeSettings(settings)`

Rules:

- `mode === 'dark'` → dark
- `mode === 'light'` → light
- `mode === 'system'` or `mode == null` or `mode === undefined` → follow system preference (`isSystemDark()`)

## Public API

### Initialization

#### `initializeThemeSettings()`

Reads stored settings, applies the effective theme via the unified pipeline (`applyCurrentTheme(...)`), and then synchronizes the HTML `dir` attribute.

Base resolution rules:

1. If `body[data-theme-color]` is present (even if invalid), the base is treated as `'brand'` (declarative intent).
2. Otherwise, the stored `base` is used if it is `'web' | 'teams' | 'brand'`.
3. Fallback is `'web'`.

Theme application:

- `'teams'`: uses `teamsLightTheme` / `teamsDarkTheme`
- `'web'`: uses `webLightTheme` / `webDarkTheme`
- `'brand'`: attempts to create a brand theme from `body[data-theme-color]` override (if valid) and/or stored ramp settings; if neither are available/valid it falls back to the default web theme.

Also synchronizes the HTML `dir` attribute (`'ltr' | 'rtl'`) with stored settings:

- If the HTML document already has `dir='ltr' | 'rtl'` set, that value is persisted to storage.
- Otherwise, if a stored `dir` exists, it is applied to the HTML element.

This affects DOM/layout only and does not change theme tokens.

### Built-in Web theme

#### `setLightTheme()`
Stores `theme='light'` and applies the current theme via `applyCurrentTheme()`.

#### `setDarkTheme()`
Stores `theme='dark'` and applies the current theme via `applyCurrentTheme()`.

#### `setSystemTheme()`
Equivalent to `setThemeMode('system')`.

#### `setThemeMode(mode)`
Sets a persistent preference for theme mode:

- `'dark'` → stores `theme='dark'` and applies via `applyCurrentTheme()`
- `'light'` → stores `theme='light'` and applies via `applyCurrentTheme()`
- `'system'` → clears stored `theme` and applies via `applyCurrentTheme()` (which will follow `isSystemDark()`)

#### `setWebTheme()`
Stores `base='web'` and applies via `applyCurrentTheme()`.

Note: if `body[data-theme-color]` is present, the effective base is treated as `'brand'` (declarative intent). In that case, `setWebTheme()` updates the stored preference but will not change the effective base until the attribute is removed.

### Built-in Teams theme

#### `setTeamsLightTheme()`
Persists `base='teams'`, applies Teams light theme.

#### `setTeamsDarkTheme()`
Persists `base='teams'`, applies Teams dark theme.

#### `setTeamsSystemTheme()`
Equivalent to `setTeamsThemeMode('system')`.

#### `setTeamsThemeMode(mode)`
Applies Teams theme based on mode (`'light'|'dark'|'system'`). For `'system'` it uses `isSystemDark()`.

Note: the Teams setters are *imperative*; they do not consult declarative attributes like `body[data-theme]` or `body[data-theme-color]`.
More specifically, they do not use declarative attributes for base/brand selection. `body[data-theme]` can still influence the effective mode for APIs that go through `applyCurrentTheme()` (for example `setTeamsTheme()`).

#### `setTeamsTheme()`
Persists `base='teams'` and applies via `applyCurrentTheme()`.

Note: if `body[data-theme-color]` is present, the effective base is treated as `'brand'` (declarative intent). In that case, `setTeamsTheme()` updates the stored preference but will not change the effective base until the attribute is removed.

### Brand theme generation

#### `createBrandTheme(settings: ThemeSettings): Theme | null`
Generates a Fluent `Theme` by:

- Validating inputs:
  - `color` must be a valid hex color
  - `hueTorsion` and `vibrancy` must be finite numbers in `[-0.5, 0.5]`
- Resolving light/dark from `settings.mode` via `resolveIsDarkFromThemeSettings`
- Building a cached brand ramp and then creating light/dark theme tokens

Returns `null` for invalid inputs.

#### `setBrandThemeFromTheme(theme: Theme)`
Applies an already-built Fluent `Theme` via `setTheme(theme)`.

#### `setBrandThemeFromSettings(settings: ThemeSettings)`
High-level helper that:

1. Calls `createBrandTheme(settings)`
2. Persists brand ramp inputs (`color/hue/vibrancy/exact`) to `localStorage`
3. Persists `base='brand'`
4. Applies the theme via `setTheme(theme)`
5. Triggers a `themeChanged` event (via `updateBodyTag(...)`)

#### Legacy convenience wrappers

These are preserved for compatibility but delegate to `setBrandThemeFromSettings(...)`:

- `setBrandTheme(color, hueTorsion, vibrancy, isDark, isExact?)`
- `setBrandThemeFromColor(color)`
- `setBrandThemeFromColorExact(color)`

#### `setBrandThemeToElement(element, settings: ThemeSettings)`
Applies a brand theme **only to a given element** via `setTheme(theme, element)`.

This does **not** persist anything to storage.

#### `getRampFromSettings(settings: ThemeSettings): BrandVariants | null`
Generates a new brand ramp (palette) from the provided settings.

- Validates inputs using the same rules as `createBrandTheme(...)`.
- Always recalculates the ramp and does **not** use the internal cache.

Returns `null` for invalid inputs.

### State and utilities

#### `isSystemDark()`
Returns whether the OS/browser preference is dark.

#### `isDarkMode()`
Returns whether the current effective theme is dark.

Resolution order:

1. `body[data-theme]` if it is `'dark' | 'light'`
2. else stored preference `theme` if it is `'dark' | 'light'`
3. else system preference (`isSystemDark()`)

#### `switchTheme()`
Toggles between `'light'` and `'dark'` preference. If no explicit preference is stored, it uses the current effective mode.

#### `addSystemThemeChangeListener()`
Adds a single `matchMedia('(prefers-color-scheme: dark)')` listener and re-applies the theme via `applyCurrentTheme()` on system changes when the user has not explicitly set `'light'` or `'dark'` (either in storage or via `body[data-theme]`).

#### `clearThemeSettings()`
Removes the stored settings object from `localStorage`.

#### `getColorRamp(): BrandVariants | null`
Returns the current, cached, custom brand ramp, or `null` if no custom ramp has been generated yet.

#### `getColorRampFromSettings(settings: ThemeSettings): BrandVariants | null`
Generates a new brand ramp (palette) from the provided settings.

- Validates inputs using the same rules as `createBrandTheme(...)`.
- Always recalculates the ramp and does **not** use the internal cache.

Returns `null` for invalid inputs.

#### `switchDirection()`
Toggles HTML `dir` between `'ltr'` and `'rtl'` and persists it.

#### `addMediaQueriesListener()`
Updates `body[data-media]` based on a fixed set of viewport breakpoints and fires a `mediaChanged` event.

## Internal pipeline (implementation detail)

The module uses a small internal pipeline to compute and apply themes consistently across Web/Teams/Brand.
These helpers are not part of the public API surface, but explain the behavior:

### `applyCurrentTheme()`

Resolves the effective mode (`'light' | 'dark' | 'system'`) and base (`'web' | 'teams' | 'brand'`) from:

- `body[data-theme]` (if present) or stored `theme` preference, then system
- `body[data-theme-color]` (forces base=`'brand'` if present) or stored `base`, then default `'web'`

Then calls `applyTheme(isDark, base)`.

### `applyTheme(isDark, base)`

Calls `resolveTheme(isDark, base)` to compute the actual Fluent `Theme`, applies it via `setTheme(...)`, and raises the `themeChanged` event via `updateBodyTag(...)`.

### `resolveTheme(isDark, base)`

Returns the effective `Theme` tokens to apply:

- `base === 'teams'` → `teamsLightTheme` / `teamsDarkTheme`
- `base === 'web'` → `webLightTheme` / `webDarkTheme`
- `base === 'brand'` → attempts to build a brand theme using:
  - `body[data-theme-color]` as a color override if it is present and valid
  - otherwise stored brand settings via `tryGetBrandSettingsFromStorage(...)`
  - brand input resolution is performed by `tryGetRampInputs(overrideColor, storedSettings)`

  If `body[data-theme-color]` is present but invalid, no brand theme is generated and the logic falls back to the default web theme.

  If there are no valid brand inputs (no override color and no stored brand color), the logic falls back to the default web theme.

## Events

### `themeChanged`
Dispatched on `document.body` when `updateBodyTag(...)` is called.

- `detail`: `{ isDark: boolean }`

### `mediaChanged`
Dispatched on `document.body` from `addMediaQueriesListener()`.

- `detail`: `{ media: string }`

## Notes

- Color values are normalized to uppercase `#RRGGBB`.
- Brand ramp caching avoids recalculating ramps for identical inputs.
- Stored theme preferences and stored brand ramp inputs live in the same storage object but are handled by separate conversion helpers.
