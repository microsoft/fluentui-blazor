---
title: Migration FluentCombobox
route: /Migration/Combobox
hidden: true
---

### Base class change 💥

`FluentCombobox` now inherits directly from `FluentSelect` instead of `ListComponentBase`.
This means all [FluentSelect migration changes](/Migration/Select) also apply to `FluentCombobox`.

All list components (`FluentSelect`, `FluentCombobox`, `FluentListbox`) now require **two** type parameters:
`TOption` (the option type) and `TValue` (the value type).

```xml
<!-- V4 -->
<FluentCombobox TOption="Country" Items="@countries"
                OptionValue="@(c => c.Code)" OptionText="@(c => c.Name)"
                @bind-SelectedOption="selectedCountry" />

<!-- V5 -->
<FluentCombobox TOption="Country" TValue="string" Items="@countries"
                OptionValue="@(c => c.Code)" OptionText="@(c => c.Name)"
                @bind-Value="selectedCountryCode" />
```

### Appearance 💥

The `Appearance` property type has changed from `Appearance?` to `ListAppearance?`.

`ListAppearance` enum has the following values:
- `FilledLighter`
- `FilledDarker`
- `Outline`
- `Transparent`

### Changed properties 💥

| V4 Property | V5 Property | Change |
|-------------|-------------|--------|
| `Value` (`string?`) | `Value` (`TValue?`) | Now generic |
| `ValueExpression` (`Expression<Func<string>>?`) | `ValueExpression` (`Expression<Func<TValue>>?`) | Now generic |
| `Disabled` (`bool`) | `Disabled` (`bool?`) | Now nullable — use `Disabled="true"` instead of just `Disabled` |
| `OptionText` (`Func<TOption, string?>`) | `OptionText` (`Func<TOption?, string>?`) | Nullable TOption, non-nullable return |
| `OptionValue` (`Func<TOption, string?>?`) | `OptionValue` (`Func<TOption?, TValue?>?`) | Returns `TValue?` instead of `string?` |
| `OptionDisabled` (`Func<TOption, bool>?`) | `OptionDisabled` (`Func<TOption?, bool>?`) | Nullable TOption |
| `SelectedOptions` (`IEnumerable<TOption>?`) | `SelectedItems` (`IEnumerable<TOption>`) | **Renamed**, now non-nullable (defaults to `[]`) |
| `SelectedOptionsChanged` | `SelectedItemsChanged` | **Renamed** |

### Removed properties 💥

- `Autocomplete` (`ComboboxAutocomplete?`) — browser autocomplete is no longer exposed.
- `ChangeOnEnterOnly`
- `Embedded`
- `Field`
- `Immediate`
- `ImmediateDelay`
- `Open` (`bool?`) — open/close state is now managed internally.
- `EnableClickToClose` (`bool?`)
- `OptionComparer` — use `OptionSelectedComparer` instead.
- `OptionSelected` — use `OptionSelectedComparer` instead.
- `OptionTitle`
- `Position` (`SelectPosition?`) — popup positioning is now handled internally.
- `SelectedOption` — use `Value` instead.
- `SelectedOptionExpression`
- `SelectedOptionsExpression`
- `Title`
- `SelectedOptionChanged` — use `ValueChanged` instead.
- `SelectedOptionsChanged` — use `SelectedItemsChanged` instead.
