---
title: Migration FluentNumberField
route: /Migration/NumberField
hidden: true
---

`FluentNumberField` has been **removed in V5** and replaced by the new `FluentNumberInput` component.

`FluentNumberInput` has been completely **rebuilt from the ground up** and provides improved number input handling, including support for different `CultureInfo` settings.


### FluentNumberField → FluentNumberInput

| V4 Property | V5 Property | Change |
|-------------|-------------|--------|
| `Appearance` (`FluentInputAppearance`) | `Appearance` (`TextInputAppearance`) | Enum renamed |
| `Disabled` | `Disabled`  | Type changed from `bool` to `bool?` |
| `Size` (`int?`) | `Size` (`TextInputSize?`) | Changed from pixel count to enum | 
| `Min` (`string?`) | `Min` (`TValue?`) | Changed from `string?` to generic type | 
| `Max` (`string?`) | `Max` (`TValue?`) | Changed from `string?` to generic type | 
| `Step` (`string`) | `Step` (`TValue?`) | Changed from `string` to generic type | 


### Removed properties
- `AutoComplete`
- `ChildContent` use `Value` or `@bind-Value` instead
- `DataList`
- `ParsingErrorMessage` use `MessageTemplate` in combination with `MessageCondition` instead
- `MaxLength`
- `MinLength`

### FluentNumberField migration

  ```razor
  <!-- V4 -->
  <FluentNumberField TValue="int"
                     @bind-Value="quantity"
                     Min="0" Max="100" Step="1"
                     Appearance="FluentInputAppearance.Outline" />

  <!-- V5: Use FluentNumberInput -->
  <FluentNumberInput @bind-Value="quantity"
                     Min="0"
                     Max="100"
                     Step="1"
                     Appearance="TextInputAppearance.Outline" />
  ```

 ### Appearance mapping

| V4 `FluentInputAppearance` | V5 `TextInputAppearance` |
|---------------------------|-------------------------|
| `FluentInputAppearance.Outline` | `TextInputAppearance.Outline` |
| `FluentInputAppearance.Filled` | `TextInputAppearance.FilledDarker` |

Migration helper available: `FluentInputAppearance.Filled.ToTextInputAppearance()`
